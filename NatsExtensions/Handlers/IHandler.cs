using System.Threading.Tasks;
using NatsExtensions.Models;

namespace NatsExtensions.Handlers
{
    /// <summary>
    ///     Interface for nats handler
    /// </summary>
    /// <typeparam name="TRequest">Request target type</typeparam>
    /// <typeparam name="TResponse">Reply target type</typeparam>
    public interface IHandler<TRequest, TResponse>
        where TRequest : Request
        where TResponse : Reply
    {
        /// <summary>
        ///     Handle received request
        /// </summary>
        /// <param name="request">Request from remote subject</param>
        /// <returns>Handler reply</returns>
        Task<TResponse> Handle(TRequest request);
    }
}