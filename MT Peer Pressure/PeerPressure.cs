using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Buzz.MachineInterface;

namespace MT_Peer_Pressure
{
	public static class PeerPressure //: INotifyPropertyChanged
	{
        public static List<Peer> peers = new List<Peer>();
 /*       public static List<Peer> Peers
        {
            get { return peers; }
            set
            {
                peers = value; 
               // if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Peers"));
            }
        }*/

		public static List<PPTarget> targets = new List<PPTarget>();

		public static List<Relation> relations = new List<Relation>();

		public static List<Effector> effectors = new List<Effector>();

		public static PPTime time = new PPTime();

		public static TimeModes mode;

		//public static IBuzzMachineHost masterMachine;
        public static event PropertyChangedEventHandler PropertyChanged;
    }
}
