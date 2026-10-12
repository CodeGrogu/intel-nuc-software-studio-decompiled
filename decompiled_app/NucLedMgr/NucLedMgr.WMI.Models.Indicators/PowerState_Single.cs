using System.Collections;
using System.Collections.Generic;
using System.Windows.Media;
using Newtonsoft.Json;
using NucLedMgr.WMI.Interfaces;
using NucLedMgr.WMI.Models.ColorTypes;

namespace NucLedMgr.WMI.Models.Indicators;

[JsonObject]
public class PowerState_Single
{
	[JsonProperty]
	public ControlItem Brightness;

	[JsonProperty]
	public ControlItem Behavior;

	[JsonProperty]
	public bool SupportedState;

	[JsonProperty]
	public IColor Color { get; set; }

	public PowerState_Single(byte brightnessIndex, byte behaviorIndex, BitArray supportedItems, Color color)
	{
		InitializeControlItems(brightnessIndex, behaviorIndex, supportedItems, color);
	}

	[JsonConstructor]
	public PowerState_Single(ControlItem behavior, ControlItem brightness, Color color)
	{
		Behavior = new ControlItem(behavior.Index, behavior.Value, supported: true);
		Behavior = new ControlItem(brightness.Index, brightness.Value, supported: true);
		Color = new SingleColor(color);
	}

	public List<ControlItem> GetControlItems()
	{
		List<ControlItem> list = new List<ControlItem>();
		if (Brightness.Supported)
		{
			list.Add(new ControlItem(Brightness.Index, Brightness.Value, Brightness.Supported));
		}
		if (Behavior.Supported)
		{
			list.Add(new ControlItem(Behavior.Index, Behavior.Value, Behavior.Supported));
		}
		return list;
	}

	public void SetControlItems(List<ControlItem> controlItems)
	{
		int num = 0;
		foreach (ControlItem controlItem in controlItems)
		{
			if (controlItem.Index == Brightness.Index)
			{
				Brightness.Value = controlItem.Value;
				num++;
			}
			else if (controlItem.Index == Behavior.Index)
			{
				Behavior.Value = controlItem.Value;
				num++;
			}
			if (num == 2)
			{
				break;
			}
		}
	}

	private void InitializeControlItems(byte brightnessIndex, byte behaviorIndex, BitArray supportedItems, Color color)
	{
		bool supported = false;
		bool supported2 = false;
		if (behaviorIndex < supportedItems.Count)
		{
			supported = supportedItems[behaviorIndex];
		}
		if (brightnessIndex < supportedItems.Count)
		{
			supported2 = supportedItems[brightnessIndex];
		}
		Behavior = new ControlItem(behaviorIndex, supported);
		Brightness = new ControlItem(brightnessIndex, supported2);
		SupportedState = Brightness.Supported && Behavior.Supported;
		Color = new SingleColor(color);
	}
}
