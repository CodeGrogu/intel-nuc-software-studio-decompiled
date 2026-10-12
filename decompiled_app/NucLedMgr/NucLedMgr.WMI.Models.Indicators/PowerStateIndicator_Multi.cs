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
using Prism.Mvvm;

namespace NucLedMgr.WMI.Models.Indicators;

[JsonObject(MemberSerialization.OptIn)]
public class PowerStateIndicator_Multi : BindableBase, IIndicator
{
	private readonly string _indicatorName = (string)Application.Current.Resources["PowerStateIndicatorString"];

	[JsonProperty]
	public PowerState_Multi S0;

	[JsonProperty]
	public PowerState_Multi S3;

	[JsonProperty]
	public PowerState_Multi ReadyMode;

	[JsonProperty]
	public PowerState_Multi S5;

	[JsonProperty]
	public bool S0Supported;

	[JsonProperty]
	public bool S3Supported;

	[JsonProperty]
	public bool ReadyModeSupported;

	[JsonProperty]
	public bool S5Supported;

	[JsonProperty]
	public IndicatorType IndicatorType => IndicatorType.PowerState;

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

	public PowerStateIndicator_Multi(ColorType colorType, BitArray supportedControlItems, Color color)
	{
		ColorType = colorType;
		S0 = new PowerState_Multi(0, 1, 2, 3, 4, 5, supportedControlItems, colorType, color, UpdateControlItem);
		S0Supported = S0.SupportedState;
		S3 = new PowerState_Multi(6, 7, 8, 9, 10, 11, supportedControlItems, colorType, color, UpdateControlItem);
		S3Supported = S3.SupportedState;
		ReadyMode = new PowerState_Multi(12, 13, 14, 15, 16, 17, supportedControlItems, colorType, color, UpdateControlItem);
		ReadyModeSupported = ReadyMode.SupportedState;
		S5 = new PowerState_Multi(18, 19, 20, 21, 22, 23, supportedControlItems, colorType, color, UpdateControlItem);
		S5Supported = S5.SupportedState;
		ControlItems = GetControlItems();
		InitializeColor(color);
	}

	[JsonConstructor]
	public PowerStateIndicator_Multi(ColorType colorType, List<ControlItem> controlItems, NucType nucType = NucType.Error, LedType ledType = LedType.Error)
	{
		ColorType = colorType;
		ControlItem controlItem = new ControlItem(0, supported: false);
		ControlItem behavior = new ControlItem(1, supported: false);
		ControlItem frequency = new ControlItem(2, supported: false);
		ControlItem controlItem2 = new ControlItem(3, supported: false);
		ControlItem controlItem3 = new ControlItem(4, supported: false);
		ControlItem controlItem4 = new ControlItem(5, supported: false);
		ControlItem controlItem5 = new ControlItem(6, supported: false);
		ControlItem behavior2 = new ControlItem(7, supported: false);
		ControlItem frequency2 = new ControlItem(8, supported: false);
		ControlItem controlItem6 = new ControlItem(9, supported: false);
		ControlItem controlItem7 = new ControlItem(10, supported: false);
		ControlItem controlItem8 = new ControlItem(11, supported: false);
		ControlItem controlItem9 = new ControlItem(12, supported: false);
		ControlItem behavior3 = new ControlItem(13, supported: false);
		ControlItem frequency3 = new ControlItem(14, supported: false);
		ControlItem controlItem10 = new ControlItem(15, supported: false);
		ControlItem controlItem11 = new ControlItem(16, supported: false);
		ControlItem controlItem12 = new ControlItem(17, supported: false);
		ControlItem controlItem13 = new ControlItem(18, supported: false);
		ControlItem behavior4 = new ControlItem(19, supported: false);
		ControlItem frequency4 = new ControlItem(20, supported: false);
		ControlItem controlItem14 = new ControlItem(21, supported: false);
		ControlItem controlItem15 = new ControlItem(22, supported: false);
		ControlItem controlItem16 = new ControlItem(23, supported: false);
		foreach (ControlItem controlItem17 in controlItems)
		{
			switch (controlItem17.Index)
			{
			case 0:
				controlItem = controlItem17;
				controlItem.Value = 50;
				break;
			case 1:
				behavior = controlItem17;
				break;
			case 2:
				frequency = controlItem17;
				break;
			case 3:
				controlItem2 = controlItem17;
				break;
			case 4:
				controlItem3 = controlItem17;
				break;
			case 5:
				controlItem4 = controlItem17;
				break;
			case 6:
				controlItem5 = controlItem17;
				controlItem5.Value = 50;
				break;
			case 7:
				behavior2 = controlItem17;
				break;
			case 8:
				frequency2 = controlItem17;
				break;
			case 9:
				controlItem6 = controlItem17;
				break;
			case 10:
				controlItem7 = controlItem17;
				break;
			case 11:
				controlItem8 = controlItem17;
				break;
			case 12:
				controlItem9 = controlItem17;
				controlItem9.Value = 50;
				break;
			case 13:
				behavior3 = controlItem17;
				break;
			case 14:
				frequency3 = controlItem17;
				break;
			case 15:
				controlItem10 = controlItem17;
				break;
			case 16:
				controlItem11 = controlItem17;
				break;
			case 17:
				controlItem12 = controlItem17;
				break;
			case 18:
				controlItem13 = controlItem17;
				controlItem13.Value = 50;
				break;
			case 19:
				behavior4 = controlItem17;
				break;
			case 20:
				frequency4 = controlItem17;
				break;
			case 21:
				controlItem14 = controlItem17;
				break;
			case 22:
				controlItem15 = controlItem17;
				break;
			case 23:
				controlItem16 = controlItem17;
				break;
			}
		}
		Color color;
		Color color2;
		if (nucType == NucType.Panther && ledType == LedType.Hdd)
		{
			color = EnumToColor(controlItem2.Value);
			color2 = EnumToColor(controlItem10.Value);
		}
		else
		{
			color = Color.FromRgb(controlItem2.Value, controlItem3.Value, controlItem4.Value);
			color2 = Color.FromRgb(controlItem10.Value, controlItem11.Value, controlItem12.Value);
		}
		S0 = new PowerState_Multi(controlItem, behavior, frequency, 3, 4, 5, colorType, color, UpdateControlItem);
		S0Supported = S0.SupportedState;
		S3 = new PowerState_Multi(controlItem5, behavior2, frequency2, 9, 10, 11, colorType, Color.FromRgb(controlItem6.Value, controlItem7.Value, controlItem8.Value), UpdateControlItem);
		S3Supported = S3.SupportedState;
		ReadyMode = new PowerState_Multi(controlItem9, behavior3, frequency3, 15, 16, 17, colorType, color2, UpdateControlItem);
		ReadyModeSupported = ReadyMode.SupportedState;
		S5 = new PowerState_Multi(controlItem13, behavior4, frequency4, 21, 22, 23, colorType, Color.FromRgb(controlItem14.Value, controlItem15.Value, controlItem16.Value), UpdateControlItem);
		S5Supported = S5.SupportedState;
		ControlItems = controlItems;
		if (nucType == NucType.Panther && ledType == LedType.Hdd)
		{
			ReadyMode.ColorSetting.CurrentColor = color2;
		}
		InitializeColor(color);
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

	public List<ControlItem> GetControlItems()
	{
		List<ControlItem> controlItems = S0.GetControlItems();
		controlItems.AddRange(S3.GetControlItems());
		controlItems.AddRange(ReadyMode.GetControlItems());
		controlItems.AddRange(S5.GetControlItems());
		return controlItems;
	}

	public void SetControlItems(List<ControlItem> controlItems)
	{
		if (S0Supported)
		{
			S0.SetControlItems(controlItems);
		}
		if (S3Supported)
		{
			S3.SetControlItems(controlItems);
		}
		if (ReadyModeSupported)
		{
			ReadyMode.SetControlItems(controlItems);
		}
		if (S5Supported)
		{
			S5.SetControlItems(controlItems);
		}
	}
}
