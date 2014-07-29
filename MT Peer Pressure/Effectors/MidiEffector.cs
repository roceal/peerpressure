using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuzzGUI.Interfaces;
using BuzzGUI.Common;

namespace MT_Peer_Pressure.Effectors
{
	public class MidiEffector : Effector
	{
		int channel; // 0-15 = channel, 16 = all
		int cc; // 0-127, 128 = note?
		int midi;
		double current;

		void getMidi(int midiIn)
		{
			midi = midiIn;
		}

		public MidiEffector(int ccIn, int channelIn)
		{
			cc = ccIn;
			channel = channelIn;
			Global.Buzz.MIDIInput += getMidi;
			current = 0.5;
			isInitialised = true;
		}

		override public double poll()
		{
			int status = MIDI.DecodeStatus(midi);
			if (cc <= 127)
			{
				// we are looking for a cc
				if (channel == 16)
				{
					// all channels, cc
					if ((status & 0xf0) == MIDI.ControlChange)
					{
						// we have a cc
						if(MIDI.DecodeData1(midi) == cc) {
							current = MIDI.DecodeData2(midi) / 127.0;
						}
					}
				}
				else
				{
					// single channel, cc
					if ((status & 0xf0) == MIDI.ControlChange)
					{
						// if we're in the right channel
						if ((status - MIDI.ControlChange) == channel)
						{
							// we have a cc
							if (MIDI.DecodeData1(midi) == cc)
							{
								current = MIDI.DecodeData2(midi) / 127.0;
							}
						}
					}
				}
			}
			else
			{
				// we are looking for a note
				if (channel == 16)
				{
					// all channels, note
					if ((status & 0xf0) == MIDI.NoteOn)
					{
						current = MIDI.DecodeData1(midi) / 127.0;
					}
				}
				else
				{
					// single channel, note
					if ((status & 0xf0) == MIDI.NoteOn)
					{
						// we have the right channel
						if ((status & 0x0F) == channel)
						{
							current = MIDI.DecodeData1(midi) / 127.0;
						}
					}
				}
			}
			return current;
		}

		public override object Clone()
		{
			MidiEffector clone = new MidiEffector(cc, channel);
			return (object)clone;
		}
	}
}
