using HappyBot.Application.Common.Models;

namespace HappyBot.Application.Common.Interfaces.Storages
{
    /// <summary>
    /// Интерфейс описывает сервис для работы с чатами
    /// </summary>
    public interface IChatStorage
    {
        /// <summary>
        /// Получить информацию о чате
        /// </summary>
        /// <param name="chatKey">Ключ чата</param>
        /// <returns>Информацию о чате</returns>
        ChatInfo? Get(string chatKey);

        /// <summary>
        /// Добавить информацию о чате
        /// </summary>
        /// <param name="chatKey">Ключ чата</param>
        /// <param name="chatInfo"></param>
        void Add(string chatKey, ChatInfo chatInfo);
		
        /// <summary>
        /// Определить, существует ли информация о таком чате
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <returns></returns>
        bool Exists(string chatKey);

        /// <summary>
        /// Сгенерировать ключ
        /// </summary>
        /// <param name="telegramId">Идентификатор телеграма</param>
        /// <param name="telegramBotId">Идентификатор телеграм бота</param>
        /// <returns></returns>
        string GenerateKey(long telegramId, int telegramBotId);
    }

}