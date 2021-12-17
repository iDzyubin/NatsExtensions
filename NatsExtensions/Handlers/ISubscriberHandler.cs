using System.Threading;
using System.Threading.Tasks;
using NatsExtensions.Contexts;
using NatsExtensions.Models;

namespace NatsExtensions.Handlers
{
    /// <summary>
    ///     Interface for subscribe (request only) handler
    /// </summary>
    /// <typeparam name="TRequest"><see cref="Request"/></typeparam>
    public interface ISubscriberHandler<TRequest> : IHandler
        where TRequest : Request 
    {
        /// <summary>
        ///     Handle received request
        /// </summary>
        /// <param name="context"><see cref="HandlerContext{TRequest}"/></param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns><see cref="Task"/></returns>
        Task Handle(HandlerContext<TRequest> context, CancellationToken cancellationToken);
    }
}