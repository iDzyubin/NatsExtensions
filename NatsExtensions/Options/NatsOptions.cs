namespace NatsExtensions.Options
{
    /// <summary>
    ///     Настройки NATS
    /// </summary>
    public class NatsOptions
    {
        /// <summary>
        ///     Название секции конфигурации
        /// </summary>
        public const string Section = "Nats";
        
        /// <summary>
        ///     Название субъекта
        /// </summary>
        public string Subject { get; set; }
    }
}