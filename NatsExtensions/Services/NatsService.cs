using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NATS.Client;
using NatsExtensions.Attributes;
using NatsExtensions.Extensions;
using NatsExtensions.Handlers;
using NatsExtensions.Models;
using NatsExtensions.Options;

namespace NatsExtensions.Services
{
    /// <inheritdoc cref="INatsService"/>
    public class NatsService : INatsService
    {
        private readonly NatsOptions _natsOptions;
        private readonly IServiceProvider _serviceProvider;
        private IConnection _connection;
        private const int Timeout = 5 * 1000;
        
        public NatsService(IConnection connection, IServiceProvider serviceProvider, IOptions<NatsOptions> natsOptions)
        {
            _connection = connection;
            _serviceProvider = serviceProvider;
            _natsOptions = natsOptions.Value;
        }
        
        /// <inheritdoc/>
        public TReply RequestReply<TRequest, TReply>(TRequest request, string subject) 
            where TRequest : IRequest 
            where TReply   : IReply
        {
            var (requestSubject, replySubject) = CheckServiceBusAttributes<TRequest, TReply>();

            var subscription = _connection.SubscribeSync($"{subject}.{replySubject.Code}");
            if (!subscription.IsValid)
            {
                throw new InvalidOperationException("Cannot connect to NATS");
            }

            var data = request.ConvertToByteArray();
            _connection.Publish($"{subject}.{requestSubject.Code}", data);

            var message = subscription.NextMessage(Timeout).Data;
            return message.ConvertFromByteArray<TReply>();
        }

        /// <inheritdoc/>
        public async Task RequestReplyAsync<TRequest, TReply>(TRequest request, string subject)
            where TRequest : IRequest 
            where TReply   : IReply
        {
            var (requestSubject, replySubject) = CheckServiceBusAttributes<TRequest, TReply>();

            using var scope = _serviceProvider.CreateScope();
            var handler = scope.ServiceProvider.GetService<IHandler<TRequest, TReply>>();
            if (handler == null)
                throw new InvalidOperationException("Handler with the same arguments not found");
            
            var subscription = _connection.SubscribeAsync($"{subject}.{replySubject.Code}", as (sender, args) =>
            {
                handler.Handle(args.Message.Data.ConvertFromByteArray<TReply>());
            });
            
            var data = request.ConvertToByteArray();
            _connection.Publish($"{subject}.{requestSubject.Code}", data);
        }

        /// <inheritdoc/>
        public Task<TReply> RequestReplyAsync<TRequest, TReply>(TRequest request, string subject, IHandler<TRequest, TReply> handler)
            where TRequest : IRequest 
            where TReply   : IReply
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Check ServiceBusAttribute availability
        /// </summary>
        /// <typeparam name="TRequest"><see cref="IRequest"/></typeparam>
        /// <typeparam name="TReply"><see cref="IReply"/></typeparam>
        /// <returns>IRequest-IReply service bus attribute values</returns>
        private (ServiceBusAttribute requestSubject, ServiceBusAttribute replySubject) CheckServiceBusAttributes<TRequest, TReply>()
        {
            if (Attribute.GetCustomAttribute(typeof(TRequest), typeof(ServiceBusAttribute)) is not ServiceBusAttribute requestSubject)
            {
                throw new InvalidOperationException("IRequest needs 'service bus' attribute for data sending");
            }
            
            if (Attribute.GetCustomAttribute(typeof(TReply), typeof(ServiceBusAttribute)) is not ServiceBusAttribute replySubject)
            {
                throw new InvalidOperationException("IReply needs 'service bus' attribute for data receiving");
            }

            return (requestSubject, replySubject);
        }
    }
}