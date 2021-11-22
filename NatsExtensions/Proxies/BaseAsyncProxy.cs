using System.Threading;
using System.Threading.Tasks;
using NatsExtensions.Models;
using NatsExtensions.Services;

namespace NatsExtensions.Proxies
{
    /// <summary>
    ///     Base logic for async request-reply interaction
    /// </summary>
    /// <typeparam name="TRequest"><see cref="Request"/></typeparam>
    public class BaseAsyncProxy<TRequest> : IAsyncProxy<TRequest> where TRequest : Request
    {
        /// <inheritdoc cref="INatsService"/>
        private readonly INatsService _natsService;

        /// <inheritdoc cref="BaseAsyncProxy{TRequest}"/>
        public BaseAsyncProxy(INatsService natsService) =>
            _natsService = natsService;

        /// <inheritdoc/>
        public async Task ExecuteAsync(TRequest request, string subject, CancellationToken token = default) =>
            await _natsService.RequestReplyAsync(request, subject);
    }
}