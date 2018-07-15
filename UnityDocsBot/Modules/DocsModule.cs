using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using UnityDocsBot.Models;
using UnityDocsBot.Services;

namespace UnityDocsBot.Modules
{
    public class DocsModule : ModuleBase<SocketCommandContext>
    {
        private readonly DoctagLookupService _doctagLookup;
        private readonly List<CommandInfo> _commands;
        private readonly DocsMasterUnityRetrievalService _docsService;

        public DocsModule(DoctagLookupService doctagLookup, CommandService service, DocsMasterUnityRetrievalService docsService)
        {
            _doctagLookup = doctagLookup;
            _commands = service.Commands.ToList();
            _docsService = docsService;
        }

        [Command("help"), Summary("Lists all bot commands.")]
        public async Task Help()
        {
            EmbedBuilder builder = new EmbedBuilder().WithAuthor("AVAILABLE COMMANDS");
            foreach (var command in _commands)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(command.Name + " ");
                foreach (var commandParameter in command.Parameters)
                {
                    sb.Append(commandParameter.IsOptional
                        ? $"<OPTIONAL: {commandParameter.Name}> "
                        : $"<{commandParameter.Name}> ");
                }

                builder.AddField(sb.ToString(), command.Summary);
            }

            builder.WithThumbnailUrl(Context.Client.CurrentUser.GetAvatarUrl()).WithColor(Color.Green);
            await ReplyAsync("", embed: builder.Build());
        }

        [Command("about"), Summary("About this bot...")]
        public async Task Info()
        {
            SocketUser user = Context.Client.GetUser(223834565544902656);
            await ReplyAsync(
                $"UNITY DOCS BOT V. {Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion}, written by {user?.Username ?? "RubyNova"}#{user?.Discriminator ?? "0404"}. If I break, please let him know!");
        }

        [Command("docs", RunMode = RunMode.Async), Summary("Searches the Manual for the desired name.")]
        public async Task Docs(string name, string version = "")
        {
            ManualEntryModel resultModel;
            if (string.IsNullOrWhiteSpace(version))
            {
                resultModel = await _docsService.GetManualEntryFromLatest(name);
            }
            else
            {
                resultModel = await _docsService.GetManualEntryFromVersion(name, version);
            }
            var builder = new EmbedBuilder().WithAuthor(new EmbedAuthorBuilder { Name = resultModel.EntryName })
                .WithThumbnailUrl(Context.Client.CurrentUser.GetAvatarUrl())
                .AddField("Description:", resultModel.Description).AddField("Full Reference:", resultModel.FullReferenceLink)
                .WithColor(Color.Green);
            await ReplyAsync("", embed: builder.Build());
        }

        [Command("doctag"), Summary("Additional documentation-related tags that aren't in the script reference.")]
        public async Task DocTag(string tag)
        {
            if (_doctagLookup.TryGet(tag.ToUpper(), out string[] lookup))
            {
                await ReplyAsync($"<{lookup[0]}>");
            }
            else
            {
                await ReplyAsync("DocTag not found.");
            }
        }

        [Command("doctags"), Summary("Lists all available tags that have associated data.")]
        public async Task DocTags()
        {
            await ReplyAsync(_doctagLookup.GetAllTags().Aggregate((x, y) => $"{x}, {y}"));
        }

        [Command("versions"), Summary("Displays the current manuals supported by this bot.")]
        public async Task Versions()
        {
            EmbedBuilder builder = new EmbedBuilder().WithAuthor("CURRENT SUPPORTED VERSIONS");
            foreach (string version in await _docsService.GetAllVersionsAsync())
            {
                builder.AddField(version,
                    "https://docs.unity3d.com/" + version +
                    "/Documentation/ScriptReference/index.html");
            }
            builder.WithThumbnailUrl(Context.Client.CurrentUser.GetAvatarUrl()).WithColor(Color.Green);
            await ReplyAsync("", embed: builder.Build());
        }
    }
}