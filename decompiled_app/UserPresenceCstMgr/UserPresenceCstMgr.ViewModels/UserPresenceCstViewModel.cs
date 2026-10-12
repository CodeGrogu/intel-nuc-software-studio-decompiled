using CustomizationMgr.Enums;
using CustomizationMgr.Interfaces;
using CustomizationMgr.ViewModels;
using UserPresenceCstMgr.Interfaces;
using UserPresenceCstMgr.Models;

namespace UserPresenceCstMgr.ViewModels;

public class UserPresenceCstViewModel : BaseDetailViewModel
{
	private const IconType ICON_TYPE = IconType.UserPresence;

	private readonly IUserPresenceCstManager _userPresenceCstManager;

	public bool Summary => false;

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

	public UserPresenceCstViewModel(IUserPresenceCstManager userPresenceCstManager, ICustomResourceManager resourceManager)
		: base(resourceManager, IconType.UserPresence)
	{
		_userPresenceCstManager = userPresenceCstManager;
	}
}
