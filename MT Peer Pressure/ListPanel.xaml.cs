using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace MT_Peer_Pressure
{
    /// <summary>
    /// Interaction logic for ListPanel.xaml
    /// </summary>
    public partial class ListPanel : UserControl, INotifyPropertyChanged
    {
        public ListPanel(Pressure p)
        {
            InitializeComponent();
            //fill(p);
            P = p;
        }

        public Pressure P { get; set; }

        
        void LV_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        public ListBox Lv { get { return LV; } }

        public ObservableCollection<string> relations { get; set; }
/*        public class relations : ObservableCollection<string>  
        {
            public relations()
            {
                Add("LightBlue");
                Add("Pink");
                Add("Red");
                Add("Purple");
                Add("Blue");
                Add("Green");
            }
        } 
        */


        // listen for new peers from Pressure, and add them
        // listen for deleted peers from Pressure, and remove them

        public void fill(Pressure p)
        {
            foreach (Peer pr in p.Peers)
            {
                // add each peer's target; value; [owner;] [type;]
                LV.Items.Add(pr.ToString());
            }
        }
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
