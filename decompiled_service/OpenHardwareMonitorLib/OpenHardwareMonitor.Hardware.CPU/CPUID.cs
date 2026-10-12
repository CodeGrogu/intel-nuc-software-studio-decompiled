using System;
using System.Text;

namespace OpenHardwareMonitor.Hardware.CPU;

internal class CPUID
{
	private readonly int group;

	private readonly int thread;

	private readonly GroupAffinity affinity;

	private readonly Vendor vendor;

	private readonly string cpuBrandString = "";

	private readonly string name = "";

	private readonly uint[,] cpuidData = new uint[0, 0];

	private readonly uint[,] cpuidExtData = new uint[0, 0];

	private readonly uint family;

	private readonly uint model;

	private readonly uint stepping;

	private readonly uint apicId;

	private readonly uint threadMaskWith;

	private readonly uint coreMaskWith;

	private readonly uint processorId;

	private readonly uint coreId;

	private readonly uint threadId;

	public const uint CPUID_0 = 0u;

	public const uint CPUID_EXT = 2147483648u;

	public string Name => name;

	public string BrandString => cpuBrandString;

	public int Group => group;

	public int Thread => thread;

	public GroupAffinity Affinity => affinity;

	public Vendor Vendor => vendor;

	public uint Family => family;

	public uint Model => model;

	public uint Stepping => stepping;

	public uint ApicId => apicId;

	public uint ProcessorId => processorId;

	public uint CoreId => coreId;

	public uint ThreadId => threadId;

	public uint[,] Data => cpuidData;

	public uint[,] ExtData => cpuidExtData;

	private static void AppendRegister(StringBuilder b, uint value)
	{
		b.Append((char)(value & 0xFF));
		b.Append((char)((value >> 8) & 0xFF));
		b.Append((char)((value >> 16) & 0xFF));
		b.Append((char)((value >> 24) & 0xFF));
	}

	private static uint NextLog2(long x)
	{
		if (x <= 0)
		{
			return 0u;
		}
		x--;
		uint num = 0u;
		while (x > 0)
		{
			x >>= 1;
			num++;
		}
		return num;
	}

	public static CPUID Get(int group, int thread)
	{
		if (thread >= 64)
		{
			return null;
		}
		GroupAffinity groupAffinity = GroupAffinity.Single((ushort)group, thread);
		GroupAffinity groupAffinity2 = ThreadAffinity.Set(groupAffinity);
		if (groupAffinity2 == GroupAffinity.Undefined)
		{
			return null;
		}
		try
		{
			return new CPUID(group, thread, groupAffinity);
		}
		finally
		{
			ThreadAffinity.Set(groupAffinity2);
		}
	}

	private CPUID(int group, int thread, GroupAffinity affinity)
	{
		this.group = group;
		this.thread = thread;
		this.affinity = affinity;
		uint num = 0u;
		uint num2 = 0u;
		Opcode.Cpuid(0u, 0u, out var eax, out var ebx, out var ecx, out var edx);
		if (eax == 0)
		{
			return;
		}
		num = eax;
		StringBuilder stringBuilder = new StringBuilder();
		AppendRegister(stringBuilder, ebx);
		AppendRegister(stringBuilder, edx);
		AppendRegister(stringBuilder, ecx);
		switch (stringBuilder.ToString())
		{
		case "GenuineIntel":
			vendor = Vendor.Intel;
			break;
		case "AuthenticAMD":
			vendor = Vendor.AMD;
			break;
		default:
			vendor = Vendor.Unknown;
			break;
		}
		eax = (ebx = (ecx = (edx = 0u)));
		Opcode.Cpuid(2147483648u, 0u, out eax, out ebx, out ecx, out edx);
		if (eax <= 2147483648u)
		{
			return;
		}
		num2 = eax - 2147483648u;
		num = Math.Min(num, 1024u);
		num2 = Math.Min(num2, 1024u);
		cpuidData = new uint[num + 1, 4];
		for (uint num3 = 0u; num3 < num + 1; num3++)
		{
			Opcode.Cpuid(num3, 0u, out cpuidData[num3, 0], out cpuidData[num3, 1], out cpuidData[num3, 2], out cpuidData[num3, 3]);
		}
		cpuidExtData = new uint[num2 + 1, 4];
		for (uint num4 = 0u; num4 < num2 + 1; num4++)
		{
			Opcode.Cpuid(2147483648u + num4, 0u, out cpuidExtData[num4, 0], out cpuidExtData[num4, 1], out cpuidExtData[num4, 2], out cpuidExtData[num4, 3]);
		}
		StringBuilder stringBuilder2 = new StringBuilder();
		for (uint num5 = 2u; num5 <= 4; num5++)
		{
			Opcode.Cpuid(2147483648u + num5, 0u, out eax, out ebx, out ecx, out edx);
			AppendRegister(stringBuilder2, eax);
			AppendRegister(stringBuilder2, ebx);
			AppendRegister(stringBuilder2, ecx);
			AppendRegister(stringBuilder2, edx);
		}
		stringBuilder2.Replace('\0', ' ');
		cpuBrandString = stringBuilder2.ToString().Trim();
		stringBuilder2.Replace("Dual-Core Processor", "");
		stringBuilder2.Replace("Triple-Core Processor", "");
		stringBuilder2.Replace("Quad-Core Processor", "");
		stringBuilder2.Replace("Six-Core Processor", "");
		stringBuilder2.Replace("Eight-Core Processor", "");
		stringBuilder2.Replace("Dual Core Processor", "");
		stringBuilder2.Replace("Quad Core Processor", "");
		stringBuilder2.Replace("12-Core Processor", "");
		stringBuilder2.Replace("16-Core Processor", "");
		stringBuilder2.Replace("24-Core Processor", "");
		stringBuilder2.Replace("32-Core Processor", "");
		stringBuilder2.Replace("64-Core Processor", "");
		stringBuilder2.Replace("6-Core Processor", "");
		stringBuilder2.Replace("8-Core Processor", "");
		stringBuilder2.Replace("with Radeon Vega Mobile Gfx", "");
		stringBuilder2.Replace("w/ Radeon Vega Mobile Gfx", "");
		stringBuilder2.Replace("with Radeon Vega Graphics", "");
		stringBuilder2.Replace("APU with Radeon(tm) HD Graphics", "");
		stringBuilder2.Replace("APU with Radeon(TM) HD Graphics", "");
		stringBuilder2.Replace("APU with AMD Radeon R2 Graphics", "");
		stringBuilder2.Replace("APU with AMD Radeon R3 Graphics", "");
		stringBuilder2.Replace("APU with AMD Radeon R4 Graphics", "");
		stringBuilder2.Replace("APU with AMD Radeon R5 Graphics", "");
		stringBuilder2.Replace("APU with Radeon(tm) R3", "");
		stringBuilder2.Replace("RADEON R2, 4 COMPUTE CORES 2C+2G", "");
		stringBuilder2.Replace("RADEON R4, 5 COMPUTE CORES 2C+3G", "");
		stringBuilder2.Replace("RADEON R5, 5 COMPUTE CORES 2C+3G", "");
		stringBuilder2.Replace("RADEON R5, 10 COMPUTE CORES 4C+6G", "");
		stringBuilder2.Replace("RADEON R7, 10 COMPUTE CORES 4C+6G", "");
		stringBuilder2.Replace("RADEON R7, 12 COMPUTE CORES 4C+8G", "");
		stringBuilder2.Replace("Radeon R5, 6 Compute Cores 2C+4G", "");
		stringBuilder2.Replace("Radeon R5, 8 Compute Cores 4C+4G", "");
		stringBuilder2.Replace("Radeon R6, 10 Compute Cores 4C+6G", "");
		stringBuilder2.Replace("Radeon R7, 10 Compute Cores 4C+6G", "");
		stringBuilder2.Replace("Radeon R7, 12 Compute Cores 4C+8G", "");
		stringBuilder2.Replace("R5, 10 Compute Cores 4C+6G", "");
		stringBuilder2.Replace("R7, 12 COMPUTE CORES 4C+8G", "");
		stringBuilder2.Replace("(R)", " ");
		stringBuilder2.Replace("(TM)", " ");
		stringBuilder2.Replace("(tm)", " ");
		stringBuilder2.Replace("CPU", " ");
		for (int i = 0; i < 10; i++)
		{
			stringBuilder2.Replace("  ", " ");
		}
		name = stringBuilder2.ToString();
		if (name.Contains("@"))
		{
			name = name.Remove(name.LastIndexOf('@'));
		}
		name = name.Trim();
		family = ((cpuidData[1, 0] & 0xFF00000) >> 20) + ((cpuidData[1, 0] & 0xF00) >> 8);
		model = ((cpuidData[1, 0] & 0xF0000) >> 12) + ((cpuidData[1, 0] & 0xF0) >> 4);
		stepping = cpuidData[1, 0] & 0xF;
		apicId = (cpuidData[1, 1] >> 24) & 0xFF;
		switch (vendor)
		{
		case Vendor.Intel:
		{
			uint num7 = (cpuidData[1, 1] >> 16) & 0xFF;
			uint num8 = ((num < 4) ? 1u : (((cpuidData[4, 0] >> 26) & 0x3F) + 1));
			threadMaskWith = NextLog2(num7 / num8);
			coreMaskWith = NextLog2(num8);
			break;
		}
		case Vendor.AMD:
			if (family == 23)
			{
				coreMaskWith = (cpuidExtData[8, 2] >> 12) & 0xF;
				threadMaskWith = NextLog2(((cpuidExtData[30, 1] >> 8) & 0xFF) + 1);
			}
			else
			{
				uint num6 = ((num2 < 8) ? 1u : ((cpuidExtData[8, 2] & 0xFF) + 1));
				coreMaskWith = NextLog2(num6);
				threadMaskWith = 0u;
			}
			break;
		default:
			threadMaskWith = 0u;
			coreMaskWith = 0u;
			break;
		}
		processorId = apicId >> (int)(coreMaskWith + threadMaskWith);
		coreId = (apicId >> (int)threadMaskWith) - (processorId << (int)coreMaskWith);
		threadId = apicId - (processorId << (int)(coreMaskWith + threadMaskWith)) - (coreId << (int)threadMaskWith);
	}
}
