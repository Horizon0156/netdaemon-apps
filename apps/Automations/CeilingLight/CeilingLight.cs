using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using JoySoftware.HomeAssistant.NetDaemon.Common.Reactive;

namespace Horizon.SmartHome.Automations
{
    public class CeilingLight : NetDaemonRxApp
    {
        private bool _lightEnabledByAutomation;

        public override Task InitializeAsync()
        {
            Entity("binary_sensor.000915699a421a_motion")
                .StateChanges
                .Where(c => c.New.State == "on")
                .Subscribe(_ => EnableCeilingLightWhenAmbientLightIsNotEnabled());

            Entity("binary_sensor.000915699a421a_motion")
                .StateChanges
                .Where(c => c.New.State == "off")
                .Subscribe(_ => DisableCelingLightIfRequired());

            return base.InitializeAsync();
        }

        private void DisableCelingLightIfRequired()
        {
            if (!_lightEnabledByAutomation) 
            {
                return;
            }

            Entity("light.retro_licht").TurnOff();
            
            _lightEnabledByAutomation = false;
        }

        private void EnableCeilingLightWhenAmbientLightIsNotEnabled()
        {
            var ambientLightState = State("light.blumentopf")?.State as string;

            var ceilingLightState = State("light.retro_licht")?.State as string;

            var illumination = (double) State("sensor.000915699a421a_illumination")?.State;
            
            if (ambientLightState == "on" || ceilingLightState == "on" || illumination > 15) 
            {
                Log($"Skipped, because {illumination} > 15");
                return;
            }

            Entity("light.retro_licht").TurnOn(new { brightness = 127 });

            _lightEnabledByAutomation = true;
        }
    }
}