using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using FeatureMgr.Enums;
using Newtonsoft.Json;
using NucLedMgr.WMI.Enums;
using NucLedMgr.WMI.Interfaces;
using NucLedMgr.WMI.Models.ColorTypes;

namespace NucLedMgr.WMI.Models.Indicators;

[JsonObject(MemberSerialization.OptIn)]
public class HDDIndicator_Multi : IIndicator
{
	[JsonProperty]
	public ControlItem Brightness;

	[JsonProperty]
	public ControlItem Behavior;

	private readonly string _indicatorName = (string)Application.Current.Resources["HDDIndicatorString"];

	private bool behaviorSupported;

	private bool brightnessSupported;

	[JsonProperty]
	public IndicatorType IndicatorType => IndicatorType.HddActivity;

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

	public HDDIndicator_Multi(ColorType colorType, BitArray supportedControlItems, Color color)
	{
		ColorType = colorType;
		InitializeControlItems(supportedControlItems);
		InitializeColor(color);
	}

	[JsonConstructor]
	public HDDIndicator_Multi(ColorType colorType, List<ControlItem> controlItems, NucType nucType = NucType.Error)
	{
		byte b = byte.MaxValue;
		byte b2 = byte.MaxValue;
		byte g = byte.MaxValue;
		ColorType = colorType;
		foreach (ControlItem controlItem in controlItems)
		{
			switch (controlItem.Index)
			{
			case 4:
				Behavior = new ControlItem(controlItem.Index, controlItem.Value, supported: true);
				behaviorSupported = true;
				break;
			case 0:
				Brightness = new ControlItem(controlItem.Index, 50, supported: true);
				brightnessSupported = true;
				break;
			case 1:
				b = controlItem.Value;
				break;
			case 2:
				g = controlItem.Value;
				break;
			case 3:
				b2 = controlItem.Value;
				break;
			}
		}
		InitializeColor((nucType != NucType.Panther) ? Color.FromRgb(b, g, b2) : EnumToColor(b));
	}

	public Color EnumToColor(int value)
	{
		return value switch
		{
			1 => Colors.Red, 
			2 => Colors.Orange, 
			3 => Colors.Yellow, 
			4 => Colors.Green, 
			5 => Colors.Blue, 
			6 => Colors.Indigo, 
			7 => Colors.Violet, 
			8 => Colors.White, 
			_ => Colors.Orange, 
		};
	}

	private void InitializeControlItems(BitArray supportedItems)
	{
		if (4 < supportedItems.Count)
		{
			behaviorSupported = supportedItems[4];
		}
		if (0 < supportedItems.Count)
		{
			brightnessSupported = supportedItems[0];
		}
		Behavior = new ControlItem(4, behaviorSupported);
		Behavior.PropertyChanged += UpdateControlItem;
		Brightness = new ControlItem(0, brightnessSupported);
		Brightness.PropertyChanged += UpdateControlItem;
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

	public void SetControlItems(List<ControlItem> controlItems)
	{
		foreach (ControlItem controlItem in controlItems)
		{
			switch (controlItem.Index)
			{
			case 0:
				Brightness.Value = controlItem.Value;
				break;
			case 4:
				if (Behavior == null)
				{
					Behavior = new ControlItem(4, supported: true);
					Behavior.PropertyChanged += UpdateControlItem;
				}
				Behavior.Value = controlItem.Value;
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
			list.Add(new ControlItem(Brightness.Index, 100, Brightness.Supported));
		}
		if (behaviorSupported)
		{
			list.Add(new ControlItem(Behavior.Index, Behavior.Value, Behavior.Supported));
		}
		list.AddRange(ColorSetting.GetControlItems());
		return list;
	}
}
