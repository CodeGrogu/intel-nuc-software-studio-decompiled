using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CustomizationMgr.Enums;
using CustomizationMgr.Interfaces;
using CustomizationMgr.ViewModels;
using Infrastructure.Models;
using MobileDisplayModeMgr.Events;
using MobileDisplayModeMgr.Interfaces;
using MobileDisplayModeMgr.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Logging;
using Prism.Regions;
using UniwillSdkClient.Events;
using UniwillSdkClient.Interfaces;
using UniwillSdkClient.UniwillService;

namespace MobileDisplayModeMgr.ViewModels;

internal class MobileDisplayModeSettingsMgr : NavBaseSummaryViewModel, IMobileDisplayModeSettingsMgr
{
	private static readonly string CARD_TITLE_STRING = "DisplayModeCardTitleString";

	private const string DETAIL_CARD_NAME = "MobileDisplayModeView";

	private const IconType CARD_ICON_TYPE = IconType.DisplayMode;

	private UniwillServiceClient _client;

	private readonly IEventAggregator _events;

	private readonly ILoggerFacade _logger;

	private MobileDisplayModeCustomSettings _customSettings;

	private bool _errorMessage;

	private bool _brightnessSupported = true;

	private bool _contrastSupported = true;

	private bool _redSupported = true;

	private bool _greenSupported = true;

	private bool _blueSupported = true;

	private bool _colorTemperatureSupported = true;

	private ObservableCollection<DisplayMode> _modes;

	private DisplayMode _currentMode;

	private int _acBrightness;

	private int _dcBrightness;

	private float _currentContrast;

	private int _currentTemp;

	private int _currentRed;

	private int _currentGreen;

	private int _currentBlue;

	private DelegateCommand<DisplayMode> _selectMode;

	private DelegateCommand _resetMode;

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

	public bool BrightnessSupported
	{
		get
		{
			return _brightnessSupported;
		}
		set
		{
			SetProperty(ref _brightnessSupported, value, "BrightnessSupported");
		}
	}

	public bool ContrastSupported
	{
		get
		{
			return _contrastSupported;
		}
		set
		{
			SetProperty(ref _contrastSupported, value, "ContrastSupported");
		}
	}

	public int BrightnessContrastMin => 0;

	public int BrightnessContrastMax => 100;

	public bool RedSupported
	{
		get
		{
			return _redSupported;
		}
		set
		{
			SetProperty(ref _redSupported, value, "RedSupported");
		}
	}

	public bool GreenSupported
	{
		get
		{
			return _greenSupported;
		}
		set
		{
			SetProperty(ref _greenSupported, value, "GreenSupported");
		}
	}

	public bool BlueSupported
	{
		get
		{
			return _blueSupported;
		}
		set
		{
			SetProperty(ref _blueSupported, value, "BlueSupported");
		}
	}

	public int ColorMin => 0;

	public int ColorMax => 255;

	public bool ColorTemperatureSupported
	{
		get
		{
			return _colorTemperatureSupported;
		}
		set
		{
			SetProperty(ref _colorTemperatureSupported, value, "ColorTemperatureSupported");
		}
	}

	public int ColorTempMin => 2400;

	public int ColorTempMax => 9500;

	public int ColorTempInterval => 100;

	public ObservableCollection<DisplayMode> Modes
	{
		get
		{
			return _modes;
		}
		set
		{
			SetProperty(ref _modes, value, "Modes");
		}
	}

	public DisplayMode CurrentMode
	{
		get
		{
			return _currentMode;
		}
		set
		{
			if (value != null && (CurrentMode == null || (CurrentMode != null && !AreTheSame(CurrentMode, value))))
			{
				SetProperty(ref _currentMode, value, "CurrentMode");
				_client.SetDisplayMode(_currentMode.Index);
				SetCurrentModeVariables();
			}
		}
	}

	public int AcBrightness
	{
		get
		{
			return _acBrightness;
		}
		set
		{
			SetProperty(ref _acBrightness, value, "AcBrightness");
			_currentMode.BrightnessAC = value;
		}
	}

	public int DcBrightness
	{
		get
		{
			return _dcBrightness;
		}
		set
		{
			SetProperty(ref _dcBrightness, value, "DcBrightness");
			_currentMode.BrightnessDC = value;
		}
	}

	public float CurrentContrast
	{
		get
		{
			return _currentContrast;
		}
		set
		{
			SetProperty(ref _currentContrast, value, "CurrentContrast");
			_currentMode.Contrast = value;
		}
	}

	public int CurrentTemp
	{
		get
		{
			return _currentTemp;
		}
		set
		{
			SetProperty(ref _currentTemp, value, "CurrentTemp");
			_currentMode.ColorTemp = value;
		}
	}

	public int CurrentRed
	{
		get
		{
			return _currentRed;
		}
		set
		{
			SetProperty(ref _currentRed, value, "CurrentRed");
			_currentMode.Red = value;
		}
	}

	public int CurrentGreen
	{
		get
		{
			return _currentGreen;
		}
		set
		{
			SetProperty(ref _currentGreen, value, "CurrentGreen");
			_currentMode.Green = value;
		}
	}

	public int CurrentBlue
	{
		get
		{
			return _currentBlue;
		}
		set
		{
			SetProperty(ref _currentBlue, value, "CurrentBlue");
			_currentMode.Blue = value;
		}
	}

	public DelegateCommand<DisplayMode> SelectMode
	{
		get
		{
			return _selectMode;
		}
		set
		{
			SetProperty(ref _selectMode, value, "SelectMode");
		}
	}

	public DelegateCommand ResetMode
	{
		get
		{
			return _resetMode;
		}
		set
		{
			SetProperty(ref _resetMode, value, "ResetMode");
		}
	}

	public MobileDisplayModeSettingsMgr(IEventAggregator events, IRegionManager regionMgr, ILoggerFacade logger, ICustomResourceManager resourceManager, IUniwillClient client)
		: base(regionMgr, resourceManager, CARD_TITLE_STRING, "MobileDisplayModeView", IconType.DisplayMode)
	{
		MobileDisplayModeSettingsMgr mobileDisplayModeSettingsMgr = this;
		_logger = logger;
		_events = events;
		events.GetEvent<SelectedDisplayModeEvent>().Subscribe(SelectedMode);
		events.GetEvent<DisplayModeUiUpdateEvent>().Subscribe(UpdateUi);
		_selectMode = new DelegateCommand<DisplayMode>(ChangeMode);
		Task.Run(() =>
		{
			try
			{
				mobileDisplayModeSettingsMgr._client = client.Client;
				if (mobileDisplayModeSettingsMgr._client == null)
				{
					throw new ArgumentNullException("Unable to connect to SDK");
				}
				mobileDisplayModeSettingsMgr._client.EnableDisplayModeManagement(Enable: true);
				if (resourceManager.CustomAppSettings?.Settings?.Find((object x) => x.GetType().Equals(typeof(MobileDisplayModeCustomSettings))) is MobileDisplayModeCustomSettings customSettings)
				{
					mobileDisplayModeSettingsMgr._customSettings = customSettings;
					mobileDisplayModeSettingsMgr._resetMode = new DelegateCommand(mobileDisplayModeSettingsMgr.ResetCurrentModeToCustomDefaults);
					mobileDisplayModeSettingsMgr.RaisePropertyChanged("ResetMode");
					if (resourceManager.IsFirstLaunch())
					{
						mobileDisplayModeSettingsMgr.LoadCustomDefaults(customSettings);
						return;
					}
				}
				else
				{
					mobileDisplayModeSettingsMgr._resetMode = new DelegateCommand(mobileDisplayModeSettingsMgr.ResetCurrentModeToClientDefaults);
					mobileDisplayModeSettingsMgr.RaisePropertyChanged("ResetMode");
				}
				mobileDisplayModeSettingsMgr.LoadClientDefaults();
			}
			catch (Exception exception)
			{
				mobileDisplayModeSettingsMgr.ErrorMessage = true;
				mobileDisplayModeSettingsMgr.OpenDetailView = null;
				mobileDisplayModeSettingsMgr._logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
			}
		}).ConfigureAwait(continueOnCapturedContext: false);
	}

	private DisplayMode CopyMode(DisplayMode mode)
	{
		return new DisplayMode
		{
			Type = mode.Type,
			Index = mode.Index,
			Red = mode.Red,
			Green = mode.Green,
			Blue = mode.Blue,
			BrightnessAC = mode.BrightnessAC,
			BrightnessDC = mode.BrightnessDC,
			ColorTemp = mode.ColorTemp,
			Contrast = mode.Contrast
		};
	}

	private void LoadCustomDefaults(MobileDisplayModeCustomSettings customSettings)
	{
		Application.Current.Dispatcher.Invoke(() =>
		{
			Modes = new ObservableCollection<DisplayMode>();
			DisplayMode[] customDisplayModes = customSettings.GetCustomDisplayModes();
			foreach (DisplayMode mode in customDisplayModes)
			{
				_client.UpdateDisplayMode(mode);
				Modes.Add(CopyMode(mode));
			}
			CurrentMode = Modes.FirstOrDefault((DisplayMode x) => x.Index == customSettings.CurrentDisplayModeIndex);
		});
	}

	private void LoadClientDefaults()
	{
		Modes = new ObservableCollection<DisplayMode>(_client.GetSavedDisplayModes());
		_client.GetCurrentDisplayMode();
	}

	private void ResetCurrentModeToCustomDefaults()
	{
		_client.UpdateDisplayMode(_customSettings.GetCustomDisplayModes().FirstOrDefault((DisplayMode x) => x.Index == CurrentMode.Index));
	}

	private void ResetCurrentModeToClientDefaults()
	{
		_client.RecoverDisplayMode(CurrentMode.Index);
	}

	private void ChangeMode(DisplayMode mode)
	{
		CurrentMode = Modes.FirstOrDefault((DisplayMode x) => x.Type == mode.Type);
	}

	private void SetEnablingVariables()
	{
		switch (CurrentMode.Type)
		{
		case DisplayModeType.Standard:
			BrightnessSupported = false;
			ContrastSupported = false;
			ColorTemperatureSupported = false;
			RedSupported = false;
			GreenSupported = false;
			BlueSupported = false;
			break;
		case DisplayModeType.Gaming:
		case DisplayModeType.Custom:
			BrightnessSupported = true;
			ContrastSupported = true;
			ColorTemperatureSupported = true;
			RedSupported = true;
			GreenSupported = true;
			BlueSupported = true;
			break;
		case DisplayModeType.Video:
			BrightnessSupported = true;
			ContrastSupported = false;
			ColorTemperatureSupported = true;
			RedSupported = false;
			GreenSupported = false;
			BlueSupported = false;
			break;
		case DisplayModeType.Reading:
			BrightnessSupported = true;
			ContrastSupported = false;
			ColorTemperatureSupported = true;
			RedSupported = false;
			GreenSupported = false;
			BlueSupported = true;
			break;
		}
	}

	public void UpdateSdk(object sender, EventArgs args)
	{
		_client.UpdateDisplayModeAsync(CurrentMode);
	}

	private void SelectedMode(DisplayMode Mode)
	{
		int i = 0;
		while (i < Modes.Count)
		{
			if (Modes[i].Index == Mode.Index)
			{
				if (!AreTheSame(Modes[i], Mode))
				{
					Application.Current.Dispatcher.Invoke(() =>
					{
						Modes[i] = Mode;
						RaisePropertyChanged("Modes");
					});
				}
				_currentMode = Modes[i];
				SetCurrentModeVariables();
				SetEnablingVariables();
				_events.GetEvent<DisplayModeUiUpdateEvent>().Publish();
				break;
			}
			int num = i + 1;
			i = num;
		}
	}

	private bool AreTheSame(DisplayMode source, DisplayMode target)
	{
		if (source == null || target == null)
		{
			return false;
		}
		if (source.Red != target.Red || source.Green != target.Green || source.Blue != target.Blue || source.BrightnessAC != target.BrightnessAC || source.BrightnessDC != target.BrightnessDC || source.Contrast != target.Contrast || source.ColorTemp != target.ColorTemp || source.Index != target.Index || source.Type != target.Type)
		{
			return false;
		}
		return true;
	}

	private void SetCurrentModeVariables()
	{
		_acBrightness = _currentMode.BrightnessAC;
		_dcBrightness = _currentMode.BrightnessDC;
		_currentContrast = _currentMode.Contrast;
		_currentTemp = _currentMode.ColorTemp;
		_currentRed = _currentMode.Red;
		_currentGreen = _currentMode.Green;
		_currentBlue = _currentMode.Blue;
	}

	private void UpdateUi()
	{
		RaisePropertyChanged("Modes");
		RaisePropertyChanged("CurrentMode");
		RaisePropertyChanged("AcBrightness");
		RaisePropertyChanged("DcBrightness");
		RaisePropertyChanged("CurrentContrast");
		RaisePropertyChanged("CurrentTemp");
		RaisePropertyChanged("CurrentRed");
		RaisePropertyChanged("CurrentGreen");
		RaisePropertyChanged("CurrentBlue");
	}
}
