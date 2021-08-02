using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HappyBot.Domain.Entities
{
    [Table(nameof(User))]
    public class User
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public long TelegramId { get; set; }
        
        public DateTime CreateDate { get; set; }

        public IList<TelegramBot> TelegramBots { get; set; }
    }
}