using System;
using System.Collections.Generic;
using System.Management;
using System.Runtime.InteropServices;

namespace OpenHardwareMonitor.Hardware.HDD;

internal class WindowsSmart : ISmart
{
	[Flags]
	protected enum AccessMode : uint
	{
		Read = 0x80000000u,
		Write = 0x40000000u,
		Execute = 0x20000000u,
		All = 0x10000000u
	}

	[Flags]
	protected enum ShareMode : uint
	{
		None = 0u,
		Read = 1u,
		Write = 2u,
		Delete = 4u
	}

	protected enum CreationMode : uint
	{
		New = 1u,
		CreateAlways,
		OpenExisting,
		OpenAlways,
		TruncateExisting
	}

	[Flags]
	protected enum FileAttribute : uint
	{
		Readonly = 1u,
		Hidden = 2u,
		System = 4u,
		Directory = 0x10u,
		Archive = 0x20u,
		Device = 0x40u,
		Normal = 0x80u,
		Temporary = 0x100u,
		SparseFile = 0x200u,
		ReparsePoint = 0x400u,
		Compressed = 0x800u,
		Offline = 0x1000u,
		NotContentIndexed = 0x2000u,
		Encrypted = 0x4000u
	}

	protected enum DriveCommand : uint
	{
		GetVersion = 475264u,
		SendDriveCommand = 508036u,
		ReceiveDriveData = 508040u
	}

	protected enum RegisterCommand : byte
	{
		SmartCmd = 176,
		IdCmd = 236
	}

	protected enum RegisterFeature : byte
	{
		SmartReadData = 208,
		SmartReadThresholds,
		SmartAutosave,
		SmartSaveAttr,
		SmartImmediateOffline,
		SmartReadLog,
		SmartWriteLog,
		SmartWriteThresholds,
		SmartEnableOperations,
		SmartDisableOperations,
		SmartStatus,
		SmartAutoOffline
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	protected struct CommandBlockRegisters
	{
		public RegisterFeature Features;

		public byte SectorCount;

		public byte LBALow;

		public byte LBAMid;

		public byte LBAHigh;

		public byte Device;

		public RegisterCommand Command;

		public byte Reserved;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	protected struct DriveCommandParameter
	{
		public uint BufferSize;

		public CommandBlockRegisters Registers;

		public byte DriveNumber;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
		public byte[] Reserved;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	protected struct DriverStatus
	{
		public byte DriverError;

		public byte IDEError;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
		public byte[] Reserved;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	protected struct DriveCommandResult
	{
		public uint BufferSize;

		public DriverStatus DriverStatus;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	protected struct DriveSmartReadDataResult
	{
		public uint BufferSize;

		public DriverStatus DriverStatus;

		public byte Version;

		public byte Reserved;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
		public DriveAttributeValue[] Attributes;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	protected struct DriveSmartReadThresholdsResult
	{
		public uint BufferSize;

		public DriverStatus DriverStatus;

		public byte Version;

		public byte Reserved;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
		public DriveThresholdValue[] Thresholds;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	protected struct Identify
	{
		public ushort GeneralConfiguration;

		public ushort NumberOfCylinders;

		public ushort Reserved;

		public ushort NumberOfHeads;

		public ushort UnformattedBytesPerTrack;

		public ushort UnformattedBytesPerSector;

		public ushort SectorsPerTrack;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public ushort[] VendorUnique;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
		public byte[] SerialNumber;

		public ushort BufferType;

		public ushort BufferSectorSize;

		public ushort NumberOfEccBytes;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public byte[] FirmwareRevision;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
		public byte[] ModelNumber;

		public ushort MoreVendorUnique;

		public ushort DoubleWordIo;

		public ushort Capabilities;

		public ushort MoreReserved;

		public ushort PioCycleTimingMode;

		public ushort DmaCycleTimingMode;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 406)]
		public byte[] More;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	protected struct DriveIdentifyResult
	{
		public uint BufferSize;

		public DriverStatus DriverStatus;

		public Identify Identify;
	}

	protected static class NativeMethods
	{
		private const string KERNEL = "kernel32.dll";

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		public static extern IntPtr CreateFile(string fileName, AccessMode desiredAccess, ShareMode shareMode, IntPtr securityAttributes, CreationMode creationDisposition, FileAttribute flagsAndAttributes, IntPtr templateFilehandle);

		[DllImport("kernel32.dll")]
		public static extern int CloseHandle(IntPtr handle);

		[DllImport("kernel32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DeviceIoControl(IntPtr handle, DriveCommand command, ref DriveCommandParameter parameter, int parameterSize, out DriveSmartReadDataResult result, int resultSize, out uint bytesReturned, IntPtr overlapped);

		[DllImport("kernel32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DeviceIoControl(IntPtr handle, DriveCommand command, ref DriveCommandParameter parameter, int parameterSize, out DriveSmartReadThresholdsResult result, int resultSize, out uint bytesReturned, IntPtr overlapped);

		[DllImport("kernel32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DeviceIoControl(IntPtr handle, DriveCommand command, ref DriveCommandParameter parameter, int parameterSize, out DriveCommandResult result, int resultSize, out uint bytesReturned, IntPtr overlapped);

		[DllImport("kernel32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DeviceIoControl(IntPtr handle, DriveCommand command, ref DriveCommandParameter parameter, int parameterSize, out DriveIdentifyResult result, int resultSize, out uint bytesReturned, IntPtr overlapped);
	}

	private const byte SMART_LBA_MID = 79;

	private const byte SMART_LBA_HI = 194;

	private const int MAX_DRIVE_ATTRIBUTES = 512;

	public IntPtr InvalidHandle => (IntPtr)(-1);

	public IntPtr OpenDrive(int driveNumber)
	{
		return NativeMethods.CreateFile("\\\\.\\PhysicalDrive" + driveNumber, AccessMode.Read | AccessMode.Write, ShareMode.Read | ShareMode.Write, IntPtr.Zero, CreationMode.OpenExisting, FileAttribute.Device, IntPtr.Zero);
	}

	public bool EnableSmart(IntPtr handle, int driveNumber)
	{
		DriveCommandParameter parameter = new DriveCommandParameter
		{
			DriveNumber = (byte)driveNumber,
			Registers = 
			{
				Features = RegisterFeature.SmartEnableOperations,
				LBAMid = 79,
				LBAHigh = 194,
				Command = RegisterCommand.SmartCmd
			}
		};
		DriveCommandResult result;
		uint bytesReturned;
		return NativeMethods.DeviceIoControl(handle, DriveCommand.SendDriveCommand, ref parameter, Marshal.SizeOf(typeof(DriveCommandParameter)), out result, Marshal.SizeOf(typeof(DriveCommandResult)), out bytesReturned, IntPtr.Zero);
	}

	public DriveAttributeValue[] ReadSmartData(IntPtr handle, int driveNumber)
	{
		DriveCommandParameter parameter = new DriveCommandParameter
		{
			DriveNumber = (byte)driveNumber,
			Registers = 
			{
				Features = RegisterFeature.SmartReadData,
				LBAMid = 79,
				LBAHigh = 194,
				Command = RegisterCommand.SmartCmd
			}
		};
		if (!NativeMethods.DeviceIoControl(handle, DriveCommand.ReceiveDriveData, ref parameter, Marshal.SizeOf((object)parameter), out DriveSmartReadDataResult result, Marshal.SizeOf(typeof(DriveSmartReadDataResult)), out uint _, IntPtr.Zero))
		{
			return new DriveAttributeValue[0];
		}
		return result.Attributes;
	}

	public DriveThresholdValue[] ReadSmartThresholds(IntPtr handle, int driveNumber)
	{
		DriveCommandParameter parameter = default;
		uint bytesReturned = 0u;
		parameter.DriveNumber = (byte)driveNumber;
		parameter.Registers.Features = RegisterFeature.SmartReadThresholds;
		parameter.Registers.LBAMid = 79;
		parameter.Registers.LBAHigh = 194;
		parameter.Registers.Command = RegisterCommand.SmartCmd;
		if (!NativeMethods.DeviceIoControl(handle, DriveCommand.ReceiveDriveData, ref parameter, Marshal.SizeOf((object)parameter), out DriveSmartReadThresholdsResult result, Marshal.SizeOf(typeof(DriveSmartReadThresholdsResult)), out bytesReturned, IntPtr.Zero))
		{
			return new DriveThresholdValue[0];
		}
		return result.Thresholds;
	}

	private string GetString(byte[] bytes)
	{
		char[] array = new char[bytes.Length];
		for (int i = 0; i < bytes.Length; i += 2)
		{
			array[i] = (char)bytes[i + 1];
			array[i + 1] = (char)bytes[i];
		}
		return new string(array).Trim(' ', '\0');
	}

	public bool ReadNameAndFirmwareRevision(IntPtr handle, int driveNumber, out string name, out string firmwareRevision)
	{
		DriveCommandParameter parameter = new DriveCommandParameter
		{
			DriveNumber = (byte)driveNumber,
			Registers = 
			{
				Command = RegisterCommand.IdCmd
			}
		};
		if (!NativeMethods.DeviceIoControl(handle, DriveCommand.ReceiveDriveData, ref parameter, Marshal.SizeOf((object)parameter), out DriveIdentifyResult result, Marshal.SizeOf(typeof(DriveIdentifyResult)), out uint _, IntPtr.Zero))
		{
			name = null;
			firmwareRevision = null;
			return false;
		}
		name = GetString(result.Identify.ModelNumber);
		firmwareRevision = GetString(result.Identify.FirmwareRevision);
		return true;
	}

	public void CloseHandle(IntPtr handle)
	{
		NativeMethods.CloseHandle(handle);
	}

	public string[] GetLogicalDrives(int driveIndex)
	{
		List<string> list = new List<string>();
		try
		{
			using ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_DiskPartition WHERE DiskIndex = " + driveIndex);
			using ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
			foreach (ManagementObject item in managementObjectCollection)
			{
				using ManagementObjectCollection managementObjectCollection2 = item.GetRelated("Win32_LogicalDisk");
				foreach (ManagementBaseObject item2 in managementObjectCollection2)
				{
					list.Add(((string)item2["Name"]).TrimEnd(':'));
				}
			}
		}
		catch
		{
		}
		return list.ToArray();
	}
}
