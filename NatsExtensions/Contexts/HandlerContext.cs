using System.Collections.Generic;
using NatsExtensions.Models;

namespace NatsExtensions.Contexts
{
    /// <summary>
    ///     Context that contains information about handler's request
    /// </summary>
    /// <typeparam name="TRequest"><see cref="Request"/></typeparam>
    public class HandlerContext<TRequest> where TRequest : Request
    {
        /// <summary>
        ///     Flag, that errors are
        /// </summary>
        public bool HasErrors => Errors?.Count > 0; 
        
        /// <summary>
        ///     Error's list
        /// </summary>
        public List<string> Errors { get; set; }
        
        /// <summary>
        ///     Request content
        /// </summary>
        public TRequest Content { get; set; }
    }
}