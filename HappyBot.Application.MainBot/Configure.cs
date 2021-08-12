using HappyBot.Application.Common.Interfaces.Brokers;
using HappyBot.Application.Common.Interfaces.InlineKeyboardButton;
using HappyBot.Application.Common.Interfaces.ReplyKeyboardButton;
using HappyBot.Application.Common.Interfaces.Services;
using HappyBot.Application.MainBot.InlineKeyboardButtons;
using HappyBot.Application.MainBot.ReplyKeyboardButtons;
using HappyBot.Application.MainBot.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HappyBot.Application.MainBot
{
    public static class Configure
    {
        public static IServiceCollection AddMainBot(this IServiceCollection services)
        {
            services.AddScoped<ITlgMenuServiceMainBot, TlgMenuService>();
            services.AddBrokers();
            services.AddReplyKeyboardButtons();
            services.AddInlineKeyboardButtons();

            return services;
        }

        private static IServiceCollection AddBrokers(this IServiceCollection services)
        {
            services.AddScoped<ITlgCommandBrokerMainBot, TlgCommandBroker>();
            
            return services;
        }

        private static IServiceCollection AddReplyKeyboardButtons(this IServiceCollection services)
        {
            services.AddScoped<IReplyKBMainBot, StartReplyKB>();
            services.AddScoped<IReplyKBMainBot, AddBotReplyKB>();
            services.AddScoped<IReplyKBMainBot, MyBotsReplyKB>();
            services.AddScoped<IReplyKBMainBot, CancelReplyKB>();
         
            return services;
        }

        private static IServiceCollection AddInlineKeyboardButtons(this IServiceCollection services)
        {
            services.AddScoped<IInlineKBMainBot, BotSettingsInlineKB>();
            return services;
        }
    }
}
