using System;
using System.Collections.Generic;
using NucLedMgr.WMI.Enums;
using NucLedMgr.WMI.Models;

namespace NucLedMgr.WMI.Interfaces;

public interface IIndicator
{
	IndicatorType IndicatorType { get; }

	string IndicatorName { get; }

	ColorType ColorType { get; }

	IColor ColorSetting { get; set; }

	List<ControlItem> ControlItems { get; set; }

	event EventHandler<ControlItem> ControlItemChanged;

	List<ControlItem> GetControlItems();

	void SetControlItems(List<ControlItem> controlItems);
}
