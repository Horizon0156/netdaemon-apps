namespace Horizon.SmartHome.Common.Homematic
{
    public interface IFluentHomematicSwitchEvent
    {
        IFluentHomematicChannelEventState Channel1();

        IFluentHomematicChannelEventState Channel2();
    }
}