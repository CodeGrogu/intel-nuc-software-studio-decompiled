using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace NucLedMgr.WMI.Controls;

public partial class ColorCanvas : UserControl, IComponentConnector
{
	public RGB Selected = new RGB();

	private double currH = 360.0;

	private double pointx;

	private double pointy;

	private Point? dragStart;

	private UIElement _lastClickedUIElement;

	private Point? _clickOffset;

	public ColorCanvas()
	{
		InitializeComponent();
		RGB[] array = HSV.GradientSpectrum();
		LinearGradientBrush linearGradientBrush = new LinearGradientBrush
		{
			StartPoint = new Point(0.0, 0.0),
			EndPoint = new Point(0.0, 1.0)
		};
		for (int i = 0; i < array.Length; i++)
		{
			GradientStop value = new GradientStop(array[i].Color(), (double)i * 0.16);
			linearGradientBrush.GradientStops.Add(value);
		}
		SpectrumGrid.Opacity = 1.0;
		SpectrumGrid.Background = linearGradientBrush;
		MiddleStop.Color = HSV.RGBFromHSV(0.0, 1.0, 1.0).Color();
		rgbText.Text = MiddleStop.Color.ToString();
		RText.Text = MiddleStop.Color.R.ToString();
		GText.Text = MiddleStop.Color.G.ToString();
		BText.Text = MiddleStop.Color.B.ToString();
	}

	private void RgbGradient_MouseMove(object sender, MouseEventArgs e)
	{
		_ = e.LeftButton;
		_ = 1;
	}

	private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
	{
	}

	private void SpectrumGrid_MouseMove(object sender, MouseEventArgs e)
	{
		if (e.LeftButton != MouseButtonState.Pressed)
		{
			return;
		}
		double y = e.GetPosition(SpectrumGrid).Y;
		if (!(y > SpectrumGrid.ActualHeight - 15.0))
		{
			SpectrumGridBar.Margin = new Thickness(0.0, y, 0.0, 0.0);
			currH = 360.0 * (y / SpectrumGrid.ActualHeight);
			MiddleStop.Color = HSV.RGBFromHSV(currH, 1.0, 1.0).Color();
			double x = ColorLense.TransformToAncestor(this).Transform(new Point(0.0, 0.0)).X;
			RGB rGB = (Selected = ((!(x < CanvasColor.ActualWidth / 2.0)) ? HSV.RGBFromHSV(currH, (CanvasColor.ActualWidth / 2.0 - (x - CanvasColor.ActualWidth / 2.0)) / CanvasColor.ActualWidth, 1.0) : HSV.RGBFromHSV(currH, 1.0, x / (CanvasColor.ActualWidth / 2.0))));
			if (rGB != null)
			{
				rgbText.Text = rGB.Hex().ToString();
				RText.Text = rGB.R.ToString();
				GText.Text = rGB.G.ToString();
				BText.Text = rGB.B.ToString();
			}
		}
	}

	private void ColorLense_MouseUp(object sender, MouseButtonEventArgs e)
	{
		_lastClickedUIElement = null;
	}

	private void ColorLense_MouseDown(object sender, MouseButtonEventArgs e)
	{
		if (e.LeftButton == MouseButtonState.Pressed)
		{
			_lastClickedUIElement = sender as UIElement;
			_clickOffset = e.GetPosition(_lastClickedUIElement);
		}
	}

	private void ColorLense_MouseMove(object sender, MouseEventArgs e)
	{
		if (e.LeftButton != MouseButtonState.Pressed || _lastClickedUIElement == null)
		{
			return;
		}
		Point position = e.GetPosition(CanvasColor);
		if (!(position.X > CanvasColor.ActualWidth + 3.0) && !(position.X < 3.0) && !(position.Y > CanvasColor.ActualHeight + 3.0) && !(position.Y < 3.0))
		{
			_lastClickedUIElement.SetValue(Canvas.LeftProperty, e.GetPosition(this).X - _clickOffset.Value.X);
			_lastClickedUIElement.SetValue(Canvas.TopProperty, e.GetPosition(this).Y - _clickOffset.Value.Y);
			double x = e.GetPosition(this).X;
			RGB rGB = (Selected = ((!(x < CanvasColor.ActualWidth / 2.0)) ? HSV.RGBFromHSV(currH, (CanvasColor.ActualWidth / 2.0 - (x - CanvasColor.ActualWidth / 2.0)) / CanvasColor.ActualWidth, 1.0) : HSV.RGBFromHSV(currH, 1.0, x / (CanvasColor.ActualWidth / 2.0))));
			if (rGB != null)
			{
				rgbText.Text = rGB.Hex().ToString();
				RText.Text = rGB.R.ToString();
				GText.Text = rGB.G.ToString();
				BText.Text = rGB.B.ToString();
			}
		}
	}
}
