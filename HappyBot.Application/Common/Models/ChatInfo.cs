namespace HappyBot.Application.Common.Models
{
    public class ChatInfo
    {
        public long TelegramId { get; set; }
        
        public int TelegramBotId { get; set; }
        
        public int UserId { get; set; }
        
        public string? LastButton { get; set; }

        public string? Argument { get; set; }

        public int? LastInlineKeyboardMessageId { get; set; }
    }
}