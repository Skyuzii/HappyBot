using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HappyBot.Domain.Entities
{
    [Table(nameof(Button))]
    public class Button
    {
        public int Id { get; set; }
        
        public string HeaderName { get; set; }
        
        public string Name { get; set; }

        public int Type { get; set; }
        
        public string MessageText { get; set; }
        
        public int TelegramBotId { get; set; }
        
        public TelegramBot TelegramBot { get; set; }

        public IList<File> Files { get; set; }
    }
}