using System.Threading.Tasks;

namespace DiscordRadio
{
    public class Program 
    {
        static Task Main(string[] args)
        {
            return new Radio().Bot(); 
        }
    }
}
