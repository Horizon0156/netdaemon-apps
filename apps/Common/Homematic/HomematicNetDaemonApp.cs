
using Horizon.SmartHome.Circuits;
using JoySoftware.HomeAssistant.NetDaemon.Common;

namespace Horizon.SmartHome.Common.Homematic
{
    public class HomematicNetDaemonApp : NetDaemonApp
    {
        public IFluentHomematicSwitchEvent HomematicSwitchEvent(string entityId)
        {
            return new FluentHomematicSwitchEventManager(entityId, this);
        }
    }
}
