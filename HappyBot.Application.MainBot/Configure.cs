using HappyBot.Application.Common.Interfaces.Brokers;
using Microsoft.Extensions.DependencyInjection;

namespace HappyBot.Application.MainBot
{
    public static class Configure
    {
        public static IServiceCollection AddMainBot(this IServiceCollection services)
        {
            services.AddSingleton<ITlgCommandBrokerMainBot, TlgCommandBroker>();
            
            return services;
        }
    }
}
