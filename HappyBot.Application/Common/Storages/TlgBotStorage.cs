using System.Collections.Concurrent;
using System.Linq;
using HappyBot.Application.Common.Interfaces.Storages;
using HappyBot.Application.Common.Models;
using HappyBot.Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace HappyBot.Application.Common.Storages
{
    public class TlgBotStorage : ITlgBotStorage
    {
        private readonly SettingsTelegramBot _settingsTelegramBot;
        private ConcurrentDictionary<int, TelegramBotDto> _telegramBots;

        public TlgBotStorage(IServiceScopeFactory serviceScopeFactory)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            _settingsTelegramBot = scope.ServiceProvider.GetRequiredService<IOptions<SettingsTelegramBot>>().Value;

            _telegramBots = new ConcurrentDictionary<int, TelegramBotDto>(dbContext.TelegramBots.ToDictionary(
                x => x.Id, telegramBot => new TelegramBotDto
                {
                    Id = telegramBot.Id,
                    Token = telegramBot.Token,
                    IsMainBot = telegramBot.IsMain,
                    IsEnable = telegramBot.IsEnable,
                    Client = new TelegramBotClient(telegramBot.Token)
                }));
            
            UpdateWebhooks();
        }

        public void UpdateWebhook(TelegramBotDto telegramBot)
        {
            telegramBot.Client.SetWebhookAsync($"{_settingsTelegramBot.Url}/api/TelegramBot/{telegramBot.Token}");
        }

        public void UpdateWebhooks()
        {
            foreach (var telegramBot in _telegramBots.Where(x => x.Value.IsEnable).Select(x => x.Value))
            {
                UpdateWebhook(telegramBot);
            }
        }

        public TelegramBotDto? Get(int telegramBotId) => _telegramBots.ContainsKey(telegramBotId) ? _telegramBots[telegramBotId] : null;

        public TelegramBotDto? Get(string token)
        {
            var telegramBot = _telegramBots.FirstOrDefault(x => x.Value.Token == token);
            return telegramBot.Equals(default) 
                ? null 
                : telegramBot.Value;
        }
    }
}