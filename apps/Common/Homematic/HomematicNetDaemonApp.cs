
using System;
using System.Reactive.Linq;
using JoySoftware.HomeAssistant.NetDaemon.Common.Reactive;

namespace Horizon.SmartHome.Common.Homematic
{
    public abstract class HomematicNetDaemonRxApp : NetDaemonRxApp
    {
        public IObservable<HomematicKeypressEvent> KeyPressEvents => MapKeypressEvents();

        private IObservable<HomematicKeypressEvent> MapKeypressEvents()
        {
            return EventChanges.Where(@event => @event.Event == "homematic.keypress")
                               .Select(@event => new HomematicKeypressEvent(
                                   @event.Data?.name, 
                                   (int) @event.Data?.channel, 
                                   @event.Data?.param));
        }
    }
}
