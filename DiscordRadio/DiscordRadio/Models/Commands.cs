using Discord;
using System.Collections.Generic;

namespace DiscordRadio.Models
{
    public static class Commands
    {
        public readonly static List<SlashCommandBuilder> List =
            new()
            {
                new SlashCommandBuilder()
                {
                    Name = "radio",
                    Description = "Load a radio station DOTNET"
                }
            };
    }
}
