using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT_Peer_Pressure.Relations
{
	class CropRelation : Relation
	{
		private double min;
		public double Min
		{
			get
			{
				return min;
			}
			set
			{
				min = value;
			}
		}
		
		private double max;
		public double Max
		{
			get
			{
				return max;
			}
			set
			{
				max = value;
			}
		}

		public CropRelation()
		{
			min = 0;
			max = 1;
			Parameters.Add(Min);
			Parameters.Add(Max);
		}

		override public object Clone()
		{
			CropRelation cr = new CropRelation();
			return (object)cr;
		}

		override public double Process(double din)
		{
			if (din > max)
				return max;
			else if (din < min)
				return min;
			else
				return din;
		}
	}
}
