using System;
using System.Threading.Tasks;

namespace UnityDocsBotConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var tcs = new TaskCompletionSource<bool>();

            Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                tcs.SetResult(true);
            };

            var t = new UnityDocsBot.UnityBotClient("json-manuals").LaunchBotAsync();

            Task.WaitAll(t, tcs.Task);
        }
    }
}
