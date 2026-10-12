using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;
using Newtonsoft.Json;

namespace NucLedMgr.WMI.Models.ColorTypes;

[JsonObject]
public class DualColor_White : DualColor
{
	public DualColor_White(byte colorIndex, Color color, PropertyChangedEventHandler controlItemChanged)
	{
		DualColor.ColorBlue = Colors.Blue;
		DualColor.ColorWhiteAmber = Colors.White;
		SupportedColors = new List<Color>
		{
			DualColor.ColorBlue,
			DualColor.ColorWhiteAmber
		};
		ColorControlItem = new ControlItem(colorIndex, ControlItemFromColor(color), supported: true);
		ColorControlItem.PropertyChanged += controlItemChanged;
		_color = color;
	}

	public DualColor_White(ControlItem color, PropertyChangedEventHandler controlItemChanged)
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
	public DualColor_White()
	{
	}
}
