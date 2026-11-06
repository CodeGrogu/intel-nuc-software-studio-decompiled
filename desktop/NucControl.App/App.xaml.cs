using Microsoft.UI.Xaml;
using NucControl.Core;

namespace NucControl.App;

public partial class App : Application
{
    private Mutex? instance;
    private EventWaitHandle? openEvent;
    private MainWindow? window;
    public App()
    {
        StartupLog.Write("Application construction");
        UnhandledException += (_, e) => StartupLog.Write("Unhandled XAML exception", e.Exception);
        AppDomain.CurrentDomain.UnhandledException += (_, e) => StartupLog.Write("Unhandled process exception", e.ExceptionObject as Exception);
        try { InitializeComponent(); StartupLog.Write("Application resources loaded"); }
        catch (Exception e) { StartupLog.Write("Application resource initialization failed", e); throw; }
    }
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        try { Launch(); }
        catch (Exception e) { StartupLog.Write("Launch failed", e); throw; }
    }
    private void Launch()
    {
        StartupLog.Write("Launch started");
        var arguments = Environment.GetCommandLineArgs();
        var demo = arguments.Contains("--demo");
        instance = new Mutex(true, demo ? "Local\\NucControl.Demo" : "Local\\NucControl.App", out var first);
        openEvent = new EventWaitHandle(false, EventResetMode.AutoReset, demo ? "Local\\NucControl.Open.Demo" : "Local\\NucControl.Open");
        if (!first) { openEvent.Set(); Exit(); return; }
        window = new MainWindow(demo ? new DemoBackend() : new PipeBackend(), demo);
        window.Activate();
        StartupLog.Write("Window activated");
        if (arguments.Contains("--tray")) window.Hide();
        _ = Task.Run(() =>
        {
            while (openEvent.WaitOne()) window.DispatcherQueue.TryEnqueue(window.Show);
        });
    }
}
