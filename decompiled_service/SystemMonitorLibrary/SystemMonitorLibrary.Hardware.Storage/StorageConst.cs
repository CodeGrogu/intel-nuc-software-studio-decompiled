using System;

namespace SystemMonitorLibrary.Hardware.Storage;

internal static class StorageConst
{
	public static readonly double BYTES_TO_GIBIBYTE = Math.Pow(2.0, 30.0);

	public const int LOCAL_FIXED_DISK_VALUE = 3;

	public const string LOGICAL_DISK_QUERY = "SELECT * FROM Win32_LogicalDisk";

	public const string LOGICAL_DISK_KEY_TYPE = "DriveType";

	public const string LOGICAL_DISK_KEY_NAME = "Name";

	public const string LOGICAL_DISK_KEY_VOLUME_NAME = "VolumeName";

	public const string LOGICAL_DISK_KEY_FREE_SPACE = "FreeSpace";

	public const string LOGICAL_DISK_KEY_SIZE = "Size";

	public const string PHYSICAL_DISK_QUERY = "SELECT * FROM MSFT_PhysicalDisk";

	public const string PHYSICAL_DISK_QUERY_SCOPE = "\\\\.\\root\\microsoft\\windows\\storage";

	public const string PHYSICAL_DISK_KEY_BUS_TYPE = "BusType";

	public const string PHYSICAL_DISK_KEY_MEDIA_TYPE = "MediaType";

	public const string NAME_KEY = "Name";

	public const string LOGICAL_DISK_NAME_KEY = "LogicalDiskName";

	public const string LOGICAL_DISK_VOLUME_NAME_KEY = "LogicalDiskVolumeName";

	public const string FREE_SPACE_KEY = "FreeSpace";

	public const string TOTAL_SIZE_KEY = "TotalSize";

	public const string INTERFACE_TYPE_KEY = "InterfaceType";

	public const string USAGE_KEY = "Usage";
}
