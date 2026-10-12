using System.Linq;
using Newtonsoft.Json;
using UniwillSdkClient.UniwillService;

namespace MobileDisplayModeMgr.Models;

[JsonObject(MemberSerialization.OptIn, Title = "MobileDisplayModeSettings")]
public class MobileDisplayModeCustomSettings
{
	private static readonly DisplayMode KING_COUNTY_STANDARD_DEFAULT = new DisplayMode
	{
		Type = DisplayModeType.Standard,
		Index = 0,
		Red = 128,
		Green = 128,
		Blue = 128,
		BrightnessAC = 50,
		BrightnessDC = 50,
		ColorTemp = 6500,
		Contrast = 50f
	};

	private static readonly DisplayMode KING_COUNTY_GAMING_DEFAULT = new DisplayMode
	{
		Type = DisplayModeType.Gaming,
		Index = 1,
		Red = 100,
		Green = 90,
		Blue = 110,
		BrightnessAC = 50,
		BrightnessDC = 50,
		ColorTemp = 6500,
		Contrast = 60f
	};

	private static readonly DisplayMode KING_COUNTY_VIDEO_DEFAULT = new DisplayMode
	{
		Type = DisplayModeType.Video,
		Index = 2,
		Red = 111,
		Green = 120,
		Blue = 115,
		BrightnessAC = 50,
		BrightnessDC = 50,
		ColorTemp = 6500,
		Contrast = 50f
	};

	private static readonly DisplayMode KING_COUNTY_READING_DEFAULT = new DisplayMode
	{
		Type = DisplayModeType.Reading,
		Index = 3,
		Red = 128,
		Green = 128,
		Blue = 128,
		BrightnessAC = 50,
		BrightnessDC = 50,
		ColorTemp = 5000,
		Contrast = 50f
	};

	private static readonly DisplayMode KING_COUNTY_CUSTOM_DEFAULT = new DisplayMode
	{
		Type = DisplayModeType.Custom,
		Index = 4,
		Red = 128,
		Green = 128,
		Blue = 128,
		BrightnessAC = 50,
		BrightnessDC = 50,
		ColorTemp = 6500,
		Contrast = 50f
	};

	private const int KING_COUNTY_DEFAULT_CURRENT_DISPLAY_MODE_INDEX = 0;

	[JsonProperty]
	public DisplayMode Standard { get; set; }

	[JsonProperty]
	public DisplayMode Gaming { get; set; }

	[JsonProperty]
	public DisplayMode Video { get; set; }

	[JsonProperty]
	public DisplayMode Reading { get; set; }

	[JsonProperty]
	public DisplayMode Custom { get; set; }

	[JsonProperty]
	public int CurrentDisplayModeIndex { get; set; }

	[JsonConstructor]
	public MobileDisplayModeCustomSettings()
	{
	}

	public MobileDisplayModeCustomSettings(DisplayMode[] displayModes, int currentDisplayModeIndex)
	{
		CurrentDisplayModeIndex = currentDisplayModeIndex;
		Standard = displayModes.FirstOrDefault((DisplayMode x) => x.Type == DisplayModeType.Standard);
		Gaming = displayModes.FirstOrDefault((DisplayMode x) => x.Type == DisplayModeType.Gaming);
		Video = displayModes.FirstOrDefault((DisplayMode x) => x.Type == DisplayModeType.Video);
		Reading = displayModes.FirstOrDefault((DisplayMode x) => x.Type == DisplayModeType.Reading);
		Custom = displayModes.FirstOrDefault((DisplayMode x) => x.Type == DisplayModeType.Custom);
	}

	public static MobileDisplayModeCustomSettings GetDefaultSettingsKingCounty()
	{
		return new MobileDisplayModeCustomSettings(new DisplayMode[5] { KING_COUNTY_STANDARD_DEFAULT, KING_COUNTY_GAMING_DEFAULT, KING_COUNTY_VIDEO_DEFAULT, KING_COUNTY_READING_DEFAULT, KING_COUNTY_CUSTOM_DEFAULT }, 0);
	}

	public DisplayMode[] GetCustomDisplayModes()
	{
		return new DisplayMode[5] { Standard, Gaming, Video, Reading, Custom };
	}
}
