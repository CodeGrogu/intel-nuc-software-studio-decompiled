using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using Newtonsoft.Json;
using NucLedMgr.WMI.Enums;
using NucLedMgr.WMI.Interfaces;
using NucLedMgr.WMI.Models.ColorTypes;

namespace NucLedMgr.WMI.Models.Indicators;

[JsonObject]
public class AlexaIndicator : IIndicator
{
	private readonly string _indicatorName = (string)Application.Current.Resources["AlexaIndicatorString"];

	[JsonProperty]
	public bool Enabled { get; set; }

	[JsonProperty]
	public bool MicEnabled { get; set; }

	[JsonProperty]
	public IColor Color { get; set; }

	public IndicatorType IndicatorType => IndicatorType.Alexa;

	public string IndicatorName => _indicatorName;

	public ColorType ColorType { get; set; }

	public IColor ColorSetting { get; set; }

	public List<ControlItem> ControlItems { get; set; }

	public event EventHandler<ControlItem> ControlItemChanged;

	private void UpdateControlItem(object sender, PropertyChangedEventArgs e)
	{
		OnControlItemChanged((ControlItem)sender);
	}

	protected virtual void OnControlItemChanged(ControlItem e)
	{
		ControlItemChanged?.Invoke(this, e);
	}

	public AlexaIndicator()
	{
		InitializeColor((Color)ColorConverter.ConvertFromString("#FF00CAFF"));
	}

	public List<ControlItem> GetControlItems()
	{
		return new List<ControlItem>();
	}

	public void SetControlItems(List<ControlItem> controlItems)
	{
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
