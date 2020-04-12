
using JoySoftware.HomeAssistant.NetDaemon.Common;

namespace Horizon.SmartHome.Common.Homematic
{
    public abstract class HomematicNetDaemonApp : NetDaemonApp
    {
        public IFluentHomematicSwitchEvent HomematicSwitchEvent(string entityId)
        {
            return new FluentHomematicSwitchEventManager(entityId, this, Logger);
        }
    }
}
