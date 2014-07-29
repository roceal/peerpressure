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

namespace MT_Peer_Pressure.Targets
{
	public partial class MachineTargetGUI : UserControl, ITargetGUI, INotifyPropertyChanged
	{
		public enum Types { All, Normal, Generators, Effects };
		// A textbox showing name of target, a button to show the get panel, close get panel on selection

		public bool isShowingList { get; set; }
		public PPTarget caller { get; set; }
		Types mode;

		IMachine target;
		public IMachine Target
		{
			get
			{
				return target;
			}
			set
			{
				target = value;
				if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Target"));
			}
		}

		public UserControl AsUserControl()
		{
			return this;
		}

		private void fillTreeList()
		{
		}

		public MachineTargetGUI(PPTarget c, IMachine t, Types m)
		{
			InitializeComponent();
			DataContext = this;
			caller = c;
			Target = t;
			mode = m;
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
