using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace OpenHardwareMonitor.Hardware.Nvidia;

internal class NvidiaGroup : IGroup
{
	private readonly List<Hardware> hardware = new List<Hardware>();

	private readonly StringBuilder report = new StringBuilder();

	public IHardware[] Hardware => hardware.ToArray();

	public NvidiaGroup(ISettings settings)
	{
		if (!NVAPI.IsAvailable)
		{
			return;
		}
		report.AppendLine("NVAPI");
		report.AppendLine();
		if (NVAPI.NvAPI_GetInterfaceVersionString(out var version) == NvStatus.OK)
		{
			report.Append(" Version: ");
			report.AppendLine(version);
		}
		NvPhysicalGpuHandle[] array = new NvPhysicalGpuHandle[64];
		if (NVAPI.NvAPI_EnumPhysicalGPUs == null)
		{
			report.AppendLine(" Error: NvAPI_EnumPhysicalGPUs not available");
			report.AppendLine();
			return;
		}
		NvStatus nvStatus = NVAPI.NvAPI_EnumPhysicalGPUs(array, out var gpuCount);
		if (nvStatus != NvStatus.OK)
		{
			report.AppendLine(" Status: " + nvStatus);
			report.AppendLine();
			return;
		}
		NVML.NvmlReturn nvmlReturn = NVML.NvmlInit();
		report.AppendLine();
		report.AppendLine("NVML");
		report.AppendLine();
		report.AppendLine(" Status: " + nvmlReturn);
		report.AppendLine();
		IDictionary<NvPhysicalGpuHandle, NvDisplayHandle> dictionary = new Dictionary<NvPhysicalGpuHandle, NvDisplayHandle>();
		if (NVAPI.NvAPI_EnumNvidiaDisplayHandle != null && NVAPI.NvAPI_GetPhysicalGPUsFromDisplay != null)
		{
			NvStatus nvStatus2 = NvStatus.OK;
			int num = 0;
			while (nvStatus2 == NvStatus.OK)
			{
				NvDisplayHandle displayHandle = default;
				nvStatus2 = NVAPI.NvAPI_EnumNvidiaDisplayHandle(num, ref displayHandle);
				num++;
				if (nvStatus2 != NvStatus.OK)
				{
					continue;
				}
				NvPhysicalGpuHandle[] array2 = new NvPhysicalGpuHandle[64];
				if (NVAPI.NvAPI_GetPhysicalGPUsFromDisplay(displayHandle, array2, out var gpuCount2) != NvStatus.OK)
				{
					continue;
				}
				for (int i = 0; i < gpuCount2; i++)
				{
					if (!dictionary.ContainsKey(array2[i]))
					{
						dictionary.Add(array2[i], displayHandle);
					}
				}
			}
		}
		report.Append("Number of GPUs: ");
		report.AppendLine(gpuCount.ToString(CultureInfo.InvariantCulture));
		for (int j = 0; j < gpuCount; j++)
		{
			dictionary.TryGetValue(array[j], out var value);
			hardware.Add(new NvidiaGPU(j, array[j], value, settings));
		}
		report.AppendLine();
	}

	public string GetReport()
	{
		return report.ToString();
	}

	public void Close()
	{
		foreach (Hardware item in hardware)
		{
			item.Close();
		}
		if (NVML.IsInitialized)
		{
			NVML.NvmlShutdown();
		}
	}
}
