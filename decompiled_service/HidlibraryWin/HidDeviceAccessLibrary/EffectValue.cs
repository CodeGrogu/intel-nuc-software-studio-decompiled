using System.Collections.Generic;
using System.Collections.Immutable;

namespace HidDeviceAccessLibrary;

public static class EffectValue
{
	public static readonly ImmutableDictionary<string, int> EffectSettingDictionary = new Dictionary<string, int>
	{
		{ "Blank", 1 },
		{ "ActiveWaking", 2 },
		{ "ActiveDirection", 3 },
		{ "ActiveEnding", 4 },
		{ "ActiveThinking", 5 },
		{ "ActiveTalking", 6 },
		{ "MicsOffStart", 7 },
		{ "MicsOff", 8 },
		{ "MicsOffEnd", 9 },
		{ "Alert", 10 },
		{ "AlertShort", 11 },
		{ "NtfIncoming", 12 },
		{ "NtfUnheard", 13 },
		{ "DoNotDisturb", 14 },
		{ "BTConnect", 15 },
		{ "BTDisconnect", 16 },
		{ "Error", 17 }
	}.ToImmutableDictionary();
}
