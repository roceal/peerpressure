using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Media;
using BuzzGUI.Interfaces;
using System.ComponentModel;
using Buzz.MachineInterface;
using BuzzGUI.Common;
using BuzzGUI.Common.InterfaceExtensions;
using MT_Peer_Pressure.Relations;
using MT_Peer_Pressure.Effectors;
using MT_Peer_Pressure.Targets;

namespace MT_Peer_Pressure
{
    public class Peer : INotifyPropertyChanged
    {
	//----------------------------------- new code
		private bool onOff = false;
		public bool OnOff
		{
			get
			{
				return onOff;
			}
			set
			{
				onOff = value;
				if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("OnOff"));
			}
		}

		// Input
		private Effector inputEffector = null;
		public Effector InputEffector
		{
			get
			{
				return inputEffector;
			}

			set
			{
				inputEffector = value;
				if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("InputEffector"));
			}

		}

		// Output
		private PPTarget target = null;
		public PPTarget Target 
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

		// Processing - Add a Relation to this list which process the input into output
		private List<Relation> relations = null;
		public List<Relation> Relations
		{
			get
			{
				if(relations == null) relations = new List<Relation>();

				return relations;
			}
			set
			{
				relations = value;
				if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Relations"));
			}
		}

		public void Update()
		{
			if ((inputEffector == null) || (relations == null) || (target == null)) return;

			double current = inputEffector.poll();
			foreach (Relation r in relations)
			{
				current = r.Process(current);
				if (current < 0) current = 0;
				else if (current > 1) current = 1;
			}
			target.Update(current);
		}

        private IMachine ownerIMachine { get; set; }
		public IMachine owner
		{
			get
			{
                return ownerIMachine;
			}
			set
			{
				ownerIMachine = value;
			}
		}

		public Peer()
		{/*
			// testing code
			Target = new ParameterTarget();
			Relations.Add(new StraightRelation());
			Relations.Add(new ScaledRelation());
			InputEffector = new LFOEffector();
		*/}

		public Peer(IMachine o, Effector e, Relation r, PPTarget t)
		{
			owner = o;
			InputEffector = e;
			Relations.Add(r);
			Target = t;
		}

		public Peer(IMachine o, Effector e, List<Relation> r, PPTarget t)
		{
			owner = o;
			InputEffector = e;
			Relations = r;
			Target = t;
		}

		public event PropertyChangedEventHandler PropertyChanged;


	}
}
