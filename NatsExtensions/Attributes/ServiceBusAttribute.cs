using System;

namespace NatsExtensions.Attributes
{
    /// <summary>
    ///     Аттрибут для запросов,
    ///     который позволяет понять откуда пришли данные
    ///     и куда их затем отправлять 
    /// </summary>
    public class ServiceBusAttribute : Attribute
    {
        /// <summary>
        ///     Код, по которому можно получить доступ к обработчику
        /// </summary>
        public int Code { get; set; }
    }
}