using System.Buffers.Binary;
using System.Text;
using System.Text.Json.Nodes;
using NucControl.Core;
using Xunit;

namespace NucControl.Tests;

public class ControlTests
{
    [Fact]
    public async Task FramesRoundTripWithShortReads()
    {
        var stream = new MemoryStream();
        await PipeBackend.WriteFrameAsync(stream, JsonValue.Create("Ping")!, TestContext.Current.CancellationToken);
        var input = new ShortReadStream(stream.ToArray());
        Assert.Equal("Ping", (await PipeBackend.ReadFrameAsync(input, TestContext.Current.CancellationToken)).GetValue<string>());
    }
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(262145)]
    public async Task InvalidLengthsAreRejected(int length)
    {
        var bytes = new byte[4]; BinaryPrimitives.WriteInt32LittleEndian(bytes, length);
        await Assert.ThrowsAsync<InvalidDataException>(() => PipeBackend.ReadFrameAsync(new MemoryStream(bytes), TestContext.Current.CancellationToken));
    }
    [Fact]
    public async Task TruncatedAndMalformedFramesFail()
    {
        await Assert.ThrowsAsync<EndOfStreamException>(() => PipeBackend.ReadFrameAsync(new MemoryStream([10, 0, 0, 0, 1]), TestContext.Current.CancellationToken));
        await Assert.ThrowsAnyAsync<System.Text.Json.JsonException>(() => PipeBackend.ReadFrameAsync(new MemoryStream([1, 0, 0, 0, 123]), TestContext.Current.CancellationToken));
    }
    [Fact]
    public void ZeroUnknownAndStaleAreDistinct()
    {
        Assert.Equal("0 RPM", new Reading(0, "EC", 1000, null).Display(" RPM", 1000));
        Assert.Equal("Unavailable", new Reading(null, "EC", 1000, "failed").Display(" RPM", 1000));
        Assert.Equal("Stale", new Reading(1, "EC", 1000, null).Display(" RPM", 9000));
    }
    [Fact]
    public async Task RefreshAndStartupDoNotWrite()
    {
        var backend = new SpyBackend(); var session = new ControlSession(backend);
        await session.RefreshAsync(); await session.RefreshAsync();
        Assert.DoesNotContain(backend.Requests, r => r.Contains("ApplyControl"));
    }
    [Fact]
    public async Task LostAcknowledgementBlocksAnotherWriteAndReconciles()
    {
        var backend = new SpyBackend { LoseAcknowledgement = true }; var session = new ControlSession(backend);
        await session.RefreshAsync();
        await Assert.ThrowsAsync<IOException>(() => session.ApplyAsync(Wire.Request("Fan", new { preset = "Cool" })));
        Assert.NotNull(session.PendingId); Assert.False(session.CanWrite);
        await Assert.ThrowsAsync<BackendException>(() => session.ApplyAsync(Wire.Request("Fan", new { preset = "Balanced" })));
        await session.RefreshAsync(); Assert.Null(session.PendingId); Assert.Equal("Cool", session.State!.FanPreset);
        Assert.Single(backend.Requests, r => r.Contains("ApplyControl"));
    }
    [Fact]
    public async Task DemoPresetsAndSelectedZoneLightingWork()
    {
        await using var backend = new DemoBackend(); var session = new ControlSession(backend); await session.RefreshAsync();
        await session.ApplyAsync(Wire.Request("Power", new { preset = "LowPower" })); await session.RefreshAsync();
        Assert.Equal(25, session.State!.PowerLimits!.Pl1Watts);
        var before = await session.ReadAsync<LightingState>("GetAwakeLighting", "AwakeLighting", new { zone_id = 2 });
        await session.ApplyAsync(Wire.Request("Lighting", new { patch = new { expected = before, brightness = 10, rgb = new[] { 255, 0, 255 } } }));
        var after = await session.ReadAsync<LightingState>("GetAwakeLighting", "AwakeLighting", new { zone_id = 2 });
        Assert.Equal(10, after.Value(0)); Assert.Equal(before.Value(2), after.Value(2)); Assert.Equal(before.Value(6), after.Value(6));
    }
    [Fact] public void BackendErrorsSurfaceTheirReason() => Assert.Throws<BackendException>(() => Wire.Decode<PanelState>(Wire.Request("Error", new { message = "Unsupported" }), "ControlPanelState"));

    [Fact]
    public async Task ProtocolFiveNeverEnablesWrites()
    {
        var session = new ControlSession(new OldBackend());
        await session.RefreshAsync();
        Assert.False(session.CanWrite); Assert.Null(session.State);
        Assert.Contains("update required", session.Message);
        await Assert.ThrowsAsync<BackendException>(() => session.ApplyAsync(Wire.Request("Fan", new { preset = "Cool" })));
    }
    [Fact]
    public async Task CancelledFrameReadCannotAppearSuccessful()
    {
        using var cancelled = new CancellationTokenSource(); cancelled.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => PipeBackend.ReadFrameAsync(new MemoryStream([2, 0, 0, 0, 123, 125]), cancelled.Token));
    }
    private sealed class OldBackend : IBackend
    {
        public Task<JsonNode> RequestAsync(JsonNode request, CancellationToken cancellationToken = default)
        {
            Assert.Equal("GetBackendInfo", request.GetValue<string>());
            return Task.FromResult(Wire.Request("BackendInfo", new BackendInfo("0.1.2", 5)));
        }
        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }

    private sealed class ShortReadStream(byte[] data) : MemoryStream(data)
    {
        public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default) => base.ReadAsync(buffer[..Math.Min(buffer.Length, 1)], cancellationToken);
    }
    private sealed class SpyBackend : IBackend
    {
        private readonly DemoBackend demo = new(); public List<string> Requests { get; } = [];
        public bool LoseAcknowledgement { get; init; }
        public async Task<JsonNode> RequestAsync(JsonNode request, CancellationToken cancellationToken = default)
        {
            Requests.Add(request.ToJsonString()); var result = await demo.RequestAsync(request, cancellationToken);
            if (LoseAcknowledgement && request is JsonObject obj && obj.ContainsKey("ApplyControl")) throw new IOException("Lost acknowledgement");
            return result;
        }
        public ValueTask DisposeAsync() => demo.DisposeAsync();
    }
}
