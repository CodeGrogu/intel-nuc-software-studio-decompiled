using System;
using Infrastructure.Converters;
using Prism.Mvvm;
using UniwillSdkClient.UniwillService;

namespace MobileBatterySettingsMgr.Models;

public class BatteryHealthMode : BindableBase
{
	private Action<BatteryMode> _setMode;

	private BatteryMode _type;

	private bool _value = true;

	public string Label => _type switch
	{
		BatteryMode.BestBattery => ResourceDictionaryConverter.GetString("BatteryHealth_Best"), 
		BatteryMode.Custom => ResourceDictionaryConverter.GetString("BatteryHealth_Custom"), 
		BatteryMode.AcMode => ResourceDictionaryConverter.GetString("BatteryHealth_MaxCharge"), 
		_ => _type.ToString(), 
	};

	public BatteryMode Type => _type;

	public bool Value
	{
		get
		{
			return _value;
		}
		set
		{
			SetProperty(ref _value, value, "Value");
			if (_value)
			{
				_setMode?.Invoke(_type);
			}
		}
	}

	public BatteryHealthMode(BatteryMode modeType, bool value, Action<BatteryMode> setMode)
	{
		_setMode = setMode;
		_type = modeType;
		_value = value;
	}
}
