using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Buzz.MachineInterface;
using BuzzGUI.Interfaces;
using BuzzGUI.Common;
using BuzzGUI.Common.InterfaceExtensions;
using MT_Peer_Pressure;
using MT_Peer_Pressure.Effectors;
using MT_Peer_Pressure.Targets;


namespace MT.Overpass
{
    [MachineDecl(Name = "Mantratronic Overpass 0.6", ShortName = "Overpass", Author = "Ronan O'Ceallaigh", MaxTracks = 100)]
    public class OverpassMachine : IBuzzMachine, INotifyPropertyChanged
    {
		IBuzzMachineHost host;
        Peer[] peers = new Peer[100];
        int[] mac = new int[100];
		
		private bool imported = false;
		Pressure pressure = null;

		int TrackCount { get { return host.Machine.ParameterGroups[2].TrackCount; } }

		public OverpassMachine(IBuzzMachineHost host)
		{
			this.host = host;
			//Gain = new Interpolator();
//            mac = new ObservableCollection<int>();
//            peers = new ObservableCollection<Peer>();

			// look for PP
			IMachine master = null;

			foreach (IMachine m in Global.Buzz.Song.Machines)
				if (m.DLL.Name == "MT Peer Pressure")
					master = m;

			// if its there set pressure
			if (master != null) pressure = (Pressure)master.ManagedMachine;
			// else we need to make one and add it to the song

			// add my effector
			pressure.Effectors.Add(new MT_Peer_Pressure.Effectors.SliderEffector(null, null, 0));
        }

        [ParameterDecl(ValueDescriptions = new[] { "---", "mute", "bypass", "solo" }, MaxValue = 3, DefValue = 0)]
        public void Mac(int v, int track)
        {
//			if (!imported) return;

			// check if we have enough peers
			if (peers[track] == null)
			{
				peers[track] = pressure.newPeer(host.Machine);
				if (host.Machine != null)
				{
					peers[track].Target = new MuteTarget();
					peers[track].InputEffector = new SliderEffector(this, host.Machine.ParameterGroups[(int)ParameterGroupType.Track].Parameters[track], track);
				}
			}
			else
			{
				// check if we have f*(£$%* IMachine to work off
				if ((peers[track].InputEffector == null) && (host.Machine != null))
				{
					peers[track].InputEffector = new SliderEffector(this, host.Machine.ParameterGroups[(int)ParameterGroupType.Track].Parameters[track], track);
				}
			}

            // if we have a peer setup
            if (peers[track] != null)
            {
                // if input == 0 remove all settings
                if (v == 0)
                {
					// update the effector
                }
                // if input == 1 (remove other settings), add mute setting
                else if (v == 1)
                {
					// update the effector
				}
                // if input == 2 (remove other setting), add bypass setting
                else if (v == 2)
                {
					// update the effector
				}
                // if input == 3 (remove other settings), add solo setting
                else if (v == 3)
                {
					// update the effector
				}
            }

            // record setting
            mac[track] = v;
        }

        public ObservableCollection<Peer> Peers
        {
            get
            {
                return new ObservableCollection<Peer>(peers);
            }
            set
            {
//                peers = (Peer[100]) value;
            }
        }

/*		public void ImportFinished(IDictionary<string, string> machineNameMap)
		{
			if ((machineState.Names != null) && (machineState.Mac != null))
			{
				Peers.Clear();
				for (int i = 0; i < machineState.Names.Count(); i++)
				{
					Peer p;
					/*
					// check the updated name map
					if (machineNameMap.ContainsKey(machineState.Names[i]))
					{
						p = new Peer(machineNameMap[machineState.Names[i]], machineState.Tracks[i]);
					}
					// add based on old name
                    else if(machineState.Names[i] != "")
					{
						p = new Peer(machineState.Names[i], machineState.Tracks[i]); 
					}
					else 
					{
						p = new Peer();
					}
					
					peers.Add(p);
					Mac(machineState.Mac[i], p.track);*
				}
			}
			imported = true;
		}
*/

/*        public class State : INotifyPropertyChanged
        {
            public State() 
            { 
                mac = new List<int>();
                tracks = new List<int>();
                names = new List<String>();
            }// NOTE: parameterless constructor is required by the xml serializer

            private List<int> mac;
            public List<int> Mac
            {
                get { return mac; }
                set
                {
                    mac = value;
                    if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Mac"));
                    // NOTE: the INotifyPropertyChanged stuff is only used for data binding in the GUI in this demo. it is not required by the serializer.
                }
            }

            private List<int> tracks;
            public List<int> Tracks
            {
                get { return tracks; }
                set
                {
                    tracks = value;
                    if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Tracks"));
                    // NOTE: the INotifyPropertyChanged stuff is only used for data binding in the GUI in this demo. it is not required by the serializer.
                }
            }

            private List<String> names;
            public List<String> Names
            {
                get { return names; }
                set
                {
                    names = value;
                    if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Names"));
                    // NOTE: the INotifyPropertyChanged stuff is only used for data binding in the GUI in this demo. it is not required by the serializer.
                }
            }

            #region INotifyPropertyChanged Members

            public event PropertyChangedEventHandler PropertyChanged;

            #endregion
        }

        State machineState = new State();
        public State MachineState			// a property called 'MachineState' gets automatically saved in songs and presets
        {
            get 
            {
				// if this is the first get, and State has not been filled, fill from peers
				if ((machineState.Names.Count == 0) && (peers != null))
				{
                    int i = 0;
                    foreach (Peer p in peers) 
                    {
//                        if((p.Target != null)&&(p.Target.Name != null)) machineState.Names.Add(p.Target.Name);
//                        else machineState.Names.Add("");
//                        machineState.Tracks.Add(p.track);
                        machineState.Mac.Add(mac[i++]);
                    }
                }
				// otherwise just return the machinestate
				return machineState; 
            }
            set
            {
                machineState = value;

				if ((machineState.Names != null) && (machineState.Mac != null))
				{
					for (int i = 0; i < machineState.Names.Count(); i++)
					{
						Peer p;
						if (machineState.Names[i] != "")
						{
//							p = new Peer(machineState.Names[i], machineState.Tracks[i]); 
						}
						else 
						{
							p = new Peer();
						}
					
//						peers.Add(p);
//						Mac(machineState.Mac[i], p.track);
					}
				}
				if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("MachineState"));
            }
        }
		*/
        public IEnumerable<IMenuItem> Commands
        {
            get
            {
                yield return new MenuItemVM()
                {
                    Text = "About...",
                    Command = new SimpleCommand()
                    {
                        CanExecuteDelegate = p => true,
                        ExecuteDelegate = p =>
                        {
                            About pgui = new About();
                            pgui.Show();
                        } //MessageBox.Show(  (new About() )
                    }
                };
            }
        }		

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

	/*
    public class MachineGUIFactory : IMachineGUIFactory { public IMachineGUI CreateGUI(IMachineGUIHost host) { return new OverpassGUI(); } }
    public partial class OverpassGUI : UserControl, IMachineGUI, INotifyPropertyChanged
    {
        IMachine machine;
        OverpassMachine overpassMachine;

        void machine_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "TrackCount":
                    //trackSelect.Items.Clear();
                    fillBox();

                    PropertyChanged.Raise(this, "MyTrack");
                    break;
            }
        }

        public class TrackVM : INotifyPropertyChanged
        {
            public int track { get; set; }
            public String name { get; set; }
            public TrackVM(int t) { track = t; name = t.ToString(); }
            public string Name { get { return name; } }

            #region INotifyPropertyChanged Members

            public event PropertyChangedEventHandler PropertyChanged;

            #endregion
        }

        List<TrackVM> myTrackList = new List<TrackVM>();
        public IEnumerable<TrackVM> MyTrackList 
		{ 
			get 
			{ 
				return myTrackList.OrderBy(t => t.name); 
			} 
		}
        TrackVM myTrack = new TrackVM(0);
        public TrackVM MyTrack
        {
            get
            {
                return myTrack;
            }
            set
            {
                myTrack = value;
                PropertyChanged.Raise(this, "MyTrack");
            }
        }

        public IMachine Machine
        {
            get { return machine; }
            set
            {
                if (machine != null)
                {
					machine.PropertyChanged -= machine_PropertyChanged;
				}

                machine = value;

                if (machine != null)
                {
					machine.PropertyChanged += machine_PropertyChanged;
					overpassMachine = (OverpassMachine)machine.ManagedMachine;
                    fillBox();
                    PropertyChanged.Raise(this, "MyTrack");
                }
            }
        }

        void fillBox() 
        {
            myTrackList.RemoveRange(0, myTrackList.Count);
            for (int i = 0; i < machine.ParameterGroups[(int)ParameterGroupType.Track].TrackCount; i++)
            {
                myTrackList.Add(new TrackVM(i));
            }
			PropertyChanged.Raise(this, "MyTrackList");
        }

        public OverpassGUI()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void getPeer_Click(object sender, RoutedEventArgs e)
        {
            int t = myTrack.track;
/*			while (overpassMachine.Peers.Count <= t)
			{
				
			}
            MachineTargetGUI mgui = new MachineTargetGUI(overpassMachine.Peers[t].Target, ((MuteTarget)overpassMachine.Peers[t].Target).Target, MachineTargetGUI.Types.Normal);
			Window w = new Window();
			w.Content = mgui;
			w.Show();
		}

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }*/
}
