using MobileKeyboardLedMgr.Storage.Data;

namespace MobileKeyboardLedMgr.Interfaces;

public interface ISettingsFileManager
{
	KeyboardLedCustomSettings CustomDefaultSettings { get; }
}
