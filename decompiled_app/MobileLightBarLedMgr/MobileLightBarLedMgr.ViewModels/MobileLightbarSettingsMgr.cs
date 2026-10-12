using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using CustomizationMgr.Enums;
using CustomizationMgr.Interfaces;
using CustomizationMgr.ViewModels;
using Infrastructure.Converters;
using Infrastructure.Models;
using MobileLightBarLedMgr.Constants;
using MobileLightBarLedMgr.Interfaces;
using MobileLightBarLedMgr.Models;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using Prism.Logging;
using Prism.Regions;
using UniwillSdkClient.Events;
using UniwillSdkClient.Interfaces;
using UniwillSdkClient.UniwillService;

namespace MobileLightBarLedMgr.ViewModels;

public class MobileLightbarSettingsMgr : NavBaseSummaryViewModel, IMobileLightbarSettingsMgr
{
	private static readonly string CARD_TITLE_STRING = LightBarConstants.LIGHTBAR_CARD_TITLE_STRING;

	private const string DETAIL_CARD_NAME = "LightbarDetailView";

	private const IconType CARD_ICON_TYPE = IconType.Performance;

	private static readonly string SECOND_CONST = ResourceDictionaryConverter.GetString("SecondString").Substring(1);

	private static readonly string SECONDS_CONST = ResourceDictionaryConverter.GetString("SecondsString").Substring(1);

	private static readonly string MINUTES_CONST = ResourceDictionaryConverter.GetString("MinutesString").Substring(1);

	private static readonly string MINUTE_CONST = ResourceDictionaryConverter.GetString("MinuteString").Substring(1);

	private static readonly string HOURS_CONST = ResourceDictionaryConverter.GetString("HoursString").Substring(1);

	private static readonly string HOUR_CONST = ResourceDictionaryConverter.GetString("HourString").Substring(1);

	private static readonly string OFF_CONST = ResourceDictionaryConverter.GetString("OffString");

	private const int AC_UI_TAB_INDEX = 0;

	private const int DC_UI_TAB_INDEX = 1;

	private const int OFF_INT = 0;

	private static readonly int IDLE_MAX_INT = (int)TimeSpan.FromHours(5.0).TotalSeconds;

	private static readonly int AC_DEFAULT = 0;

	private static readonly int DC_DEFAULT = (int)TimeSpan.FromSeconds(30.0).TotalSeconds;

	private static readonly int ONE_SECOND = (int)TimeSpan.FromSeconds(1.0).TotalSeconds;

	private static readonly int ONE_MINUTE = (int)TimeSpan.FromMinutes(1.0).TotalSeconds;

	private static readonly int TWO_MINUTES = (int)TimeSpan.FromMinutes(2.0).TotalSeconds;

	private static readonly int ONE_HOUR = (int)TimeSpan.FromHours(1.0).TotalSeconds;

	private static readonly int TWO_HOURS = (int)TimeSpan.FromHours(2.0).TotalSeconds;

	private readonly IEventAggregator _events;

	private readonly ILoggerFacade _logger;

	private IUniwillClient _clientgenerator;

	private UniwillServiceClient _client;

	private static readonly ObservableCollection<Color> STARTER_COLORS = new ObservableCollection<Color>
	{
		Colors.Red,
		Colors.Orange,
		Colors.Yellow,
		Colors.Green,
		Colors.Blue,
		Colors.Indigo,
		Colors.Violet
	};

	private ObservableCollection<string> _initialIdleTimerOptions = new ObservableCollection<string>
	{
		OFF_CONST,
		5 + SECONDS_CONST,
		10 + SECONDS_CONST,
		15 + SECONDS_CONST,
		30 + SECONDS_CONST,
		45 + SECONDS_CONST,
		1 + MINUTE_CONST,
		2 + MINUTES_CONST,
		5 + MINUTES_CONST,
		10 + MINUTES_CONST,
		30 + MINUTES_CONST,
		1 + HOUR_CONST,
		2 + HOURS_CONST,
		3 + HOURS_CONST
	};

	private Tuple<int, int> _defaultIdleTimerValues;

	private LightbarProfile _profile;

	private LightbarProfile _previousProfile;

	private LightbarProfile _defaultProfile;

	private bool _errorMessage;

	private ObservableCollection<Color> _recentColors = STARTER_COLORS;

	private bool _lightbarEnabled = true;

	private bool _acEnabled = true;

	private bool _acPreviouslyEnabled = true;

	private int _acRed;

	private int _acGreen;

	private int _acBlue;

	private string _currentAcTimerValue;

	private Color _acOldColor;

	private Color _acNewColor;

	private Color _acColor;

	private bool _acRainbow;

	private bool _breathingEnabled;

	private bool _dcEnabled = true;

	private bool _dcPreviouslyEnabled = true;

	private int _dcRed;

	private int _dcGreen;

	private int _dcBlue;

	private Color _dcOldColor;

	private Color _dcNewColor;

	private Color _dcColor;

	private bool _dcRainbow;

	private string _currentDcTimerValue;

	private DelegateCommand<Tuple<Color, int>> _selectColor;

	private DelegateCommand _resetAc;

	private DelegateCommand _resetDc;

	private DelegateCommand _sendToSdk;

	public LightbarProfile Profile
	{
		get
		{
			return _profile;
		}
		set
		{
			SetProperty(ref _profile, value, "Profile");
		}
	}

	public bool ErrorMessage
	{
		get
		{
			return _errorMessage;
		}
		set
		{
			SetProperty(ref _errorMessage, value, "ErrorMessage");
		}
	}

	public int RgbMin => 0;

	public int RgbMax => 100;

	public ObservableCollection<Color> RecentColors
	{
		get
		{
			return _recentColors;
		}
		set
		{
			SetProperty(ref _recentColors, value, "RecentColors");
		}
	}

	public bool LightbarEnabled
	{
		get
		{
			return _lightbarEnabled;
		}
		set
		{
			SetProperty(ref _lightbarEnabled, value, "LightbarEnabled");
			if (value)
			{
				Profile = CopyToProfile(_previousProfile);
				GetProfileColors();
				GetProfileEffects();
				_acEnabled = _acPreviouslyEnabled;
				if (!_acEnabled)
				{
					DisableAc();
				}
				_dcEnabled = _dcPreviouslyEnabled;
				if (!_dcEnabled)
				{
					DisableDc();
				}
				SendProfileToSDK();
			}
			else
			{
				_acPreviouslyEnabled = _acEnabled;
				_dcPreviouslyEnabled = _dcEnabled;
				_previousProfile = CopyToProfile(Profile);
				DisableLightbar();
				_acColor = _acNewColor;
				_dcColor = _dcNewColor;
			}
			RefreshUI();
		}
	}

	public bool AcEnabled
	{
		get
		{
			return _acEnabled;
		}
		set
		{
			if (_acEnabled == value)
			{
				return;
			}
			SetProperty(ref _acEnabled, value, "AcEnabled");
			if (value)
			{
				_lightbarEnabled = true;
				RestorePreviousSettings(Profile.AcSetting);
				return;
			}
			if (Profile.AcSetting.R_Brightness != 0 || Profile.AcSetting.G_Brightness != 0 || Profile.AcSetting.B_Brightness != 0)
			{
				_previousProfile.AcSetting = CopySetting(Profile.AcSetting);
			}
			else
			{
				_previousProfile.AcSetting = new LightbarSetting
				{
					R_Brightness = ReturnInt(_acOldColor.R),
					G_Brightness = ReturnInt(_acOldColor.G),
					B_Brightness = ReturnInt(_acOldColor.B),
					Effect = Profile.AcSetting.Effect
				};
			}
			_previousProfile.BreathingEnable = _breathingEnabled;
			SetToBlack(Profile.AcSetting);
			if (!_acEnabled && !_dcEnabled)
			{
				_lightbarEnabled = false;
			}
			SendProfileToSDK();
			RefreshUI();
		}
	}

	public int AcRed
	{
		get
		{
			return _acRed;
		}
		set
		{
			SetProperty(ref _acRed, value, "AcRed");
			SetProfileColors();
		}
	}

	public int AcGreen
	{
		get
		{
			return _acGreen;
		}
		set
		{
			SetProperty(ref _acGreen, value, "AcGreen");
			SetProfileColors();
		}
	}

	public int AcBlue
	{
		get
		{
			return _acBlue;
		}
		set
		{
			SetProperty(ref _acBlue, value, "AcBlue");
			SetProfileColors();
		}
	}

	public string CurrentAcTimerValue
	{
		get
		{
			return _currentAcTimerValue;
		}
		set
		{
			if (!(_currentAcTimerValue == value))
			{
				SetProperty(ref _currentAcTimerValue, value, "CurrentAcTimerValue");
				SendIdleTimerToSdk(value, isAcTimer: true);
			}
		}
	}

	public ObservableCollection<string> AcTimerValues => _initialIdleTimerOptions;

	public Color AcNewColor
	{
		get
		{
			return _acNewColor;
		}
		set
		{
			SetProperty(ref _acNewColor, value, "AcNewColor");
		}
	}

	public Color AcColor
	{
		get
		{
			return _acColor;
		}
		set
		{
			_acOldColor = _acColor;
			SetProperty(ref _acColor, value, "AcColor");
			UpdateRecentColors(value);
		}
	}

	public bool AcRainbow
	{
		get
		{
			return _acRainbow;
		}
		set
		{
			SetProperty(ref _acRainbow, value, "AcRainbow");
			if (value)
			{
				Profile.AcSetting.Effect = LightBarEffect.Rainbow;
			}
			else
			{
				Profile.AcSetting.Effect = LightBarEffect.Monocolor;
			}
			SendProfileToSDK();
		}
	}

	public bool BreathingEnabled
	{
		get
		{
			return _breathingEnabled;
		}
		set
		{
			SetProperty(ref _breathingEnabled, value, "BreathingEnabled");
			Profile.BreathingEnable = value;
			SendProfileToSDK();
		}
	}

	public bool DcEnabled
	{
		get
		{
			return _dcEnabled;
		}
		set
		{
			if (_dcEnabled == value)
			{
				return;
			}
			SetProperty(ref _dcEnabled, value, "DcEnabled");
			if (value)
			{
				_lightbarEnabled = true;
				RestorePreviousSettings(Profile.DcSetting);
				return;
			}
			if (Profile.DcSetting.R_Brightness != 0 || Profile.DcSetting.G_Brightness != 0 || Profile.DcSetting.B_Brightness != 0)
			{
				_previousProfile.DcSetting = CopySetting(Profile.DcSetting);
			}
			else
			{
				_previousProfile.DcSetting = new LightbarSetting
				{
					R_Brightness = ReturnInt(_dcOldColor.R),
					G_Brightness = ReturnInt(_dcOldColor.G),
					B_Brightness = ReturnInt(_dcOldColor.B),
					Effect = Profile.DcSetting.Effect
				};
			}
			SetToBlack(Profile.DcSetting);
			if (!_acEnabled && !_dcEnabled)
			{
				_lightbarEnabled = false;
			}
			SendProfileToSDK();
			RefreshUI();
		}
	}

	public int DcRed
	{
		get
		{
			return _dcRed;
		}
		set
		{
			SetProperty(ref _dcRed, value, "DcRed");
			SetProfileColors();
		}
	}

	public int DcGreen
	{
		get
		{
			return _dcGreen;
		}
		set
		{
			SetProperty(ref _dcGreen, value, "DcGreen");
			SetProfileColors();
		}
	}

	public int DcBlue
	{
		get
		{
			return _dcBlue;
		}
		set
		{
			SetProperty(ref _dcBlue, value, "DcBlue");
			SetProfileColors();
		}
	}

	public Color DcNewColor
	{
		get
		{
			return _dcNewColor;
		}
		set
		{
			SetProperty(ref _dcNewColor, value, "DcNewColor");
		}
	}

	public Color DcColor
	{
		get
		{
			return _dcColor;
		}
		set
		{
			_dcOldColor = _dcColor;
			SetProperty(ref _dcColor, value, "DcColor");
			UpdateRecentColors(value);
		}
	}

	public bool DcRainbow
	{
		get
		{
			return _dcRainbow;
		}
		set
		{
			SetProperty(ref _dcRainbow, value, "DcRainbow");
			if (value)
			{
				Profile.DcSetting.Effect = LightBarEffect.Rainbow;
			}
			else
			{
				Profile.DcSetting.Effect = LightBarEffect.Monocolor;
			}
			SendProfileToSDK();
		}
	}

	public string CurrentDcTimerValue
	{
		get
		{
			return _currentDcTimerValue;
		}
		set
		{
			if (!(_currentDcTimerValue == value))
			{
				SetProperty(ref _currentDcTimerValue, value, "CurrentDcTimerValue");
				SendIdleTimerToSdk(value, isAcTimer: false);
			}
		}
	}

	public ObservableCollection<string> DcTimerValues => _initialIdleTimerOptions;

	public DelegateCommand<Tuple<Color, int>> SelectColor
	{
		get
		{
			return _selectColor;
		}
		set
		{
			SetProperty(ref _selectColor, value, "SelectColor");
		}
	}

	public DelegateCommand ResetAc
	{
		get
		{
			return _resetAc;
		}
		set
		{
			SetProperty(ref _resetAc, value, "ResetAc");
		}
	}

	public DelegateCommand ResetDc
	{
		get
		{
			return _resetDc;
		}
		set
		{
			SetProperty(ref _resetDc, value, "ResetDc");
		}
	}

	public DelegateCommand SendToSdk
	{
		get
		{
			return _sendToSdk;
		}
		set
		{
			SetProperty(ref _sendToSdk, value, "SendToSdk");
		}
	}

	public MobileLightbarSettingsMgr(ICustomResourceManager customResourceManager, IEventAggregator events, IRegionManager regionMgr, ILoggerFacade logger, ICustomResourceManager resourceManager, IUniwillClient client)
		: base(regionMgr, resourceManager, CARD_TITLE_STRING, "LightbarDetailView", IconType.Performance)
	{
		MobileLightbarSettingsMgr mobileLightbarSettingsMgr = this;
		ErrorMessage = false;
		_events = events;
		_logger = logger;
		Task.Run(() =>
		{
			try
			{
				mobileLightbarSettingsMgr._clientgenerator = client;
				mobileLightbarSettingsMgr._client = client.Client;
				if (mobileLightbarSettingsMgr._client == null)
				{
					throw new ArgumentNullException("Unable to connect to SDK");
				}
				if (customResourceManager.CustomAppSettings?.Settings?.Find((object x) => x.GetType().Equals(typeof(MobileLightBarCustomSettings))) is MobileLightBarCustomSettings customSettings)
				{
					mobileLightbarSettingsMgr.LoadCustomDefaults(customSettings);
				}
				else
				{
					mobileLightbarSettingsMgr.LoadClientDefaults();
				}
				if (customResourceManager.IsFirstLaunch())
				{
					mobileLightbarSettingsMgr.Profile = mobileLightbarSettingsMgr.CopyToProfile(mobileLightbarSettingsMgr._defaultProfile);
					mobileLightbarSettingsMgr.SendProfileToSDK();
					mobileLightbarSettingsMgr.UpdateIdleTimer(ValueTuple.Create(mobileLightbarSettingsMgr._defaultIdleTimerValues.Item1, mobileLightbarSettingsMgr._defaultIdleTimerValues.Item2));
					mobileLightbarSettingsMgr.SendIdleTimerToSdk(mobileLightbarSettingsMgr._currentAcTimerValue, isAcTimer: true);
					mobileLightbarSettingsMgr.SendIdleTimerToSdk(mobileLightbarSettingsMgr._currentDcTimerValue, isAcTimer: false);
					mobileLightbarSettingsMgr.PopulateProfile();
				}
				else
				{
					mobileLightbarSettingsMgr.Profile = mobileLightbarSettingsMgr._client.GetCurrentLightbarProfile();
					Tuple<int, int> lightBarIdleTimerValue = mobileLightbarSettingsMgr._client.GetLightBarIdleTimerValue();
					mobileLightbarSettingsMgr.UpdateIdleTimer(ValueTuple.Create(lightBarIdleTimerValue.Item1, lightBarIdleTimerValue.Item2));
					mobileLightbarSettingsMgr.PopulateProfile();
					mobileLightbarSettingsMgr.DetermineEnabledStatus();
				}
				mobileLightbarSettingsMgr._selectColor = new DelegateCommand<Tuple<Color, int>>(mobileLightbarSettingsMgr.ChooseNewColor);
				mobileLightbarSettingsMgr._resetAc = new DelegateCommand(mobileLightbarSettingsMgr.RestoreAcDefaults);
				mobileLightbarSettingsMgr._resetDc = new DelegateCommand(mobileLightbarSettingsMgr.RestoreDcDefaults);
				mobileLightbarSettingsMgr._sendToSdk = new DelegateCommand(mobileLightbarSettingsMgr.SendProfileToSDK);
				mobileLightbarSettingsMgr._events.GetEvent<LightbarProfileNotificationEvent>().Subscribe(mobileLightbarSettingsMgr.EnsureProfileObjecstMatch);
				mobileLightbarSettingsMgr._events.GetEvent<LightbarIdleTimerEvent>().Subscribe(mobileLightbarSettingsMgr.UpdateIdleTimer);
			}
			catch (Exception exception)
			{
				mobileLightbarSettingsMgr.ErrorMessage = true;
				mobileLightbarSettingsMgr.OpenDetailView = null;
				mobileLightbarSettingsMgr._logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
			}
		}).ConfigureAwait(continueOnCapturedContext: false);
	}

	private void LoadCustomDefaults(MobileLightBarCustomSettings customSettings)
	{
		_defaultProfile = JsonConvert.DeserializeObject<LightbarProfile>(JsonConvert.SerializeObject(customSettings.LightBarProfile));
		int[]? array = JsonConvert.DeserializeObject<int[]>(JsonConvert.SerializeObject(customSettings.IdleTimerValuesInSeconds));
		_initialIdleTimerOptions = new ObservableCollection<string>();
		int[] array2 = array;
		foreach (int num in array2)
		{
			if (num >= 0 && num < IDLE_MAX_INT)
			{
				_initialIdleTimerOptions.Add(SecondsToTimerString(num));
			}
		}
		_defaultIdleTimerValues = JsonConvert.DeserializeObject<Tuple<int, int>>(JsonConvert.SerializeObject(customSettings.AcDcIdleTimerValues));
	}

	private void LoadClientDefaults()
	{
		_defaultProfile = CreateDefaultProfile();
		_defaultIdleTimerValues = new Tuple<int, int>(AC_DEFAULT, DC_DEFAULT);
	}

	private void ChooseNewColor(Tuple<Color, int> commandValues)
	{
		if (commandValues != null)
		{
			if (commandValues.Item2 == 0)
			{
				_acOldColor = AcColor;
				AcColor = commandValues.Item1;
				_acNewColor = commandValues.Item1;
				_acRed = ReturnInt(_acColor.R);
				_acGreen = ReturnInt(_acColor.G);
				_acBlue = ReturnInt(_acColor.B);
			}
			if (commandValues.Item2 == 1)
			{
				_dcOldColor = DcColor;
				DcColor = commandValues.Item1;
				_dcNewColor = commandValues.Item1;
				_dcRed = ReturnInt(_dcColor.R);
				_dcGreen = ReturnInt(_dcColor.G);
				_dcBlue = ReturnInt(_dcColor.B);
			}
			SetProfileColors();
			_client.SetLightbarProfile(Profile);
			RefreshUI();
		}
	}

	private void UpdateIdleTimer((int Ac, int Dc) IdleTimer)
	{
		if (!_initialIdleTimerOptions.Contains(SecondsToTimerString(IdleTimer.Ac)))
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				_initialIdleTimerOptions.Add(SecondsToTimerString(IdleTimer.Ac));
			});
		}
		if (!_initialIdleTimerOptions.Contains(SecondsToTimerString(IdleTimer.Dc)))
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				_initialIdleTimerOptions.Add(SecondsToTimerString(IdleTimer.Dc));
			});
		}
		_initialIdleTimerOptions = new ObservableCollection<string>(_initialIdleTimerOptions.OrderBy((string i) => TimerStringToSeconds(i)));
		_currentAcTimerValue = AcTimerValues.FirstOrDefault((string x) => x.Contains(SecondsToTimerString(IdleTimer.Ac)));
		_currentDcTimerValue = DcTimerValues.FirstOrDefault((string x) => x.Contains(SecondsToTimerString(IdleTimer.Dc)));
	}

	private string SecondsToTimerString(int seconds)
	{
		if (seconds <= 0)
		{
			return OFF_CONST;
		}
		if (seconds == ONE_SECOND)
		{
			return ONE_SECOND + SECOND_CONST;
		}
		if (seconds < ONE_MINUTE)
		{
			return seconds + SECONDS_CONST;
		}
		if (seconds >= ONE_MINUTE && seconds < TWO_MINUTES)
		{
			return seconds / ONE_MINUTE + MINUTE_CONST;
		}
		if (seconds >= ONE_MINUTE && seconds < ONE_HOUR)
		{
			return seconds / ONE_MINUTE + MINUTES_CONST;
		}
		if (seconds >= ONE_HOUR && seconds < TWO_HOURS)
		{
			return seconds / ONE_HOUR + HOUR_CONST;
		}
		return seconds / ONE_HOUR + HOURS_CONST;
	}

	private int TimerStringToSeconds(string timerString)
	{
		if (timerString.Equals(OFF_CONST))
		{
			return 0;
		}
		if (timerString.Contains(SECONDS_CONST))
		{
			return GetInt(timerString);
		}
		if (timerString.Contains(SECOND_CONST))
		{
			return ONE_SECOND;
		}
		if (timerString.Contains(MINUTES_CONST))
		{
			return GetInt(timerString) * ONE_MINUTE;
		}
		if (timerString.Contains(MINUTE_CONST))
		{
			return ONE_MINUTE;
		}
		if (timerString.Contains(HOURS_CONST))
		{
			return GetInt(timerString) * ONE_HOUR;
		}
		if (timerString.Contains(HOUR_CONST))
		{
			return ONE_HOUR;
		}
		return -1;
	}

	private int GetInt(string str)
	{
		string text = "";
		for (int i = 0; i < str.Length; i++)
		{
			if (char.IsDigit(str[i]))
			{
				text += str[i];
				continue;
			}
			return int.Parse(text);
		}
		return -1;
	}

	private void UpdateRecentColors(Color newColor)
	{
		if (newColor == Colors.Black || newColor == Colors.Transparent || RecentColors.Contains(newColor))
		{
			return;
		}
		Application.Current.Dispatcher.Invoke(() =>
		{
			if (RecentColors.Count >= 7)
			{
				RecentColors.Remove(RecentColors.First());
			}
			RecentColors.Add(newColor);
		});
	}

	private void EnsureProfileObjecstMatch(LightbarProfile sdkProfile)
	{
		if (!(JsonConvert.SerializeObject(sdkProfile) == JsonConvert.SerializeObject(Profile)))
		{
			RepopulateProfile(sdkProfile);
		}
	}

	private void RepopulateProfile(LightbarProfile profile)
	{
		_previousProfile = CopyToProfile(_profile);
		Profile = CopyToProfile(profile);
		GetProfileColors();
		if (LightbarEnabled)
		{
			_acPreviouslyEnabled = true;
			_dcPreviouslyEnabled = true;
		}
		else
		{
			_acPreviouslyEnabled = _acEnabled;
			_dcPreviouslyEnabled = _dcEnabled;
		}
		GetProfileEffects();
		RefreshUI();
	}

	private void PopulateProfile()
	{
		SetPreviousProfile();
		_acPreviouslyEnabled = true;
		_dcPreviouslyEnabled = true;
		GetProfileColors();
		GetProfileEffects();
		RefreshUI();
	}

	private void SetPreviousProfile()
	{
		_previousProfile = CopyToProfile(_defaultProfile);
		if (_previousProfile.AcSetting.R_Brightness == 0 && _previousProfile.AcSetting.G_Brightness == 0 && _previousProfile.AcSetting.B_Brightness == 0)
		{
			_previousProfile.AcSetting.R_Brightness = 100;
			_previousProfile.AcSetting.G_Brightness = 100;
			_previousProfile.AcSetting.B_Brightness = 100;
		}
		if (_previousProfile.DcSetting.R_Brightness == 0 && _previousProfile.DcSetting.G_Brightness == 0 && _previousProfile.DcSetting.B_Brightness == 0)
		{
			_previousProfile.DcSetting.R_Brightness = 100;
			_previousProfile.DcSetting.G_Brightness = 100;
			_previousProfile.DcSetting.B_Brightness = 100;
		}
	}

	private void RestorePreviousSettings(LightbarSetting setting)
	{
		if (setting == Profile.AcSetting)
		{
			Profile.AcSetting = CopySetting(_previousProfile.AcSetting);
			Profile.BreathingEnable = _previousProfile.BreathingEnable;
		}
		else
		{
			if (setting != Profile.DcSetting)
			{
				return;
			}
			Profile.DcSetting = CopySetting(_previousProfile.DcSetting);
		}
		GetProfileColors();
		GetProfileEffects();
		SendProfileToSDK();
		RefreshUI();
	}

	private void RestoreAcDefaults()
	{
		RepopulateProfile(new LightbarProfile
		{
			AcSetting = new LightbarSetting
			{
				R_Brightness = _defaultProfile.AcSetting.R_Brightness,
				G_Brightness = _defaultProfile.AcSetting.G_Brightness,
				B_Brightness = _defaultProfile.AcSetting.B_Brightness,
				Effect = _defaultProfile.AcSetting.Effect
			},
			DcSetting = new LightbarSetting
			{
				R_Brightness = _dcRed,
				G_Brightness = _dcGreen,
				B_Brightness = _dcBlue,
				Effect = (_dcRainbow ? LightBarEffect.Rainbow : LightBarEffect.Monocolor)
			},
			BreathingEnable = _defaultProfile.BreathingEnable
		});
		UpdateIdleTimer(ValueTuple.Create(_defaultIdleTimerValues.Item1, TimerStringToSeconds(_currentDcTimerValue)));
		SendIdleTimerToSdk(_currentAcTimerValue, isAcTimer: true);
		SendProfileToSDK();
	}

	private void RestoreDcDefaults()
	{
		RepopulateProfile(new LightbarProfile
		{
			AcSetting = new LightbarSetting
			{
				R_Brightness = _acRed,
				G_Brightness = _acGreen,
				B_Brightness = _acBlue,
				Effect = (_acRainbow ? LightBarEffect.Rainbow : LightBarEffect.Monocolor)
			},
			DcSetting = new LightbarSetting
			{
				R_Brightness = _defaultProfile.DcSetting.R_Brightness,
				G_Brightness = _defaultProfile.DcSetting.G_Brightness,
				B_Brightness = _defaultProfile.DcSetting.B_Brightness,
				Effect = _defaultProfile.DcSetting.Effect
			},
			BreathingEnable = _breathingEnabled
		});
		UpdateIdleTimer(ValueTuple.Create(TimerStringToSeconds(_currentAcTimerValue), _defaultIdleTimerValues.Item2));
		SendIdleTimerToSdk(_currentDcTimerValue, isAcTimer: false);
		SendProfileToSDK();
	}

	private void SetToBlack(LightbarSetting setting)
	{
		if (setting == Profile.AcSetting)
		{
			DisableAc();
		}
		else
		{
			if (setting != Profile.DcSetting)
			{
				return;
			}
			DisableDc();
		}
		SetProfileColors();
	}

	private bool isEnabled(Color color)
	{
		if (color == Colors.Black)
		{
			return false;
		}
		return true;
	}

	private void DetermineEnabledStatus()
	{
		_acEnabled = isEnabled(AcColor);
		_dcEnabled = isEnabled(DcColor);
		_lightbarEnabled = _acEnabled || _dcEnabled;
	}

	private void DisableAc()
	{
		Profile.AcSetting.R_Brightness = 0;
		_acRed = 0;
		Profile.AcSetting.G_Brightness = 0;
		_acGreen = 0;
		Profile.AcSetting.B_Brightness = 0;
		_acBlue = 0;
		Profile.AcSetting.Effect = LightBarEffect.Monocolor;
		_acRainbow = false;
		Profile.BreathingEnable = false;
		_breathingEnabled = false;
	}

	private void DisableDc()
	{
		Profile.DcSetting.R_Brightness = 0;
		_dcRed = 0;
		Profile.DcSetting.G_Brightness = 0;
		_dcGreen = 0;
		Profile.DcSetting.B_Brightness = 0;
		_dcBlue = 0;
		Profile.DcSetting.Effect = LightBarEffect.Monocolor;
		_dcRainbow = false;
	}

	private void DisableLightbar()
	{
		DisableAc();
		DisableDc();
		SetProfileColors();
		_client.SetLightbarProfile(Profile);
	}

	private LightbarSetting CopySetting(LightbarSetting setting)
	{
		return new LightbarSetting
		{
			R_Brightness = setting.R_Brightness,
			G_Brightness = setting.G_Brightness,
			B_Brightness = setting.B_Brightness,
			Effect = setting.Effect
		};
	}

	private LightbarProfile CopyToProfile(LightbarProfile sourceProf)
	{
		LightbarProfile lightbarProfile = new LightbarProfile();
		if (AcEnabled)
		{
			lightbarProfile.AcSetting = CopySetting(sourceProf.AcSetting);
			lightbarProfile.BreathingEnable = sourceProf.BreathingEnable;
		}
		else if (!AcEnabled && _previousProfile != null)
		{
			lightbarProfile.AcSetting = CopySetting(_previousProfile.AcSetting);
			lightbarProfile.BreathingEnable = _previousProfile.BreathingEnable;
		}
		else
		{
			lightbarProfile.AcSetting = CopySetting(_defaultProfile.AcSetting);
			lightbarProfile.BreathingEnable = _defaultProfile.BreathingEnable;
		}
		if (DcEnabled)
		{
			lightbarProfile.DcSetting = CopySetting(sourceProf.DcSetting);
		}
		else if (!DcEnabled && _previousProfile != null)
		{
			lightbarProfile.DcSetting = CopySetting(_previousProfile.DcSetting);
		}
		else
		{
			lightbarProfile.DcSetting = CopySetting(_defaultProfile.DcSetting);
		}
		return lightbarProfile;
	}

	private void GetProfileColors()
	{
		_acRed = Profile.AcSetting.R_Brightness;
		_acGreen = Profile.AcSetting.G_Brightness;
		_acBlue = Profile.AcSetting.B_Brightness;
		_acColor = Color.FromRgb(GetByte(AcRed), GetByte(AcGreen), GetByte(AcBlue));
		_acNewColor = _acColor;
		_dcRed = Profile.DcSetting.R_Brightness;
		_dcGreen = Profile.DcSetting.G_Brightness;
		_dcBlue = Profile.DcSetting.B_Brightness;
		_dcColor = Color.FromRgb(GetByte(DcRed), GetByte(DcGreen), GetByte(DcBlue));
		_dcNewColor = _dcColor;
	}

	private void SetProfileColors()
	{
		Profile.AcSetting.R_Brightness = AcRed;
		Profile.AcSetting.G_Brightness = AcGreen;
		Profile.AcSetting.B_Brightness = AcBlue;
		AcNewColor = Color.FromRgb(GetByte(AcRed), GetByte(AcGreen), GetByte(AcBlue));
		Profile.DcSetting.R_Brightness = DcRed;
		Profile.DcSetting.G_Brightness = DcGreen;
		Profile.DcSetting.B_Brightness = DcBlue;
		DcNewColor = Color.FromRgb(GetByte(DcRed), GetByte(DcGreen), GetByte(DcBlue));
	}

	public void SendProfileToSDK()
	{
		Task.Run(() =>
		{
			try
			{
				_client.SetLightbarProfile(Profile);
				if (AcColor != _acNewColor)
				{
					AcColor = _acNewColor;
				}
				if (DcColor != _dcNewColor)
				{
					DcColor = _dcNewColor;
				}
				RefreshUI();
			}
			catch (Exception exception)
			{
				_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
				try
				{
					_client = _clientgenerator.Client;
				}
				catch
				{
					_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
				}
			}
		}).ConfigureAwait(continueOnCapturedContext: false);
	}

	private byte GetByte(int c)
	{
		return (byte)((double)c / 100.0 * 255.0);
	}

	private int ReturnInt(byte b)
	{
		return (int)(double)(b * 100 / 255);
	}

	private void GetProfileEffects()
	{
		if (LightbarEnabled)
		{
			_breathingEnabled = Profile.BreathingEnable;
			switch (Profile.AcSetting.Effect)
			{
			case LightBarEffect.Monocolor:
				_acRainbow = false;
				break;
			case LightBarEffect.Rainbow:
				AcRainbow = true;
				break;
			}
			switch (Profile.DcSetting.Effect)
			{
			case LightBarEffect.Monocolor:
				_dcRainbow = false;
				break;
			case LightBarEffect.Rainbow:
				_dcRainbow = true;
				break;
			}
		}
		else
		{
			_breathingEnabled = false;
			_acRainbow = false;
			_dcRainbow = false;
		}
		if (_acRainbow)
		{
			_acEnabled = true;
		}
		if (_dcRainbow)
		{
			_dcEnabled = true;
		}
	}

	private LightbarProfile CreateDefaultProfile()
	{
		return new LightbarProfile
		{
			AcSetting = new LightbarSetting
			{
				Effect = LightBarEffect.Rainbow,
				R_Brightness = 100,
				G_Brightness = 100,
				B_Brightness = 100
			},
			DcSetting = new LightbarSetting
			{
				Effect = LightBarEffect.Monocolor,
				R_Brightness = 0,
				G_Brightness = 0,
				B_Brightness = 0
			},
			BreathingEnable = false
		};
	}

	private void SendIdleTimerToSdk(string timerValue, bool isAcTimer)
	{
		if (timerValue.Equals(OFF_CONST))
		{
			_client.SetLightBarIdleTimerValue(0, isAcTimer);
		}
		else
		{
			_client.SetLightBarIdleTimerValue(TimerStringToSeconds(timerValue), isAcTimer);
		}
	}

	private void RefreshUI()
	{
		RaisePropertyChanged("AcRed");
		RaisePropertyChanged("AcGreen");
		RaisePropertyChanged("AcBlue");
		RaisePropertyChanged("AcColor");
		RaisePropertyChanged("AcNewColor");
		RaisePropertyChanged("AcEnabled");
		RaisePropertyChanged("CurrentAcTimerValue");
		RaisePropertyChanged("DcRed");
		RaisePropertyChanged("DcGreen");
		RaisePropertyChanged("DcBlue");
		RaisePropertyChanged("DcColor");
		RaisePropertyChanged("DcNewColor");
		RaisePropertyChanged("DcEnabled");
		RaisePropertyChanged("CurrentDcTimerValue");
		RaisePropertyChanged("AcRainbow");
		RaisePropertyChanged("BreathingEnabled");
		RaisePropertyChanged("DcRainbow");
		RaisePropertyChanged("LightbarEnabled");
	}
}
