using Discord;
using Discord.WebSocket;
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

            await client.LoginAsync(TokenType.Bot, Config.GetDiscordToken());
            await client.StartAsync();

            return new DiscordSession(client); 
        }
    }
}
