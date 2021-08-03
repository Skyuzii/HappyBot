using System.Threading.Tasks;
using HappyBot.Application.Common.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HappyBot.Application.Common.Interfaces.Buttons
{
    public interface IButton
    {
        /// <summary>
        /// Название колбэка
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// Выполнить обработчик
        /// </summary>
        /// <param name="botClient">Телеграм бот клиент</param>
        /// <param name="chatInfo">Информация о текущем чате</param>
        /// <param name="message">Обновление из телеграм</param>
        Task Execute(ITelegramBotClient botClient, ChatInfo chatInfo, Message message);
    }
}