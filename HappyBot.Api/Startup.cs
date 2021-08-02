using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HappyBot.Application;
using HappyBot.Application.Common.Interfaces.Storages;
using HappyBot.Application.Common.Models;
using HappyBot.Application.MainBot;
using HappyBot.Application.MinorBot;
using HappyBot.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace HappyBot.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<SettingsTelegramBot>(Configuration.GetSection(nameof(SettingsTelegramBot)));
            services.AddControllers()
                .AddNewtonsoftJson();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "HappyBot.Api", Version = "v1"});
            });

            services.AddInfrastructure(Configuration);
            services.AddApplication();
            services.AddMainBot();
            services.AddMinorBot();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ITlgBotStorage tlgBotStorage)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // using var scope = scopeFactory.CreateScope();
            // scope.ServiceProvider.GetRequiredService<ITlgBotStorage>().UpdateWebhooks();
            tlgBotStorage.UpdateWebhooks();
            
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HappyBot.Api v1"));
            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}