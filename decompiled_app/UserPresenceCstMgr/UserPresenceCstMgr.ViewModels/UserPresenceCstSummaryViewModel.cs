using CustomizationMgr.Enums;
using CustomizationMgr.Interfaces;
using CustomizationMgr.ViewModels;
using Prism.Regions;
using UserPresenceCstMgr.Interfaces;
using UserPresenceCstMgr.Models;

namespace UserPresenceCstMgr.ViewModels;

public class UserPresenceCstSummaryViewModel : NavBaseSummaryViewModel
{
	private const string CARD_TITLE_STRING = "UserPresenceCstTitleString";

	private const string DETAIL_CARD_NAME = "UserPresenceCstView";

	private const IconType CARD_ICON_TYPE = IconType.UserPresence;

	private readonly IUserPresenceCstManager _userPresenceCstManager;

	public bool Summary => true;

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

	public UserPresenceCstSummaryViewModel(IRegionManager regionMgr, IUserPresenceCstManager userPresenceCstManager, ICustomResourceManager resourceManager)
		: base(regionMgr, resourceManager, "UserPresenceCstTitleString", "UserPresenceCstView", IconType.UserPresence)
	{
		_userPresenceCstManager = userPresenceCstManager;
	}
}
