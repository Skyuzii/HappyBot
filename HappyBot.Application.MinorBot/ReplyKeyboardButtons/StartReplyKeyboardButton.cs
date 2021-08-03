using System.Threading.Tasks;
using HappyBot.Application.Common.Interfaces.Buttons;
using HappyBot.Application.Common.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HappyBot.Application.MinorBot.ReplyKeyboardButtons
{
    public class StartReplyKeyboardButton : IButton
    {
        public string Name { get; }

        public Task Execute(ITelegramBotClient botClient, ChatInfo chatInfo, Message message)
        {
            throw new System.NotImplementedException();
        }
    }
}