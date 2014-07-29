using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Buzz.MachineInterface;
using BuzzGUI.Interfaces;
using BuzzGUI.Common;
using BuzzGUI.Common.InterfaceExtensions;
using MT_Peer_Pressure;
using MT_Peer_Pressure.Effectors;

namespace mt_LFO
{
	[MachineDecl(Name = "Mantratronic LFO Anywhere", ShortName = "LFOAny", Author = "Ronan O'Ceallaigh", MaxTracks = 10)]
	public class MTLFOMachine : IBuzzMachine, INotifyPropertyChanged
	{
		IBuzzMachineHost host;
		Pressure pressure = null;
		Peer[] peers = new Peer[10];
		int tracks = 1;

		public MTLFOMachine(IBuzzMachineHost host)
		{
			this.host = host;

			// look for PP
			IMachine master = null;

			foreach (IMachine m in Global.Buzz.Song.Machines)
				if (m.DLL.Name == "MT Peer Pressure")
					master = m;

			// if its there set pressure
			if (master != null) pressure = (Pressure)master.ManagedMachine;

			tracks = 0;
			peers[0] = pressure.newPeer(host.Machine);
			peers[0].InputEffector = new LFOEffector();
		}

		private void checkTracks(int t)
		{
			if (t > tracks)
			{
				for (int i = tracks; i < t; i++)
				{
					peers[i] = pressure.newPeer(host.Machine);
					peers[i].InputEffector = new LFOEffector();
					tracks++;
				}
			}
		}

		[ParameterDecl(ValueDescriptions = new[] { "Hz", "Tick", "Tick / 256", "SubTick", "Ms", "Second" }, MaxValue = 5, DefValue = 0)]
		public void Mode(int v, int track)
		{
			checkTracks(track);
			((LFOEffector)peers[track].InputEffector).Mode = (float)v / ((LFOEffector)peers[track].InputEffector).MaxModes;
		}

		public enum LFOShapes { Sine, SawDown, SawUp, Square };
		[ParameterDecl(ValueDescriptions = new[] { "Sine", "Saw Down", "Saw Up", "Square" }, MaxValue = 3, DefValue = 0)]
		public void Shape(int v, int track)
		{
			((LFOEffector)peers[track].InputEffector).Shape = (float)v / ((LFOEffector)peers[track].InputEffector).MaxShapes;
		}

		[ParameterDecl(MaxValue = 65534, DefValue = 1000)]
		public void Speed(int v, int track)
		{
			((LFOEffector)peers[track].InputEffector).Speed = v / 65534.0;
		}

		[ParameterDecl(ValueDescriptor = Descriptors.Percentage, MaxValue = 65534, DefValue = 65534)]
		public void Depth(int v, int track)
		{
			((LFOEffector)peers[track].InputEffector).Depth = v / 65534.0;
		}

		[ParameterDecl(ValueDescriptor = Descriptors.Percentage, MaxValue = 65534, DefValue = 32767)]
		public void Centre(int v, int track)
		{
			((LFOEffector)peers[track].InputEffector).Centre = v / 65534.0;
		}


		int TrackCount { get { return host.Machine.ParameterGroups[(int)ParameterGroupType.Track].TrackCount; } }

		
		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;
		
		#endregion
	}
}
