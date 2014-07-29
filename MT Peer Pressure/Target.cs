using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BuzzGUI.Interfaces;
using BuzzGUI.Common;
using BuzzGUI.Common.InterfaceExtensions;

namespace MT_Peer_Pressure
{
	public class PPTarget : INotifyPropertyChanged, ICloneable
	{
		private ITargetGUI gui;
		public ITargetGUI Gui
		{
			get
			{
				return gui;
			}
			set
			{
				gui = value;
				if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Gui"));
			}
		}

		private ITargetState state;
		public ITargetState State 
		{
			get
			{
				return state;
			}
			set
			{
				state = value;
			}
		}

		virtual public object Clone()
		{
			return null;
		}

		virtual public UserControl Display()
		{
			return null;
		}

		virtual public Object Target { get; set; }

		public PPTarget()
		{
		}

		virtual public void Update(double dout)
		{
		}

		public override string ToString()
		{
			return base.ToString();
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}

	public interface ITargetGUI  
	{
		//abstract public enum Types { };

//		private bool choosing = false;
		bool isShowingList { get; set; }
		PPTarget caller { get; set; }
		UserControl AsUserControl();
/*		{
			get
			{
				return choosing;
			}
			set
			{
				choosing = value;
				if (choosing == true)
				{
					// show the choice of machines
				}
			}
		}*/
	//	public event PropertyChangedEventHandler PropertyChanged;
	}

	public interface ITargetState
	{
		PPTarget toPPTarget();
	}

}
