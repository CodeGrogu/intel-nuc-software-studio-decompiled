using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Media;
using SystemPerformanceMgr.Events;
using SystemPerformanceMgr.Interfaces;
using SystemPerformanceMgr.Models.Events;
using CustomizationMgr.Interfaces;
using FeatureMgr.Enums;
using FeatureMgr.Models;
using Infrastructure.Converters;
using Infrastructure.Events;
using Infrastructure.Models;
using IntelSdkClient.Events;
using IntelSdkClient.PerformanceTuning.Interface;
using IntelSdkClient.PerformanceTuningService;
using Prism.Events;
using Prism.Logging;
using Prism.Mvvm;

namespace SystemPerformanceMgr.Models;

public class NssPerformanceModeManager : BindableBase, INucPerformanceModeManager
{
	private static readonly string PROFILES_PATH = Path.Combine(ApplicationConstants.GetProfileFolderPath(), "PerformanceTuningProfile.json");

	private static readonly string CUSTOM_PROFILE_PATH = Path.Combine(ApplicationConstants.GetProfileFolderPath(), "PerformanceTuningCustomProfile.json");

	private static readonly string Fan_PROFILE_PATH = Path.Combine(ApplicationConstants.GetProfileFolderPath(), "FanProfile.json");

	private static readonly string Supported_Fan_Path = Path.Combine(ApplicationConstants.GetProfileFolderPath(), "SupportedFanList.json");

	private static readonly string FixedDutyCycle_Fan_Path = Path.Combine(ApplicationConstants.GetProfileFolderPath(), "FixedDutyCycle.json");

	public static string CUSTOM = "Custom";

	public static string FIXED = "Fixed";

	public static string DEFAULT_FAN_MODE = "Balanced";

	public static int DEFAULT_BYTE_PLACE = 16;

	private readonly IEventAggregator _events;

	private readonly ILoggerFacade _logger;

	private readonly ICustomResourceManager _customResourceManager;

	private readonly IPerformanceTuningClient _performanceTuningClient;

	private PowerProfile _currentPowerMode;

	private ObservableCollection<PowerProfile> _supportedCpuModes;

	private string _currentFanMode;

	private FanSetting[] _fanSettings;

	private FanSetting23[] _fanSettings23;

	private NucType _nucType;

	private bool _isFanOff;

	private int _fanOffTempMinValue;

	private int _fanOffTempMaxValue;

	private int _fixedDutyCycle;

	private Dictionary<string, bool> _fanoffState;

	private Dictionary<string, byte[]> _fixedDutyCycleDic;

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

	public PowerSetting CurrentBiosPowerSetting { get; private set; }

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

	public FanSetting[] FanSettings
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

	public FanSetting23[] FanSettings23
	{
		get
		{
			return _fanSettings23;
		}
		set
		{
			SetProperty(ref _fanSettings23, value, "FanSettings23");
		}
	}

	public NucType NucType
	{
		get
		{
			return _nucType;
		}
		set
		{
			SetProperty(ref _nucType, value, "NucType");
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

	public Dictionary<string, bool> FanoffState
	{
		get
		{
			return _fanoffState;
		}
		set
		{
			SetProperty(ref _fanoffState, value, "FanoffState");
		}
	}

	public Dictionary<string, byte[]> FixedDutyCycleDic
	{
		get
		{
			return _fixedDutyCycleDic;
		}
		set
		{
			SetProperty(ref _fixedDutyCycleDic, value, "FixedDutyCycleDic");
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

	public NssPerformanceModeManager(IEventAggregator events, ILoggerFacade logger, ICustomResourceManager customResourceManager, IPerformanceTuningClient performanceTuningClient)
	{
		_events = events;
		_logger = logger;
		_customResourceManager = customResourceManager;
		_performanceTuningClient = performanceTuningClient;
		PowerProfile? savedSetting = null;
		string savedSetting2 = null;
		NucType = PlatformDetail.GetNucType();
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
			GetSupportedFanModes(savedSetting2);
			GetSupportedFanTypes();
		}
		else
		{
			FanModesSupported = false;
		}
		AllTauValues();
		if (NucType != NucType.Serpent)
		{
			SetDefaultTemprature();
		}
		PublishTrayAppMenuItems();
		RemoveCustomMode();
		if (_nucType != NucType.Phantom && _nucType != NucType.Panther)
		{
			return;
		}
		FanoffState = new Dictionary<string, bool>();
		if (File.Exists(Fan_PROFILE_PATH))
		{
			FanoffState = GetSaveFanOffCapability();
			if (FanoffState != null)
			{
				foreach (KeyValuePair<string, bool> item in FanoffState.ToList())
				{
					if (item.Key.ToString() != FIXED)
					{
						SetFanOffConfiguration(item.Key, item.Value);
					}
				}
				return;
			}
			FanoffState = new Dictionary<string, bool>();
		}
		else if (_currentFanMode != FIXED)
		{
			SaveFanOffCapability(_currentFanMode, isFanOff: false);
		}
	}

	private void IsPerformanceDriverError(string Error)
	{
		_performanceDriverError = Error;
	}

	private void RemoveCustomMode()
	{
		_nucType = PlatformDetail.GetNucType();
		if ((_nucType.Equals(NucType.Panther) || _nucType.Equals(NucType.Phantom)) && SupportedFanModes.Contains(CUSTOM))
		{
			string item = SupportedFanModes.Where((string x) => x == CUSTOM).ToList()[0];
			SupportedFanModes.Remove(item);
		}
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

	private void SetDefaultTemprature()
	{
		if (SupportedFanModes != null)
		{
			FanProfile fanProfileData = _performanceTuningClient.GetFanProfileData(SupportedFanModes.FirstOrDefault((string x) => string.Equals(x, "Cool", StringComparison.OrdinalIgnoreCase)));
			Temp_Min = 0;
			Temp_Max = 100;
			Duty_Min = 1;
			Duty_Max = fanProfileData.fanSettings[0].PrimaryDutyIncrement;
		}
	}

	public FanProfile GetFanProfileData(string currentFanMode)
	{
		return _performanceTuningClient.GetFanProfileData(currentFanMode);
	}

	public FanProfile23 GetFanProfileData23(string currentFanMode)
	{
		return _performanceTuningClient.GetFanProfileData23(currentFanMode);
	}

	private void GetCurrentBiosPowerSetting()
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
					PL1 = (byte)CurrentBiosPowerSetting.PL1,
					PL2 = (byte)CurrentBiosPowerSetting.PL2,
					Tau = (byte)CurrentBiosPowerSetting.Tau
				};
			}
			SupportedCpuModes.Add(Custom);
			foreach (PowerProfile supportedCpuMode in SupportedCpuModes)
			{
				if (!savedSetting.HasValue && supportedCpuMode.PL1.Equals((byte)CurrentBiosPowerSetting.PL1) && supportedCpuMode.PL2.Equals((byte)CurrentBiosPowerSetting.PL2) && supportedCpuMode.Tau.Equals((byte)CurrentBiosPowerSetting.Tau))
				{
					_currentPowerMode = supportedCpuMode;
					_currentPowerMode.IsSelected = true;
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
			SetMaxValues(SupportedCpuModes.FirstOrDefault((PowerProfile x) => x.Name.Equals("MAX PERFORMANCE", StringComparison.OrdinalIgnoreCase)));
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
				if (NucType == NucType.Serpent)
				{
					FanoffState = new Dictionary<string, bool>();
					SupportedFanModes = GetSupportedFanModeList();
					if (SupportedFanModes == null)
					{
						SupportedFanModes = new List<string>();
						SupportedFanModes.AddRange(_performanceTuningClient.GetSupportedFanModes());
					}
					if (_currentBIOSFanMode == FIXED && !SupportedFanModes.Contains(FIXED))
					{
						SupportedFanModes.Add(FIXED);
					}
					if (SupportedFanModes.Contains(FIXED))
					{
						SetFixedFanDutyCycle(savedSetting);
					}
					SaveSupportedFanModeList();
				}
				else
				{
					SupportedFanModes = new List<string>();
					SupportedFanModes.AddRange(_performanceTuningClient.GetSupportedFanModes());
				}
				CurrentFanMode = string.Empty;
			}
			if (string.IsNullOrEmpty(savedSetting))
			{
				CurrentFanMode = SupportedFanModes.Find((string x) => x == _currentBIOSFanMode);
				if (NucType != NucType.Serpent && (string.IsNullOrWhiteSpace(CurrentFanMode) || CurrentFanMode == CUSTOM))
				{
					CurrentFanMode = DEFAULT_FAN_MODE;
				}
			}
			else
			{
				CurrentFanMode = SupportedFanModes.Find((string x) => x == savedSetting);
			}
			SetFanMode(CurrentFanMode);
			if (CurrentFanMode == CUSTOM && File.Exists(Fan_PROFILE_PATH))
			{
				FanoffState = GetSaveFanOffCapability();
				if (FanoffState != null)
				{
					KeyValuePair<string, bool> keyValuePair = FanoffState.ToList().FirstOrDefault((KeyValuePair<string, bool> x) => x.Key == savedSetting);
					SetFanOffConfiguration(keyValuePair.Key, keyValuePair.Value);
				}
			}
			RemoveDuplicateFanModes();
			if (CurrentFanMode == CUSTOM)
			{
				_fanSettings = GetSaveCustomSettings();
				if (_fanSettings != null)
				{
					SetFanCustomMode(_fanSettings);
				}
				else if (NucType == NucType.Serpent)
				{
					_fanSettings23 = _performanceTuningClient.GetFanProfileData23(CurrentFanMode).fanSettings;
				}
				else
				{
					_fanSettings = _performanceTuningClient.GetFanProfileData(CurrentFanMode).fanSettings;
				}
			}
			else if (NucType == NucType.Serpent)
			{
				FanProfile23 fanProfileData = _performanceTuningClient.GetFanProfileData23(CurrentFanMode);
				if (fanProfileData.fanSettings != null)
				{
					_fanSettings23 = fanProfileData.fanSettings;
				}
				else
				{
					_events.GetEvent<PerformanceDriverErrorEvent>().Publish("PerformanceServiceNotCompatibleError");
				}
			}
			else
			{
				_fanSettings = _performanceTuningClient.GetFanProfileData(CurrentFanMode).fanSettings;
			}
			if (_fanSettings != null)
			{
				_fanOffTempMinValue = 0;
				_fanOffTempMaxValue = ((_fanSettings.Length != 0) ? (_fanSettings[0].PrimaryMinimumTemperature - 1) : 0);
			}
		}
		else
		{
			_events.GetEvent<PerformanceDriverErrorEvent>().Publish("FanModeError");
		}
		return SupportedFanModes;
	}

	private void RemoveDuplicateFanModes()
	{
		SupportedFanModes = SupportedFanModes.Distinct().ToList();
	}

	private void GetSupportedFanTypes()
	{
		SupportedFanTypes = new List<string>();
		List<string> list = _performanceTuningClient.SupportedFanTypes()?.ToList();
		if (list.Count == 0)
		{
			_events.GetEvent<PerformanceDriverErrorEvent>().Publish("FantypesError");
			return;
		}
		foreach (string item in list)
		{
			SupportedFanTypes.Add(ResourceDictionaryConverter.GetString(item).Replace(":", ""));
		}
	}

	public bool SetFanMode(string NewMode)
	{
		CurrentFanMode = NewMode;
		SaveCurrentSettings();
		if (NewMode == FIXED)
		{
			SetFixedFanDutyCycle(NewMode);
		}
		return _performanceTuningClient.SetFanMode(NewMode);
	}

	public bool SetFanCustomMode(FanSetting[] fanSettings)
	{
		CurrentFanMode = CUSTOM;
		_fanSettings = fanSettings;
		FanControlAllFanSettings fanCustom = new FanControlAllFanSettings
		{
			fanSettings = fanSettings
		};
		SaveCustomSettings();
		SaveCurrentSettings();
		return _performanceTuningClient.SetFanCustom(fanCustom);
	}

	private Tuple<PowerProfile, string> GetSavedSettings()
	{
		return NssJsonSerializer.Deserialize<Tuple<PowerProfile, string>>(PROFILES_PATH, new List<Type> { typeof(Tuple<PowerProfile, string>) });
	}

	private FanSetting[] GetSaveCustomSettings()
	{
		return NssJsonSerializer.Deserialize<FanSetting[]>(CUSTOM_PROFILE_PATH, new List<Type> { typeof(FanSetting[]) });
	}

	private List<string> GetSupportedFanModeList()
	{
		return NssJsonSerializer.Deserialize<List<string>>(Supported_Fan_Path, new List<Type> { typeof(List<string>) });
	}

	private Dictionary<string, bool> GetSaveFanOffCapability()
	{
		return NssJsonSerializer.Deserialize<Dictionary<string, bool>>(Fan_PROFILE_PATH, new List<Type> { typeof(Dictionary<string, bool>) });
	}

	private void SaveCurrentSettings()
	{
		NssJsonSerializer.Serialize(new Tuple<PowerProfile, string>(CurrentPowerMode, CurrentFanMode), PROFILES_PATH);
	}

	private void SaveCustomSettings()
	{
		FanSetting[] fanSettings = FanSettings;
		if (fanSettings != null && fanSettings.Length != 0)
		{
			NssJsonSerializer.Serialize(FanSettings, CUSTOM_PROFILE_PATH);
		}
	}

	public void SaveFanOffCapability(string selectedFanMode, bool isFanOff)
	{
		IsFanOff = isFanOff;
		if (FanoffState != null)
		{
			if (FanoffState.ContainsKey(selectedFanMode))
			{
				FanoffState[selectedFanMode] = IsFanOff;
			}
			else
			{
				FanoffState.Add(selectedFanMode, isFanOff);
			}
		}
		NssJsonSerializer.Serialize(FanoffState, Fan_PROFILE_PATH);
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
			SaveFanOffCapability(selectedFanMode, isFanOff);
			return true;
		}
		return false;
	}

	private void SetFixedFanDutyCycle(string currentSavedMode)
	{
		_fixedDutyCycleDic = new Dictionary<string, byte[]>();
		if (File.Exists(FixedDutyCycle_Fan_Path))
		{
			_fixedDutyCycleDic = GetFanDutyCycleFixed();
			if (_fixedDutyCycleDic != null)
			{
				FixedDutyCycle = _fixedDutyCycleDic[FIXED][DEFAULT_BYTE_PLACE];
				if (currentSavedMode == FIXED)
				{
					SetFanDutyCycleFixedMode(FIXED, _fixedDutyCycleDic[FIXED]);
				}
			}
			else
			{
				_fixedDutyCycleDic = new Dictionary<string, byte[]>();
				CreateFanDutyCycleFixedMode();
			}
		}
		else
		{
			CreateFanDutyCycleFixedMode();
		}
	}

	public void CreateFanDutyCycleFixedMode()
	{
		FanProfile23 fanProfileData = GetFanProfileData23(_currentBIOSFanMode);
		FixedDutyCycle = fanProfileData.profile[DEFAULT_BYTE_PLACE];
		SetFanDutyCycleFixedMode(FIXED, fanProfileData.profile);
		SaveFanDutyCycleFixed(_currentBIOSFanMode, fanProfileData.profile);
	}

	public void SetFanDutyCycleFixedMode(string selectedFanMode, byte[] FanBytes)
	{
		FixedDutyCycle = FanBytes[DEFAULT_BYTE_PLACE];
		_performanceTuningClient.SetFanDutyCycleFixedMode(selectedFanMode, FanBytes);
		SaveFanDutyCycleFixed(selectedFanMode, FanBytes);
	}

	private void SaveFanDutyCycleFixed(string selectedFanMode, byte[] FanBytes)
	{
		if (_fixedDutyCycleDic != null)
		{
			_fixedDutyCycleDic[selectedFanMode] = FanBytes;
		}
		NssJsonSerializer.Serialize(_fixedDutyCycleDic, FixedDutyCycle_Fan_Path);
	}

	private Dictionary<string, byte[]> GetFanDutyCycleFixed()
	{
		return NssJsonSerializer.Deserialize<Dictionary<string, byte[]>>(FixedDutyCycle_Fan_Path, new List<Type> { typeof(Dictionary<string, bool>) });
	}

	~NssPerformanceModeManager()
	{
		SaveCurrentSettings();
		SaveCustomSettings();
		SaveFanOffCapability(CurrentFanMode, IsFanOff);
		SaveSupportedFanModeList();
	}
}
