using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
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
        private readonly SettingsTelegramBot _settingsTelegramBot;
        private ConcurrentDictionary<int, TelegramBotDto> _telegramBots;

        public TlgBotStorage(IServiceScopeFactory serviceScopeFactory)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
            _settingsTelegramBot = scope.ServiceProvider.GetRequiredService<IOptions<SettingsTelegramBot>>().Value;

            var telegramBots = dbContext.TelegramBots.Include(x => x.Buttons);
            _telegramBots = new ConcurrentDictionary<int, TelegramBotDto>(telegramBots.ToDictionary(
                x => x.Id, telegramBot => new TelegramBotDto
                {
                    Id = telegramBot.Id,
                    Name = telegramBot.Name,
                    Token = telegramBot.Token,
                    UserId = telegramBot.UserId,
                    IsMainBot = telegramBot.IsMain,
                    IsEnable = telegramBot.IsEnable,
                    Client = new TelegramBotClient(telegramBot.Token),
                    Buttons = mapper.Map<IList<ButtonDto>>(telegramBot.Buttons)
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

        public TelegramBotDto? GetById(int telegramBotId) => _telegramBots.ContainsKey(telegramBotId) ? _telegramBots[telegramBotId] : null;

        public TelegramBotDto? GetByToken(string token)
        {
            var telegramBot = _telegramBots.FirstOrDefault(x => x.Value.Token == token);
            return telegramBot.Equals(default) 
                ? null 
                : telegramBot.Value;
        }

        public IList<TelegramBotDto> GetByUserId(int userId)
        {
            return _telegramBots
                .Where(x => x.Value.UserId == userId)
                .Select(x => x.Value)
                .ToList();
        }

        public void Add(TelegramBotDto telegramBotDto)
        {
            _telegramBots.TryAdd(telegramBotDto.Id, telegramBotDto);
        }
    }
}