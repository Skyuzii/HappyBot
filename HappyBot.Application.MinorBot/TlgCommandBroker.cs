using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HappyBot.Application.Common.Interfaces.Brokers;
using HappyBot.Application.Common.Interfaces.Buttons;
using HappyBot.Application.Common.Interfaces.InlineKeyboardButton;
using HappyBot.Application.Common.Interfaces.ReplyKeyboardButton;
using HappyBot.Application.Common.Interfaces.Storages;
using HappyBot.Application.Common.Models;
using HappyBot.Infrastructure.Database;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = HappyBot.Domain.Entities.User;

namespace HappyBot.Application.MinorBot
{
    public class TlgCommandBroker : ITlgCommandBrokerMinorBot
    {
        private readonly IChatStorage _chatStorage;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<TlgCommandBroker> _logger;
        private readonly Dictionary<string, IButton> _buttons;
        private readonly Dictionary<UpdateType, Func<ITelegramBotClient, ChatInfo, Update, Task>> _updateHandlers;

        public TlgCommandBroker(
            IChatStorage chatStorage,
            ApplicationDbContext dbContext,
            ILogger<TlgCommandBroker> logger,
            IEnumerable<IReplyKeyboardButtonMinorBot> replyKeyboardButtonsMinorBot,
            IEnumerable<IInlineKeyboardButtonMinorBot> inlineKeyboardButtonMinorBot)
        {
            _chatStorage = chatStorage;
            _dbContext = dbContext;
            _logger = logger;
            _buttons = replyKeyboardButtonsMinorBot
                .Concat<IButton>(inlineKeyboardButtonMinorBot)
                .ToDictionary(x => x.Name, y => y);
            
            _updateHandlers = new Dictionary<UpdateType, Func<ITelegramBotClient, ChatInfo, Update, Task>>
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
                await updateHandler.Value.Invoke(telegramBotDto.Client, chatInfo, update);
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
                
                var user = new User
                {
                    TelegramId = chatId.Value,
                    CreateDate = DateTime.Now,
                    Name = $"{update.Message?.Chat.Username}"
                };
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();

                chatInfo.UserId = user.Id;
            }

            return chatInfo;
        }
        
        private async Task ParseMessage(ITelegramBotClient telegramBotClient, ChatInfo chatInfo, Update update)
        {
            if (update.Message == null) return;
            var msgText = (update.Message.Text ?? update.Message.Caption).Trim();
			
            await CommonParseUpdate(telegramBotClient, chatInfo, msgText, update.Message);
        }
		
        private async Task ParseCallbackQuery(ITelegramBotClient telegramBotClient, ChatInfo chatInfo, Update update)
        {
            var msgText = update.CallbackQuery?.Data?.Trim();
            if (msgText == null) return;

            await CommonParseUpdate(telegramBotClient, chatInfo, msgText, update.CallbackQuery.Message);
        }

        private async Task CommonParseUpdate(ITelegramBotClient telegramBotClient, ChatInfo chatInfo, string msgText, Message message)
        {
            if (chatInfo.LastInlineKeyboardMessageId.HasValue)
            {
                // await DeleteMessageAsync(telegramBotClient, chatInfo);
            }
            if(_buttons.ContainsKey(msgText))
            {
                chatInfo.Argument = null;
                chatInfo.LastButton = msgText;
                await _buttons[msgText].Execute(telegramBotClient, chatInfo, message);
            }
            else if (chatInfo.LastButton != null && _buttons.ContainsKey(chatInfo.LastButton))
            {
                chatInfo.Argument = msgText;
                await _buttons[chatInfo.LastButton].Execute(telegramBotClient, chatInfo, message);
            }
            else
            {
                chatInfo.LastButton = null;
                chatInfo.LastInlineKeyboardMessageId = (await telegramBotClient.SendTextMessageAsync(chatInfo.TelegramId, "Неизвестная команда")).MessageId;
            }
        }
    }
}