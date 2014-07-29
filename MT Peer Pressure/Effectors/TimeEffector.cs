using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT_Peer_Pressure.Effectors
{
	class TimeEffector : Effector
	{
		private int mode;
		public double Mode
		{
			get { return mode / (mode / (double)Enum.GetNames(PeerPressure.mode.GetType()).Count()); }
			set {
				mode = (int)(value * Enum.GetNames(PeerPressure.mode.GetType()).Count());
				time.mode = (TimeModes)mode;
			}
		}

		PPTime time;
		public TimeEffector()
		{
			time = new PPTime();
		}

		override public double poll()
		{
			return time.current();
		}

		public override object Clone()
		{
			TimeEffector clone = new TimeEffector();
			return (object)clone;
		}
	}
}
