using System;
using System.Threading.Tasks;
using JoySoftware.HomeAssistant.NetDaemon.Common;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Extensions.Logging;

namespace Horizon.SmartHome.Common.Homematic
{
    public partial class FluentHomematicSwitchEventManager :
        IFluentHomematicSwitchEvent, 
        IFluentHomematicChannelEventState,
        IFluentHomematicEventState,
        IStateEntity,
        IStateAction
    {
        private int? _channelFilter;

        private KeyPressType? _keyPressTypeFilter;

        private IEntity? _currentEntities;
        
        private IAction? _currentAction;

        private readonly string _deviceIdFilter;

        private readonly INetDaemonApp _daemonApp;

        private readonly ILogger? _logger;

        public FluentHomematicSwitchEventManager(
            string deviceId,
            INetDaemonApp daemonApp,
            ILogger? logger)
        {
            _daemonApp = daemonApp;
            _logger = logger;
            _deviceIdFilter = deviceId;
        }

        public IStateEntity UseEntities(params string[] entityId)
        {
            _currentEntities = _daemonApp.Entity(entityId);
            
            return this;
        }

        public IExecute Call(Func<Task> callback)
        {
            return _daemonApp.Event("homematic.keypress")
                             .Call((eventType, eventData) => ProcessHomematicKeypressEvent(eventData, callback));
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

        public IStateAction TurnOff()
        {
            _currentAction = _currentEntities?.TurnOff();

            return this;
        }

        public IStateAction TurnOn()
        {
            _currentAction = _currentEntities?.TurnOn();

            return this;
        }

        public IStateAction Toggle()
        {
            _currentAction = _currentEntities?.Toggle();

            return this;
        }

        public IStateAction SetState(dynamic state)
        {
            _currentAction = _currentEntities?.SetState(state);

            return this;
        }

        public IStateAction WithAttribute(string name, object value)
        {
            _currentAction?.WithAttribute(name, value);

            return this;
        }

        void IExecute.Execute()
        {
            Call(ExecuteConfiguredAction).Execute();
        }

        private async Task ExecuteConfiguredAction()
        {
            if (_currentAction != null)
            {
                // As the current action is managed by the Entity Manager,
                // we use the proper instance to persist the action. 
                var entityManager = _currentAction as EntityManager;
                await entityManager!.ExecuteAsync(true);
            }
        }

        private async Task ProcessHomematicKeypressEvent(dynamic? eventData, Func<Task> callback)
        {
            if (eventData == null)
            {
                _logger?.Log(LogLevel.Warning, "Ignored homematic keypress event due to mising event data.");
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
                _logger?.LogWarning(e, $"  {{Id}}: Ignored homematic keypress event due to invalid event data.");
            }
            catch (ArgumentException e)
            {
                _logger?.LogWarning(e, $"  {{Id}}: Ignored homematic keypress event due to invalid event data.");
            }
        }
    }
}