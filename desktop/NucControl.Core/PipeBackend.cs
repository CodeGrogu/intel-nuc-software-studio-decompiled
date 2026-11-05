using System.Buffers.Binary;
using System.IO.Pipes;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace NucControl.Core;

public sealed class PipeBackend : IBackend
{
    private readonly SemaphoreSlim serial = new(1, 1);
    // A request owns its connection. A cancelled/truncated response cannot poison the next frame.
    public async Task<JsonNode> RequestAsync(JsonNode request, CancellationToken cancellationToken = default)
    {
        await serial.WaitAsync(cancellationToken);
        try
        {
            using var deadline = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            deadline.CancelAfter(TimeSpan.FromSeconds(15));
            await using var pipe = new NamedPipeClientStream(".", "Nuc11PhStudioV1", PipeDirection.InOut, PipeOptions.Asynchronous);
            await pipe.ConnectAsync(deadline.Token);
            await WriteFrameAsync(pipe, request, deadline.Token);
            return await ReadFrameAsync(pipe, deadline.Token);
        }
        finally { serial.Release(); }
    }
    public static async Task WriteFrameAsync(Stream stream, JsonNode request, CancellationToken token)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(request);
        if (bytes.Length is 0 or > Wire.MaxFrame) throw new InvalidDataException("Invalid outgoing frame length");
        var header = new byte[4];
        BinaryPrimitives.WriteInt32LittleEndian(header, bytes.Length);
        await stream.WriteAsync(header, token);
        await stream.WriteAsync(bytes, token);
        await stream.FlushAsync(token);
    }
    public static async Task<JsonNode> ReadFrameAsync(Stream stream, CancellationToken token)
    {
        var header = new byte[4];
        await stream.ReadExactlyAsync(header, token);
        var length = BinaryPrimitives.ReadInt32LittleEndian(header);
        if (length is <= 0 or > Wire.MaxFrame) throw new InvalidDataException("Invalid incoming frame length");
        var payload = new byte[length];
        await stream.ReadExactlyAsync(payload, token);
        return JsonNode.Parse(payload) ?? throw new JsonException("Empty response");
    }
    public ValueTask DisposeAsync() { serial.Dispose(); return ValueTask.CompletedTask; }
}
