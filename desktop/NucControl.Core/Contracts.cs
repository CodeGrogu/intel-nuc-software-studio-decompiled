using System.Text.Json;
using System.Text.Json.Nodes;

namespace NucControl.Core;

public static class Wire
{
    public const int MaxFrame = 256 * 1024;
    public static readonly JsonSerializerOptions Json = new() { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower };
    public static JsonNode Request(string name, object body) => new JsonObject { [name] = JsonSerializer.SerializeToNode(body, Json) };
    public static T Decode<T>(JsonNode response, string name)
    {
        if (response["Error"] is JsonNode error) throw new BackendException(error["message"]?.GetValue<string>() ?? "Backend error");
        var node = response[name] ?? throw new BackendException($"Missing {name} response");
        var value = node.Deserialize<T>(Json);
        return value is null ? throw new BackendException($"Empty {name} response") : value;
    }
}
public sealed class BackendException(string message) : Exception(message);
public sealed record BackendInfo(string Version, int ProtocolRevision);
public sealed record PowerLimits(int Pl1Watts, int Pl2Watts, int TauSeconds);
public sealed record PowerPreset(string Name, string PresetKind, int Pl1Watts, int Pl2Watts, int TauSeconds, bool IsBiosDefault);
public sealed record PanelState(string BackendInstance, string Model, string Bios, bool DeviceEligible,
    PowerPreset[] PowerPresets, PowerLimits? PowerLimits, string? PowerUnavailable,
    string? FanPreset, string? FanUnavailable, uint LedZoneMask);
public sealed record Reading(double? Value, string Source, long TimestampMs, string? Error)
{
    public string Display(string unit, long now) => Value is null ? "Unavailable" : now - TimestampMs > 6_000 ? "Stale" : $"{Value:0}{unit}";
}
public sealed record TelemetryStatus(Reading CpuTemperature, Reading GpuTemperature, Reading CpuFan, Reading GpuFan);
public sealed record LedItem(int Index, int? FirmwareStatus, int? Value, string? Error);
public sealed record LedFields(int ZoneId, int IndicatorMode, uint AdvertisedItemsMask, uint SuccessfullyReadItemsMask, bool Complete, LedItem[] Items);
public sealed record LightingState(int ZoneId, int ActiveRole, LedFields Fields, string? Unavailable)
{
    public int? Value(int index) => Fields.Items.FirstOrDefault(i => i.Index == index && i.FirmwareStatus == 0)?.Value;
    public bool Editable => Unavailable is null && Enumerable.Range(0, 6).All(i => Value(i) is not null) && Value(1) == 0;
}
public sealed record OperationStatus(string OperationId, string BackendInstance, string Outcome, string Message);
public sealed record Preferences(bool LaunchAtStartup = false, int SelectedZone = 2);

public interface IBackend : IAsyncDisposable
{
    Task<JsonNode> RequestAsync(JsonNode request, CancellationToken cancellationToken = default);
}
