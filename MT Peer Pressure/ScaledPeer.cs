using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuzzGUI.Interfaces;
using BuzzGUI.Common;
using BuzzGUI.Common.InterfaceExtensions;

namespace MT_Peer_Pressure
{
    class ScaledPeer : LimitedPeer
    {
        public void Update(double x)
        {
            if (target == null) return;

            // scale within limits
            x = (Max - Min) + Min;

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
