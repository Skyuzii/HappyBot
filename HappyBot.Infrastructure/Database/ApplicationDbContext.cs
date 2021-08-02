using HappyBot.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HappyBot.Infrastructure.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        
        public DbSet<TelegramBot> TelegramBots { get; set; }
        
        public DbSet<Button> Buttons { get; set; }
        
        public DbSet<File> Files { get; set; }
    }
}