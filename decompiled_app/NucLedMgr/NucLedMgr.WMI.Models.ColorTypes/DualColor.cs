using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Newtonsoft.Json;
using NucLedMgr.WMI.Interfaces;
using Prism.Mvvm;

namespace NucLedMgr.WMI.Models.ColorTypes;

[JsonObject]
public abstract class DualColor : BindableBase, IColor
{
	public const byte BLUE = 0;

	public const byte WHITE_AMBER = 1;

	protected Color _color;

	[JsonProperty]
	public static Color ColorBlue;

	[JsonProperty]
	public static Color ColorWhiteAmber;

	[JsonProperty]
	public ControlItem ColorControlItem;

	[JsonProperty]
	public Color CurrentColor
	{
		get
		{
			return _color;
		}
		set
		{
			UpdateControlItem(value);
			SetProperty(ref _color, value, "CurrentColor");
		}
	}

	[JsonProperty]
	public List<Color> SupportedColors { get; protected set; }

	public void SetColor(List<ControlItem> controlItems)
	{
		ControlItem controlItem = controlItems.First((ControlItem i) => i.Index == ColorControlItem.Index);
		if (controlItem != null)
		{
			CurrentColor = ColorFromControlItem(controlItem.Value);
		}
	}

	protected void UpdateControlItem(Color color)
	{
		ColorControlItem.Value = ControlItemFromColor(color);
	}

	public List<ControlItem> GetControlItems()
	{
		List<ControlItem> list = new List<ControlItem>();
		if (ColorControlItem.Supported)
		{
			list.Add(new ControlItem(ColorControlItem.Index, ColorControlItem.Value, ColorControlItem.Supported));
		}
		return list;
	}

	protected Color ColorFromControlItem(byte Value)
	{
		return Value switch
		{
			0 => ColorBlue, 
			1 => ColorWhiteAmber, 
			_ => Colors.Transparent, 
		};
	}

	protected byte ControlItemFromColor(Color color)
	{
		if (color == ColorBlue)
		{
			return 0;
		}
		if (color == ColorWhiteAmber)
		{
			return 1;
		}
		return byte.MaxValue;
	}
}
