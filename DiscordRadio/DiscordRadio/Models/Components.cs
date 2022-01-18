using Discord;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace DiscordRadio.Models
{
    public static class Components
    {
        public static SelectMenuBuilder StationSelectorMenu = new SelectMenuBuilder()
        {
            Placeholder = "Stations",
            CustomId = "radio-station-menu",
            MinValues = 1,
            MaxValues = 1,
            Options = Config.GetStations().Select(station =>
                new SelectMenuOptionBuilder()
                {
                    Label = station.Name,
                    Description = $"{station.Bitrate}kbps - {station.Description}", // Max 100 char
                        Value = station.Name,
                    Emote = new Emoji("📻") // Cute lil radio
                    }).ToList()
        };

        // Radio Embed
    }
}
