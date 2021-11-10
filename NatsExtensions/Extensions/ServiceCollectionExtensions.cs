using Microsoft.Extensions.DependencyInjection;
using NatsExtensions.Handlers;
using NatsExtensions.HostedServices;
using NatsExtensions.Models;
using NatsExtensions.Proxies;

namespace NatsExtensions.Extensions
{
    /// <summary>
    ///     DI extensions
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        ///     Register NATS handler for handle request from external
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <typeparam name="TRequest"><see cref="Request"/></typeparam>
        /// <typeparam name="TReply"><see cref="Reply"/></typeparam>
        /// <typeparam name="THandler"><see cref="IHandler{TRequest,TReply}"/></typeparam>
        /// <returns>The same service collection so that multiple calls can be chained.</returns>
        public static IServiceCollection AddNatsHandler<TRequest, TReply, THandler>(this IServiceCollection services)
            where TRequest : Request
            where TReply : Reply
            where THandler : class, IHandler<TRequest, TReply>
        {
            services.AddTransient<IHandler<TRequest, TReply>, THandler>();
            services.AddHostedService<RegisterHandlerService<TRequest, TReply>>();
            return services;
        }

        /// <summary>
        ///     Register NATS proxy for isolate logic of sending request to external
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
            services.AddTransient<IProxy<TRequest, TReply>, TProxy>();
            return services;
        }
    }
}