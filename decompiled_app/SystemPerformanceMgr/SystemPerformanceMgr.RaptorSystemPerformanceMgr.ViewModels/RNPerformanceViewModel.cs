using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using SystemPerformanceMgr.RaptorSystemPerformanceMgr.Enums;
using SystemPerformanceMgr.RaptorSystemPerformanceMgr.Interfaces;
using CustomizationMgr.Interfaces;
using Infrastructure.Converters;
using IntelSdkClient.Events;
using IntelSdkClient.PerformanceTuningService;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace SystemPerformanceMgr.RaptorSystemPerformanceMgr.ViewModels;

internal class RNPerformanceViewModel : BindableBase
{
	private readonly IRNPerformanceModeManager _performanceModeManager;

	private readonly IEventAggregator _events;

	private readonly uint _max_PL1;

	private readonly uint _max_PL2;

	private readonly double _max_Tau;

	public static string MAX_PERFORMANCE_MODE = "MAX PERFORMANCE";

	public static string FAN_COOL_MODE = "Cool";

	private readonly string CustomFanMode = "Custom";

	private readonly string FixedFanMode = "Fixed";

	private readonly int CustomFanModeIndex = 3;

	private readonly int FixedFanModeIndex = 4;

	private readonly int _duty_min;

	private readonly int _duty_max;

	private uint _pl1;

	private uint _pl2;

	private double _tau;

	private DoubleCollection _tauValues;

	private int _fanTurnOnTemp;

	private int _fanOffTempMaxValue;

	private int _fanTurnOffTemp;

	private int _temperatureMax;

	private int _minDuty;

	private int _dutyCycleIncrement;

	private int _fixedFanDutyCycle;

	private int _minFanDutyCycle;

	private int _maxFanDutyCycle;

	private PowerSetting _currentBiosSetting;

	private bool _isSliderEnable;

	private bool _isCheckboxVisible;

	private bool _isFansliderEnable;

	private bool _saveButtonEnabled;

	private bool _powerSaveButtonEnable;

	private ObservableCollection<PowerProfile> _cpuPowerModes;

	private PowerProfile _selectedCpuPowerMode;

	private bool _showFanSaveButton;

	private bool _showPowerSaveButton;

	private string _selectedFanMode;

	private FanProfileRN _fanProfile;

	private ObservableCollection<string> _fanModes;

	private ObservableCollection<string> _fanTypes;

	private string _selectedFanType;

	private bool _showTempMaxMin;

	private bool _isTempDetailVisible;

	private bool _isFixedFanDetailVisible;

	private List<FanProfileRN> _fanProfiles;

	private string _fanOffInfoText;

	private DelegateCommand _save;

	private DelegateCommand _saveCustomPowerMode;

	private DelegateCommand _updateSelection;

	public bool Supported { get; }

	public string CpuUsageRegion => "PerformanceTuningCpuRegion";

	public string dGpuUsageRegion => "PerformanceTuningGpuRegion";

	public string PerformanceFanRegionRN => "PerformanceTunningFanRNView";

	public uint Max_PL1 => _max_PL1;

	public uint Max_PL2 => _max_PL2;

	public double Max_Tau => _max_Tau;

	public int Duty_Min => _duty_min;

	public int Duty_Max => _duty_max;

	public double Min_Tau { get; set; }

	public uint PL1
	{
		get
		{
			return _pl1;
		}
		set
		{
			SetProperty(ref _pl1, value, "PL1");
		}
	}

	public uint PL2
	{
		get
		{
			return _pl2;
		}
		set
		{
			SetProperty(ref _pl2, value, "PL2");
		}
	}

	public double Tau
	{
		get
		{
			return _tau;
		}
		set
		{
			SetProperty(ref _tau, value, "Tau");
		}
	}

	public DoubleCollection TauValues
	{
		get
		{
			return _tauValues;
		}
		set
		{
			SetProperty(ref _tauValues, value, "TauValues");
		}
	}

	public int FanTurnOnTemp
	{
		get
		{
			return _fanTurnOnTemp;
		}
		set
		{
			SetProperty(ref _fanTurnOnTemp, value, "FanTurnOnTemp");
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

	public int FanTurnOffTemp
	{
		get
		{
			return _fanTurnOffTemp;
		}
		set
		{
			SetProperty(ref _fanTurnOffTemp, value, "FanTurnOffTemp");
		}
	}

	public int TemperatureMax
	{
		get
		{
			return _temperatureMax;
		}
		set
		{
			SetProperty(ref _temperatureMax, value, "TemperatureMax");
		}
	}

	public int MinDuty
	{
		get
		{
			return _minDuty;
		}
		set
		{
			SetProperty(ref _minDuty, value, "MinDuty");
		}
	}

	public int DutyCycleIncrement
	{
		get
		{
			return _dutyCycleIncrement;
		}
		set
		{
			SetProperty(ref _dutyCycleIncrement, value, "DutyCycleIncrement");
		}
	}

	public int FixedFanDutyCycle
	{
		get
		{
			return _fixedFanDutyCycle;
		}
		set
		{
			SetProperty(ref _fixedFanDutyCycle, value, "FixedFanDutyCycle");
		}
	}

	public int MinFanDutyCycle
	{
		get
		{
			return _minFanDutyCycle;
		}
		set
		{
			SetProperty(ref _minFanDutyCycle, value, "MinFanDutyCycle");
		}
	}

	public int MaxFanDutyCycle
	{
		get
		{
			return _maxFanDutyCycle;
		}
		set
		{
			SetProperty(ref _maxFanDutyCycle, value, "MaxFanDutyCycle");
		}
	}

	public PowerSetting CurrentBiosSetting
	{
		get
		{
			return _currentBiosSetting;
		}
		set
		{
			SetProperty(ref _currentBiosSetting, value, "CurrentBiosSetting");
		}
	}

	public bool IsSliderEnable
	{
		get
		{
			return _isSliderEnable;
		}
		set
		{
			SetProperty(ref _isSliderEnable, value, "IsSliderEnable");
		}
	}

	public bool IsCheckboxVisible
	{
		get
		{
			return _isCheckboxVisible;
		}
		set
		{
			SetProperty(ref _isCheckboxVisible, value, "IsCheckboxVisible");
		}
	}

	public bool IsFanSliderEnable
	{
		get
		{
			return _isFansliderEnable;
		}
		set
		{
			SetProperty(ref _isFansliderEnable, value, "IsFanSliderEnable");
		}
	}

	public bool SaveButtonEnabled
	{
		get
		{
			return _saveButtonEnabled;
		}
		set
		{
			SetProperty(ref _saveButtonEnabled, value, "SaveButtonEnabled");
		}
	}

	public bool PowerSaveButtonEnable
	{
		get
		{
			return _powerSaveButtonEnable;
		}
		set
		{
			_powerSaveButtonEnable = value;
		}
	}

	public ObservableCollection<PowerProfile> CpuPowerModes
	{
		get
		{
			return _cpuPowerModes;
		}
		set
		{
			SetProperty(ref _cpuPowerModes, value, "CpuPowerModes");
		}
	}

	public PowerProfile SelectedCpuPowerMode
	{
		get
		{
			return _selectedCpuPowerMode;
		}
		set
		{
			SetProperty(ref _selectedCpuPowerMode, value, "SelectedCpuPowerMode");
			SetPowerModeValues(value);
		}
	}

	public bool DefaultSelected { get; set; }

	public bool ShowFanSaveButton
	{
		get
		{
			return _showFanSaveButton;
		}
		set
		{
			SetProperty(ref _showFanSaveButton, value, "ShowFanSaveButton");
		}
	}

	public bool ShowPowerSaveButton
	{
		get
		{
			return _showPowerSaveButton;
		}
		set
		{
			SetProperty(ref _showPowerSaveButton, value, "ShowPowerSaveButton");
		}
	}

	public string SelectedFanMode
	{
		get
		{
			return _selectedFanMode;
		}
		set
		{
			int num = FanTypes.IndexOf(SelectedFantype);
			if (value != CustomFanMode && value != ResourceDictionaryConverter.GetString(FixedFanMode))
			{
				IRNPerformanceModeManager performanceModeManager = _performanceModeManager;
				FanType fanType = (FanType)num;
				performanceModeManager.SetFanMode(value, fanType.ToString(), CurrentFanProfile);
			}
			SetProperty(ref _selectedFanMode, value, "SelectedFanMode");
			SetFanData(num);
		}
	}

	public FanProfileRN CurrentFanProfile
	{
		get
		{
			return _fanProfile;
		}
		set
		{
			SetProperty(ref _fanProfile, value, "CurrentFanProfile");
		}
	}

	public ObservableCollection<string> FanModes
	{
		get
		{
			return _fanModes;
		}
		set
		{
			SetProperty(ref _fanModes, value, "FanModes");
		}
	}

	public ObservableCollection<string> FanTypes
	{
		get
		{
			return _fanTypes;
		}
		set
		{
			SetProperty(ref _fanTypes, value, "FanTypes");
		}
	}

	public string SelectedFantype
	{
		get
		{
			return _selectedFanType;
		}
		set
		{
			if (value != CustomFanMode)
			{
				SetProperty(ref _selectedFanType, value, "SelectedFantype");
				int index = FanTypes.IndexOf(_selectedFanType);
				_fanProfile = FanProfiles.FirstOrDefault((FanProfileRN x) =>
				{
					string fanType = x.fanType;
					FanType fanType2 = (FanType)index;
					return fanType.Equals(fanType2.ToString());
				});
				_selectedFanMode = _fanProfile.CurrentBiosFanMode;
				SetFanData(index);
			}
		}
	}

	public bool ShowTempMaxMin
	{
		get
		{
			return _showTempMaxMin;
		}
		set
		{
			SetProperty(ref _showTempMaxMin, value, "ShowTempMaxMin");
		}
	}

	public bool IsTempDetailVisible
	{
		get
		{
			return _isTempDetailVisible;
		}
		set
		{
			SetProperty(ref _isTempDetailVisible, value, "IsTempDetailVisible");
		}
	}

	public bool IsFixedFanDetailVisible
	{
		get
		{
			return _isFixedFanDetailVisible;
		}
		set
		{
			SetProperty(ref _isFixedFanDetailVisible, value, "IsFixedFanDetailVisible");
		}
	}

	public List<FanProfileRN> FanProfiles
	{
		get
		{
			return _performanceModeManager.FanProfiles;
		}
		set
		{
			_fanProfiles = value;
		}
	}

	public string FanOffInfoText
	{
		get
		{
			return _fanOffInfoText;
		}
		set
		{
			SetProperty(ref _fanOffInfoText, value, "FanOffInfoText");
		}
	}

	public DelegateCommand Save
	{
		get
		{
			return _save;
		}
		set
		{
			SetProperty(ref _save, value, "Save");
		}
	}

	public DelegateCommand SaveCustomPowerMode
	{
		get
		{
			return _saveCustomPowerMode;
		}
		set
		{
			SetProperty(ref _saveCustomPowerMode, value, "SaveCustomPowerMode");
		}
	}

	public DelegateCommand UpdateSelection
	{
		get
		{
			return _updateSelection;
		}
		set
		{
			SetProperty(ref _updateSelection, value, "UpdateSelection");
		}
	}

	public RNPerformanceViewModel(IEventAggregator events, IRNPerformanceModeManager performanceModeManager, ICustomResourceManager resourceManager)
	{
		_events = events;
		_performanceModeManager = performanceModeManager;
		Supported = _performanceModeManager.PowerModesSupported;
		if (Supported)
		{
			_currentBiosSetting = _performanceModeManager.CurrentBiosPowerSetting;
			_max_PL1 = _performanceModeManager.Max_PL1;
			_max_PL2 = _performanceModeManager.Max_PL2;
			_max_Tau = _performanceModeManager.Max_Tau;
			_duty_min = _performanceModeManager.Duty_Min;
			_duty_max = _performanceModeManager.Duty_Max;
			_cpuPowerModes = new ObservableCollection<PowerProfile>(_performanceModeManager.SupportedCpuModes);
			_selectedCpuPowerMode = CpuPowerModes.FirstOrDefault((PowerProfile x) => string.Equals(x.Name, _performanceModeManager.CurrentPowerMode.Name, StringComparison.OrdinalIgnoreCase));
			_selectedCpuPowerMode.IsSelected = true;
			RaisePropertyChanged("CpuPowerModes");
			_save = new DelegateCommand(SaveCustomFanMode);
			_saveCustomPowerMode = new DelegateCommand(SetCustomPowerMode);
			_updateSelection = new DelegateCommand(SetDefault);
			_fanModes = new ObservableCollection<string>(_performanceModeManager.SupportedFanModes);
			_fanTypes = new ObservableCollection<string>(_performanceModeManager.SupportedFanTypes);
			_selectedFanType = _fanTypes[0];
			_selectedFanMode = _fanModes.FirstOrDefault((string x) => x.Equals(_performanceModeManager.CurrentFanMode));
			GetTauValues();
			SetPowerModeValues(_selectedCpuPowerMode);
			_events.GetEvent<SwitchCpuModesEvent>().Subscribe(SwitchPowerProfiles);
			UpdateCpuModes();
			_isTempDetailVisible = true;
			SetFanData(0);
		}
	}

	private void SwitchPowerProfiles(PowerProfile powerProfile)
	{
		_selectedCpuPowerMode = powerProfile;
		_selectedCpuPowerMode.IsSelected = true;
		SelectedCpuPowerMode = powerProfile;
		UpdateCpuModes();
		RaisePropertyChanged("CpuPowerModes");
		RaisePropertyChanged("SelectedCpuPowerMode");
	}

	public void PowerLevel1SelectionChanged(object sender, EventArgs args)
	{
		_selectedCpuPowerMode.PL1 = (byte)((Slider)sender).Value;
		_powerSaveButtonEnable = true;
		DefaultSelected = false;
		RaisePropertyChanged("SelectedCpuPowerMode");
		RaisePropertyChanged("DefaultSelected");
		RaisePropertyChanged("CpuPowerModes");
		RaisePropertyChanged("PowerSaveButtonEnable");
	}

	public void PowerLevel2SelectionChanged(object sender, EventArgs args)
	{
		_selectedCpuPowerMode.PL2 = (byte)((Slider)sender).Value;
		_powerSaveButtonEnable = true;
		DefaultSelected = false;
		RaisePropertyChanged("DefaultSelected");
		RaisePropertyChanged("PowerSaveButtonEnable");
	}

	public void TauSelectionChanged(object sender, EventArgs args)
	{
		_selectedCpuPowerMode.Tau = (byte)((Slider)sender).Value;
		_powerSaveButtonEnable = true;
		DefaultSelected = false;
		RaisePropertyChanged("DefaultSelected");
		RaisePropertyChanged("PowerSaveButtonEnable");
	}

	public void FanTurnOnTemperatureChange(object sender, EventArgs args)
	{
		_fanTypes.IndexOf(_selectedFanType);
		CurrentFanProfile.fanSettings[CustomFanModeIndex].PrimaryMinimumTemperature = (byte)((Slider)sender).Value;
		FanOffTempMaxValue = (byte)((Slider)sender).Value;
		if (CurrentFanProfile.fanSettings[CustomFanModeIndex].PrimaryMinimumTemperature < FanTurnOffTemp)
		{
			FanTurnOffTemp = CurrentFanProfile.fanSettings[CustomFanModeIndex].PrimaryMinimumTemperature;
		}
		CurrentFanProfile.fanSettings[CustomFanModeIndex].FanModeBytes[2] = CurrentFanProfile.fanSettings[CustomFanModeIndex].PrimaryMinimumTemperature;
		_saveButtonEnabled = true;
		RaisePropertyChanged("SaveButtonEnabled");
		RaisePropertyChanged("FanOffTempMaxValue");
	}

	public void FanTurnOffTemperatureChange(object sender, EventArgs args)
	{
		_fanTypes.IndexOf(_selectedFanType);
		int num = _fanTurnOnTemp - _fanTurnOffTemp;
		BitArray bitArray = new BitArray(new byte[1] { (byte)num });
		int num2 = CurrentFanProfile.fanSettings[CustomFanModeIndex].PrimaryFanCutOffTemperatureOffsetAndRampDownRate;
		int num3 = 0;
		for (int i = 0; i < 6; i++)
		{
			num3 = i + 2;
			int num4 = Convert.ToInt32(bitArray[i]);
			int num5 = 1 << num3;
			num2 = (num2 & ~num5) | ((num4 << num3) & num5);
		}
		CurrentFanProfile.fanSettings[CustomFanModeIndex].PrimaryFanCutOffTemperatureOffsetAndRampDownRate = (byte)num2;
		if (_fanTurnOffTemp > _fanTurnOnTemp)
		{
			_fanTurnOffTemp = _fanTurnOnTemp;
		}
		CurrentFanProfile.fanSettings[CustomFanModeIndex].FanModeBytes[5] = CurrentFanProfile.fanSettings[CustomFanModeIndex].PrimaryFanCutOffTemperatureOffsetAndRampDownRate;
		_saveButtonEnabled = true;
		RaisePropertyChanged("SaveButtonEnabled");
	}

	public void MinDutyCycleChange(object sender, EventArgs args)
	{
		_fanTypes.IndexOf(_selectedFanType);
		int num = (byte)((Slider)sender).Value;
		BitArray bitArray = new BitArray(new byte[1] { (byte)num });
		int num2 = CurrentFanProfile.fanSettings[CustomFanModeIndex].PrimaryMinimumDutyCycleAndRampAcceleration;
		int num3 = 0;
		for (int i = 0; i < 6; i++)
		{
			num3 = i + 2;
			int num4 = Convert.ToInt32(bitArray[i]);
			int num5 = 1 << num3;
			num2 = (num2 & ~num5) | ((num4 << num3) & num5);
		}
		CurrentFanProfile.fanSettings[CustomFanModeIndex].PrimaryMinimumDutyCycleAndRampAcceleration = (byte)num2;
		CurrentFanProfile.fanSettings[CustomFanModeIndex].FanModeBytes[3] = (byte)num2;
		_saveButtonEnabled = true;
		RaisePropertyChanged("SaveButtonEnabled");
	}

	public void DutyCycleChange(object sender, EventArgs args)
	{
		_fanTypes.IndexOf(_selectedFanType);
		int num = (byte)((Slider)sender).Value;
		BitArray bitArray = new BitArray(new byte[1] { (byte)num });
		int num2 = CurrentFanProfile.fanSettings[CustomFanModeIndex].PrimaryDutyCycleIncrementAndSamplingPeriod;
		int num3 = 0;
		for (int i = 0; i < 4; i++)
		{
			num3 = i + 4;
			int num4 = Convert.ToInt32(bitArray[i]);
			int num5 = 1 << num3;
			num2 = (num2 & ~num5) | ((num4 << num3) & num5);
		}
		CurrentFanProfile.fanSettings[CustomFanModeIndex].PrimaryMinimumDutyCycleAndRampAcceleration = (byte)num2;
		CurrentFanProfile.fanSettings[CustomFanModeIndex].FanModeBytes[4] = CurrentFanProfile.fanSettings[CustomFanModeIndex].PrimaryMinimumDutyCycleAndRampAcceleration;
		_saveButtonEnabled = true;
		RaisePropertyChanged("SaveButtonEnabled");
	}

	public void MaximumDutySliderChange(object sender, EventArgs args)
	{
		_fanTypes.IndexOf(_selectedFanType);
		CurrentFanProfile.fanSettings[CustomFanModeIndex].MaximumDutyCycle = (byte)((Slider)sender).Value;
		CurrentFanProfile.fanSettings[CustomFanModeIndex].FanModeBytes[13] = CurrentFanProfile.fanSettings[CustomFanModeIndex].MaximumDutyCycle;
		_saveButtonEnabled = true;
		RaisePropertyChanged("SaveButtonEnabled");
	}

	public void FixedFanDutyCycleSliderChange(object sender, EventArgs args)
	{
		_fanTypes.IndexOf(_selectedFanType);
		int fanControlModeAndSpeedRampRateLimit = CurrentFanProfile.fanSettings[FixedFanModeIndex].FanControlModeAndSpeedRampRateLimit;
		BitArray bitArray = new BitArray(new byte[1] { (byte)fanControlModeAndSpeedRampRateLimit });
		bitArray[6] = true;
		int[] array = new int[1];
		bitArray.CopyTo(array, 0);
		CurrentFanProfile.fanSettings[FixedFanModeIndex].FanControlModeAndSpeedRampRateLimit = (byte)array[0];
		CurrentFanProfile.fanSettings[FixedFanModeIndex].MaximumDutyCycle = (byte)((Slider)sender).Value;
		CurrentFanProfile.fanSettings[FixedFanModeIndex].FanModeBytes[13] = CurrentFanProfile.fanSettings[FixedFanModeIndex].MaximumDutyCycle;
		CurrentFanProfile.fanSettings[FixedFanModeIndex].FanModeBytes[12] = CurrentFanProfile.fanSettings[FixedFanModeIndex].FanControlModeAndSpeedRampRateLimit;
		_saveButtonEnabled = true;
		RaisePropertyChanged("SaveButtonEnabled");
	}

	private void SetPowerModeValues(PowerProfile profile)
	{
		if (profile.Name == ResourceDictionaryConverter.GetString("Custom"))
		{
			_isSliderEnable = true;
			_showPowerSaveButton = true;
			_powerSaveButtonEnable = true;
			_isCheckboxVisible = true;
		}
		else
		{
			_performanceModeManager.SetPowerMode(_selectedCpuPowerMode);
			_isSliderEnable = false;
			_showPowerSaveButton = false;
			_isCheckboxVisible = false;
			DefaultSelected = false;
			if (!IsFanSliderEnable && !IsSliderEnable)
			{
				_showPowerSaveButton = false;
				_powerSaveButtonEnable = false;
			}
			if (_selectedCpuPowerMode.Name == MAX_PERFORMANCE_MODE)
			{
				_performanceModeManager.SetFansToCoolMode();
				SelectedFantype = FanTypes[0];
			}
		}
		_pl1 = profile.PL1;
		_pl2 = profile.PL2;
		_tau = profile.Tau;
		RefreshUI();
	}

	public void SaveCustomFanMode()
	{
		if (_selectedFanMode == CustomFanMode)
		{
			if (IsFanSliderEnable && _performanceModeManager.SetFanMode(CustomFanMode, SelectedFantype, CurrentFanProfile))
			{
				_saveButtonEnabled = false;
			}
		}
		else if (_selectedFanMode == ResourceDictionaryConverter.GetString(FixedFanMode) && IsFanSliderEnable && _performanceModeManager.SetFanMode(FixedFanMode, SelectedFantype, CurrentFanProfile))
		{
			_saveButtonEnabled = false;
		}
		RaisePropertyChanged("SaveButtonEnabled");
	}

	private void SetCustomPowerMode()
	{
		if (IsSliderEnable && _performanceModeManager.SetPowerMode(_selectedCpuPowerMode))
		{
			int num = _performanceModeManager.SupportedCpuModes.IndexOf(_performanceModeManager.SupportedCpuModes.FirstOrDefault((PowerProfile X) => X.Name == ResourceDictionaryConverter.GetString("Custom")));
			if (num > -1)
			{
				CpuPowerModes.RemoveAt(num);
				_selectedCpuPowerMode.IsSelected = true;
				CpuPowerModes.Insert(num, _selectedCpuPowerMode);
				CpuPowerModes = new ObservableCollection<PowerProfile>(_cpuPowerModes.ToList());
			}
			_powerSaveButtonEnable = false;
			RaisePropertyChanged("CpuPowerModes");
			RaisePropertyChanged("SelectedCpuPowerMode");
			RaisePropertyChanged("PowerSaveButtonEnable");
		}
	}

	private void SetDefault()
	{
		if (DefaultSelected)
		{
			_selectedCpuPowerMode.PL1 = _performanceModeManager.CurrentBiosPowerSetting.PL1;
			_selectedCpuPowerMode.PL2 = _performanceModeManager.CurrentBiosPowerSetting.PL2;
			_selectedCpuPowerMode.Tau = (uint)_performanceModeManager.CurrentBiosPowerSetting.Tau;
			_powerSaveButtonEnable = true;
			RaisePropertyChanged("SelectedCpuPowerMode");
			RaisePropertyChanged("PowerSaveButtonEnable");
			RaisePropertyChanged("DefaultSelected");
			SetPowerModeValues(_selectedCpuPowerMode);
		}
	}

	private void SetFanData(int index)
	{
		_fanProfile = FanProfiles.FirstOrDefault((FanProfileRN x) =>
		{
			string fanType = x.fanType;
			FanType fanType2 = (FanType)index;
			return fanType.Equals(fanType2.ToString());
		});
		if (_selectedFanMode == CustomFanMode)
		{
			_isTempDetailVisible = true;
			FanSettingRN fanSettingRN = default;
			List<byte> list = new List<byte>();
			fanSettingRN = _fanProfile.fanSettings.FirstOrDefault((FanSettingRN x) => x.FanModeName == _fanProfile.CurrentBiosFanMode);
			list = _fanProfile.fanSettings.FirstOrDefault((FanSettingRN x) => x.FanModeName == _fanProfile.CurrentBiosFanMode).FanModeBytes.ToList();
			_fanProfile.fanSettings[CustomFanModeIndex] = fanSettingRN;
			_fanProfile.fanSettings[CustomFanModeIndex].FanModeBytes = list.ToArray();
			_fanProfile.fanSettings[CustomFanModeIndex].FanModeName = CustomFanMode;
			int fanControlModeAndSpeedRampRateLimit = _fanProfile.fanSettings[CustomFanModeIndex].FanControlModeAndSpeedRampRateLimit;
			BitArray bitArray = new BitArray(new byte[1] { (byte)fanControlModeAndSpeedRampRateLimit });
			bitArray[6] = false;
			int[] array = new int[1];
			bitArray.CopyTo(array, 0);
			bitArray.CopyTo(array, 0);
			_fanProfile.fanSettings[CustomFanModeIndex].FanControlModeAndSpeedRampRateLimit = (byte)array[0];
			_fanProfile.fanSettings[CustomFanModeIndex].FanModeBytes[12] = (byte)array[0];
			SetSliderValues(_fanProfile.fanSettings[CustomFanModeIndex]);
			_isFansliderEnable = true;
			_showFanSaveButton = true;
			_saveButtonEnabled = true;
		}
		else if (_selectedFanMode == ResourceDictionaryConverter.GetString(FixedFanMode))
		{
			_isFansliderEnable = true;
			_isTempDetailVisible = false;
			_showFanSaveButton = true;
			_saveButtonEnabled = true;
			FanSettingRN fanSettingRN2 = default;
			List<byte> list2 = new List<byte>();
			fanSettingRN2 = _fanProfile.fanSettings.FirstOrDefault((FanSettingRN x) => x.FanModeName == _fanProfile.CurrentBiosFanMode);
			list2 = _fanProfile.fanSettings.FirstOrDefault((FanSettingRN x) => x.FanModeName == _fanProfile.CurrentBiosFanMode).FanModeBytes.ToList();
			_fanProfile.fanSettings[FixedFanModeIndex] = fanSettingRN2;
			_fanProfile.fanSettings[FixedFanModeIndex].FanModeBytes = list2.ToArray();
			_fixedFanDutyCycle = _fanProfile.fanSettings[FixedFanModeIndex].MaximumDutyCycle;
			_fanProfile.fanSettings[FixedFanModeIndex].FanModeName = FixedFanMode;
			int fanControlModeAndSpeedRampRateLimit2 = _fanProfile.fanSettings[FixedFanModeIndex].FanControlModeAndSpeedRampRateLimit;
			BitArray bitArray2 = new BitArray(new byte[1] { (byte)fanControlModeAndSpeedRampRateLimit2 });
			bitArray2[6] = true;
			int[] array2 = new int[1];
			bitArray2.CopyTo(array2, 0);
			_fanProfile.fanSettings[FixedFanModeIndex].FanControlModeAndSpeedRampRateLimit = (byte)array2[0];
			_fanProfile.fanSettings[FixedFanModeIndex].FanModeBytes[12] = (byte)array2[0];
		}
		else
		{
			_isTempDetailVisible = true;
			_isFansliderEnable = false;
			_showFanSaveButton = false;
			_selectedFanMode = _fanProfile.CurrentBiosFanMode;
			FanSettingRN sliderValues = _fanProfile.fanSettings.FirstOrDefault((FanSettingRN x) => x.FanModeName == _selectedFanMode);
			SetSliderValues(sliderValues);
			if (!IsFanSliderEnable && !IsSliderEnable)
			{
				_showFanSaveButton = false;
				_saveButtonEnabled = true;
			}
			if (_temperatureMax > 0)
			{
				_showTempMaxMin = true;
			}
			else
			{
				_showTempMaxMin = false;
			}
		}
		RefreshUI();
	}

	public void SetSliderValues(FanSettingRN fanSetting)
	{
		_fanTurnOnTemp = fanSetting.PrimaryMinimumTemperature;
		int num = fanSetting.PrimaryFanCutOffTemperatureOffsetAndRampDownRate >> 2;
		_fanTurnOffTemp = _fanTurnOnTemp - num;
		_minFanDutyCycle = fanSetting.PrimaryMinimumDutyCycleAndRampAcceleration >> 2;
		_dutyCycleIncrement = fanSetting.PrimaryDutyCycleIncrementAndSamplingPeriod >> 4;
		_maxFanDutyCycle = fanSetting.MaximumDutyCycle;
		_fanOffTempMaxValue = _fanTurnOnTemp;
	}

	private void GetTauValues()
	{
		_tauValues = new DoubleCollection();
		foreach (double tauValue in _performanceModeManager.TauValues)
		{
			_tauValues.Add(tauValue);
			if (tauValue == Max_Tau)
			{
				break;
			}
		}
		Min_Tau = TauValues[0];
		RaisePropertyChanged("TauValues");
	}

	public void UpdateCpuModes()
	{
		int num = _performanceModeManager.SupportedCpuModes.IndexOf(_performanceModeManager.SupportedCpuModes.FirstOrDefault((PowerProfile X) => X.Name == _selectedCpuPowerMode.Name));
		if (num > -1)
		{
			_cpuPowerModes.RemoveAt(num);
			_cpuPowerModes.Insert(num, _selectedCpuPowerMode);
			_cpuPowerModes = new ObservableCollection<PowerProfile>(_cpuPowerModes.ToList());
		}
	}

	private void RefreshUI()
	{
		RaisePropertyChanged("FanTurnOnTemp");
		RaisePropertyChanged("FanTurnOffTemp");
		RaisePropertyChanged("MinDuty");
		RaisePropertyChanged("MaxFanDutyCycle");
		RaisePropertyChanged("DutyCycleIncrement");
		RaisePropertyChanged("MinFanDutyCycle");
		RaisePropertyChanged("IsFanSliderEnable");
		RaisePropertyChanged("CurrentFanProfile");
		RaisePropertyChanged("ShowFanSaveButton");
		RaisePropertyChanged("ShowPowerSaveButton");
		RaisePropertyChanged("PL1");
		RaisePropertyChanged("PL2");
		RaisePropertyChanged("Tau");
		RaisePropertyChanged("IsSliderEnable");
		RaisePropertyChanged("SaveButtonEnabled");
		RaisePropertyChanged("IsCheckboxVisible");
		RaisePropertyChanged("DefaultSelected");
		RaisePropertyChanged("FanTypes");
		RaisePropertyChanged("SelectedFantype");
		RaisePropertyChanged("IsTempDetailVisible");
		RaisePropertyChanged("FanOffTempMaxValue");
		RaisePropertyChanged("FanOffInfoText");
		RaisePropertyChanged("FixedFanDutyCycle");
		RaisePropertyChanged("SelectedFanMode");
	}
}
