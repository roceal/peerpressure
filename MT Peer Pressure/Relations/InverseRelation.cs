using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT_Peer_Pressure.Relations
{
	class InverseRelation : Relation
	{
		public InverseRelation()
		{
		}

		override public object Clone()
		{
			InverseRelation ir = new InverseRelation();
			return (object)ir;
		}

		override public double Process(double din)
		{
			return (1 - din);
		}
	}
}
