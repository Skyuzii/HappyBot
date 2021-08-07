using System.Collections.Concurrent;
using HappyBot.Application.Common.Interfaces.Storages;
using HappyBot.Application.Common.Models;

namespace HappyBot.Application.Common.Storages
{
    public class ChatStorage : IChatStorage
    {
        private readonly ConcurrentDictionary<string, ChatInfo> _chats;
        public ChatStorage()
        {
            _chats = new ConcurrentDictionary<string, ChatInfo>();
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