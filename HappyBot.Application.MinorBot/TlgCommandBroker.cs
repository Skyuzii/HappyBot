using System;
using System.Threading.Tasks;
using HappyBot.Application.Common.Interfaces.Brokers;
using HappyBot.Application.Common.Models;
using Telegram.Bot.Types;

namespace HappyBot.Application.MinorBot
{
    public class TlgCommandBroker : ITlgCommandBrokerMinorBot
    {
        public async Task ParseUpdate(TelegramBotDto telegramBotDto, Update update)
        {
            await telegramBotDto.Client.SendTextMessageAsync(update.Message.Chat.Id, "Minor бот тебя приветсвует");
            Console.WriteLine("Minor бот!");
        }
    }
}