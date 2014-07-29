using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT_Peer_Pressure.Effectors
{
	class NullEffector : Effector
	{
		public NullEffector()
		{
			isInitialised = true;
		}

		override public double poll()
		{
			return 0.5;
		}

		public override object Clone()
		{
			NullEffector clone = new NullEffector();
			return (object)clone;
		}
	}
}
