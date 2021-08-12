using System.Reflection;
using HappyBot.Application.Common.Interfaces.Storages;
using HappyBot.Application.Common.Storages;
using Microsoft.Extensions.DependencyInjection;

namespace HappyBot.Application
{
    public static class Configure
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<IChatStorage, ChatStorage>();
            services.AddSingleton<ITlgBotStorage, TlgBotStorage>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            
            return services;
        }
    }
}