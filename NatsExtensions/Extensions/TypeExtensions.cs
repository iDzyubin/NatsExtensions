using System;

namespace NatsExtensions.Extensions
{
    /// <summary>
    ///     Extensions for request-reply
    /// </summary>
    internal static class TypeExtensions
    {
        public static TAttribute GetAttribute<TAttribute>(this Type type) where TAttribute : Attribute => 
            Attribute.GetCustomAttribute(type, typeof(TAttribute)) as TAttribute;
    }
}