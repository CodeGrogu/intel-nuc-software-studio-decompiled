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
public class SoftwareIndicator_Multi : IIndicator
{
	private bool behaviorSupported;

	private bool brightnessSupported;

	private bool frequencySupported;

	private static readonly string _indicatorName = (string)Application.Current.Resources["SoftwareIndicatorString"];

	[JsonProperty]
	public IndicatorType IndicatorType => IndicatorType.Software;

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

	public ControlItem Brightness { get; set; }

	public ControlItem Frequency { get; set; }

	public ControlItem Behavior { get; set; }

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

	public SoftwareIndicator_Multi(ColorType colorType, BitArray supportedControlItems, Color color)
	{
		ColorType = colorType;
		InitializeControlItems(supportedControlItems);
		InitializeColor(color);
	}

	[JsonConstructor]
	public SoftwareIndicator_Multi(ColorType colorType, List<ControlItem> controlItems)
	{
		byte r = byte.MaxValue;
		byte b = byte.MaxValue;
		byte g = byte.MaxValue;
		foreach (ControlItem controlItem in controlItems)
		{
			switch (controlItem.Index)
			{
			case 1:
				Behavior = new ControlItem(controlItem.Index, controlItem.Value, supported: true);
				Behavior.PropertyChanged += UpdateControlItem;
				break;
			case 0:
				Brightness = new ControlItem(controlItem.Index, 100, supported: true);
				Brightness.PropertyChanged += UpdateControlItem;
				break;
			case 2:
				Frequency = new ControlItem(controlItem.Index, controlItem.Value, supported: true);
				Frequency.PropertyChanged += UpdateControlItem;
				break;
			case 3:
				r = controlItem.Value;
				break;
			case 4:
				g = controlItem.Value;
				break;
			case 5:
				b = controlItem.Value;
				break;
			}
		}
		Color color = Color.FromRgb(r, g, b);
		InitializeColor(color);
	}

	private void InitializeControlItems(BitArray supportedItems)
	{
		if (1 < supportedItems.Count)
		{
			behaviorSupported = supportedItems[1];
		}
		if (0 < supportedItems.Count)
		{
			brightnessSupported = supportedItems[0];
		}
		if (2 < supportedItems.Count)
		{
			frequencySupported = supportedItems[2];
		}
		Behavior = new ControlItem(1, behaviorSupported);
		Behavior.PropertyChanged += UpdateControlItem;
		Brightness = new ControlItem(0, brightnessSupported);
		Brightness.PropertyChanged += UpdateControlItem;
		Frequency = new ControlItem(2, frequencySupported);
		Frequency.PropertyChanged += UpdateControlItem;
	}

	private void InitializeColor(Color color)
	{
		if (ColorType == ColorType.DualAmber)
		{
			ColorSetting = new DualColor_Amber(3, color, UpdateControlItem);
		}
		else if (ColorType == ColorType.DualWhite)
		{
			ColorSetting = new DualColor_White(3, color, UpdateControlItem);
		}
		else if (ColorType == ColorType.RGB)
		{
			ColorSetting = new RGBColor(3, 4, 5, color, UpdateControlItem);
		}
	}

	public void SetControlItems(List<ControlItem> controlItems)
	{
		foreach (ControlItem controlItem in controlItems)
		{
			switch (controlItem.Index)
			{
			case 0:
				Brightness.Value = 50;
				brightnessSupported = true;
				break;
			case 1:
				Behavior.Value = controlItem.Value;
				behaviorSupported = true;
				break;
			case 2:
				Frequency.Value = controlItem.Value;
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
		if (behaviorSupported)
		{
			list.Add(new ControlItem(Behavior.Index, Behavior.Value, Behavior.Supported));
		}
		if (frequencySupported)
		{
			list.Add(new ControlItem(Frequency.Index, Frequency.Value, Frequency.Supported));
		}
		list.AddRange(ColorSetting.GetControlItems());
		return list;
	}
}
