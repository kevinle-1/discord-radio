using Discord;
using Discord.WebSocket;
using DiscordRadio.Models;
using DiscordRadio.Providers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordRadio
{
    public class Radio
    {
        private DiscordSession discord;

        private static Configuration config; 

        public Radio()
        {
            config = JsonConvert.DeserializeObject<Configuration>(
                File.ReadAllText(Configuration.ConfigurationFile));
        }

        public async Task Bot()
        {
            discord = await DiscordSession.GetDiscordClient();
            
            discord.Client.Ready += BuildCommands;
            discord.Client.SlashCommandExecuted += SlashCommandHandler;

            await Task.Delay(Timeout.Infinite);
        }

        public static async Task HandleRadioCommandAsync(SocketSlashCommand command)
        {
            
            var stationSelector = new SelectMenuBuilder()
            {
                Placeholder = "Stations",
                CustomId = "radio-station-menu",
                MinValues = 1,
                MaxValues = 1,
                Options = config.Stations.Select(station =>
                    new SelectMenuOptionBuilder()
                    {
                        Label = station.Name,
                        Description = $"{station.Bitrate}kbps",
                        Value = station.Name,
                        Emote = new Emoji("📻")
                    }).ToList()
            };

            var builder = new ComponentBuilder()
                .WithSelectMenu(stationSelector);

            try
            {
                await command.RespondAsync("Select a station:", components: builder.Build());

            }
            catch (Discord.Net.HttpException e)
            {
                Console.WriteLine(e.Message); 
            }
        }

        public static async Task SlashCommandHandler(SocketSlashCommand command)
        {
            var handler = Commands.cmd
                .Single(cmd => cmd.Key.Name == command.Data.Name)
                .Value;

            await handler(command); 
        }

        public async Task BuildCommands()
        {
            foreach(var command in Commands.cmd.Keys)
            {
                await discord.Client.
                    CreateGlobalApplicationCommandAsync(command.Build());
            }
        }
    }
}
