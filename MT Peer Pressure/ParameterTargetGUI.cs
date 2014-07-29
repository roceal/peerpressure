using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using BuzzGUI.Interfaces;
using BuzzGUI.Common;
using BuzzGUI.Common.InterfaceExtensions;
using MT_Peer_Pressure.Targets;

namespace MT_Peer_Pressure
{
	public partial class ParameterTargetGUI : UserControl, ITargetGUI, INotifyPropertyChanged
	{
		public enum Types { All, Normal };
		// A textbox showing name of target, a button to show the get panel, close get panel on selection

		private bool isChoosing = false;
		public bool isShowingList { get { return isChoosing; } set { isChoosing = this.Choice.IsEnabled = value; } }
		public PPTarget caller { get; set; }
		Types mode;

		Tuple<IParameter, int> target;
		public Tuple<IParameter, int> Target
		{
			get
			{
				return target;
			}
			set
			{
				target = value;
				// send it up the line to PPTarget
				((ParameterTarget)caller).Target = value;
				if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Target"));
			}
		}

		private void getTarget(object sender, RoutedEventArgs e)
		{
			if (isShowingList == true)
			{
				// close the coice stackpanel
				Choice.Visibility = Visibility.Collapsed;
				isShowingList = false;
			}
			else
			{
				// reload the machine+param list
				fillTreeList();

				// open the choice stackpanel
				Choice.Visibility = Visibility.Visible;
				isShowingList = true;
			}
		}

		private void selectTarget(object sender, RoutedEventArgs e)
		{
			TreeViewItem tItem = (TreeViewItem)machineTree.SelectedItem;

			// if the item selected in the list is acceptable (eg: bottom leaf)
			if (!tItem.HasItems)
			{
				// set target to item
				TreeViewItem pItem = (TreeViewItem)tItem.Parent;
				IParameter param = (IParameter)pItem.Tag;
				Target = new Tuple<IParameter, int>(param, pItem.Items.IndexOf(tItem));

				// fill textblocks
				fillTextBlocks();
			}
		}

		private void fillTextBlocks()
		{
			// get them from target
			MachineName.Text = target.Item1.Group.Machine.Name;
			ParameterName.Text = target.Item1.Name;
			TrackName.Text = target.Item2.ToString();
		}

		private void fillTreeList()
		{
			machineTree.Items.Clear();
			foreach (IMachine m in BuzzGUI.Common.Global.Buzz.Song.Machines)
			{
				//  get machine and name
				TreeViewItem mach = new TreeViewItem() { Header = m.Name, Tag = m };
				IEnumerable<IParameter> ps = m.AllNonInputParameters();//. .AllParameters();
				for (int i = 0; i < ps.Count(); i++)
				{
					// get param and name
					IParameter p = ps.ElementAt(i);
					TreeViewItem param = new TreeViewItem() { Header = p.Name, Tag = p };
					for (int j = 0; j < p.Group.TrackCount; j++)
					{
						TreeViewItem track = new TreeViewItem() { Header = j.ToString() };
						track.Tag = "track " + j.ToString();
						// add tracks
						param.Items.Add(track);
					}
					// add params
					mach.Items.Add(param);
				}
				//add machine
				machineTree.Items.Add(mach);
			}
		}


		public ParameterTargetGUI(PPTarget c, Tuple<IParameter, int> t, Types m)
		{
			InitializeComponent();
			DataContext = this;
			caller = c;
			Target = t;
			mode = m;
			isShowingList = false;
		}

		public UserControl AsUserControl()
		{
			return this;
		}

		/*		public void Update(double dout)
				{
				}
				*/
		public event PropertyChangedEventHandler PropertyChanged;
	}
}
