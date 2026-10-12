using System.ComponentModel;
using System.ServiceProcess;
using System.Threading.Tasks;
using Infrastructure.Logging;
using NucSoftwareStudioService.FeatureSupport.Models;
using NucSoftwareStudioService.LedControl.Models;
using NucSoftwareStudioService.PerformanceTuning.Models;
using NucSoftwareStudioService.SystemMonitor.Models;

namespace NucSoftwareStudioService;

internal class NssService : ServiceBase
{
	private ServiceLedManager _ledManager;

	private ServiceAppSettingsManager _appSettingsManager;

	private SystemMonitorManager _sysMonitor;

	private PerformanceTuningManager _sysPerformance;

	private NssServiceLogger _logger;

	private IContainer components;

	public NssService()
	{
		AutoLog = false;
		_logger = new NssServiceLogger();
		InitializeComponent();
	}

	protected override void OnStart(string[] args)
	{
		Task.Run(() =>
		{
			_logger.Log("---------------- Starting NSS Service ----------------", LogEventId.Information);
			_appSettingsManager = new ServiceAppSettingsManager(_logger);
			_ledManager = new ServiceLedManager(_logger, _appSettingsManager.SupportedFeatures);
			_sysMonitor = new SystemMonitorManager(_logger);
			_sysPerformance = new PerformanceTuningManager(_logger);
		});
	}

	protected override void OnStop()
	{
		_logger.Log("---------------- Stopping NSS Service ----------------", LogEventId.Information);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		components = new Container();
		ServiceName = "NssService";
	}
}
