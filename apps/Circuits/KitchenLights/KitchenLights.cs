using System.Threading.Tasks;
using Horizon.SmartHome.Common.Homematic;

namespace Horizon.SmartHome.Circuits
{
    public class KitchenLights : HomematicNetDaemonApp
    {
        public override Task InitializeAsync()
        {
            HomematicSwitchEvent("000858A99D84DE")
                .Channel1()
                .PressedShort()
                .UseEntities("light.kuche")
                .TurnOn()
                .WithAttribute("brightness", 127)
                .Execute();

            HomematicSwitchEvent("000858A99D84DE")
                .Channel1()
                .PressedLong()
                .UseEntities("light.kuche")
                .TurnOn()
                .WithAttribute("brightness", 255)
                .Execute();

            HomematicSwitchEvent("000858A99D84DE")
                .Channel2()
                .PressedShort()
                .UseEntities("light.kuche")
                .TurnOff()
                .Execute();

            return Task.CompletedTask;
        }
    }
}