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

        private IAudioClient activeAudioClient;
        private Thread streamingThread; 

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
            var stationName = arg.Data.Values.Single();

            var channel = (arg.User as IGuildUser)?.VoiceChannel;

            if (channel == null)
            {
                await arg.RespondAsync("Join a voice channel!");
                return;
            }

            var botInChannel = await channel.GetUserAsync(discord.Client.CurrentUser.Id) != null;

            try
            {
                streamingThread = new Thread(async () =>
                {
                    if(!botInChannel || activeAudioClient == null)
                        activeAudioClient = await channel.ConnectAsync();

                    if (streamingThread != null)
                    {
                        streamingThread.Interrupt(); // Not cleaning up properly 
                    }

                    await new AudioStreamer(activeAudioClient)
                    .StreamAudio(Config.GetStationUrlByName(stationName));
                });

                streamingThread.Start(); 
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message); 
            }

            // Set bot nick 
            await arg.RespondAsync($"Connecting to station: {stationName}"); // TODO: Use an embed
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
