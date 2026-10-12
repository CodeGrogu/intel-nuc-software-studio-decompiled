using System;
using System.ServiceModel;
using Infrastructure.Logging;
using NucSoftwareStudioService.PerformanceTuning.Services;

namespace NucSoftwareStudioService.PerformanceTuning.Models;

public class PerformanceTuningManager
{
	public NssServiceLogger _logger;

	private ServiceHost _wcfPerformanceTuningServiceHost;

	private readonly PerformanceTuningService _performanceTuningService;

	public PerformanceTuningManager(NssServiceLogger logger)
	{
		_logger = logger;
		_performanceTuningService = new PerformanceTuningService();
		OpenWcfServices();
	}

	private void OpenWcfServices()
	{
		_wcfPerformanceTuningServiceHost = new ServiceHost(_performanceTuningService, new Uri("http://localhost:43568/NssService/service/PerformanceTuning"));
		_wcfPerformanceTuningServiceHost.Open();
	}
}
