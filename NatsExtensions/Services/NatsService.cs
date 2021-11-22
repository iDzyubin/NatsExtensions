using System;
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
        public TReply RequestReply<TRequest, TReply>(TRequest request, string subject) where TRequest : Request where TReply : Reply
        {
            var replySubject = typeof(TReply).GetAttribute<ServiceBusAttribute>();
            if (replySubject == null)
            {
                throw new InvalidOperationException("Reply model does not contain [ServiceBus] attribute");
            }

            var subscription = _connection.SubscribeSync($"{subject}.{replySubject.Code}");
            if (!subscription.IsValid)
            {
                throw new InvalidOperationException("Cannot connect to NATS");
            }

            var requestSubject = typeof(TRequest).GetAttribute<ServiceBusAttribute>();
            if (requestSubject == null)
            {
                throw new InvalidOperationException("Request model does not contain [ServiceBus] attribute");
            }
            
            _connection.Publish($"{subject}.{requestSubject.Code}", request.ConvertToByteArray());

            var message = subscription.NextMessage(_natsOptions.Timeout).Data;
            return message.ConvertFromByteArray<TReply>();
        }

        /// <inheritdoc/>
        public Task RequestReplyAsync<TRequest>(TRequest request, string subject) where TRequest : Request
        {
            var requestSubject = typeof(TRequest).GetAttribute<ServiceBusAttribute>();
            if (requestSubject == null)
            {
                throw new InvalidOperationException("Request model does not contain [ServiceBus] attribute");
            }
            
            _connection.Publish($"{subject}.{requestSubject.Code}", request.ConvertToByteArray());
            return Task.CompletedTask;
        }
    }
}