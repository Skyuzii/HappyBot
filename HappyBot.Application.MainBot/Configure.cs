using HappyBot.Application.Common.Interfaces.Brokers;
using HappyBot.Application.Common.Interfaces.Buttons;
using HappyBot.Application.Common.Interfaces.ReplyKeyboardButton;
using HappyBot.Application.MainBot.ReplyKeyboardButtons;
using Microsoft.Extensions.DependencyInjection;

namespace HappyBot.Application.MainBot
{
    public static class Configure
    {
        public static IServiceCollection AddMainBot(this IServiceCollection services)
        {
            services.AddScoped<ITlgCommandBrokerMainBot, TlgCommandBroker>();
            services.AddScoped<IReplyKeyboardButtonMainBot, StartReplyKeyboardButton>();
            
            return services;
        }
    }
}
