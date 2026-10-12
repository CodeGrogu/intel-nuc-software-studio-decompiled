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
internal class DisableIndicator : IIndicator
{
	private readonly string _indicatorName = (string)Application.Current.Resources["DisabledIndicatorString"];

	[JsonProperty]
	public IndicatorType IndicatorType
	{
		get
		{
			return IndicatorType.Disable;
		}
		set
		{
		}
	}

	public string IndicatorName => _indicatorName;

	public ColorType ColorType { get; set; }

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

	public DisableIndicator(ColorType colorType, BitArray supportedControlItems = null)
	{
		ColorType = colorType;
		ColorSetting = new RGBColor(0, 0, 0, Color.FromRgb(0, 0, 0), UpdateControlItem);
	}

	public List<ControlItem> GetControlItems()
	{
		return new List<ControlItem>();
	}

	public void SetControlItems(List<ControlItem> controlItems)
	{
		ControlItems.Clear();
	}
}
