using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using CustomizationMgr.Enums;
using CustomizationMgr.Interfaces;
using Infrastructure.Converters;
using Infrastructure.Events;
using Infrastructure.Models;
using NucSoftwareStudio.Models;
using NucSoftwareStudio.Utilities;
using Prism.Commands;
using Prism.Events;
using Prism.Logging;
using Prism.Mvvm;
using Windows.Storage;

namespace NucSoftwareStudio.ViewModels;

public class MainWindowViewModel : BindableBase
{
	private const string EULA_KEY = "EULA";

	private const string EULA_ACCEPTED_VALUE = "Accepted";

	private double _windowWidth;

	private double _windowHeight;

	private readonly ILoggerFacade _logger;

	private readonly ICustomResourceManager _resourceManager;

	private readonly IEventAggregator _events;

	private string _appNameString = "";

	private DelegateCommand _showSettings;

	private DelegateCommand _closeSettings;

	private string _version;

	private string _licenses;

	private DelegateCommand _licensesClicked;

	private bool _settingsVisible;

	private bool _showEula;

	private DelegateCommand _closeEula;

	private DelegateCommand _acceptEula;

	public double WindowWidth
	{
		get
		{
			return _windowWidth;
		}
		set
		{
			SetProperty(ref _windowWidth, value, "WindowWidth");
		}
	}

	public double WindowHeight
	{
		get
		{
			return _windowHeight;
		}
		set
		{
			SetProperty(ref _windowHeight, value, "WindowHeight");
		}
	}

	public string AppNameString
	{
		get
		{
			return _appNameString;
		}
		set
		{
			SetProperty(ref _appNameString, value, "AppNameString");
		}
	}

	public DelegateCommand ShowSettings
	{
		get
		{
			return _showSettings;
		}
		set
		{
			SetProperty(ref _showSettings, value, "ShowSettings");
		}
	}

	public DelegateCommand CloseSettings
	{
		get
		{
			return _closeSettings;
		}
		set
		{
			SetProperty(ref _closeSettings, value, "CloseSettings");
		}
	}

	public string Version
	{
		get
		{
			return _version;
		}
		set
		{
			SetProperty(ref _version, value, "Version");
		}
	}

	public string Licenses
	{
		get
		{
			return _licenses;
		}
		set
		{
			SetProperty(ref _licenses, value, "Licenses");
		}
	}

	public DelegateCommand LicensesClicked
	{
		get
		{
			return _licensesClicked;
		}
		set
		{
			SetProperty(ref _licensesClicked, value, "LicensesClicked");
		}
	}

	public bool SettingsVisible
	{
		get
		{
			return _settingsVisible;
		}
		set
		{
			SetProperty(ref _settingsVisible, value, "SettingsVisible");
		}
	}

	public bool ShowEula
	{
		get
		{
			return _showEula;
		}
		set
		{
			SetProperty(ref _showEula, value, "ShowEula");
		}
	}

	public DelegateCommand CloseEula
	{
		get
		{
			return _closeEula;
		}
		set
		{
			SetProperty(ref _closeEula, value, "CloseEula");
		}
	}

	public DelegateCommand AcceptEula
	{
		get
		{
			return _acceptEula;
		}
		set
		{
			SetProperty(ref _acceptEula, value, "AcceptEula");
		}
	}

	public bool ShowIntelLogo { get; set; }

	public BitmapSource AppLogo { get; set; }

	public MainWindowViewModel(ILoggerFacade logger, ICustomResourceManager resourceManager, IEventAggregator events)
	{
		_logger = logger;
		_resourceManager = resourceManager;
		_events = events;
		_events.GetEvent<LogoLoadedEvent>().Subscribe(SetLogo);
		_events.GetEvent<CustomAppTitleEvent>().Subscribe((bool _) =>
		{
			AppNameString = ResourceDictionaryConverter.GetString("MainTitleBarAppName");
		});
		_version = ResourceDictionaryConverter.GetString("Version") + " " + AppVersionData.GetVersionString();
		_licenses = ResourceDictionaryConverter.GetString("Licenses");
		_settingsVisible = false;
		_showSettings = new DelegateCommand(() =>
		{
			if (!ShowEula)
			{
				SettingsVisible = true;
			}
		});
		_closeSettings = new DelegateCommand(() =>
		{
			SettingsVisible = false;
		});
		_closeEula = new DelegateCommand(Application.Current.Shutdown);
		_acceptEula = new DelegateCommand(EulaAccepted);
		_licensesClicked = new DelegateCommand(OnLicensesClicked);
		SetWindowSize();
		_showEula = CheckEulaState();
		ApplicationConstants.CreateOptimizationsFolderWithMulticoreJIT();
		Task.Run(() =>
		{
			try
			{
				LicensesUtil.StoreLicenses();
			}
			catch (Exception exception)
			{
				_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
			}
		}).ConfigureAwait(continueOnCapturedContext: false);
	}

	private void SetLogo()
	{
		KeyValuePair<AppLogoType, BitmapSource> keyValuePair = _resourceManager.AppLogo();
		if (keyValuePair.Key == AppLogoType.Default)
		{
			ShowIntelLogo = true;
			RaisePropertyChanged("ShowIntelLogo");
			return;
		}
		ShowIntelLogo = false;
		if (keyValuePair.Key == AppLogoType.Custom)
		{
			AppLogo = keyValuePair.Value;
			RaisePropertyChanged("AppLogo");
		}
	}

	private void SetWindowSize()
	{
		double primaryScreenWidth = SystemParameters.PrimaryScreenWidth;
		double primaryScreenHeight = SystemParameters.PrimaryScreenHeight;
		double num = primaryScreenWidth / primaryScreenHeight;
		WindowWidth = primaryScreenWidth / 1.4;
		WindowHeight = _windowWidth / num;
	}

	private bool CheckEulaState()
	{
		object obj = null;
		try
		{
			obj = ApplicationData.Current.LocalSettings.Values["EULA"];
		}
		catch (Exception exception)
		{
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
		}
		if (obj != null)
		{
			return !((string)obj).Equals("Accepted");
		}
		return true;
	}

	private void EulaAccepted()
	{
		ShowEula = false;
		try
		{
			ApplicationData.Current.LocalSettings.Values["EULA"] = "Accepted";
		}
		catch (Exception exception)
		{
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
		}
	}

	private void OnLicensesClicked()
	{
		try
		{
			if (!Directory.Exists(LicensesUtil.GetLicensesDirectoryRealPath()))
			{
				LicensesUtil.StoreLicenses();
			}
			Process.Start(LicensesUtil.GetLicensesDirectoryRealPath());
		}
		catch (Exception exception)
		{
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
		}
	}
}
