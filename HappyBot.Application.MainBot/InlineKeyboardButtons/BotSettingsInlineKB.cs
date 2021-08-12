using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HappyBot.Application.Common.Constants;
using HappyBot.Application.Common.Interfaces.InlineKeyboardButton;
using HappyBot.Application.Common.Interfaces.Services;
using HappyBot.Application.Common.Interfaces.Storages;
using HappyBot.Application.Common.Models;
using HappyBot.Domain.Entities;
using HappyBot.Infrastructure.Database;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace HappyBot.Application.MainBot.InlineKeyboardButtons
{
    public class BotSettingsInlineKB : IInlineKBMainBot
    {
        public string Name => InlineKBMainBotConstants.BOT_SETTINGS_PREFIX;


        private readonly IMapper _mapper;
        private readonly ITlgBotStorage _tlgBotStorage;
        private readonly ApplicationDbContext _dbContext;
        private readonly ITlgMenuServiceMainBot _tlgMenuService;

        public BotSettingsInlineKB(
            IMapper mapper,
            ITlgBotStorage tlgBotStorage,
            ApplicationDbContext dbContext,
            ITlgMenuServiceMainBot tlgMenuService)
        {
            _mapper = mapper;
            _tlgBotStorage = tlgBotStorage;
            _dbContext = dbContext;
            _tlgMenuService = tlgMenuService;
        }

        public async Task Execute(ITelegramBotClient botClient, ChatInfo chatInfo, Update update)
        {
            string data = string.Empty;
            if (update.CallbackQuery == null)
            {
                chatInfo.Argument = (update.Message.Text ?? update.Message.Caption).Trim();
                data = chatInfo.LastButton;
            }
            else
            {
                data = update.CallbackQuery.Data;
            }
            
            var arguments = data.Split('/');
            var telegramBotId = int.Parse(arguments[2]);
            var telegramBot = _tlgBotStorage.GetById(telegramBotId);
            
            switch(arguments[1])
            {
                case "Menu":
                    await SendBotMenu(botClient, telegramBot, chatInfo, arguments);
                    break;
                case "EditStartMessage":
                    await EditStartMessage(botClient, telegramBot, chatInfo, arguments);
                    break;
            }
        }

        private async Task SendBotMenu(ITelegramBotClient botClient, TelegramBotDto telegramBot, ChatInfo chatInfo, string[] arguments)
        {
            var enableButton = new InlineKeyboardButton();
            if (telegramBot.IsEnable)
            {
                enableButton.Text = InlineKBMainBotConstants.BOT_SETTINGS_DISABLE_HEADER;
                enableButton.CallbackData = InlineKBMainBotConstants.BOT_SETTINGS_DISABLE + telegramBot.Id;
            }
            else
            {
                enableButton.Text = InlineKBMainBotConstants.BOT_SETTINGS_ENABLE_HEADER;
                enableButton.CallbackData = InlineKBMainBotConstants.BOT_SETTINGS_ENABLE + telegramBot.Id;
            }

            var editStartMessageButton = new InlineKeyboardButton
            {
                Text = InlineKBMainBotConstants.BOT_SETTINGS_EDIT_START_MESSAGE_HEADER,
                CallbackData = InlineKBMainBotConstants.BOT_SETTINGS_EDIT_START_MESSAGE + telegramBot.Id
            };

            var menu = new InlineKeyboardMarkup(new[]
            {
                new [] {enableButton, editStartMessageButton}
            });

            chatInfo.LastInlineKeyboardMessageId =
                (await botClient.SendTextMessageAsync(
                    chatInfo.TelegramId,
                    $"Меню для {telegramBot.Name}",
                    replyMarkup: menu))
                .MessageId;
        }

        private async Task EditStartMessage(ITelegramBotClient botClient, TelegramBotDto telegramBot, ChatInfo chatInfo, string[] arguments)
        {
            if (chatInfo.Argument == null)
            {
                chatInfo.LastButton = InlineKBMainBotConstants.BOT_SETTINGS_EDIT_START_MESSAGE + telegramBot.Id;
                await botClient.SendTextMessageAsync(chatInfo.TelegramId, "Введите стартовое сообщение", replyMarkup: _tlgMenuService.GetCancelButton());
                return;
            }

            var startButtonDto = telegramBot.Buttons.FirstOrDefault(x => x.Name == ReplyKBMainBotConstants.START);
            if (startButtonDto != null)
            {
                startButtonDto.MessageText = chatInfo.Argument;
                await botClient.SendTextMessageAsync(chatInfo.TelegramId, "Сообщение успешно обновлено", replyMarkup: _tlgMenuService.GetMainMenu());
                return;
            }
            
            startButtonDto = new ButtonDto
            {
                Type = 1,
                Name = "/start",
                TelegramBotId = telegramBot.Id,
                MessageText = chatInfo.Argument
            };

            telegramBot.Buttons.Add(startButtonDto);

            var startButton = _mapper.Map<Button>(startButtonDto);
            await _dbContext.Buttons.AddAsync(startButton);
            await _dbContext.SaveChangesAsync();

            await botClient.SendTextMessageAsync(chatInfo.TelegramId, "Сообщение успешно обновлено", replyMarkup: _tlgMenuService.GetMainMenu());
            chatInfo.LastButton = null;
        }
    }
}