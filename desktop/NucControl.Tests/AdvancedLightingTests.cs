using NucControl.Core;
using Xunit;

namespace NucControl.Tests;
public class AdvancedLightingTests
{
    [Fact]
    public async Task FailedOrPartialOperationDraftRetainsTargetAndRebasesOnlyAfterFreshRead()
    {
        var demo = new DemoBackend(); var session = new ControlSession(demo); await session.RefreshAsync(); var draft = new AdvancedLightingDraft();
        Task<LightingModeState> Read(int z, int m) => session.ReadAsync<LightingModeState>("GetLightingMode", "LightingMode", new { zone_id = z, indicator_mode = m });
        await draft.LoadAsync(6, 4, Read); draft.SetValue(AdvancedLighting.Fields(4).Single(f => f.Index == 3), 70);
        await Assert.ThrowsAsync<IOException>(() => draft.LoadAsync(6, 4, (_, _) => Task.FromException<LightingModeState>(new IOException("read failed")), true));
        Assert.Equal(4, draft.SelectedMode); Assert.Equal(70, draft.Value(3)); Assert.False(draft.CanApply);
        await Assert.ThrowsAsync<BackendException>(() => draft.LoadAsync(6, 0, Read, true));
        await draft.LoadAsync(6, 4, Read, true); Assert.Equal(70, draft.Value(3)); Assert.Equal(0, draft.Snapshot!.Value(3));
        await session.ApplyAsync(draft.CreateMutation());
        await draft.LoadAsync(6, 4, Read, true); Assert.False(draft.IsDirty); Assert.False(draft.CanApply);
    }
    [Fact]
    public async Task DraftAndDemoApplyStandbyEffectsWithoutAwakeOrOtherZoneChanges()
    {
        await using var backend = new DemoBackend(); var session = new ControlSession(backend); await session.RefreshAsync();
        Task<LightingModeState> Read(int zone, int mode) => session.ReadAsync<LightingModeState>("GetLightingMode", "LightingMode", new { zone_id = zone, indicator_mode = mode });
        var other = await Read(4, 0); var draft = new AdvancedLightingDraft(); await draft.LoadAsync(2, 0, Read);
        var before = draft.Snapshot!;
        draft.SetValue(AdvancedLighting.Fields(0, true).Single(f => f.Index == 13), 2);
        draft.SetValue(AdvancedLighting.Fields(0, true).Single(f => f.Index == 14), 7);
        await session.ApplyAsync(draft.CreateMutation());
        var after = await Read(2, 0); Assert.Equal(2, after.Value(13)); Assert.Equal(7, after.Value(14)); Assert.Equal(before.Value(1), after.Value(1));
        var otherAfter = await Read(4, 0); Assert.Equal(other.Fields!.Items, otherAfter.Fields!.Items);
    }
    [Fact]
    public async Task SoftwareUnknownAndFrontRightBlueRemainBlockedButRedIsEditable()
    {
        await using var backend = new DemoBackend(); var session = new ControlSession(backend); await session.RefreshAsync();
        var draft = new AdvancedLightingDraft();
        await draft.LoadAsync(6, 4, (z, m) => session.ReadAsync<LightingModeState>("GetLightingMode", "LightingMode", new { zone_id = z, indicator_mode = m }));
        Assert.Null(draft.Value(0));
        Assert.Throws<BackendException>(() => draft.SetValue(AdvancedLighting.Fields(4).Single(f => f.Index == 0), 50));
        Assert.Throws<BackendException>(() => draft.SetValue(AdvancedLighting.Fields(4).Single(f => f.Index == 5), 12));
        draft.SetValue(AdvancedLighting.Fields(4).Single(f => f.Index == 3), 40);
        await session.ApplyAsync(draft.CreateMutation());
        var power = await session.ReadAsync<LightingModeState>("GetLightingMode", "LightingMode", new { zone_id = 6, indicator_mode = 0 });
        Assert.Equal(40, power.Value(3)); Assert.Equal(4, power.ActiveRole); // Same-zone RGB alias, different role.
    }
    [Fact]
    public async Task DelayedRoleReadCannotReplaceNewerDraft()
    {
        var demo = new DemoBackend(); var session = new ControlSession(demo);
        Task<LightingModeState> Read(int z, int m) => session.ReadAsync<LightingModeState>("GetLightingMode", "LightingMode", new { zone_id = z, indicator_mode = m });
        var late = new TaskCompletionSource<LightingModeState>(TaskCreationOptions.RunContinuationsAsynchronously); var draft = new AdvancedLightingDraft();
        var first = draft.LoadAsync(2, 0, (_, _) => late.Task);
        Assert.False(draft.CanApply); Assert.Null(draft.Snapshot);
        await draft.LoadAsync(2, 1, Read); draft.SetValue(AdvancedLighting.Fields(1)[0], 20);
        late.SetResult(await Read(2, 0)); Assert.False(await first); Assert.Equal(1, draft.Snapshot!.IndicatorMode); Assert.Equal(20, draft.Value(0));
    }
    [Fact]
    public async Task DisabledRoleUsesNoFieldReadsOrFakeValues()
    {
        var demo = new DemoBackend(); var session = new ControlSession(demo); await session.RefreshAsync(); var draft = new AdvancedLightingDraft();
        await draft.LoadAsync(2, 6, (z, m) => session.ReadAsync<LightingModeState>("GetLightingMode", "LightingMode", new { zone_id = z, indicator_mode = m }));
        Assert.Null(draft.Snapshot!.Fields); Assert.True(draft.CanApply);
        await session.ApplyAsync(draft.CreateMutation());
        var after = await session.ReadAsync<LightingState>("GetAwakeLighting", "AwakeLighting", new { zone_id = 2 });
        Assert.Equal(6, after.ActiveRole); Assert.Equal(50, after.Value(0));
    }
    [Fact]
    public async Task StaleAdvancedPatchIsRejectedAndModeSelectionDoesNotWrite()
    {
        var demo = new DemoBackend(); var session = new ControlSession(demo); await session.RefreshAsync(); var draft = new AdvancedLightingDraft();
        Task<LightingModeState> Read(int z, int m) => session.ReadAsync<LightingModeState>("GetLightingMode", "LightingMode", new { zone_id = z, indicator_mode = m });
        await draft.LoadAsync(2, 1, Read); Assert.Equal(0, (await Read(2, 0)).ActiveRole);
        draft.SetValue(AdvancedLighting.Fields(1)[0], 20); var mutation = draft.CreateMutation();
        await session.ApplyAsync(mutation); await session.RefreshAsync();
        var rejected = await session.ApplyAsync(mutation); Assert.Equal("Rejected", rejected.Outcome);
    }
}
