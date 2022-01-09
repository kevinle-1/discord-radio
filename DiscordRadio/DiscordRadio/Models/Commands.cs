using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordRadio.Models
{
    public static class Commands
    {
        public readonly static Dictionary<SlashCommandBuilder, Func<SocketSlashCommand, Task>> cmd = 
            new()
            {
                {
                    new SlashCommandBuilder()
                    {
                        Name = "radio",
                        Description = "Load a radio station"
                    },
                    async (command) => await Radio.HandleRadioCommandAsync(command)
                }
            };
    }
}
