using AlexaLightBarMgr.Storage.Data;

namespace AlexaLightBarMgr.Interfaces;

public interface IAlexaSettingsFileManager
{
	AlexaSettingsData ReadSavedAlexaSettings();

	void SaveAlexaSettings(AlexaSettingsData alexaSettingsData);
}
