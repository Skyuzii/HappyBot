using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using HappyBot.Application.Common.Interfaces.Storages;
using HappyBot.Application.Common.Models;
using HappyBot.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace HappyBot.Application.Common.Storages
{
    public class TlgBotStorage : ITlgBotStorage
    {
        private ApplicationDbContext _dbContext;
        private readonly SettingsTelegramBot _settingsTelegramBot;
        private ConcurrentDictionary<int, TelegramBotDto> _telegramBots;

        public TlgBotStorage(IServiceScopeFactory serviceScopeFactory)
        {
            using var scope = serviceScopeFactory.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            _settingsTelegramBot = scope.ServiceProvider.GetRequiredService<IOptions<SettingsTelegramBot>>().Value;

            InitTelegramBots();
        }

        public void InitTelegramBots()
        {
            var res = _dbContext.TelegramBots.ToList();
            _telegramBots = new ConcurrentDictionary<int, TelegramBotDto>(_dbContext.TelegramBots.ToDictionary(
                x => x.Id, telegramBot => new TelegramBotDto
                {
                    Id = telegramBot.Id,
                    Token = telegramBot.Token,
                    Client = new TelegramBotClient(telegramBot.Token),
                    IsMainBot = telegramBot.Token == _settingsTelegramBot.SettingsMainBot.Token
                }));
        }

        public void UpdateWebhooks()
        {
            foreach (var telegramBot in _telegramBots.Select(x => x.Value))
            {
                telegramBot.Client.SetWebhookAsync($"{_settingsTelegramBot.Url}/api/TelegramBot/{telegramBot.Token}/{telegramBot.Id}");
                //telegramBot.Client.SetWebhookAsync($"{_settingsTelegramBot.Url}/api/TelegramBot/{telegramBot.Token}");
            }
        }
        
        public TelegramBotDto? Get(int telegramBotId) => _telegramBots.ContainsKey(telegramBotId) ? _telegramBots[telegramBotId] : null;
    }
}