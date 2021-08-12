using System.Linq;
using System.Threading.Tasks;
using HappyBot.Application.Common.Constants;
using HappyBot.Application.Common.Interfaces.ReplyKeyboardButton;
using HappyBot.Application.Common.Interfaces.Storages;
using HappyBot.Application.Common.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace HappyBot.Application.MainBot.ReplyKeyboardButtons
{
    public class MyBotsReplyKB : IReplyKBMainBot
    {
        public string Name => ReplyKBMainBotConstants.MY_BOTS;

        private readonly ITlgBotStorage _tlgBotStorage;

        public MyBotsReplyKB(ITlgBotStorage tlgBotStorage)
        {
            _tlgBotStorage = tlgBotStorage;
        }

        public async Task Execute(ITelegramBotClient botClient, ChatInfo chatInfo, Update update)
        {
            var telegramBots = _tlgBotStorage.GetByUserId(chatInfo.UserId);

            var myBotsMenu = telegramBots.Select(telegramBot => new InlineKeyboardButton
            {
                Text = telegramBot.Name,
                CallbackData = InlineKBMainBotConstants.BOT_SETTINGS_MENU + telegramBot.Id
            }).ToArray();

            chatInfo.LastInlineKeyboardMessageId = (await botClient.SendTextMessageAsync(
                chatInfo.TelegramId,
                "Ваши боты",
                replyMarkup: new InlineKeyboardMarkup(new[] {myBotsMenu}))).MessageId;
        }
    }
}