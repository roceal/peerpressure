using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuzzGUI.Interfaces;
using BuzzGUI.Common;
using BuzzGUI.Common.InterfaceExtensions;

namespace MT_Peer_Pressure
{
    class LimitedPeer : Peer
    {
        public double Min { get; set; }
        public double Max { get; set; }
        
        public void Update(double x) {
            if (target == null) return;

            // check limits
            if (x < Min) x = Min;
            else if (x > Max) x = Max;

            // set the parameter according to what type it is
            switch (target.Type)
            {
                case ParameterType.Note:
                    int note = (int)(x * 120); //(c0-b9)
                    note = note + (4 * (note / 12)); //buzznote
                    target.SetValue(track, note);
                    break;
                case ParameterType.Word:
                    int word = target.MinValue + (int)(x * (target.MaxValue - target.MinValue));
                    target.SetValue(track, word);
                    break;
                case ParameterType.Byte:
                    int byt = target.MinValue + (int)(x * (target.MaxValue - target.MinValue));
                    target.SetValue(track, byt);
                    break;
                case ParameterType.Switch:
                    int switc = (int)(x);
                    target.SetValue(track, switc);
                    break;
            }
        }
    }
}
