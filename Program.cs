using System.Threading.Tasks;
using JoySoftware.HomeAssistant.NetDaemon.DaemonRunner.Service;

namespace Horizon.SmartHome
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            await Runner.Run(args);
        }
    }
}
