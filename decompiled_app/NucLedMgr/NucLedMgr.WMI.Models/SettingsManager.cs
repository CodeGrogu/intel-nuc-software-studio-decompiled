using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using FeatureMgr.Enums;
using FeatureMgr.Models;
using Infrastructure.Events;
using Infrastructure.Models;
using IntelSdkClient.Alexa;
using IntelSdkClient.NucLed;
using NucLedMgr.Interfaces;
using NucLedMgr.WMI.CpuUsage;
using NucLedMgr.WMI.Enums;
using NucLedMgr.WMI.Events;
using NucLedMgr.WMI.Interfaces;
using NucLedMgr.WMI.Models.ColorTypes;
using NucLedMgr.WMI.Models.Indicators;
using Prism.Events;
using Prism.Logging;
using Prism.Mvvm;

namespace NucLedMgr.WMI.Models;

public class SettingsManager : BindableBase, ISettingsManager
{
	private readonly IEventAggregator _events;

	private readonly IColorManager _colorMgr;

	private readonly ILoggerFacade _logger;

	private readonly NucLedManager _ledService = new NucLedManager();

	private readonly AlexaSettingsManager _alexaSettingsManager = new AlexaSettingsManager();

	private ServiceHost _serviceHost;

	private CpuService _cpuData;

	private readonly CpuServiceClient _client;

	private static readonly string _profilesDirectory = Path.Combine(ApplicationConstants.GetProfileFolderPath(), "LedProfiles\\");

	private const string JSON_FILE_EXTENSION = ".json";

	private const int S0_BRIGHTNESS_INDEX = 0;

	private const int S0_BEHAVIOR_INDEX = 1;

	private const int S0_FREQUENCY_INDEX = 2;

	private const int S0_COLOR_RED_INDEX = 3;

	private const int S0_GREEN_INDEX = 4;

	private const int S0_BLUE_INDEX = 5;

	private const int S3_BRIGHTNESS_INDEX = 6;

	private const int S3_BEHAVIOR_INDEX = 7;

	private const int S3_FREQUENCY_INDEX = 8;

	private const int S3_COLOR_RED_INDEX = 9;

	private const int S3_GREEN_INDEX = 10;

	private const int S3_BLUE_INDEX = 11;

	private const int RM_BRIGHTNESS_INDEX = 12;

	private const int RM_BEHAVIOR_INDEX = 13;

	private const int RM_FREQUENCY_INDEX = 14;

	private const int RM_COLOR_RED_INDEX = 15;

	private const int RM_GREEN_INDEX = 16;

	private const int RM_BLUE_INDEX = 17;

	private const int S5_BRIGHTNESS_INDEX = 18;

	private const int S5_BEHAVIOR_INDEX = 19;

	private const int S5_FREQUENCY_INDEX = 20;

	private const int S5_COLOR_RED_INDEX = 21;

	private const int S5_GREEN_INDEX = 22;

	private const int S5_BLUE_INDEX = 23;

	private const int DEFAULT_BRIGHTNESS = 50;

	private const int SOLID_BEHAVIOR_INDEX = 0;

	private const int BREATHING_BEHAVIOR_INDEX = 1;

	private const int PULSING_BEHAVIOR_INDEX = 2;

	private const int STROBING_BEHAVIOR_INDEX = 3;

	private const int HDD_BRIGHTNESS_INDEX = 0;

	private const int HDD_COLOR_RED_INDEX = 1;

	private const int HDD_GREEN_INDEX = 2;

	private const int HDD_BLUE_INDEX = 3;

	private const int HDD_BEHAVIOR_INDEX = 4;

	private const int ETHERNET_TYPE_INDEX = 0;

	private const int ETHERNET_BRIGHTNESS_INDEX = 1;

	private const int ETHERNET_COLOR_RED_INDEX = 2;

	private const int ETHERNET_GREEN_INDEX = 3;

	private const int ETHERNET_BLUE_INDEX = 4;

	private const int POWERLIMIT_INDICATION_SCHEME_INDEX = 0;

	private const int POWERLIMIT_BRIGHTNESS_INDEX = 1;

	private const int POWERLIMIT_COLOR_RED_INDEX = 2;

	private const int POWERLIMIT_GREEN_INDEX = 3;

	private const int POWERLIMIT_BLUE_INDEX = 4;

	private const int S0_DEFAULT_FREQUENCY = 1;

	private const int S3_DEFAULT_FREQUENCY = 1;

	private const int RM_DEFAULT_FREQUENCY = 10;

	private const int S5_DEFAULT_FREQUENCY = 1;

	private const int HDD_OFF_TO_ON = 0;

	private const int LAN_ONE_AND_TWO = 2;

	private const int GREEN_TO_RED = 0;

	private const int POWER_BUTTON_DEFAULT_COLOR = 0;

	private const int HDD_DEFAULT_COLOR = 2;

	private const int POWER_BUTTON_COLOR_INDEX = 3;

	private readonly List<Type> types = new List<Type>
	{
		typeof(EthernetIndicator_Multi),
		typeof(HDDIndicator_Multi),
		typeof(HDDIndicator_Single),
		typeof(PowerLimitIndicator_Multi),
		typeof(PowerStateIndicator_Multi),
		typeof(PowerStateIndicator_Single),
		typeof(AlexaIndicator),
		typeof(PowerState_Multi),
		typeof(PowerState_Single),
		typeof(SoftwareIndicator_Multi),
		typeof(WifiIndicator_Multi),
		typeof(DisableIndicator),
		typeof(RGBColor),
		typeof(SingleColor),
		typeof(DualColor_Amber),
		typeof(DualColor_White),
		typeof(LedProfile),
		typeof(Led),
		typeof(ControlItem),
		typeof(WmiColorPickerData),
		typeof(NucType)
	};

	private Led _selectedSetting;

	private bool _ledsLoaded;

	private NucType _nucType;

	private ObservableCollection<Led> _currentSettings;

	private LedProfile _currentProfile;

	private bool _disabled = true;

	private ObservableCollection<Led> _previousSettings;

	private ObservableCollection<IIndicator> _indicators;

	private Dictionary<string, ObservableCollection<Led>> _previousProfiles;

	private bool _processingChanges { get; set; }

	public Led PowerButton { get; set; }

	public Led Front1 { get; set; }

	public Led Front2 { get; set; }

	public Led Front3 { get; set; }

	public Led Skull { get; set; }

	public Led Eyes { get; set; }

	public Led Hdd { get; set; }

	public Led SelectedSetting
	{
		get
		{
			return _selectedSetting;
		}
		set
		{
			SetProperty(ref _selectedSetting, value, "SelectedSetting");
			SetLedTypesForSerpent(value);
			RaisePropertyChanged("SelectedLed");
		}
	}

	public bool LedsLoaded
	{
		get
		{
			return _ledsLoaded;
		}
		set
		{
			SetProperty(ref _ledsLoaded, value, "LedsLoaded");
			_events.GetEvent<LedsLoadedEvent>().Publish(value);
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

	public ObservableCollection<Led> CurrentSettings
	{
		get
		{
			return _currentSettings;
		}
		set
		{
			SetProperty(ref _currentSettings, value, "CurrentSettings");
			_currentSettings = new ObservableCollection<Led>(_currentSettings.OrderBy((Led Led) => Led.LedType));
			PopulateLeds();
		}
	}

	public LedProfile CurrentProfile
	{
		get
		{
			return _currentProfile;
		}
		set
		{
			SetProperty(ref _currentProfile, value, "CurrentProfile");
			if (value == null)
			{
				if (GetProfileByName(LedConstants.DEFAULT) != null)
				{
					SetProperty(ref _currentProfile, GetProfileByName(LedConstants.DEFAULT), "CurrentProfile");
				}
				else if (Profiles.Count != 0)
				{
					SetProperty(ref _currentProfile, Profiles[0], "CurrentProfile");
				}
			}
			LedProfile ledProfile = value;
			if (ledProfile != null && !ledProfile.InterfacesSet)
			{
				foreach (Led led in value.Leds)
				{
					led.SetInterfaces(_events, _colorMgr, _client, _ledService, _alexaSettingsManager);
				}
				RaisePropertyChanged("PowerButton");
				RaisePropertyChanged("HDD");
				RaisePropertyChanged("Front1");
				RaisePropertyChanged("Front2");
				RaisePropertyChanged("Front3");
				RaisePropertyChanged("Skull");
				RaisePropertyChanged("Eyes");
				if (value != null)
				{
					value.InterfacesSet = true;
				}
			}
			if (value == null)
			{
				return;
			}
			CurrentSettings = new ObservableCollection<Led>(_currentProfile.Leds);
			if (CurrentSettings.Any((Led x) => x.SelectedIndicator.IndicatorType != IndicatorType.Disable))
			{
				_disabled = false;
			}
			else
			{
				_disabled = true;
			}
			Task.Run(() =>
			{
				SetLeds();
				if (CurrentSettings.Count > 0)
				{
					SelectedSetting = CurrentSettings[0];
				}
				_events.GetEvent<WmiProfileChangedEvent>().Publish(_currentProfile.Name);
				_events.GetEvent<UpdateUIEvent>().Publish();
				_events.GetEvent<ProfileChangedEvent>().Publish(_currentProfile);
				SetLastUsedProfile(value.Name);
			});
		}
	}

	public bool Disabled
	{
		get
		{
			return _disabled;
		}
		set
		{
			SetProperty(ref _disabled, value, "Disabled");
			if (!value)
			{
				ApplyPrevious();
			}
			else
			{
				DisableAll();
			}
			_events.GetEvent<UpdateUIEvent>().Publish();
		}
	}

	public ObservableCollection<Led> PreviousSettings
	{
		get
		{
			return _previousSettings;
		}
		set
		{
			SetProperty(ref _previousSettings, value, "PreviousSettings");
		}
	}

	public ObservableCollection<LedProfile> Profiles { get; }

	public ObservableCollection<IIndicator> Indicators
	{
		get
		{
			return _indicators;
		}
		set
		{
			SetProperty(ref _indicators, value, "Indicators");
		}
	}

	public Dictionary<string, ObservableCollection<Led>> PreviousProfiles
	{
		get
		{
			return _previousProfiles;
		}
		set
		{
			SetProperty(ref _previousProfiles, value, "PreviousProfiles");
		}
	}

	public SettingsManager(IEventAggregator events, IColorManager colorMgr, ILoggerFacade logger)
	{
		_events = events;
		_colorMgr = colorMgr;
		_logger = logger;
		Profiles = new ObservableCollection<LedProfile>();
		Profiles.CollectionChanged += (object s, NotifyCollectionChangedEventArgs e) =>
		{
			UpdateTrayApp();
		};
		CurrentSettings = new ObservableCollection<Led>();
		PreviousSettings = new ObservableCollection<Led>();
		PreviousProfiles = new Dictionary<string, ObservableCollection<Led>>();
		_nucType = PlatformDetail.GetNucType();
		Directory.CreateDirectory(_profilesDirectory);
		if (IsDirectoryEmpty(_profilesDirectory))
		{
			CreateFirstProfile(LedConstants.DEFAULT);
			if (NucType == NucType.Panther)
			{
				ResetPanther();
			}
		}
		else
		{
			GetSavedProfiles();
			if (_currentProfile == null && Profiles.Count > 0)
			{
				_currentProfile = Profiles[0];
			}
			else if (_currentProfile == null && Profiles.Count == 0)
			{
				try
				{
					_currentProfile = Profiles[0];
				}
				catch (Exception exception)
				{
					_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
				}
			}
			_currentSettings = new ObservableCollection<Led>(_currentProfile?.Leds);
			if (_currentSettings.Count > 0)
			{
				SelectedSetting = _currentSettings.First();
				_disabled = true;
				foreach (Led currentSetting in _currentSettings)
				{
					if (currentSetting.SelectedIndicator.IndicatorType != IndicatorType.Disable)
					{
						_disabled = false;
					}
				}
			}
			else
			{
				NucType = NucType.Error;
				_events.GetEvent<LedErrorEvent>().Publish(NucType);
			}
		}
		_events.GetEvent<WmiProfileChangedEvent>().Publish(_currentProfile.Name);
		foreach (Led currentSetting2 in _currentSettings)
		{
			currentSetting2.NucType = NucType;
			_events.GetEvent<LedColorBehaviorChangeEvent>().Publish(currentSetting2);
		}
		PopulateLeds();
		LedsLoaded = true;
		_processingChanges = false;
		RaisePropertyChanged("LedsLoaded");
	}

	private bool IsDirectoryEmpty(string path)
	{
		return !Directory.EnumerateFileSystemEntries(path).Any();
	}

	private void CreateFirstProfile(string name)
	{
		GetSupportedLeds();
		List<Led> leds = new List<Led>(CurrentSettings);
		_currentProfile = new LedProfile(name, leds);
		if (_currentSettings.Count > 0)
		{
			SelectedSetting = _currentSettings.First();
		}
		else
		{
			NucType = NucType.Error;
			_events.GetEvent<LedErrorEvent>().Publish(NucType);
		}
		SaveProfile(CurrentProfile, setAsCurrentProfile: true);
	}

	private void SetLeds()
	{
		LedsLoaded = false;
		foreach (Led currentSetting in _currentSettings)
		{
			LedType ledType = currentSetting.LedType;
			IndicatorType indicatorType = currentSetting.SelectedIndicator.IndicatorType;
			if (Disabled && indicatorType != IndicatorType.Disable)
			{
				Application.Current.Dispatcher.Invoke(() =>
				{
					Disabled = false;
				});
			}
			WmiWrapper.SetIndicator(ledType, indicatorType, _ledService.Wmi, NucType);
			if (NucType != NucType.Serpent)
			{
				foreach (ControlItem controlItem in currentSetting.SelectedIndicator.ControlItems)
				{
					if (controlItem.Supported)
					{
						WmiWrapper.SetControlItem(ledType, indicatorType, controlItem.Index, controlItem.Value, _ledService.Wmi, NucType);
					}
				}
			}
			else if (currentSetting.LedType == LedType.Hdd)
			{
				if (currentSetting.SelectedIndicator.IndicatorType != IndicatorType.Alexa)
				{
					currentSetting.AlexaEnable = false;
					WmiWrapper.SetAlexa(currentSetting.AlexaEnable, _ledService.Wmi);
					SetControlItem(currentSetting);
				}
				else
				{
					currentSetting.AlexaEnable = true;
					WmiWrapper.SetAlexa(currentSetting.AlexaEnable, _ledService.Wmi);
				}
			}
			else
			{
				SetControlItem(currentSetting);
			}
			SelectedSetting = _currentSettings.First();
			_events.GetEvent<LedColorBehaviorChangeEvent>().Publish(currentSetting);
		}
		LedsLoaded = true;
	}

	private void SetControlItem(Led led)
	{
		foreach (ControlItem controlItem in led.SelectedIndicator.ControlItems)
		{
			if (controlItem.Supported)
			{
				WmiWrapper.SetControlItem(led.LedType, led.SelectedIndicator.IndicatorType, controlItem.Index, controlItem.Value, _ledService.Wmi, NucType);
			}
		}
	}

	private byte ColorToEnum(Color color)
	{
		if (color == Colors.Red)
		{
			return 1;
		}
		if (color == Colors.Orange)
		{
			return 2;
		}
		if (color == Colors.Yellow)
		{
			return 3;
		}
		if (color == Colors.Green)
		{
			return 4;
		}
		if (color == Colors.Blue)
		{
			return 5;
		}
		if (color == Colors.Indigo)
		{
			return 6;
		}
		if (color == Colors.Violet)
		{
			return 7;
		}
		if (color == Colors.White)
		{
			return 8;
		}
		return 2;
	}

	private void PopulateLeds()
	{
		PowerButton = CurrentSettings.FirstOrDefault((Led x) => x.LedType == LedType.PowerButton);
		Front1 = CurrentSettings.FirstOrDefault((Led x) => x.LedType == LedType.Front1);
		Front2 = CurrentSettings.FirstOrDefault((Led x) => x.LedType == LedType.Front2);
		Front3 = CurrentSettings.FirstOrDefault((Led x) => x.LedType == LedType.Front3);
		Skull = CurrentSettings.FirstOrDefault((Led x) => x.LedType == LedType.Skull);
		Eyes = CurrentSettings.FirstOrDefault((Led x) => x.LedType == LedType.Eyes);
		Hdd = CurrentSettings.FirstOrDefault((Led x) => x.LedType == LedType.Hdd);
	}

	private void OpenCpuService()
	{
		_cpuData = new CpuService(_events, _logger);
		_serviceHost = new ServiceHost(_cpuData, new Uri("net.pipe://localhost/CpuService"));
		_serviceHost.AddServiceEndpoint(typeof(ICpuService), new NetNamedPipeBinding(), "NssCpuService");
		_serviceHost.Open();
	}

	public Task SaveProfile(LedProfile profile, bool setAsCurrentProfile)
	{
		return Task.Run(() =>
		{
			try
			{
				NssJsonSerializer.SerializeWithBinding(profile, Path.Combine(_profilesDirectory, profile.Name + ".json"), types);
			}
			catch (Exception exception)
			{
				_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
				return;
			}
			Application.Current.Dispatcher.Invoke(() =>
			{
				if (!Profiles.Contains(GetProfileByName(profile.Name)))
				{
					Profiles.Add(profile);
					PopulateLeds();
				}
			});
			if (setAsCurrentProfile && CurrentProfile != null && !profile.Name.Equals(CurrentProfile.Name))
			{
				CurrentProfile = GetProfileByName(profile.Name);
			}
			UpdateTrayApp();
		});
	}

	private void GetSupportedLeds()
	{
		CurrentSettings = new ObservableCollection<Led>();
		foreach (LedType supportedLed in WmiWrapper.GetSupportedLeds(_ledService.Wmi, NucType))
		{
			Led led = new Led(_ledService, _alexaSettingsManager, supportedLed, _colorMgr, _events, _client, NucType);
			if (led.SelectedIndicator != null)
			{
				CurrentSettings.Add(led);
				if (led.SelectedIndicator.IndicatorType != IndicatorType.Disable)
				{
					_disabled = false;
				}
			}
		}
	}

	private void GetSavedProfiles()
	{
		try
		{
			List<string> profiles = Directory.GetFiles(_profilesDirectory).ToList();
			DirectoryInfo directoryInfo = new DirectoryInfo(_profilesDirectory);
			string text = _profilesDirectory + (from f in directoryInfo.GetFiles()
				orderby f.LastWriteTime descending
				select f).First().ToString();
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text);
			ProcessJsonFile(text);
			profiles.Remove(text);
			_currentProfile = GetProfileByName(fileNameWithoutExtension);
			foreach (Led led in _currentProfile.Leds)
			{
				led.SetInterfaces(_events, _colorMgr, _client, _ledService, _alexaSettingsManager);
			}
			Task.Run(() =>
			{
				foreach (string item in profiles)
				{
					ProcessJsonFile(item);
				}
			});
		}
		catch (Exception exception)
		{
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
		}
	}

	public void ProcessJsonFile(string fileName, string newName = null)
	{
		string name = Path.GetFileNameWithoutExtension(fileName);
		if (newName != null)
		{
			fileName = _profilesDirectory + fileName + ".json";
			name = newName;
		}
		try
		{
			if (GetProfileByName(name) != null)
			{
				return;
			}
			LedProfile ledProf = NssJsonSerializer.DeserializeWithBinding<LedProfile>(fileName, types);
			if (ledProf != null)
			{
				Application.Current.Dispatcher.Invoke(() =>
				{
					if (newName != null)
					{
						ledProf.Name = newName;
					}
					if (ledProf.Name == LedConstants.DEFAULT)
					{
						ledProf.Default = true;
					}
					else
					{
						ledProf.Default = false;
					}
					if (ledProf.Leds != null)
					{
						for (int i = 0; i < ledProf.Leds.Count; i++)
						{
							ledProf.Leds[i].NucType = NucType;
						}
					}
					Profiles.Add(ledProf);
				});
			}
			else
			{
				CreateFirstProfile(name);
				if (!Profiles.Contains(GetProfileByName(_currentProfile.Name)))
				{
					Profiles.Add(_currentProfile);
				}
			}
		}
		catch (Exception exception)
		{
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
		}
	}

	public LedProfile GetProfileByName(string name)
	{
		if (Profiles == null || Profiles.Count == 0)
		{
			return null;
		}
		return Profiles.FirstOrDefault((LedProfile f) => f.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
	}

	public void RemoveProfiles(List<string> names)
	{
		Task.Run(() =>
		{
			foreach (string name in names)
			{
				LedProfile profile = GetProfileByName(name);
				if (profile != null && Profiles.Count > 1)
				{
					Application.Current.Dispatcher.Invoke(() =>
					{
						Profiles.Remove(profile);
					});
					if (profile.Name.Equals(CurrentProfile.Name))
					{
						CurrentProfile = GetProfileByName(LedConstants.DEFAULT) ?? Profiles[0];
					}
					else
					{
						CurrentProfile = GetProfileByName(CurrentProfile.Name) ?? Profiles[0];
					}
					File.Delete(_profilesDirectory + profile.Name + ".json");
				}
				if (PreviousProfiles.ContainsKey(name))
				{
					PreviousProfiles.Remove(name);
				}
			}
		});
		Task.Run(() =>
		{
			List<string> savedProfileNames = GetSavedProfileNames();
			foreach (string name2 in names)
			{
				if (savedProfileNames.Contains(name2))
				{
					File.Delete(_profilesDirectory + name2 + ".json");
				}
			}
		});
	}

	public Task RemoveAllProfileFiles()
	{
		return Task.Run(() =>
		{
			foreach (string item in Directory.GetFiles(_profilesDirectory).ToList())
			{
				File.Delete(item);
			}
		});
	}

	private void UpdateTrayApp()
	{
		Dictionary<string, EventHandler> dictionary = new Dictionary<string, EventHandler>();
		foreach (LedProfile profile in Profiles)
		{
			dictionary.Add(profile.Name, (object s, EventArgs e) =>
			{
				CurrentProfile = profile;
			});
		}
		if (Profiles.Count > 0)
		{
			_events.GetEvent<EneLedProfilesChangedEvent>().Publish(dictionary);
			if (_currentProfile == null && Profiles.Count > 0)
			{
				_currentProfile = Profiles[0];
			}
			if (CurrentProfile != null)
			{
				_events.GetEvent<EneProfileChangedEvent>().Publish(CurrentProfile.Name);
			}
		}
	}

	public void Reset()
	{
		if (_processingChanges)
		{
			return;
		}
		_processingChanges = true;
		_disabled = false;
		Task.Run(() =>
		{
			foreach (Led currentSetting in _currentSettings)
			{
				LedsLoaded = false;
				switch (currentSetting.LedType)
				{
				case LedType.PowerButton:
				case LedType.Skull:
					WmiWrapper.SetIndicator(currentSetting.LedType, IndicatorType.PowerState, _ledService.Wmi, NucType);
					if (currentSetting.S0Supported)
					{
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 0, 50, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 1, 0, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 2, 1, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 3, 0, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 4, 0, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 5, byte.MaxValue, _ledService.Wmi, NucType);
					}
					if (currentSetting.S3Supported)
					{
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 6, 50, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 7, 1, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 8, 1, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 9, 0, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 10, 0, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 11, byte.MaxValue, _ledService.Wmi, NucType);
					}
					if (currentSetting.ReadyModeSupported)
					{
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 12, 50, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 13, 1, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 14, 10, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 15, 0, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 16, 0, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 17, byte.MaxValue, _ledService.Wmi, NucType);
					}
					if (currentSetting.S5Supported)
					{
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 18, 50, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 19, 1, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 20, 1, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 21, 0, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 22, 0, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 23, byte.MaxValue, _ledService.Wmi, NucType);
					}
					currentSetting.GetSelectedIndicator();
					currentSetting.SetGradient();
					currentSetting.SetBehaviorAndFrequency();
					break;
				case LedType.Eyes:
					WmiWrapper.SetIndicator(currentSetting.LedType, IndicatorType.PowerState, _ledService.Wmi, NucType);
					if (currentSetting.S0Supported)
					{
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 0, 50, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 1, 0, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 2, 1, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 3, byte.MaxValue, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 4, 0, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 5, 0, _ledService.Wmi, NucType);
					}
					if (currentSetting.S3Supported)
					{
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 6, 50, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 7, 1, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 8, 1, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 9, byte.MaxValue, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 10, 0, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 11, 0, _ledService.Wmi, NucType);
					}
					if (currentSetting.ReadyModeSupported)
					{
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 12, 50, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 13, 1, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 14, 10, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 15, byte.MaxValue, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 16, 0, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 17, 0, _ledService.Wmi, NucType);
					}
					if (currentSetting.S5Supported)
					{
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 18, 50, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 19, 1, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 20, 1, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 21, byte.MaxValue, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 22, 0, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 23, 0, _ledService.Wmi, NucType);
					}
					currentSetting.GetSelectedIndicator();
					currentSetting.SetGradient();
					currentSetting.SetBehaviorAndFrequency();
					break;
				case LedType.Hdd:
				case LedType.Front1:
					if (NucType == NucType.Serpent)
					{
						currentSetting.AlexaEnable = false;
						WmiWrapper.SetAlexa(currentSetting.AlexaEnable, _ledService.Wmi);
						WmiWrapper.SetIndicator(currentSetting.LedType, IndicatorType.HddActivity, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.HddActivity, 0, 50, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.HddActivity, 1, 0, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.HddActivity, 2, byte.MaxValue, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.HddActivity, 3, 0, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.HddActivity, 4, 0, _ledService.Wmi, NucType);
					}
					else
					{
						WmiWrapper.SetIndicator(currentSetting.LedType, IndicatorType.HddActivity, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.HddActivity, 0, 50, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.HddActivity, 1, byte.MaxValue, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.HddActivity, 2, 0, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.HddActivity, 3, 0, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.HddActivity, 4, 0, _ledService.Wmi, NucType);
					}
					currentSetting.GetSelectedIndicator();
					currentSetting.SetGradient();
					currentSetting.SetBehaviorAndFrequency();
					break;
				case LedType.Front2:
					WmiWrapper.SetIndicator(currentSetting.LedType, IndicatorType.Ethernet, _ledService.Wmi, NucType);
					WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.Ethernet, 0, 2, _ledService.Wmi, NucType);
					WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.Ethernet, 1, 50, _ledService.Wmi, NucType);
					WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.Ethernet, 2, 0, _ledService.Wmi, NucType);
					WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.Ethernet, 3, 128, _ledService.Wmi, NucType);
					WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.Ethernet, 4, 0, _ledService.Wmi, NucType);
					currentSetting.GetSelectedIndicator();
					currentSetting.SetGradient();
					currentSetting.SetBehaviorAndFrequency();
					break;
				case LedType.Front3:
					WmiWrapper.SetIndicator(currentSetting.LedType, IndicatorType.PowerLimit, _ledService.Wmi, NucType);
					WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerLimit, 0, 0, _ledService.Wmi, NucType);
					WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerLimit, 1, 50, _ledService.Wmi, NucType);
					WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerLimit, 2, 75, _ledService.Wmi, NucType);
					WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerLimit, 3, 0, _ledService.Wmi, NucType);
					WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerLimit, 4, 130, _ledService.Wmi, NucType);
					currentSetting.GetSelectedIndicator();
					currentSetting.SetGradient();
					currentSetting.SetBehaviorAndFrequency();
					break;
				}
			}
			Application.Current.Dispatcher.Invoke(() =>
			{
				foreach (Led currentSetting2 in _currentSettings)
				{
					currentSetting2.GetSelectedColors();
					currentSetting2.GetSupportedIndicators();
					currentSetting2.RefreshUi();
					_events.GetEvent<UpdateUIEvent>().Publish();
				}
				PopulateLeds();
				RaisePropertyChanged("PowerButton");
				RaisePropertyChanged("HDD");
				RaisePropertyChanged("Front1");
				RaisePropertyChanged("Front2");
				RaisePropertyChanged("Front3");
				RaisePropertyChanged("Skull");
				RaisePropertyChanged("Eyes");
				RaisePropertyChanged("CurrentProfile");
			});
			ResetRecheck();
			_processingChanges = false;
			LedsLoaded = true;
		});
	}

	private void SetLedTypesForSerpent(Led settings)
	{
		if (_nucType != NucType.Serpent)
		{
			if (SelectedSetting.SupportedIndicators != null)
			{
				_indicators = new ObservableCollection<IIndicator>(SelectedSetting.SupportedIndicators.OrderBy((IIndicator Indicator) => Indicator.IndicatorType));
				_selectedSetting.GetIndicatorFromCollection();
			}
		}
		else
		{
			if (SelectedSetting.SupportedIndicators == null)
			{
				return;
			}
			if (settings.Name == LedConstants.MULTIPURPOSELED_NAME)
			{
				IEnumerable<IIndicator> enumerable = SelectedSetting.SupportedIndicators.Where((IIndicator x) => x.IndicatorType == IndicatorType.PowerState);
				if (enumerable != null && enumerable.ToList().Count > 0)
				{
					IIndicator item = SelectedSetting.SupportedIndicators.Where((IIndicator x) => x.IndicatorType == IndicatorType.PowerState)?.ToList()[0];
					SelectedSetting.SupportedIndicators.Remove(item);
				}
			}
			else
			{
				IEnumerable<IIndicator> enumerable2 = SelectedSetting.SupportedIndicators.Where((IIndicator x) => x.IndicatorType == IndicatorType.Alexa);
				if (enumerable2 != null && enumerable2.ToList().Count > 0)
				{
					IIndicator item2 = SelectedSetting.SupportedIndicators.Where((IIndicator x) => x.IndicatorType == IndicatorType.Alexa)?.ToList()[0];
					SelectedSetting.SupportedIndicators.Remove(item2);
				}
			}
			_indicators = new ObservableCollection<IIndicator>(SelectedSetting.SupportedIndicators.OrderBy((IIndicator Indicator) => Indicator.IndicatorType));
			_selectedSetting.GetIndicatorFromCollection();
		}
	}

	public void ResetPanther()
	{
		if (_processingChanges)
		{
			return;
		}
		_processingChanges = true;
		_disabled = false;
		Task.Run(() =>
		{
			foreach (Led currentSetting in _currentSettings)
			{
				switch (currentSetting.LedType)
				{
				case LedType.PowerButton:
					WmiWrapper.SetIndicator(currentSetting.LedType, IndicatorType.PowerState, _ledService.Wmi, NucType);
					if (currentSetting.ColorType == ColorType.DualAmber)
					{
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 3, 0, _ledService.Wmi, NucType);
					}
					if (currentSetting.S0Supported)
					{
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 0, 50, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 1, 0, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 2, 1, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 3, 0, _ledService.Wmi, NucType);
					}
					if (currentSetting.ReadyModeSupported)
					{
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 12, 50, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 13, 1, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 14, 10, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.PowerState, 15, 0, _ledService.Wmi, NucType);
					}
					currentSetting.GetSelectedIndicator();
					currentSetting.SetGradient();
					currentSetting.SetBehaviorAndFrequency();
					currentSetting.GetSelectedColors();
					break;
				case LedType.Hdd:
					WmiWrapper.SetIndicator(currentSetting.LedType, IndicatorType.HddActivity, _ledService.Wmi, NucType);
					if (currentSetting.ColorType == ColorType.RGB)
					{
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.HddActivity, 1, 2, _ledService.Wmi, NucType);
						WmiWrapper.SetControlItem(currentSetting.LedType, IndicatorType.HddActivity, 4, 0, _ledService.Wmi, NucType);
					}
					currentSetting.GetSelectedIndicator();
					currentSetting.SetGradient();
					currentSetting.SetBehaviorAndFrequency();
					currentSetting.GetSelectedColors();
					break;
				}
			}
			Application.Current.Dispatcher.Invoke(() =>
			{
				foreach (Led currentSetting2 in _currentSettings)
				{
					currentSetting2.GetSelectedColors();
				}
				PopulateLeds();
				_selectedSetting.GetIndicatorFromCollection();
				RaisePropertyChanged("PowerButton");
				RaisePropertyChanged("HDD");
			});
			ResetRecheck();
			_processingChanges = false;
		});
	}

	private void Reset(Led setting)
	{
		switch (setting.LedType)
		{
		case LedType.PowerButton:
		case LedType.Skull:
			WmiWrapper.SetIndicator(setting.LedType, IndicatorType.PowerState, _ledService.Wmi, NucType);
			if (setting.ColorType == ColorType.DualAmber)
			{
				WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 3, 0, _ledService.Wmi, NucType);
			}
			else
			{
				if (setting.S0Supported)
				{
					WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 0, 50, _ledService.Wmi, NucType);
					WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 1, 0, _ledService.Wmi, NucType);
					WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 2, 1, _ledService.Wmi, NucType);
					WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 3, 0, _ledService.Wmi, NucType);
					WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 4, 0, _ledService.Wmi, NucType);
					WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 5, byte.MaxValue, _ledService.Wmi, NucType);
				}
				if (setting.S3Supported)
				{
					WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 6, 50, _ledService.Wmi, NucType);
					WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 7, 1, _ledService.Wmi, NucType);
					WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 8, 1, _ledService.Wmi, NucType);
					WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 9, 0, _ledService.Wmi, NucType);
					WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 10, 0, _ledService.Wmi, NucType);
					WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 11, byte.MaxValue, _ledService.Wmi, NucType);
				}
				if (setting.ReadyModeSupported)
				{
					WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 12, 50, _ledService.Wmi, NucType);
					WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 13, 1, _ledService.Wmi, NucType);
					WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 14, 10, _ledService.Wmi, NucType);
					WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 15, 0, _ledService.Wmi, NucType);
					WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 16, 0, _ledService.Wmi, NucType);
					WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 17, byte.MaxValue, _ledService.Wmi, NucType);
				}
				if (setting.S5Supported)
				{
					WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 18, 50, _ledService.Wmi, NucType);
					WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 19, 1, _ledService.Wmi, NucType);
					WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 20, 1, _ledService.Wmi, NucType);
					WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 21, 0, _ledService.Wmi, NucType);
					WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 22, 0, _ledService.Wmi, NucType);
					WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 23, byte.MaxValue, _ledService.Wmi, NucType);
				}
			}
			setting.GetSelectedIndicator();
			setting.SetGradient();
			setting.SetBehaviorAndFrequency();
			setting.GetSelectedColors();
			break;
		case LedType.Eyes:
			WmiWrapper.SetIndicator(setting.LedType, IndicatorType.PowerState, _ledService.Wmi, NucType);
			if (setting.S0Supported)
			{
				WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 0, 50, _ledService.Wmi, NucType);
				WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 1, 0, _ledService.Wmi, NucType);
				WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 2, 1, _ledService.Wmi, NucType);
				WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 3, byte.MaxValue, _ledService.Wmi, NucType);
				WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 4, 0, _ledService.Wmi, NucType);
				WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 5, 0, _ledService.Wmi, NucType);
			}
			if (setting.S3Supported)
			{
				WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 6, 50, _ledService.Wmi, NucType);
				WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 7, 1, _ledService.Wmi, NucType);
				WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 8, 1, _ledService.Wmi, NucType);
				WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 9, byte.MaxValue, _ledService.Wmi, NucType);
				WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 10, 0, _ledService.Wmi, NucType);
				WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 11, 0, _ledService.Wmi, NucType);
			}
			if (setting.ReadyModeSupported)
			{
				WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 12, 50, _ledService.Wmi, NucType);
				WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 13, 1, _ledService.Wmi, NucType);
				WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 14, 10, _ledService.Wmi, NucType);
				WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 15, byte.MaxValue, _ledService.Wmi, NucType);
				WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 16, 0, _ledService.Wmi, NucType);
				WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 17, 0, _ledService.Wmi, NucType);
			}
			if (setting.S5Supported)
			{
				WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 18, 50, _ledService.Wmi, NucType);
				WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 19, 1, _ledService.Wmi, NucType);
				WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 20, 1, _ledService.Wmi, NucType);
				WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 21, byte.MaxValue, _ledService.Wmi, NucType);
				WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 22, 0, _ledService.Wmi, NucType);
				WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerState, 23, 0, _ledService.Wmi, NucType);
			}
			setting.GetSelectedIndicator();
			setting.SetGradient();
			setting.SetBehaviorAndFrequency();
			setting.GetSelectedColors();
			break;
		case LedType.Hdd:
			WmiWrapper.SetIndicator(setting.LedType, IndicatorType.HddActivity, _ledService.Wmi, NucType);
			if (setting.ColorType == ColorType.RGB)
			{
				WmiWrapper.SetControlItem(setting.LedType, IndicatorType.HddActivity, 1, 2, _ledService.Wmi, NucType);
			}
			setting.GetSelectedIndicator();
			setting.SetGradient();
			setting.SetBehaviorAndFrequency();
			setting.GetSelectedColors();
			break;
		case LedType.Front1:
			WmiWrapper.SetIndicator(setting.LedType, IndicatorType.HddActivity, _ledService.Wmi, NucType);
			WmiWrapper.SetControlItem(setting.LedType, IndicatorType.HddActivity, 0, 50, _ledService.Wmi, NucType);
			WmiWrapper.SetControlItem(setting.LedType, IndicatorType.HddActivity, 1, byte.MaxValue, _ledService.Wmi, NucType);
			WmiWrapper.SetControlItem(setting.LedType, IndicatorType.HddActivity, 2, 0, _ledService.Wmi, NucType);
			WmiWrapper.SetControlItem(setting.LedType, IndicatorType.HddActivity, 3, 0, _ledService.Wmi, NucType);
			WmiWrapper.SetControlItem(setting.LedType, IndicatorType.HddActivity, 4, 0, _ledService.Wmi, NucType);
			setting.GetSelectedIndicator();
			setting.SetGradient();
			setting.SetBehaviorAndFrequency();
			setting.GetSelectedColors();
			break;
		case LedType.Front2:
			WmiWrapper.SetIndicator(setting.LedType, IndicatorType.Ethernet, _ledService.Wmi, NucType);
			WmiWrapper.SetControlItem(setting.LedType, IndicatorType.Ethernet, 0, 2, _ledService.Wmi, NucType);
			WmiWrapper.SetControlItem(setting.LedType, IndicatorType.Ethernet, 1, 50, _ledService.Wmi, NucType);
			WmiWrapper.SetControlItem(setting.LedType, IndicatorType.Ethernet, 2, 0, _ledService.Wmi, NucType);
			WmiWrapper.SetControlItem(setting.LedType, IndicatorType.Ethernet, 3, 128, _ledService.Wmi, NucType);
			WmiWrapper.SetControlItem(setting.LedType, IndicatorType.Ethernet, 4, 0, _ledService.Wmi, NucType);
			setting.GetSelectedIndicator();
			setting.SetGradient();
			setting.SetBehaviorAndFrequency();
			setting.GetSelectedColors();
			break;
		case LedType.Front3:
			WmiWrapper.SetIndicator(setting.LedType, IndicatorType.PowerLimit, _ledService.Wmi, NucType);
			WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerLimit, 0, 0, _ledService.Wmi, NucType);
			WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerLimit, 1, 50, _ledService.Wmi, NucType);
			WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerLimit, 2, 75, _ledService.Wmi, NucType);
			WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerLimit, 3, 0, _ledService.Wmi, NucType);
			WmiWrapper.SetControlItem(setting.LedType, IndicatorType.PowerLimit, 4, 130, _ledService.Wmi, NucType);
			setting.GetSelectedIndicator();
			setting.SetGradient();
			setting.SetBehaviorAndFrequency();
			setting.GetSelectedColors();
			break;
		}
	}

	private void ResetRecheck()
	{
		int num = 0;
		foreach (Led currentSetting in _currentSettings)
		{
			switch (currentSetting.LedType)
			{
			case LedType.PowerButton:
			case LedType.Skull:
				if (currentSetting.SelectedColor != Color.FromRgb(0, 0, byte.MaxValue))
				{
					num++;
					Reset(currentSetting);
				}
				break;
			case LedType.Hdd:
				if (NucType == NucType.Serpent)
				{
					if (currentSetting.SelectedColor != Color.FromRgb(byte.MaxValue, 0, 0))
					{
						num++;
						Reset(currentSetting);
					}
				}
				else if (currentSetting.SelectedColor != Colors.Orange)
				{
					num++;
					Reset(currentSetting);
				}
				break;
			case LedType.Eyes:
			case LedType.Front1:
				if (currentSetting.SelectedColor != Color.FromRgb(byte.MaxValue, 0, 0))
				{
					num++;
					Reset(currentSetting);
				}
				break;
			case LedType.Front2:
			case LedType.Front3:
				if (currentSetting.SelectedColor != Color.FromRgb(0, 128, 0))
				{
					num++;
					Reset(currentSetting);
				}
				break;
			}
		}
		if (num <= 0)
		{
			return;
		}
		Application.Current.Dispatcher.Invoke(() =>
		{
			foreach (Led currentSetting2 in _currentSettings)
			{
				currentSetting2.GetSelectedColors();
			}
			PopulateLeds();
			_selectedSetting.GetIndicatorFromCollection();
			RaisePropertyChanged("PowerButton");
			RaisePropertyChanged("HDD");
			RaisePropertyChanged("Front1");
			RaisePropertyChanged("Front2");
			RaisePropertyChanged("Front3");
			RaisePropertyChanged("Skull");
			RaisePropertyChanged("Eyes");
		});
	}

	public void DisableAll()
	{
		_events.GetEvent<ProcessingCompleteEvent>().Publish(payload: false);
		_disabled = true;
		Task.Run(() =>
		{
			LedsLoaded = false;
			List<Led> list = CurrentSettings.Select((Led objEntity) => (Led)objEntity.Clone()).ToList();
			PreviousSettings = new ObservableCollection<Led>(list);
			if (!PreviousProfiles.ContainsKey(CurrentProfile.Name))
			{
				PreviousProfiles.Add(CurrentProfile.Name, PreviousSettings);
			}
			else
			{
				PreviousProfiles[CurrentProfile.Name] = PreviousSettings;
			}
			int i = 0;
			while (i < _currentSettings.Count)
			{
				WmiWrapper.SetIndicator(_currentSettings[i].LedType, IndicatorType.Disable, _ledService.Wmi, NucType);
				_currentSettings[i].GetSelectedIndicator();
				Application.Current.Dispatcher.Invoke(() =>
				{
					_currentSettings[i].GetSelectedColors();
					_currentSettings[i].RefreshUi();
					_events.GetEvent<LedColorBehaviorChangeEvent>().Publish(_currentSettings[i]);
				});
				int num = i + 1;
				i = num;
			}
			RaisePropertyChanged("PowerButton");
			RaisePropertyChanged("HDD");
			RaisePropertyChanged("Front1");
			RaisePropertyChanged("Front2");
			RaisePropertyChanged("Front3");
			RaisePropertyChanged("Skull");
			RaisePropertyChanged("Eyes");
			SetLeds();
			PopulateLeds();
			SaveProfile(_currentProfile, setAsCurrentProfile: true);
			_events.GetEvent<ProcessingCompleteEvent>().Publish(payload: true);
			LedsLoaded = true;
		});
	}

	private void SetLastUsedProfile(string profName)
	{
		try
		{
			File.SetLastWriteTime(_profilesDirectory + profName + ".json", DateTime.Now);
		}
		catch
		{
		}
	}

	public void ApplyPrevious()
	{
		_events.GetEvent<ProcessingCompleteEvent>().Publish(payload: false);
		if (PreviousProfiles.ContainsKey(CurrentProfile.Name))
		{
			PreviousSettings = PreviousProfiles[CurrentProfile.Name];
		}
		if (PreviousSettings.Count == 0)
		{
			_processingChanges = false;
			_events.GetEvent<ProcessingCompleteEvent>().Publish(payload: true);
			Reset();
			return;
		}
		LedProfile previousVersion = new LedProfile(CurrentProfile.Name, PreviousSettings.ToList());
		try
		{
			NssJsonSerializer.SerializeWithBinding(previousVersion, Path.Combine(_profilesDirectory, previousVersion.Name + ".json"), types);
			previousVersion = NssJsonSerializer.DeserializeWithBinding<LedProfile>(_profilesDirectory + previousVersion.Name + ".json", types);
			Application.Current.Dispatcher.Invoke(() =>
			{
				Profiles.Remove(_currentProfile);
				Profiles.Add(previousVersion);
				CurrentProfile = GetProfileByName(previousVersion.Name);
			});
			if (_currentProfile != null && NucType != NucType.Serpent)
			{
				CurrentProfile = GetProfileByName(_currentProfile.Name);
			}
		}
		catch (Exception exception)
		{
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
			return;
		}
		_disabled = false;
		_events.GetEvent<ProcessingCompleteEvent>().Publish(payload: true);
		_processingChanges = false;
	}

	public List<string> GetSavedProfileNames()
	{
		List<string> list = new List<string>();
		foreach (string item in Directory.GetFiles(_profilesDirectory).ToList())
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(item);
			list.Add(fileNameWithoutExtension);
		}
		return list;
	}
}
