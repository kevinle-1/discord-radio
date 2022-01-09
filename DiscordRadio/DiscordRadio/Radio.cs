using Discord.WebSocket;
using DiscordRadio.Models;
using DiscordRadio.Providers;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordRadio
{
    public class Radio
    {
        private DiscordSession discord;

        public async Task Bot()
        {
            discord = await DiscordSession.GetDiscordClient();
            
            discord.Client.Ready += BuildCommands;
            discord.Client.SlashCommandExecuted += SlashCommandHandler;

            await Task.Delay(Timeout.Infinite);
        }

        public async Task SlashCommandHandler(SocketSlashCommand command)
        {
            await command.RespondAsync("Test");
        }

        public async Task BuildCommands()
        {
            foreach(var command in Commands.List)
            {
                await discord.Client.
                    CreateGlobalApplicationCommandAsync(command.Build());
            }
        }
    }
}
