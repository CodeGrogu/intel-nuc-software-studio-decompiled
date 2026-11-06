using System.Text.Json;
using System.Text.Json.Nodes;

namespace NucControl.Core;

public sealed record PresetField(int Index, int Value);
public sealed record PresetZone(int ZoneId, int Mode, PresetField[] Fields);
public sealed record LightingPreset(string Name, string Model, string Bios, PresetZone[] Zones, int[] SkippedZones)
{
    public static readonly int[] ZoneIds = [0, 2, 4, 5, 6];
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name) || Name.Length > 64 || Name.Any(char.IsControl)
            || string.IsNullOrWhiteSpace(Model) || string.IsNullOrWhiteSpace(Bios)
            || Zones is null || Zones.Length is < 1 or > 5 || SkippedZones is null
            || Zones.Any(z => z is null) || Zones.Select(z => z.ZoneId).Distinct().Count() != Zones.Length
            || SkippedZones.Distinct().Count() != SkippedZones.Length
            || SkippedZones.Any(z => !ZoneIds.Contains(z) || Zones.Any(v => v.ZoneId == z)))
            throw new BackendException("Invalid lighting preset.");
        foreach (var zone in Zones)
        {
            if (!ZoneIds.Contains(zone.ZoneId) || !AdvancedLighting.Roles.Any(r => r.Id == zone.Mode) || zone.Fields is null)
                throw new BackendException("Preset contains an unsupported zone or function.");
            var definitions = Definitions(zone.Mode).Where(f => Safe(zone.ZoneId, zone.Mode, f.Index)).ToArray();
            if (zone.Fields.Length != definitions.Length || zone.Fields.Where(f => f is not null).Select(f => f.Index).Distinct().Count() != zone.Fields.Length
                || zone.Fields.Any(f => f is null || !definitions.Any(d => d.Index == f.Index && f.Value >= 0 && f.Value <= d.Maximum)))
                throw new BackendException("Preset contains missing, unsafe or invalid lighting fields.");
        }
    }
    private static bool Safe(int zone, int mode, int index) => !(mode == 4 && (index == 0 || zone == 6 && index == 5));
    private static LightingField[] Definitions(int mode) => mode == 0 ? [.. AdvancedLighting.Fields(0), .. AdvancedLighting.Fields(0, true)] : AdvancedLighting.Fields(mode);
    public static async Task<LightingPreset> CaptureAsync(string name, PanelState panel, Func<int, int, Task<LightingModeState>> read)
    {
        var zones = new List<PresetZone>(); var skipped = new List<int>();
        foreach (var zone in ZoneIds.Where(z => (panel.LedZoneMask & (1u << z)) != 0))
        {
            var initial = await read(zone, 0);
            if (initial.ActiveRole == 2) { skipped.Add(zone); continue; }
            var state = initial.ActiveRole == 0 ? initial : await read(zone, initial.ActiveRole);
            if (state.ZoneId != zone || state.ActiveRole != initial.ActiveRole || state.IndicatorMode != initial.ActiveRole || state.Unavailable is not null)
                throw new BackendException("Lighting changed or is unavailable; capture again.");
            var fields = Definitions(state.IndicatorMode).Where(f => Safe(zone, state.IndicatorMode, f.Index))
                .Select(f => new PresetField(f.Index, state.Value(f.Index) ?? throw new BackendException("Cannot save an unreadable lighting field."))).ToArray();
            zones.Add(new(zone, state.IndicatorMode, fields));
        }
        var preset = new LightingPreset(name.Trim(), panel.Model, panel.Bios, zones.ToArray(), skipped.ToArray());
        preset.Validate(); return preset;
    }
    public async Task<JsonNode> CreateMutationAsync(PanelState panel, Func<int, int, Task<LightingModeState>> read)
    {
        Validate();
        if (Model != panel.Model || Bios != panel.Bios || !panel.AdvancedLightingAvailable)
            throw new BackendException("Preset belongs to another device or BIOS.");
        var patches = new JsonArray();
        foreach (var zone in Zones)
        {
            var state = await read(zone.ZoneId, zone.Mode);
            if (state.ZoneId != zone.ZoneId || state.IndicatorMode != zone.Mode || state.Unavailable is not null || state.IndicatorMask != 0x77)
                throw new BackendException("Preset target is unavailable.");
            foreach (var value in zone.Fields)
                if (state.Value(value.Index) is null) throw new BackendException("Preset field is currently unreadable.");
            patches.Add(JsonSerializer.SerializeToNode(new { expected = state, writes = zone.Fields, activate = true }, Wire.Json));
        }
        return new JsonObject { ["LightingPreset"] = new JsonObject { ["patches"] = patches } };
    }
    public string Summary => string.Join("\n\n", Zones.Select(z => $"Zone {z.ZoneId}: {AdvancedLighting.Roles.Single(r => r.Id == z.Mode).Name}\n" + string.Join("\n", z.Fields.Select(v => {
        var definition = Definitions(z.Mode).Single(f => f.Index == v.Index);
        var slot = z.Mode == 0 ? v.Index >= 12 ? "Modern Standby " : "Awake " : "";
        var value = definition.Choices is { } choices ? choices[v.Value] : v.Value.ToString();
        return $"{slot}{definition.Label}: {value}";
    }))))
        + (SkippedZones.Length == 0 ? "" : $"\nEthernet zones left untouched: {string.Join(", ", SkippedZones)}")
        + "\nSoftware brightness and front-right Software blue are preserved, not saved. Inactive function settings are preserved except shared same-zone RGB.";

}

public sealed class LightingPresetStore(string directory)
{
    private sealed record Document(int Version, LightingPreset[] Presets);
    private string FilePath => Path.Combine(directory, "lighting-presets.json");
    public LightingPreset[] Load()
    {
        if (!File.Exists(FilePath)) return [];
        if (new FileInfo(FilePath).Length > 256 * 1024) throw new BackendException("Lighting preset file is too large.");
        var doc = JsonSerializer.Deserialize<Document>(File.ReadAllText(FilePath), Wire.Json);
        if (doc is null || doc.Version != 1 || doc.Presets is null || doc.Presets.Length > 32) throw new BackendException("Unsupported lighting preset file.");
        Validate(doc.Presets); return doc.Presets;
    }
    private static void Validate(LightingPreset[] presets)
    {
        if (presets.Length > 32 || presets.Any(p => p is null) || presets.Select(p => p.Name).Distinct(StringComparer.OrdinalIgnoreCase).Count() != presets.Length)
            throw new BackendException("Preset names must be unique; at most 32 presets are supported.");
        foreach (var preset in presets) preset.Validate();
    }
    public void Save(LightingPreset[] presets)
    {
        Validate(presets);
        var json = JsonSerializer.Serialize(new Document(1, presets), Wire.Json);
        if (System.Text.Encoding.UTF8.GetByteCount(json) > 256 * 1024) throw new BackendException("Lighting preset file is too large.");
        Directory.CreateDirectory(directory); File.WriteAllText(FilePath + ".tmp", json); File.Move(FilePath + ".tmp", FilePath, true);
    }
}
