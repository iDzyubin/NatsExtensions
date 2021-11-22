using NatsExtensions.Handlers;
using NatsExtensions.Models;

namespace NatsExtensions.Proxies
{
    /// <summary>
    ///     Proxy for isolate sync request-reply logic in self
    /// </summary>
    /// <typeparam name="TRequest"><see cref="IRequest"/></typeparam>
    /// <typeparam name="TReply"><see cref="IReply"/></typeparam>
    public interface ISyncProxy<TRequest, TReply>
        where TRequest : IRequest
        where TReply : IReply
    {
        /// <summary>
        ///     Execute sync request
        /// </summary>
        /// <param name="request"><see cref="IRequest"/></param>
        /// <param name="subject">Subject, that handles request</param>
        /// <returns><see cref="IReply"/></returns>
        TReply Execute(TRequest request, string subject);
    }
}