using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MT_Peer_Pressure.Relations
{
	public class StraightRelation : Relation, INotifyPropertyChanged
	{
		public StraightRelation()
		{
		}

		public override object Clone()
		{
			StraightRelation clone = new StraightRelation();
			return (object)clone;
		}

		override public double Process(double din)
		{
			return din;
		}
		new public event PropertyChangedEventHandler PropertyChanged;
	}

}
