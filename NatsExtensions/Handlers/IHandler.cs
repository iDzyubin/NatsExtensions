using System.Threading.Tasks;
using NatsExtensions.Models;

namespace NatsExtensions.Handlers
{
    /// <summary>
    ///     Interface for nats handler
    /// </summary>
    /// <typeparam name="TRequest"><see cref="IRequest"/></typeparam>
    /// <typeparam name="TReply"><see cref="IReply"/></typeparam>
    public interface IHandler<TRequest, TReply>
        where TRequest : IRequest, IReply
        where TReply   : IRequest, IReply
    {
        /// <summary>
        ///     Handle received request
        /// </summary>
        /// <param name="request">IRequest from remote subject</param>
        /// <returns>Handler reply</returns>
        Task<TReply> Handle(TRequest request);
    }
}