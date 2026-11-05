using System.Text.Json;
using System.Text.Json.Nodes;

namespace NucControl.Core;

/// Explicitly synthetic state. This backend never constructs a pipe or driver handle.
public sealed class DemoBackend : IBackend
{
    private PanelState state = new("demo", "NUC11PHi7 · Demo", "Synthetic demonstration", true,
        [new("Low power", "LowPower", 25, 60, 32, false), new("Balanced", "Balanced", 30, 60, 32, true), new("Performance", "MaxPerformance", 34, 60, 32, false)],
        new(30, 60, 32), null, "Balanced", null, 0x75);
    private readonly Dictionary<int, LightingState> lights = new[] { 0, 2, 4, 5, 6 }.ToDictionary(id => id, id =>
        new LightingState(id, id < 4 ? 0 : id - 3, new(id, 0, 0xffffff, 0xffffff, true,
            Enumerable.Range(0, 24).Select(i => new LedItem(i, 0, i switch { 0 => 50, 2 => 1, 5 => 255, _ => 0 }, null)).ToArray()), null));
    private readonly Dictionary<string, OperationStatus> operations = [];
    private static JsonNode Reply<T>(string name, T body) => Wire.Request(name, body!);
    public Task<JsonNode> RequestAsync(JsonNode request, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var name = request is JsonValue ? request.GetValue<string>() : request.AsObject().First().Key;
        JsonNode result = name switch
        {
            "GetBackendInfo" => Reply("BackendInfo", new BackendInfo("demo", 6)),
            "GetControlPanelState" => Reply("ControlPanelState", state),
            "GetAwakeLighting" => Reply("AwakeLighting", lights[request[name]!["zone_id"]!.GetValue<int>()]),
            "GetTelemetryStatus" => Telemetry(),
            "GetOperationStatus" => Reply("OperationStatus", operations[request[name]!["operation_id"]!.GetValue<string>()]),
            "ApplyControl" => Apply(request[name]!),
            _ => Reply("Error", new { message = "Unsupported demo request" })
        };
        return Task.FromResult(result);
    }
    private static JsonNode Telemetry()
    {
        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        Reading R(double value) => new(value, "Synthetic demo", now, null);
        return Reply("TelemetryStatus", new TelemetryStatus(R(47), R(42), R(2120), R(2300)));
    }
    private JsonNode Apply(JsonNode body)
    {
        var id = body["operation_id"]!.GetValue<string>();
        if (operations.TryGetValue(id, out var old)) return Reply("OperationStatus", old);
        var mutation = body["mutation"]!;
        if (mutation["Power"] is JsonNode power)
        {
            var preset = state.PowerPresets.Single(p => p.PresetKind == power["preset"]!.GetValue<string>());
            state = state with { PowerLimits = new(preset.Pl1Watts, preset.Pl2Watts, preset.TauSeconds) };
        }
        if (mutation["Fan"] is JsonNode fan) state = state with { FanPreset = fan["preset"]!.GetValue<string>() };
        if (mutation["Lighting"]?["patch"] is JsonNode patch)
        {
            var expected = patch["expected"]!.Deserialize<LightingState>(Wire.Json)!;
            var current = lights[expected.ZoneId];
            if (!JsonNode.DeepEquals(JsonSerializer.SerializeToNode(current, Wire.Json), patch["expected"]))
                return Reply("OperationStatus", new OperationStatus(id, "demo", "Rejected", "Lighting changed; refresh first."));
            var items = current.Fields.Items.Select(i => i with
            {
                Value = i.Index == 0 && patch["brightness"] is JsonNode b ? b.GetValue<int>()
                : i.Index is >= 3 and <= 5 && patch["rgb"] is JsonArray rgb ? rgb[i.Index - 3]!.GetValue<int>() : i.Value
            }).ToArray();
            lights[current.ZoneId] = current with { ActiveRole = 0, Fields = current.Fields with { Items = items } };
        }
        var status = new OperationStatus(id, "demo", "Verified", "Demo setting applied. No hardware was accessed.");
        operations[id] = status;
        return Reply("OperationStatus", status);
    }
    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
