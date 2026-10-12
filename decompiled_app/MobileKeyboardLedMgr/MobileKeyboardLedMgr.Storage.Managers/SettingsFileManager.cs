using CustomizationMgr.Interfaces;
using MobileKeyboardLedMgr.Interfaces;
using MobileKeyboardLedMgr.Storage.Data;

namespace MobileKeyboardLedMgr.Storage.Managers;

internal class SettingsFileManager : ISettingsFileManager
{
	public KeyboardLedCustomSettings CustomDefaultSettings { get; private set; }

	public SettingsFileManager(ICustomResourceManager customResourceManager)
	{
		LoadCustomDefaultSettings(customResourceManager);
	}

	private void LoadCustomDefaultSettings(ICustomResourceManager customResourceManager)
	{
		if (customResourceManager.CustomAppSettings?.Settings?.Find((object x) => x.GetType().Equals(typeof(KeyboardLedCustomSettings))) is KeyboardLedCustomSettings customDefaultSettings)
		{
			CustomDefaultSettings = customDefaultSettings;
		}
	}
}
