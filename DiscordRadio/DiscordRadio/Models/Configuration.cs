using DiscordRadio.Models;
using System.Collections.Generic;

namespace DiscordRadio
{
    class Configuration
    {
        public string Token { get; set; }

        public List<Station> Stations { get; set; }
    }
}
