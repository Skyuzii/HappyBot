using HappyBot.Application.Common.Models;

namespace HappyBot.Application.Common.Interfaces.Storages
{
    public interface ITlgBotStorage
    {
        /// <summary>
        /// Обновить вебхук телеграм бота
        /// <param name="telegramBot">Телеграм бот</param>
        /// </summary>
        void UpdateWebhook(TelegramBotDto telegramBot);
        
        /// <summary>
        /// Обновить вебхуки телеграм ботов
        /// </summary>
        void UpdateWebhooks();

        /// <summary>
        /// Получить телеграм бота
        /// </summary>
        /// <param name="telegramBotId">Идентификатор телеграм бота</param>
        /// <returns>Телеграм бот</returns>
        TelegramBotDto? Get(int telegramBotId);
        
        /// <summary>
        /// Получить телеграм бота
        /// </summary>
        /// <param name="token">Токен</param>
        /// <returns>Телеграм бот</returns>
        TelegramBotDto? Get(string token);
    }
}