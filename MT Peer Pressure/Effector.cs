using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using BuzzGUI.Interfaces;
using BuzzGUI.Common;

namespace MT_Peer_Pressure
{

	abstract public class Effector : INotifyPropertyChanged, ICloneable
	{
		// An effector is a way of getting input for the peer relation, and must
 		// be able to output a double from 0..1 at any time once initialised. 
		// An example would be, a machine with a number of parameters setting 
		// by pattern the level of a long int. It's Effector should out put
		// a double from 0..1 by dividing the parameter / 65536. If the effector
		// has parameters it can implement the showParameters method to open a
		// panel.

		public enum EffectorTypes { LFO };

		public Effector()
		{
		}

		abstract public object Clone();

		// set this to true when the effector is ready to output
		public bool isInitialised = false;

		// return the current input
		abstract public double poll();

		// open a panel to allow the user to mess with the parameters
		//virtual void showParameters();

		// allow PP to get and set the effector's parameters
		private List<double> parameters = new List<double>();

		public List<double> Parameters
		{
			get
			{
				return parameters;
			}
			set
			{
				parameters = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Parameters"));

			}


		}

		public event PropertyChangedEventHandler PropertyChanged;
	}

}
