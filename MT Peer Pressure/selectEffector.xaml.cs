using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MT_Peer_Pressure
{
	/// <summary>
	/// Interaction logic for selectTarget.xaml
	/// </summary>
	public partial class selectEffector : Window
	{
		Peer peer;
		List<Effector> effectors;

		public selectEffector(Peer pIn, List<Effector> eIn)
		{
			InitializeComponent();

			peer = pIn;
			effectors = eIn;

			foreach (Effector e in effectors)
			{
				comboBox1.Items.Add(e.GetType().Name);
			}
		}

		private void button1_Click(object sender, RoutedEventArgs e)
		{
			if (comboBox1.SelectedItem != null)
			{
				Effector ef = (Effector)effectors[comboBox1.SelectedIndex].Clone();
				peer.InputEffector = ef;
				this.Close();
			}
		}
	}
}
