using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT_Peer_Pressure.Effectors
{
	class AudioEffector : Effector
	{
		List<double> buffer;
		double current;

		public AudioEffector()
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
			AudioEffector clone = new AudioEffector();
			return (object)clone;
		}
	}
}
