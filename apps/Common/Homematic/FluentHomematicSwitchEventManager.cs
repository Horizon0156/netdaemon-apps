using System;
using System.Threading.Tasks;
using JoySoftware.HomeAssistant.NetDaemon.Common;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Extensions.Logging;

namespace Horizon.SmartHome.Common.Homematic
{
    public partial class FluentHomematicSwitchEventManager : 
        FluentEventManager, 
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
        
        private readonly INetDaemon _daemon;

        public FluentHomematicSwitchEventManager(string deviceId, INetDaemon daemon) : base(new[] { "homematic.keypress" }, daemon)
        {
            _daemon = daemon;
            _deviceIdFilter = deviceId;
        }

        public IStateEntity UseEntities(params string[] entityId)
        {
            _currentEntities = _daemon.Entity(entityId);
            
            return this;
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
            if (_currentAction != null)
            {
                // As the current action is managed by the Entity Manager,
                // we use the proper instance to persist the action. 
                var entityManager = _currentAction as EntityManager;

                Call(async () => await entityManager!.ExecuteAsync(true));
            }

            Execute();
        }
    }
}