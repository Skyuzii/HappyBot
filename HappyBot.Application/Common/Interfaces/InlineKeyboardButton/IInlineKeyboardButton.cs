using System.Threading.Tasks;
using HappyBot.Application.Common.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HappyBot.Application.Common.Interfaces.InlineKeyboardButton
{
    /// <summary>
    /// Базовый интерфейс для обработчиков inline кнопок
    /// </summary>
    public interface IInlineKeyboardButton
    {
        /// <summary>
        /// Название кнопки
        /// </summary>
        public string HeaderName { get; set; }

        /// <summary>
        /// Название колбэка
        /// </summary>
        public string CallbackName { get; set; }
        
        /// <summary>
        /// Выполнить обработчик
        /// </summary>
        /// <param name="botClient">Телеграм бот клиент</param>
        /// <param name="chatInfo">Информация о текущем чате</param>
        /// <param name="update">Обновление из телеграм</param>
        Task Execute(ITelegramBotClient botClient, ChatInfo chatInfo, Update update);
    }
}