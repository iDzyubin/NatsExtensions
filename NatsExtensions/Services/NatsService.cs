using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NATS.Client;
using NatsExtensions.Attributes;
using NatsExtensions.Extensions;
using NatsExtensions.Models;
using NatsExtensions.Options;

namespace NatsExtensions.Services
{
    /// <inheritdoc cref="INatsService"/>
    public class NatsService : INatsService
    {
        private readonly IConnection _connection;
        private readonly NatsOptions _natsOptions;

        public NatsService(IConnection connection, IOptions<NatsOptions> options)
        {
            _connection = connection;
            _natsOptions = options.Value;
        }
        
        /// <inheritdoc/>
        public TReply RequestReply<TRequest, TReply>(TRequest request, string subject, CancellationToken cancellationToken) where TRequest : Request where TReply : Reply
        {
            var requestSubject = typeof(TRequest).GetAttribute<ServiceBusAttribute>();
            if (requestSubject == null)
                throw new InvalidOperationException("Request model does not contain [ServiceBus] attribute");
            
            var result = _connection.Request($"{subject}.{requestSubject.Code}", request.ConvertToByteArray(), _natsOptions.Timeout);
            return result.Data.ConvertFromByteArray<TReply>();
        }

        /// <inheritdoc/>
        public async Task<TReply> RequestReplyAsync<TRequest, TReply>(TRequest request, string subject, CancellationToken cancellationToken) where TRequest : Request where TReply : Reply
        {
            var requestSubject = typeof(TRequest).GetAttribute<ServiceBusAttribute>();
            if (requestSubject == null)
                throw new InvalidOperationException("Request model does not contain [ServiceBus] attribute");

            var result = await _connection.RequestAsync($"{subject}.{requestSubject.Code}", request.ConvertToByteArray(), _natsOptions.Timeout);
            return result.Data.ConvertFromByteArray<TReply>();
        }

        /// <inheritdoc/>
        public Task PublistAsync<TRequest>(TRequest request, string subject, CancellationToken cancellationToken) where TRequest : Request
        {
            var requestSubject = typeof(TRequest).GetAttribute<ServiceBusAttribute>();
            if (requestSubject == null)
                throw new InvalidOperationException("Request model does not contain [ServiceBus] attribute");

            _connection.Publish($"{subject}.{requestSubject.Code}", request.ConvertToByteArray());
            return Task.CompletedTask;
        }
    }
}