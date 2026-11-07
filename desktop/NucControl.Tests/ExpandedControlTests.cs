using System.Text.Json.Nodes;
using NucControl.Core;
using Xunit;

namespace NucControl.Tests;
public class ExpandedControlTests
{
    [Fact]
    public async Task CustomPowerUsesFirmwareBoundsAndExactPrecondition()
    {
        await using var demo = new DemoBackend(); var session = new ControlSession(demo); await session.RefreshAsync();
        var state = session.State!; var bounds = state.CustomPowerBounds!; var raw = state.PowerReadback!;
        Assert.Equal(16, raw.RawRegister!.Length);
        foreach (var invalid in new[] { new PowerLimits(14,60,32), new(35,60,32), new(30,61,32), new(30,60,33), new(30,20,32) })
            Assert.Throws<BackendException>(() => bounds.CreateMutation(raw, invalid));
        var forged = bounds.CreateMutation(raw with { RawRegister = "0000000000000000" }, new(27,50,24));
        Assert.Equal("Rejected", (await session.ApplyAsync(forged)).Outcome); await session.RefreshAsync();
        var mutation = bounds.CreateMutation(raw, new(27,50,24));
        Assert.Equal("Verified", (await session.ApplyAsync(mutation)).Outcome); await session.RefreshAsync();
        Assert.Equal(new PowerLimits(27,50,24), session.State!.PowerLimits);
        Assert.Equal("Rejected", (await session.ApplyAsync(mutation)).Outcome);
    }
    [Fact]
    public async Task Fixed70RequiresBalancedAndReturnsToEitherStockPreset()
    {
        await using var demo = new DemoBackend(); var session = new ControlSession(demo); await session.RefreshAsync();
        await session.ApplyAsync(Wire.Request("Fan", new { preset = "Cool" })); await session.RefreshAsync();
        Assert.False(session.State!.FixedFan70Available);
        Assert.Equal("Rejected", (await session.ApplyAsync(Wire.Request("FixedFan70", new { }))).Outcome); await session.RefreshAsync();
        await session.ApplyAsync(Wire.Request("Fan", new { preset = "Balanced" })); await session.RefreshAsync();
        await session.ApplyAsync(Wire.Request("FixedFan70", new { })); await session.RefreshAsync();
        Assert.True(session.State!.FixedFan70); Assert.Null(session.State.FanPreset);
        await session.ApplyAsync(Wire.Request("Fan", new { preset = "Cool" })); await session.RefreshAsync();
        Assert.False(session.State!.FixedFan70); Assert.Equal("Cool", session.State.FanPreset);
    }
    [Fact]
    public async Task SavePreviewIncludesAllRolesAndUnknownBrightnessAndAckIsNotVerification()
    {
        await using var demo = new DemoBackend(); var session = new ControlSession(demo); await session.RefreshAsync();
        var snapshot = await session.ReadAsync<LightingSaveState>("GetLightingSaveState", "LightingSaveState");
        Assert.Equal(5, snapshot.Zones.Length); Assert.All(snapshot.Zones, zone => Assert.Equal(5, zone.Fields.Length));
        Assert.Contains("Modern Standby", snapshot.Summary(snapshot.Zones[0])); Assert.Contains("Ethernet", snapshot.Summary(snapshot.Zones[0])); Assert.Contains("unavailable", snapshot.Summary(snapshot.Zones[0]));
        var status = await session.ApplyAsync(Wire.Request("SaveLighting", new { expected = snapshot }));
        Assert.Equal("Acknowledged", status.Outcome); Assert.Null(session.PendingId); Assert.False(session.CanWrite);
        await session.RefreshAsync(); Assert.True(session.CanWrite);
    }
    [Fact]
    public async Task SaveRejectsChangedInactiveStandbyField()
    {
        await using var demo = new DemoBackend(); var session = new ControlSession(demo); await session.RefreshAsync();
        var snapshot = await session.ReadAsync<LightingSaveState>("GetLightingSaveState", "LightingSaveState");
        var draft = new AdvancedLightingDraft();
        await draft.LoadAsync(6,0,(z,m) => session.ReadAsync<LightingModeState>("GetLightingMode","LightingMode", new { zone_id=z, indicator_mode=m }));
        draft.Activate = false; draft.SetValue(AdvancedLighting.Fields(0,true).Single(f => f.Index == 17), 77);
        await session.ApplyAsync(draft.CreateMutation()); await session.RefreshAsync();
        Assert.Equal("Rejected", (await session.ApplyAsync(Wire.Request("SaveLighting", new { expected = snapshot }))).Outcome);
    }
    [Fact]
    public async Task LostSaveAcknowledgementReconcilesWithoutResubmittingSave()
    {
        await using var backend = new LostSaveBackend(); var session = new ControlSession(backend); await session.RefreshAsync();
        var expected = await session.ReadAsync<LightingSaveState>("GetLightingSaveState", "LightingSaveState");
        await Assert.ThrowsAsync<BackendTransportException>(() => session.ApplyAsync(Wire.Request("SaveLighting", new { expected })));
        Assert.NotNull(session.PendingId); await session.RefreshAsync();
        Assert.Null(session.PendingId); Assert.True(session.CanWrite); Assert.Equal(1, backend.SaveCalls); Assert.Contains("acknowledged",session.Message);
    }
    private sealed class LostSaveBackend : IBackend
    {
        private readonly DemoBackend demo = new(); public int SaveCalls { get; private set; }
        public async Task<JsonNode> RequestAsync(JsonNode request, CancellationToken token=default)
        {
            var reply = await demo.RequestAsync(request,token);
            if (request is JsonObject && request["ApplyControl"]?["mutation"]?["SaveLighting"] is not null)
            { SaveCalls++; throw new BackendTransportException(true,new IOException("lost save acknowledgement")); }
            return reply;
        }
        public ValueTask DisposeAsync() => demo.DisposeAsync();
    }
}
