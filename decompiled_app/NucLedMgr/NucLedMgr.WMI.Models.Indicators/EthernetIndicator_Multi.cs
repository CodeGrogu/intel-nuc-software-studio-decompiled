using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using Newtonsoft.Json;
using NucLedMgr.WMI.Enums;
using NucLedMgr.WMI.Interfaces;
using NucLedMgr.WMI.Models.ColorTypes;

namespace NucLedMgr.WMI.Models.Indicators;

[JsonObject(MemberSerialization.OptIn)]
public class EthernetIndicator_Multi : IIndicator
{
	private readonly string _indicatorName = (string)Application.Current.Resources["EthernetIndicatorString"];

	public ControlItem Type;

	public ControlItem Brightness;

	private bool typeSupported;

	private bool brightnessSupported;

	[JsonProperty]
	public IndicatorType IndicatorType
	{
		get
		{
			return IndicatorType.Ethernet;
		}
		set
		{
		}
	}

	[JsonProperty]
	public ColorType ColorType { get; set; }

	public string IndicatorName => _indicatorName;

	[JsonProperty]
	public List<ControlItem> ControlItems
	{
		get
		{
			return GetControlItems();
		}
		set
		{
			SetControlItems(value);
		}
	}

	[JsonProperty]
	public IColor ColorSetting { get; set; }

	public event EventHandler<ControlItem> ControlItemChanged;

	private void UpdateControlItem(object sender, PropertyChangedEventArgs e)
	{
		OnControlItemChanged((ControlItem)sender);
	}

	protected virtual void OnControlItemChanged(ControlItem e)
	{
		ControlItemChanged?.Invoke(this, e);
	}

	public EthernetIndicator_Multi(ColorType colorType, BitArray supportedControlItems, Color color)
	{
		ColorType = colorType;
		InitializeControlItems(supportedControlItems);
		InitializeColor(color);
	}

	[JsonConstructor]
	public EthernetIndicator_Multi(ColorType colorType, List<ControlItem> controlItems)
	{
		ColorType = colorType;
		byte r = byte.MaxValue;
		byte b = byte.MaxValue;
		byte g = byte.MaxValue;
		foreach (ControlItem controlItem in controlItems)
		{
			switch (controlItem.Index)
			{
			case 0:
				Type = new ControlItem(controlItem.Index, controlItem.Value, supported: true);
				typeSupported = true;
				break;
			case 1:
				Brightness = new ControlItem(controlItem.Index, 50, supported: true);
				brightnessSupported = true;
				break;
			case 2:
				r = controlItem.Value;
				break;
			case 3:
				g = controlItem.Value;
				break;
			case 4:
				b = controlItem.Value;
				break;
			}
		}
		Color color = Color.FromRgb(r, g, b);
		InitializeColor(color);
	}

	private void InitializeControlItems(BitArray supportedItems)
	{
		if (0 < supportedItems.Count)
		{
			typeSupported = supportedItems[0];
		}
		if (1 < supportedItems.Count)
		{
			brightnessSupported = supportedItems[1];
		}
		Type = new ControlItem(0, typeSupported);
		Type.PropertyChanged += UpdateControlItem;
		Brightness = new ControlItem(1, brightnessSupported);
		Brightness.PropertyChanged += UpdateControlItem;
	}

	private void InitializeColor(Color color)
	{
		if (ColorType == ColorType.DualAmber)
		{
			ColorSetting = new DualColor_Amber(2, color, UpdateControlItem);
		}
		else if (ColorType == ColorType.DualWhite)
		{
			ColorSetting = new DualColor_White(2, color, UpdateControlItem);
		}
		else if (ColorType == ColorType.RGB)
		{
			ColorSetting = new RGBColor(2, 3, 4, color, UpdateControlItem);
		}
	}

	public void SetControlItems(List<ControlItem> controlItems)
	{
		ControlItem controlItem = null;
		ControlItem green = null;
		ControlItem blue = null;
		foreach (ControlItem controlItem2 in controlItems)
		{
			switch (controlItem2.Index)
			{
			case 1:
				Brightness = controlItem2;
				break;
			case 0:
				Type = controlItem2;
				break;
			case 2:
				controlItem = controlItem2;
				break;
			case 4:
				blue = controlItem2;
				break;
			case 3:
				green = controlItem2;
				break;
			}
		}
		switch (ColorType)
		{
		case ColorType.DualAmber:
			ColorSetting = new DualColor_Amber(controlItem, UpdateControlItem);
			break;
		case ColorType.DualWhite:
			ColorSetting = new DualColor_White(controlItem, UpdateControlItem);
			break;
		case ColorType.RGB:
			ColorSetting = new RGBColor(controlItem, green, blue, UpdateControlItem);
			break;
		}
	}

	public List<ControlItem> GetControlItems()
	{
		List<ControlItem> list = new List<ControlItem>();
		if (brightnessSupported)
		{
			list.Add(new ControlItem(Brightness.Index, Brightness.Value, Brightness.Supported));
		}
		if (typeSupported)
		{
			list.Add(new ControlItem(Type.Index, Type.Value, Type.Supported));
		}
		list.AddRange(ColorSetting.GetControlItems());
		return list;
	}
}
