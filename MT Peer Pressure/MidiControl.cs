using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuzzGUI.Interfaces;
using BuzzGUI.Common;
using BuzzGUI.Common.InterfaceExtensions;

namespace MT_Peer_Pressure
{
    public class MidiControl : External
    {
        int cc;
        int channel;

        public MidiControl(int icc, int ichan)
        {
            cc = icc;
            channel = ichan;
        }

        public void Update(double d)
        {
            //val = 
            
        }
    }
}
