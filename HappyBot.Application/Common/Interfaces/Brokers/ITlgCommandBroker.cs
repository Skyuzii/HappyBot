using System.Threading.Tasks;
using HappyBot.Application.Common.Models;
using Telegram.Bot.Types;

namespace HappyBot.Application.Common.Interfaces.Brokers
{
    /// <summary>
    /// Интерфейс описывает брокера обработки команд для бота телеграма
    /// </summary>
    public interface ITlgCommandBroker
    {
        /// <summary>
        /// Обработать входящего обновления телеграм
        /// </summary>
        /// <param name="update">Данные обновления</param>
        Task ParseUpdate(TelegramBotDto telegramBotDto, Update update);
    }
}