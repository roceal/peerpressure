using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MT_Peer_Pressure.Relations
{
	public class SteppedRelation : Relation, INotifyPropertyChanged
	{
		private double lastValue;
		private double minDelta;
		private int steps;
		public int Steps
		{
			get
			{
				return steps;
			}
			set
			{
				steps = value;
				minDelta = 1.0 / steps;
			}
		}

		public SteppedRelation()
		{
			lastValue = 0;
			Parameters.Add(Steps);
		}
		public override object Clone()
		{
			SteppedRelation clone = new SteppedRelation();
			return (object)clone;
		}

		override public double Process(double din)
		{
			// check we are different enough
			double delta = Math.Abs(lastValue - din);
			if (delta >= minDelta)
			{
				// update
				lastValue = lastValue + ((lastValue - din) % minDelta);
			}
			return lastValue;
		}
		new public event PropertyChangedEventHandler PropertyChanged;
	}
}
