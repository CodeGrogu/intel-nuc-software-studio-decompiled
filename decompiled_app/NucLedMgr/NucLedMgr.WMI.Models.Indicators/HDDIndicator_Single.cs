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
public class HDDIndicator_Single : IIndicator
{
	public ControlItem Brightness;

	public ControlItem Behavior;

	private readonly string _indicatorName = (string)Application.Current.Resources["HDDIndicatorString"];

	[JsonProperty]
	public IndicatorType IndicatorType => IndicatorType.HddActivity;

	[JsonProperty]
	public ColorType ColorType => ColorType.SingleColor;

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

	public HDDIndicator_Single(ColorType colorType, BitArray supportedControlItems, Color color)
	{
		InitializeControlItems(supportedControlItems);
		InitializeColor(color);
	}

	[JsonConstructor]
	public HDDIndicator_Single(ColorType colorType, List<ControlItem> controlItems)
	{
		foreach (ControlItem controlItem in controlItems)
		{
			switch (controlItem.Index)
			{
			case 4:
				Behavior = new ControlItem(controlItem.Index, controlItem.Value, supported: true);
				break;
			case 0:
				Brightness = new ControlItem(controlItem.Index, 100, supported: true);
				break;
			}
		}
		Color blue = Colors.Blue;
		InitializeColor(blue);
	}

	public void SetControlItems(List<ControlItem> controlItems)
	{
		foreach (ControlItem controlItem in controlItems)
		{
			switch (controlItem.Index)
			{
			case 0:
				Brightness.Value = 100;
				break;
			case 4:
				Behavior.Value = controlItem.Value;
				break;
			}
		}
	}

	public List<ControlItem> GetControlItems()
	{
		List<ControlItem> list = new List<ControlItem>();
		if (Brightness.Supported)
		{
			list.Add(new ControlItem(Brightness.Index, 100, Brightness.Supported));
		}
		if (Behavior.Supported)
		{
			list.Add(new ControlItem(Behavior.Index, Behavior.Value, Behavior.Supported));
		}
		return list;
	}

	private void InitializeControlItems(BitArray supportedItems)
	{
		bool supported = false;
		bool supported2 = false;
		for (int i = 0; i < supportedItems.Count; i++)
		{
			switch (i)
			{
			case 4:
				supported = supportedItems[i];
				break;
			case 0:
				supported2 = supportedItems[i];
				break;
			}
		}
		Behavior = new ControlItem(4, supported);
		Behavior.PropertyChanged += UpdateControlItem;
		Brightness = new ControlItem(0, supported2);
		Brightness.PropertyChanged += UpdateControlItem;
	}

	private void InitializeColor(Color color)
	{
		ColorSetting = new SingleColor(color);
	}
}
