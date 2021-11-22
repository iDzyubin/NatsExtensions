﻿using System;
using NatsExtensions.Models;

namespace NatsExtensions.Proxies
{
    /// <summary>
    ///     Proxy for isolate request-reply logic in self
    /// </summary>
    /// <typeparam name="TRequest"><see cref="IRequest"/></typeparam>
    /// <typeparam name="TReply"><see cref="IReply"/></typeparam>
    [Obsolete("It will be deleted in next versions. Use ISyncProxy or IAsyncProxy interfaces")]
    public interface IProxy<TRequest, TReply>
        where TRequest : IRequest
        where TReply : IReply
    {
        /// <summary>
        ///     Execute request
        /// </summary>
        /// <param name="request">IRequest for receiving data</param>
        /// <param name="subject">Subject, that handles request</param>
        /// <returns>IReply from other side</returns>
        TReply Execute(TRequest request, string subject);
    }
}