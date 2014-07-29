using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using BuzzGUI.Interfaces;
using Buzz.MachineInterface;


namespace MT_Peer_Pressure.Effectors
{
	public class SliderEffector : Effector, INotifyPropertyChanged
	{
		int track;
		IBuzzMachine OM;
		IParameter sliderParam;

		public SliderEffector(IBuzzMachine source, IParameter sp, int t)
		{
			OM = source;
			sliderParam = sp;
			track = t;

			isInitialised = true;
		}

		override public double poll()
		{
			return (double)sliderParam.GetValue(track) / (double)sliderParam.MaxValue;
		}

		public override object Clone()
		{
			SliderEffector clone = new SliderEffector(OM, sliderParam, track);
			return (object)clone;
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
