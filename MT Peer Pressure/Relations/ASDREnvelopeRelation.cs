using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Buzz.MachineInterface;
using BuzzGUI.Common;
using BuzzGUI.Common.InterfaceExtensions;

namespace MT_Peer_Pressure.Relations
{
	class ASDREnvelopeRelation : Relation
	{
		IBuzzMachineHost mi;

		// evelope variables
		private bool on;
		private float position;
		private int part;

		// time variables
		PPTime time;
		private int sample;
		private int subtick;
		private int tick;
		private DateTime start;

		private int MaxModes = Enum.GetValues(typeof(TimeModes)).Length;
		private int mode;
		private double Mode
		{
			get
			{
				return (mode / (double)MaxModes);
			}
			set
			{
				mode = (int)(value * MaxModes);
				time.mode = (TimeModes)mode;
				// update the time to now TODO: move into action or check in process whether to call
				if (mode >= (int)TimeModes.millisecond)
				{
					start = DateTime.Now;
				}
				else
				{
					// update the start position
					if (mode == (int)TimeModes.sample)
					{
						sample = Global.Buzz.Song.PlayPosition;
					}
					else if (mode == (int)TimeModes.subtick)
					{
						subtick = mi.SubTickInfo.CurrentSubTick + (Global.Buzz.Song.PlayPosition / mi.SubTickInfo.SamplesPerSubTick);
					}
					else if (mode == (int)TimeModes.tick)
					{
						tick = (Global.Buzz.Song.PlayPosition / mi.MasterInfo.SamplesPerTick);
					}
				}

				// reset envelope
				part = 0;
				position = 0;
				
//				if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Mode"));
			}
		}

		private Int16 attack;
		public Int16 Attack
		{
			get
			{
				return attack;
			}
			set
			{
				attack = value;
			}
		}

		private Int16 decay;
		public Int16 Decay
		{
			get
			{
				return decay;
			}
			set
			{
				decay = value;
			}
		}

		private Int16 sustain;
		public Int16 Sustain
		{
			get
			{
				return sustain;
			}
			set
			{
				sustain = value;
			}
		}

		private Int16 release;
		public Int16 Release
		{
			get
			{
				return release;
			}
			set
			{
				release = value;
			}
		}

		public ASDREnvelopeRelation(IBuzzMachineHost ibmh)
		{
			mi = ibmh;
			attack = 100;
			decay = 100;
			sustain = 100;
			release = 100;
			mode = (int)TimeModes.millisecond;
			Parameters.Add(Mode);
			Parameters.Add(Attack);
			Parameters.Add(Decay);
			Parameters.Add(Sustain);
			Parameters.Add(Release);
		}

		override public object Clone()
		{
			ASDREnvelopeRelation cr = new ASDREnvelopeRelation(mi);
			return (object)cr;
		}

		private int currentTimeLimit()
		{
			if (part == 1) return attack;
			if (part == 2) return decay;
			if (part == 4) return release;
			else return 0;
		}

		private void advance()
		{
			switch (mode)
			{
				case (int)TimeModes.sample:
					position = (Global.Buzz.Song.PlayPosition - sample) / currentTimeLimit();
					break;
				case (int)TimeModes.subtick:
					position = (mi.SubTickInfo.CurrentSubTick + (Global.Buzz.Song.PlayPosition / mi.SubTickInfo.SamplesPerSubTick) - subtick) / currentTimeLimit();
					break;
				case (int)TimeModes.tick:
					position = ((Global.Buzz.Song.PlayPosition / mi.MasterInfo.SamplesPerTick)) / currentTimeLimit();
					break;
				case (int)TimeModes.millisecond:
					position = (DateTime.Now.Millisecond - start.Millisecond) / currentTimeLimit();
					break;
				case (int)TimeModes.second:
					position = (DateTime.Now.Second - start.Second) / currentTimeLimit();
					break;
				case (int)TimeModes.minute:
					position = (DateTime.Now.Minute - start.Minute) / currentTimeLimit();
					break;
				case (int)TimeModes.hour:
					position = (DateTime.Now.Hour - start.Hour) / currentTimeLimit();
					break;
			}
		}

		override public double Process(double din)
		{
			double dout = 0;

			if (din == 1) on = true;
			else { on = false; if (part > 0) part = 4; }

			// if we've reached position 1, advance part
			if (position >= 1) { part++; position = 0; }
			
			if (on == true)
			{
				// if we were off, start the envelope
				if (part == 0) { part = 1; position = 0; }

				// if we're in attack or decay, continue
				// if we're in sustain, return
				switch (part)
				{
					case (1): // attack
						dout = position;
						break;
					case (2): // decay 
						// o = (1-p) * (1 - sustain) + sustain
						dout = ((1 - position) * (1 - (sustain / (double)Int16.MaxValue))) + (sustain / (double)Int16.MaxValue);
						break;
					case (3): // sustain
						dout = sustain / (double)Int16.MaxValue;
						break;
					default:
						// we have an error, kill the envelope
						part = 0;
						dout = 0;
						break;
				}
			}

			if (part == 4)
			{
				// we are releasing
				// if we've reached position 1, reset part, return 0
				if (position >= 1) { part = 0; position = 0; dout = 0;}
				// else continue releasing o = 1 * (1-sustain) + sustain
				return (position * (1 - (sustain / (double)Int16.MaxValue))) + (sustain / (double)Int16.MaxValue);
			}

			if ((part == 1) || (part == 2) || (part == 4)) advance();

			return dout;
		}
	}
}
