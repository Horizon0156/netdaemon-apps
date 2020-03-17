namespace Horizon.SmartHome.Circuits
{
    public interface IFluentHomematicSwitchEvent
    {
        IFluentHomematicChannelEventState Channel1();

        IFluentHomematicChannelEventState Channel2();
    }
}