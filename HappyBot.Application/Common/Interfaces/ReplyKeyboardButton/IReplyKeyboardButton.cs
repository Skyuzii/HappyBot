using System.Threading.Tasks;
using HappyBot.Application.Common.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HappyBot.Application.Common.Interfaces.ReplyKeyboardButton
{
    /// <summary>
    /// Базовый интерфейс для обработчиков reply кнопок
    /// </summary>
    public interface IReplyKeyboardButton
    {
        /// <summary>
        /// Название кнопки
        /// </summary>
        public string Name { get; }         
        
        /// <summary>
        /// Выполнить обработчик
        /// </summary>
        /// <param name="botClient">Телеграм бот клиент</param>
        /// <param name="chatInfo">Информация о текущем чате</param>
        /// <param name="update">Обновление из телеграм</param>
        Task Execute(ITelegramBotClient botClient, ChatInfo chatInfo, Update update);
    }
}