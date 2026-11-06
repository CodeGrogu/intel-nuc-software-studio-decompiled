using System.Text.Json.Nodes;

namespace NucControl.Core;

public sealed record RgbColor(int R, int G, int B)
{
    public int[] ToArray() => [R, G, B];
}

/// Owns selection, asynchronous reads and the draft together. No UI event can target
/// an old snapshot while a different selected zone is loading.
public sealed class LightingDraft(int selectedZone = 2)
{
    private long generation;
    public int SelectedZone { get; private set; } = selectedZone;
    public LightingState? Snapshot { get; private set; }
    public bool IsLoading { get; private set; }
    public bool IsDirty { get; private set; }
    public int? Brightness { get; private set; }
    public RgbColor? Color { get; private set; }
    public bool CanEdit => !IsLoading && Snapshot?.ZoneId == SelectedZone && Snapshot.Editable;
    public string BrightnessLabel => Brightness is int value ? $"Brightness · {value}%" : "Brightness · unavailable";

    public async Task<bool> LoadAsync(int zone, Func<int, Task<LightingState>> read, bool retainDraft = false)
    {
        if (retainDraft && zone != SelectedZone) throw new InvalidOperationException("Cannot transfer a draft to another zone.");
        var current = ++generation;
        SelectedZone = zone;
        Snapshot = null;
        IsLoading = true;
        if (!retainDraft) { Brightness = null; Color = null; IsDirty = false; }
        try
        {
            var state = await read(zone);
            if (current != generation) return false;
            if (state.ZoneId != zone || state.Fields.ZoneId != zone) throw new BackendException("Lighting response belongs to another zone.");
            Snapshot = state;
            if (!retainDraft) ResetValues(state);
            return true;
        }
        catch
        {
            if (current != generation) return false;
            Snapshot = null;
            throw;
        }
        finally { if (current == generation) IsLoading = false; }
    }
    public void Clear()
    {
        ++generation;
        Snapshot = null; IsLoading = false; IsDirty = false; Brightness = null; Color = null;
    }
    private void ResetValues(LightingState state)
    {
        Brightness = state.Value(0);
        Color = state.Value(3) is int r && state.Value(4) is int g && state.Value(5) is int b ? new(r, g, b) : null;
        IsDirty = false;
    }
    public void Discard()
    {
        if (IsLoading) return;
        if (Snapshot is { } state) ResetValues(state); else Clear();
    }
    public void SetBrightness(int value)
    {
        if (!CanEdit) return;
        if (value is < 0 or > 100) throw new ArgumentOutOfRangeException(nameof(value));
        Brightness = value; IsDirty = true;
    }
    public void SetColor(RgbColor value)
    {
        if (!CanEdit) return;
        if (value.ToArray().Any(v => v is < 0 or > 255)) throw new ArgumentOutOfRangeException(nameof(value));
        Color = value; IsDirty = true;
    }
    public JsonNode CreateMutation()
    {
        if (!CanEdit || !IsDirty || Snapshot is null || Brightness is null || Color is null)
            throw new BackendException("Wait for the selected light to load before applying its draft.");
        var rgb = Color.ToArray();
        var changedRgb = Enumerable.Range(0, 3).Any(i => rgb[i] != Snapshot.Value(i + 3));
        return Wire.Request("Lighting", new { patch = new {
            expected = Snapshot,
            brightness = Brightness == Snapshot.Value(0) ? (int?)null : Brightness,
            rgb = changedRgb ? rgb : null
        } });
    }
}
