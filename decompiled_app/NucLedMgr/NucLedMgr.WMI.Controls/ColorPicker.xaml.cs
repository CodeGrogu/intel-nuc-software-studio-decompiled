using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace NucLedMgr.WMI.Controls;

public partial class ColorPicker : UserControl, IComponentConnector
{
	private const string DISPLAY_RECENT_COLORS = "DisplayRecentColors";

	private const string STANDARD_COLORS_ARRAY = "StandardColorsArray";

	public static readonly DependencyProperty DisplayRecentColorsProperty = DependencyProperty.Register("DisplayRecentColors", typeof(Visibility), typeof(ColorPicker));

	public static readonly DependencyProperty StandardColorsArrayProperty = DependencyProperty.Register("StandardColorsArray", typeof(Visibility), typeof(ColorPicker));

	public Visibility DisplayRecentColors
	{
		get
		{
			return (Visibility)GetValue(DisplayRecentColorsProperty);
		}
		set
		{
			SetValue(DisplayRecentColorsProperty, value);
		}
	}

	public Visibility StandardColorsArray
	{
		get
		{
			return (Visibility)GetValue(StandardColorsArrayProperty);
		}
		set
		{
			SetValue(StandardColorsArrayProperty, value);
		}
	}

	public ColorPicker()
	{
		InitializeComponent();
		DataContext = this;
	}
}
