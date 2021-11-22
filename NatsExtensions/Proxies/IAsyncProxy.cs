using System.Threading;
using System.Threading.Tasks;
using NatsExtensions.Handlers;
using NatsExtensions.Models;

namespace NatsExtensions.Proxies
{
    /// <summary>
    ///     Proxy for isolate async request-reply logic in self
    /// </summary>
    /// <typeparam name="TRequest"><see cref="IRequest"/></typeparam>
    /// <typeparam name="TReply"><see cref="IReply"/></typeparam>
    public interface IAsyncProxy<TRequest, TReply>
        where TRequest : IRequest
        where TReply : IReply
    {
        /// <summary>
        ///     Execute async request
        /// </summary>
        /// <param name="request"><see cref="IRequest"/></param>
        /// <param name="subject">Subject, that handles request</param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="IReply"/></returns>
        Task ExecuteAsync(TRequest request, string subject, CancellationToken token = default);
        
        /// <summary>
        ///     Execute async request
        /// </summary>
        /// <param name="request"><see cref="IRequest"/></param>
        /// <param name="subject">Subject, that handles request</param>
        /// <param name="handler"><see cref="IHandler{TRequest,TReply}"/></param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="IReply"/></returns>
        Task ExecuteAsync(TRequest request, string subject, IHandler<TRequest, TReply> handler, CancellationToken token = default);
    }
}