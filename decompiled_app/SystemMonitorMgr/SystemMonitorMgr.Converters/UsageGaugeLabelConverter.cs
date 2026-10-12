using SystemMonitorMgr.Enums;
using Infrastructure.Converters;

namespace SystemMonitorMgr.Converters;

public static class UsageGaugeLabelConverter
{
	private const string CPU_LABEL = "Cpu";

	private const string dGPU_LABEL = "dGpuSystemDataType";

	private const string iGPU_LABEL = "iGpuSystemDataType";

	private const string STORAGE_LABEL = "SSD0SystemDataType";

	private const string MEMORY_LABEL = "MemorySystemDataType";

	public static string LabelFromType(SystemDataType type)
	{
		switch (type)
		{
		case SystemDataType.Cpu:
			return ResourceDictionaryConverter.GetString("Cpu");
		case SystemDataType.dGpu:
			return ResourceDictionaryConverter.GetString("dGpuSystemDataType");
		case SystemDataType.iGpu:
			return ResourceDictionaryConverter.GetString("iGpuSystemDataType");
		case SystemDataType.SSD0:
		case SystemDataType.SSD1:
		case SystemDataType.SSD2:
		case SystemDataType.SSD3:
			return ResourceDictionaryConverter.GetString("SSD0SystemDataType");
		case SystemDataType.Memory:
			return ResourceDictionaryConverter.GetString("MemorySystemDataType");
		default:
			return "";
		}
	}
}
