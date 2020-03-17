using System.Threading.Tasks;
using Horizon.SmartHome.Common;

namespace Horizon.SmartHome.Circuits
{
    public class LightCircuits : HomematicNetDaemonApp
    {
        public override Task InitializeAsync()
        {
            HomematicSwitchEvent("000858A99D84DE")
                .Channel1()
                .PressedShort()
                .Call(async () => await TurnKitchenLightsOnAsync(127))
                .Execute();

            HomematicSwitchEvent("000858A99D84DE")
                .Channel1()
                .PressedLong()
                .Call(async () => await TurnKitchenLightsOnAsync(255))
                .Execute();

            HomematicSwitchEvent("000858A99D84DE")
                .Channel2()
                .PressedShort()
                .Call(TurnKitchenLightsOffAsync)
                .Execute();

            return Task.CompletedTask;
        }

        private async Task TurnKitchenLightsOnAsync(int brightness)
        {
            await Entity("light.kuche")
                    .TurnOn()
                    .WithAttribute("brightness", brightness)
                    .ExecuteAsync();
        }

        private async Task TurnKitchenLightsOffAsync()
        {
            await Entity("light.kuche")
                    .TurnOff()
                    .ExecuteAsync();
        }
    }
}