using System;
using NatsExtensions.Attributes;

namespace NatsExtensions.Extensions
{
    /// <summary>
    ///     Extensions for request-reply
    /// </summary>
    internal static class RequestReplyExtensions
    {
        public static ServiceBusAttribute TryGetServiceBusAttributeValue(this Type type)
            => Attribute.GetCustomAttribute(type, typeof(ServiceBusAttribute)) is ServiceBusAttribute subject
                ? subject
                : throw new InvalidOperationException($"Type '{type}' needs 'service bus' attribute for data sending");
    }
}