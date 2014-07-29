using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace MT_Peer_Pressure.Effectors
{
	class ButtonEffector : Effector
	{
		public Button button { get; set; }
		
		double current;

		public ButtonEffector()
		{
			current = 0.0;
			isInitialised = true;
		}

		override public double poll()
		{
			if (button.IsPressed == true)
				current = 1.0;
			else if (button.IsPressed == false)
				current = 0.0;

			return current;
		}

		public override object Clone()
		{
			ButtonEffector clone = new ButtonEffector();
			return (object)clone;
		}


	}
}
