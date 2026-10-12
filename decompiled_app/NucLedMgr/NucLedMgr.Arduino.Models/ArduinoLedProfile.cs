using System.Collections.Generic;
using System.Linq;
using Infrastructure.Models;
using Infrastructure.Models.FeatureMgr;
using IntelSdkClient.ArduinoService;
using Newtonsoft.Json;

namespace NucLedMgr.Arduino.Models;

[JsonObject(MemberSerialization.OptIn)]
internal class ArduinoLedProfile : FeatureSettings
{
	public const FeatureType FEATURE_TYPE = FeatureType.ChassisLed;

	[JsonProperty]
	public string Name { get; set; }

	[JsonProperty]
	public List<LedPattern> Leds { get; set; }

	public bool Selected { get; set; }

	public bool InterfacesSet { get; set; }

	public ArduinoLedProfile(ArduinoLedProfile profile)
	{
		Name = profile.Name;
		Leds = profile.Leds.ToList();
		Selected = profile.Selected;
	}

	public ArduinoLedProfile(string name, List<LedPattern> leds)
		: base(FeatureType.ChassisLed)
	{
		Name = name;
		Leds = leds;
		Selected = false;
		InterfacesSet = true;
	}

	[JsonConstructor]
	public ArduinoLedProfile()
	{
		Selected = false;
		InterfacesSet = false;
	}
}
