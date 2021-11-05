using System.Threading.Tasks;
using NatsExtensions.Models;

namespace NatsExtensions.Handlers
{
    /// <summary>
    ///     Интерфейс обработчика запросов из шины данных
    /// </summary>
    /// <typeparam name="TRequest">Тип запроса</typeparam>
    /// <typeparam name="TResponse">Тип ответа на запрос</typeparam>
    public interface IHandler<TRequest, TResponse>
        where TRequest : Request
        where TResponse : Reply
    {
        /// <summary>
        ///     Обработать полученный запрос
        /// </summary>
        /// <param name="request">Запрос на обработку данных</param>
        /// <returns>Ответ на запрос</returns>
        Task<TResponse> Handle(TRequest request);
    }
}