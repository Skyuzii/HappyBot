using Telegram.Bot.Types.ReplyMarkups;

namespace HappyBot.Application.Common.Interfaces.Services
{
    public interface ITlgMenuServiceMainBot
    {
        /// <summary>
        /// Получить главное меню
        /// </summary>
        /// <returns>Главное меню</returns>
        ReplyKeyboardMarkup GetMainMenu();

        /// <summary>
        /// Получить кнопку - Отмена
        /// </summary>
        /// <returns>Кнопка - Отмена</returns>
        ReplyKeyboardMarkup GetCancelButton();
    }
}