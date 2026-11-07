using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace NucControl.App;

internal static class DesktopIntegration
{
    public static void SetStartup(bool enabled)
    {
        using var key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
        if (enabled) key.SetValue("NucControl", $"\"{Environment.ProcessPath}\" --tray");
        else key.DeleteValue("NucControl", false);
    }
    public static bool StartupEnabled()
    {
        using var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
        return key?.GetValue("NucControl") is string value && value == $"\"{Environment.ProcessPath}\" --tray";
    }
    internal delegate nint Subclass(nint hwnd, uint message, nuint wparam, nint lparam, nuint id, nuint data);
    [DllImport("user32.dll")] internal static extern uint GetDpiForWindow(nint hwnd);
    internal static void SetMinimumSize(nint hwnd, nint minMaxInfo)
    {
        var scale = GetDpiForWindow(hwnd) / 96.0;
        Marshal.WriteInt32(minMaxInfo, 24, (int)(400 * scale));
        Marshal.WriteInt32(minMaxInfo, 28, (int)(360 * scale));
    }
    [DllImport("comctl32.dll")] internal static extern bool SetWindowSubclass(nint hwnd, Subclass callback, nuint id, nuint data);
    [DllImport("comctl32.dll")] internal static extern nint DefSubclassProc(nint hwnd, uint message, nuint wparam, nint lparam);
}
