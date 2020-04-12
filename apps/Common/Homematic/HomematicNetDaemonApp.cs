
using System;
using Horizon.SmartHome.Circuits;
using JoySoftware.HomeAssistant.NetDaemon.Common;
using Microsoft.Extensions.Logging;

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
