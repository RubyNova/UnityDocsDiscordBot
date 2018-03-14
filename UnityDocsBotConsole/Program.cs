using System;

namespace UnityDocsBotConsole
{
    class Program
    {
        static void Main(string[] args) => new UnityDocsBot.UnityBotClient("json-manuals").LaunchBotAsync().GetAwaiter().GetResult();
    }
}
