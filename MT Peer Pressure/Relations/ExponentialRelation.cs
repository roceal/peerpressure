using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT_Peer_Pressure.Relations
{
	class ExponentialRelation : Relation
	{
		public ExponentialRelation()
		{
		}

		override public object Clone()
		{
			ExponentialRelation er = new ExponentialRelation();
			return (object)er;
		}

		override public double Process(double din)
		{
			return (Math.Exp(din) - 1);
		}
	}
}
