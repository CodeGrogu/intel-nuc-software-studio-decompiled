using System;
using System.Collections.Generic;
using System.IO;
using Infrastructure.Converters;
using Infrastructure.Models;
using MobileKeyboardSettingsMgr.Interface;
using Prism.Events;
using Prism.Mvvm;
using UniwillSdkClient.Events;

namespace MobileKeyboardSettingsMgr.Models;

internal class OsdStateModel : BindableBase, ISystemSetting
{
	public delegate bool EnableOsdDelegate(bool state);

	public static readonly string OSD_SETTINGS_DIRECTORY = Path.Combine(ApplicationConstants.GetProfileFolderPath(), "OSDSettings\\");

	private const string OSD_SETTINGS_FILE = "OSDSettings.json";

	private IEventAggregator _events;

	private bool _value = true;

	private EnableOsdDelegate _enableOsd;

	public string Label => ResourceDictionaryConverter.GetString("FuncKeyOnScreenDisplayString");

	public bool Value
	{
		get
		{
			return _value;
		}
		set
		{
			SetProperty(ref _value, value, "Value");
			_enableOsd(value);
			_events.GetEvent<OsdEnabledEvent>().Publish(value);
			SerializeOsdState(value);
		}
	}

	public OsdStateModel(IEventAggregator events, bool value, EnableOsdDelegate enableOsd)
	{
		_events = events;
		_value = value;
		_enableOsd = enableOsd;
		Directory.CreateDirectory(OSD_SETTINGS_DIRECTORY);
	}

	public static void SerializeOsdState(bool state)
	{
		NssJsonSerializer.Serialize(state, Path.Combine(OSD_SETTINGS_DIRECTORY, "OSDSettings.json"));
	}

	public static bool GetPreviousOsdState()
	{
		return NssJsonSerializer.Deserialize<bool>(OSD_SETTINGS_DIRECTORY + "OSDSettings.json", new List<Type> { typeof(bool) });
	}
}
