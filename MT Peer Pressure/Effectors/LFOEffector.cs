using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using BuzzGUI.Interfaces;

namespace MT_Peer_Pressure.Effectors
{
	public class LFOEffector : Effector, INotifyPropertyChanged
	{
		public enum LFOModes { Hz, Tick, Tick256, SubTick, Millisecond, Second };
		public enum LFOShapes { Sine, SawDown, SawUp, Square };

		private double position = 0;
		private int mode = 0; // 0 = hz
		public int MaxModes = 6;
		private int shape = 0; // 0 = sine
		public int MaxShapes = 4;
		private double speed = 1; // 1 = 1hz
		private double depth = 0.5; // 0.5 = 50%
		private double centre = 0.5;
		private DateTime start;

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
		public double Shape
		{
			get
			{
				return (shape / (double)MaxShapes);
			}
			set
			{
				shape = (int)(value * MaxShapes);
				if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Shape"));
			}
		}
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
		public double Depth
		{
			get
			{
				return depth;
			}
			set
			{
				depth = value;
				if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Depth"));
			}
		}

		public double Centre
		{
			get
			{
				return centre;
			}
			set
			{
				centre = value;
				if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Centre"));
			}
		}

		public LFOEffector()
		{
			Parameters.Add(Mode);
			Parameters.Add(Shape);
			Parameters.Add(Speed);
			Parameters.Add(Depth);
			Parameters.Add(Centre);
			start = DateTime.Now;
			isInitialised = true;
		}

		public override object Clone()
		{
			LFOEffector clone = new LFOEffector();
			return (object)clone;
		}

		override public double poll()
		{
			// checks
			int msFromStart = (DateTime.Now - start).Milliseconds;
			SongTime now = new SongTime();

			switch (mode)
			{
				case (int)LFOModes.Hz:
					{
						position = (msFromStart / (speed * 1000));
						break;
					}
				case (int)LFOModes.Tick:
					{
						position = (now.CurrentTick / speed);
						break;
					}
				case (int)LFOModes.Tick256:
					{
						position = (now.CurrentTick / 256.0) / speed;
						break;
					}
				case (int)LFOModes.SubTick:
					{
						position = ((now.CurrentTick * now.CurrentSubTick) + now.CurrentSubTick) / speed;
						break;
					}
				case (int)LFOModes.Millisecond:
					{
						position = msFromStart / speed;
						break;
					}
				case (int)LFOModes.Second:
					{
						position = (msFromStart / 1000) / speed;
						break;
					}
				default: position = 0;
					break;
			}
	
			switch (shape)
			{
				case (int)LFOShapes.Sine:
					{
						return ((Math.Sin(position * 2 * Math.PI) * depth) / 2) + centre;
					}
				case (int)LFOShapes.SawDown:
					{
						return ((1 - (position - Math.Truncate(position))) * depth / 2) + centre;
					}
				case (int)LFOShapes.SawUp:
					{
						return ((position - ((int)position)) * depth / 2) + centre;
					}
				case (int)LFOShapes.Square:
					{
						if ((position - ((int)position)) > 0.5) return 1.0;
						else return 0;
					}
			}
			// else (throw an exception for problems?)
			return 0;
		}

		new public event PropertyChangedEventHandler PropertyChanged;
	}
}
