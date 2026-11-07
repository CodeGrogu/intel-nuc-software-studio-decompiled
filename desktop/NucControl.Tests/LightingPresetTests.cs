using NucControl.Core;
using System.Text.Json.Nodes;
using Xunit;
namespace NucControl.Tests;
public class LightingPresetTests
{
    private static Task<LightingModeState> Read(ControlSession session, int zone, int mode) => session.ReadAsync<LightingModeState>("GetLightingMode", "LightingMode", new { zone_id = zone, indicator_mode = mode });
    [Fact]
    public async Task CapturePersistsIntentAndExplicitApplyUsesFreshPreconditions()
    {
        await using var demo = new DemoBackend(); var session = new ControlSession(demo); await session.RefreshAsync();
        var preset = await LightingPreset.CaptureAsync("Evening", session.State!, (z,m) => Read(session,z,m));
        Assert.Equal(4, preset.Zones.Length); Assert.Contains("Modern Standby", preset.Summary); Assert.Contains("Brightness", preset.Summary);
        var directory = Path.Combine(Path.GetTempPath(), "nuc-presets-" + Guid.NewGuid());
        try
        {
            var store = new LightingPresetStore(directory); store.Save([preset]); var saved = Assert.Single(store.Load());
            var draft = new AdvancedLightingDraft(); await draft.LoadAsync(0,0,(z,m) => Read(session,z,m)); draft.SetValue(AdvancedLighting.Fields(0)[0], 19);
            await session.ApplyAsync(draft.CreateMutation()); await session.RefreshAsync();
            Assert.Equal(19, (await Read(session,0,0)).Value(0)); // saving/loading never applies
            Assert.Equal("Verified", (await session.ApplyAsync(await saved.CreateMutationAsync(session.State!, (z,m) => Read(session,z,m)))).Outcome);
            Assert.Equal(preset.Zones[0].Fields.Single(f => f.Index == 0).Value, (await Read(session,0,0)).Value(0));
            Assert.Throws<BackendException>(() => store.Save([preset, preset with { Name = "EVENING" }]));
            await session.RefreshAsync();
            await Assert.ThrowsAsync<BackendException>(() => saved.CreateMutationAsync(session.State! with { Bios = "Other" }, (z,m) => Read(session,z,m)));
        }
        finally { if (Directory.Exists(directory)) Directory.Delete(directory, true); }
    }
    [Fact]
    public async Task StaleLaterZoneRejectsWholeBatchAndDuplicateZonesAreRejected()
    {
        await using var demo = new DemoBackend(); var session = new ControlSession(demo); await session.RefreshAsync();
        var preset = await LightingPreset.CaptureAsync("Test", session.State!, (z,m) => Read(session,z,m));
        var mutation = await preset.CreateMutationAsync(session.State!, (z,m) => Read(session,z,m));
        mutation["LightingPreset"]!["patches"]![0]!["writes"]![0]!["value"] = 17;
        var original = (await Read(session,0,0)).Value(0);
        var draft = new AdvancedLightingDraft(); await draft.LoadAsync(6,0,(z,m) => Read(session,z,m)); draft.SetValue(AdvancedLighting.Fields(0)[0], 18);
        await session.ApplyAsync(draft.CreateMutation()); await session.RefreshAsync();
        Assert.Equal("Rejected", (await session.ApplyAsync(mutation)).Outcome);
        Assert.Equal(original, (await Read(session,0,0)).Value(0));
        Assert.Throws<BackendException>(() => (preset with { Zones = [preset.Zones[0],preset.Zones[0]] }).Validate());
    }
    [Fact]
    public async Task SoftwareDefectsAreOmittedAndResetClearsRoleOnlyDraft()
    {
        await using var demo = new DemoBackend(); var session = new ControlSession(demo); await session.RefreshAsync();
        var draft = new AdvancedLightingDraft(); await draft.LoadAsync(6,6,(z,m) => Read(session,z,m));
        Assert.True(draft.CanApply); draft.Discard(); Assert.False(draft.CanApply); Assert.False(draft.Activate);

        var preset = await LightingPreset.CaptureAsync("Software", session.State!, (z,m) => Read(session,z,m));
        var front = preset.Zones.Single(z => z.ZoneId == 6);
        Assert.DoesNotContain(front.Fields, f => f.Index is 0 or 5);
        Assert.Throws<BackendException>(() => (preset with { Zones = [front with { Fields = [..front.Fields, new(5,42)] }] }).Validate());
    }
    [Fact]
    public async Task EthernetActiveZoneIsSkippedAndUnknownEditableFieldsFailCapture()
    {
        await using var demo = new DemoBackend(); var session = new ControlSession(demo); await session.RefreshAsync();
        var preset = await LightingPreset.CaptureAsync("Skip", session.State!, async (z,m) => (await Read(session,z,m)) with { ActiveRole = z == 2 ? 2 : 0 });
        Assert.Equal([2], preset.SkippedZones); Assert.DoesNotContain(preset.Zones, z => z.ZoneId == 2);
        await Assert.ThrowsAsync<BackendException>(() => LightingPreset.CaptureAsync("Unknown", session.State!, async (z,m) => {
            var value = await Read(session,z,m); return value with { Fields = value.Fields! with { SuccessfullyReadItemsMask = 0 } };
        }));
    }
}
