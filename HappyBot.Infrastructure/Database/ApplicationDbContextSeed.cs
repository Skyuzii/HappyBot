using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HappyBot.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HappyBot.Infrastructure.Database
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SetDefaultDataAsync(ApplicationDbContext dbContext)
        {
            try
            {
                await dbContext.Database.MigrateAsync();
                if (dbContext.TelegramBots.Any()) return;

                var user = new User
                {
                    Name = "Musa",
                    TelegramId = 699777633,
                    CreateDate = DateTime.Now
                };

                await dbContext.Users.AddAsync(user);
                await dbContext.SaveChangesAsync();

                var telegramBots = new List<TelegramBot>
                {
                    new TelegramBot
                    {
                        UserId = user.Id,
                        Token = "1839804433:AAGMFRAsioslcEpnp3gKAg_R-ZmpqPOkNvM"
                    },
                    new TelegramBot
                    {
                        UserId = user.Id,
                        Token = "1839584284:AAEg9S3VlKcSMLGCf2-ywjgRkXdgruEhr3w"
                    },
                    new TelegramBot
                    {
                        UserId = user.Id,
                        Token = "1780697325:AAFV1r1MeMlYg39X0aEjEjOoP96KrL-pGAw"
                    },
                };

                foreach (var telegramBot in telegramBots)
                {
                    await dbContext.TelegramBots.AddAsync(telegramBot);
                }

                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        } 
    }
}