using DiscordRadio.Models;
using System.Collections.Generic;

namespace DiscordRadio
{
    class Configuration
    {
        public const string ConfigurationFile = "config.json";

        public string Token { get; set; }

        public List<Station> Stations { get; set; }
    }
}
