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

        public static Embed BuildStationSelectEmbed(Station station)
        {
            return new EmbedBuilder()
            {
                Title = $"Now Playing: {station.Name}",
                Url = station.Stream,
                Description = $"**Bitrate:** {station.Bitrate}kbps\nPerth, Australia\n\n{station.Description}",
                ThumbnailUrl = station.Thumbnail,
                Color = new Color(139, 205, 80),
                Footer = new EmbedFooterBuilder()
                {
                    Text = "Warning: Currently in beta, likely to have issues"
                }
            }.Build();
        }
    }
}
