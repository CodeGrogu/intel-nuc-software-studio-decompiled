using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using Infrastructure.Logging;
using NucSoftwareStudioService.FeatureSupport.Services;

namespace NucSoftwareStudioService.FeatureSupport.Models;

public class ServiceAppSettingsManager
{
	private readonly NssServiceLogger _logger;

	private readonly AppSettingsService _settingsService;

	private ServiceHost _wcfSettingsServiceHost;

	public List<Feature> SupportedFeatures = new List<Feature>();

	public ServiceAppSettingsManager(NssServiceLogger logger)
	{
		_logger = logger;
		_settingsService = new AppSettingsService(logger);
		SupportedFeatures = _settingsService.SupportedFeatures;
		OpenWcfServices();
	}

	~ServiceAppSettingsManager()
	{
		_wcfSettingsServiceHost.Close();
	}

	private void OpenWcfServices()
	{
		try
		{
			_wcfSettingsServiceHost = new ServiceHost(_settingsService, new Uri("http://localhost:43564/NssService/service/AppSettings"));
			_wcfSettingsServiceHost.Open();
		}
		catch (Exception ex)
		{
			_logger.Log(ex.Message, LogEventId.Exception, EventLogEntryType.Error);
		}
	}
}
