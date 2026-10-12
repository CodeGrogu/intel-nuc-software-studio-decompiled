using SystemMonitorMgr.Events;
using SystemMonitorMgr.Interfaces;
using CustomizationMgr.Interfaces;
using FeatureMgr.Enums;
using FeatureMgr.Models;
using Prism.Events;
using Prism.Logging;
using Prism.Regions;

namespace SystemMonitorMgr.ViewModels;

public class SystemMonitorCardViewModel : SystemMonitorViewModel
{
	private bool _isBatterySupported;

	private bool _isRNFanView;

	public NucType NUCType = PlatformDetail.GetNucType();

	public bool IsRNFanView
	{
		get
		{
			return _isRNFanView;
		}
		set
		{
			SetProperty(ref _isRNFanView, value, "IsRNFanView");
		}
	}

	public new bool IsBatterySupported
	{
		get
		{
			return _isBatterySupported;
		}
		set
		{
			SetProperty(ref _isBatterySupported, value, "IsBatterySupported");
		}
	}

	public SystemMonitorCardViewModel(IRegionManager regionMgr, IEventAggregator events, ILoggerFacade logger, ISystemMonitor monitor, ICustomResourceManager resourceManager)
		: base(regionMgr, events, logger, monitor, resourceManager)
	{
		if (NUCType == NucType.Raptor)
		{
			_isRNFanView = true;
		}
		else
		{
			_isRNFanView = false;
		}
		events.GetEvent<BatterySupportedEvent>().Subscribe(BatterySupported);
	}

	private void BatterySupported(bool isBatterySupported)
	{
		IsBatterySupported = isBatterySupported;
	}
}
