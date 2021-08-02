using System;
using System.Threading.Tasks;
using HappyBot.Application.Common.Interfaces.Brokers;
using HappyBot.Application.Common.Interfaces.Storages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace HappyBot.Api.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class TelegramBotController : ControllerBase
    {
        private readonly ITlgBotStorage _tlgBotStorage;
        private readonly ILogger<TelegramBotController> _logger;
        private readonly ITlgCommandBrokerMainBot _tlgCommandBrokerMainBot;
        private readonly ITlgCommandBrokerMinorBot _tlgCommandBrokerMinorBot;

        public TelegramBotController(
            ILogger<TelegramBotController> logger,
            ITlgBotStorage tlgBotStorage,
            ITlgCommandBrokerMainBot tlgCommandBrokerMainBot,
            ITlgCommandBrokerMinorBot tlgCommandBrokerMinorBot)
        {
            _logger = logger;
            _tlgBotStorage = tlgBotStorage;
            _tlgCommandBrokerMainBot = tlgCommandBrokerMainBot;
            _tlgCommandBrokerMinorBot = tlgCommandBrokerMinorBot;
        }
        
        [HttpPost("{token}/{telegramBotId}")]
        public async Task Handle(string token, int telegramBotId, [FromBody] Update update)
        {
            var telegramBot = _tlgBotStorage.Get(telegramBotId);
            if (telegramBot == null)
            {
                _logger.LogError($"Telegram bot not found -> {telegramBotId} - {token}");
                return;
            }
            
            if (telegramBot.IsMainBot)
            {
                await _tlgCommandBrokerMainBot.ParseUpdate(telegramBot, update);
            }
            else
            {
                await _tlgCommandBrokerMinorBot.ParseUpdate(telegramBot, update);
            }
        }

    }
}