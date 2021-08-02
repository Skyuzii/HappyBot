using System.Collections.Concurrent;
using System.Linq;
using HappyBot.Application.Common.Interfaces.Storages;
using HappyBot.Application.Common.Models;
using HappyBot.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HappyBot.Application.Common.Storages
{
    public class ChatStorage : IChatStorage
    {
        private readonly ConcurrentDictionary<string, ChatInfo> _chats;
        public ChatStorage(IServiceScopeFactory serviceScopeFactory)
        {
            _chats = new ConcurrentDictionary<string, ChatInfo>();
            
            using var scope = serviceScopeFactory.CreateScope();
            var users = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Users
                .Include(x => x.TelegramBots)
                .ToList();

            foreach (var user in users)
            {
                foreach (var telegramBot in user.TelegramBots)
                {
                    var key = GenerateKey(user.Id, telegramBot.Id);
                    var chatInfo = new ChatInfo
                    {
                        UserId = user.Id,
                        TelegramBotId = telegramBot.Id,
                        TelegramId = user.TelegramId
                    };

                    _chats.TryAdd(key, chatInfo);
                }
            }
        }
        
        public ChatInfo? Get(string chatKey)
        {
            _chats.TryGetValue(chatKey, out var chatInfo);
            return chatInfo;
        }

        public void Add(string chatKey, ChatInfo chatInfo)
        {
            _chats.TryAdd(chatKey, chatInfo);
        }

        public bool Exists(string chatKey) => _chats.ContainsKey(chatKey);

        public string GenerateKey(long telegramId, int telegramBotId) => $"{telegramId}:{telegramBotId}";
    }
}