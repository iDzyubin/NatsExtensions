using System.Threading.Tasks;
using NatsExtensions.Handlers;
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
        /// <param name="request">IRequest body</param>
        /// <param name="subject">Subject, that handles request</param>
        /// <typeparam name="TRequest">IRequest type</typeparam>
        /// <typeparam name="TReply">IReply type</typeparam>
        /// <returns>IReply body</returns>
        TReply RequestReply<TRequest, TReply>(TRequest request, string subject)
            where TRequest : IRequest
            where TReply   : IReply;
        
        /// <summary>
        ///     Send async request to the remote handler
        /// </summary>
        /// <param name="request">IRequest body</param>
        /// <param name="subject">Subject, that handles request</param>
        /// <typeparam name="TRequest">IRequest type</typeparam>
        /// <typeparam name="TReply">IReply type</typeparam>
        /// <returns>IReply body</returns>
        Task RequestReplyAsync<TRequest, TReply>(TRequest request, string subject)
            where TRequest : IRequest, IReply
            where TReply   : IRequest, IReply;
        
        /// <summary>
        ///     Send async request to the remote handler
        /// </summary>
        /// <param name="request">IRequest body</param>
        /// <param name="subject">Subject, that handles request</param>
        /// <param name="handler"><see cref="IHandler{TRequest,TReply}"/></param>
        /// <typeparam name="TRequest">IRequest type</typeparam>
        /// <typeparam name="TReply">IReply type</typeparam>
        /// <returns>IReply body</returns>
        Task RequestReplyAsync<TRequest, TReply>(TRequest request, string subject, IHandler<TRequest, TReply> handler)
            where TRequest : IRequest, IReply
            where TReply   : IRequest, IReply;
    }
}