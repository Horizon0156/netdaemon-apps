using System.Threading.Tasks;
using JoySoftware.HomeAssistant.NetDaemon.Common;

namespace Horizon.SmartHome.Automations
{
    public class GuineaPigNightLight : NetDaemonApp
    {
        public override Task InitializeAsync()
        {
            Entity("sun.sun")
                .WhenStateChange(to: "below_horizon")
                .UseEntity("light.moppi_licht")
                .TurnOn()
                .WithAttribute("brightness", 127)
                .Execute();

            Scheduler.RunDaily(
                "23:00:00", 
                async () => await Entity("light.moppi_licht").TurnOff().ExecuteAsync());

            return base.InitializeAsync();
        }
    }
}

