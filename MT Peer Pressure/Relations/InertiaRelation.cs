using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MT_Peer_Pressure;

namespace MT_Peer_Pressure.Relations
{
	class InertiaRelation : Relation
	{
//		public enum InertiaModes {millisecond};
//		private int MaxModes = 1;
		private int mode;
		private double Mode
		{
			get
			{
				return (mode / (double)Enum.GetNames(PeerPressure.mode.GetType()).Count());
			}
			set
			{
				mode = (int)(value * Enum.GetNames(PeerPressure.mode.GetType()).Count());
				pptime.mode = (TimeModes)mode;
				if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Mode"));
			}
		}

		private double time;
		public double Time
		{
			get
			{
				return time;
			}
			set
			{
				time = value;
			}
		}

		private double average;
		private DateTime last;
		private PPTime pptime;

		public InertiaRelation()
		{
			time = 1000;
			Parameters.Add(Time);
			mode = 1;
			Parameters.Add(Mode);
			average = 0.5;
			last = DateTime.Now;
			pptime = new PPTime();
			pptime.mode = (TimeModes)mode;
		}

		override public double Process(double din)
		{
			//int diff = (DateTime.Now - last).Milliseconds;
			int diff = pptime.current();
			average = ((average * (time - diff)) + (din * diff)) / time;//(average / (time - diff.Millisecond)) + (din / time
			return average;
		}

		public override object Clone()
		{
			InertiaRelation clone = new InertiaRelation();
			return (object)clone;
		}

		new public event PropertyChangedEventHandler PropertyChanged;
	}
}
