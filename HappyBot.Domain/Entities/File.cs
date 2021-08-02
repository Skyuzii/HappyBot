using System.ComponentModel.DataAnnotations.Schema;

namespace HappyBot.Domain.Entities
{
    [Table(nameof(File))]
    public class File
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public string Extension { get; set; }

        public int ButtonId { get; set; }
        
        public Button Button { get; set; }
    }
}