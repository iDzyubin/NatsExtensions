using System;
using System.Text.Json;

namespace NatsExtensions.Extensions
{
    /// <summary>
    ///     Данный конвертер предназначен для преобразования данных
    ///     при передаче на транспортном уровне
    /// </summary>
    public static class DataConverterExtensions
    {
        /// <summary>
        ///     Преобразовываем объект в массив байт
        /// </summary>
        /// <param name="reply">Данные ответа на запрос</param>
        /// <typeparam name="T">Тип ответа на запрос</typeparam>
        /// <returns>Массив байт</returns>
        public static byte[] ConvertToByteArray<T>(this T reply) where T : class
            => reply != null
                ? JsonSerializer.SerializeToUtf8Bytes(reply)
                : default;

        /// <summary>
        ///     Преобразовываем массив байт в объект
        /// </summary>
        /// <param name="data">Полученные данные</param>
        /// <typeparam name="T">Тип, в который преобразуем полученные данные</typeparam>
        /// <returns>Объект, заполненный данными</returns>
        public static T ConvertFromByteArray<T>(this byte[] data) where T : class
            => data != null
                ? JsonSerializer.Deserialize<T>(new ReadOnlySpan<byte>(data))
                : default;
    }
}