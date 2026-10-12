using System.Collections.Generic;
using CustomizationMgr.Enums;
using Newtonsoft.Json;

namespace CustomizationMgr.Models;

[JsonObject]
public class CustomAppSettings
{
	[JsonProperty]
	public string DefaultLanguage { get; }

	[JsonProperty]
	public AppLogoType LogoType { get; }

	[JsonProperty]
	public List<object> Settings { get; }

	[JsonConstructor]
	public CustomAppSettings(string defaultLanguage, AppLogoType logoType, List<object> settings)
	{
		DefaultLanguage = defaultLanguage;
		LogoType = logoType;
		Settings = settings;
	}

	public static CustomAppSettings GetDefaultSettings(List<object> DefaultSettings)
	{
		return new CustomAppSettings("en-US", AppLogoType.Default, DefaultSettings);
	}
}
