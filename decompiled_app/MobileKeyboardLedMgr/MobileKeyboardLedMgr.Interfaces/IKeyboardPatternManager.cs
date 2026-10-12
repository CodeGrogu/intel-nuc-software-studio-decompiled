using System.Collections.Generic;
using MobileKeyboardLedMgr.Models;
using MobileKeyboardLedMgr.Models.KingCounty;
using MobileKeyboardLedMgr.Models.Profiles;
using UniwillSdkClient.UniwillService;

namespace MobileKeyboardLedMgr.Interfaces;

public interface IKeyboardPatternManager
{
	RGBKeyboardCapabilities KeyboardType { get; }

	int AcBrightness { get; set; }

	int DcBrightness { get; set; }

	int Speed { get; set; }

	PresetPatternsKC PresetPatterns { get; }

	bool On { get; set; }

	bool ErrorMessage { get; set; }

	bool ProjectAuroraEnabled { get; set; }

	bool ProjectAuroraInstalled { get; }

	KeyboardUiData KeyboardUiData { get; }

	List<KeyboardPattern> DefaultPatterns { get; }

	KeyboardPattern SelectedPattern { get; set; }

	void ApplyPattern(string Name);

	void ApplySelectedPattern();

	void ApplyBrightness(bool isAcBrightness);

	void EnableLeds(bool Enable);

	void UpdateIdleTimer(int tickValue, bool isAc);
}
