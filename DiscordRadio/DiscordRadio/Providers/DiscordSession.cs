using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace DiscordRadio.Providers
{
    public class DiscordSession
    {
        public DiscordSocketClient Client { get; }

        private DiscordSession(DiscordSocketClient client)
        {
            Client = client;
        }

        public static async Task<DiscordSession> GetDiscordClient()
        {
            var client = new DiscordSocketClient();

            var config = JsonConvert.DeserializeObject<Configuration>(
                File.ReadAllText(Configuration.ConfigurationFile)).Token;

            await client.LoginAsync(TokenType.Bot, config);
            await client.StartAsync();

            return new DiscordSession(client); 
        }
    }
}
