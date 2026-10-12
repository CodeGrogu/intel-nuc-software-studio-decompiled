using System;
using System.Diagnostics;
using System.ServiceModel;
using SystemMonitorLibrary.Client;
using Infrastructure.Logging;
using NucSoftwareStudioService.SystemMonitor.Services;
using NucSoftwareStudioService.Utilities;

namespace NucSoftwareStudioService.SystemMonitor.Models;

public class SystemMonitorManager
{
	public NssServiceLogger _logger;

	private SystemMonitorLibraryClient _monitor;

	private readonly SystemMonitorService _systemMonitorService;

	private ServiceHost _wcfSystemMonitorServiceHost;

	public SystemMonitorManager(NssServiceLogger logger)
	{
		_logger = logger;
		_monitor = new SystemMonitorLibraryClient(_logger);
		_systemMonitorService = new SystemMonitorService(SubscribeToSystemMonitor);
		OpenWcfServices();
	}

	~SystemMonitorManager()
	{
		_wcfSystemMonitorServiceHost?.Close();
		if (_monitor != null)
		{
			_monitor.UpdateDataCallback = null;
		}
	}

	private void SubscribeToSystemMonitor(bool Subscribe)
	{
		if (_monitor != null)
		{
			if (Subscribe)
			{
				_monitor.Subscribe(Subscribe, _systemMonitorService.Callback.SystemMonitorDataCallback);
			}
			else
			{
				_monitor.Subscribe(Subscribe);
			}
		}
	}

	private void OpenWcfServices()
	{
		_wcfSystemMonitorServiceHost = new ServiceHost(_systemMonitorService, new Uri("http://localhost:43566/NssService/service/SystemMonitor"), new Uri(UrlUtil.ConvertUri("http://localhost:43566/NssService/service/SystemMonitor", ConvType.N_PIPE)));
		try
		{
			_wcfSystemMonitorServiceHost.Open();
		}
		catch (Exception ex)
		{
			_logger?.Log(ex.Message, LogEventId.Exception, EventLogEntryType.Error);
		}
	}
}
