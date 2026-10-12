using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Media;
using SystemPerformanceMgr.Events;
using SystemPerformanceMgr.Models.Events;
using SystemPerformanceMgr.RaptorSystemPerformanceMgr.Enums;
using SystemPerformanceMgr.RaptorSystemPerformanceMgr.Interfaces;
using CustomizationMgr.Interfaces;
using Infrastructure.Converters;
using Infrastructure.Events;
using Infrastructure.Models;
using IntelSdkClient.Events;
using IntelSdkClient.PerformanceTuning.Interface;
using IntelSdkClient.PerformanceTuningService;
using Prism.Events;
using Prism.Logging;
using Prism.Mvvm;

namespace SystemPerformanceMgr.RaptorSystemPerformanceMgr.Models;

internal class RNPerformanceModeManager : BindableBase, IRNPerformanceModeManager
{
	private static readonly string PROFILES_PATH = Path.Combine(ApplicationConstants.GetProfileFolderPath(), "PerformanceTuningProfile.json");

	private static readonly string CUSTOM_PROFILE_PATH = Path.Combine(ApplicationConstants.GetProfileFolderPath(), "PerformanceTuningCustomProfile.json");

	private static readonly string Fan_PROFILE_PATH = Path.Combine(ApplicationConstants.GetProfileFolderPath(), "FanProfile.json");

	private static readonly string Supported_Fan_Path = Path.Combine(ApplicationConstants.GetProfileFolderPath(), "SupportedFanList.json");

	private static readonly string FixedDutyCycle_Fan_Path = Path.Combine(ApplicationConstants.GetProfileFolderPath(), "FixedDutyCycle.json");

	public static string CUSTOM = "Custom";

	public static string FIXED = "Fixed";

	public static string DEFAULT_FAN_MODE = "Balanced";

	public static string DEFAULT_POWER_MODE = "BALANCED";

	public static string MAX_POWER_MODE = "MAX PERFORMANCE";

	public static string COOL_FAN_MODE = "Cool";

	public static int DEFAULT_BYTE_PLACE = 16;

	private readonly IEventAggregator _events;

	private readonly ILoggerFacade _logger;

	private readonly ICustomResourceManager _customResourceManager;

	private readonly IPerformanceTuningClient _performanceTuningClient;

	private PowerProfile _currentPowerMode;

	private ObservableCollection<PowerProfile> _supportedCpuModes;

	private string _currentFanMode;

	private FanSettingRN[] _fanSettings;

	private List<FanProfileRN> _fanProfiles;

	private bool _isFanOff;

	private int _fanOffTempMinValue;

	private int _fanOffTempMaxValue;

	private int _fixedDutyCycle;

	private PowerProfile _custom;

	private string _performanceDriverError;

	private string _currentBIOSFanMode;

	public PowerProfile CurrentPowerMode
	{
		get
		{
			return _currentPowerMode;
		}
		set
		{
			_currentPowerMode = value;
			_events.GetEvent<PowerModeChangedEvent>().Publish();
		}
	}

	public ObservableCollection<PowerProfile> SupportedCpuModes
	{
		get
		{
			return _supportedCpuModes;
		}
		set
		{
			SetProperty(ref _supportedCpuModes, value, "SupportedCpuModes");
		}
	}

	public PowerSetting CurrentBiosPowerSetting { get; set; }

	public string CurrentFanMode
	{
		get
		{
			return _currentFanMode;
		}
		private set
		{
			_currentFanMode = value;
			_events.GetEvent<FanModeChangedEvent>().Publish();
		}
	}

	public FanSettingRN[] FanSettings
	{
		get
		{
			return _fanSettings;
		}
		set
		{
			SetProperty(ref _fanSettings, value, "FanSettings");
		}
	}

	public List<FanProfileRN> FanProfiles
	{
		get
		{
			return _fanProfiles;
		}
		set
		{
			SetProperty(ref _fanProfiles, value, "FanProfiles");
		}
	}

	public bool IsFanOff
	{
		get
		{
			return _isFanOff;
		}
		set
		{
			SetProperty(ref _isFanOff, value, "IsFanOff");
		}
	}

	public int FanOffTempMinValue
	{
		get
		{
			return _fanOffTempMinValue;
		}
		set
		{
			SetProperty(ref _fanOffTempMinValue, value, "FanOffTempMinValue");
		}
	}

	public int FanOffTempMaxValue
	{
		get
		{
			return _fanOffTempMaxValue;
		}
		set
		{
			SetProperty(ref _fanOffTempMaxValue, value, "FanOffTempMaxValue");
		}
	}

	public int FixedDutyCycle
	{
		get
		{
			return _fixedDutyCycle;
		}
		set
		{
			SetProperty(ref _fixedDutyCycle, value, "FixedDutyCycle");
		}
	}

	public List<string> SupportedFanModes { get; private set; }

	public List<string> SupportedFanTypes { get; private set; }

	public DoubleCollection TauValues { get; private set; }

	public bool PowerModesSupported { get; private set; }

	public bool FanModesSupported { get; private set; }

	public PowerProfile Custom
	{
		get
		{
			return _custom;
		}
		set
		{
			SetProperty(ref _custom, value, "Custom");
		}
	}

	public uint Max_PL1 { get; set; }

	public uint Max_PL2 { get; set; }

	public uint Max_Tau { get; set; }

	public int Temp_Min { get; set; }

	public int Temp_Max { get; set; }

	public int Duty_Min { get; set; }

	public int Duty_Max { get; set; }

	public string CurrentBIOSFanMode
	{
		get
		{
			return _currentBIOSFanMode;
		}
		set
		{
			_currentBIOSFanMode = value;
		}
	}

	public string PerformanceDriverError
	{
		get
		{
			return _performanceDriverError;
		}
		set
		{
			SetProperty(ref _performanceDriverError, value, "PerformanceDriverError");
		}
	}

	public RNPerformanceModeManager(IEventAggregator events, ILoggerFacade logger, ICustomResourceManager customResourceManager, IPerformanceTuningClient performanceTuningClient)
	{
		_events = events;
		_logger = logger;
		_customResourceManager = customResourceManager;
		_performanceTuningClient = performanceTuningClient;
		PowerProfile? savedSetting = null;
		string savedSetting2 = null;
		Tuple<PowerProfile, string> savedSettings = GetSavedSettings();
		if (savedSettings != null)
		{
			savedSetting = savedSettings.Item1;
			savedSetting2 = savedSettings.Item2;
		}
		_events.GetEvent<PerformanceDriverErrorEvent>().Subscribe(IsPerformanceDriverError);
		if (_performanceTuningClient.GetPowerLevelControlSupported() == true)
		{
			PowerModesSupported = true;
			GetSupportedPowerModes(savedSetting);
		}
		else
		{
			PowerModesSupported = false;
			_events.GetEvent<PerformanceDriverErrorEvent>().Publish("PowerModeNotSupportedError");
		}
		if (_performanceTuningClient.IsFanEcSupported() == true && _performanceTuningClient.IsFanProfilesSupported() == true)
		{
			FanModesSupported = true;
			GetSupportedFanTypes();
			GetSupportedFanModes(savedSetting2);
		}
		else
		{
			FanModesSupported = false;
		}
		AllTauValues();
		PublishTrayAppMenuItems();
	}

	private void IsPerformanceDriverError(string Error)
	{
		_performanceDriverError = Error;
	}

	private void PublishTrayAppMenuItems()
	{
		Dictionary<string, EventHandler> dictionary = new Dictionary<string, EventHandler>();
		if (SupportedCpuModes == null)
		{
			return;
		}
		foreach (PowerProfile SupportedCpuMode in SupportedCpuModes)
		{
			if (SupportedCpuMode.Name != ResourceDictionaryConverter.GetString("Custom"))
			{
				dictionary.Add(ResourceDictionaryConverter.GetString(SupportedCpuMode.Name), (object s, EventArgs e) =>
				{
					SwitchProfiles(SupportedCpuMode.Name);
				});
			}
		}
		_events.GetEvent<AddTrayAppMenuItemsEvent>().Publish(dictionary);
	}

	private void SwitchProfiles(string Name)
	{
		CurrentPowerMode = SupportedCpuModes.FirstOrDefault((PowerProfile x) => x.Name == Name);
		_events.GetEvent<SwitchCpuModesEvent>().Publish(CurrentPowerMode);
	}

	public FanProfile23 GetFanProfileData23(string currentFanMode)
	{
		return _performanceTuningClient.GetFanProfileData23(currentFanMode);
	}

	public void GetCurrentBiosPowerSetting()
	{
		CurrentBiosPowerSetting = _performanceTuningClient.GetCurrentPowerSettings();
	}

	private void GetSupportedPowerModes(PowerProfile? savedSetting)
	{
		if (_performanceTuningClient.GetSupportedPowerProfiles().Length != 0)
		{
			if (SupportedCpuModes != null)
			{
				return;
			}
			GetCurrentBiosPowerSetting();
			SupportedCpuModes = new ObservableCollection<PowerProfile>(_performanceTuningClient.GetSupportedPowerProfiles());
			ChangePowerProfilesIndex();
			SupportedCpuModes = new ObservableCollection<PowerProfile>(SupportedCpuModes);
			if (savedSetting?.Name == ResourceDictionaryConverter.GetString("Custom"))
			{
				_custom = savedSetting.Value;
				_custom.TauHt = 110;
			}
			else
			{
				_custom = new PowerProfile
				{
					Name = ResourceDictionaryConverter.GetString("Custom"),
					PL1 = CurrentBiosPowerSetting.PL1,
					PL2 = CurrentBiosPowerSetting.PL2,
					Tau = (uint)CurrentBiosPowerSetting.Tau
				};
			}
			SupportedCpuModes.Add(Custom);
			foreach (PowerProfile supportedCpuMode in SupportedCpuModes)
			{
				if (!savedSetting.HasValue && supportedCpuMode.PL1.Equals(CurrentBiosPowerSetting.PL1) && supportedCpuMode.PL2.Equals(CurrentBiosPowerSetting.PL2) && supportedCpuMode.Tau.Equals((uint)CurrentBiosPowerSetting.Tau))
				{
					_currentPowerMode = supportedCpuMode;
					_currentPowerMode.IsSelected = true;
					if (CurrentBiosPowerSetting.PL1 == 0 && CurrentBiosPowerSetting.PL2 == 0 && CurrentBiosPowerSetting.Tau == 0)
					{
						_currentPowerMode = SupportedCpuModes.FirstOrDefault((PowerProfile mode) => mode.Name == DEFAULT_POWER_MODE);
						int index = SupportedCpuModes.IndexOf(_custom);
						_custom.PL1 = _currentPowerMode.PL1;
						_custom.PL2 = _currentPowerMode.PL2;
						_custom.Tau = _currentPowerMode.Tau;
						SupportedCpuModes[index] = _custom;
					}
					break;
				}
				if (savedSetting.HasValue)
				{
					if (string.Equals(supportedCpuMode.Name, savedSetting.Value.Name, StringComparison.OrdinalIgnoreCase))
					{
						_currentPowerMode = supportedCpuMode;
						_currentPowerMode.IsSelected = true;
						break;
					}
					if (supportedCpuMode.PL1.Equals(savedSetting.Value.PL1) && supportedCpuMode.PL2.Equals(savedSetting.Value.PL2) && supportedCpuMode.Tau.Equals(savedSetting.Value.Tau))
					{
						_currentPowerMode = supportedCpuMode;
						_currentPowerMode.IsSelected = true;
						break;
					}
				}
			}
			PowerSetting customPowerSetting = new PowerSetting
			{
				PL1 = _currentPowerMode.PL1,
				PL2 = _currentPowerMode.PL2,
				Tau = (int)_currentPowerMode.Tau
			};
			_performanceTuningClient.SetCustomPowerSetting(customPowerSetting);
			SetMaxValues(SupportedCpuModes.FirstOrDefault((PowerProfile x) => x.Name.Equals(MAX_POWER_MODE, StringComparison.OrdinalIgnoreCase)));
		}
		else
		{
			_events.GetEvent<PerformanceDriverErrorEvent>().Publish("CPUModesError");
		}
	}

	private void ChangePowerProfilesIndex()
	{
		if (SupportedCpuModes.Count > 2)
		{
			PowerProfile item = SupportedCpuModes[0];
			SupportedCpuModes.RemoveAt(0);
			SupportedCpuModes.Insert(2, item);
			item = SupportedCpuModes[1];
			SupportedCpuModes.RemoveAt(1);
			SupportedCpuModes.Insert(0, item);
		}
	}

	private void SetMaxValues(PowerProfile powerProfile)
	{
		Max_PL1 = powerProfile.PL1;
		Max_PL2 = powerProfile.PL2;
		Max_Tau = powerProfile.Tau;
	}

	public bool SetPowerMode(PowerProfile NewMode)
	{
		CurrentPowerMode = NewMode;
		SaveCurrentSettings();
		if (NewMode.Name == ResourceDictionaryConverter.GetString("Custom"))
		{
			PowerSetting customPowerSetting = new PowerSetting
			{
				PL1 = NewMode.PL1,
				PL2 = NewMode.PL2,
				Tau = (int)NewMode.Tau
			};
			return _performanceTuningClient.SetCustomPowerSetting(customPowerSetting);
		}
		return _performanceTuningClient.ApplyBiosPowerProfile(NewMode.Name);
	}

	public List<string> GetSupportedFanModes(string savedSetting)
	{
		if (_performanceTuningClient.GetSupportedFanModes().Length != 0)
		{
			_currentBIOSFanMode = _performanceTuningClient.GetCurrentBiosFanModeName();
			if (SupportedFanModes == null)
			{
				SupportedFanModes = new List<string>();
				SupportedFanModes.AddRange(_performanceTuningClient.GetSupportedFanModes());
				CurrentFanMode = string.Empty;
			}
			SupportedFanModes.Add(ResourceDictionaryConverter.GetString(FIXED));
			RemoveDuplicateFanModes();
			FanProfiles = GetFanProfileDataRN();
			FanProfileRN fanProfile = FanProfiles.FirstOrDefault((FanProfileRN x) => x.fanType.Equals(FanType.CPUFan1.ToString()));
			CurrentFanMode = fanProfile.CurrentBiosFanMode;
			if (_currentPowerMode.Name == MAX_POWER_MODE)
			{
				SetFansToCoolMode();
			}
			else
			{
				SetFanMode(CurrentFanMode, FanType.CPUFan1.ToString(), fanProfile);
			}
		}
		else
		{
			_events.GetEvent<PerformanceDriverErrorEvent>().Publish("FanModeError");
		}
		return SupportedFanModes;
	}

	public void SetFansToCoolMode()
	{
		foreach (string supportedFanType in SupportedFanTypes)
		{
			int fanTypeIndex = SupportedFanTypes.IndexOf(supportedFanType);
			FanProfileRN fanProfile = FanProfiles.FirstOrDefault((FanProfileRN x) =>
			{
				string fanType2 = x.fanType;
				FanType fanType3 = (FanType)fanTypeIndex;
				return fanType2.Equals(fanType3.ToString());
			});
			string cOOL_FAN_MODE = COOL_FAN_MODE;
			FanType fanType = (FanType)fanTypeIndex;
			SetFanMode(cOOL_FAN_MODE, fanType.ToString(), fanProfile);
		}
	}

	public List<FanProfileRN> GetFanProfileDataRN()
	{
		if (!File.Exists(Fan_PROFILE_PATH))
		{
			return GetFanProfileDataFromService();
		}
		FanProfiles = GetSavedFanProfle();
		if (FanProfiles == null || FanProfiles.Any((FanProfileRN profilie) => profilie.fanType.ToString() == "TopFan") || FanProfiles.Any((FanProfileRN profilie) => profilie.fanType.ToString() == "BottomFan"))
		{
			FanProfiles = GetFanProfileDataFromService();
		}
		return FanProfiles;
	}

	private List<FanProfileRN> GetFanProfileDataFromService()
	{
		FanProfiles = _performanceTuningClient.GetFanProfileDataRN();
		SaveFanProfile(FanProfiles);
		return FanProfiles;
	}

	private void RemoveDuplicateFanModes()
	{
		SupportedFanModes = SupportedFanModes.Distinct().ToList();
	}

	private void GetSupportedFanTypes()
	{
		SupportedFanTypes = new List<string>();
		List<string> list = _performanceTuningClient.SupportedFanTypes()?.ToList();
		if (list != null && list.Count == 0)
		{
			_events.GetEvent<PerformanceDriverErrorEvent>().Publish("FantypesError");
			return;
		}
		int num = list.IndexOf(FanType.CPUFan2.ToString());
		if (num > 0)
		{
			string item = list[num];
			list.RemoveAt(num);
			list.Insert(1, item);
			{
				foreach (string item2 in list)
				{
					switch (item2)
					{
					case "CPUFan":
						SupportedFanTypes.Add(ResourceDictionaryConverter.GetString("CPUFan1").Replace(":", ""));
						break;
					case "CPUFan2":
						SupportedFanTypes.Add(ResourceDictionaryConverter.GetString("CPUFan2").Replace(":", ""));
						break;
					case "SystemFan1":
						SupportedFanTypes.Add(ResourceDictionaryConverter.GetString("SystemFan1").Replace(":", ""));
						break;
					case "SystemFan2":
						SupportedFanTypes.Add(ResourceDictionaryConverter.GetString("SystemFan2").Replace(":", ""));
						break;
					}
				}
				return;
			}
		}
		_events.GetEvent<PerformanceDriverErrorEvent>().Publish("FantypesError");
	}

	public bool SetFanMode(string NewMode, string FanType, FanProfileRN fanProfile)
	{
		int num = SupportedFanTypes.IndexOf(ResourceDictionaryConverter.GetString(FanType).Replace(":", ""));
		if (num >= 0)
		{
			fanProfile.CurrentBiosFanMode = NewMode;
			CurrentFanMode = NewMode;
			FanProfiles[num] = fanProfile;
			bool flag = _performanceTuningClient.SetFanModeRN(NewMode, FanType, FanProfiles);
			if (flag)
			{
				SaveFanProfile(FanProfiles);
			}
			return flag;
		}
		return false;
	}

	public bool SetFanCustomMode(string NewMode, FanSettingRN[] fanSettings, string FanType)
	{
		int num = FanProfiles.FindIndex((FanProfileRN x) => x.fanType.Equals(FanType));
		FanProfileRN value = FanProfiles.FirstOrDefault((FanProfileRN x) => x.fanType.Equals(FanType));
		value.CurrentBiosFanMode = NewMode;
		if (NewMode == CUSTOM)
		{
			value.fanSettings[3] = fanSettings[num];
		}
		else if (NewMode == FIXED)
		{
			value.fanSettings[4] = fanSettings[num];
			value.fanSettings[4].FanModeName = NewMode;
		}
		FanProfiles[num] = value;
		CurrentFanMode = fanSettings[num].FanModeName;
		SaveFanProfile(FanProfiles);
		return _performanceTuningClient.SetFanModeRN(CurrentFanMode, FanType, FanProfiles);
	}

	private Tuple<PowerProfile, string> GetSavedSettings()
	{
		return NssJsonSerializer.Deserialize<Tuple<PowerProfile, string>>(PROFILES_PATH, new List<Type> { typeof(Tuple<PowerProfile, string>) });
	}

	private FanSettingRN[] GetSaveCustomSettings()
	{
		return NssJsonSerializer.Deserialize<FanSettingRN[]>(CUSTOM_PROFILE_PATH, new List<Type> { typeof(FanSetting[]) });
	}

	private List<string> GetSupportedFanModeList()
	{
		return NssJsonSerializer.Deserialize<List<string>>(Supported_Fan_Path, new List<Type> { typeof(List<string>) });
	}

	private List<FanProfileRN> GetSavedFanProfle()
	{
		return NssJsonSerializer.Deserialize<List<FanProfileRN>>(Fan_PROFILE_PATH, new List<Type> { typeof(FanProfileRN) });
	}

	public void SaveFanProfile(List<FanProfileRN> fanProfile)
	{
		NssJsonSerializer.Serialize(fanProfile, Fan_PROFILE_PATH);
	}

	private void SaveCurrentSettings()
	{
		NssJsonSerializer.Serialize(new Tuple<PowerProfile, string>(CurrentPowerMode, CurrentFanMode), PROFILES_PATH);
	}

	private void SaveCustomSettings()
	{
		FanSettingRN[] fanSettings = FanSettings;
		if (fanSettings != null && fanSettings.Length != 0)
		{
			NssJsonSerializer.Serialize(FanSettings, CUSTOM_PROFILE_PATH);
		}
	}

	private void SaveSupportedFanModeList()
	{
		if (SupportedFanModes.Count > 0)
		{
			NssJsonSerializer.Serialize(SupportedFanModes, Supported_Fan_Path);
		}
	}

	private void AllTauValues()
	{
		TauValues = new DoubleCollection();
		TauValues.Add(1.0);
		TauValues.Add(2.0);
		TauValues.Add(3.0);
		TauValues.Add(4.0);
		TauValues.Add(5.0);
		TauValues.Add(6.0);
		TauValues.Add(7.0);
		TauValues.Add(8.0);
		TauValues.Add(10.0);
		TauValues.Add(12.0);
		TauValues.Add(14.0);
		TauValues.Add(16.0);
		TauValues.Add(20.0);
		TauValues.Add(24.0);
		TauValues.Add(28.0);
		TauValues.Add(32.0);
		TauValues.Add(40.0);
		TauValues.Add(48.0);
		TauValues.Add(58.0);
		TauValues.Add(64.0);
		TauValues.Add(80.0);
		TauValues.Add(96.0);
		TauValues.Add(112.0);
		TauValues.Add(128.0);
		TauValues.Add(160.0);
		TauValues.Add(192.0);
		TauValues.Add(224.0);
	}

	public bool SetFanOffConfiguration(string selectedFanMode, bool isFanOff)
	{
		if (_performanceTuningClient.SetFanOffConfiguration(selectedFanMode, isFanOff))
		{
			return true;
		}
		return false;
	}

	private void SaveFanDutyCycleFixed(string selectedFanMode, byte[] FanBytes)
	{
	}

	private Dictionary<string, byte[]> GetFanDutyCycleFixed()
	{
		return NssJsonSerializer.Deserialize<Dictionary<string, byte[]>>(FixedDutyCycle_Fan_Path, new List<Type> { typeof(Dictionary<string, bool>) });
	}

	~RNPerformanceModeManager()
	{
		SaveCurrentSettings();
		SaveCustomSettings();
		SaveFanProfile(FanProfiles);
		SaveSupportedFanModeList();
	}
}
