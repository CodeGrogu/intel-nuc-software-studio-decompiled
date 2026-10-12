using System;
using Infrastructure.Converters;
using MobileKeyboardSettingsMgr.Interface;
using Prism.Mvvm;
using UniwillSdkClient.UniwillService;

namespace MobileKeyboardSettingsMgr.Models;

internal class KeyStateModel : BindableBase, ISystemSetting
{
	private Action<KeyState> _setState;

	private KeyType _type;

	private bool _value = true;

	private bool _invert;

	public string Label => _type switch
	{
		KeyType.WinKey => ResourceDictionaryConverter.GetString("WinKeyString"), 
		KeyType.DisableTouchPadToggle => ResourceDictionaryConverter.GetString("TrackPadString"), 
		KeyType.FnKey => ResourceDictionaryConverter.GetString("FunctionKeyLockString"), 
		KeyType.NumPad => ResourceDictionaryConverter.GetString("NumPadString"), 
		_ => _type.ToString(), 
	};

	public KeyType Type => _type;

	public bool Value
	{
		get
		{
			return _value;
		}
		set
		{
			SetProperty(ref _value, _invert ? (!value) : value, "Value");
			_setState?.Invoke(new KeyState
			{
				Type = _type,
				State = _value
			});
		}
	}

	public KeyStateModel(KeyType type, bool value, Action<KeyState> setState)
	{
		_type = type;
		_value = value;
		_setState = setState;
	}

	public KeyStateModel(KeyType type, bool value, bool invert, Action<KeyState> setState)
		: this(type, invert ? (!value) : value, setState)
	{
		_invert = invert;
	}

	public void UpdateValue(bool newValue)
	{
		_value = (_invert ? (!newValue) : newValue);
		RaisePropertyChanged("Value");
	}
}
