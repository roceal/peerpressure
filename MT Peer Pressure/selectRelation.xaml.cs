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
	public partial class selectRelation : Window
	{
		Peer peer;
		List<Relation> relations;

		public selectRelation(Peer pIn, List<Relation> rIn)
		{
			InitializeComponent();

			peer = pIn;
			relations = rIn;

			foreach (Relation r in relations)
			{
				comboBox1.Items.Add(r.GetType().Name);
			}
		}

		private void button1_Click(object sender, RoutedEventArgs e)
		{
			if (comboBox1.SelectedItem != null)
			{
				Relation r = (Relation)relations[comboBox1.SelectedIndex].Clone();
				peer.Relations.Add(r);
				this.Close();
			}
		}
	}
}
