using System.Text.Json.Nodes;

namespace NucControl.Core;

public sealed record LightingModeState(int ZoneId, int IndicatorMode, int ActiveRole, uint IndicatorMask, LedFields? Fields, string? Unavailable)
{
    public int? Value(int index) => Fields is null ? null : new LightingState(ZoneId, ActiveRole, Fields, Unavailable).Value(index);
}
public sealed record LightingField(int Index, string Label, int Maximum, string[]? Choices = null);

public static class AdvancedLighting
{
    public static readonly (int Id, string Name)[] Roles = [(0, "Power state"), (1, "Disk activity"), (4, "Software"), (5, "Power limit"), (6, "Disabled")];
    private static readonly string[] Effects = ["Solid", "Breathing", "Pulsing", "Strobing"];
    public static LightingField[] Fields(int mode, bool standby = false)
    {
        var offset = standby ? 12 : 0;
        return mode switch
        {
            0 => [new(offset, "Brightness (%)", 100), new(offset + 1, "Effect", 3, Effects), new(offset + 2, "Frequency setting", 10), new(offset + 3, "Red", 255), new(offset + 4, "Green", 255), new(offset + 5, "Blue", 255)],
            1 => [new(0, "Brightness (%)", 100), new(1, "Red", 255), new(2, "Green", 255), new(3, "Blue", 255), new(4, "Activity", 1, ["Off to on", "On to off"])],
            4 => [new(0, "Brightness (%)", 100), new(1, "Effect", 3, Effects), new(2, "Frequency setting", 10), new(3, "Red", 255), new(4, "Green", 255), new(5, "Blue", 255)],
            5 => [new(0, "Indication", 1, ["Green to red", "Custom color"]), new(1, "Brightness (%)", 100), new(2, "Red", 255), new(3, "Green", 255), new(4, "Blue", 255)],
            _ => []
        };
    }
    public static bool CanEdit(LightingModeState state, LightingField field) => state.Unavailable is null
        && state.Value(field.Index) is not null && !(state.IndicatorMode == 4 && (field.Index == 0 || state.ZoneId == 6 && field.Index == 5));
    public static string Notice(int mode) => mode switch
    {
        0 => "Modern Standby is the firmware's Ready Mode. Frequency uses the vendor's 0-10 scale; no physical Hz conversion is assumed.",
        1 => "Disk activity controls the selected light's activity indication.",
        4 => "Software brightness is unavailable in this BIOS. Front-right blue cannot be written safely in Software mode.",
        5 => "Configuration can be read back. The automatic power-limit color trigger has not been verified on this device.",
        6 => "Disable the selected light without overwriting its stored colors or effects.",
        _ => "Unsupported function"
    };
}

public sealed class AdvancedLightingDraft
{
    private long generation;
    private readonly Dictionary<int, int> changes = [];
    public LightingModeState? Snapshot { get; private set; }
    public int SelectedZone { get; private set; } = -1;
    public int SelectedMode { get; private set; } = -1;
    public bool IsLoading { get; private set; }
    public bool Activate { get; set; } = true;
    public bool IsDirty => changes.Count != 0;
    public bool CanApply => !IsLoading && Snapshot is { Unavailable: null } state
        && changes.All(v => AdvancedLighting.Fields(state.IndicatorMode, v.Key >= 12).FirstOrDefault(f => f.Index == v.Key) is { } definition && AdvancedLighting.CanEdit(state, definition))
        && (changes.Count != 0 || Activate && state.ActiveRole != state.IndicatorMode);
    public int? Value(int index) => changes.TryGetValue(index, out var value) ? value : Snapshot?.Value(index);
    public async Task<bool> LoadAsync(int zone, int mode, Func<int, int, Task<LightingModeState>> read, bool retainChanges = false)
    {
        if (retainChanges && SelectedZone >= 0 && (SelectedZone != zone || SelectedMode != mode)) throw new BackendException("Cannot move an advanced draft to another target.");
        SelectedZone = zone; SelectedMode = mode;
        var current = ++generation; Snapshot = null; if (!retainChanges) changes.Clear(); IsLoading = true;
        try
        {
            var state = await read(zone, mode);
            if (current != generation) return false;
            if (state.ZoneId != zone || state.IndicatorMode != mode || state.Fields is { } f && (f.ZoneId != zone || f.IndicatorMode != mode))
                throw new BackendException("Lighting response does not match the selected zone and function.");
            Snapshot = state;
            foreach (var index in changes.Keys.Where(i => state.Value(i) == changes[i]).ToArray()) changes.Remove(index);
            return true;
        }
        catch { if (current != generation) return false; throw; }
        finally { if (current == generation) IsLoading = false; }
    }
    public void SetValue(LightingField field, int value)
    {
        if (IsLoading || Snapshot is not { } state || !AdvancedLighting.CanEdit(state, field)) throw new BackendException("This lighting field is unavailable.");
        var known = AdvancedLighting.Fields(state.IndicatorMode, field.Index >= 12).FirstOrDefault(f => f.Index == field.Index);
        if (known is null || value < 0 || value > known.Maximum) throw new BackendException("Lighting value is outside the supported range.");
        if (state.Value(field.Index) == value) changes.Remove(field.Index); else changes[field.Index] = value;
    }
    public void Discard() { changes.Clear(); Activate = false; }
    public JsonNode CreateMutation()
    {
        if (!CanApply) throw new BackendException("Read the selected light before applying changes.");
        return Wire.Request("AdvancedLighting", new { patch = new { expected = Snapshot,
            writes = changes.OrderBy(v => v.Key).Select(v => new { index = v.Key, value = v.Value }).ToArray(), activate = Activate } });
    }
}
