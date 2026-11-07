using System.Text.Json.Nodes;
using NucControl.Core;
using Xunit;
namespace NucControl.Tests;
public class LegacyConnectionTests
{
    private sealed class Legacy : IBackend
    {
        public List<string> Requests { get; } = [];
        public Task<JsonNode> RequestAsync(JsonNode request, CancellationToken cancellationToken = default)
        {
            var name = request.GetValue<string>(); Requests.Add(name);
            return Task.FromResult(name switch
            {
                "GetBackendInfo" => Wire.Request("BackendInfo", new BackendInfo("0.1.2",5)),
                "GetVerifiedHardwareState" => JsonNode.Parse("""{"VerifiedHardwareState":{"snapshot":{"driver_status":"Connected","wmi_status":"Partial LED readback","telemetry":{"cpu_temp_c":0,"active_pl1_watts":30}},"capabilities":{"driver_connected":true,"wmi_connected":true}}}""")!,
                _ => throw new InvalidOperationException(name)
            });
        }
        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }
    [Fact]
    public async Task Revision5DetailsAreReadOnlyAndNeverPromoteLegacyMetricsOrControls()
    {
        await using var backend = new Legacy(); var session = new ControlSession(backend);
        await session.RefreshAsync(); Assert.False(session.CanWrite); Assert.Null(session.State);
        var status = await session.ReadLegacyConnectionAsync(); Assert.True(status.Capabilities.DriverConnected); Assert.Equal("Partial LED readback", status.Snapshot.WmiStatus);
        await Assert.ThrowsAsync<BackendException>(() => session.ApplyAsync(Wire.Request("Power",new { preset="Balanced" })));
        Assert.Equal(new[] { "GetBackendInfo", "GetVerifiedHardwareState" },backend.Requests);
    }
    [Fact]
    public async Task NewBackendCannotUseLegacyInspection()
    {
        await using var backend=new DemoBackend(); var session=new ControlSession(backend); await session.RefreshAsync();
        await Assert.ThrowsAsync<BackendException>(() => session.ReadLegacyConnectionAsync());
    }
}
