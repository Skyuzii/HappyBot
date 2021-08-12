using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HappyBot.Domain.Entities
{
    [Table(nameof(TelegramBot))]
    public class TelegramBot
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public string Token { get; set; }
        
        public int UserId { get; set; }
        
        public User User { get; set; }

        public bool IsMain { get; set; }

        public bool IsEnable { get; set; }

        public List<Button> Buttons { get; set; }
    }
}