using System.Threading.Tasks;
using HappyBot.Application.Common.Interfaces.ReplyKeyboardButton;
using HappyBot.Application.Common.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HappyBot.Application.MinorBot.ReplyKeyboardButton
{
    public class StartReplyKeyboardButton : IReplyKeyboardButtonMinorBot
    {
        public string Name { get; }

        public Task Execute(ITelegramBotClient botClient, ChatInfo chatInfo, Update update)
        {
            throw new System.NotImplementedException();
        }
    }
}