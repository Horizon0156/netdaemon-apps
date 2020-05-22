using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using JoySoftware.HomeAssistant.NetDaemon.Common;
using JoySoftware.HomeAssistant.NetDaemon.Common.Reactive;

namespace Horizon.SmartHome.Automations
{
    public class GuineaPigNightLight : NetDaemonRxApp
    {
        public override Task InitializeAsync()
        {
            Entity("sun.sun")
                .StateChanges
                .Where(c => c.New.State == "below_horizon")
                .Subscribe(_ => Entity("light.moppi_licht").TurnOn(new { brightness = 127}));

            // This is disposed in base class
            RunDaily("23:00:00", () => Entity("light.moppi_licht").TurnOff());

            return base.InitializeAsync();
        }
    }
}

