using System;
using Microsoft.Extensions.DependencyInjection;
using NATS.Client;
using NatsExtensions.Handlers;
using NatsExtensions.HostedServices;
using NatsExtensions.Models;
using NatsExtensions.Options;
using NatsExtensions.Proxies;
using NatsExtensions.Services;

namespace NatsExtensions.Extensions
{
    /// <summary>
    ///     DI extensions
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNatsExtensions(this IServiceCollection services, Action<NatsOptions> action)
        {
            var natsOptions = new NatsOptions();
            action.Invoke(natsOptions);
            
            return services.AddTransient(_ =>
            {
                var factory = new ConnectionFactory();
                var options = ConnectionFactory.GetDefaultOptions();
                options.Url = natsOptions.ConnectionString;
                return factory.CreateConnection();
            })
           .AddTransient<INatsService, NatsService>()
           .Configure<NatsOptions>(action.Invoke);
        }

        /// <summary>
        ///     Register NATS handler for handle request from external
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="subject">Target subject</param>
        /// <typeparam name="TRequest"><see cref="Request"/></typeparam>
        /// <typeparam name="TReply"><see cref="Reply"/></typeparam>
        /// <typeparam name="THandler"><see cref="IHandler{TRequest,TReply}"/></typeparam>
        /// <returns>The same service collection so that multiple calls can be chained.</returns>
        public static IServiceCollection AddHandler<TRequest, TReply, THandler>(this IServiceCollection services, string subject)
            where TRequest : Request
            where TReply : Reply
            where THandler : class, IHandler<TRequest, TReply>
        {
            return services
                .AddTransient<IHandler<TRequest, TReply>, THandler>()
                .AddHostedService<RegisterHandlerService<TRequest, TReply>>(subject);
        }

        public static IServiceCollection AddHandlers(this IServiceCollection services, string subject, Action<RequestHandlerRegisterBuilder> action)
        {
            if (string.IsNullOrWhiteSpace(subject))
                throw new ArgumentException("Subject cannot be empty");
            
            if (action == null)
                throw new ArgumentException("Action cannot be null");

            var builder = new RequestHandlerRegisterBuilder();
            action.Invoke(builder);
            
            
        }


        /// <summary>
        ///     Register NATS proxy for isolate logic of sync sending request to external
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <typeparam name="TRequest"><see cref="Request"/></typeparam>
        /// <typeparam name="TReply"><see cref="Reply"/></typeparam>
        /// <typeparam name="TProxy"><see cref="IProxy{TRequest,TReply}"/></typeparam>
        /// <returns>The same service collection so that multiple calls can be chained.</returns>
        public static IServiceCollection AddNatsProxy<TRequest, TReply, TProxy>(this IServiceCollection services)
            where TRequest : Request
            where TReply : Reply
            where TProxy : class, IProxy<TRequest, TReply>
        {
            return services.AddTransient<IProxy<TRequest, TReply>, TProxy>();
        }

        /// <summary>
        ///     Register NATS proxy for isolate logic of async sending request to external
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <typeparam name="TRequest"><see cref="Request"/></typeparam>
        /// <typeparam name="TReply"><see cref="Reply"/></typeparam>
        /// <typeparam name="TProxy"><see cref="IProxy{TRequest,TReply}"/></typeparam>
        /// <returns>The same service collection so that multiple calls can be chained.</returns>
        public static IServiceCollection AddNatsAsyncProxy<TRequest, TReply, TProxy>(this IServiceCollection services)
            where TRequest : Request
            where TReply : Reply
            where TProxy : class, IProxy<TRequest, TReply>
        {
            return services.AddTransient<IProxy<TRequest, TReply>, TProxy>();
        }
    }
}