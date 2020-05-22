using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Horizon.SmartHome.Common.Homematic;

namespace Horizon.SmartHome.Circuits
{
    public class KitchenLights : HomematicNetDaemonRxApp
    {
        public override Task InitializeAsync()
        {
            KeyPressEvents.Where(e => e.Address == "000858A99D84DE" 
                                   && e.Channel == 1 
                                   && e.Action == KeyPressAction.PressShort)
                          .Subscribe(_ => CallService("scene", "turn_on", new { entity_id = "scene.kuche_gedimmt"}));
            
            KeyPressEvents.Where(e => e.Address == "000858A99D84DE" 
                                   && e.Channel == 1 
                                   && e.Action == KeyPressAction.PressLong)
                          .Subscribe(_ => CallService("scene", "turn_on", new { entity_id = "scene.kuche_hell"}));

            KeyPressEvents.Where(e => e.Address == "000858A99D84DE" 
                                   && e.Channel == 2
                                   && e.Action == KeyPressAction.PressShort)
                          .Subscribe(_ => Entities("light.tisch_1", "light.tisch_2").TurnOff());

            return base.InitializeAsync();
        }
    }
}