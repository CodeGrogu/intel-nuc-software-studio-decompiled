namespace NucControl.App;

/// Local, bounded diagnostics only. Never suppress the original startup exception.
internal static class StartupLog
{
    private static readonly object Sync = new();
    internal static void Write(string stage, Exception? error = null)
    {
        try
        {
            lock (Sync)
            {
                var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NucControl",
                    Environment.GetCommandLineArgs().Contains("--demo") ? "Demo" : "", "Logs");
                Directory.CreateDirectory(directory);
                var file = Path.Combine(directory, "startup.log");
                if (File.Exists(file) && new FileInfo(file).Length > 64 * 1024) File.Move(file, file + ".previous", true);
                var details = error is null ? "" : $"HRESULT=0x{error.HResult:X8}\n{error}";
                if (details.Length > 12000) details = details[..12000];
                File.AppendAllText(file, $"{DateTimeOffset.UtcNow:O} pid={Environment.ProcessId} version={typeof(App).Assembly.GetName().Version} {stage}\n{details}\n");
            }
        }
        catch (Exception e) when (e is IOException or UnauthorizedAccessException or System.Security.SecurityException)
        {
            // Failure to write diagnostics must not hide the original failure.
        }
    }
}
