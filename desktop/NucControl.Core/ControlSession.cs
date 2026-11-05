using System.Text.Json.Nodes;

namespace NucControl.Core;

public sealed class ControlSession(IBackend backend)
{
    private readonly SemaphoreSlim serial = new(1, 1);
    public BackendInfo? Info { get; private set; }
    public PanelState? State { get; private set; }
    public string? PendingId { get; private set; }
    private string? pendingInstance;
    public string Message { get; private set; } = "Connecting…";
    public bool CanWrite => Info?.ProtocolRevision == 6 && State?.DeviceEligible == true && PendingId is null;
    public Task<T> ReadAsync<T>(string request, string response, object? body = null) => ReadAsync<T>(
        body is null ? JsonValue.Create(request)! : Wire.Request(request, body), response);
    private async Task<T> ReadAsync<T>(JsonNode request, string response) => Wire.Decode<T>(await backend.RequestAsync(request), response);
    public async Task RefreshAsync()
    {
        await serial.WaitAsync();
        try
        {
            Info = await ReadAsync<BackendInfo>("GetBackendInfo", "BackendInfo");
            if (Info.ProtocolRevision != 6) { State = null; Message = "Backend update required for controls (protocol 6)."; return; }
            var fresh = await ReadAsync<PanelState>("GetControlPanelState", "ControlPanelState");
            if (PendingId is not null)
            {
                if (pendingInstance != fresh.BackendInstance) { PendingId = null; Message = "Service restarted. State refreshed; previous write outcome is unknown."; }
                else
                {
                    var status = await ReadAsync<OperationStatus>("GetOperationStatus", "OperationStatus", new { operation_id = PendingId });
                    Message = status.Message;
                    if (status.Outcome is "Verified" or "Rejected" or "PartialFailure") PendingId = null;
                }
            }
            else Message = fresh.DeviceEligible ? "Connected" : "Device or firmware is outside the reviewed configuration.";
            State = fresh;
        }
        catch { State = null; Message = "Backend unavailable. Check the service, then reconnect."; throw; }
        finally { serial.Release(); }
    }
    public async Task<OperationStatus> ApplyAsync(JsonNode mutation)
    {
        await serial.WaitAsync();
        try
        {
            if (!CanWrite) throw new BackendException("Refresh a compatible backend before applying settings.");
            PendingId = Guid.NewGuid().ToString("D");
            pendingInstance = State!.BackendInstance;
            // A lost acknowledgement leaves PendingId intact. Never resend this mutation automatically.
            var status = await ReadAsync<OperationStatus>(Wire.Request("ApplyControl", new { operation_id = PendingId, mutation }), "OperationStatus");
            if (status.BackendInstance != pendingInstance) throw new BackendException("Backend instance changed during operation.");
            Message = status.Message;
            if (status.Outcome is "Verified" or "Rejected" or "PartialFailure") PendingId = null;
            return status;
        }
        finally { serial.Release(); }
    }
}
