// Used in place of missing effector modules, contain name of missing eff, param contents. replace when possible with actual effector, save as actual

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT_Peer_Pressure.Effectors
{
    public class ErrorEffector : Effector
    {
        override public double poll()
        {
            return 0.5;
        }
        public override object Clone()
        {
            ErrorEffector clone = new ErrorEffector();
            return (object)clone;
        }
    }
}
