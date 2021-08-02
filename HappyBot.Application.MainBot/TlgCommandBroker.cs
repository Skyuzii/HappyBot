using System;
using System.Threading.Tasks;
using HappyBot.Application.Common.Interfaces.Brokers;
using HappyBot.Application.Common.Models;
using Telegram.Bot.Types;

namespace HappyBot.Application.MainBot
{
    public class TlgCommandBroker : ITlgCommandBrokerMainBot
    {
        public async Task ParseUpdate(TelegramBotDto telegramBotDto, Update update)
        {
            await telegramBotDto.Client.SendTextMessageAsync(update.Message.Chat.Id, "Main бот тебя приветсвует");
            Console.WriteLine("Main бот!");
        }
    }
}