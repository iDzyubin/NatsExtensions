using NatsExtensions.Models;
using NatsExtensions.Services;

namespace NatsExtensions.Proxies
{
    /// <summary>
    ///     Base logic for sync request-reply interaction
    /// </summary>
    /// <typeparam name="TRequest"><see cref="IRequest"/></typeparam>
    /// <typeparam name="TReply"><see cref="IReply"/></typeparam>
    public class BaseSyncProxy<TRequest, TReply> : ISyncProxy<TRequest, TReply>
        where TRequest : IRequest
        where TReply : IReply
    {
        /// <inheritdoc cref="INatsService"/>
        private readonly INatsService _natsService;

        /// <inheritdoc cref="BaseAsyncProxy{TRequest,TReply}"/>
        public BaseSyncProxy(INatsService natsService) =>
            _natsService = natsService;
        
        /// <inheritdoc/>
        public TReply Execute(TRequest request, string subject) =>
            _natsService.RequestReply<TRequest, TReply>(request, subject);
    }
}