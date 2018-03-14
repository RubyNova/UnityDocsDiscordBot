using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using UnityDocsBot.Modules;

namespace UnityDocsBot.Services
{
    //This service is just copied code from Foxbot's example, if it ain't broke don't fix it ¯\_(ツ)_/¯
    public class CommandHandlingService
    {
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private IServiceProvider _provider;
        public static string CurrentUnityVersion { get; set; }

        public CommandHandlingService(IServiceProvider provider, DiscordSocketClient discord, CommandService commands, string currentUnityVersion)
        {
            _discord = discord;
            _commands = commands;
            _provider = provider;
            CurrentUnityVersion = currentUnityVersion;
            _discord.MessageReceived += MessageReceived;
        }

        public async Task InitializeAsync(IServiceProvider provider)
        {
            _provider = provider;
            await _commands.AddModulesAsync(Assembly.GetExecutingAssembly());
            // Add additional initialization code here...
        }

        private async Task MessageReceived(SocketMessage rawMessage)
        {
            //Console.Out.WriteLineAsync("");
            // Ignore system messages and messages from bots
            if (!(rawMessage is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;

            int argPos = 0;
            if (!message.HasMentionPrefix(_discord.CurrentUser, ref argPos)) return;

            var context = new DocsCommandContext(_discord, message, _provider.GetRequiredService<Dictionary<string, Dictionary<string, string[]>>>(), _provider.GetRequiredService<List<string>>(), CurrentUnityVersion, _commands.Commands.ToList());
            var result = await _commands.ExecuteAsync(context, argPos, _provider);

            if (result.Error.HasValue &&
                result.Error.Value != CommandError.UnknownCommand)
                await context.Channel.SendMessageAsync(result.ToString());
            else if (result.Error.HasValue &&
                     result.Error.Value == CommandError.UnknownCommand)
                await _commands.ExecuteAsync(context, "docs" + message.Content.Replace(_discord.CurrentUser.Mention.Replace("!", ""), ""), _provider);
        }

    }
}
