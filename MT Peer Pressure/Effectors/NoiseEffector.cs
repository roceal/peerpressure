using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MT_Peer_Pressure.Effectors
{
	public class NoiseEffector : Effector, INotifyPropertyChanged
	{
		Random random;
		public NoiseEffector()
		{
			random = new Random();
			isInitialised = true;
		}

		override public double poll()
		{
			return random.NextDouble();
		}

		public override object Clone()
		{
			NoiseEffector clone = new NoiseEffector();
			return (object)clone;
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
