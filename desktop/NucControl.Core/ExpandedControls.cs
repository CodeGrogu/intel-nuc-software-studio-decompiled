using System.Text.Json.Nodes;

namespace NucControl.Core;
public sealed record CustomPowerBounds(int MinimumPl1Watts, int MaximumPl1Watts, int MinimumPl2Watts, int MaximumPl2Watts, int[] TauSeconds)
{
    public bool Allows(PowerLimits limits) => limits.Pl1Watts >= MinimumPl1Watts && limits.Pl1Watts <= MaximumPl1Watts
        && limits.Pl2Watts >= MinimumPl2Watts && limits.Pl2Watts <= MaximumPl2Watts && limits.Pl2Watts >= limits.Pl1Watts && TauSeconds.Contains(limits.TauSeconds);
    public JsonNode CreateMutation(PowerReadback expected, PowerLimits limits)
    {
        if (expected.RawRegister is null || expected.RawRegister.Length != 16 || !expected.RawRegister.All(Uri.IsHexDigit) || !Allows(limits)) throw new BackendException("Read exact power state and choose values within the firmware limits.");
        return Wire.Request("CustomPower", new { patch = new { expected, limits } });
    }
}
public sealed record LightingSaveZone(int ZoneId, int ActiveRole, uint IndicatorMask, LedFields[] Fields);
public sealed record LightingSaveState(uint ZoneMask, LightingSaveZone[] Zones)
{
    public string Summary(LightingSaveZone zone)
    {
        string Value(LedFields fields, int i) => new LightingState(zone.ZoneId, zone.ActiveRole, fields, null).Value(i)?.ToString() ?? "unavailable";
        var lines = new List<string> { $"Current function: {RoleName(zone.ActiveRole)}" };
        foreach (var mode in zone.Fields)
        {
            if (mode.IndicatorMode == 0)
            {
                foreach (var (offset, name) in new[] { (0, "Awake"), (12, "Modern Standby") })
                    lines.Add($"{name}: brightness {Value(mode, offset)}%, effect {Effect(Value(mode, offset + 1))}, frequency {Value(mode, offset + 2)}, RGB {Value(mode, offset + 3)}/{Value(mode, offset + 4)}/{Value(mode, offset + 5)}");
            }
            else
            {
                var definitions = mode.IndicatorMode == 2 ? new LightingField[] { new(1,"Brightness (%)",100),new(2,"Red",255),new(3,"Green",255),new(4,"Blue",255) } : AdvancedLighting.Fields(mode.IndicatorMode);
                lines.Add($"{RoleName(mode.IndicatorMode)}: " + string.Join(", ", definitions.Select(f => $"{f.Label} {Value(mode, f.Index)}")));
            }
        }
        return string.Join("\n", lines);
    }
    private static string Effect(string value) => int.TryParse(value, out var index) && index is >= 0 and <= 3 ? new[] { "Solid", "Breathing", "Pulsing", "Strobing" }[index] : value;
    public static string RoleName(int mode) => mode == 2 ? "Ethernet (existing settings)" : AdvancedLighting.Roles.FirstOrDefault(r => r.Id == mode).Name ?? "Unknown";
}
