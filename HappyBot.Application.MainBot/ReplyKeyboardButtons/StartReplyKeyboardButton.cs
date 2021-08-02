using System.Threading.Tasks;
using HappyBot.Application.Common.Interfaces.ReplyKeyboardButton;
using HappyBot.Application.Common.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HappyBot.Application.MainBot.ReplyKeyboardButton
{
    public class StartReplyKeyboardButton : IReplyKeyboardButtonMainBot
    {
        public string Name { get; }

        public Task Execute(ITelegramBotClient botClient, ChatInfo chatInfo, Update update)
        {
            throw new System.NotImplementedException();
        }
    }
}