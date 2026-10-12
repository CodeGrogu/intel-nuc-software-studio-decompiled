using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace NucLedMgr.WMI.Controls;

public partial class ColorPickerSerpent : UserControl, IComponentConnector
{
	private const string DISPLAY_RECENT_COLORS = "SerpentDisplayRecentColors";

	private const string STANDARD_COLORS_ARRAY = "SerpentStandardColorsArray";

	public static readonly DependencyProperty SerpentDisplayRecentColorsProperty = DependencyProperty.Register("SerpentDisplayRecentColors", typeof(Visibility), typeof(ColorPickerSerpent));

	public static readonly DependencyProperty SerpentStandardColorsArrayProperty = DependencyProperty.Register("SerpentStandardColorsArray", typeof(Visibility), typeof(ColorPickerSerpent));

	public Visibility SerpentDisplayRecentColors
	{
		get
		{
			return (Visibility)GetValue(SerpentDisplayRecentColorsProperty);
		}
		set
		{
			SetValue(SerpentDisplayRecentColorsProperty, value);
		}
	}

	public Visibility SerpentStandardColorsArray
	{
		get
		{
			return (Visibility)GetValue(SerpentStandardColorsArrayProperty);
		}
		set
		{
			SetValue(SerpentStandardColorsArrayProperty, value);
		}
	}

	public ColorPickerSerpent()
	{
		InitializeComponent();
		DataContext = this;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
