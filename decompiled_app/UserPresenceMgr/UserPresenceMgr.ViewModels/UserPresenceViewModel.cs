using CustomizationMgr.Enums;
using CustomizationMgr.Interfaces;
using CustomizationMgr.ViewModels;
using UserPresenceMgr.Interfaces;
using UserPresenceMgr.Models;

namespace UserPresenceMgr.ViewModels;

public class UserPresenceViewModel : BaseDetailViewModel
{
	private const IconType ICON_TYPE = IconType.UserPresence;

	private readonly IUserPresenceManager _userPresenceManager;

	public bool Summary => false;

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

	public UserPresenceViewModel(IUserPresenceManager userPresenceManager, ICustomResourceManager resourceManager)
		: base(resourceManager, IconType.UserPresence)
	{
		_userPresenceManager = userPresenceManager;
	}
}
