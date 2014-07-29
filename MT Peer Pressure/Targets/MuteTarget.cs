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
	public class MuteTarget : PPTarget, INotifyPropertyChanged
	{
		private IMachine target;
		new public IMachine Target
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

		public MuteTarget()
		{
			// get a machine (generator or effect only) from the user
			// setup gui
			Gui = new MachineTargetGUI(this, target, MachineTargetGUI.Types.Normal);
		}

		public override object Clone()
		{
			MuteTarget clone = new MuteTarget();
			return (object)clone;
		}


		public override string ToString()
		{
			return "Mute: " + target.Name;
		}

		override public UserControl Display()
		{
			TextBlock tb = new TextBlock();
			tb.Text = target.Name + "-mute";
			UserControl uc = new UserControl();
			uc.Content = tb;
			return uc;
		}

		override public void Update(double dout)
		{
			// if we have a peer setup
			if (target != null)
			{
				// if dout > .5 mute
				if (dout > 0.5)
				{
					Application.Current.Dispatcher.BeginInvoke(new Action(() =>
					{
						target.IsMuted = true;
					}));
				}
				// else unmute
				else
				{
					Application.Current.Dispatcher.BeginInvoke(new Action(() =>
					{
						target.IsMuted = false;
					}));
				}
			}
		}
		new public event PropertyChangedEventHandler PropertyChanged;
	}

}
