using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using Newtonsoft.Json;
using NucLedMgr.WMI.Interfaces;
using Prism.Mvvm;

namespace NucLedMgr.WMI.Models.ColorTypes;

[JsonObject]
public class RGBColor : BindableBase, IColor
{
	private Color _color;

	[JsonProperty]
	public ControlItem Red;

	[JsonProperty]
	public ControlItem Green;

	[JsonProperty]
	public ControlItem Blue;

	[JsonProperty]
	public Color CurrentColor
	{
		get
		{
			return _color;
		}
		set
		{
			UpdateControlItems(value);
			SetProperty(ref _color, value, "CurrentColor");
		}
	}

	[JsonProperty]
	public List<Color> SupportedColors { get; }

	public RGBColor(byte redIndex, byte greenIndex, byte blueIndex, Color color, PropertyChangedEventHandler controlItemChanged)
	{
		Red = new ControlItem(redIndex, color.R, supported: true);
		Red.PropertyChanged += controlItemChanged;
		Green = new ControlItem(greenIndex, color.G, supported: true);
		Green.PropertyChanged += controlItemChanged;
		Blue = new ControlItem(blueIndex, color.B, supported: true);
		Blue.PropertyChanged += controlItemChanged;
		_color = color;
		SupportedColors = new List<Color>();
	}

	public RGBColor(ControlItem red, ControlItem green, ControlItem blue, PropertyChangedEventHandler controlItemChanged)
	{
		Red = red;
		if (Red != null)
		{
			Red.PropertyChanged += controlItemChanged;
		}
		Green = green;
		if (Green != null)
		{
			Green.PropertyChanged += controlItemChanged;
		}
		Blue = blue;
		if (Blue != null)
		{
			Blue.PropertyChanged += controlItemChanged;
		}
		if (Red != null && Green != null && Blue != null)
		{
			_color = Color.FromRgb(Red.Value, Green.Value, Blue.Value);
		}
	}

	public RGBColor(ControlItem red, ControlItem green, ControlItem blue)
	{
		Red = red;
		Green = green;
		Blue = blue;
		if (Red != null && Green != null && Blue != null)
		{
			_color = Color.FromRgb(Red.Value, Green.Value, Blue.Value);
		}
	}

	[JsonConstructor]
	public RGBColor()
	{
	}

	private void UpdateControlItems(Color newColor)
	{
		if (newColor.R != Red.Value)
		{
			Red.Value = newColor.R;
		}
		if (newColor.G != Green.Value)
		{
			Green.Value = newColor.G;
		}
		if (newColor.B != Blue.Value)
		{
			Blue.Value = newColor.B;
		}
	}

	public void SetColor(List<ControlItem> controlItems)
	{
		ControlItem controlItem = controlItems.FirstOrDefault((ControlItem i) => i.Index == Red.Index);
		ControlItem controlItem2 = controlItems.FirstOrDefault((ControlItem i) => i.Index == Green.Index);
		ControlItem controlItem3 = controlItems.FirstOrDefault((ControlItem i) => i.Index == Blue.Index);
		byte r = 0;
		byte g = 0;
		byte b = 0;
		if (controlItem != null)
		{
			r = controlItem.Value;
		}
		if (controlItem2 != null)
		{
			g = controlItem2.Value;
		}
		if (controlItem3 != null)
		{
			b = controlItem3.Value;
		}
		CurrentColor = Color.FromRgb(r, g, b);
	}

	public List<ControlItem> GetControlItems()
	{
		List<ControlItem> list = new List<ControlItem>();
		if (Red.Supported)
		{
			list.Add(new ControlItem(Red.Index, Red.Value, Red.Supported));
		}
		if (Green.Supported)
		{
			list.Add(new ControlItem(Green.Index, Green.Value, Green.Supported));
		}
		if (Blue.Supported)
		{
			list.Add(new ControlItem(Blue.Index, Blue.Value, Blue.Supported));
		}
		return list;
	}
}
