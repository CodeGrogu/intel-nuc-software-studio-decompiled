using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Newtonsoft.Json;
using NucLedMgr.WMI.Enums;
using NucLedMgr.WMI.Interfaces;

namespace NucLedMgr.WMI.Models.Indicators;

[JsonObject(MemberSerialization.OptIn)]
public class PowerStateIndicator_Single : IIndicator
{
	private readonly string _indicatorName = (string)Application.Current.Resources["PowerStateIndicatorString"];

	[JsonProperty]
	public PowerState_Single S0;

	[JsonProperty]
	public PowerState_Single S3;

	[JsonProperty]
	public bool S0Supported;

	[JsonProperty]
	public bool S3Supported;

	[JsonProperty]
	public IndicatorType IndicatorType => IndicatorType.PowerState;

	[JsonProperty]
	public ColorType ColorType => ColorType.SingleColor;

	public string IndicatorName => _indicatorName;

	[JsonProperty]
	public IColor ColorSetting { get; set; }

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

	public event EventHandler<ControlItem> ControlItemChanged;

	private void UpdateControlItem(object sender, PropertyChangedEventArgs e)
	{
		OnControlItemChanged((ControlItem)sender);
	}

	protected virtual void OnControlItemChanged(ControlItem e)
	{
		ControlItemChanged?.Invoke(this, e);
	}

	public PowerStateIndicator_Single(ColorType colorType, BitArray supportedControlItems, Color color)
	{
		S0 = new PowerState_Single(0, 1, supportedControlItems, color);
		S3 = new PowerState_Single(6, 7, supportedControlItems, color);
		S0Supported = S0.SupportedState;
		S3Supported = S3.SupportedState;
		S0.Brightness.PropertyChanged += UpdateControlItem;
		S0.Behavior.PropertyChanged += UpdateControlItem;
		S3.Brightness.PropertyChanged += UpdateControlItem;
		S3.Behavior.PropertyChanged += UpdateControlItem;
	}

	[JsonConstructor]
	public PowerStateIndicator_Single(ColorType colorType, List<ControlItem> controlItems)
	{
		Color blue = Colors.Blue;
		ControlItem brightness = controlItems.FirstOrDefault((ControlItem s) => s.Index == 0);
		ControlItem behavior = controlItems.FirstOrDefault((ControlItem s) => s.Index == 1);
		S0 = new PowerState_Single(behavior, brightness, blue);
		Color blue2 = Colors.Blue;
		ControlItem brightness2 = controlItems.FirstOrDefault((ControlItem s) => s.Index == 0);
		ControlItem behavior2 = controlItems.FirstOrDefault((ControlItem s) => s.Index == 1);
		S3 = new PowerState_Single(behavior2, brightness2, blue2);
	}

	public void SetControlItems(List<ControlItem> controlItems)
	{
		S0.SetControlItems(controlItems);
		S3.SetControlItems(controlItems);
	}

	public List<ControlItem> GetControlItems()
	{
		List<ControlItem> controlItems = S0.GetControlItems();
		controlItems.AddRange(S3.GetControlItems());
		return controlItems;
	}
}
