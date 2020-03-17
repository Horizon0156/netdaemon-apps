using System;
using System.Threading.Tasks;
using JoySoftware.HomeAssistant.NetDaemon.Common;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Extensions.Logging;

namespace Horizon.SmartHome.Circuits
{
    public partial class FluentHomematicSwitchEventManager : FluentEventManager, IFluentHomematicSwitchEvent, IFluentHomematicChannelEventState, IFluentHomematicEventState
    {
        private int? _channelFilter;

        private KeyPressType? _keyPressTypeFilter;

        private readonly string _deviceIdFilter;
        
        private readonly INetDaemon _daemon;

        public FluentHomematicSwitchEventManager(string deviceId, INetDaemon daemon) : base(new[] { "homematic.keypress" }, daemon)
        {
            _daemon = daemon;
            _deviceIdFilter = deviceId;
        }

        public IExecute Call(Func<Task> callback)
        {
            return base.Call((eventType, eventData) => ProcessHomematicKeypressEvent(eventData, callback));
        }

        public IFluentHomematicEventState PressedLong()
        {
            _keyPressTypeFilter = KeyPressType.PRESS_LONG;

            return this;
        }

        public IFluentHomematicEventState PressedShort()
        {
            _keyPressTypeFilter = KeyPressType.PRESS_SHORT;

            return this;
        }

        public IFluentHomematicChannelEventState Channel1()
        {
            _channelFilter = 1;

            return this;
        }

        public IFluentHomematicChannelEventState Channel2()
        {
            _channelFilter = 2;

            return this;
        }


        private async Task ProcessHomematicKeypressEvent(dynamic? eventData, Func<Task> callback)
        {
            if (eventData == null)
            {
                _daemon.Logger?.Log(LogLevel.Warning, "Ignored homematic keypress event due to mising event data.");
                return;
            }

            try
            {
                var deviceId = eventData.name;
                var channel = (int)eventData.channel;
                var keyPressType = Enum.Parse(typeof(KeyPressType), eventData.param);

                if (deviceId == _deviceIdFilter && channel == _channelFilter && keyPressType == _keyPressTypeFilter)
                {
                    await callback.Invoke();
                }
            }
            catch (RuntimeBinderException e)
            {
                _daemon.Logger?.Log(LogLevel.Warning, e, "Ignored homematic keypress event due to invalid event data.");
            }
            catch (ArgumentException e)
            {
                _daemon.Logger?.Log(LogLevel.Warning, e, "Ignored homematic keypress event due to invalid event data.");
            }
        }
    }
}