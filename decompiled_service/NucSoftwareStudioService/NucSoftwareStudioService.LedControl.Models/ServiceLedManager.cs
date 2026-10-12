using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.ServiceModel;
using Infrastructure.Logging;
using Microsoft.Win32;
using Newtonsoft.Json;
using NucSoftwareStudioService.FeatureSupport.Models;
using NucSoftwareStudioService.LedControl.Services;

namespace NucSoftwareStudioService.LedControl.Models;

public class ServiceLedManager
{
	private const string SETTINGS_FILE_NAME = "LedServiceSettings";

	private static readonly string _settingsFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "LedServiceSettings");

	private readonly AlexaSettingsService _alexaSettingsService;

	private readonly AlexaStateService _alexaStateService;

	private readonly NucLedService _nucLedService;

	private readonly ArduinoService _arduinoService;

	private readonly EneService _eneService;

	private readonly NssServiceLogger _logger;

	private ServiceHost _wcfAlexaStateServiceHost;

	private ServiceHost _wcfAlexaSettingsServiceHost;

	private ServiceHost _wcfNucLedServiceHost;

	private ServiceHost _wcfArduinoServiceHost;

	private ServiceHost _wcfEneServiceHost;

	private readonly List<Feature> _supportedFeatures = new List<Feature>();

	public ServiceLedManager(NssServiceLogger logger, List<Feature> SupportedFeatures)
	{
		_logger = logger;
		_supportedFeatures = SupportedFeatures;
		if (IsAlexaSupported() && UpdateRegKey())
		{
			AlexaLedSettings savedSettings = GetSavedSettings();
			_alexaStateService = new AlexaStateService(savedSettings, _logger);
			_alexaSettingsService = new AlexaSettingsService(savedSettings, SetSavedSettings, _alexaStateService.EnableAlexa, _alexaStateService.TuneAlexaEffects);
		}
		_nucLedService = new NucLedService();
		_arduinoService = new ArduinoService();
		_eneService = new EneService();
		OpenWcfServices();
	}

	~ServiceLedManager()
	{
		_wcfAlexaSettingsServiceHost?.Close();
		_wcfAlexaStateServiceHost?.Close();
		_wcfNucLedServiceHost?.Close();
		_wcfEneServiceHost?.Close();
	}

	private AlexaLedSettings GetSavedSettings()
	{
		AlexaLedSettings alexaLedSettings = new AlexaLedSettings();
		if (File.Exists(_settingsFile))
		{
			AlexaLedSettings alexaLedSettings2 = null;
			try
			{
				alexaLedSettings2 = JsonConvert.DeserializeObject<AlexaLedSettings>(File.ReadAllText(_settingsFile));
			}
			catch (Exception ex)
			{
				_logger?.Log(ex.Message, LogEventId.Exception, EventLogEntryType.Error);
			}
			if (alexaLedSettings2 != null)
			{
				alexaLedSettings.Enabled = alexaLedSettings2.Enabled;
				if (alexaLedSettings2.AlexaAnimationSettings != null)
				{
					alexaLedSettings.AlexaAnimationSettings = alexaLedSettings2.AlexaAnimationSettings;
				}
				if (alexaLedSettings2.LedInterface != FeatureInterfaceType.Error)
				{
					alexaLedSettings.LedInterface = alexaLedSettings2.LedInterface;
				}
				else
				{
					alexaLedSettings.LedInterface = GetAlexaLightBarInterface();
					if (alexaLedSettings.LedInterface != FeatureInterfaceType.Error)
					{
						SetSavedSettings(alexaLedSettings);
					}
				}
			}
		}
		else
		{
			alexaLedSettings.LedInterface = GetAlexaLightBarInterface();
		}
		return alexaLedSettings;
	}

	private FeatureInterfaceType GetAlexaLightBarInterface()
	{
		foreach (Feature supportedFeature in _supportedFeatures)
		{
			if (supportedFeature.Type == FeatureType.LightbarMultiColor)
			{
				if (supportedFeature.SupportedInterfaces.Contains(FeatureInterfaceType.Wmi))
				{
					return FeatureInterfaceType.Wmi;
				}
				if (supportedFeature.SupportedInterfaces.Contains(FeatureInterfaceType.Usb))
				{
					return FeatureInterfaceType.Usb;
				}
				return FeatureInterfaceType.Error;
			}
		}
		return FeatureInterfaceType.Error;
	}

	private bool IsAlexaSupported()
	{
		foreach (Feature supportedFeature in _supportedFeatures)
		{
			if (supportedFeature.SupportedInterfaces.Contains(FeatureInterfaceType.Alexa))
			{
				return true;
			}
		}
		return false;
	}

	private bool UpdateRegKey()
	{
		try
		{
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\WOW6432Node\\Amazon\\Alexa", writable: true);
			if (registryKey == null)
			{
				registryKey = Registry.LocalMachine.CreateSubKey("Software\\WOW6432Node\\Amazon\\Alexa", writable: true);
			}
			object obj = registryKey?.GetValue("Extension");
			string text = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "NssAlexaStateHandler.dll");
			if (obj != null && obj is string text2 && text2 == text)
			{
				return true;
			}
			registryKey.SetValue("Extension", text);
			return true;
		}
		catch (Exception ex)
		{
			_logger?.Log(ex.Message, LogEventId.Exception, EventLogEntryType.Error);
			return false;
		}
	}

	private void SetSavedSettings(AlexaLedSettings settings)
	{
		File.WriteAllText(_settingsFile, JsonConvert.SerializeObject(settings));
	}

	private void OpenWcfServices()
	{
		foreach (Feature supportedFeature in _supportedFeatures)
		{
			if (supportedFeature.Type == FeatureType.ChassisLed && supportedFeature.SupportedInterfaces.Contains(FeatureInterfaceType.Wmi))
			{
				_wcfNucLedServiceHost = new ServiceHost(_nucLedService, new Uri("http://localhost:43565/NssService/service/NucLed"));
				try
				{
					_wcfNucLedServiceHost.Open();
				}
				catch (Exception ex)
				{
					_logger?.Log(ex.Message, LogEventId.Exception, EventLogEntryType.Error);
				}
			}
			if (supportedFeature.Type == FeatureType.ChassisLed && supportedFeature.SupportedInterfaces.Contains(FeatureInterfaceType.Arduino))
			{
				_wcfArduinoServiceHost = new ServiceHost(_arduinoService, new Uri("http://localhost:43567/NssService/service/Arduino"));
				_wcfArduinoServiceHost.Open();
				_wcfEneServiceHost = new ServiceHost(_eneService, new Uri("http://localhost:43569/NssService/service/Ene"));
				_wcfEneServiceHost.Open();
			}
			if (!supportedFeature.SupportedInterfaces.Contains(FeatureInterfaceType.Alexa))
			{
				continue;
			}
			if (_wcfAlexaSettingsServiceHost == null)
			{
				_wcfAlexaSettingsServiceHost = new ServiceHost(_alexaSettingsService, new Uri("http://localhost:43562/NssService/service/AlexaSettings"));
				try
				{
					_logger.Log("Alexa Settings Service opening", LogEventId.Information);
					_wcfAlexaSettingsServiceHost.Open();
				}
				catch (Exception ex2)
				{
					_logger?.Log(ex2.Message, LogEventId.Exception, EventLogEntryType.Error);
				}
			}
			else
			{
				_logger?.Log("Alexa Settings Service already running", LogEventId.Information);
			}
			if (_wcfAlexaStateServiceHost == null)
			{
				_wcfAlexaStateServiceHost = new ServiceHost(_alexaStateService, new Uri("http://localhost:43563/NssService/service/AlexaState"));
				try
				{
					_logger.Log("Alexa State Service opening", LogEventId.Information);
					_wcfAlexaStateServiceHost.Open();
				}
				catch (Exception ex3)
				{
					_logger?.Log(ex3.Message, LogEventId.Exception, EventLogEntryType.Error);
				}
			}
			else
			{
				_logger?.Log("Alexa Settings Service already running", LogEventId.Information);
			}
		}
	}
}
