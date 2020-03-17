using JoySoftware.HomeAssistant.NetDaemon.Common;

namespace Horizon.SmartHome.Circuits
{
    public static class NetDaemonExtensions
    {
        public static IFluentHomematicSwitchEvent HomematicSwitchEvent(this NetDaemonApp daemon, string entityId)
        {
            return new FluentHomematicSwitchEventManager(entityId, daemon);
        }
    }
}