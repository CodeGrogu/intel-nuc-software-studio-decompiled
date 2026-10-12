using System.Collections.Generic;
using CustomizationMgr.Enums;
using Newtonsoft.Json;

namespace CustomizationMgr.Models;

[JsonObject]
public class CustomAppTitleSettings
{
	[JsonProperty]
	public List<AppTitleLocation> CustomAppTitleLocation { get; }

	[JsonProperty]
	public Dictionary<string, string> CustomAppTitle { get; }

	[JsonProperty]
	public string DefaultTitleLanguage { get; }

	[JsonConstructor]
	public CustomAppTitleSettings(List<AppTitleLocation> customAppTitleLocation, Dictionary<string, string> customAppTitle, string defaultTitleLanguage)
	{
		CustomAppTitleLocation = customAppTitleLocation;
		CustomAppTitle = customAppTitle;
		DefaultTitleLanguage = defaultTitleLanguage;
	}

	public static CustomAppTitleSettings GetDefaultSettings()
	{
		List<AppTitleLocation> customAppTitleLocation = new List<AppTitleLocation>
		{
			AppTitleLocation.MainTitleBar,
			AppTitleLocation.TrayAppHover,
			AppTitleLocation.TrayAppMenu
		};
		Dictionary<string, string> customAppTitle = new Dictionary<string, string>
		{
			{ "en-US", "My Custom App Name" },
			{ "es-ES", "Mi nombre de aplicación personalizado" }
		};
		string defaultTitleLanguage = "en-US";
		return new CustomAppTitleSettings(customAppTitleLocation, customAppTitle, defaultTitleLanguage);
	}
}
