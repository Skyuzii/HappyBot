using Telegram.Bot;

namespace HappyBot.Application.Common.Models
{
    public class TelegramBotDto
    {
        public int Id { get; set; }
        public string Token { get; set; }

        public bool IsMainBot { get; set; }
        
        public ITelegramBotClient Client { get; set; }
    }
}