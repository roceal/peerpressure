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
using System.Windows.Threading;

namespace MT.Overpass
{
	/// <summary>
	/// Interaction logic for About.xaml
	/// </summary>
	public partial class About : Window
	{
		IList<Ellipse> yellows;
		IList<Ellipse> reds;
		IList<Ellipse> greens;
		int numberCircles = 12;
		string text;
		TextBlock tb;
		int scrollerPos = 0;

		void fillCircles()
		{
			Point centre = new Point(100, 100);
			RotateTransform rt = new RotateTransform(360.0 / numberCircles);
			rt.CenterX = 45;
			rt.CenterY = 45;

			TranslateTransform yoffset = new TranslateTransform(20, 20);
			TranslateTransform goffset = new TranslateTransform(21, 20);
			TranslateTransform roffset = new TranslateTransform(19, 20);

			RotateTransform rt2;
			yellows = new List<Ellipse>();
			reds = new List<Ellipse>();
			greens = new List<Ellipse>();

			// do in order of visibility
			for (int i = 0; i < numberCircles; i++)
			{
				// a red
				Ellipse rc = new Ellipse();
				rc.Stroke = new SolidColorBrush(Color.FromArgb(127, 255, 0, 0));
				rc.Height = 50;
				rc.Width = 50;
				rt2 = rt.Clone();
				rt2.Angle = (rt2.Angle * i) + (rt2.Angle / 3);
				TransformGroup rtg = new TransformGroup();
				rtg.Children.Add(rt2);
				rtg.Children.Add(roffset);
				rc.RenderTransform = rtg;
				// a green
				Ellipse gc = new Ellipse();
				gc.Stroke = new SolidColorBrush(Color.FromArgb(127, 0, 255, 0));
				gc.Height = 50;
				gc.Width = 50;
				rt2 = rt.Clone();
				rt2.Angle = (rt2.Angle * i) - (rt2.Angle / 3);
				TransformGroup gtg = new TransformGroup();
				gtg.Children.Add(rt2);
				gtg.Children.Add(goffset);
				gc.RenderTransform = gtg;
				//                yc.RenderTransformOrigin = centre;
				canvas1.Children.Add(rc);
				canvas1.Children.Add(gc);

				//canvas1.Children[canvas1.Children.IndexOf(yc)].t
				reds.Add(rc);
				greens.Add(gc);
			}

			for (int i = 0; i < numberCircles; i++)
			{
				// add a yellow
				Ellipse yc = new Ellipse();
				yc.Stroke = Brushes.Yellow; // new SolidColorBrush(Color.FromRgb(0, 255, 255));
				yc.Height = 50;
				yc.Width = 50;
				rt2 = rt.Clone();
				rt2.Angle *= i;
				TransformGroup ytg = new TransformGroup();
				ytg.Children.Add(rt2);
				ytg.Children.Add(yoffset);
				yc.RenderTransform = ytg;

				canvas1.Children.Add(yc);
				yellows.Add(yc);
			}

	
		}

		void RotateCircles()
		{
			// rotate each circle around the centre
			if (yellows != null)
			{
				foreach (Ellipse e in yellows)
				{
					// get the rotation transform
					TransformGroup v = (TransformGroup)e.RenderTransform;
					RotateTransform rt = (RotateTransform)v.Children[0];
					// add to the angle (+1 unless angle >=360, then 0)
					rt.Angle = rt.Angle + 1;
					if (rt.Angle >= 360) rt.Angle = 0;
				}
			}
			if (greens != null)
			{
				foreach (Ellipse e in greens)
				{
					// get the rotation transform
					TransformGroup v = (TransformGroup)e.RenderTransform;
					RotateTransform rt = (RotateTransform)v.Children[0];
					// add to the angle (+1 unless angle >=360, then 0)
					rt.Angle = rt.Angle - 0.5;
					if (rt.Angle <= 0) rt.Angle = 360;
				}
			}
			if (reds != null)
			{
				foreach (Ellipse e in reds)
				{
					// get the rotation transform
					TransformGroup v = (TransformGroup)e.RenderTransform;
					RotateTransform rt = (RotateTransform)v.Children[0];
					// add to the angle (+1 unless angle >=360, then 0)
					rt.Angle = rt.Angle - 1.5;
					if (rt.Angle <= 0) rt.Angle = 360;
				}
			}
		}

		void AdvanceScroller()
		{
//			canvas1.Children.Remove(tb);
			scrollerPos++;
			if ((scrollerPos + 16) >= text.Length) scrollerPos = 0;
			tb.Text = text.Substring(scrollerPos, 16);
//			int i = canvas1.Children.IndexOf(tb);
//			canvas1.Children.Add(tb);
//			tb.RenderTransform = new TranslateTransform(20, 120);
		}

		public About()
		{
			InitializeComponent();

			text = "                mantratronic overpass v0.5 -- 2011 beerware -- muting, soloing, and bypassing control machine -- thanks to oskari, unz, wde & intox for the help and tpz for the logo -- greets to all buzzers! --                 ";
			tb = new TextBlock();
			tb.Background = null;
			tb.Foreground = new SolidColorBrush(Colors.AntiqueWhite);
			tb.Text = text.Substring(0,16);
			tb.RenderTransform = new TranslateTransform(20, 120);
			canvas1.Children.Add(tb);

			fillCircles();

			this.IsVisibleChanged += (sender, e) =>
			{
				if (IsVisible && rotationTimer == null)
				{
					SetTimer();
				}
				else if (!IsVisible && rotationTimer != null)
				{
					rotationTimer.Stop();
					rotationTimer = null;
				}
			};
		}


		DispatcherTimer rotationTimer;
		DispatcherTimer scrollerTimer;
		void SetTimer()
		{
			rotationTimer = new DispatcherTimer();
			rotationTimer.Interval = TimeSpan.FromMilliseconds(50);
			rotationTimer.Tick += (sender, e) =>
			{
				RotateCircles();
			};
			scrollerTimer = new DispatcherTimer();
			scrollerTimer.Interval = TimeSpan.FromMilliseconds(150);
			scrollerTimer.Tick += (sender, e) =>
			{
				AdvanceScroller();
			};
			scrollerTimer.Start();
			rotationTimer.Start();
		}
	}

	public class DrawingCanvas : Canvas
	{
		private List<Visual> visuals = new List<Visual>();
		protected override int VisualChildrenCount
		{
			get { return visuals.Count; }
		}
		protected override Visual GetVisualChild(int index)
		{
			return visuals[index];
		}
		public void AddVisual(Visual visual)
		{
			visuals.Add(visual);
			base.AddVisualChild(visual);
			base.AddLogicalChild(visual);
		}
		public void DeleteVisual(Visual visual)
		{
			visuals.Remove(visual);
			base.RemoveVisualChild(visual);
			base.RemoveLogicalChild(visual);
		}
	}
}
