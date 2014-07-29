using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Buzz.MachineInterface;
using BuzzGUI.Interfaces;
using MT_Peer_Pressure.Targets;

namespace MT_Peer_Pressure
{
	[MachineGUIFactoryDecl(PreferWindowedGUI = true, IsGUIResizable = true, UseThemeStyles = true)]
	public class MachineGUIFactory : IMachineGUIFactory { public IMachineGUI CreateGUI(IMachineGUIHost host) { return new PressureGUI(); } }
	public partial class PressureGUI : UserControl, IMachineGUI, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		IMachine machine;
		Pressure pressureMachine;
		ListBox LV;
        //PeerPressure PP;

		ParameterViewer PV;


		public ObservableCollection<PeerVM> GUIPeers { get; private set; }

		#region button functions
		private void newButton(object sender, RoutedEventArgs e)
		{
			Peer p = pressureMachine.newPeer(machine);
			machine.ParameterGroups[(int)ParameterGroupType.Track].TrackCount++;
			GUIPeers.Add(new PeerVM(p, null));
			PropertyChanged(this, new PropertyChangedEventArgs("GUIPeers"));
			//SelectionWindow sw = new SelectionWindow(p);
			//sw.Show();

			// TESTING
//			GUIPeers[0].p.Target.Target = machine.ParameterGroups[(int)ParameterGroupType.Global].Parameters[0];
//			pressureMachine.Targets[0].Target = machine.ParameterGroups[(int)ParameterGroupType.Global].Parameters[0];
		}

		private void deleteButton(object sender, RoutedEventArgs e)
		{
			// TODO: delete selected peer, move peers and track up around it
			//machine.ParameterGroups[(int)ParameterGroupType.Track].TrackCount--;
		}

		private void moveUpButton(object sender, RoutedEventArgs e)
		{
			// TODO: move selected peer and track up 1
		}

        private void updateButton(object sender, RoutedEventArgs e)
        {
            // debug: force update on selected PeerVM
            if (PeerListBox.SelectedIndex == -1) return;
            GUIPeers[PeerListBox.SelectedIndex].Update();
            PropertyChanged(this, new PropertyChangedEventArgs("GUIPeers"));
        }

		private void moveDownButton(object sender, RoutedEventArgs e)
		{
			// TODO: move selected peer and track down 1
		}

		private void selectTarget(object sender, RoutedEventArgs e)
		{
		}

		private void setEffectorButton(object sender, RoutedEventArgs e)
		{
			if (PeerListBox.SelectedIndex == -1) return;
			// TODO: Use GetType to figure out which Target type i have been given, create the right type, assign it, and then show its gui
//			Type targetType = GUIPeers[PeerListBox.SelectedIndex].p.Target.GetType();
			PPTarget tIn = GUIPeers[PeerListBox.SelectedIndex].p.Target;
			Window w = new Window();

//			if (GUIPeers[PeerListBox.SelectedIndex].p.InputEffector == null)
//			{
				// TODO: find the type and get a new one of the correct type
				// pop up a choose target type window first
				w = new selectEffector(GUIPeers[PeerListBox.SelectedIndex].p, pressureMachine.Effectors);
				// show it
				w.Show();
//			}
//			else
//			{

				// and get the gui
//				w.Content = tIn.Gui.AsUserControl();
				// show it
//				w.Show();
//			}
                GUIPeers[PeerListBox.SelectedIndex].Update();
        }

		void setRelationButton(object sender, RoutedEventArgs e)
		{
			if (PeerListBox.SelectedIndex == -1) return;

			// TODO: Use GetType to figure out which Target type i have been given, create the right type, assign it, and then show its gui
			//			Type targetType = GUIPeers[PeerListBox.SelectedIndex].p.Target.GetType();
			PPTarget tIn = GUIPeers[PeerListBox.SelectedIndex].p.Target;
			Window w = new Window();

			if (GUIPeers[PeerListBox.SelectedIndex].p.Relations == null)
			{
				// TODO: find the type and get a new one of the correct type
				// pop up a choose target type window first
				w = new selectRelation(GUIPeers[PeerListBox.SelectedIndex].p, pressureMachine.Relations);
				// show it
				w.Show();
			}
			else
			{

				// adds atm
				w = new selectRelation(GUIPeers[PeerListBox.SelectedIndex].p, pressureMachine.Relations);
				// show it
				w.Show();

				// and get the gui
				//				w.Content = tIn.Gui.AsUserControl();
				// show it
				//				w.Show();
			}
            GUIPeers[PeerListBox.SelectedIndex].Update();
        }

		void setTargetButton(object sender, RoutedEventArgs e)
		{
			if (PeerListBox.SelectedIndex == -1) return;

			// TODO: Use GetType to figure out which Target type i have been given, create the right type, assign it, and then show its gui
			//			Type targetType = GUIPeers[PeerListBox.SelectedIndex].p.Target.GetType();
			PPTarget tIn = GUIPeers[PeerListBox.SelectedIndex].p.Target;
			Window w = new Window();

//			if (GUIPeers[PeerListBox.SelectedIndex].p.Target == null)
//			{
				// TODO: find the type and get a new one of the correct type
				// pop up a choose target type window first
				w = new selectTarget(GUIPeers[PeerListBox.SelectedIndex].p, pressureMachine.Targets);
				// show it
				w.Show();
//			}
				/* move to change target?
			else
			{

				// and get the gui
				w.Content = tIn.Gui.AsUserControl();
				// show it
				w.Show();
			}*/
                GUIPeers[PeerListBox.SelectedIndex].Update();
        }
		void editTargetButton(object sender, RoutedEventArgs e)
		{
			PPTarget tIn = GUIPeers[PeerListBox.SelectedIndex].p.Target;
			if ((tIn == null) || (tIn.Gui == null)) return;

			Window w = new Window();
			w.Content = tIn.Gui.AsUserControl();
			// show it
			w.Show();
            GUIPeers[PeerListBox.SelectedIndex].Update();
        }

		#endregion

		public IMachine Machine
		{
			get { return machine; }
			set
			{
				if (machine != null)
				{
					//BindingOperations.ClearBinding(LV, ListBox.ItemsSourceProperty);
					pressureMachine = (Pressure)machine.ManagedMachine;
					pressureMachine.PeerAdded -= peerAdded;
					pressureMachine.PeerRemoved -= peerRemoved;
				}

				machine = value;

				if (machine != null)
				{
					if (machine.ManagedMachine == null) return;
					pressureMachine = (Pressure)machine.ManagedMachine;
					
					//pressureMachine.PeerAdded += new Action<Peer>(peerAdded);
					pressureMachine.PeerAdded += peerAdded;
					pressureMachine.PeerRemoved += peerRemoved;

					for (int i = 0; i < pressureMachine.Peers.Count; i++)
					{
						peerAdded(this, PeerPressure.peers[i], null /*pressureMachine.Tracks[i]*/, null);
					}
					//PV.Content = GUIPeers[0].p.InputEffector.Parameters;
//					PV.Content = GUIPeers[0].p.Relations[0].Parameters;
				}
			}
		}

		#region actions for MM
		void peerAdded(object sender, Peer peer, PPTrackVM track, EventArgs e)
		{
			PeerVM pvm = new PeerVM(peer, track);
			if (pvm == null)
			{
				throw new Exception("bad pvm");
			}
			GUIPeers.Add(pvm);

			if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("GUIPeers"));
		}

		void peerRemoved(object sender, Peer peer, EventArgs e)
		{
			GUIPeers.Remove(GUIPeers.First(pr => pr.p == peer));
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("GUIPeers"));
        }
		#endregion

		public PressureGUI()
		{
			InitializeComponent();
			DataContext = this;
			GUIPeers = new ObservableCollection<PeerVM>();
			Height = 300;
			Width = 500;
			PV = new ParameterViewer();
			PViewer.Content = PV;

            LV = new ListBox();
            //LV.
		}

		#region actions for gui

		// on selection in row
		// on selection of row

		void PeerList_Selection_Changed(object sender, RoutedEventArgs e)
		{
//			PV.Content.Clear();
			PV.Contents = GUIPeers[PeerListBox.SelectedIndex].p.InputEffector.Parameters;
			//					PV.Content = GUIPeers[0].p.Relations[0].Parameters;
			//RaiseEvent(
		}

		#endregion

		// strip library and module names from strings
		static private string cleanName(string sIn)
		{
			string sOut;

/*			sOut = sIn;
			if (sOut.Contains("MT_Peer_Pressure."))
			{
				sOut.Replace("MT_Peer_Pressure.", "");
			}*/
			string[] classes = sIn.Split('.');
			sOut = classes.Last();

//			if (sOut.Contains("Target."))
//			{
//				sOut.Replace("MT_Peer_Pressure.", "");
//			}

			return sOut;
		}
	}
}
