namespace Horizon.SmartHome.Common.Homematic
{
    public interface IFluentHomematicChannelEventState
    {
        IFluentHomematicEventState PressedShort();
        
        IFluentHomematicEventState PressedLong();    
    }
}