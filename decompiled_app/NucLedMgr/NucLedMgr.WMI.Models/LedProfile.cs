using System.Collections.Generic;
using System.Linq;
using Infrastructure.Models;
using Infrastructure.Models.FeatureMgr;
using Newtonsoft.Json;

namespace NucLedMgr.WMI.Models;

[JsonObject(MemberSerialization.OptIn)]
public class LedProfile : FeatureSettings
{
	public const FeatureType FEATURE_TYPE = FeatureType.ChassisLed;

	[JsonProperty]
	public string Name { get; set; }

	[JsonProperty]
	public List<Led> Leds { get; set; }

	public bool Selected { get; set; }

	public bool TextboxSelected { get; set; }

	public bool InterfacesSet { get; set; }

	[JsonProperty]
	public bool Default { get; set; }

	public LedProfile(LedProfile profile)
	{
		Name = profile.Name;
		Leds = profile.Leds.ToList();
		Selected = profile.Selected;
	}

	public LedProfile(string name, List<Led> leds)
		: base(FeatureType.ChassisLed)
	{
		Name = name;
		Leds = leds;
		Selected = false;
		if (name == LedConstants.DEFAULT)
		{
			Default = true;
		}
		else
		{
			Default = false;
		}
		InterfacesSet = true;
	}

	[JsonConstructor]
	public LedProfile()
	{
		Selected = false;
		InterfacesSet = false;
	}
}
