using HappyBot.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HappyBot.Infrastructure
{
    public static class Configure
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), builder =>
                    builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            
            return services;
        }
    }
}