using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using Microsoft.Win32.SafeHandles;

namespace OpenHardwareMonitor.Hardware;

internal class KernelDriver
{
	private enum ServiceAccessRights : uint
	{
		SERVICE_ALL_ACCESS = 983551u
	}

	private enum ServiceControlManagerAccessRights : uint
	{
		SC_MANAGER_ALL_ACCESS = 983103u
	}

	private enum ServiceType : uint
	{
		SERVICE_KERNEL_DRIVER = 1u,
		SERVICE_FILE_SYSTEM_DRIVER
	}

	private enum StartType : uint
	{
		SERVICE_BOOT_START,
		SERVICE_SYSTEM_START,
		SERVICE_AUTO_START,
		SERVICE_DEMAND_START,
		SERVICE_DISABLED
	}

	private enum ErrorControl : uint
	{
		SERVICE_ERROR_IGNORE,
		SERVICE_ERROR_NORMAL,
		SERVICE_ERROR_SEVERE,
		SERVICE_ERROR_CRITICAL
	}

	private enum ServiceControl : uint
	{
		SERVICE_CONTROL_STOP = 1u,
		SERVICE_CONTROL_PAUSE,
		SERVICE_CONTROL_CONTINUE,
		SERVICE_CONTROL_INTERROGATE,
		SERVICE_CONTROL_SHUTDOWN,
		SERVICE_CONTROL_PARAMCHANGE,
		SERVICE_CONTROL_NETBINDADD,
		SERVICE_CONTROL_NETBINDREMOVE,
		SERVICE_CONTROL_NETBINDENABLE,
		SERVICE_CONTROL_NETBINDDISABLE,
		SERVICE_CONTROL_DEVICEEVENT,
		SERVICE_CONTROL_HARDWAREPROFILECHANGE,
		SERVICE_CONTROL_POWEREVENT,
		SERVICE_CONTROL_SESSIONCHANGE
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	private struct ServiceStatus
	{
		public uint dwServiceType;

		public uint dwCurrentState;

		public uint dwControlsAccepted;

		public uint dwWin32ExitCode;

		public uint dwServiceSpecificExitCode;

		public uint dwCheckPoint;

		public uint dwWaitHint;
	}

	private enum FileAccess : uint
	{
		GENERIC_READ = 2147483648u,
		GENERIC_WRITE = 1073741824u
	}

	private enum CreationDisposition : uint
	{
		CREATE_NEW = 1u,
		CREATE_ALWAYS,
		OPEN_EXISTING,
		OPEN_ALWAYS,
		TRUNCATE_EXISTING
	}

	private enum FileAttributes : uint
	{
		FILE_ATTRIBUTE_NORMAL = 0x80u
	}

	private static class NativeMethods
	{
		private const string ADVAPI = "advapi32.dll";

		private const string KERNEL = "kernel32.dll";

		[DllImport("advapi32.dll")]
		public static extern IntPtr OpenSCManager(string machineName, string databaseName, ServiceControlManagerAccessRights dwAccess);

		[DllImport("advapi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CloseServiceHandle(IntPtr hSCObject);

		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern IntPtr CreateService(IntPtr hSCManager, string lpServiceName, string lpDisplayName, ServiceAccessRights dwDesiredAccess, ServiceType dwServiceType, StartType dwStartType, ErrorControl dwErrorControl, string lpBinaryPathName, string lpLoadOrderGroup, string lpdwTagId, string lpDependencies, string lpServiceStartName, string lpPassword);

		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern IntPtr OpenService(IntPtr hSCManager, string lpServiceName, ServiceAccessRights dwDesiredAccess);

		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DeleteService(IntPtr hService);

		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool StartService(IntPtr hService, uint dwNumServiceArgs, string[] lpServiceArgVectors);

		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ControlService(IntPtr hService, ServiceControl dwControl, ref ServiceStatus lpServiceStatus);

		[DllImport("kernel32.dll")]
		public static extern bool DeviceIoControl(SafeFileHandle device, IOControlCode ioControlCode, [In][MarshalAs(UnmanagedType.AsAny)] object inBuffer, uint inBufferSize, [Out][MarshalAs(UnmanagedType.AsAny)] object outBuffer, uint nOutBufferSize, out uint bytesReturned, IntPtr overlapped);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr CreateFile(string lpFileName, FileAccess dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, CreationDisposition dwCreationDisposition, FileAttributes dwFlagsAndAttributes, IntPtr hTemplateFile);
	}

	private string id;

	private SafeFileHandle device;

	private const int ERROR_SERVICE_EXISTS = -2147023823;

	private const int ERROR_SERVICE_ALREADY_RUNNING = -2147023840;

	public bool IsOpen => device != null;

	public KernelDriver(string id)
	{
		this.id = id;
	}

	public bool Install(string path, out string errorMessage)
	{
		IntPtr intPtr = NativeMethods.OpenSCManager(null, null, ServiceControlManagerAccessRights.SC_MANAGER_ALL_ACCESS);
		if (intPtr == IntPtr.Zero)
		{
			errorMessage = "OpenSCManager returned zero.";
			return false;
		}
		IntPtr intPtr2 = NativeMethods.CreateService(intPtr, id, id, ServiceAccessRights.SERVICE_ALL_ACCESS, ServiceType.SERVICE_KERNEL_DRIVER, StartType.SERVICE_DEMAND_START, ErrorControl.SERVICE_ERROR_NORMAL, path, null, null, null, null, null);
		if (intPtr2 == IntPtr.Zero)
		{
			if (Marshal.GetHRForLastWin32Error() == -2147023823)
			{
				errorMessage = "Service already exists";
				return false;
			}
			errorMessage = "CreateService returned the error: " + Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error()).Message;
			NativeMethods.CloseServiceHandle(intPtr);
			return false;
		}
		if (!NativeMethods.StartService(intPtr2, 0u, null) && Marshal.GetHRForLastWin32Error() != -2147023840)
		{
			errorMessage = "StartService returned the error: " + Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error()).Message;
			NativeMethods.CloseServiceHandle(intPtr2);
			NativeMethods.CloseServiceHandle(intPtr);
			return false;
		}
		NativeMethods.CloseServiceHandle(intPtr2);
		NativeMethods.CloseServiceHandle(intPtr);
		try
		{
			FileSecurity accessControl = File.GetAccessControl("\\\\.\\" + id);
			accessControl.SetSecurityDescriptorSddlForm("O:BAG:SYD:(A;;FA;;;SY)(A;;FA;;;BA)");
			File.SetAccessControl("\\\\.\\" + id, accessControl);
		}
		catch
		{
		}
		errorMessage = null;
		return true;
	}

	public bool Open()
	{
		device = new SafeFileHandle(NativeMethods.CreateFile("\\\\.\\" + id, (FileAccess)3221225472u, 0u, IntPtr.Zero, CreationDisposition.OPEN_EXISTING, FileAttributes.FILE_ATTRIBUTE_NORMAL, IntPtr.Zero), ownsHandle: true);
		if (device.IsInvalid)
		{
			device.Close();
			device.Dispose();
			device = null;
		}
		return device != null;
	}

	public bool DeviceIOControl(IOControlCode ioControlCode, object inBuffer)
	{
		if (device == null)
		{
			return false;
		}
		uint bytesReturned;
		return NativeMethods.DeviceIoControl(device, ioControlCode, inBuffer, (inBuffer != null) ? ((uint)Marshal.SizeOf(inBuffer)) : 0u, null, 0u, out bytesReturned, IntPtr.Zero);
	}

	public bool DeviceIOControl<T>(IOControlCode ioControlCode, object inBuffer, ref T outBuffer)
	{
		if (device == null)
		{
			return false;
		}
		object obj = outBuffer;
		bool result = NativeMethods.DeviceIoControl(device, ioControlCode, inBuffer, (inBuffer != null) ? ((uint)Marshal.SizeOf(inBuffer)) : 0u, obj, (uint)Marshal.SizeOf(obj), out var _, IntPtr.Zero);
		outBuffer = (T)obj;
		return result;
	}

	public void Close()
	{
		if (device != null)
		{
			device.Close();
			device.Dispose();
			device = null;
		}
	}

	public bool Delete()
	{
		IntPtr intPtr = NativeMethods.OpenSCManager(null, null, ServiceControlManagerAccessRights.SC_MANAGER_ALL_ACCESS);
		if (intPtr == IntPtr.Zero)
		{
			return false;
		}
		IntPtr intPtr2 = NativeMethods.OpenService(intPtr, id, ServiceAccessRights.SERVICE_ALL_ACCESS);
		if (intPtr2 == IntPtr.Zero)
		{
			return true;
		}
		ServiceStatus lpServiceStatus = default;
		NativeMethods.ControlService(intPtr2, ServiceControl.SERVICE_CONTROL_STOP, ref lpServiceStatus);
		NativeMethods.DeleteService(intPtr2);
		NativeMethods.CloseServiceHandle(intPtr2);
		NativeMethods.CloseServiceHandle(intPtr);
		return true;
	}
}
