using System.Threading.Tasks;
using AutoMapper;
using HappyBot.Application.Common.Constants;
using HappyBot.Application.Common.Interfaces.ReplyKeyboardButton;
using HappyBot.Application.Common.Interfaces.Services;
using HappyBot.Application.Common.Interfaces.Storages;
using HappyBot.Application.Common.Models;
using HappyBot.Domain.Entities;
using HappyBot.Infrastructure.Database;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HappyBot.Application.MainBot.ReplyKeyboardButtons
{
    public class AddBotReplyKB : IReplyKBMainBot
    {
        public string Name => ReplyKBMainBotConstants.ADD_BOT;


        private readonly ITlgBotStorage _tlgBotStorage
            ;
        private readonly ApplicationDbContext _dbContext;
        private readonly ITlgMenuServiceMainBot _tlgMenuService;
        private readonly IMapper _mapper;

        public AddBotReplyKB(
            IMapper mapper,
            ITlgBotStorage tlgBotStorage,
            ApplicationDbContext dbContext,
            ITlgMenuServiceMainBot tlgMenuService)
        {
            _tlgBotStorage = tlgBotStorage;
            _dbContext = dbContext;
            _tlgMenuService = tlgMenuService;
            _mapper = mapper;
        }
        
        public async Task Execute(ITelegramBotClient botClient, ChatInfo chatInfo, Update update)
        {
            if (chatInfo.Argument == null)
            {
                await botClient.SendTextMessageAsync(chatInfo.TelegramId, "Введите токен бота", replyMarkup: _tlgMenuService.GetCancelButton());
                return;
            }

            var token = chatInfo.Argument;
            if (!token.Contains(":"))
            {
                await botClient.SendTextMessageAsync(chatInfo.TelegramId, "Проверьте правильность токена", replyMarkup: _tlgMenuService.GetCancelButton());
                return;
            }

            var telegramBotClient = new TelegramBotClient(token);
            var telegramBot = new TelegramBot
            {
                Token = token,
                UserId = chatInfo.UserId,
                Name = (await telegramBotClient.GetMeAsync()).Username
            };

            await _dbContext.TelegramBots.AddAsync(telegramBot);
            await _dbContext.SaveChangesAsync();

            var telegramBotDto = _mapper.Map<TelegramBotDto>(telegramBot);
            telegramBotDto.Client = telegramBotClient;

            _tlgBotStorage.Add(telegramBotDto);

            await botClient.SendTextMessageAsync(chatInfo.TelegramId, "Бот успешно добавлен", replyMarkup: _tlgMenuService.GetMainMenu());
            chatInfo.LastButton = null;
        }
    }
}