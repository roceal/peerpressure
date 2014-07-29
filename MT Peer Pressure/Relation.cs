using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Media;
using BuzzGUI.Interfaces;
using BuzzGUI.Common;
using BuzzGUI.Common.InterfaceExtensions;

namespace MT_Peer_Pressure
{
	abstract public class Relation : INotifyPropertyChanged, ICloneable
	{
		private List<double> parameters;
		public List<double> Parameters
		{
			get 
			{
				return parameters;
			}
			set
			{
				parameters = value;
			}


		}

		public Relation()
		{
			parameters = new List<double>();
		}

		abstract public object Clone();

		abstract public double Process(double din);

		public event PropertyChangedEventHandler PropertyChanged;
	}




}
