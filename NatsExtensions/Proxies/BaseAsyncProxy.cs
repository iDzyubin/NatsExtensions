using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NatsExtensions.Handlers;
using NatsExtensions.Models;
using NatsExtensions.Services;

namespace NatsExtensions.Proxies
{
    /// <summary>
    ///     Base logic for async request-reply interaction
    /// </summary>
    /// <typeparam name="TRequest"><see cref="IRequest"/></typeparam>
    /// <typeparam name="TReply"><see cref="IReply"/></typeparam>
    public class BaseAsyncProxy<TRequest, TReply> : IAsyncProxy<TRequest, TReply>
        where TRequest : IRequest
        where TReply : IReply
    {
        /// <inheritdoc cref="IServiceProvider"/>
        private readonly IServiceProvider _serviceProvider;
        
        /// <inheritdoc cref="INatsService"/>
        private readonly INatsService _natsService;

        /// <inheritdoc cref="BaseAsyncProxy{TRequest,TReply}"/>
        public BaseAsyncProxy(INatsService natsService, IServiceProvider serviceProvider)
        {
            _natsService = natsService;
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc/>
        public async Task ExecuteAsync(TRequest request, string subject, CancellationToken token = default)
        {
            using var scope = _serviceProvider.CreateScope();
            var handler = scope.ServiceProvider.GetService<IHandler<TRequest, TReply>>();
            if (handler == null)
                throw new InvalidOperationException("Handler with the same arguments not found");
            
            await ExecuteAsync(request, subject, handler);
        }

        /// <inheritdoc/>
        public async Task ExecuteAsync(TRequest request, string subject, IHandler<TRequest, TReply> handler, CancellationToken token = default)
        {
            await _natsService.RequestReplyAsync(request, subject, handler);
        }
    }
}