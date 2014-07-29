using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using MT_Peer_Pressure;

namespace MT_Peer_Pressure.Targets
{
	class MultiTarget : PPTarget
	{
		private List<PPTarget> targets;
		public override object Target
		{
			get
			{
				return (Object)targets;
			}
			set
			{
				targets = (List<PPTarget>)value;
			}
		}

		public MultiTarget()
		{
			targets = new List<PPTarget>();
		}

		public override object Clone()
		{
			MultiTarget clone = new MultiTarget();
			return (object)clone;
		}

		override public void Update(double dout)
		{
			foreach (PPTarget ppt in targets)
				ppt.Update(dout);
		}
	}
}
