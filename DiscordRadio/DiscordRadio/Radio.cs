using Discord;
using Discord.Audio;
using Discord.WebSocket;
using DiscordRadio.Models;
using DiscordRadio.Providers;
using System;
using System.Linq;
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
            
            discord.Client.Ready += RegisterCommands;

            discord.Client.SlashCommandExecuted += SlashCommandHandler;
            discord.Client.SelectMenuExecuted += StationSelectMenuHandler;

            await Task.Delay(Timeout.Infinite);
        }

        public async Task StationSelectMenuHandler(SocketMessageComponent arg)
        {
            var station = Config.GetStationByName(arg.Data.Values.Single());
            IAudioClient audioClient = null; 

            var channel = (arg.User as IGuildUser)?.VoiceChannel;

            if (channel == null)
            {
                await arg.RespondAsync("Join a voice channel!");
                return;
            }

            var botInChannel = await channel.GetUserAsync(discord.Client.CurrentUser.Id) != null;

            try
            {
                new Thread(async () =>
                {
                    var audioClient = await channel.ConnectAsync();

                    await new AudioStreamer(audioClient)
                    .StreamAudio(station.Stream);
                }).Start();

                await arg.RespondAsync(embed: Components.BuildStationSelectEmbed(station));
                // TODO: Consider deleting original message 
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message); // TODO better 
            }
        }

        public static async Task RadioCommandHandler(SocketSlashCommand command)
        {
            var builder = new ComponentBuilder()
                .WithSelectMenu(Components.StationSelectorMenu);

            await command.RespondAsync("Select a station:", components: builder.Build());
        }

        public static async Task SlashCommandHandler(SocketSlashCommand command)
        {
            var handler = Commands.cmd
                .Single(cmd => cmd.Key.Name == command.Data.Name)
                .Value;

            await handler(command); 
        }

        public async Task RegisterCommands()
        {
            foreach(var command in Commands.cmd.Keys)
            {
                await discord.Client.
                    CreateGlobalApplicationCommandAsync(command.Build());
            }
        }
    }
}
