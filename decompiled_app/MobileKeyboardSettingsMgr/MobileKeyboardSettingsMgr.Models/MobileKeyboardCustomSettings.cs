using Newtonsoft.Json;

namespace MobileKeyboardSettingsMgr.Models;

[JsonObject(Title = "MobileKeyboardSettings")]
public class MobileKeyboardCustomSettings
{
	private const bool KING_COUNTY_DEFAULT_FNKEY = false;

	private const bool KING_COUNTY_DEFAULT_NUMPAD = false;

	private const bool KING_COUNTY_DEFAULT_WINKEY = false;

	private const bool KING_COUNTY_DEFAULT_TOUCHPAD_KEY_DISABLED = false;

	private const bool KING_COUNTY_DEFAULT_ONSCREEN_DISPLAY = false;

	[JsonProperty]
	public bool FnKey { get; set; }

	[JsonProperty]
	public bool NumPad { get; set; }

	[JsonProperty]
	public bool WinKey { get; set; }

	[JsonProperty]
	public bool TouchPadKeyDisabled { get; set; }

	[JsonProperty]
	public bool OnScreenDisplay { get; set; }

	[JsonConstructor]
	public MobileKeyboardCustomSettings(bool fnKey, bool numPad, bool winKey, bool touchPadKeyDisabled, bool onScreenDisplay)
	{
		FnKey = fnKey;
		NumPad = numPad;
		WinKey = winKey;
		TouchPadKeyDisabled = touchPadKeyDisabled;
		OnScreenDisplay = onScreenDisplay;
	}

	public static MobileKeyboardCustomSettings GetDefaultSettingKingCounty()
	{
		return new MobileKeyboardCustomSettings(fnKey: false, numPad: false, winKey: false, touchPadKeyDisabled: false, onScreenDisplay: false);
	}
}
