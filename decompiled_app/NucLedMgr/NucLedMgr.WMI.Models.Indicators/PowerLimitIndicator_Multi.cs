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
public class PowerLimitIndicator_Multi : IIndicator
{
	private readonly string _indicatorName = (string)Application.Current.Resources["PowerLimitIndicatorString"];

	public ControlItem Brightness;

	public ControlItem Indication;

	private bool indicationSupported;

	private bool brightnessSupported;

	[JsonProperty]
	public IndicatorType IndicatorType => IndicatorType.PowerLimit;

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

	public string IndicatorName => _indicatorName;

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

	public PowerLimitIndicator_Multi(ColorType colorType, BitArray supportedControlItems, Color color)
	{
		ColorType = colorType;
		InitializeControlItems(supportedControlItems);
		InitializeColor(color);
	}

	[JsonConstructor]
	public PowerLimitIndicator_Multi(ColorType colorType, List<ControlItem> controlItems, IColor colorSetting)
	{
		foreach (ControlItem controlItem in controlItems)
		{
			switch (controlItem.Index)
			{
			case 0:
				Indication = new ControlItem(controlItem.Index, controlItem.Value, supported: true);
				indicationSupported = true;
				break;
			case 1:
				Brightness = new ControlItem(controlItem.Index, 50, supported: true);
				brightnessSupported = true;
				break;
			}
		}
		ColorType = colorType;
		ColorSetting = colorSetting;
		ControlItems = controlItems;
	}

	public PowerLimitIndicator_Multi(ColorType colorType, List<ControlItem> controlItems)
	{
		byte r = byte.MaxValue;
		byte b = byte.MaxValue;
		byte g = byte.MaxValue;
		ColorType = colorType;
		foreach (ControlItem controlItem in controlItems)
		{
			switch (controlItem.Index)
			{
			case 0:
				Indication = new ControlItem(controlItem.Index, controlItem.Value, supported: true);
				indicationSupported = true;
				break;
			case 1:
				Brightness = new ControlItem(controlItem.Index, 100, supported: true);
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

	public void SetControlItems(List<ControlItem> controlItems)
	{
		foreach (ControlItem controlItem in controlItems)
		{
			switch (controlItem.Index)
			{
			case 1:
				Brightness.Value = controlItem.Value;
				break;
			case 0:
				Indication.Value = controlItem.Value;
				break;
			}
		}
		ColorSetting.SetColor(controlItems);
	}

	public List<ControlItem> GetControlItems()
	{
		List<ControlItem> list = new List<ControlItem>();
		if (brightnessSupported)
		{
			list.Add(new ControlItem(Brightness.Index, Brightness.Value, Brightness.Supported));
		}
		if (indicationSupported)
		{
			list.Add(new ControlItem(Indication.Index, Indication.Value, Indication.Supported));
		}
		if (ColorSetting != null)
		{
			list.AddRange(ColorSetting.GetControlItems());
		}
		return list;
	}

	private void InitializeControlItems(BitArray supportedItems)
	{
		if (0 < supportedItems.Count)
		{
			indicationSupported = supportedItems[0];
		}
		if (1 < supportedItems.Count)
		{
			brightnessSupported = supportedItems[1];
		}
		Indication = new ControlItem(0, indicationSupported);
		Indication.PropertyChanged += UpdateControlItem;
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
}
