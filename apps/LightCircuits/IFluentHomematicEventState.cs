using System;
using System.Threading.Tasks;
using JoySoftware.HomeAssistant.NetDaemon.Common;

namespace Horizon.SmartHome.Circuits
{
    public interface IFluentHomematicEventState 
    {
        IExecute Call(Func<Task> callback);

        IStateEntity UseEntities(params string[] entityId);
    }
}