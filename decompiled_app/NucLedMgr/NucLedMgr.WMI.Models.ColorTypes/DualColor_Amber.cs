using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;
using Newtonsoft.Json;

namespace NucLedMgr.WMI.Models.ColorTypes;

[JsonObject]
public class DualColor_Amber : DualColor
{
	public DualColor_Amber(byte colorIndex, Color color, PropertyChangedEventHandler controlItemChanged)
	{
		DualColor.ColorBlue = Colors.Blue;
		DualColor.ColorWhiteAmber = Color.FromRgb(byte.MaxValue, 194, 0);
		SupportedColors = new List<Color>
		{
			DualColor.ColorBlue,
			DualColor.ColorWhiteAmber
		};
		ColorControlItem = new ControlItem(colorIndex, ControlItemFromColor(color), supported: true);
		ColorControlItem.PropertyChanged += controlItemChanged;
		_color = color;
	}

	public DualColor_Amber(ControlItem color, PropertyChangedEventHandler controlItemChanged)
	{
		SupportedColors = new List<Color>
		{
			DualColor.ColorBlue,
			DualColor.ColorWhiteAmber
		};
		ColorControlItem = color;
		if (ColorControlItem != null)
		{
			ColorControlItem.PropertyChanged += controlItemChanged;
			_color = ColorFromControlItem(ColorControlItem.Value);
		}
		else
		{
			_color = Colors.Transparent;
		}
	}

	[JsonConstructor]
	public DualColor_Amber()
	{
	}
}
