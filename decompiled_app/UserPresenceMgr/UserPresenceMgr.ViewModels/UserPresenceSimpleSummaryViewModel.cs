using ContextSensingClient;
using CustomizationMgr.Enums;
using CustomizationMgr.Interfaces;
using CustomizationMgr.ViewModels;
using Prism.Events;
using UserPresenceMgr.Events;
using UserPresenceMgr.Interfaces;
using UserPresenceMgr.Models;

namespace UserPresenceMgr.ViewModels;

public class UserPresenceSimpleSummaryViewModel : BaseSummaryViewModel
{
	private const IconType CARD_ICON_TYPE = IconType.UserPresence;

	private readonly IUserPresenceManager _userPresenceManager;

	private const string ENABLE_PROPERTY = "Enable";

	private bool _enable;

	private const string TIME_TO_LOCK_PROPERTY = "TimeToLock";

	private int _timeToLock;

	private const string LOCK_VALUE_PROPERTY = "LockValue";

	public bool Summary => true;

	public bool ServiceAvailable => _userPresenceManager.ServiceAvailable;

	public bool FeatureSupported => _userPresenceManager.FeatureSupported;

	public LockFeatureData LockFeatures
	{
		get
		{
			return _userPresenceManager.LockFeatures;
		}
		set
		{
			_userPresenceManager.LockFeatures = value;
		}
	}

	public WakeFeatureData WakeFeatures
	{
		get
		{
			return _userPresenceManager.WakeFeatures;
		}
		set
		{
			_userPresenceManager.WakeFeatures = value;
		}
	}

	public NoLockOnPresenceFeatureData NoLockOnPresenceFeatures
	{
		get
		{
			return _userPresenceManager.NoLockOnPresenceFeatures;
		}
		set
		{
			_userPresenceManager.NoLockOnPresenceFeatures = value;
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

	public double LockValue => LockFeatures.PreDimInterval + LockFeatures.LockAfterDimInterval;

	public UserPresenceSimpleSummaryViewModel(IEventAggregator events, IUserPresenceManager userPresenceManager, ICustomResourceManager resourceManager)
		: base(resourceManager, IconType.UserPresence)
	{
		_userPresenceManager = userPresenceManager;
		events.GetEvent<UserPresenceSettingUpdatedEvent>().Subscribe(UpdateValues);
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
