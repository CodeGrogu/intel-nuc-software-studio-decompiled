using System;
using System.Runtime.InteropServices;

namespace OpenHardwareMonitor.Hardware;

internal static class ThreadAffinity
{
	private static class NativeMethods
	{
		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		public struct GROUP_AFFINITY
		{
			public UIntPtr Mask;

			[MarshalAs(UnmanagedType.U2)]
			public ushort Group;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U2)]
			public ushort[] Reserved;
		}

		private const string KERNEL = "kernel32.dll";

		private const string LIBC = "libc";

		[DllImport("kernel32.dll")]
		public static extern UIntPtr SetThreadAffinityMask(IntPtr handle, UIntPtr mask);

		[DllImport("kernel32.dll")]
		public static extern IntPtr GetCurrentThread();

		[DllImport("kernel32.dll")]
		public static extern ushort GetActiveProcessorGroupCount();

		[DllImport("kernel32.dll")]
		public static extern bool SetThreadGroupAffinity(IntPtr thread, ref GROUP_AFFINITY groupAffinity, out GROUP_AFFINITY previousGroupAffinity);

		[DllImport("libc")]
		public static extern int sched_getaffinity(int pid, IntPtr maskSize, ref ulong mask);

		[DllImport("libc")]
		public static extern int sched_setaffinity(int pid, IntPtr maskSize, ref ulong mask);
	}

	public static int ProcessorGroupCount { get; }

	static ThreadAffinity()
	{
		ProcessorGroupCount = GetProcessorGroupCount();
	}

	private static int GetProcessorGroupCount()
	{
		if (OperatingSystem.IsUnix)
		{
			return 1;
		}
		try
		{
			return NativeMethods.GetActiveProcessorGroupCount();
		}
		catch
		{
			return 1;
		}
	}

	public static bool IsValid(GroupAffinity affinity)
	{
		if (OperatingSystem.IsUnix && affinity.Group > 0)
		{
			return false;
		}
		try
		{
			GroupAffinity groupAffinity = Set(affinity);
			if (groupAffinity == GroupAffinity.Undefined)
			{
				return false;
			}
			Set(groupAffinity);
			return true;
		}
		catch
		{
			return false;
		}
	}

	public static GroupAffinity Set(GroupAffinity affinity)
	{
		if (affinity == GroupAffinity.Undefined)
		{
			return GroupAffinity.Undefined;
		}
		if (OperatingSystem.IsUnix)
		{
			if (affinity.Group > 0)
			{
				throw new ArgumentOutOfRangeException("affinity.Group");
			}
			ulong mask = 0uL;
			if (NativeMethods.sched_getaffinity(0, (IntPtr)8, ref mask) != 0)
			{
				return GroupAffinity.Undefined;
			}
			ulong mask2 = affinity.Mask;
			if (NativeMethods.sched_setaffinity(0, (IntPtr)8, ref mask2) != 0)
			{
				return GroupAffinity.Undefined;
			}
			return new GroupAffinity(0, mask);
		}
		UIntPtr mask3;
		try
		{
			mask3 = (UIntPtr)affinity.Mask;
		}
		catch (OverflowException)
		{
			throw new ArgumentOutOfRangeException("affinity.Mask");
		}
		NativeMethods.GROUP_AFFINITY groupAffinity = new NativeMethods.GROUP_AFFINITY
		{
			Group = affinity.Group,
			Mask = mask3
		};
		IntPtr currentThread = NativeMethods.GetCurrentThread();
		try
		{
			if (NativeMethods.SetThreadGroupAffinity(currentThread, ref groupAffinity, out var previousGroupAffinity))
			{
				return new GroupAffinity(previousGroupAffinity.Group, (ulong)previousGroupAffinity.Mask);
			}
			return GroupAffinity.Undefined;
		}
		catch (EntryPointNotFoundException)
		{
			if (affinity.Group > 0)
			{
				throw new ArgumentOutOfRangeException("affinity.Group");
			}
			ulong mask4 = (ulong)NativeMethods.SetThreadAffinityMask(currentThread, mask3);
			return new GroupAffinity(0, mask4);
		}
	}
}
