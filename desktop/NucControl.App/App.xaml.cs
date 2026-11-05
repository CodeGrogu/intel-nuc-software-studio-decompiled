using Microsoft.UI.Xaml;
using NucControl.Core;

namespace NucControl.App;

public partial class App : Application
{
    private Mutex? instance;
    private EventWaitHandle? openEvent;
    private MainWindow? window;
    public App() => InitializeComponent();
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        instance = new Mutex(true, "Local\\NucControl.App", out var first);
        openEvent = new EventWaitHandle(false, EventResetMode.AutoReset, "Local\\NucControl.Open");
        if (!first) { openEvent.Set(); Exit(); return; }
        var arguments = Environment.GetCommandLineArgs();
        var demo = arguments.Contains("--demo");
        window = new MainWindow(demo ? new DemoBackend() : new PipeBackend(), demo);
        window.Activate();
        if (arguments.Contains("--tray")) window.Hide();
        _ = Task.Run(() =>
        {
            while (openEvent.WaitOne()) window.DispatcherQueue.TryEnqueue(window.Show);
        });
    }
}
