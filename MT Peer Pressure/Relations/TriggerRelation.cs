using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT_Peer_Pressure.Relations
{
	class TriggerRelation : Relation
	{
		private double limit;
		public double Limit
		{
			get
			{
				return limit;
			}
			set
			{
				limit = value;
			}
		}
		
		public TriggerRelation()
		{
			limit = 0.5;
			Parameters.Add(Limit);
		}

		override public object Clone()
		{
			TriggerRelation tr = new TriggerRelation();
			return (object)tr;
		}

		override public double Process(double din)
		{
			if (din >= limit)
				return 1;
			else
				return 0;
		}
	}
}
