using NatsExtensions.Models;

namespace NatsExtensions.Proxies
{
    /// <summary>
    ///     Proxy for isolate sync request-reply logic in self
    /// </summary>
    /// <typeparam name="TRequest"><see cref="Request"/></typeparam>
    /// <typeparam name="TReply"><see cref="Reply"/></typeparam>
    public interface ISyncProxy<TRequest, TReply>
        where TRequest : Request
        where TReply : Reply
    {
        /// <summary>
        ///     Execute sync request
        /// </summary>
        /// <param name="request"><see cref="Request"/></param>
        /// <param name="subject">Subject, that handles request</param>
        /// <returns><see cref="Reply"/></returns>
        TReply Execute(TRequest request, string subject);
    }
}