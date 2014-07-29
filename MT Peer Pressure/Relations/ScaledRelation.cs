using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MT_Peer_Pressure.Relations
{
	public class ScaledRelation : Relation, INotifyPropertyChanged
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
				if (max >= value)
				{
					min = value;
				}
				else
				{
					min = max;
					Max = value;
				}
				if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Min"));
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
				if (value >= min)
				{
					max = value;
				}
				else
				{
					max = min;
					Min = value;
				}
				if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Max"));
			}
		}

		public ScaledRelation()
		{
			min = 0;
			max = 1;
			Parameters.Add(Min);
			Parameters.Add(Max);
		}

		public override double Process(double din)
		{
			return (din * (max - min)) + min;
		}

		public override object Clone()
		{
			ScaledRelation clone = new ScaledRelation();
			return (object)clone;
		}
	
		new public event PropertyChangedEventHandler PropertyChanged;
	}
}
