using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading;

namespace OpenHardwareMonitor.Hardware;

internal static class Ring0
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	private struct WrmsrInput
	{
		public uint Register;

		public ulong Value;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	private struct WriteIoPortInput
	{
		public uint PortNumber;

		public byte Value;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	private struct ReadPciConfigInput
	{
		public uint PciAddress;

		public uint RegAddress;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	private struct WritePciConfigInput
	{
		public uint PciAddress;

		public uint RegAddress;

		public uint Value;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	private struct ReadMemoryInput
	{
		public ulong address;

		public uint unitSize;

		public uint count;
	}

	private static KernelDriver driver;

	private static string fileName;

	private static Mutex isaBusMutex;

	private static Mutex pciBusMutex;

	private static readonly StringBuilder report = new StringBuilder();

	private const uint OLS_TYPE = 40000u;

	private static IOControlCode IOCTL_OLS_GET_REFCOUNT = new IOControlCode(40000u, 2049u, IOControlCode.Access.Any);

	private static IOControlCode IOCTL_OLS_GET_DRIVER_VERSION = new IOControlCode(40000u, 2048u, IOControlCode.Access.Any);

	private static IOControlCode IOCTL_OLS_READ_MSR = new IOControlCode(40000u, 2081u, IOControlCode.Access.Any);

	private static IOControlCode IOCTL_OLS_WRITE_MSR = new IOControlCode(40000u, 2082u, IOControlCode.Access.Any);

	private static IOControlCode IOCTL_OLS_READ_IO_PORT_BYTE = new IOControlCode(40000u, 2099u, IOControlCode.Access.Read);

	private static IOControlCode IOCTL_OLS_WRITE_IO_PORT_BYTE = new IOControlCode(40000u, 2102u, IOControlCode.Access.Write);

	private static IOControlCode IOCTL_OLS_READ_PCI_CONFIG = new IOControlCode(40000u, 2129u, IOControlCode.Access.Read);

	private static IOControlCode IOCTL_OLS_WRITE_PCI_CONFIG = new IOControlCode(40000u, 2130u, IOControlCode.Access.Write);

	private static IOControlCode IOCTL_OLS_READ_MEMORY = new IOControlCode(40000u, 2113u, IOControlCode.Access.Read);

	public const uint InvalidPciAddress = uint.MaxValue;

	public static bool IsOpen => driver != null;

	private static Assembly GetAssembly()
	{
		return typeof(Ring0).Assembly;
	}

	private static string GetTempFileName()
	{
		string location = GetAssembly().Location;
		if (!string.IsNullOrEmpty(location))
		{
			try
			{
				string text = Path.ChangeExtension(location, ".sys");
				using (File.Create(text))
				{
					return text;
				}
			}
			catch (Exception)
			{
			}
		}
		try
		{
			return Path.GetTempFileName();
		}
		catch (IOException)
		{
		}
		catch (UnauthorizedAccessException)
		{
		}
		catch (NotSupportedException)
		{
		}
		return null;
	}

	private static bool ExtractDriver(string fileName)
	{
		string text = "OpenHardwareMonitor.Hardware." + (OperatingSystem.Is64BitOperatingSystem ? "WinRing0x64.sys" : "WinRing0.sys");
		string[] manifestResourceNames = GetAssembly().GetManifestResourceNames();
		byte[] array = null;
		for (int i = 0; i < manifestResourceNames.Length; i++)
		{
			if (manifestResourceNames[i].Replace('\\', '.') == text)
			{
				using Stream stream = GetAssembly().GetManifestResourceStream(manifestResourceNames[i]);
				array = new byte[stream.Length];
				stream.Read(array, 0, array.Length);
			}
		}
		if (array == null)
		{
			return false;
		}
		try
		{
			using FileStream fileStream = new FileStream(fileName, FileMode.Create);
			fileStream.Write(array, 0, array.Length);
			fileStream.Flush();
		}
		catch (IOException)
		{
			return false;
		}
		for (int j = 0; j < 20; j++)
		{
			try
			{
				if (File.Exists(fileName) && new FileInfo(fileName).Length == array.Length)
				{
					return true;
				}
				Thread.Sleep(100);
			}
			catch (IOException)
			{
				Thread.Sleep(10);
			}
		}
		return false;
	}

	public static void Open()
	{
		if (OperatingSystem.IsUnix || driver != null)
		{
			return;
		}
		report.Length = 0;
		driver = new KernelDriver("WinRing0_1_2_0");
		driver.Open();
		if (!driver.IsOpen)
		{
			fileName = GetTempFileName();
			if (fileName != null && ExtractDriver(fileName))
			{
				if (driver.Install(fileName, out var errorMessage))
				{
					driver.Open();
					if (!driver.IsOpen)
					{
						driver.Delete();
						report.AppendLine("Status: Opening driver failed after install");
					}
				}
				else
				{
					string text = errorMessage;
					driver.Delete();
					Thread.Sleep(2000);
					if (driver.Install(fileName, out var errorMessage2))
					{
						driver.Open();
						if (!driver.IsOpen)
						{
							driver.Delete();
							report.AppendLine("Status: Opening driver failed after reinstall");
						}
					}
					else
					{
						report.AppendLine("Status: Installing driver \"" + fileName + "\" failed" + (File.Exists(fileName) ? " and file exists" : ""));
						report.AppendLine("First Exception: " + text);
						report.AppendLine("Second Exception: " + errorMessage2);
					}
				}
			}
			else
			{
				report.AppendLine("Status: Extracting driver failed");
			}
			try
			{
				if (File.Exists(fileName))
				{
					File.Delete(fileName);
				}
				fileName = null;
			}
			catch (IOException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
		}
		if (!driver.IsOpen)
		{
			driver = null;
		}
		string name = "Global\\Access_ISABUS.HTP.Method";
		try
		{
			isaBusMutex = new Mutex(initiallyOwned: false, name);
		}
		catch (UnauthorizedAccessException)
		{
			try
			{
				isaBusMutex = Mutex.OpenExisting(name, MutexRights.Synchronize);
			}
			catch
			{
			}
		}
		string name2 = "Global\\Access_PCI";
		try
		{
			pciBusMutex = new Mutex(initiallyOwned: false, name2);
		}
		catch (UnauthorizedAccessException)
		{
			try
			{
				pciBusMutex = Mutex.OpenExisting(name2, MutexRights.Synchronize);
			}
			catch
			{
			}
		}
	}

	public static void Close()
	{
		if (driver == null)
		{
			return;
		}
		uint outBuffer = 0u;
		driver.DeviceIOControl(IOCTL_OLS_GET_REFCOUNT, null, ref outBuffer);
		driver.Close();
		if (outBuffer <= 1)
		{
			driver.Delete();
		}
		driver = null;
		if (isaBusMutex != null)
		{
			isaBusMutex.Close();
			isaBusMutex = null;
		}
		if (pciBusMutex != null)
		{
			pciBusMutex.Close();
			pciBusMutex = null;
		}
		if (fileName == null || !File.Exists(fileName))
		{
			return;
		}
		try
		{
			File.Delete(fileName);
			fileName = null;
		}
		catch (IOException)
		{
		}
		catch (UnauthorizedAccessException)
		{
		}
	}

	public static string GetReport()
	{
		if (report.Length > 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Ring0");
			stringBuilder.AppendLine();
			stringBuilder.Append(report);
			stringBuilder.AppendLine();
			return stringBuilder.ToString();
		}
		return null;
	}

	public static bool WaitIsaBusMutex(int millisecondsTimeout)
	{
		if (isaBusMutex == null)
		{
			return true;
		}
		try
		{
			return isaBusMutex.WaitOne(millisecondsTimeout, exitContext: false);
		}
		catch (AbandonedMutexException)
		{
			return true;
		}
		catch (InvalidOperationException)
		{
			return false;
		}
	}

	public static void ReleaseIsaBusMutex()
	{
		if (isaBusMutex != null)
		{
			isaBusMutex.ReleaseMutex();
		}
	}

	public static bool WaitPciBusMutex(int millisecondsTimeout)
	{
		if (pciBusMutex == null)
		{
			return true;
		}
		try
		{
			return pciBusMutex.WaitOne(millisecondsTimeout, exitContext: false);
		}
		catch (AbandonedMutexException)
		{
			return true;
		}
		catch (InvalidOperationException)
		{
			return false;
		}
	}

	public static void ReleasePciBusMutex()
	{
		if (pciBusMutex != null)
		{
			pciBusMutex.ReleaseMutex();
		}
	}

	public static bool Rdmsr(uint index, out uint eax, out uint edx)
	{
		if (driver == null)
		{
			eax = 0u;
			edx = 0u;
			return false;
		}
		ulong outBuffer = 0uL;
		bool result = driver.DeviceIOControl(IOCTL_OLS_READ_MSR, index, ref outBuffer);
		edx = (uint)((outBuffer >> 32) & 0xFFFFFFFFu);
		eax = (uint)(outBuffer & 0xFFFFFFFFu);
		return result;
	}

	public static bool RdmsrTx(uint index, out uint eax, out uint edx, GroupAffinity affinity)
	{
		GroupAffinity affinity2 = ThreadAffinity.Set(affinity);
		bool result = Rdmsr(index, out eax, out edx);
		ThreadAffinity.Set(affinity2);
		return result;
	}

	public static bool Wrmsr(uint index, uint eax, uint edx)
	{
		if (driver == null)
		{
			return false;
		}
		WrmsrInput wrmsrInput = new WrmsrInput
		{
			Register = index,
			Value = (((ulong)edx << 32) | eax)
		};
		return driver.DeviceIOControl(IOCTL_OLS_WRITE_MSR, wrmsrInput);
	}

	public static byte ReadIoPort(uint port)
	{
		if (driver == null)
		{
			return 0;
		}
		uint outBuffer = 0u;
		driver.DeviceIOControl(IOCTL_OLS_READ_IO_PORT_BYTE, port, ref outBuffer);
		return (byte)(outBuffer & 0xFF);
	}

	public static void WriteIoPort(uint port, byte value)
	{
		if (driver != null)
		{
			WriteIoPortInput writeIoPortInput = new WriteIoPortInput
			{
				PortNumber = port,
				Value = value
			};
			driver.DeviceIOControl(IOCTL_OLS_WRITE_IO_PORT_BYTE, writeIoPortInput);
		}
	}

	public static uint GetPciAddress(byte bus, byte device, byte function)
	{
		return (uint)(((bus & 0xFF) << 8) | ((device & 0x1F) << 3) | (function & 7));
	}

	public static bool ReadPciConfig(uint pciAddress, uint regAddress, out uint value)
	{
		if (driver == null || (regAddress & 3) != 0)
		{
			value = 0u;
			return false;
		}
		ReadPciConfigInput readPciConfigInput = new ReadPciConfigInput
		{
			PciAddress = pciAddress,
			RegAddress = regAddress
		};
		value = 0u;
		return driver.DeviceIOControl(IOCTL_OLS_READ_PCI_CONFIG, readPciConfigInput, ref value);
	}

	public static bool WritePciConfig(uint pciAddress, uint regAddress, uint value)
	{
		if (driver == null || (regAddress & 3) != 0)
		{
			return false;
		}
		WritePciConfigInput writePciConfigInput = new WritePciConfigInput
		{
			PciAddress = pciAddress,
			RegAddress = regAddress,
			Value = value
		};
		return driver.DeviceIOControl(IOCTL_OLS_WRITE_PCI_CONFIG, writePciConfigInput);
	}

	public static bool ReadMemory<T>(ulong address, ref T buffer)
	{
		if (driver == null)
		{
			return false;
		}
		ReadMemoryInput readMemoryInput = new ReadMemoryInput
		{
			address = address,
			unitSize = 1u,
			count = (uint)Marshal.SizeOf((object)buffer)
		};
		return driver.DeviceIOControl(IOCTL_OLS_READ_MEMORY, readMemoryInput, ref buffer);
	}
}
