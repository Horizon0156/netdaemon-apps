using System.Linq;
using System.Threading.Tasks;
using JoySoftware.HomeAssistant.NetDaemon.Common;

namespace Horizon.SmartHome.Automations.CeilingLight
{
    public class CeilingLight : NetDaemonApp
    {
        private bool _lightEnabledByAutomation;

        public override Task InitializeAsync()
        {
            Entity("binary_sensor.hmip_smi_000915699a421a_motion")
                .WhenStateChange(from: "off", to: "on")
                .Call(async (_, __, ___) => await EnableCeilingLightWhenAmbientLightIsNotEnabled())
                .Execute();

            Entity("binary_sensor.hmip_smi_000915699a421a_motion")
                .WhenStateChange(from: "on", to: "off")
                .Call(async (_, __, ___) => await DisableCelingLightIfRequired())
                .Execute();

            return base.InitializeAsync();
        }

        private async Task DisableCelingLightIfRequired()
        {
            if (!_lightEnabledByAutomation) 
            {
                return;
            }

            await Entity("light.retro_licht")
                    .TurnOff()
                    .ExecuteAsync();
            
            _lightEnabledByAutomation = false;
        }

        private async Task EnableCeilingLightWhenAmbientLightIsNotEnabled()
        {
            var ambientLightState = 
                State.FirstOrDefault(e => e.EntityId == "light.blumentopf")?.State as string;

            var ceilingLightState = 
                State.FirstOrDefault(e => e.EntityId == "light.retro_licht")?.State as string;

            if (ambientLightState == "on" || ceilingLightState == "on") 
            {
                return;
            }

            await Entity("light.retro_licht")
                    .TurnOn()
                    .WithAttribute("brightness", 127)
                    .ExecuteAsync();
            _lightEnabledByAutomation = true;
        }
    }
}