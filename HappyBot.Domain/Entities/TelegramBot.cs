using System.ComponentModel.DataAnnotations.Schema;

namespace HappyBot.Domain.Entities
{
    [Table(nameof(TelegramBot))]
    public class TelegramBot
    {
        public int Id { get; set; }
        
        public string Token { get; set; }
        
        public int UserId { get; set; }
        
        public User User { get; set; }
    }
}