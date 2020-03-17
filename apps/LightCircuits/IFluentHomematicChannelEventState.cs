namespace Horizon.SmartHome.Circuits
{
    public interface IFluentHomematicChannelEventState
    {
        IFluentHomematicEventState PressedShort();
        
        IFluentHomematicEventState PressedLong();    
    }
}