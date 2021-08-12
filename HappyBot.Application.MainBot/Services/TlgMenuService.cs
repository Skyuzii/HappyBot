using HappyBot.Application.Common.Constants;
using HappyBot.Application.Common.Interfaces.Services;
using Telegram.Bot.Types.ReplyMarkups;

namespace HappyBot.Application.MainBot.Services
{
    public class TlgMenuService : ITlgMenuServiceMainBot
    {
        public ReplyKeyboardMarkup GetMainMenu()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new []
                    {
                        new KeyboardButton(ReplyKBMainBotConstants.ADD_BOT), 
                        new KeyboardButton(ReplyKBMainBotConstants.MY_BOTS) 
                    }
                }
            };
        }
        
        public ReplyKeyboardMarkup GetCancelButton()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new []
                    {
                        new KeyboardButton(ReplyKBMainBotConstants.CANCEL), 
                    }
                }
            };
        }
    }
}