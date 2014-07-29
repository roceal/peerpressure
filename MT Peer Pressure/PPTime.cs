using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuzzGUI.Common;
using BuzzGUI.Interfaces;
using Buzz.MachineInterface;

namespace MT_Peer_Pressure
{
	public enum TimeModes { sample, subtick, tick, millisecond, second, minute, hour };
	public class PPTime
	{
		DateTime start;
		SongTime songtime;

		public PPTime()
		{
			// get start time
			start = DateTime.Now;
			songtime = new SongTime();
		}

		public void reset()
		{
			// reset start time
			start = DateTime.Now;
			songtime = new SongTime();
		}

		public int current()
		{
			switch (mode)
			{
				case TimeModes.sample :
					return (DateTime.Now - start).Milliseconds * songtime.SamplesPerSec / 1000;
				case TimeModes.subtick :
					return (songtime.CurrentTick * songtime.SubTicksPerTick) + songtime.CurrentSubTick;
				case TimeModes.tick:
					return songtime.CurrentTick;
				case TimeModes.millisecond:
					return (DateTime.Now - start).Milliseconds;
				case TimeModes.second:
					return (DateTime.Now - start).Seconds;
				case TimeModes.minute:
					return (DateTime.Now - start).Minutes;
				case TimeModes.hour:
					return (DateTime.Now - start).Hours;
				default:
					return 0;
			}
		}

		public TimeModes mode { get; set; }
	}
}
