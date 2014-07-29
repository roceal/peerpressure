using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT_Peer_Pressure.Relations
{
	class NoiseRelation : Relation
	{
		private double lastValue;

		private double noise;
		public double Noise
		{
			get
			{
				return noise;
			}
			set
			{
				noise = value;
			}
		}

		private Int16 frequency;
		public Int16 Frequency 
		{
			get 
			{
				return frequency;
			}


			set 
			{
				frequency = value;
			}
		}

		public NoiseRelation()
		{
			lastValue = 0;
			noise = 0.1;
			frequency = 1;
			Parameters.Add(Noise);
			Parameters.Add(Frequency);
		}

		public override object Clone()
		{
			NoiseRelation clone = new NoiseRelation();
			return (object)clone;
		}

		override public double Process(double din)
		{
			// check if enough time has passed
				// get new random for lastValue
			return din + lastValue;
		}
	}
}
