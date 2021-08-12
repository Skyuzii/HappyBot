using System.Threading.Tasks;
using HappyBot.Application.Common.Constants;
using HappyBot.Application.Common.Interfaces.ReplyKeyboardButton;
using HappyBot.Application.Common.Interfaces.Services;
using HappyBot.Application.Common.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HappyBot.Application.MainBot.ReplyKeyboardButtons
{
    public class StartReplyKB : IReplyKBMainBot
    {
        public string Name => ReplyKBMainBotConstants.START;
        
        private readonly ITlgMenuServiceMainBot _tlgMenuService;
        public StartReplyKB(ITlgMenuServiceMainBot tlgMenuService)
        {
            _tlgMenuService = tlgMenuService;
        }
        
        public async Task Execute(ITelegramBotClient botClient, ChatInfo chatInfo, Update update)
        {
            var msg = "Добро пожаловать\n" +
                      "Вас привествует Happy Bot\n" +
                      "Хотите сделать необычный подарок для любимого человека?\n" +
                      "Я вам с радостью помогу\n";

            await botClient.SendTextMessageAsync(chatInfo.TelegramId, msg, replyMarkup: _tlgMenuService.GetMainMenu());
            chatInfo.LastButton = null;
        }
    }
}