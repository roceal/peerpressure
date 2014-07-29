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
	public partial class selectTarget : Window
	{
		Peer peer;
		List<PPTarget> targets;

		public selectTarget(Peer pIn, List<PPTarget> tIn)
		{
			InitializeComponent();

			peer = pIn;
			targets = tIn;

			foreach (PPTarget t in targets)
			{
				comboBox1.Items.Add(t.GetType().Name);
			}
		}

		private void button1_Click(object sender, RoutedEventArgs e)
		{
			if (comboBox1.SelectedItem != null)
			{
				PPTarget ppt = (PPTarget)targets[comboBox1.SelectedIndex].Clone();
				peer.Target = ppt;
				this.Close();
			}
			
		}
	}
}
