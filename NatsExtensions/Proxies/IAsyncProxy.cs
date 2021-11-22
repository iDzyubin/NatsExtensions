using System.Threading;
using System.Threading.Tasks;
using NatsExtensions.Models;

namespace NatsExtensions.Proxies
{
    /// <summary>
    ///     Proxy for isolate async request-reply logic in self
    /// </summary>
    /// <typeparam name="TRequest"><see cref="Request"/></typeparam>
    public interface IAsyncProxy<TRequest> where TRequest : Request
    {
        /// <summary>
        ///     Execute async request
        /// </summary>
        /// <param name="request"><see cref="Request"/></param>
        /// <param name="subject">Subject, that handles request</param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="Task"/></returns>
        Task ExecuteAsync(TRequest request, string subject, CancellationToken token = default);
    }
}