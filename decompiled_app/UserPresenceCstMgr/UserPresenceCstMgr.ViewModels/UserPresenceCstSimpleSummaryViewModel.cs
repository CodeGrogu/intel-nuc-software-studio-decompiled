using CustomizationMgr.Enums;
using CustomizationMgr.Interfaces;
using CustomizationMgr.ViewModels;
using Intel.Cst.Client;
using Prism.Events;
using UserPresenceCstMgr.Events;
using UserPresenceCstMgr.Interfaces;
using UserPresenceCstMgr.Models;

namespace UserPresenceCstMgr.ViewModels;

public class UserPresenceCstSimpleSummaryViewModel : BaseSummaryViewModel
{
	private const IconType CARD_ICON_TYPE = IconType.UserPresence;

	private readonly IUserPresenceCstManager _userPresenceCstManager;

	private const string ENABLE_PROPERTY = "Enable";

	private bool _enable;

	private const string TIME_TO_LOCK_PROPERTY = "TimeToLock";

	private int _timeToLock;

	private const string LOCK_VALUE_PROPERTY = "LockValue";

	public bool Summary => true;

	public bool ServiceAvailable => _userPresenceCstManager.ServiceAvailable;

	public bool FeatureSupported => _userPresenceCstManager.FeatureSupported;

	public LockFeatureData LockFeatures
	{
		get
		{
			return _userPresenceCstManager.LockFeatures;
		}
		set
		{
			_userPresenceCstManager.LockFeatures = value;
		}
	}

	public WakeFeatureData WakeFeatures
	{
		get
		{
			return _userPresenceCstManager.WakeFeatures;
		}
		set
		{
			_userPresenceCstManager.WakeFeatures = value;
		}
	}

	public NoLockOnPresenceFeatureData NoLockOnPresenceFeatures
	{
		get
		{
			return _userPresenceCstManager.NoLockOnPresenceFeatures;
		}
		set
		{
			_userPresenceCstManager.NoLockOnPresenceFeatures = value;
		}
	}

	public bool Enable
	{
		get
		{
			return _enable;
		}
		set
		{
			LockFeatures.EnableLockWithExternalMonitor = value;
			WakeFeatures.EnableWakeWithExternalMonitor = value;
			NoLockOnPresenceFeatures.EnableNLoPWithExternalMonitor = value;
			SetProperty(ref _enable, value, "Enable");
		}
	}

	public int TimeToLock
	{
		get
		{
			return _timeToLock;
		}
		set
		{
			LockFeatures.DimmingInterval = value / LockFeatures.DimmingDivisor;
			LockFeatures.LockAfterDimInterval = value;
			SetProperty(ref _timeToLock, value, "TimeToLock");
			RaisePropertyChanged("LockValue");
		}
	}

	public double LockValue => LockFeatures.LockAfterDimInterval;

	public UserPresenceCstSimpleSummaryViewModel(IEventAggregator events, IUserPresenceCstManager userPresenceCstManager, ICustomResourceManager resourceManager)
		: base(resourceManager, IconType.UserPresence)
	{
		_userPresenceCstManager = userPresenceCstManager;
		events.GetEvent<UserPresenceCstSettingUpdatedEvent>().Subscribe(UpdateValues);
		_timeToLock = LockFeatures.LockAfterDimInterval;
		_enable = LockFeatures.EnableLockWithExternalMonitor && WakeFeatures.EnableWakeWithExternalMonitor && NoLockOnPresenceFeatures.EnableNLoPWithExternalMonitor;
	}

	private void UpdateValues(FeatureType data)
	{
		if (data == FeatureType.LOCK)
		{
			_timeToLock = LockFeatures.LockAfterDimInterval;
		}
		_enable = LockFeatures.EnableLockWithExternalMonitor && WakeFeatures.EnableWakeWithExternalMonitor && NoLockOnPresenceFeatures.EnableNLoPWithExternalMonitor;
		RaisePropertyChanged("TimeToLock");
		RaisePropertyChanged("Enable");
		RaisePropertyChanged("LockValue");
	}
}
