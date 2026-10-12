using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace OpenHardwareMonitor.Hardware;

internal static class Opcode
{
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	public delegate ulong RdtscDelegate();

	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	public delegate bool CpuidDelegate(uint index, uint ecxValue, out uint eax, out uint ebx, out uint ecx, out uint edx);

	[Flags]
	public enum AllocationType : uint
	{
		COMMIT = 0x1000u,
		RESERVE = 0x2000u,
		RESET = 0x80000u,
		LARGE_PAGES = 0x20000000u,
		PHYSICAL = 0x400000u,
		TOP_DOWN = 0x100000u,
		WRITE_WATCH = 0x200000u
	}

	[Flags]
	public enum MemoryProtection : uint
	{
		EXECUTE = 0x10u,
		EXECUTE_READ = 0x20u,
		EXECUTE_READWRITE = 0x40u,
		EXECUTE_WRITECOPY = 0x80u,
		NOACCESS = 1u,
		READONLY = 2u,
		READWRITE = 4u,
		WRITECOPY = 8u,
		GUARD = 0x100u,
		NOCACHE = 0x200u,
		WRITECOMBINE = 0x400u
	}

	[Flags]
	public enum FreeType
	{
		DECOMMIT = 0x4000,
		RELEASE = 0x8000
	}

	private static class NativeMethods
	{
		private const string KERNEL = "kernel32.dll";

		[DllImport("kernel32.dll")]
		public static extern IntPtr VirtualAlloc(IntPtr lpAddress, UIntPtr dwSize, AllocationType flAllocationType, MemoryProtection flProtect);

		[DllImport("kernel32.dll")]
		public static extern bool VirtualFree(IntPtr lpAddress, UIntPtr dwSize, FreeType dwFreeType);
	}

	private static IntPtr codeBuffer;

	private static ulong size;

	public static RdtscDelegate Rdtsc;

	private static readonly byte[] RDTSC_32 = new byte[3] { 15, 49, 195 };

	private static readonly byte[] RDTSC_64 = new byte[10] { 15, 49, 72, 193, 226, 32, 72, 11, 194, 195 };

	public static CpuidDelegate Cpuid;

	private static readonly byte[] CPUID_32 = new byte[68]
	{
		85, 139, 236, 131, 236, 16, 139, 69, 8, 139,
		77, 12, 83, 15, 162, 86, 141, 117, 240, 137,
		6, 139, 69, 16, 137, 94, 4, 137, 78, 8,
		137, 86, 12, 139, 77, 240, 137, 8, 139, 69,
		20, 139, 77, 244, 137, 8, 139, 69, 24, 139,
		77, 248, 137, 8, 139, 69, 28, 139, 77, 252,
		94, 137, 8, 91, 201, 194, 24, 0
	};

	private static readonly byte[] CPUID_64_WINDOWS = new byte[37]
	{
		72, 137, 92, 36, 8, 139, 193, 139, 202, 15,
		162, 65, 137, 0, 72, 139, 68, 36, 40, 65,
		137, 25, 72, 139, 92, 36, 8, 137, 8, 72,
		139, 68, 36, 48, 137, 16, 195
	};

	private static readonly byte[] CPUID_64_LINUX = new byte[27]
	{
		73, 137, 210, 73, 137, 203, 83, 137, 248, 137,
		241, 15, 162, 65, 137, 2, 65, 137, 27, 65,
		137, 8, 65, 137, 17, 91, 195
	};

	public static void Open()
	{
		byte[] array;
		byte[] array2;
		if (IntPtr.Size == 4)
		{
			array = RDTSC_32;
			array2 = CPUID_32;
		}
		else
		{
			array = RDTSC_64;
			array2 = ((!OperatingSystem.IsUnix) ? CPUID_64_WINDOWS : CPUID_64_LINUX);
		}
		size = (ulong)(array.Length + array2.Length);
		if (OperatingSystem.IsUnix)
		{
			Assembly assembly = Assembly.Load("Mono.Posix, Version=2.0.0.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756");
			MethodInfo method = assembly.GetType("Mono.Unix.Native.Syscall").GetMethod("mmap");
			Type type = assembly.GetType("Mono.Unix.Native.MmapProts");
			object obj = Enum.ToObject(type, (int)type.GetField("PROT_READ").GetValue(null) | (int)type.GetField("PROT_WRITE").GetValue(null) | (int)type.GetField("PROT_EXEC").GetValue(null));
			Type type2 = assembly.GetType("Mono.Unix.Native.MmapFlags");
			object obj2 = Enum.ToObject(type2, (int)type2.GetField("MAP_ANONYMOUS").GetValue(null) | (int)type2.GetField("MAP_PRIVATE").GetValue(null));
			codeBuffer = (IntPtr)method.Invoke(null, new object[6]
			{
				IntPtr.Zero,
				size,
				obj,
				obj2,
				-1,
				0
			});
		}
		else
		{
			codeBuffer = NativeMethods.VirtualAlloc(IntPtr.Zero, (UIntPtr)size, AllocationType.COMMIT | AllocationType.RESERVE, MemoryProtection.EXECUTE_READWRITE);
		}
		Marshal.Copy(array, 0, codeBuffer, array.Length);
		Rdtsc = Marshal.GetDelegateForFunctionPointer(codeBuffer, typeof(RdtscDelegate)) as RdtscDelegate;
		IntPtr intPtr = (IntPtr)((long)codeBuffer + array.Length);
		Marshal.Copy(array2, 0, intPtr, array2.Length);
		Cpuid = Marshal.GetDelegateForFunctionPointer(intPtr, typeof(CpuidDelegate)) as CpuidDelegate;
	}

	public static void Close()
	{
		Rdtsc = null;
		Cpuid = null;
		if (OperatingSystem.IsUnix)
		{
			Assembly.Load("Mono.Posix, Version=2.0.0.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756").GetType("Mono.Unix.Native.Syscall").GetMethod("munmap")
				.Invoke(null, new object[2] { codeBuffer, size });
		}
		else
		{
			NativeMethods.VirtualFree(codeBuffer, UIntPtr.Zero, FreeType.RELEASE);
		}
	}

	public static bool CpuidTx(uint index, uint ecxValue, out uint eax, out uint ebx, out uint ecx, out uint edx, GroupAffinity affinity)
	{
		GroupAffinity groupAffinity = ThreadAffinity.Set(affinity);
		if (groupAffinity == GroupAffinity.Undefined)
		{
			eax = (ebx = (ecx = (edx = 0u)));
			return false;
		}
		Cpuid(index, ecxValue, out eax, out ebx, out ecx, out edx);
		ThreadAffinity.Set(groupAffinity);
		return true;
	}
}
