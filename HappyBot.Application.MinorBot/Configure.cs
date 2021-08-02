using HappyBot.Application.Common.Interfaces.Brokers;
using Microsoft.Extensions.DependencyInjection;

namespace HappyBot.Application.MinorBot
{
    public static class Configure
    {
        public static IServiceCollection AddMinorBot(this IServiceCollection services)
        {
            services.AddSingleton<ITlgCommandBrokerMinorBot, TlgCommandBroker>();
            
            return services;
        }
    }
}