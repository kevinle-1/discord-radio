using DiscordRadio.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DiscordRadio
{
    public static class Config
    {
        public const string ConfigurationFile = "config.json";

        private static readonly Configuration config = 
            JsonConvert.DeserializeObject<Configuration>(
                File.ReadAllText(ConfigurationFile));

        public static string GetDiscordToken()
        {
            return config.Token; 
        }

        public static List<Station> GetStations()
        {
            return config.Stations;
        }

        public static string GetStationUrlByName(string name)
        {
            return config.Stations
                .Single(s => s.Name == name).Stream;
        }
    }

    public class Configuration
    {
        public string Token { get; set; }

        public List<Station> Stations { get; set; }
    }
}
