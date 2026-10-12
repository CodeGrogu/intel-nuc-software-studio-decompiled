using System.Collections.Generic;
using System.Linq;
using Infrastructure.Models;
using Infrastructure.Models.FeatureMgr;
using IntelSdkClient.EneService;
using Newtonsoft.Json;
using NucLedMgr.ENE.Enums;
using NucLedMgr.WMI.Models;

namespace NucLedMgr.ENE.Models;

public class LedProfile : FeatureSettings
{
	public const FeatureType FEATURE_TYPE = FeatureType.ChassisLed;

	[JsonProperty]
	public string Name { get; set; }

	[JsonProperty]
	public List<LedPattern> Leds { get; set; }

	public bool Selected { get; set; }

	public bool TextboxSelected { get; set; }

	public bool InterfacesSet { get; set; }

	public bool Default { get; set; }

	[JsonProperty]
	public Dictionary<EneLedZone, string> ZoneNames { get; set; }

	public LedProfile(LedProfile profile)
	{
		Name = profile.Name;
		Leds = profile.Leds.ToList();
		Selected = profile.Selected;
	}

	public LedProfile(string name, List<LedPattern> leds, Dictionary<EneLedZone, string> zonename = null)
		: base(FeatureType.ChassisLed)
	{
		Name = name;
		Leds = leds;
		Selected = false;
		InterfacesSet = true;
		if (Name == LedConstants.DEFAULT)
		{
			Default = true;
		}
		else
		{
			Default = false;
		}
		ZoneNames = zonename;
	}

	[JsonConstructor]
	public LedProfile()
	{
		Selected = false;
		InterfacesSet = false;
	}
}
