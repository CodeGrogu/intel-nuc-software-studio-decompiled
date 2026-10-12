using System.Windows.Media;

namespace NucLedMgr.WMI.CpuUsage;

public static class CpuServiceConstants
{
	public const string SERVICE_URI = "net.pipe://localhost/CpuService";

	public const string ENDPOINT_NAME = "NssCpuService";

	public const int MONITOR_TIMER = 5000;

	public const int UPDATE_TIMER = 1000;

	public const string CATEGORY_NAME = "Processor";

	public const string COUNTER_NAME = "% Processor Time";

	public const string INSTANCE_NAME = "_Total";

	public static readonly Color GREEN = Color.FromRgb(0, 128, 0);

	public static readonly Color RED = Color.FromRgb(byte.MaxValue, 0, 0);

	public static readonly Color BLACK = Color.FromRgb(0, 0, 0);
}
