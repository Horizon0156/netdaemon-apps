
using Horizon.SmartHome.Circuits;
using JoySoftware.HomeAssistant.NetDaemon.Common;

namespace Horizon.SmartHome.Common
{
    public class HomematicNetDaemonApp : NetDaemonApp
    {
        public IFluentHomematicSwitchEvent HomematicSwitchEvent(string entityId)
        {
            return new FluentHomematicSwitchEventManager(entityId, this);
        }
    }
}
