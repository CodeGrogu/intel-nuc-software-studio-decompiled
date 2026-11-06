using System.Text.Json;
using System.Text.Json.Nodes;

namespace NucControl.Core;

/// Explicitly synthetic state. This backend never constructs a pipe or driver handle.
public sealed class DemoBackend : IBackend
{
    private PanelState state = new("demo", "NUC11PHi7 · Demo", "Synthetic demonstration", true,
        [new("Low power", "LowPower", 25, 60, 32, false), new("Balanced", "Balanced", 30, 60, 32, true), new("Performance", "MaxPerformance", 34, 60, 32, false)],
        new(30, 60, 32), null, "Balanced", null, 0x75, DemoPower(30, 60, 32), new(15, 34, 20, 60, [1,2,3,4,5,6,7,8,10,12,14,16,20,24,28,32]), false, true, true);
    private readonly Dictionary<int, LightingState> lights = new[] { 0, 2, 4, 5, 6 }.ToDictionary(id => id, id =>
        new LightingState(id, id < 4 ? 0 : id - 3, new(id, 0, 0x03f03f, 0x03f03f, true,
            Enumerable.Range(0, 24).Where(i => i < 6 || i is >= 12 and < 18).Select(i => new LedItem(i, 0, i switch { 0 => 50, 2 => 1, 5 => 255, _ => 0 }, null)).ToArray()), null));
    private readonly Dictionary<(int Zone, int Mode), LedFields> modeFields = [];
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
            "GetLightingSaveState" => Reply("LightingSaveState", SaveState()),
            "GetLightingMode" => Reply("LightingMode", ModeState(request[name]!["zone_id"]!.GetValue<int>(), request[name]!["indicator_mode"]!.GetValue<int>())),
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
            state = state with { PowerLimits = new(preset.Pl1Watts, preset.Pl2Watts, preset.TauSeconds), PowerReadback = DemoPower(preset.Pl1Watts, preset.Pl2Watts, preset.TauSeconds) };
        }
        if (mutation["Fan"] is JsonNode fan) state = state with { FanPreset = fan["preset"]!.GetValue<string>(), FixedFan70 = false, FixedFan70Available = fan["preset"]!.GetValue<string>() == "Balanced" };
        if (mutation["FixedFan70"] is not null)
        {
            if (!state.FixedFan70Available) return Finish(new(id, "demo", "Rejected", "Select Balanced before fixed70."));
            state = state with { FanPreset = null, FixedFan70 = true };
        }
        if (mutation["CustomPower"]?["patch"] is JsonNode custom)
        {
            var expected = custom["expected"]!.Deserialize<PowerReadback>(Wire.Json)!;
            var limits = custom["limits"]!.Deserialize<PowerLimits>(Wire.Json)!;
            if (expected != state.PowerReadback || !state.CustomPowerBounds!.Allows(limits)) return Finish(new(id, "demo", "Rejected", "Power changed or limits are outside firmware bounds."));
            state = state with { PowerLimits = limits, PowerReadback = DemoPower(limits.Pl1Watts, limits.Pl2Watts, limits.TauSeconds) };
        }
        if (mutation["SaveLighting"]?["expected"] is JsonNode saved)
        {
            if (!JsonNode.DeepEquals(saved, JsonSerializer.SerializeToNode(SaveState(), Wire.Json))) return Finish(new(id, "demo", "Rejected", "Global lighting changed; review again."));
            return Finish(new(id, "demo", "Acknowledged", "Demo save request acknowledged. No hardware or durable BIOS storage was accessed."));
        }
        if (mutation["AdvancedLighting"]?["patch"] is JsonNode advanced)
        {
            var expected = advanced["expected"]!.Deserialize<LightingModeState>(Wire.Json)!;
            var current = ModeState(expected.ZoneId, expected.IndicatorMode);
            if (!JsonNode.DeepEquals(JsonSerializer.SerializeToNode(current, Wire.Json), advanced["expected"]))
                return Finish(new(id, "demo", "Rejected", "Lighting changed; refresh first."));
            var writes = advanced["writes"]!.AsArray();
            var seen = new HashSet<int>();
            foreach (var write in writes)
            {
                var index = write!["index"]!.GetValue<int>(); var value = write["value"]!.GetValue<int>();
                var field = AdvancedLighting.Fields(current.IndicatorMode, index >= 12).FirstOrDefault(f => f.Index == index);
                if (!seen.Add(index) || field is null || !AdvancedLighting.CanEdit(current, field) || value < 0 || value > field.Maximum)
                    return Finish(new(id, "demo", "Rejected", "Unsupported lighting field."));
            }
            if (current.Fields is { } fields)
            {
                var values = writes.ToDictionary(w => w!["index"]!.GetValue<int>(), w => w!["value"]!.GetValue<int>());
                var updated = fields with { Items = fields.Items.Select(i => values.TryGetValue(i.Index, out var v) ? i with { Value = v } : i).ToArray() };
                if (current.IndicatorMode == 0) lights[current.ZoneId] = lights[current.ZoneId] with { Fields = updated };
                else modeFields[(current.ZoneId, current.IndicatorMode)] = updated;
                var rgbStart = current.IndicatorMode switch { 0 or 4 => 3, 1 => 1, 5 => 2, _ => -1 };
                if (rgbStart >= 0 && values.Keys.Any(i => i >= rgbStart && i < rgbStart + 3))
                    SynchronizeRgb(current.ZoneId, Enumerable.Range(rgbStart, 3).Select(i => updated.Items.Single(v => v.Index == i).Value!.Value).ToArray());
            }
            if (advanced["activate"]!.GetValue<bool>()) lights[current.ZoneId] = lights[current.ZoneId] with { ActiveRole = current.IndicatorMode };
        }
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
            SynchronizeRgb(current.ZoneId, Enumerable.Range(3, 3).Select(i => items.Single(v => v.Index == i).Value!.Value).ToArray());
        }
        var status = new OperationStatus(id, "demo", "Verified", "Demo setting applied. No hardware was accessed.");
        return Finish(status);
    }
    private static PowerReadback DemoPower(int pl1, int pl2, int tau)
    {
        var table = new Dictionary<int,int> { [1]=10,[2]=11,[3]=75,[4]=12,[5]=43,[6]=76,[7]=108,[8]=13,[10]=45,[12]=77,[14]=109,[16]=14,[20]=46,[24]=78,[28]=110,[32]=15 };
        var code = table[tau];
        var raw = (ulong)(uint)(pl1 * 8) | ((ulong)(uint)(pl2 * 8) << 32) | ((ulong)(uint)code << 17) | (1UL << 15) | (1UL << 47);
        return new(pl1 * 8, pl2 * 8, code, tau, raw.ToString("X16"));
    }
    private LightingSaveState SaveState() => new(0x75, new[] { 0,2,4,5,6 }.Select(zone => new LightingSaveZone(zone, lights[zone].ActiveRole, 0x77,
        new[] { 0,1,2,4,5 }.Select(mode => ModeState(zone, mode, true).Fields!).ToArray())).ToArray());
    private LightingModeState ModeState(int zone, int mode, bool forSave = false)
    {
        if (!AdvancedLighting.Roles.Any(r => r.Id == mode) && !(forSave && mode == 2)) throw new BackendException("Unsupported or deferred role");
        var light = lights[zone];
        if (mode == 6) return new(zone, mode, light.ActiveRole, 0x77, null, null);
        if (mode == 0) return new(zone, mode, light.ActiveRole, 0x77, light.Fields, null);
        if (!modeFields.TryGetValue((zone, mode), out var fields))
        {
            var start = mode == 1 ? 1 : mode == 4 ? 3 : 2;
            var mask = mode == 4 ? 63u : mode == 2 ? 30u : 31u;
            fields = new(zone, mode, mask, mode == 4 ? mask & ~1u : mask, mode != 4,
                (mode == 2 ? new LightingField[] { new(1,"Brightness",100),new(2,"Red",255),new(3,"Green",255),new(4,"Blue",255) } : AdvancedLighting.Fields(mode)).Select(f => new LedItem(f.Index, mode == 4 && f.Index == 0 ? 228 : 0,
                    mode == 4 && f.Index == 0 ? null : f.Index >= start && f.Index < start + 3 ? light.Value(f.Index - start + 3)
                    : f.Label.StartsWith("Brightness") ? 50 : 0, null)).ToArray());
            modeFields[(zone, mode)] = fields;
        }
        return new(zone, mode, light.ActiveRole, 0x77, fields, null);
    }
    private void SynchronizeRgb(int zone, int[] rgb)
    {
        var light = lights[zone];
        lights[zone] = light with { Fields = light.Fields with { Items = light.Fields.Items.Select(i => i.Index is >= 3 and <= 5 ? i with { Value = rgb[i.Index - 3] } : i).ToArray() } };
        foreach (var key in modeFields.Keys.Where(k => k.Zone == zone).ToArray())
        {
            var start = key.Mode == 1 ? 1 : key.Mode == 4 ? 3 : 2;
            var fields = modeFields[key];
            modeFields[key] = fields with { Items = fields.Items.Select(i => i.Index >= start && i.Index < start + 3 ? i with { Value = rgb[i.Index - start] } : i).ToArray() };
        }
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
