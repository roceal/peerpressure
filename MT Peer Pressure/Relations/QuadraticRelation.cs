using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT_Peer_Pressure.Relations
{
	class QuadraticRelation : Relation
	{
		public QuadraticRelation()
		{
		}

		override public object Clone()
		{
			QuadraticRelation qr = new QuadraticRelation();
			return (object)qr;
		}

		override public double Process(double din)
		{
			return (0.5 *((din*din) + din));
		}
	}
}
