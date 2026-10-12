using System.Collections.Generic;

namespace MobileKeyboardLedMgr.Models.Profiles;

public class SerializableKeyboardProfile
{
	public string Name { get; set; }

	public int AcIdleTimer { get; set; }

	public int DcIdleTimer { get; set; }

	public bool IsEnabled { get; set; } = true;

	public SerializableKeyboardPattern Pattern { get; set; }

	public SerializableKeyboardProfile(KeyboardProfile profile)
	{
		if (profile != null)
		{
			Name = profile.Name;
			AcIdleTimer = profile.AcIdleTimer;
			DcIdleTimer = profile.DcIdleTimer;
			IsEnabled = profile.IsEnabled;
			Pattern = new SerializableKeyboardPattern(profile.Pattern);
		}
	}

	public override bool Equals(object obj)
	{
		if (obj is SerializableKeyboardProfile serializableKeyboardProfile && Name == serializableKeyboardProfile.Name && AcIdleTimer == serializableKeyboardProfile.AcIdleTimer && DcIdleTimer == serializableKeyboardProfile.DcIdleTimer && IsEnabled == serializableKeyboardProfile.IsEnabled)
		{
			return Pattern == serializableKeyboardProfile.Pattern;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return ((((unchecked(-1046661295 * -1521134295) + EqualityComparer<string>.Default.GetHashCode(Name)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(AcIdleTimer)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(DcIdleTimer)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(IsEnabled)) * -1521134295 + EqualityComparer<SerializableKeyboardPattern>.Default.GetHashCode(Pattern);
	}
}
