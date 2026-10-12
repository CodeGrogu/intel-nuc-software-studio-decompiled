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
using Prism.Mvvm;

namespace NucLedMgr.WMI.Models.Indicators;

[JsonObject(MemberSerialization.OptIn)]
public class WifiIndicator_Multi : BindableBase, IIndicator
{
	private bool brightnessSupported;

	private bool blueSupported;

	private bool greenSupported;

	private bool redSupported;

	public ControlItem Brightness;

	private readonly string _indicatorName = (string)Application.Current.Resources["WiFiIndicatorString"];

	[JsonProperty]
	public IndicatorType IndicatorType => IndicatorType.Wifi;

	[JsonProperty]
	public ColorType ColorType { get; private set; }

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

	public string IndicatorName => _indicatorName;

	public event EventHandler<ControlItem> ControlItemChanged;

	private void UpdateControlItem(object sender, PropertyChangedEventArgs e)
	{
		OnControlItemChanged((ControlItem)sender);
	}

	protected virtual void OnControlItemChanged(ControlItem e)
	{
		ControlItemChanged?.Invoke(this, e);
	}

	public WifiIndicator_Multi(ColorType colorType)
	{
		ColorType = colorType;
		Brightness.PropertyChanged += UpdateControlItem;
	}

	public WifiIndicator_Multi(ColorType colorType, BitArray supportedControlItems, Color color)
	{
		ColorType = colorType;
		InitializeControlItems(supportedControlItems);
		InitializeColor(color);
	}

	[JsonConstructor]
	public WifiIndicator_Multi(ColorType colorType, List<ControlItem> controlItems)
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
				Brightness = new ControlItem(controlItem.Index, 50, supported: true);
				controlItem.Value = 50;
				break;
			case 1:
				r = controlItem.Value;
				break;
			case 2:
				g = controlItem.Value;
				break;
			case 3:
				b = controlItem.Value;
				break;
			}
		}
		SetControlItems(controlItems);
		Color color = Color.FromRgb(r, g, b);
		InitializeColor(color);
	}

	public void SetControlItems(List<ControlItem> controlItems)
	{
		foreach (ControlItem controlItem in controlItems)
		{
			if (controlItem.Index == 0)
			{
				Brightness.Value = controlItem.Value;
				brightnessSupported = true;
			}
		}
		if (ColorSetting != null)
		{
			ColorSetting.SetColor(controlItems);
		}
	}

	public List<ControlItem> GetControlItems()
	{
		List<ControlItem> list = new List<ControlItem>();
		if (brightnessSupported)
		{
			list.Add(new ControlItem(Brightness.Index, Brightness.Value, brightnessSupported));
		}
		list.AddRange(ColorSetting.GetControlItems());
		return list;
	}

	private void InitializeControlItems(BitArray supportedItems)
	{
		for (int i = 0; i < supportedItems.Count; i++)
		{
			switch (i)
			{
			case 0:
				brightnessSupported = supportedItems[i];
				break;
			case 1:
				redSupported = supportedItems[i];
				break;
			case 2:
				greenSupported = supportedItems[i];
				break;
			case 3:
				blueSupported = supportedItems[i];
				break;
			}
		}
		Brightness = new ControlItem(0, brightnessSupported);
	}

	private void InitializeColor(Color color)
	{
		if (ColorType == ColorType.DualAmber)
		{
			ColorSetting = new DualColor_Amber(1, color, UpdateControlItem);
		}
		else if (ColorType == ColorType.DualWhite)
		{
			ColorSetting = new DualColor_White(1, color, UpdateControlItem);
		}
		else if (ColorType == ColorType.RGB)
		{
			ColorSetting = new RGBColor(1, 2, 3, color, UpdateControlItem);
		}
	}
}
