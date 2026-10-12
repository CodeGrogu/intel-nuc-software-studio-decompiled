using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace OpenHardwareMonitor.Hardware;

public static class OperatingSystem
{
	public static bool IsUnix { get; }

	public static bool Is64BitOperatingSystem { get; }

	static OperatingSystem()
	{
		int platform = (int)Environment.OSVersion.Platform;
		IsUnix = platform == 4 || platform == 6 || platform == 128;
		Is64BitOperatingSystem = GetIs64BitOperatingSystem();
	}

	private static bool GetIs64BitOperatingSystem()
	{
		if (IntPtr.Size == 8)
		{
			return true;
		}
		try
		{
			bool wow64Process;
			return IsWow64Process(Process.GetCurrentProcess().Handle, out wow64Process) & wow64Process;
		}
		catch (EntryPointNotFoundException)
		{
			return false;
		}
	}

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool IsWow64Process(IntPtr hProcess, out bool wow64Process);
}
