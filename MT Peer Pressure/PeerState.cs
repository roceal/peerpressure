using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuzzGUI.Common;
using BuzzGUI.Interfaces;

namespace MT_Peer_Pressure
{
	[Serializable()]
	public class PeerState 
	{
		// normal variables
		public string ownerName {get; set;}
		public bool onOff { get; set; }

		// module names
		public string effectorName { get; set; }
		public List<string> relationNames { get; set; }
		public string targetName {get; set;}

		// module params
		public List<double> parameters { get; set; }
		public ITargetState targetState { get; set; }

		public PeerState(string oN, bool oO, string eN, List<string> rNs, string tN, List<double> ps, ITargetState ts)
		{
			ownerName = oN;
			onOff = oO;

			effectorName = eN;
			relationNames = rNs;
			targetName = tN;

			parameters = ps;
			targetState = ts;
		}

		public PeerState(Peer pIn)
		{
			// TODO: convert the Peer into a PeerState
            try
            {
                if (pIn == null) return;
                if (pIn.owner != null) ownerName = pIn.owner.Name;
                onOff = pIn.OnOff;

                effectorName = pIn.InputEffector.GetType().ToString();
                foreach (double d in pIn.InputEffector.Parameters)
                {
                    parameters.Add(d);
                }

                relationNames = new List<string>();
                foreach (Relation r in pIn.Relations)
                {
                    relationNames.Add(r.GetType().ToString());
                    foreach (double d in r.Parameters)
                    {
                        parameters.Add(d);
                    }
                }

                targetName = pIn.Target.GetType().ToString();
                targetState = pIn.Target.State;
            }
            catch (Exception e)
            {
                BuzzGUI.Common.DebugConsole.WriteLine("failed to save:" + e.Message);
            }
		}

		public Peer toPeer()
		{
			// search for the owner machine
			IMachine ma = (IMachine)Global.Buzz.Song.Machines.Select(m => m.Name == ownerName);
				// if it doesnt exist return null
			if (ma == null) return null;

			// search for the module types
			// create new modules
			// if any dont exist return null
			Effector ef = (Effector)PeerPressure.effectors.Select(e => e.GetType().ToString() == effectorName);
			if (ef == null) return null;

			List<Relation> rels = new List<Relation>();
			foreach (string s in relationNames)
			{
				rels.Add((Relation)PeerPressure.relations.Select(r => r.GetType().ToString() == s));
				if (rels.Last() == null) return null;
			}

			PPTarget ppt = (PPTarget)((PPTarget)PeerPressure.targets.Select(t => t.GetType().ToString() == targetName)).Clone();
			if (ppt == null) return null;

			// fill the parameters
			int i;
			for (i = 0; i < ef.Parameters.Count(); i++)
			{
				ef.Parameters[i] = parameters[i];
			}
			for (int j = 0; j < rels.Count(); j++)
			{
				for (int k = 0; k < rels[j].Parameters.Count(); k++)
				{
					rels[j].Parameters[k] = parameters[i++];
				}
			}

			// create empty peer with correct owner
			// add the modules
			Peer ret = new Peer(ma, ef, rels, ppt);
			return ret;
		}
	}
}
