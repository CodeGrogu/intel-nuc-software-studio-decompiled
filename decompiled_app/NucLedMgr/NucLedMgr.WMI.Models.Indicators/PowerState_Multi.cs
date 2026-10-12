using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Media;
using Newtonsoft.Json;
using NucLedMgr.WMI.Enums;
using NucLedMgr.WMI.Interfaces;
using NucLedMgr.WMI.Models.ColorTypes;

namespace NucLedMgr.WMI.Models.Indicators;

[JsonObject]
public class PowerState_Multi
{
	[JsonProperty]
	public ControlItem Brightness;

	[JsonProperty]
	public ControlItem Frequency;

	[JsonProperty]
	public ControlItem Behavior;

	[JsonProperty]
	public bool SupportedState;

	private bool behaviorSupported;

	private bool brightnessSupported;

	private bool frequencySupported;

	[JsonProperty]
	public IColor ColorSetting { get; set; }

	public PowerState_Multi(byte brightnessIndex, byte behaviorIndex, byte frequencyIndex, byte colorRedIndex, byte greenIndex, byte blueIndex, BitArray supportedItems, ColorType colorType, Color color, PropertyChangedEventHandler controlItemChanged)
	{
		InitializeControlItems(brightnessIndex, behaviorIndex, frequencyIndex, supportedItems);
		InitializeColor(colorRedIndex, greenIndex, blueIndex, colorType, color, controlItemChanged);
		Brightness.PropertyChanged += controlItemChanged;
		Behavior.PropertyChanged += controlItemChanged;
		Frequency.PropertyChanged += controlItemChanged;
	}

	public PowerState_Multi(ControlItem brightness, ControlItem behavior, ControlItem frequency, byte colorRedIndex, byte greenIndex, byte blueIndex, ColorType colorType, Color color, PropertyChangedEventHandler controlItemChanged)
	{
		Brightness = brightness;
		Behavior = behavior;
		Frequency = frequency;
		Brightness.PropertyChanged += controlItemChanged;
		Behavior.PropertyChanged += controlItemChanged;
		Frequency.PropertyChanged += controlItemChanged;
		SupportedState = Brightness.Supported && Behavior.Supported && Frequency.Supported;
		InitializeColor(colorRedIndex, greenIndex, blueIndex, colorType, color, controlItemChanged);
	}

	[JsonConstructor]
	public PowerState_Multi()
	{
	}

	private void InitializeControlItems(byte brightnessIndex, byte behaviorIndex, byte frequencyIndex, BitArray supportedItems)
	{
		if (behaviorIndex < supportedItems.Count)
		{
			behaviorSupported = supportedItems[behaviorIndex];
		}
		if (brightnessIndex < supportedItems.Count)
		{
			brightnessSupported = supportedItems[brightnessIndex];
		}
		if (frequencyIndex < supportedItems.Count)
		{
			frequencySupported = supportedItems[frequencyIndex];
		}
		Behavior = new ControlItem(behaviorIndex, behaviorSupported);
		Brightness = new ControlItem(brightnessIndex, brightnessSupported);
		Frequency = new ControlItem(frequencyIndex, frequencySupported);
		SupportedState = behaviorSupported || brightnessSupported || frequencySupported;
	}

	private void InitializeColor(byte colorRedIndex, byte greenIndex, byte blueIndex, ColorType colorType, Color color, PropertyChangedEventHandler controlItemChanged)
	{
		switch (colorType)
		{
		case ColorType.DualAmber:
			ColorSetting = new DualColor_Amber(colorRedIndex, color, controlItemChanged);
			break;
		case ColorType.DualWhite:
			ColorSetting = new DualColor_White(colorRedIndex, color, controlItemChanged);
			break;
		case ColorType.RGB:
			ColorSetting = new RGBColor(colorRedIndex, greenIndex, blueIndex, color, controlItemChanged);
			break;
		}
	}

	public List<ControlItem> GetControlItems()
	{
		List<ControlItem> list = new List<ControlItem>();
		if (Brightness != null)
		{
			list.Add(new ControlItem(Brightness.Index, Brightness.Value, Brightness.Supported));
		}
		if (Behavior != null)
		{
			list.Add(new ControlItem(Behavior.Index, Behavior.Value, Behavior.Supported));
		}
		if (Frequency != null)
		{
			list.Add(new ControlItem(Frequency.Index, Frequency.Value, Frequency.Supported));
		}
		list.AddRange(ColorSetting.GetControlItems());
		return list;
	}

	public void SetControlItems(List<ControlItem> controlItems)
	{
		Task.Run(() =>
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
				else if (controlItem.Index == Frequency.Index)
				{
					Frequency.Value = controlItem.Value;
					num++;
				}
				if (num >= 3)
				{
					break;
				}
			}
		});
		ColorSetting.SetColor(controlItems);
	}
}
