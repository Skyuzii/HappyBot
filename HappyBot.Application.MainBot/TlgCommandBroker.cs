using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HappyBot.Application.Common.Interfaces.Brokers;
using HappyBot.Application.Common.Interfaces.Buttons;
using HappyBot.Application.Common.Interfaces.InlineKeyboardButton;
using HappyBot.Application.Common.Interfaces.ReplyKeyboardButton;
using HappyBot.Application.Common.Interfaces.Services;
using HappyBot.Application.Common.Interfaces.Storages;
using HappyBot.Application.Common.Models;
using HappyBot.Infrastructure.Database;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = HappyBot.Domain.Entities.User;

namespace HappyBot.Application.MainBot
{
    public class TlgCommandBroker : ITlgCommandBrokerMainBot
    {
        private readonly IChatStorage _chatStorage;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<TlgCommandBroker> _logger;
        private readonly ITlgMenuServiceMainBot _tlgMenuService;
        private readonly Dictionary<string, IButton> _buttons;
        private readonly Dictionary<UpdateType, Func<ITelegramBotClient, ChatInfo, Update, string, Task>> _updateHandlers;

        public TlgCommandBroker(
            IChatStorage chatStorage,
            ApplicationDbContext dbContext,
            ILogger<TlgCommandBroker> logger,
            ITlgMenuServiceMainBot tlgMenuService,
            IEnumerable<IReplyKBMainBot> replyKeyboardButtonsMainBot,
            IEnumerable<IInlineKBMainBot> inlineKeyboardButtonMainBot)
        {
            _chatStorage = chatStorage;
            _dbContext = dbContext;
            _logger = logger;
            _tlgMenuService = tlgMenuService;
            _buttons = replyKeyboardButtonsMainBot
                .Concat<IButton>(inlineKeyboardButtonMainBot)
                .ToDictionary(x => x.Name, y => y);
            
            _updateHandlers = new Dictionary<UpdateType, Func<ITelegramBotClient, ChatInfo, Update, string, Task>>
            {
                {UpdateType.Message, ParseMessage},
                {UpdateType.CallbackQuery, ParseCallbackQuery}
            };
        }
        
        public async Task ParseUpdate(TelegramBotDto telegramBotDto, Update update)
        {
            var chatInfo = await GetChat(telegramBotDto, update);
            var updateHandler = _updateHandlers.FirstOrDefault(x => x.Key == update.Type);
            if (updateHandler.Equals(default))
            {
                _logger.LogError($"Не поддерживаемый тип обновления - {update.Type.ToString()}");
                return;
            }

            try
            {
                await updateHandler.Value.Invoke(telegramBotDto.Client, chatInfo, update, null);
            }
            catch (Exception ex)
            {
                chatInfo.LastButton = null;
                await telegramBotDto.Client.SendTextMessageAsync(chatInfo.TelegramId, "Произошла ошибка");
                _logger.LogError(ex, $"{updateHandler.Value.Method.Name} -> {ex.Message} [chatId: {chatInfo.TelegramId}]");
            }

        }

        private async Task<ChatInfo> GetChat(TelegramBotDto telegramBotDto, Update update)
        {
            var chatId = update.Message?.Chat.Id ?? update.EditedMessage?.Chat.Id ?? update.CallbackQuery?.Message.Chat.Id;
            var chatKey = _chatStorage.GenerateKey(chatId!.Value, telegramBotDto.Id);
            var chatInfo = _chatStorage.Get(chatKey);
            if (chatInfo == null)
            {
                chatInfo = new ChatInfo
                {
                    TelegramId = chatId.Value,
                    TelegramBotId = telegramBotDto.Id,
                };
					
                _chatStorage.Add(chatKey, chatInfo);

                var user = _dbContext.Users.FirstOrDefault(x => x.TelegramId == chatId.Value);
                if (user == null)
                {
                    user = new User
                    {
                        TelegramId = chatId.Value,
                        CreateDate = DateTime.Now,
                        Name = $"{update.Message.Chat.Username}"
                    };
                    await _dbContext.Users.AddAsync(user);
                    await _dbContext.SaveChangesAsync();
                }

                chatInfo.UserId = user.Id;
            }

            return chatInfo;
        }
        
        private async Task DeleteMessageLastInlineKeyboardMessage(ITelegramBotClient telegramBotClient, ChatInfo chatInfo)
        {
            try
            {
                await telegramBotClient.DeleteMessageAsync(chatInfo.TelegramId, chatInfo.LastInlineKeyboardMessageId!.Value);
            }
            finally
            {
                chatInfo.LastInlineKeyboardMessageId = null;
            }
        }

        
        private async Task ParseMessage(ITelegramBotClient telegramBotClient, ChatInfo chatInfo, Update update, string message = null)
        {
            if (update.Message == null) return;
            message ??= (update.Message.Text ?? update.Message.Caption).Trim();
            
            if (chatInfo.LastInlineKeyboardMessageId.HasValue)
            {
                await DeleteMessageLastInlineKeyboardMessage(telegramBotClient, chatInfo);
            }
            
            if(_buttons.ContainsKey(message))
            {
                chatInfo.Argument = null;
                chatInfo.LastButton = message;
                await _buttons[message].Execute(telegramBotClient, chatInfo, update);
            }
            else if (chatInfo.LastButton != null && _buttons.ContainsKey(chatInfo.LastButton))
            {
                chatInfo.Argument = message;
                await _buttons[chatInfo.LastButton].Execute(telegramBotClient, chatInfo, update);
            }
            else
            {
                await ParseCallbackQuery(telegramBotClient, chatInfo, update, message);
            }
        }

        private async Task ParseCallbackQuery(ITelegramBotClient telegramBotClient, ChatInfo chatInfo, Update update, string message = null)
        {
            if (message == null)
            {
                message = update.CallbackQuery.Data;
            }
            else
            {
                chatInfo.Argument = message;
                message = chatInfo.LastButton;
            }

            if (chatInfo.LastInlineKeyboardMessageId.HasValue)
            {
                await DeleteMessageLastInlineKeyboardMessage(telegramBotClient, chatInfo);
            }

            var index = message.IndexOf('/');
            if (index == -1)
            {
                await telegramBotClient.SendTextMessageAsync(chatInfo.TelegramId, "Главное меню", replyMarkup: _tlgMenuService.GetMainMenu());
                return;
            }

            var callback = message.Substring(0, index + 1);
            if (_buttons.ContainsKey(callback))
            {
                if (update.CallbackQuery != null)
                    chatInfo.LastButton = callback;
    
                await _buttons[callback].Execute(telegramBotClient, chatInfo, update);
                return;
            }

            chatInfo.LastButton = null;
            chatInfo.LastInlineKeyboardMessageId = (await telegramBotClient.SendTextMessageAsync(chatInfo.TelegramId, "Неизвестная команда")).MessageId;
        }
    }
}