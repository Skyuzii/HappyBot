using System.Threading.Tasks;
using HappyBot.Application.Common.Constants;
using HappyBot.Application.Common.Interfaces.ReplyKeyboardButton;
using HappyBot.Application.Common.Interfaces.Services;
using HappyBot.Application.Common.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HappyBot.Application.MainBot.ReplyKeyboardButtons
{
    public class CancelReplyKB : IReplyKBMainBot
    {
        public string Name => ReplyKBMainBotConstants.CANCEL;

        private readonly ITlgMenuServiceMainBot _tlgMenuService;

        public CancelReplyKB(ITlgMenuServiceMainBot tlgMenuService)
        {
            _tlgMenuService = tlgMenuService;
        }

        public async Task Execute(ITelegramBotClient botClient, ChatInfo chatInfo, Update update)
        {
            chatInfo.Argument = null;
            chatInfo.LastButton = null;
            chatInfo.LastInlineKeyboardMessageId = null;
            await botClient.SendTextMessageAsync(chatInfo.TelegramId, "Главное меню", replyMarkup: _tlgMenuService.GetMainMenu());
        }
    }
}