using System.Diagnostics;
using AlexaLightBarMgr.Interfaces;
using AlexaLightBarMgr.Storage.Data;
using CustomizationMgr.Enums;
using CustomizationMgr.Interfaces;
using CustomizationMgr.ViewModels;
using Prism.Commands;
using Prism.Logging;

namespace AlexaLightBarMgr.ViewModels;

public class AlexaLightbarViewModel : BaseSummaryViewModel
{
	private const IconType CARD_ICON_TYPE = IconType.LightBar;

	private readonly ILoggerFacade _logger;

	private readonly IAlexaSettingsFileManager _alexaSettingsFileManager;

	private readonly AlexaServiceClient _alexaSettingsClient;

	private bool _on;

	private bool _alexaSettingHostNotPresent;

	private bool _downloadNeeded;

	private DelegateCommand _openMicrosoftLink;

	private bool _firstTimeInstall;

	public bool On
	{
		get
		{
			return _on;
		}
		set
		{
			SetProperty(ref _on, value, "On");
			_alexaSettingsClient.EnableAlexa(_on);
			_alexaSettingsFileManager.SaveAlexaSettings(new AlexaSettingsData
			{
				LightbarEnabled = _on
			});
		}
	}

	public bool AlexaSettingHostNotPresent
	{
		get
		{
			return _alexaSettingHostNotPresent;
		}
		set
		{
			SetProperty(ref _alexaSettingHostNotPresent, value, "AlexaSettingHostNotPresent");
		}
	}

	public bool A4pcDownloadNeeded
	{
		get
		{
			return _downloadNeeded;
		}
		set
		{
			SetProperty(ref _downloadNeeded, value, "A4pcDownloadNeeded");
		}
	}

	public DelegateCommand OpenMicrosoftLink
	{
		get
		{
			return _openMicrosoftLink;
		}
		set
		{
			SetProperty(ref _openMicrosoftLink, value, "OpenMicrosoftLink");
		}
	}

	public bool FirstTimeInstall
	{
		get
		{
			return _firstTimeInstall;
		}
		set
		{
			SetProperty(ref _firstTimeInstall, value, "FirstTimeInstall");
		}
	}

	public AlexaLightbarViewModel(ILoggerFacade logger, ICustomResourceManager resourceManager, IAlexaSettingsFileManager alexaSettingsFileManager)
		: base(resourceManager, IconType.LightBar)
	{
		_logger = logger;
		_alexaSettingsFileManager = alexaSettingsFileManager;
		_on = false;
		_downloadNeeded = true;
		_alexaSettingHostNotPresent = true;
		_openMicrosoftLink = new DelegateCommand(OpenLink);
		_alexaSettingsClient = new AlexaServiceClient(_logger);
		if (!AlexaServiceClient.A4pcDownloadNeeded())
		{
			_downloadNeeded = false;
		}
		if (!_alexaSettingsClient.AlexaHostPresent)
		{
			return;
		}
		_alexaSettingHostNotPresent = false;
		if (!_downloadNeeded)
		{
			if (resourceManager.IsFirstLaunch())
			{
				On = true;
				return;
			}
			AlexaSettingsData alexaSettingsData = _alexaSettingsFileManager.ReadSavedAlexaSettings();
			On = alexaSettingsData.LightbarEnabled;
		}
	}

	private void OpenLink()
	{
		Process.Start("https://www.microsoft.com/en-us/p/alexa/9n12z3cctcnz?cid=intellightring");
	}
}
