using System.Threading;
using System.Threading.Tasks;
using NatsExtensions.Contexts;
using NatsExtensions.Models;

namespace NatsExtensions.Handlers
{
    /// <summary>
    ///     Interface for request-reply handler
    /// </summary>
    /// <typeparam name="TRequest"><see cref="Request"/></typeparam>
    /// <typeparam name="TReply"><see cref="Reply"/></typeparam>
    public interface IRequestHandler<TRequest, TReply> : IHandler 
        where TRequest : Request 
        where TReply : Reply
    {
        /// <summary>
        ///     Handle received request
        /// </summary>
        /// <param name="context"><see cref="HandlerContext{TRequest}"/></param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns><see cref="Reply"/></returns>
        Task<TReply> Handle(HandlerContext<TRequest> context, CancellationToken cancellationToken);
    }
}