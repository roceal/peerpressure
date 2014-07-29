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
using BuzzGUI.Interfaces;
using BuzzGUI.Common;
using BuzzGUI.Common.InterfaceExtensions;

namespace MT_Peer_Pressure
{
    /// <summary>
    /// Interaction logic for Peer.xaml
    /// </summary>
    public partial class SelectionWindow : Window
    {
        Peer peer;
        public SelectionWindow(Peer pin)
        {
            peer = pin; 
            InitializeComponent();
            // choose machine param and track
            foreach (IMachine m in BuzzGUI.Common.Global.Buzz.Song.Machines)
            {
                //  get machine and name
                TreeViewItem mach = new TreeViewItem() { Header = m.Name, Tag = m };
                IEnumerable<IParameter> ps = m.AllParameters();
                for (int i = 0; i < ps.Count(); i++)
                {
                    // get param and name
                    IParameter p = ps.ElementAt(i);
                    TreeViewItem param = new TreeViewItem() { Header = p.Name, Tag = p };
                    for (int j = 0; j < p.Group.TrackCount; j++)
                    {
                        TreeViewItem track = new TreeViewItem() { Header = j.ToString() };
                        track.Tag = "track " + j.ToString();
                        // add tracks
                        param.Items.Add(track);
                    }
                    // add params
                    mach.Items.Add(param);
                }
                //add machine
                treeView1.Items.Add(mach);
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
/*            TreeViewItem t = (TreeViewItem)treeView1.SelectedItem;

            IParameter tp;
			MuteTarget mt = null;
			int tr;

            if (!t.HasItems)
            { // we are in the track level, joy!
                TreeViewItem p = (TreeViewItem)t.Parent;
                tp = (IParameter)p.Tag;

                tr = Int32.Parse((String)t.Header);
            }
            else if ((t.Parent != treeView1) && (t.Parent != null))
            { // we are on the param, just take track zero
                tp = (IParameter)t.Tag;
                tr = 0;
            }
            else
            { // we havent gotten an acceptable selection, set the peer to null
                tr = 0;
                tp = null;
            }
            peer.track = tr;
            peer.Target = mt;
            Close();
            
			*/
        }
    }
}
