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
using BuzzGUI.Common;

namespace MT_Peer_Pressure
{
	/// <summary>
	/// Interaction logic for ParameterViewer.xaml
	/// </summary>
	public partial class ParameterViewer : UserControl
	{
		public List<double> content;
		public List<double> Contents
		{
			get
			{
				return content;
			}
			set
			{
				content = value;
				fillGrid();
                
			}
		}

        private void fillGrid()
        {
            if (content == null) return;
            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Vertical;
            maingrid.Children.Clear();

            foreach (object o in content)
            {
                StackPanel param = new StackPanel();
                param.Orientation = Orientation.Horizontal;

                // TODO: get the param name
                Label name = new Label();
                name.Content = o.GetType().Name;
                param.Children.Add(name);

                // TODO: change to modifiable controller (slider or buzz equiv)
                TextBox value = new TextBox();
                value.Text = o.ToString();
                param.Children.Add(value);

                //ParameterSlider ps = new ParameterSlider();
                //ps.

                sp.Children.Add(param);
            }
            maingrid.Children.Add(sp);
            
        }
                    
        public ParameterViewer()
		{
			InitializeComponent();
			fillGrid();
		}
	}
}
