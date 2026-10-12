using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using FeatureMgr.Enums;
using FeatureMgr.Models;
using Infrastructure.Events;
using Infrastructure.Models;
using IntelSdkClient.Arduino.Interface;
using IntelSdkClient.ArduinoService;
using NucLedMgr.Arduino.Enums;
using NucLedMgr.Arduino.Events;
using NucLedMgr.Arduino.Interfaces;
using NucLedMgr.Interfaces;
using NucLedMgr.WMI.Models;
using Prism.Events;
using Prism.Logging;
using Prism.Mvvm;

namespace NucLedMgr.Arduino.Models;

public class ArduinoPatternManager : BindableBase, IArduinoPatternManager
{
	private readonly IEventAggregator _events;

	private readonly ILoggerFacade _logger;

	private readonly IArduinoClient _arduinoClient;

	private readonly SerialDeviceClient _client;

	private readonly IColorManager _colorManager;

	public static readonly int BRIGHTNESS_MIN = 0;

	public static readonly int BRIGHTNESS_MAX = 5;

	public static readonly int SPEED_MIN = 0;

	public static readonly int SPEED_MAX = 5;

	public static readonly int DEFAULT_SPEED = 3;

	public static readonly Color DEFAULT_COLOR1 = ArduinoColorPickerData.ColorFromFastLedHsv(0);

	public static readonly Color DEFAULT_COLOR2 = ArduinoColorPickerData.ColorFromFastLedHsv(96);

	public static readonly Color DEFAULT_COLOR3 = ArduinoColorPickerData.ColorFromFastLedHsv(160);

	private static readonly string _profilesDirectory = Path.Combine(ApplicationConstants.GetProfileFolderPath(), "LedProfiles\\");

	private const string JSON_FILE_EXTENSION = ".json";

	private List<LedPattern> _ledPatterns;

	private ArduinoLedData _selectedZone;

	private ObservableCollection<ArduinoLedData> _arduinoZones;

	private NucType _nucType;

	private bool _on;

	private LedProfile _currentProfile;

	private string _serialPortError;

	public List<LedPattern> CurrentLedPatterns
	{
		get
		{
			return _ledPatterns;
		}
		set
		{
			SetProperty(ref _ledPatterns, value, "CurrentLedPatterns");
		}
	}

	public ArduinoLedData SelectedZone
	{
		get
		{
			return _selectedZone;
		}
		set
		{
			SetProperty(ref _selectedZone, value, "SelectedZone");
		}
	}

	public ObservableCollection<ArduinoLedData> ArduinoZones
	{
		get
		{
			return _arduinoZones;
		}
		set
		{
			SetProperty(ref _arduinoZones, value, "ArduinoZones");
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

	public bool On
	{
		get
		{
			return _on;
		}
		set
		{
			SetProperty(ref _on, value, "On");
			EnableAll(_on);
		}
	}

	public ObservableCollection<LedProfile> Profiles { get; }

	public LedProfile CurrentProfile
	{
		get
		{
			return _currentProfile;
		}
		set
		{
			if (_currentProfile == value)
			{
				return;
			}
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
			if (value != null && _client != null)
			{
				_client.SetLedPattern(_currentProfile.Leds, SetOnly: false);
				_events.GetEvent<PreviousLedSetEvent>().Publish(_currentProfile.Leds);
			}
			if (_currentProfile != null)
			{
				_events.GetEvent<ArduinoProfileChangedEvent>().Publish(_currentProfile.Name);
			}
		}
	}

	public string SerialPortError
	{
		get
		{
			return _serialPortError;
		}
		set
		{
			SetProperty(ref _serialPortError, value, "SerialPortError");
		}
	}

	public ArduinoPatternManager(IEventAggregator events, IColorManager colorMgr, ILoggerFacade logger, IArduinoClient arduinoClient)
	{
		_events = events;
		_logger = logger;
		_arduinoClient = arduinoClient;
		_client = new SerialDeviceClient(events, logger, arduinoClient);
		_colorManager = colorMgr;
		_events.GetEvent<SerialPortAvailable>().Subscribe(IsLedAvailable);
		_events.GetEvent<CurrentArduinoPatternsEvent>().Subscribe(UpdateFromDevice);
		_events.GetEvent<CurrentArduinoButtonStatusEvent>().Subscribe(DisableFromDevice);
		_events.GetEvent<ArduinoZoneSettingChangeEvent>().Subscribe(UpdateZoneSetting);
		colorMgr.First = DEFAULT_COLOR1;
		colorMgr.Second = DEFAULT_COLOR2;
		colorMgr.Third = DEFAULT_COLOR3;
		Profiles = new ObservableCollection<LedProfile>();
		Profiles.CollectionChanged += (object s, NotifyCollectionChangedEventArgs e) =>
		{
			UpdateTrayApp();
		};
		_nucType = PlatformDetail.GetNucType();
		List<LedPattern> list = GetPreviousPatterns();
		if (list == null)
		{
			list = new List<LedPattern>();
		}
		LedPattern previous = list.Find((LedPattern x) => SerialDeviceClient.ConvertChannelToZone(x.Channel) == MonsterCoveZone.Skull);
		LedPattern previous2 = list.Find((LedPattern x) => SerialDeviceClient.ConvertChannelToZone(x.Channel) == MonsterCoveZone.BottomFront);
		LedPattern previous3 = list.Find((LedPattern x) => SerialDeviceClient.ConvertChannelToZone(x.Channel) == MonsterCoveZone.BottomRight);
		LedPattern previous4 = list.Find((LedPattern x) => SerialDeviceClient.ConvertChannelToZone(x.Channel) == MonsterCoveZone.BottomLeft);
		_arduinoZones = new ObservableCollection<ArduinoLedData>
		{
			new ArduinoLedData(events, colorMgr, MonsterCoveZone.Skull, previous),
			new ArduinoLedData(events, colorMgr, MonsterCoveZone.BottomFront, previous2),
			new ArduinoLedData(events, colorMgr, MonsterCoveZone.BottomRight, previous3),
			new ArduinoLedData(events, colorMgr, MonsterCoveZone.BottomLeft, previous4)
		};
		_selectedZone = _arduinoZones.First();
		Refresh();
		Directory.CreateDirectory(_profilesDirectory);
		if (IsDirectoryEmpty(_profilesDirectory))
		{
			CreateFirstProfile(LedConstants.DEFAULT);
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
		}
		_events.GetEvent<ArduinoProfileChangedEvent>().Publish(_currentProfile.Name);
	}

	private void IsLedAvailable(string error)
	{
		_serialPortError = error;
	}

	private bool IsDirectoryEmpty(string path)
	{
		return !Directory.EnumerateFileSystemEntries(path).Any();
	}

	private void CreateFirstProfile(string name)
	{
		_currentProfile = new LedProfile(name, CurrentLedPatterns);
		SaveProfile(CurrentProfile, setAsCurrentProfile: true);
	}

	public Task SaveProfile(LedProfile profile, bool setAsCurrentProfile)
	{
		return Task.Run(() =>
		{
			try
			{
				NssJsonSerializer.Serialize(profile, Path.Combine(_profilesDirectory, profile.Name + ".json"));
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
				}
			});
			if (setAsCurrentProfile && CurrentProfile != null && !profile.Name.Equals(CurrentProfile.Name))
			{
				CurrentProfile = GetProfileByName(profile.Name);
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

	public LedProfile GetProfileByName(string name)
	{
		if (Profiles == null || Profiles.Count == 0)
		{
			return null;
		}
		return Profiles.FirstOrDefault((LedProfile f) => f.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
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
			LedProfile ledProf = NssJsonSerializer.DeserializeWithBinding<LedProfile>(fileName, new List<Type> { typeof(LedProfile) });
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
					File.Delete(_profilesDirectory + profile.Name + ".json");
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

	private static string GetPreviousProfilePath()
	{
		return Path.Combine(ApplicationConstants.GetProfileFolderPath(), "PreviousLedProfile.json");
	}

	private List<LedPattern> GetPreviousPatterns()
	{
		if (File.Exists(GetPreviousProfilePath()))
		{
			return NssJsonSerializer.Deserialize<List<LedPattern>>(GetPreviousProfilePath(), new List<Type> { typeof(LedPattern) });
		}
		return new List<LedPattern>();
	}

	public void ApplyPrevious()
	{
		bool flag = true;
		foreach (ArduinoLedData arduinoZone in ArduinoZones)
		{
			if (arduinoZone.PreviousPattern.PatternIndex != 0)
			{
				flag = false;
				break;
			}
		}
		List<LedPattern> list = new List<LedPattern>();
		if (flag)
		{
			foreach (ArduinoLedData arduinoZone2 in ArduinoZones)
			{
				LedPattern ledPattern = new LedPattern
				{
					Channel = arduinoZone2.PreviousPattern.Channel,
					PatternIndex = 1
				};
				ledPattern.Palette[0] = 0;
				ledPattern.MaxBrightness = 5;
				ledPattern.RainBow = 1;
				ledPattern.Speed = 5;
				list.Add(ledPattern);
			}
		}
		else
		{
			foreach (ArduinoLedData arduinoZone3 in ArduinoZones)
			{
				list.Add(arduinoZone3.PreviousPattern);
			}
		}
		_client.SetLedPattern(list, SetOnly: false);
	}

	public void EnableAll(bool Enable)
	{
		Task.Run(() =>
		{
			_events.GetEvent<LedAvailable>().Publish(payload: false);
			if (Enable)
			{
				_client.EnableAll();
			}
			else
			{
				_client.DisableAll();
			}
		});
	}

	public void Refresh()
	{
		Task.Run(() =>
		{
			_events.GetEvent<LedAvailable>().Publish(payload: false);
			_client.GetCurrentPattern();
		});
	}

	public void Reset()
	{
		Task.Run(() =>
		{
			_events.GetEvent<LedAvailable>().Publish(payload: false);
			_colorManager.First = DEFAULT_COLOR1;
			_colorManager.Second = DEFAULT_COLOR2;
			_colorManager.Third = DEFAULT_COLOR3;
			_client.Reset();
		});
	}

	private void UpdateZoneSetting(Tuple<LedPattern, bool> Data)
	{
		_client.SetLedPattern(new List<LedPattern> { Data.Item1 }, Data.Item2);
		_events.GetEvent<RefreshArduinoUiEvent>().Publish(On);
	}

	private void UpdateFromDevice(LedStatus status)
	{
		Task.Run(() =>
		{
			if (status != null)
			{
				_on = status.ButtonStatus != BntStatus.Off;
				LedPattern[] ledPatternList = status.LedPatternList;
				foreach (LedPattern ledPattern in ledPatternList)
				{
					MonsterCoveZone index = SerialDeviceClient.ConvertChannelToZone(ledPattern.Channel);
					ArduinoZones.FirstOrDefault((ArduinoLedData x) => x.Index == index)?.UpdateFromArduinoLedPattern(ledPattern);
				}
				_events.GetEvent<RefreshArduinoUiEvent>().Publish(On);
				CurrentLedPatterns = status.LedPatternList.ToList();
				SaveasCurrentProfile();
				if (status.LedPatternList.Count() > 0)
				{
					_events.GetEvent<LedAvailable>().Publish(payload: true);
				}
				else
				{
					_events.GetEvent<LedAvailable>().Publish(payload: false);
				}
			}
		});
	}

	private void SaveasCurrentProfile()
	{
		if (CurrentProfile != null)
		{
			CurrentProfile.Leds = CurrentLedPatterns;
			SaveProfile(CurrentProfile, setAsCurrentProfile: true);
		}
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

	private void DisableFromDevice(BntStatus status)
	{
		_on = status != BntStatus.Off;
		_events.GetEvent<RefreshArduinoUiEvent>().Publish(On);
	}

	~ArduinoPatternManager()
	{
		List<LedPattern> list = new List<LedPattern>();
		foreach (ArduinoLedData arduinoZone in ArduinoZones)
		{
			list.Add(arduinoZone.PreviousPattern);
		}
		NssJsonSerializer.Serialize(list, GetPreviousProfilePath());
		_client.Disconnect();
	}
}
