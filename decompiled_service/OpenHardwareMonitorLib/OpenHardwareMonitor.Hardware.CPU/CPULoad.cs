using System;
using System.Runtime.InteropServices;

namespace OpenHardwareMonitor.Hardware.CPU;

internal class CPULoad
{
	protected struct SystemProcessorPerformanceInformation
	{
		public long IdleTime;

		public long KernelTime;

		public long UserTime;

		public long Reserved0;

		public long Reserved1;

		public ulong Reserved2;
	}

	protected enum SystemInformationClass
	{
		SystemBasicInformation = 0,
		SystemCpuInformation = 1,
		SystemPerformanceInformation = 2,
		SystemTimeOfDayInformation = 3,
		SystemProcessInformation = 5,
		SystemProcessorPerformanceInformation = 8
	}

	protected static class NativeMethods
	{
		[DllImport("ntdll.dll")]
		public static extern int NtQuerySystemInformation(SystemInformationClass informationClass, [Out] SystemProcessorPerformanceInformation[] informations, int structSize, out IntPtr returnLength);
	}

	private readonly CPUID[][] cpuid;

	private long[] idleTimes;

	private long[] totalTimes;

	private float totalLoad;

	private readonly float[] coreLoads;

	private readonly bool available;

	public bool IsAvailable => available;

	private static bool GetTimes(out long[] idle, out long[] total)
	{
		SystemProcessorPerformanceInformation[] array = new SystemProcessorPerformanceInformation[64];
		int num = Marshal.SizeOf(typeof(SystemProcessorPerformanceInformation));
		idle = null;
		total = null;
		if (NativeMethods.NtQuerySystemInformation(SystemInformationClass.SystemProcessorPerformanceInformation, array, array.Length * num, out var returnLength) != 0)
		{
			return false;
		}
		idle = new long[(int)returnLength / num];
		total = new long[(int)returnLength / num];
		for (int i = 0; i < idle.Length; i++)
		{
			idle[i] = array[i].IdleTime;
			total[i] = array[i].KernelTime + array[i].UserTime;
		}
		return true;
	}

	public CPULoad(CPUID[][] cpuid)
	{
		this.cpuid = cpuid;
		coreLoads = new float[cpuid.Length];
		totalLoad = 0f;
		try
		{
			GetTimes(out idleTimes, out totalTimes);
		}
		catch (Exception)
		{
			idleTimes = null;
			totalTimes = null;
		}
		if (idleTimes != null)
		{
			available = true;
		}
	}

	public float GetTotalLoad()
	{
		return totalLoad;
	}

	public float GetCoreLoad(int core)
	{
		return coreLoads[core];
	}

	public void Update()
	{
		if (idleTimes == null || !GetTimes(out var idle, out var total))
		{
			return;
		}
		for (int i = 0; i < Math.Min(total.Length, totalTimes.Length); i++)
		{
			if (total[i] - totalTimes[i] < 100000)
			{
				return;
			}
		}
		if (idle == null || total == null)
		{
			return;
		}
		float num = 0f;
		int num2 = 0;
		for (int j = 0; j < cpuid.Length; j++)
		{
			float num3 = 0f;
			for (int k = 0; k < cpuid[j].Length; k++)
			{
				long num4 = cpuid[j][k].Thread;
				if (num4 < idle.Length && num4 < totalTimes.Length)
				{
					float num5 = (float)(idle[num4] - idleTimes[num4]) / (float)(total[num4] - totalTimes[num4]);
					num3 += num5;
					num += num5;
					num2++;
				}
			}
			num3 = 1f - num3 / (float)cpuid[j].Length;
			num3 = ((num3 < 0f) ? 0f : num3);
			coreLoads[j] = num3 * 100f;
		}
		if (num2 > 0)
		{
			num = 1f - num / (float)num2;
			num = ((num < 0f) ? 0f : num);
		}
		else
		{
			num = 0f;
		}
		totalLoad = num * 100f;
		totalTimes = total;
		idleTimes = idle;
	}
}
