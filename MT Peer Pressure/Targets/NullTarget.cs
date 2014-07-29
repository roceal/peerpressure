using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT_Peer_Pressure.Targets
{
	public class NullTarget : PPTarget
	{
		public NullTarget()
		{
			Gui = null;
		}

		public override object Clone()
		{
			NullTarget clone = new NullTarget();
			return (object)clone;
		}

		override public void Update(double dout)
		{
		}

		public override string ToString()
		{
			return "NULL";
		}
	}
}
