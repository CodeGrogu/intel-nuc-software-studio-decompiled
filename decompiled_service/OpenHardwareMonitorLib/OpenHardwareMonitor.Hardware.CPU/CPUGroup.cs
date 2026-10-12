using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace OpenHardwareMonitor.Hardware.CPU;

internal class CPUGroup : IGroup
{
	private readonly List<GenericCPU> hardware = new List<GenericCPU>();

	private readonly CPUID[][][] threads;

	public IHardware[] Hardware => hardware.ToArray();

	private static CPUID[][] GetProcessorThreads()
	{
		List<CPUID> list = new List<CPUID>();
		for (int i = 0; i < ThreadAffinity.ProcessorGroupCount; i++)
		{
			for (int j = 0; j < 64; j++)
			{
				try
				{
					if (ThreadAffinity.IsValid(GroupAffinity.Single((ushort)i, j)))
					{
						CPUID cPUID = CPUID.Get(i, j);
						if (cPUID != null)
						{
							list.Add(cPUID);
						}
					}
				}
				catch (ArgumentOutOfRangeException)
				{
				}
			}
		}
		SortedDictionary<uint, List<CPUID>> sortedDictionary = new SortedDictionary<uint, List<CPUID>>();
		foreach (CPUID item in list)
		{
			sortedDictionary.TryGetValue(item.ProcessorId, out var value);
			if (value == null)
			{
				value = new List<CPUID>();
				sortedDictionary.Add(item.ProcessorId, value);
			}
			value.Add(item);
		}
		CPUID[][] array = new CPUID[sortedDictionary.Count][];
		int num = 0;
		foreach (List<CPUID> value2 in sortedDictionary.Values)
		{
			array[num] = value2.ToArray();
			num++;
		}
		return array;
	}

	private static CPUID[][] GroupThreadsByCore(IEnumerable<CPUID> threads)
	{
		SortedDictionary<uint, List<CPUID>> sortedDictionary = new SortedDictionary<uint, List<CPUID>>();
		foreach (CPUID thread in threads)
		{
			sortedDictionary.TryGetValue(thread.CoreId, out var value);
			if (value == null)
			{
				value = new List<CPUID>();
				sortedDictionary.Add(thread.CoreId, value);
			}
			value.Add(thread);
		}
		CPUID[][] array = new CPUID[sortedDictionary.Count][];
		int num = 0;
		foreach (List<CPUID> value2 in sortedDictionary.Values)
		{
			array[num] = value2.ToArray();
			num++;
		}
		return array;
	}

	public CPUGroup(ISettings settings)
	{
		CPUID[][] processorThreads = GetProcessorThreads();
		threads = new CPUID[processorThreads.Length][][];
		int num = 0;
		CPUID[][] array = processorThreads;
		foreach (CPUID[] array2 in array)
		{
			if (array2.Length == 0)
			{
				continue;
			}
			CPUID[][] array3 = GroupThreadsByCore(array2);
			threads[num] = array3;
			switch (array2[0].Vendor)
			{
			case Vendor.Intel:
				hardware.Add(new IntelCPU(num, array3, settings));
				break;
			case Vendor.AMD:
				switch (array2[0].Family)
				{
				case 15u:
					hardware.Add(new AMD0FCPU(num, array3, settings));
					break;
				case 16u:
				case 17u:
				case 18u:
				case 20u:
				case 21u:
				case 22u:
					hardware.Add(new AMD10CPU(num, array3, settings));
					break;
				case 23u:
					hardware.Add(new AMD17CPU(num, array3, settings));
					break;
				default:
					hardware.Add(new GenericCPU(num, array3, settings));
					break;
				}
				break;
			default:
				hardware.Add(new GenericCPU(num, array3, settings));
				break;
			}
			num++;
		}
	}

	private static void AppendCpuidData(StringBuilder r, uint[,] data, uint offset)
	{
		for (int i = 0; i < data.GetLength(0); i++)
		{
			r.Append(" ");
			r.Append((i + offset).ToString("X8", CultureInfo.InvariantCulture));
			for (int j = 0; j < 4; j++)
			{
				r.Append("  ");
				r.Append(data[i, j].ToString("X8", CultureInfo.InvariantCulture));
			}
			r.AppendLine();
		}
	}

	public string GetReport()
	{
		if (threads == null)
		{
			return null;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("CPUID");
		stringBuilder.AppendLine();
		for (int i = 0; i < threads.Length; i++)
		{
			stringBuilder.AppendLine("Processor " + i);
			stringBuilder.AppendLine();
			stringBuilder.AppendFormat("Processor Vendor: {0}{1}", threads[i][0][0].Vendor, Environment.NewLine);
			stringBuilder.AppendFormat("Processor Brand: {0}{1}", threads[i][0][0].BrandString, Environment.NewLine);
			stringBuilder.AppendFormat("Family: 0x{0}{1}", threads[i][0][0].Family.ToString("X", CultureInfo.InvariantCulture), Environment.NewLine);
			stringBuilder.AppendFormat("Model: 0x{0}{1}", threads[i][0][0].Model.ToString("X", CultureInfo.InvariantCulture), Environment.NewLine);
			stringBuilder.AppendFormat("Stepping: 0x{0}{1}", threads[i][0][0].Stepping.ToString("X", CultureInfo.InvariantCulture), Environment.NewLine);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("CPUID Return Values");
			stringBuilder.AppendLine();
			for (int j = 0; j < threads[i].Length; j++)
			{
				for (int k = 0; k < threads[i][j].Length; k++)
				{
					stringBuilder.AppendLine(" CPU Group: " + threads[i][j][k].Group);
					stringBuilder.AppendLine(" CPU Thread: " + threads[i][j][k].Thread);
					stringBuilder.AppendLine(" APIC ID: " + threads[i][j][k].ApicId);
					stringBuilder.AppendLine(" Processor ID: " + threads[i][j][k].ProcessorId);
					stringBuilder.AppendLine(" Core ID: " + threads[i][j][k].CoreId);
					stringBuilder.AppendLine(" Thread ID: " + threads[i][j][k].ThreadId);
					stringBuilder.AppendLine();
					stringBuilder.AppendLine(" Function  EAX       EBX       ECX       EDX");
					AppendCpuidData(stringBuilder, threads[i][j][k].Data, 0u);
					AppendCpuidData(stringBuilder, threads[i][j][k].ExtData, 2147483648u);
					stringBuilder.AppendLine();
				}
			}
		}
		return stringBuilder.ToString();
	}

	public void Close()
	{
		foreach (GenericCPU item in hardware)
		{
			item.Close();
		}
	}
}
