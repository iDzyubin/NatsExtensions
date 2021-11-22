using System.Threading.Tasks;
using NatsExtensions.Models;

namespace NatsExtensions.Services
{
    /// <summary>
    ///     Interface for NATS business logic
    /// </summary>
    public interface INatsService
    {
        /// <summary>
        ///     Send sync request to the remote handler
        /// </summary>
        /// <param name="request"><see cref="Request"/></param>
        /// <param name="subject">Subject, that handles request</param>
        /// <typeparam name="TRequest"><see cref="Request"/></typeparam>
        /// <typeparam name="TReply"><see cref="Reply"/></typeparam>
        /// <returns><see cref="Reply"/></returns>
        TReply RequestReply<TRequest, TReply>(TRequest request, string subject) where TRequest : Request where TReply : Reply;
        
        /// <summary>
        ///     Send async request to the remote handler
        /// </summary>
        /// <param name="request"><see cref="Request"/></param>
        /// <param name="subject">Subject, that handles request</param>
        /// <typeparam name="TRequest"><see cref="Request"/></typeparam>
        /// <returns><see cref="Task"/></returns>
        /// <remarks>
        ///     Should exists handler that intercepts response from external system
        /// </remarks>
        Task RequestReplyAsync<TRequest>(TRequest request, string subject) where TRequest : Request;
    }
}