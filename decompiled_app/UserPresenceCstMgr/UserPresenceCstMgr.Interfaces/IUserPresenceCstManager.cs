using UserPresenceCstMgr.Models;

namespace UserPresenceCstMgr.Interfaces;

public interface IUserPresenceCstManager
{
	bool ServiceAvailable { get; }

	bool FeatureSupported { get; }

	LockFeatureData LockFeatures { get; set; }

	WakeFeatureData WakeFeatures { get; set; }

	NoLockOnPresenceFeatureData NoLockOnPresenceFeatures { get; set; }
}
