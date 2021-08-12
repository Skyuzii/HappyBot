using System.Collections.Generic;
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
        /// Получить телеграм бота по индектификатору
        /// </summary>
        /// <param name="telegramBotId">Идентификатор телеграм бота</param>
        /// <returns>Телеграм бот</returns>
        TelegramBotDto? GetById(int telegramBotId);
        
        /// <summary>
        /// Получить телеграм бота по токену
        /// </summary>
        /// <param name="token">Токен</param>
        /// <returns>Телеграм бот</returns>
        TelegramBotDto? GetByToken(string token);

        /// <summary>
        /// Получить телеграм ботов по идентифкатору пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns>Список телеграм ботов</returns>
        IList<TelegramBotDto> GetByUserId(int userId);
        
        /// <summary>
        /// Добавить телеграм бота
        /// </summary>
        /// <param name="telegramBotDto">телеграм бот</param>
        void Add(TelegramBotDto telegramBotDto);
    }
}