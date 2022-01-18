using Discord.Audio;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DiscordRadio.Providers
{
    public class AudioStreamer
    {
        private readonly IAudioClient audioClient; 

        public AudioStreamer(IAudioClient _audioClient)
        {
            audioClient = _audioClient; 
        }

        public async Task StreamAudio(string streamUrl)
        {
            var discord = audioClient.CreatePCMStream(AudioApplication.Mixed);
            await discord.FlushAsync();

            using (var ffmpeg = GetStream(streamUrl))
            using (var output = ffmpeg.StandardOutput.BaseStream)
            {
                try { await output.CopyToAsync(discord); }
                finally { await discord.FlushAsync(); }
            }
        }

        private Process GetStream(string url)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-hide_banner -loglevel panic -i \"{url}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
            });
        }
    }
}
