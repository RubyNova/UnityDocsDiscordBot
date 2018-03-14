using System;
using System.Collections.Generic;
using System.Text;
using Discord.Commands;
using Discord.WebSocket;

namespace UnityDocsBot.Modules
{
    public class DocsCommandContext : SocketCommandContext
    {
        public Dictionary<string, Dictionary<string, string[]>> Docs { get; }
        public string CurrentVersion { get; }

        public List<CommandInfo> Commands { get; }

        public DocsCommandContext(DiscordSocketClient client, SocketUserMessage msg, Dictionary<string, Dictionary<string, string[]>> docs, List<string> versions, string currentVersion, List<CommandInfo> commands) : base(client, msg)
        {
            Docs = docs;
            CurrentVersion = currentVersion;
            Commands = commands;
        }

    }
}
