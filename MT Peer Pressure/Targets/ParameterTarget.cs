using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows;
using BuzzGUI.Interfaces;
using BuzzGUI.Common;
using BuzzGUI.Common.InterfaceExtensions;

namespace MT_Peer_Pressure.Targets
{
	public class ParameterTarget : PPTarget, INotifyPropertyChanged
	{
		private Tuple<IParameter, int> target;
		new public Tuple<IParameter, int> Target
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

		public ParameterTarget()
		{
			// setup gui
			Gui = new ParameterTargetGUI(this, target, ParameterTargetGUI.Types.Normal);
		}

		public override object Clone()
		{
			ParameterTarget clone = new ParameterTarget();
			return (object)clone;
		}

		public override string ToString()
		{
			if ((target == null)||(target.Item1 == null)) return "Not Ready";
			return ("Param: " + target.Item1.Name);
		}

		override public UserControl Display()
		{
			TextBlock tb = new TextBlock();
			tb.Text = target.Item1.Group.Machine.Name + " " + target.Item1.Name + target.Item2.ToString();
			UserControl uc = new UserControl();
			uc.Content = tb;
			return uc;
		}

		override public void Update(double dout)
		{
			// if we have a peer setup
			if ((target != null) && (target.Item1 != null))
			{
				// set the parameter according to what type it is
				switch (target.Item1.Type)
				{
					case ParameterType.Note:
						int note = (int)(dout * 120); //(c0-b9)
						note = note + (4 * (note / 12)); //buzznote
						target.Item1.SetValue(target.Item2, note);
						break;
					case ParameterType.Word:
						int word = target.Item1.MinValue + (int)(dout * (target.Item1.MaxValue - target.Item1.MinValue));
						target.Item1.SetValue(target.Item2, word);
						break;
					case ParameterType.Byte:
						int byt = target.Item1.MinValue + (int)(dout * (target.Item1.MaxValue - target.Item1.MinValue));
						target.Item1.SetValue(target.Item2, byt);
						break;
					case ParameterType.Switch:
						int switc = (int)(dout);
						target.Item1.SetValue(target.Item2, switc);
						break;
				}
			}
		}

		new public event PropertyChangedEventHandler PropertyChanged;
	}
}
