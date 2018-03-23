using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private Dictionary<string, Dictionary<string, string[]>> _data;
        private string _currentVersion;
        private List<string> _versions;
        private IConfigurationRoot _config;

        public UnityBotClient(string jsonPath)
        {
            _data = new Dictionary<string, Dictionary<string, string[]>>();
            foreach (var file in Directory.GetFiles(jsonPath))
            {
                _data.Add(new FileInfo(file).Name.Replace(".json", "").Replace('-', '.'), JsonConvert.DeserializeObject<Dictionary<string, string[]>>(File.ReadAllText(file)));
            }
            _currentVersion = _data.Keys.OrderByDescending(Convert.ToDecimal).First();
            _versions = _data.Keys.ToList();
        }

        public async Task LaunchBotAsync()
        {
            _client = new DiscordSocketClient();
            var services = ConfigureServices();
            services.GetRequiredService<LoggingService>();
            await services.GetRequiredService<CommandHandlingService>().InitializeAsync(services);
            _config = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddJsonFile("config.json")
                .Build();
            await _client.LoginAsync(TokenType.Bot, _config["BotToken"]); //TODO: commit seppuku
            await _client.StartAsync();
        }

        private IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                // Base
                .AddSingleton(_currentVersion)
                .AddSingleton(_client)
                .AddSingleton(_data)
                .AddSingleton(_versions)
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<DocLookupService>()
                // Logging
                .AddSingleton<LoggingService>()
                // Add additional services here...
                .BuildServiceProvider();
        }

        //private IConfiguration BuildConfig()
        //{
        //    return new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("config.json")
        //        .Build();
        //}
    }
}
