using System.Text.Json.Nodes;
using NucControl.Core;
using Xunit;

namespace NucControl.Tests;

public class RecoveryTests
{
    private static JsonNode Fan() => Wire.Request("Fan", new { preset = "Cool" });
    private static TaskCompletionSource<T> Deferred<T>() => new(TaskCreationOptions.RunContinuationsAsynchronously);
    private static LightingState Light(int zone, int? brightness = 50, int? blue = 255) => new(zone, 0,
        new(zone, 0, 63, 63, true, Enumerable.Range(0, 6).Select(i => new LedItem(i, 0,
            i == 0 ? brightness : i == 5 ? blue : 0, null)).ToArray()), null);

    [Fact]
    public async Task SelectedZoneIsInvalidatedBeforeAwaitAndStaleCompletionsCannotReplaceIt()
    {
        var draft = new LightingDraft(); await draft.LoadAsync(2, id => Task.FromResult(Light(id)));
        draft.SetBrightness(12);
        var first = Deferred<LightingState>(); var second = Deferred<LightingState>();
        var readingFirst = draft.LoadAsync(4, _ => first.Task);
        Assert.True(draft.IsLoading); Assert.Null(draft.Snapshot); Assert.False(draft.CanEdit);
        draft.SetBrightness(90); Assert.Null(draft.Brightness);
        Assert.Throws<BackendException>(() => draft.CreateMutation());
        var readingSecond = draft.LoadAsync(6, _ => second.Task);
        second.SetResult(Light(6)); Assert.True(await readingSecond);
        draft.SetBrightness(20);
        first.SetResult(Light(4)); Assert.False(await readingFirst);
        Assert.Equal(6, draft.SelectedZone); Assert.Equal(20, draft.Brightness);
        Assert.Equal(6, draft.CreateMutation()["Lighting"]!["patch"]!["expected"]!["zone_id"]!.GetValue<int>());
    }

    [Fact]
    public async Task LateReadFailureDoesNotClearNewSelectionAndWrongZoneResponsesFailClosed()
    {
        var draft = new LightingDraft(); var old = Deferred<LightingState>();
        var reading = draft.LoadAsync(2, _ => old.Task);
        await draft.LoadAsync(4, id => Task.FromResult(Light(id)));
        old.SetException(new IOException("late")); Assert.False(await reading);
        Assert.Equal(4, draft.Snapshot!.ZoneId);
        await Assert.ThrowsAsync<BackendException>(() => draft.LoadAsync(6, _ => Task.FromResult(Light(2))));
        Assert.Null(draft.Snapshot); Assert.False(draft.CanEdit); Assert.False(draft.IsLoading);
    }

    [Fact]
    public async Task UnknownFieldsStayUnknownWhileReadableZeroAndRgbSurvive()
    {
        var draft = new LightingDraft();
        await draft.LoadAsync(2, id => Task.FromResult(Light(id, null)));
        Assert.Null(draft.Brightness); Assert.Contains("unavailable", draft.BrightnessLabel);
        Assert.Equal(new RgbColor(0, 0, 255), draft.Color); Assert.False(draft.CanEdit);
        await draft.LoadAsync(2, id => Task.FromResult(Light(id, 0, null)));
        Assert.Equal(0, draft.Brightness); Assert.Contains("0%", draft.BrightnessLabel); Assert.Null(draft.Color);
        var state = Light(2) with { Fields = Light(2).Fields with { SuccessfullyReadItemsMask = 62 } };
        Assert.Null(state.Value(0));
        var failed = Light(2) with { Fields = Light(2).Fields with { Items = [new(0, 228, 99, null)] } };
        Assert.Null(failed.Value(0));
    }

    [Fact]
    public async Task RetainedDraftRebasesOnlyItsOwnZoneAndDoesNotWriteOnRead()
    {
        var draft = new LightingDraft(); await draft.LoadAsync(2, id => Task.FromResult(Light(id)));
        draft.SetBrightness(20);
        await draft.LoadAsync(2, id => Task.FromResult(Light(id, 60)), true);
        Assert.Equal(20, draft.Brightness); Assert.Equal(60, draft.Snapshot!.Value(0));
        Assert.Equal(20, draft.CreateMutation()["Lighting"]!["patch"]!["brightness"]!.GetValue<int>());
        await Assert.ThrowsAsync<InvalidOperationException>(() => draft.LoadAsync(4, id => Task.FromResult(Light(id)), true));
        draft.Discard(); Assert.False(draft.IsDirty); Assert.Equal(60, draft.Brightness);
    }

    [Theory]
    [InlineData(0, false)] // Connect failed: no request frame was attempted.
    [InlineData(1, true)]  // Partial write: reconciliation required.
    [InlineData(2, true)]  // Full write, response lost.
    public async Task ActualTransportReportsSubmissionBoundary(int phase, bool uncertain)
    {
        var error = await Assert.ThrowsAsync<BackendTransportException>(() => PipeBackend.ExchangeAsync(
            _ => phase == 0 ? Task.FromException<Stream>(new IOException("connect"))
                : Task.FromResult<Stream>(new FailingStream(phase)), Fan(), TestContext.Current.CancellationToken));
        Assert.Equal(uncertain, error.MayHaveBeenSubmitted);
    }

    [Fact]
    public async Task AccessDeniedBeforeConnectIsNotSubmitted()
    {
        var error = await Assert.ThrowsAsync<BackendTransportException>(() => PipeBackend.ExchangeAsync(
            _ => Task.FromException<Stream>(new UnauthorizedAccessException("pipe ACL denied")), Fan(), TestContext.Current.CancellationToken));
        Assert.False(error.MayHaveBeenSubmitted);
    }

    [Fact]
    public async Task PreSubmitFailureClearsPendingButRequiresFreshState()
    {
        var backend = new ScriptBackend(); var session = new ControlSession(backend); await session.RefreshAsync();
        backend.Handler = request => Is(request, "ApplyControl")
            ? Task.FromException<JsonNode>(new BackendTransportException(false, new IOException("connect"))) : null;
        await Assert.ThrowsAsync<BackendTransportException>(() => session.ApplyAsync(Fan()));
        Assert.Null(session.PendingId); Assert.False(session.CanWrite);
        backend.Requests.Clear(); await session.RefreshAsync();
        Assert.True(session.CanWrite); Assert.DoesNotContain(backend.Requests, r => Is(r, "ReconcileOperation"));
    }

    [Fact]
    public async Task PendingReconciliationUsesOriginalTokenBeforeAnyGatedRead()
    {
        var backend = new ScriptBackend(); var session = new ControlSession(backend); await session.RefreshAsync();
        var original = session.Admission!;
        backend.Handler = request => Is(request, "ApplyControl")
            ? Task.FromException<JsonNode>(new BackendTransportException(true, new IOException("lost")))
            : Is(request, "ReconcileOperation") ? Task.FromResult(Status(request, "Pending")) : null;
        await Assert.ThrowsAsync<BackendTransportException>(() => session.ApplyAsync(Fan()));
        var id = session.PendingId;
        backend.Requests.Clear(); await session.RefreshAsync();
        var only = Assert.Single(backend.Requests);
        Assert.True(Is(only, "ReconcileOperation")); Assert.Equal(original.AdmissionEpoch, only["ReconcileOperation"]!["admission_epoch"]!.GetValue<ulong>());
        Assert.Equal(id, session.PendingId); Assert.Null(session.State); Assert.False(session.CanWrite);
    }

    [Fact]
    public async Task FencedUnknownCannotUnlockUntilFreshStateAndNeverReplays()
    {
        var backend = new ScriptBackend(); var session = new ControlSession(backend); await session.RefreshAsync();
        JsonNode? envelope = null;
        backend.Handler = request => { if (!Is(request, "ApplyControl")) return null; envelope = request.DeepClone(); return Task.FromException<JsonNode>(new BackendTransportException(true, new IOException("lost before admission"))); };
        await Assert.ThrowsAsync<BackendTransportException>(() => session.ApplyAsync(Fan()));
        backend.Handler = request => Name(request) == "GetControlPanelState" ? Task.FromException<JsonNode>(new IOException("read failed")) : null;
        await Assert.ThrowsAsync<IOException>(() => session.RefreshAsync());
        Assert.NotNull(session.PendingId); Assert.False(session.CanWrite);
        var delayed = Wire.Decode<OperationStatus>(await backend.Demo.RequestAsync(envelope!, TestContext.Current.CancellationToken), "OperationStatus");
        Assert.Equal("Rejected", delayed.Outcome);
        backend.Handler = null; await session.RefreshAsync();
        Assert.Null(session.PendingId); Assert.True(session.CanWrite); Assert.Equal("Balanced", session.State!.FanPreset);
        Assert.Single(backend.Requests, r => Is(r, "ApplyControl"));
    }

    [Fact]
    public async Task ExpiredAcceptedHistoryIsFencedAndActualStateIsRefreshed()
    {
        var backend = new ScriptBackend(); var session = new ControlSession(backend); await session.RefreshAsync();
        backend.Handler = async request => { if (!Is(request, "ApplyControl")) return await backend.Demo.RequestAsync(request); await backend.Demo.RequestAsync(request); throw new BackendTransportException(true, new IOException("lost")); };
        await Assert.ThrowsAsync<BackendTransportException>(() => session.ApplyAsync(Fan()));
        backend.Handler = null;
        for (var i = 0; i < 127; i++)
            await backend.Demo.RequestAsync(Wire.Request("ApplyControl", new { operation_id = $"other-{i}", backend_instance = "demo", admission_epoch = 1, mutation = Fan() }), TestContext.Current.CancellationToken);
        await session.RefreshAsync();
        Assert.True(session.CanWrite); Assert.Contains("unavailable", session.Message); Assert.Equal("Cool", session.State!.FanPreset);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task MismatchedStatusIdentityNeverUnlocks(bool wrongId)
    {
        var backend = new ScriptBackend(); var session = new ControlSession(backend); await session.RefreshAsync();
        backend.Handler = request => Is(request, "ApplyControl") ? Task.FromResult(Wire.Request("OperationStatus",
            new OperationStatus(wrongId ? "wrong" : request["ApplyControl"]!["operation_id"]!.GetValue<string>(), wrongId ? "demo" : "other", "Verified", "bad"))) : null;
        await Assert.ThrowsAsync<BackendException>(() => session.ApplyAsync(Fan()));
        Assert.NotNull(session.PendingId); Assert.False(session.CanWrite);
    }

    [Fact]
    public async Task InstanceChangeDuringReconciliationStillRequiresConsistentFreshSnapshot()
    {
        var backend = new ScriptBackend(); var session = new ControlSession(backend); await session.RefreshAsync();
        backend.Handler = r => Is(r, "ApplyControl") ? Task.FromException<JsonNode>(new IOException("lost")) : null;
        await Assert.ThrowsAsync<IOException>(() => session.ApplyAsync(Fan()));
        backend.Handler = r => Is(r, "ReconcileOperation") ? Task.FromResult(Status(r, "Uncertain", "new-instance"))
            : Name(r) == "GetControlSessionInfo" ? Task.FromResult(Wire.Request("ControlSessionInfo", new ControlSessionInfo("new-instance", 1))) : null;
        await Assert.ThrowsAsync<BackendException>(() => session.RefreshAsync()); // Panel still belongs to demo.
        Assert.NotNull(session.PendingId); Assert.False(session.CanWrite);
        backend.Handler = null; await session.RefreshAsync(); Assert.True(session.CanWrite);
    }

    [Fact]
    public async Task ExactPowerSelectionIgnoresLegacyRoundedTriple()
    {
        var backend = new DemoBackend(); var session = new ControlSession(backend); await session.RefreshAsync();
        var state = session.State!; var balanced = state.PowerPresets.Single(p => p.PresetKind == "Balanced");
        Assert.True(state.MatchesPowerPreset(balanced));
        var fractional = state with { PowerReadback = new(244, 480, 15, 32) };
        Assert.False(fractional.MatchesPowerPreset(balanced)); Assert.Contains("30.5", fractional.PowerReadback.Display);
        var unknown = state with { PowerReadback = new(240, 480, 127, null) };
        Assert.False(unknown.MatchesPowerPreset(balanced)); Assert.Contains("Unknown time window", unknown.PowerReadback.Display);
    }

    private static string Name(JsonNode r) => r is JsonValue ? r.GetValue<string>() : r.AsObject().First().Key;
    private static bool Is(JsonNode r, string name) => Name(r) == name;
    private static JsonNode Status(JsonNode r, string outcome, string instance = "demo") => Wire.Request("OperationStatus",
        new OperationStatus(r[Name(r)]!["operation_id"]!.GetValue<string>(), instance, outcome, "test status"));
    private sealed class ScriptBackend : IBackend
    {
        public DemoBackend Demo { get; } = new();
        public List<JsonNode> Requests { get; } = [];
        public Func<JsonNode, Task<JsonNode>?>? Handler { get; set; }
        public Task<JsonNode> RequestAsync(JsonNode request, CancellationToken token = default)
        { Requests.Add(request.DeepClone()); return Handler?.Invoke(request) ?? Demo.RequestAsync(request, token); }
        public ValueTask DisposeAsync() => Demo.DisposeAsync();
    }
    private sealed class FailingStream(int phase) : MemoryStream
    {
        public override ValueTask WriteAsync(ReadOnlyMemory<byte> bytes, CancellationToken token = default)
            => phase == 1 ? ValueTask.FromException(new IOException("partial write")) : base.WriteAsync(bytes, token);
        public override ValueTask<int> ReadAsync(Memory<byte> bytes, CancellationToken token = default)
            => ValueTask.FromException<int>(new IOException("response lost"));
    }
}
