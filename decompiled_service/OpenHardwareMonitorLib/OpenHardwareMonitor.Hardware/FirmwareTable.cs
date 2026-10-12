using System;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenHardwareMonitor.Hardware;

internal static class FirmwareTable
{
	public enum Provider
	{
		ACPI = 1094930505,
		FIRM = 1179210317,
		RSMB = 1381190978
	}

	private static class NativeMethods
	{
		private const string KERNEL = "kernel32.dll";

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern int EnumSystemFirmwareTables(Provider firmwareTableProviderSignature, IntPtr firmwareTableBuffer, int bufferSize);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern int GetSystemFirmwareTable(Provider firmwareTableProviderSignature, int firmwareTableID, IntPtr firmwareTableBuffer, int bufferSize);
	}

	public static byte[] GetTable(Provider provider, string table)
	{
		int table2 = (int)(((uint)table[3] << 24) | ((uint)table[2] << 16) | ((uint)table[1] << 8) | table[0]);
		return GetTable(provider, table2);
	}

	public static byte[] GetTable(Provider provider, int table)
	{
		int systemFirmwareTable;
		try
		{
			systemFirmwareTable = NativeMethods.GetSystemFirmwareTable(provider, table, IntPtr.Zero, 0);
		}
		catch (DllNotFoundException)
		{
			return null;
		}
		catch (EntryPointNotFoundException)
		{
			return null;
		}
		if (systemFirmwareTable <= 0)
		{
			return null;
		}
		IntPtr intPtr = Marshal.AllocHGlobal(systemFirmwareTable);
		NativeMethods.GetSystemFirmwareTable(provider, table, intPtr, systemFirmwareTable);
		if (Marshal.GetLastWin32Error() != 0)
		{
			return null;
		}
		byte[] array = new byte[systemFirmwareTable];
		Marshal.Copy(intPtr, array, 0, systemFirmwareTable);
		Marshal.FreeHGlobal(intPtr);
		return array;
	}

	public static string[] EnumerateTables(Provider provider)
	{
		int num;
		try
		{
			num = NativeMethods.EnumSystemFirmwareTables(provider, IntPtr.Zero, 0);
		}
		catch (DllNotFoundException)
		{
			return null;
		}
		catch (EntryPointNotFoundException)
		{
			return null;
		}
		IntPtr intPtr = Marshal.AllocHGlobal(num);
		NativeMethods.EnumSystemFirmwareTables(provider, intPtr, num);
		byte[] array = new byte[num];
		Marshal.Copy(intPtr, array, 0, num);
		Marshal.FreeHGlobal(intPtr);
		string[] array2 = new string[num / 4];
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i] = Encoding.ASCII.GetString(array, 4 * i, 4);
		}
		return array2;
	}
}
