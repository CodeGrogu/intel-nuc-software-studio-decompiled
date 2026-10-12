using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Infrastructure.Events;
using Infrastructure.Models;
using IntelSdkClient.ENE.Interface;
using IntelSdkClient.EneService;
using NucLedMgr.ENE.Enums;
using NucLedMgr.ENE.Events;
using NucLedMgr.ENE.Interfaces;
using NucLedMgr.Interfaces;
using NucLedMgr.WMI.Models;
using Prism.Events;
using Prism.Logging;
using Prism.Mvvm;

namespace NucLedMgr.ENE.Models;

public class EnePatternManager : BindableBase, IEnePatternManager
{
	private readonly IEventAggregator _events;

	private readonly ILoggerFacade _logger;

	private readonly IColorManager _colorManager;

	private readonly IEneClient _eneClient;

	private static readonly string _profilesDirectory = Path.Combine(ApplicationConstants.GetProfileFolderPath(), "LedProfiles\\");

	private const string JSON_FILE_EXTENSION = ".json";

	public static readonly int BRIGHTNESS_MIN = 0;

	public static readonly int BRIGHTNESS_MAX = 5;

	public static readonly int SPEED_MIN = 0;

	public static readonly int SPEED_MAX = 5;

	public static readonly int DEFAULT_SPEED = 3;

	public static readonly Color DEFAULT_COLOR1 = EneColorPickerData.ColorFromFastLedHsv(0);

	public static readonly Color DEFAULT_COLOR2 = EneColorPickerData.ColorFromFastLedHsv(96);

	public static readonly Color DEFAULT_COLOR3 = EneColorPickerData.ColorFromFastLedHsv(160);

	private bool _on;

	private string _serialPortError;

	private EneLedData _selectedZone;

	private ObservableCollection<EneLedData> _eneZones;

	private List<LedPattern> _ledPatterns;

	private LedProfile _currentProfile;

	private List<LedPattern> _previousLedPattern;

	public bool On
	{
		get
		{
			return _on;
		}
		set
		{
			SetProperty(ref _on, value, "On");
			SelectedZone.ButtonStatus = _on;
			EnableAll(_on);
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

	public ObservableCollection<LedProfile> Profiles { get; }

	public EneLedData SelectedZone
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

	public ObservableCollection<EneLedData> EneZones
	{
		get
		{
			return _eneZones;
		}
		set
		{
			SetProperty(ref _eneZones, value, "EneZones");
		}
	}

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
			if (value != null && _eneClient != null)
			{
				Task.Run(() =>
				{
					_events.GetEvent<LedAvailable>().Publish(payload: false);
					_eneClient.SetLedPattern(_currentProfile.Leds);
					GetCurrentPattern();
					_events.GetEvent<PreviousEneLedSetEvent>().Publish(_currentProfile.Leds);
				});
			}
			_events.GetEvent<ArduinoProfileChangedEvent>().Publish(_currentProfile?.Name);
		}
	}

	public List<LedPattern> PreviousLedPattern
	{
		get
		{
			return _previousLedPattern;
		}
		set
		{
			SetProperty(ref _previousLedPattern, value, "PreviousLedPattern");
		}
	}

	public Dictionary<EneLedZone, string> zoneNames { get; set; }

	public EnePatternManager(IEventAggregator events, IColorManager colorMgr, ILoggerFacade logger, IEneClient eneClient)
	{
		_events = events;
		_colorManager = colorMgr;
		_logger = logger;
		_eneClient = eneClient;
		_on = true;
		_colorManager.First = DEFAULT_COLOR1;
		_colorManager.Second = DEFAULT_COLOR2;
		_colorManager.Third = DEFAULT_COLOR3;
		Profiles = new ObservableCollection<LedProfile>();
		Profiles.CollectionChanged += (object s, NotifyCollectionChangedEventArgs e) =>
		{
			UpdateTrayApp();
		};
		_serialPortError = eneClient.CheckSeriportConnection();
		if (!string.IsNullOrWhiteSpace(_serialPortError))
		{
			return;
		}
		int[] ledCount = GetLedCount();
		_eneZones = new ObservableCollection<EneLedData>
		{
			new EneLedData(events, colorMgr, EneLedZone.LogoHeader, ledCount[0]),
			new EneLedData(events, colorMgr, EneLedZone.LeftHeader, ledCount[1]),
			new EneLedData(events, colorMgr, EneLedZone.RightHeader, ledCount[2]),
			new EneLedData(events, colorMgr, EneLedZone.FrontHeader, ledCount[3])
		};
		_selectedZone = _eneZones.First();
		GetCurrentPattern();
		zoneNames = new Dictionary<EneLedZone, string>();
		_events.GetEvent<PreviousEneLedSetEvent>().Subscribe(SetPreviousPattern);
		_events.GetEvent<EneZoneSettingChangeEvent>().Subscribe(UpdateZoneSetting);
		Directory.CreateDirectory(_profilesDirectory);
		if (IsDirectoryEmpty(_profilesDirectory))
		{
			AddZoneNames();
			CreateFirstProfile(LedConstants.DEFAULT);
			return;
		}
		GetSavedProfiles();
		UpdateZoneName();
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

	private int[] GetLedCount()
	{
		return ((IEnumerable<byte>)_eneClient.LedConnectionStatus()).Select((Func<byte, int>)((byte x) => x)).ToArray();
	}

	private void UpdateZoneName()
	{
		foreach (EneLedData eneZone in _eneZones)
		{
			if (_currentProfile.ZoneNames.ContainsKey(eneZone.Index))
			{
				eneZone.Name = _currentProfile.ZoneNames[eneZone.Index];
			}
			else
			{
				SetdefalutZoneName();
			}
		}
	}

	private void SetdefalutZoneName()
	{
		_currentProfile.ZoneNames.Clear();
		foreach (EneLedData eneZone in _eneZones)
		{
			_currentProfile.ZoneNames.Add(eneZone.Index, eneZone.InitialName);
			eneZone.Name = eneZone.InitialName;
		}
		SaveProfile(_currentProfile, setAsCurrentProfile: true);
	}

	private void AddZoneNames()
	{
		zoneNames.Clear();
		foreach (EneLedData eneZone in _eneZones)
		{
			zoneNames.Add(eneZone.Index, eneZone.Name);
		}
	}

	private bool IsDirectoryEmpty(string path)
	{
		return !Directory.EnumerateFileSystemEntries(path).Any();
	}

	private void UpdateZoneSetting(Tuple<LedPattern, bool> Data)
	{
		try
		{
			Task.Run(() =>
			{
				_events.GetEvent<LedAvailable>().Publish(payload: false);
				_ = Data.Item2;
				_eneClient.SetLedPattern(new List<LedPattern> { Data.Item1 });
				Thread.Sleep(1000);
				GetCurrentPattern();
			});
			_events.GetEvent<RefreshEneUiEvent>().Publish(On);
		}
		catch (Exception exception)
		{
			_events.GetEvent<LedAvailable>().Publish(payload: true);
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
		}
	}

	private void SetPreviousPattern(List<LedPattern> pattern)
	{
		_previousLedPattern = pattern;
	}

	public void Reset()
	{
		try
		{
			Task.Run(() =>
			{
				_events.GetEvent<LedAvailable>().Publish(payload: false);
				_colorManager.First = DEFAULT_COLOR1;
				_colorManager.Second = DEFAULT_COLOR2;
				_colorManager.Third = DEFAULT_COLOR3;
				_eneClient.EneCommands("RESET_ALL_PATTERN");
				GetCurrentPattern();
			});
		}
		catch (Exception exception)
		{
			_events.GetEvent<LedAvailable>().Publish(payload: true);
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
		}
	}

	public void GetCurrentPattern()
	{
		try
		{
			if (_eneClient.GetConnectionStatus())
			{
				_eneClient.EneCommands("COMMAND_DELAY");
				LedStatus currentPatterns = _eneClient.GetCurrentPatterns();
				UpdateFromDevice(currentPatterns);
			}
		}
		catch (Exception exception)
		{
			_events.GetEvent<LedAvailable>().Publish(payload: true);
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
		}
	}

	public void EnableAll(bool Enable)
	{
		try
		{
			Task.Run(() =>
			{
				_events.GetEvent<LedAvailable>().Publish(payload: false);
				if (Enable)
				{
					_eneClient.EneCommands("ENABLE_ALL");
				}
				else
				{
					_eneClient.EneCommands("DISABLE_ALL");
				}
				_eneClient.EneCommands("COMMAND_DELAY");
				if (PreviousLedPattern != null)
				{
					_eneClient.SetLedPattern(PreviousLedPattern);
				}
				Thread.Sleep(1000);
				GetCurrentPattern();
			});
		}
		catch (Exception exception)
		{
			_events.GetEvent<LedAvailable>().Publish(payload: true);
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
		}
	}

	private void CreateFirstProfile(string name)
	{
		_currentProfile = new LedProfile(name, CurrentLedPatterns, zoneNames);
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
				_events.GetEvent<LedAvailable>().Publish(payload: true);
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
			_events.GetEvent<LedAvailable>().Publish(payload: true);
		}
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
			_events.GetEvent<LedAvailable>().Publish(payload: true);
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

	public LedProfile GetProfileByName(string name)
	{
		if (Profiles == null || Profiles.Count == 0)
		{
			return null;
		}
		return Profiles.FirstOrDefault((LedProfile f) => f.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
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
					EneLedZone index = EneLedHelper.ConvertChannelToZone(ledPattern.Channel);
					EneLedData eneLedData = EneZones.FirstOrDefault((EneLedData x) => x.Index == index);
					eneLedData.ButtonStatus = status.ButtonStatus != BntStatus.Off;
					eneLedData?.UpdateFromEneLedPattern(ledPattern);
				}
				_events.GetEvent<RefreshEneUiEvent>().Publish(On);
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

	private void UpdateTrayApp()
	{
		Dictionary<string, EventHandler> dictionary = new Dictionary<string, EventHandler>();
		foreach (LedProfile profile in Profiles)
		{
			if (!dictionary.ContainsKey(profile.Name))
			{
				dictionary.Add(profile.Name, (object s, EventArgs e) =>
				{
					CurrentProfile = profile;
				});
			}
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
}
