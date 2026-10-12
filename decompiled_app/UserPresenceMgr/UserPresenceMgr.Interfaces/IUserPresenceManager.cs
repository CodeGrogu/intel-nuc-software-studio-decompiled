using UserPresenceMgr.Models;

namespace UserPresenceMgr.Interfaces;

public interface IUserPresenceManager
{
	bool ServiceAvailable { get; }

	bool FeatureSupported { get; }

	LockFeatureData LockFeatures { get; set; }

	WakeFeatureData WakeFeatures { get; set; }

	NoLockOnPresenceFeatureData NoLockOnPresenceFeatures { get; set; }
}
