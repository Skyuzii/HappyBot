using System.Threading.Tasks;
using HappyBot.Application.Common.Interfaces.Buttons;
using HappyBot.Application.Common.Interfaces.ReplyKeyboardButton;
using HappyBot.Application.Common.Models;
using HappyBot.Application.MainBot.Constants;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HappyBot.Application.MainBot.ReplyKeyboardButtons
{
    public class StartReplyKeyboardButton : IReplyKeyboardButtonMainBot
    {
        public string Name => ReplyKeyboardButtonConstants.START;

        public async Task Execute(ITelegramBotClient botClient, ChatInfo chatInfo, Message message)
        {
            var msg = $"Добро пожаловать\n" +
                      $"Вас привествует Happy Bot\n" +
                      $"Хотите сделать необычный подарок для любимого человека?\n" +
                      $"Я вам с радостью помогу\n";

            await botClient.SendTextMessageAsync(chatInfo.TelegramId, msg);
        }
    }
}