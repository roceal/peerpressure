using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MT_Peer_Pressure.Relations
{
	public class FrequencyRelation : Relation, INotifyPropertyChanged
	{
		public enum LFOModes { Hz };
		private DateTime last;
		private double lastValue;
		private int MaxModes = 1;
		private int mode;
		public double Mode
		{
			get
			{
				return (mode / (double)MaxModes);
			}
			set
			{
				mode = (int)(value * MaxModes);
				if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Mode"));
			}
		}

		private double speed;
		public double Speed
		{
			get
			{
				return speed;
			}
			set
			{
				speed = value;
				if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Speed"));
			}
		}

		public FrequencyRelation()
		{
			lastValue = 0;
			mode = 0;
			speed = 20.0;
			Parameters.Add(Mode);
			Parameters.Add(Speed);
		}

		public override object Clone()
		{
			FrequencyRelation clone = new FrequencyRelation();
			return (object)clone;
		}

		override public double Process(double din)
		{
			switch (mode)
			{
				case (int)LFOModes.Hz:
					{
						// 1hz
						if ((DateTime.Now - last).Milliseconds > (1.0 / speed))
						{
							last = DateTime.Now;
							lastValue = din;
						}
						break;
					}
			}
			return lastValue;
		}
		new public event PropertyChangedEventHandler PropertyChanged;
	}
}
