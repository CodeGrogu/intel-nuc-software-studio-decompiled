using System.Text.Json.Nodes;

namespace NucControl.Core;

public enum ExitBlock { None, Busy, UnresolvedOperation }

public sealed class ControlSession(IBackend backend)
{
    /// An in-flight request blocks exit; an unresolved operation needs confirmation because refresh may never clear it.
    public static ExitBlock ExitBlockFor(bool busy, string? pendingId) => busy ? ExitBlock.Busy : pendingId is null ? ExitBlock.None : ExitBlock.UnresolvedOperation;
    private readonly SemaphoreSlim serial = new(1, 1);
    public BackendInfo? Info { get; private set; }
    public ControlSessionInfo? Admission { get; private set; }
    public PanelState? State { get; private set; }
    public string? PendingId { get; private set; }
    private ControlSessionInfo? pendingAdmission;
    public string Message { get; private set; } = "Connecting…";
    public bool CanWrite => Info?.ProtocolRevision == 6 && State?.DeviceEligible == true
        && Admission?.BackendInstance == State.BackendInstance && PendingId is null;
    public Task<T> ReadAsync<T>(string request, string response, object? body = null) => ReadAsync<T>(
        body is null ? JsonValue.Create(request)! : Wire.Request(request, body), response);
    private async Task<T> ReadAsync<T>(JsonNode request, string response) => Wire.Decode<T>(await backend.RequestAsync(request), response);
    public Task<LegacyConnectionState> ReadLegacyConnectionAsync()
    {
        if (Info?.ProtocolRevision != 5 || PendingId is not null) throw new BackendException("Legacy connection inspection requires an idle revision-5 backend.");
        return ReadAsync<LegacyConnectionState>("GetVerifiedHardwareState", "VerifiedHardwareState");
    }
    private static bool Terminal(string outcome) => outcome is "Acknowledged" or "Verified" or "Rejected" or "PartialFailure" or "Uncertain";
    private static void ValidateStatus(OperationStatus status, string id)
    {
        if (status.OperationId != id || string.IsNullOrWhiteSpace(status.BackendInstance)
            || (status.Outcome != "Pending" && !Terminal(status.Outcome)))
            throw new BackendException("Invalid operation status; controls remain locked until reconciliation.");
    }
    public async Task RefreshAsync()
    {
        await serial.WaitAsync();
        State = null; // Never unlock from a terminal acknowledgement alone.
        try
        {
            string? recoveredMessage = null;
            if (PendingId is string id)
            {
                var status = await ReadAsync<OperationStatus>("ReconcileOperation", "OperationStatus", new
                {
                    operation_id = id,
                    backend_instance = pendingAdmission!.BackendInstance,
                    admission_epoch = pendingAdmission.AdmissionEpoch
                });
                ValidateStatus(status, id);
                if (status.BackendInstance != pendingAdmission.BackendInstance && status.Outcome != "Uncertain")
                    throw new BackendException("Unexpected backend instance in reconciliation.");
                Message = status.Message;
                if (status.Outcome == "Pending") return; // No hardware-gated requests while pending.
                recoveredMessage = status.Message;
            }
            Info = await ReadAsync<BackendInfo>("GetBackendInfo", "BackendInfo");
            if (Info.ProtocolRevision != 6) { Message = "Backend update required for controls (protocol 6)."; return; }
            var admission = await ReadAsync<ControlSessionInfo>("GetControlSessionInfo", "ControlSessionInfo");
            if (string.IsNullOrWhiteSpace(admission.BackendInstance) || admission.AdmissionEpoch == 0)
                throw new BackendException("Invalid admission metadata.");
            var fresh = await ReadAsync<PanelState>("GetControlPanelState", "ControlPanelState");
            if (fresh.BackendInstance != admission.BackendInstance)
                throw new BackendException("Service changed during refresh. Refresh again before applying settings.");
            Admission = admission;
            State = fresh;
            PendingId = null;
            pendingAdmission = null;
            Message = recoveredMessage ?? (fresh.DeviceEligible ? "Connected" : "Device or firmware is outside the reviewed configuration.");
        }
        catch { State = null; Message = PendingId is null ? "Backend unavailable. Refresh to reconnect." : "Write outcome unresolved. Refresh to reconcile; no command will be replayed."; throw; }
        finally { serial.Release(); }
    }
    public async Task<OperationStatus> ApplyAsync(JsonNode mutation)
    {
        await serial.WaitAsync();
        try
        {
            if (!CanWrite) throw new BackendException("Refresh a compatible backend before applying settings.");
            PendingId = Guid.NewGuid().ToString("D");
            pendingAdmission = Admission!;
            State = null;
            var status = await ReadAsync<OperationStatus>(Wire.Request("ApplyControl", new
            {
                operation_id = PendingId,
                backend_instance = pendingAdmission.BackendInstance,
                admission_epoch = pendingAdmission.AdmissionEpoch,
                mutation
            }), "OperationStatus");
            ValidateStatus(status, PendingId);
            if (status.BackendInstance != pendingAdmission.BackendInstance && status.Outcome != "Rejected")
                throw new BackendException("Backend instance changed during operation.");
            if (status.Outcome == "Uncertain") throw new BackendException("Reconcile the uncertain operation before continuing.");
            Message = status.Message;
            if (Terminal(status.Outcome)) { PendingId = null; pendingAdmission = null; }
            return status;
        }
        catch (BackendTransportException e) when (!e.MayHaveBeenSubmitted)
        {
            PendingId = null;
            pendingAdmission = null;
            State = null;
            Message = "Command was not submitted. Refresh before applying a new setting.";
            throw;
        }
        finally { serial.Release(); }
    }
}
