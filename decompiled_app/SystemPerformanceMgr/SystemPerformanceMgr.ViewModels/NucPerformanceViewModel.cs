using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using SystemPerformanceMgr.Interfaces;
using SystemPerformanceMgr.Models;
using CustomizationMgr.Interfaces;
using FeatureMgr.Enums;
using Infrastructure.Converters;
using IntelSdkClient.Events;
using IntelSdkClient.PerformanceTuningService;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace SystemPerformanceMgr.ViewModels;

public class NucPerformanceViewModel : BindableBase
{
	private readonly INucPerformanceModeManager _performanceModeManager;

	private readonly IEventAggregator _events;

	private readonly uint _max_PL1;

	private readonly uint _max_PL2;

	private readonly double _max_Tau;

	private readonly string CustomFanMode = "Custom";

	private readonly string FixedFanMode = "Fixed";

	private readonly int _duty_min;

	private readonly int _duty_max;

	private uint _pl1;

	private uint _pl2;

	private double _tau;

	private DoubleCollection _tauValues;

	private int _fanOffTempMinValue;

	private int _fanOffTempMaxValue;

	private int _fanMinTempMinValue;

	private int _fanMinTempMaxValue;

	private int _fanMaxTempMinValue;

	private int _fanMaxTempMaxValue;

	private int _temperatureMin;

	private int _temperatureMax;

	private int _minDuty;

	private int _dutyIncrement;

	private int _offTemperature;

	private int _fixedFanDutyCycle;

	private PowerSetting _currentBiosSetting;

	private bool _isSliderEnable;

	private bool _isCheckboxVisible;

	private bool _isFansliderEnable;

	private bool _saveButtonEnabled;

	private ObservableCollection<PowerProfile> _cpuPowerModes;

	private PowerProfile _selectedCpuPowerMode;

	private bool _showSaveButton;

	private string _selectedFanMode;

	private FanProfile _fanProfile;

	private FanProfile23 _fanProfile23;

	private FanSetting _currentFanSetting;

	private FanSetting[] _fanSettings;

	private ObservableCollection<string> _fanModes;

	private ObservableCollection<string> _fanTypes;

	private string _selectedFanType;

	private bool _showTempMaxMin;

	private bool _isTempDetailVisible;

	private bool _isFixedFanDetailVisible;

	private bool _isFanOff;

	private bool _isBeastFanGrid;

	private bool _isPhantomFanGrid;

	private NucType _nucType;

	private bool _isFanOffEnable;

	private string _fanOffInfoText;

	private DelegateCommand _save;

	private DelegateCommand _updateSelection;

	public bool Supported { get; }

	public string CpuUsageRegion => "PerformanceTuningCpuRegion";

	public string dGpuUsageRegion => "PerformanceTuningGpuRegion";

	public string PerformanceFanRegion => "PerformanceTuningFanView";

	public string PerformanceFanRegionPH => "PerformanceTuningFanPHView";

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

	public int FanMinTempMinValue
	{
		get
		{
			return _fanMinTempMinValue;
		}
		set
		{
			SetProperty(ref _fanMinTempMinValue, value, "FanMinTempMinValue");
		}
	}

	public int FanMinTempMaxValue
	{
		get
		{
			return _fanMinTempMaxValue;
		}
		set
		{
			SetProperty(ref _fanMinTempMaxValue, value, "FanMinTempMaxValue");
		}
	}

	public int FanMaxTempMinValue
	{
		get
		{
			return _fanMaxTempMinValue;
		}
		set
		{
			SetProperty(ref _fanMaxTempMinValue, value, "FanMaxTempMinValue");
		}
	}

	public int FanMaxTempMaxValue
	{
		get
		{
			return _fanMaxTempMaxValue;
		}
		set
		{
			SetProperty(ref _fanMaxTempMaxValue, value, "FanMaxTempMaxValue");
		}
	}

	public int TemperatureMin
	{
		get
		{
			return _temperatureMin;
		}
		set
		{
			SetProperty(ref _temperatureMin, value, "TemperatureMin");
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

	public int DutyIncrement
	{
		get
		{
			return _dutyIncrement;
		}
		set
		{
			SetProperty(ref _dutyIncrement, value, "DutyIncrement");
		}
	}

	public int OffTemperature
	{
		get
		{
			return _offTemperature;
		}
		set
		{
			SetProperty(ref _offTemperature, value, "OffTemperature");
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

	public bool ShowSaveButton
	{
		get
		{
			return _showSaveButton;
		}
		set
		{
			SetProperty(ref _showSaveButton, value, "ShowSaveButton");
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
			_selectedFanType = FanTypes[0];
			if (NucType != NucType.Serpent)
			{
				if (value != CustomFanMode)
				{
					if (_performanceModeManager.SetFanMode(value))
					{
						SetProperty(ref _selectedFanMode, value, "SelectedFanMode");
						SetFanData(0);
					}
				}
				else
				{
					SetProperty(ref _selectedFanMode, value, "SelectedFanMode");
					SetFanData(0);
				}
			}
			else if (_performanceModeManager.SetFanMode(value))
			{
				SetProperty(ref _selectedFanMode, value, "SelectedFanMode");
				SetFanData(0);
			}
		}
	}

	public FanProfile CurrentFanProfile
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

	public FanProfile23 CurrentFanProfile23
	{
		get
		{
			return _fanProfile23;
		}
		set
		{
			SetProperty(ref _fanProfile23, value, "CurrentFanProfile23");
		}
	}

	public FanSetting CurrentFanSetting
	{
		get
		{
			return _currentFanSetting;
		}
		set
		{
			SetProperty(ref _currentFanSetting, value, "CurrentFanSetting");
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
				int num = FanTypes.IndexOf(_selectedFanType);
				SetFanData(num);
				SetFanMinMax(num);
			}
			else
			{
				SetProperty(ref _selectedFanType, value, "SelectedFantype");
				SetFanData(0);
				SetFanMinMax(0);
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

	public bool IsFanOff
	{
		get
		{
			return _isFanOff;
		}
		set
		{
			SetProperty(ref _isFanOff, value, "IsFanOff");
			SetFanOff();
		}
	}

	public bool IsBeastFanGrid
	{
		get
		{
			return _isBeastFanGrid;
		}
		set
		{
			SetProperty(ref _isBeastFanGrid, value, "IsBeastFanGrid");
		}
	}

	public bool IsPhantomFanGrid
	{
		get
		{
			return _isPhantomFanGrid;
		}
		set
		{
			SetProperty(ref _isPhantomFanGrid, value, "IsPhantomFanGrid");
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

	public bool IsFanOffEnable
	{
		get
		{
			return _isFanOffEnable;
		}
		set
		{
			SetProperty(ref _isFanOffEnable, value, "IsFanOffEnable");
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

	public NucPerformanceViewModel(IEventAggregator events, INucPerformanceModeManager performanceModeManager, ICustomResourceManager resourceManager)
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
			_save = new DelegateCommand(SaveCustomProfile);
			_updateSelection = new DelegateCommand(SetDefault);
			_fanModes = new ObservableCollection<string>(_performanceModeManager.SupportedFanModes);
			_fanTypes = new ObservableCollection<string>(_performanceModeManager.SupportedFanTypes);
			_selectedFanType = _fanTypes[0];
			_fanSettings = _performanceModeManager.FanSettings;
			if (_fanSettings == null)
			{
				_fanSettings = new FanSetting[_fanTypes.Count];
			}
			_selectedFanMode = _fanModes.FirstOrDefault((string x) => x.Equals(_performanceModeManager.CurrentFanMode));
			_nucType = _performanceModeManager.NucType;
			GetTauValues();
			SetPowerModeValues(_selectedCpuPowerMode);
			if (_nucType == NucType.Serpent)
			{
				_fanProfile23 = _performanceModeManager.GetFanProfileData23(_selectedFanMode);
			}
			else
			{
				_fanProfile = _performanceModeManager.GetFanProfileData(_selectedFanMode);
			}
			_events.GetEvent<SwitchCpuModesEvent>().Subscribe(SwitchPowerProfiles);
			UpdateCpuModes();
			NucChecks();
			SetFanData(0);
			if (NucType != NucType.Serpent)
			{
				SetFanMinMax(0);
			}
		}
	}

	private void SetFanMinMax(int FanIndex)
	{
		_fanOffTempMinValue = 0;
		if (_selectedFanMode != CustomFanMode)
		{
			_fanOffTempMaxValue = _fanProfile.fanSettings[FanIndex].PrimaryMinimumTemperature - 1;
			_fanMinTempMinValue = _fanProfile.fanSettings[FanIndex].PrimaryFanOffTemperature + 1;
			_fanMinTempMaxValue = _fanProfile.fanSettings[FanIndex].PrimaryMaximumTemperature - 1;
			_fanMaxTempMinValue = _fanProfile.fanSettings[FanIndex].PrimaryMinimumTemperature + 1;
		}
		else
		{
			_fanOffTempMaxValue = _fanSettings[FanIndex].PrimaryMinimumTemperature - 1;
			_fanMinTempMinValue = _fanSettings[FanIndex].PrimaryFanOffTemperature + 1;
			_fanMinTempMaxValue = _fanSettings[FanIndex].PrimaryMaximumTemperature - 1;
			_fanMaxTempMinValue = _fanSettings[FanIndex].PrimaryMinimumTemperature + 1;
		}
		_fanMaxTempMaxValue = 100;
		RefreshUI();
	}

	private void NucChecks()
	{
		if (_nucType == NucType.Panther || _nucType == NucType.Phantom || _nucType == NucType.Serpent)
		{
			_isTempDetailVisible = false;
		}
		else
		{
			_isTempDetailVisible = true;
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
		_saveButtonEnabled = true;
		DefaultSelected = false;
		RaisePropertyChanged("SelectedCpuPowerMode");
		RaisePropertyChanged("DefaultSelected");
		RaisePropertyChanged("CpuPowerModes");
		RaisePropertyChanged("SaveButtonEnabled");
	}

	public void PowerLevel2SelectionChanged(object sender, EventArgs args)
	{
		_selectedCpuPowerMode.PL2 = (byte)((Slider)sender).Value;
		_saveButtonEnabled = true;
		DefaultSelected = false;
		RaisePropertyChanged("DefaultSelected");
		RaisePropertyChanged("SaveButtonEnabled");
	}

	public void TauSelectionChanged(object sender, EventArgs args)
	{
		_selectedCpuPowerMode.Tau = (byte)((Slider)sender).Value;
		_saveButtonEnabled = true;
		DefaultSelected = false;
		RaisePropertyChanged("DefaultSelected");
		RaisePropertyChanged("SaveButtonEnabled");
	}

	public void MinimumTemperatureChange(object sender, EventArgs args)
	{
		int num = _fanTypes.IndexOf(_selectedFanType);
		_fanSettings[num].PrimaryMinimumTemperature = (byte)((Slider)sender).Value;
		_currentFanSetting.PrimaryMinimumTemperature = (byte)((Slider)sender).Value;
		_saveButtonEnabled = true;
		_fanOffTempMaxValue = (byte)((Slider)sender).Value - 1;
		_fanMaxTempMinValue = (byte)((Slider)sender).Value + 1;
		RaisePropertyChanged("SaveButtonEnabled");
		RaisePropertyChanged("FanOffTempMaxValue");
		RaisePropertyChanged("FanMaxTempMinValue");
	}

	public void MaximumTemperatureChange(object sender, EventArgs args)
	{
		int num = _fanTypes.IndexOf(_selectedFanType);
		_fanSettings[num].PrimaryMaximumTemperature = (byte)((Slider)sender).Value;
		_currentFanSetting.PrimaryMaximumTemperature = (byte)((Slider)sender).Value;
		_saveButtonEnabled = true;
		_fanMinTempMaxValue = (byte)((Slider)sender).Value - 1;
		RaisePropertyChanged("SaveButtonEnabled");
		RaisePropertyChanged("FanMinTempMaxValue");
	}

	public void MinimumDutySelectionChanged(object sender, EventArgs args)
	{
		int num = _fanTypes.IndexOf(_selectedFanType);
		_fanSettings[num].PrimaryMinimumDuty = (byte)((Slider)sender).Value;
		_currentFanSetting.PrimaryMinimumDuty = (byte)((Slider)sender).Value;
		_saveButtonEnabled = true;
		RaisePropertyChanged("SaveButtonEnabled");
	}

	public void DutyIncrementSelectionChanged(object sender, EventArgs args)
	{
		int num = _fanTypes.IndexOf(_selectedFanType);
		_fanSettings[num].PrimaryDutyIncrement = (byte)((Slider)sender).Value;
		_currentFanSetting.PrimaryDutyIncrement = (byte)((Slider)sender).Value;
		_saveButtonEnabled = true;
		RaisePropertyChanged("SaveButtonEnabled");
	}

	public void FanOffTemperatureSelectionChanged(object sender, EventArgs args)
	{
		int num = _fanTypes.IndexOf(_selectedFanType);
		_fanSettings[num].PrimaryFanOffTemperature = (byte)((Slider)sender).Value;
		_currentFanSetting.PrimaryFanOffTemperature = (byte)((Slider)sender).Value;
		_saveButtonEnabled = true;
		_fanMinTempMinValue = (byte)((Slider)sender).Value + 1;
		RaisePropertyChanged("SaveButtonEnabled");
		RaisePropertyChanged("FanMinTempMinValue");
	}

	public void FixedDutyCycleSelectionChanged(object sender, EventArgs args)
	{
		_fanProfile23 = _performanceModeManager.GetFanProfileData23(_selectedFanMode);
		_fixedFanDutyCycle = (byte)((Slider)sender).Value;
		byte[] profile = _fanProfile23.profile;
		if (profile.Length != 0)
		{
			profile[16] = (byte)_fixedFanDutyCycle;
		}
		_performanceModeManager.SetFanDutyCycleFixedMode(_selectedFanMode, profile);
	}

	private void SetPowerModeValues(PowerProfile profile)
	{
		if (profile.Name == ResourceDictionaryConverter.GetString("Custom"))
		{
			if (NucType == NucType.Panther || NucType == NucType.Phantom || _nucType == NucType.Serpent)
			{
				_isSliderEnable = false;
				_showSaveButton = false;
				_isCheckboxVisible = true;
				DefaultSelected = true;
			}
			else
			{
				_isSliderEnable = true;
				_showSaveButton = true;
				_saveButtonEnabled = true;
				_isCheckboxVisible = true;
			}
		}
		else
		{
			_performanceModeManager.SetPowerMode(_selectedCpuPowerMode);
			_isSliderEnable = false;
			_isCheckboxVisible = false;
			DefaultSelected = false;
			if (!IsFanSliderEnable && !IsSliderEnable)
			{
				_showSaveButton = false;
				_saveButtonEnabled = false;
			}
		}
		_pl1 = profile.PL1;
		_pl2 = profile.PL2;
		_tau = profile.Tau;
		RefreshUI();
	}

	public void SaveCustomProfile()
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
			_saveButtonEnabled = false;
			RaisePropertyChanged("CpuPowerModes");
			RaisePropertyChanged("SelectedCpuPowerMode");
		}
		if (IsFanSliderEnable && _performanceModeManager.SetFanCustomMode(_fanSettings))
		{
			_saveButtonEnabled = false;
		}
		RaisePropertyChanged("SaveButtonEnabled");
	}

	private void SetDefault()
	{
		if (DefaultSelected)
		{
			_selectedCpuPowerMode.PL1 = (byte)_performanceModeManager.CurrentBiosPowerSetting.PL1;
			_selectedCpuPowerMode.PL2 = (byte)_performanceModeManager.CurrentBiosPowerSetting.PL2;
			_selectedCpuPowerMode.Tau = (byte)_performanceModeManager.CurrentBiosPowerSetting.Tau;
			_saveButtonEnabled = true;
			RaisePropertyChanged("SelectedCpuPowerMode");
			RaisePropertyChanged("SaveButtonEnabled");
			SetPowerModeValues(_selectedCpuPowerMode);
		}
	}

	private void SetFanOff()
	{
		_performanceModeManager.SetFanOffConfiguration(SelectedFanMode, IsFanOff);
	}

	private void SetFanData(int index)
	{
		if (_nucType == NucType.Serpent)
		{
			_fanProfile23 = _performanceModeManager.GetFanProfileData23(_selectedFanMode);
			if (_selectedFanMode == CustomFanMode)
			{
				IsFanOffEnable = true;
			}
			else
			{
				IsFanOffEnable = false;
			}
			if (_selectedFanMode != NssPerformanceModeManager.FIXED)
			{
				byte secondaryRampAccelerationTemperature = _fanProfile23.fanSettings[0].SecondaryRampAccelerationTemperature;
				if (new BitArray(new byte[1] { secondaryRampAccelerationTemperature })[7])
				{
					_isFanOff = true;
				}
				else
				{
					_isFanOff = false;
				}
				if (_selectedFanMode == CustomFanMode)
				{
					_performanceModeManager.SaveFanOffCapability(_selectedFanMode, _isFanOff);
				}
				_isPhantomFanGrid = true;
				int primaryMinimumTemperature = _fanProfile23.fanSettings[index].PrimaryMinimumTemperature;
				int num = _fanProfile23.fanSettings[index].PrimaryFanCutOffTemperatureOffsetAndRampDownRate >> 2;
				_offTemperature = primaryMinimumTemperature - num;
				_fanOffInfoText = string.Format(ResourceDictionaryConverter.GetString("FanOffInfoText"), OffTemperature);
				IsTempDetailVisible = false;
				IsFixedFanDetailVisible = false;
			}
			else
			{
				_isPhantomFanGrid = true;
				IsTempDetailVisible = true;
				IsFixedFanDetailVisible = true;
				_fixedFanDutyCycle = _performanceModeManager.FixedDutyCycle;
			}
		}
		else
		{
			_fanProfile = _performanceModeManager.GetFanProfileData(_selectedFanMode);
			if (_nucType == NucType.Phantom || _nucType == NucType.Panther)
			{
				IsFanOffEnable = true;
				byte configuration = _fanProfile.fanSettings[0].Configuration2;
				if (new BitArray(new byte[1] { configuration })[7])
				{
					_isFanOff = true;
				}
				else
				{
					_isFanOff = false;
				}
				_performanceModeManager.SaveFanOffCapability(_selectedFanMode, _isFanOff);
				_isPhantomFanGrid = true;
				_offTemperature = _fanProfile.fanSettings[index].PrimaryFanOffTemperature;
				_fanOffInfoText = string.Format(ResourceDictionaryConverter.GetString("FanOffInfoText"), OffTemperature);
			}
			else
			{
				if (_selectedFanMode != CustomFanMode)
				{
					_isFansliderEnable = false;
					_temperatureMin = _fanProfile.fanSettings[index].PrimaryMinimumTemperature;
					_temperatureMax = _fanProfile.fanSettings[index].PrimaryMaximumTemperature;
					_minDuty = _fanProfile.fanSettings[index].PrimaryMinimumDuty;
					_dutyIncrement = _fanProfile.fanSettings[index].PrimaryDutyIncrement;
					_offTemperature = _fanProfile.fanSettings[index].PrimaryFanOffTemperature;
					if (!IsFanSliderEnable && !IsSliderEnable)
					{
						_showSaveButton = false;
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
				else
				{
					_isFansliderEnable = true;
					_saveButtonEnabled = true;
					_showSaveButton = true;
					_temperatureMin = _fanSettings[index].PrimaryMinimumTemperature;
					_temperatureMax = _fanSettings[index].PrimaryMaximumTemperature;
					_minDuty = _fanSettings[index].PrimaryMinimumDuty;
					_dutyIncrement = _fanSettings[index].PrimaryDutyIncrement;
					_offTemperature = _fanSettings[index].PrimaryFanOffTemperature;
					_showTempMaxMin = true;
				}
				IsBeastFanGrid = true;
			}
		}
		RefreshUI();
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
		RaisePropertyChanged("TemperatureMin");
		RaisePropertyChanged("TemperatureMax");
		RaisePropertyChanged("MinDuty");
		RaisePropertyChanged("TemperatureMax");
		RaisePropertyChanged("DutyIncrement");
		RaisePropertyChanged("OffTemperature");
		RaisePropertyChanged("IsFanSliderEnable");
		RaisePropertyChanged("CurrentFanProfile");
		RaisePropertyChanged("ShowSaveButton");
		RaisePropertyChanged("PL1");
		RaisePropertyChanged("PL2");
		RaisePropertyChanged("Tau");
		RaisePropertyChanged("IsSliderEnable");
		RaisePropertyChanged("SaveButtonEnabled");
		RaisePropertyChanged("IsCheckboxVisible");
		RaisePropertyChanged("DefaultSelected");
		RaisePropertyChanged("FanTypes");
		RaisePropertyChanged("SelectedFantype");
		RaisePropertyChanged("ShowTempMaxMin");
		RaisePropertyChanged("IsFanOff");
		RaisePropertyChanged("IsBeastFanGrid");
		RaisePropertyChanged("IsPhantomFanGrid");
		RaisePropertyChanged("FanOffTempMaxValue");
		RaisePropertyChanged("FanMinTempMinValue");
		RaisePropertyChanged("FanMinTempMaxValue");
		RaisePropertyChanged("FanMaxTempMinValue");
		RaisePropertyChanged("FanMaxTempMaxValue");
		RaisePropertyChanged("FanOffInfoText");
		RaisePropertyChanged("FixedFanDutyCycle");
	}
}
