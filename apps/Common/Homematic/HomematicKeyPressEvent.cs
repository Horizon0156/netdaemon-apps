
using System;

namespace Horizon.SmartHome.Common.Homematic 
{
    public class HomematicKeypressEvent
    {
        public HomematicKeypressEvent(string address, int channel, string action)
        {
            Address = address;
            Channel = channel;

            switch (action)
            {
                case "PRESS_SHORT":
                    Action = KeyPressAction.PressShort;
                    break;
                case "PRESS_LONG":
                    Action = KeyPressAction.PressLong;
                    break;
                default:
                    throw new ArgumentException("Unknown keypress action", nameof(action));
            }
        }

        public string Address {get; }

        public int Channel { get; }

        public KeyPressAction Action { get; }
    }
}