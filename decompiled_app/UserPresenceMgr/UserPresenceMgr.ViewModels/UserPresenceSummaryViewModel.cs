using CustomizationMgr.Enums;
using CustomizationMgr.Interfaces;
using CustomizationMgr.ViewModels;
using Prism.Regions;
using UserPresenceMgr.Interfaces;
using UserPresenceMgr.Models;

namespace UserPresenceMgr.ViewModels;

public class UserPresenceSummaryViewModel : NavBaseSummaryViewModel
{
	private const string CARD_TITLE_STRING = "UserPresenceTitleString";

	private const string DETAIL_CARD_NAME = "UserPresenceView";

	private const IconType CARD_ICON_TYPE = IconType.UserPresence;

	private readonly IUserPresenceManager _userPresenceManager;

	public bool Summary => true;

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

	public UserPresenceSummaryViewModel(IRegionManager regionMgr, IUserPresenceManager userPresenceManager, ICustomResourceManager resourceManager)
		: base(regionMgr, resourceManager, "UserPresenceTitleString", "UserPresenceView", IconType.UserPresence)
	{
		_userPresenceManager = userPresenceManager;
	}
}
