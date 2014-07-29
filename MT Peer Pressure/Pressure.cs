using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.ComponentModel;
using System.Collections.ObjectModel;
using BuzzGUI.Common;
using Buzz.MachineInterface;
using BuzzGUI.Interfaces;



namespace MT_Peer_Pressure
{

    [MachineDecl(Name = "MT Peer Pressure", ShortName = "Pressure", Author = "Mantratronic", MaxTracks = 256)]
    public class Pressure : IBuzzMachine, INotifyPropertyChanged
    {
        IBuzzMachineHost host;
		bool isMasterPeer;
		IMachine masterPeer;

		public IMachine HostMachine
		{
			get
			{
				return host.Machine;
			}
			set
			{
//				host = value;
			}
		}

		private List<PPTrackVM> tracks;
		public List<PPTrackVM> Tracks
		{
			get
			{
                if (tracks == null)
                    tracks = new List<PPTrackVM>();
				return tracks;
			}
			set
			{
				tracks = value;
				if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Tracks"));
			}
		}

        public Pressure(IBuzzMachineHost host)
        {
            this.host = host;
			isMasterPeer = false;
			IMachine master = null;

			// set up the first empty track
			tracks = new List<PPTrackVM>();

			foreach (IMachine m in Global.Buzz.Song.Machines)
				if ((m.DLL.Name == "MT Peer Pressure")&&(m != host.Machine))
					master = m;

			// if one exists, link to that
			if (master != null)
			{
				//peers = ((Pressure)master.ManagedMachine).Peers;
				//targets = ((Pressure)master.ManagedMachine).Targets;
				//effectors = ((Pressure)master.ManagedMachine).Effectors;
				//relations = ((Pressure)master.ManagedMachine).Relations;
				masterPeer = master;
			}
			// if none exist, setup the peers and module lists
			else
			{
				// claim MasterPeer
				isMasterPeer = true;
				//PeerPressure.masterMachine = host;

				// setup main collections
				PeerPressure.peers = new List<Peer>();
				// modules
				//PeerPressure.targets = new List<PPTarget>();
				//PeerPressure.effectors = new List<Effector>();
				//PeerPressure.relations = new List<Relation>();

				// add the default modules	
				PeerPressure.targets.Add(new Targets.MultiTarget());
				PeerPressure.targets.Add(new Targets.MuteTarget());
				PeerPressure.targets.Add(new Targets.NullTarget());
				PeerPressure.targets.Add(new Targets.ParameterTarget());
				PeerPressure.effectors.Add(new Effectors.AudioEffector());
				PeerPressure.effectors.Add(new Effectors.ButtonEffector());
				PeerPressure.effectors.Add(new Effectors.LFOEffector());
				PeerPressure.effectors.Add(new Effectors.MidiEffector(0, 0));
				PeerPressure.effectors.Add(new Effectors.NoiseEffector());
				PeerPressure.effectors.Add(new Effectors.NullEffector());
				PeerPressure.effectors.Add(new Effectors.OSCEffector());
				PeerPressure.effectors.Add(new Effectors.SliderEffector(this, null, 0));
				PeerPressure.relations.Add(new Relations.ASDREnvelopeRelation(host));
				PeerPressure.relations.Add(new Relations.CropRelation());
				PeerPressure.relations.Add(new Relations.ExponentialRelation());
				PeerPressure.relations.Add(new Relations.FrequencyRelation());
				PeerPressure.relations.Add(new Relations.InertiaRelation());
				PeerPressure.relations.Add(new Relations.InverseRelation());
				PeerPressure.relations.Add(new Relations.NoiseRelation());
				PeerPressure.relations.Add(new Relations.QuadraticRelation());
				PeerPressure.relations.Add(new Relations.ScaledRelation());
				PeerPressure.relations.Add(new Relations.SteppedRelation());
				PeerPressure.relations.Add(new Relations.StraightRelation());
				PeerPressure.relations.Add(new Relations.TriggerRelation());

				//tracks.Add(new PPTrackVM());
                // and a first peer
                newPeer(this.HostMachine);
               
                /* lock (PeerPressure.peers)
                               {
                                   PeerPressure.peers.Add(newPeer(host.Machine));
                               }*/
            }
            /*
			if (machineState.Peers.Count() > 0)[
			{
				// we are loading?

				//machineState.IsMaster = isMasterPeer;
				tracks = machineState.Tracks;

				foreach (PeerState ps in machineState.Peers)
				{
					Peers.Add(ps.toPeer());
				}
			}*/
		}

		#region parameters
		[ParameterDecl(MaxValue = 127, DefValue = 1, Description = "Updates per tick")]
        public int updatesPerTick { get; set; }

		[ParameterDecl(MaxValue = 254, DefValue = 0, Description = "Peer id, 0 = current track")]
		public void PeerID(int v, int track)
		{
			// 0 - no id set or id too high; use track number
			if ( (v == 0) || (v > Peers.Count()) ) v = track; 
			tracks[track].peerIndex = v;
		}

        [ParameterDecl(MaxValue = 1, DefValue = 0, Description = "Peer on/off", ValueDescriptions = new[] { "off", "on" })]
        public void PeerOnOff(bool v, int track) {
			PeerPressure.peers[tracks[track].peerIndex].OnOff = v;
        }

		// zero are null/reset values
		[ParameterDecl(MaxValue = 254, DefValue = 0, Description = "Effector Parameter")]
		public void EffectorP(int v, int track)
		{
			tracks[track].effectorParameter = v;
		}

		[ParameterDecl(MaxValue = 65534, DefValue = 0, Description = "Effector Data", ValueDescriptor = Descriptors.Percentage)]
		public void EffectorD(int v, int track)
		{
			tracks[track].effectorData = v;

			// if effector on this track isnt on zero and peer exists //and peer is on
			if ((tracks[track].effectorParameter != 0) && (PeerPressure.peers[track] != null) && (PeerPressure.peers[tracks[track].peerIndex].OnOff))
			{
				// and it's setup
				if (PeerPressure.peers[track].InputEffector != null) 
				{
					// and the effector has enough params
					if (PeerPressure.peers[track].InputEffector.Parameters.Count() >= tracks[track].effectorParameter) 
					{
						PeerPressure.peers[track].InputEffector.Parameters[tracks[track].effectorParameter - 1] = ((double)tracks[track].effectorData / 65535.0);
					}
				}
			}
		}

		[ParameterDecl(MaxValue = 254, DefValue = 0, Description = "Relation Number")]
		public void RelationN(int v, int track)
		{
			tracks[track].relationID = v;
		}

		[ParameterDecl(MaxValue = 254, DefValue = 0, Description = "Relation Parameter")]
		public void RelationP(int v, int track)
		{
			tracks[track].relationParameter = v;
		}

		[ParameterDecl(MaxValue = 65534, DefValue = 0, Description = "Relation Data", ValueDescriptor = Descriptors.Percentage)]
		public void RelationD(int v, int track)
		{
			tracks[track].relationData = v;

			// if relation id and param on this track isnt on zero and peer exists //and peer is on
			if ((tracks[track].relationID != 0) && (tracks[track].relationParameter != 0) && (PeerPressure.peers[track] != null) && (PeerPressure.peers[tracks[track].peerIndex].OnOff))
			{
				// and it's setup
				if ((PeerPressure.peers[track].Relations != null) && (PeerPressure.peers[track].Relations[tracks[track].relationID - 1] != null))
				{
					// and the effector has enough params
					if (PeerPressure.peers[track].Relations[tracks[track].relationID - 1].Parameters.Count() >= tracks[track].relationParameter)
					{
						PeerPressure.peers[track].Relations[tracks[track].relationID - 1].Parameters[tracks[track].relationParameter - 1] = ((double)tracks[track].relationData / 65535.0);
					}
				}
			}
		}
		#endregion

//		public static List<Peer> peers;
		public List<Peer> Peers
		{
			get {
                //make sure it's init'd, init if not
                if (PeerPressure.peers == null)
                {
                    PeerPressure.peers = new List<Peer>();
                }
                return PeerPressure.peers; 
            }
			set
			{
				PeerPressure.peers = value;
				if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Peers"));
			}
		}

		private int subTickCount = 0;
		public bool Work(Sample[] output, int n, WorkModes mode)
        {
			if ((host.SubTickInfo.CurrentSubTick == 0) || (subTickCount > (host.SubTickInfo.SubTicksPerTick / updatesPerTick)))
			{
				for (int i = 0; i < Peers.Count; i++)
				{
					Peer p = Peers[i];
					if ((p != null) && p.OnOff)//&& (tracks[i].peerOnOff))
					{
						p.Update();
					}
				}

				subTickCount = 0;
			}
			if (host.SubTickInfo.PosInSubTick == 0)
				subTickCount++;
			return true;
        }

		#region interface to clients

		public Peer newPeer(IMachine m)
        {
			// make a straight relation
			Relations.StraightRelation sr = new MT_Peer_Pressure.Relations.StraightRelation();
			// make the relations list
			List<Relation> lr = new List<Relation>();
			lr.Add(sr);

			// make null effector and target
			Effectors.NullEffector ne = new MT_Peer_Pressure.Effectors.NullEffector();
			Targets.NullTarget nt = new MT_Peer_Pressure.Targets.NullTarget();

			// make the peer
			Peer p = new Peer(m, ne, lr, nt);

			// add peer
            Peers.Add(p);

			// if we are getting a request from self
			if (m == host.Machine)
			{
				// add track
				PPTrackVM t = new PPTrackVM();
				t.peerIndex = Peers.Count();
				tracks.Add(t);
			}

			return p;
        }

		// Collections of all loaded targets, relations and effectors. PP should be able to allow other machines to add to them on load and remove on unload
		//static private List<PPTarget> targets = new List<PPTarget>();
		public List<PPTarget> Targets
		{
			get
			{
				return PeerPressure.targets;
			}
			set
			{
				PeerPressure.targets = value;
				if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Targets"));
			}
		}
		
		//static private List<Relation> relations = new List<Relation>();

		public List<Relation> Relations
		{
			get
			{
				return PeerPressure.relations;
			}

			set
			{
				PeerPressure.relations = value;
				if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Relations"));
			}
		}

		//static private List<Effector> effectors = new List<Effector>();
		public List<Effector> Effectors
		{
			get
			{
				return PeerPressure.effectors;
			}

			set
			{
				PeerPressure.effectors = value;
				if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Effectors"));
			}
		}
		#endregion

		#region load/save state
		public class State
        {
			// should contain track info, peer + module data (inc target id data), and on/off and master states
			private List<PPTrackVM> tracks;
			private List<PeerState> peers;
			private bool on, isMaster;

            public State() 
			{ 
				on = false;
				isMaster = false;
				tracks = new List<PPTrackVM>();
				peers = new List<PeerState>();
            //    Peers = new ObservableCollection<Peer>(peers); 
            }	// NOTE: parameterless constructor is required by the xml serializer

            public bool On 
            { 
                get 
                { 
                    return on; 
                }
                set
                {
                    on = value;
                }
            }

			public bool IsMaster
			{
				get
				{
					return isMaster;
				}
				set
				{
					isMaster = value;
				}
			}

			public List<PPTrackVM> Tracks
			{
				get
				{
					return tracks;
				}
				set
				{
					tracks = value;
				}
			}

			public List<PeerState> Peers
			{
				get
				{
					return peers;
				}
				set
				{
					peers = value;
				}
			}

			public event PropertyChangedEventHandler PropertyChanged;
        }

        State machineState = new State();
        public State MachineState		
        {
            get 
			{
 				// everything that PP owns to file
				machineState.IsMaster = isMasterPeer;
				machineState.Tracks = tracks;

				machineState.Peers = new List<PeerState>();
				foreach (Peer p in Peers)
				{
					machineState.Peers.Add(new PeerState(p));
				}

				return machineState; 
			}
            set
            {
                machineState = value;
				isMasterPeer = machineState.IsMaster;
				tracks = machineState.Tracks;
				Peers = new List<Peer>();
				foreach (PeerState ps in machineState.Peers)
				{
					Peers.Add(ps.toPeer());
				}
//                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("MachineState"));
            }
        }

		public void ImportFinished(IDictionary<string, string> machineNameMap)
		{
            
			foreach (Peer p in Peers)
			{
                if (p == null)
                {
                    // we need to try again?
                }

                else if (p.owner != null)
                {
                    if (machineNameMap.ContainsKey(p.owner.Name))
                    {
                        // we need to replace the owner
                    }
                }

			}
             
		}
		#endregion

		public event PropertyChangedEventHandler PropertyChanged;
		public event Action<Object, Peer, PPTrackVM, EventArgs> PeerAdded;	  
		public event Action<Object, Peer, EventArgs> PeerRemoved;

		//------------------------------------- new code
		// on MachineState set
		void MachineState_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			// TODO: reload the peers/targets/owners from state
		}

		// on ParameterGroups[Track].TrackCount set
		void TrackCount_PropertyChanged(object sender, PropertyChangedEventArgs e) // host.machine?
		{
			// if our track count changes, 
			int delta = host.Machine.ParameterGroups[(int)ParameterGroupType.Track].TrackCount - Peers.Count;
			// compare to old number
			if (delta > 0)
			{
				// delete last if less
				delta = Math.Abs(delta);
				for (int i = 0; i < delta; i++)
				{
					Peers.Remove(Peers.Last());
					tracks.Remove(tracks.Last());
				}
			}
			else if (delta < 0)
			{
				// add if more
				delta = Math.Abs(delta);
				for (int i = 0; i < delta; i++)
				{
					Peers.Add(newPeer(host.Machine));

					// fire the event
					PeerAdded(this, Peers.Last(), tracks.Last(), new EventArgs());
				}
				if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("GUIPeers"));

			}
		}

		#region utils

/*		public PPTarget getNewOfType(PPTarget pptIn)
		{
			foreach (PPTarget t in Targets)
			{
				Type.GetType(t.GetType().Name);
			}
			return null;
		}
		*/


		#endregion
	}

	public class PPTrackVM : INotifyPropertyChanged
	{
		public int effectorParameter { get; set; }
		public int effectorData { get; set; }
		public int relationID { get; set; }
		public int relationParameter { get; set; }
		public int relationData { get; set; }
		public int peerIndex { get; set; }

		public PPTrackVM()
		{
			effectorParameter = 0;
			effectorData = 0;
			relationID = 0;
			relationParameter = 0;
			relationData = 0;
			peerIndex = -1;
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
