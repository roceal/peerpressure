using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT_Peer_Pressure.Effectors
{
	class OSCEffector : Effector
	{
		double current;

		public OSCEffector()
		{
			current = 0.5;
			isInitialised = true;
		}

		override public double poll()
		{
			return current;
		}

		public override object Clone()
		{
			OSCEffector clone = new OSCEffector();
			return (object)clone;
		}
	}
}
