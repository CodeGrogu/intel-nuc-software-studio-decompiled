using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace NucSoftwareStudio.Models;

public static class MainWindowLauncher
{
	private const int SW_RESTORE = 9;

	[DllImport("USER32.DLL")]
	public static extern bool SetForegroundWindow(IntPtr hWnd);

	[DllImport("USER32.dll")]
	private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

	public static bool SetRunningAppToForeground(string MainWindowTitle)
	{
		try
		{
			Process[] processesByName = Process.GetProcessesByName("NucSoftwareStudio");
			foreach (Process process in processesByName)
			{
				if (process.MainWindowTitle.Equals(MainWindowTitle))
				{
					IntPtr mainWindowHandle = process.MainWindowHandle;
					ShowWindow(mainWindowHandle, 9);
					SetForegroundWindow(mainWindowHandle);
					return true;
				}
			}
		}
		catch (Exception)
		{
			return false;
		}
		return false;
	}
}
