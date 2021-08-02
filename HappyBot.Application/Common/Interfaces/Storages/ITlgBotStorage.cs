using HappyBot.Application.Common.Models;

namespace HappyBot.Application.Common.Interfaces.Storages
{
    public interface ITlgBotStorage
    {
        /// <summary>
        /// Инициализировать список телеграм ботов
        /// </summary>
        void InitTelegramBots();

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
    }
}