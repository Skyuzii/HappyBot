using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HappyBot.Infrastructure.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HappyBot.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var hostBuild = CreateHostBuilder(args).Build();

            using var scope = hostBuild.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
            await ApplicationDbContextSeed.SetDefaultDataAsync(dbContext);

            await hostBuild.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}