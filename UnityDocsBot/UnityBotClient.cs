using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using UnityDocsBot.Services;

namespace UnityDocsBot
{
    public class UnityBotClient
    {
        private DiscordSocketClient _client;
        private IConfigurationRoot _config;

        public async Task LaunchBotAsync()
        {
            _client = new DiscordSocketClient();
            var services = ConfigureServices();
            services.GetRequiredService<LoggingService>();
            await services.GetRequiredService<CommandHandlingService>().InitializeAsync(services);
            _config = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddJsonFile("config.json")
                .Build();
            await _client.LoginAsync(TokenType.Bot, _config["BotToken"]);
            await _client.StartAsync();
        }

        private IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton<HttpClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
                .AddTransient<DoctagLookupService>()
                .AddTransient<DocsMasterUnityRetrievalService>()
                .AddSingleton<LoggingService>()
                .BuildServiceProvider();
        }
    }
}
