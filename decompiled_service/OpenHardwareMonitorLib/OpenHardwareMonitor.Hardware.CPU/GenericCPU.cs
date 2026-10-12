using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace OpenHardwareMonitor.Hardware.CPU;

internal class GenericCPU : Hardware
{
	protected readonly CPUID[][] cpuid;

	protected readonly uint family;

	protected readonly uint model;

	protected readonly uint stepping;

	protected readonly int processorIndex;

	protected readonly int coreCount;

	private readonly bool hasModelSpecificRegisters;

	private readonly bool hasTimeStampCounter;

	private readonly bool isInvariantTimeStampCounter;

	private readonly double estimatedTimeStampCounterFrequency;

	private readonly double estimatedTimeStampCounterFrequencyError;

	private ulong lastTimeStampCount;

	private long lastTime;

	private double timeStampCounterFrequency;

	private readonly Vendor vendor;

	private readonly CPULoad cpuLoad;

	private readonly Sensor totalLoad;

	private readonly Sensor[] coreLoads;

	public override HardwareType HardwareType => HardwareType.CPU;

	public bool HasModelSpecificRegisters => hasModelSpecificRegisters;

	public bool HasTimeStampCounter => hasTimeStampCounter;

	public double TimeStampCounterFrequency => timeStampCounterFrequency;

	protected string CoreString(int i)
	{
		if (coreCount == 1)
		{
			return "CPU Core";
		}
		return "CPU Core #" + (i + 1);
	}

	public GenericCPU(int processorIndex, CPUID[][] cpuid, ISettings settings)
		: base(cpuid[0][0].Name, CreateIdentifier(cpuid[0][0].Vendor, processorIndex), settings)
	{
		this.cpuid = cpuid;
		vendor = cpuid[0][0].Vendor;
		family = cpuid[0][0].Family;
		model = cpuid[0][0].Model;
		stepping = cpuid[0][0].Stepping;
		this.processorIndex = processorIndex;
		coreCount = cpuid.Length;
		if (cpuid[0][0].Data.GetLength(0) > 1 && (cpuid[0][0].Data[1, 3] & 0x20) != 0)
		{
			hasModelSpecificRegisters = true;
		}
		else
		{
			hasModelSpecificRegisters = false;
		}
		if (cpuid[0][0].Data.GetLength(0) > 1 && (cpuid[0][0].Data[1, 3] & 0x10) != 0)
		{
			hasTimeStampCounter = true;
		}
		else
		{
			hasTimeStampCounter = false;
		}
		if (cpuid[0][0].ExtData.GetLength(0) > 7 && (cpuid[0][0].ExtData[7, 3] & 0x100) != 0)
		{
			isInvariantTimeStampCounter = true;
		}
		else
		{
			isInvariantTimeStampCounter = false;
		}
		if (coreCount > 1)
		{
			totalLoad = new Sensor("CPU Total", 0, SensorType.Load, this, settings);
		}
		else
		{
			totalLoad = null;
		}
		coreLoads = new Sensor[coreCount];
		for (int i = 0; i < coreLoads.Length; i++)
		{
			coreLoads[i] = new Sensor(CoreString(i), i + 1, SensorType.Load, this, settings);
		}
		cpuLoad = new CPULoad(cpuid);
		if (cpuLoad.IsAvailable)
		{
			Sensor[] array = coreLoads;
			foreach (Sensor sensor in array)
			{
				ActivateSensor(sensor);
			}
			if (totalLoad != null)
			{
				ActivateSensor(totalLoad);
			}
		}
		if (hasTimeStampCounter)
		{
			GroupAffinity affinity = ThreadAffinity.Set(cpuid[0][0].Affinity);
			EstimateTimeStampCounterFrequency(out estimatedTimeStampCounterFrequency, out estimatedTimeStampCounterFrequencyError);
			ThreadAffinity.Set(affinity);
		}
		else
		{
			estimatedTimeStampCounterFrequency = 0.0;
		}
		timeStampCounterFrequency = estimatedTimeStampCounterFrequency;
	}

	private static Identifier CreateIdentifier(Vendor vendor, int processorIndex)
	{
		string text = vendor switch
		{
			Vendor.AMD => "amdcpu", 
			Vendor.Intel => "intelcpu", 
			_ => "genericcpu", 
		};
		return new Identifier(text, processorIndex.ToString(CultureInfo.InvariantCulture));
	}

	private void EstimateTimeStampCounterFrequency(out double frequency, out double error)
	{
		EstimateTimeStampCounterFrequency(0.0, out var frequency2, out var error2);
		EstimateTimeStampCounterFrequency(0.0, out frequency2, out error2);
		error = double.MaxValue;
		frequency = 0.0;
		for (int i = 0; i < 5; i++)
		{
			EstimateTimeStampCounterFrequency(0.025, out frequency2, out error2);
			if (error2 < error)
			{
				error = error2;
				frequency = frequency2;
			}
			if (error < 0.0001)
			{
				break;
			}
		}
	}

	private void EstimateTimeStampCounterFrequency(double timeWindow, out double frequency, out double error)
	{
		long num = (long)(timeWindow * (double)Stopwatch.Frequency);
		long num2 = Stopwatch.GetTimestamp() + (long)Math.Ceiling(0.001 * (double)num);
		long num3 = num2 + num;
		while (Stopwatch.GetTimestamp() < num2)
		{
		}
		ulong num4 = Opcode.Rdtsc();
		long timestamp = Stopwatch.GetTimestamp();
		while (Stopwatch.GetTimestamp() < num3)
		{
		}
		ulong num5 = Opcode.Rdtsc();
		long timestamp2 = Stopwatch.GetTimestamp();
		double num6 = num3 - num2;
		frequency = 1E-06 * ((double)(num5 - num4) * (double)Stopwatch.Frequency) / num6;
		double num7 = (double)(timestamp - num2) / num6;
		double num8 = (double)(timestamp2 - num3) / num6;
		error = num7 + num8;
	}

	private static void AppendMSRData(StringBuilder r, uint msr, GroupAffinity affinity)
	{
		if (Ring0.RdmsrTx(msr, out var eax, out var edx, affinity))
		{
			r.Append(" ");
			r.Append(msr.ToString("X8", CultureInfo.InvariantCulture));
			r.Append("  ");
			r.Append(edx.ToString("X8", CultureInfo.InvariantCulture));
			r.Append("  ");
			r.Append(eax.ToString("X8", CultureInfo.InvariantCulture));
			r.AppendLine();
		}
	}

	protected virtual uint[] GetMSRs()
	{
		return null;
	}

	public override string GetReport()
	{
		StringBuilder stringBuilder = new StringBuilder();
		switch (vendor)
		{
		case Vendor.AMD:
			stringBuilder.AppendLine("AMD CPU");
			break;
		case Vendor.Intel:
			stringBuilder.AppendLine("Intel CPU");
			break;
		default:
			stringBuilder.AppendLine("Generic CPU");
			break;
		}
		stringBuilder.AppendLine();
		stringBuilder.AppendFormat("Name: {0}{1}", name, Environment.NewLine);
		stringBuilder.AppendFormat("Number of Cores: {0}{1}", coreCount, Environment.NewLine);
		stringBuilder.AppendFormat("Threads per Core: {0}{1}", cpuid[0].Length, Environment.NewLine);
		stringBuilder.AppendLine(string.Format(CultureInfo.InvariantCulture, "Timer Frequency: {0} MHz", new object[1] { (double)Stopwatch.Frequency * 1E-06 }));
		StringBuilder stringBuilder2 = stringBuilder;
		string text;
		if (hasTimeStampCounter)
		{
			text = (isInvariantTimeStampCounter ? "Invariant" : "Not Invariant");
		}
		else
		{
			text = "None";
		}
		stringBuilder2.AppendLine("Time Stamp Counter: " + text);
		stringBuilder.AppendLine(string.Format(CultureInfo.InvariantCulture, "Estimated Time Stamp Counter Frequency: {0} MHz", new object[1] { Math.Round(estimatedTimeStampCounterFrequency * 100.0) * 0.01 }));
		stringBuilder.AppendLine(string.Format(CultureInfo.InvariantCulture, "Estimated Time Stamp Counter Frequency Error: {0} Mhz", new object[1] { Math.Round(estimatedTimeStampCounterFrequency * estimatedTimeStampCounterFrequencyError * 100000.0) * 1E-05 }));
		stringBuilder.AppendLine(string.Format(CultureInfo.InvariantCulture, "Time Stamp Counter Frequency: {0} MHz", new object[1] { Math.Round(timeStampCounterFrequency * 100.0) * 0.01 }));
		stringBuilder.AppendLine();
		uint[] mSRs = GetMSRs();
		if (mSRs != null && mSRs.Length != 0)
		{
			for (int i = 0; i < cpuid.Length; i++)
			{
				stringBuilder.AppendLine("MSR Core #" + (i + 1));
				stringBuilder.AppendLine();
				stringBuilder.AppendLine(" MSR       EDX       EAX");
				uint[] array = mSRs;
				foreach (uint msr in array)
				{
					AppendMSRData(stringBuilder, msr, cpuid[i][0].Affinity);
				}
				stringBuilder.AppendLine();
			}
		}
		return stringBuilder.ToString();
	}

	public override void Update()
	{
		if (hasTimeStampCounter && isInvariantTimeStampCounter)
		{
			GroupAffinity affinity = ThreadAffinity.Set(cpuid[0][0].Affinity);
			long timestamp = Stopwatch.GetTimestamp();
			ulong num = Opcode.Rdtsc();
			long timestamp2 = Stopwatch.GetTimestamp();
			ThreadAffinity.Set(affinity);
			double num2 = (double)(timestamp2 - lastTime) / (double)Stopwatch.Frequency;
			if ((double)(timestamp2 - timestamp) / (double)Stopwatch.Frequency < 0.0001)
			{
				if (lastTime != 0L && num2 > 0.5 && num2 < 2.0)
				{
					timeStampCounterFrequency = (double)(num - lastTimeStampCount) / (1000000.0 * num2);
				}
				lastTimeStampCount = num;
				lastTime = timestamp2;
			}
		}
		if (cpuLoad.IsAvailable)
		{
			cpuLoad.Update();
			for (int i = 0; i < coreLoads.Length; i++)
			{
				coreLoads[i].Value = cpuLoad.GetCoreLoad(i);
			}
			if (totalLoad != null)
			{
				totalLoad.Value = cpuLoad.GetTotalLoad();
			}
		}
	}
}
