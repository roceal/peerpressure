using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuzzGUI.Interfaces;
using BuzzGUI.Common;
using BuzzGUI.Common.InterfaceExtensions;

namespace MT_Peer_Pressure
{
    abstract public class External
    {
        public int val;
        public int Value { get { Update(3); return val; } set { val = value; } }

        public External()
        {
        }
    
        public abstract void Update(double d);
//        {
//            throw new System.NotImplementedException();
//        }
    }
}
