using System.Text.Json;
using System.Text.Json.Nodes;

namespace NucControl.Core;

/// Explicitly synthetic state. This backend never constructs a pipe or driver handle.
public sealed class DemoBackend : IBackend
{
    private PanelState state = new("demo", "NUC11PHi7 · Demo", "Synthetic demonstration", true,
        [new("Low power", "LowPower", 25, 60, 32, false), new("Balanced", "Balanced", 30, 60, 32, true), new("Performance", "MaxPerformance", 34, 60, 32, false)],
        new(30, 60, 32), null, "Balanced", null, 0x75, new(240, 480, 15, 32));
    private readonly Dictionary<int, LightingState> lights = new[] { 0, 2, 4, 5, 6 }.ToDictionary(id => id, id =>
        new LightingState(id, id < 4 ? 0 : id - 3, new(id, 0, 0xffffff, 0xffffff, true,
            Enumerable.Range(0, 24).Select(i => new LedItem(i, 0, i switch { 0 => 50, 2 => 1, 5 => 255, _ => 0 }, null)).ToArray()), null));
    private readonly Dictionary<string, OperationStatus> operations = [];
    private readonly Dictionary<string, string> fingerprints = [];
    private ulong epoch = 1;
    private static JsonNode Reply<T>(string name, T body) => Wire.Request(name, body!);
    public Task<JsonNode> RequestAsync(JsonNode request, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var name = request is JsonValue ? request.GetValue<string>() : request.AsObject().First().Key;
        JsonNode result = name switch
        {
            "GetBackendInfo" => Reply("BackendInfo", new BackendInfo("demo", 6)),
            "GetControlSessionInfo" => Reply("ControlSessionInfo", new ControlSessionInfo("demo", epoch)),
            "ReconcileOperation" => Reconcile(request[name]!),
            "GetControlPanelState" => Reply("ControlPanelState", state),
            "GetAwakeLighting" => Reply("AwakeLighting", lights[request[name]!["zone_id"]!.GetValue<int>()]),
            "GetTelemetryStatus" => Telemetry(),
            "GetOperationStatus" => Reply("OperationStatus", operations.GetValueOrDefault(request[name]!["operation_id"]!.GetValue<string>()) ?? new OperationStatus(request[name]!["operation_id"]!.GetValue<string>(), "demo", "Uncertain", "Unknown operation; reconcile before continuing.")),
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
        var mutation = body["mutation"]!;
        var fingerprint = mutation.ToJsonString();
        if (body["backend_instance"]?.GetValue<string>() != "demo")
            return Reply("OperationStatus", new OperationStatus(id, "demo", "Rejected", "Service instance changed."));
        if (operations.TryGetValue(id, out var old)) return fingerprints[id] == fingerprint
            ? Reply("OperationStatus", old) : Reply("Error", new { message = "Operation ID reused with a different command" });
        if (body["admission_epoch"]?.GetValue<ulong>() != epoch || epoch == ulong.MaxValue)
            return Reply("OperationStatus", new OperationStatus(id, "demo", "Rejected", "Admission epoch closed."));
        fingerprints[id] = fingerprint;
        if (mutation["Power"] is JsonNode power)
        {
            var preset = state.PowerPresets.Single(p => p.PresetKind == power["preset"]!.GetValue<string>());
            state = state with { PowerLimits = new(preset.Pl1Watts, preset.Pl2Watts, preset.TauSeconds), PowerReadback = new(preset.Pl1Watts * 8, preset.Pl2Watts * 8, 15, preset.TauSeconds) };
        }
        if (mutation["Fan"] is JsonNode fan) state = state with { FanPreset = fan["preset"]!.GetValue<string>() };
        if (mutation["Lighting"]?["patch"] is JsonNode patch)
        {
            var expected = patch["expected"]!.Deserialize<LightingState>(Wire.Json)!;
            var current = lights[expected.ZoneId];
            if (!JsonNode.DeepEquals(JsonSerializer.SerializeToNode(current, Wire.Json), patch["expected"]))
                return Finish(new OperationStatus(id, "demo", "Rejected", "Lighting changed; refresh first."));
            var items = current.Fields.Items.Select(i => i with
            {
                Value = i.Index == 0 && patch["brightness"] is JsonNode b ? b.GetValue<int>()
                : i.Index is >= 3 and <= 5 && patch["rgb"] is JsonArray rgb ? rgb[i.Index - 3]!.GetValue<int>() : i.Value
            }).ToArray();
            lights[current.ZoneId] = current with { ActiveRole = 0, Fields = current.Fields with { Items = items } };
        }
        var status = new OperationStatus(id, "demo", "Verified", "Demo setting applied. No hardware was accessed.");
        return Finish(status);
    }
    private JsonNode Finish(OperationStatus status)
    {
        operations[status.OperationId] = status;
        if (operations.Count >= 128)
        {
            var first = operations.Keys.First(); operations.Remove(first); fingerprints.Remove(first);
            if (epoch < ulong.MaxValue) epoch++;
        }
        return Reply("OperationStatus", status);
    }
    private JsonNode Reconcile(JsonNode body)
    {
        var id = body["operation_id"]!.GetValue<string>();
        var expectedEpoch = body["admission_epoch"]!.GetValue<ulong>();
        if (body["backend_instance"]!.GetValue<string>() != "demo")
            return Reply("OperationStatus", new OperationStatus(id, "demo", "Uncertain", "Old service instance cannot execute; refresh actual state."));
        if (expectedEpoch == 0 || expectedEpoch > epoch) return Reply("Error", new { message = "Invalid admission epoch" });
        if (operations.TryGetValue(id, out var known)) return Reply("OperationStatus", known);
        if (expectedEpoch == epoch && epoch < ulong.MaxValue) epoch++;
        return Reply("OperationStatus", new OperationStatus(id, "demo", "Uncertain", "Outcome unavailable; old epoch fenced. Refresh actual state."));
    }
    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
