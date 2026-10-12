namespace SystemMonitorLibrary.Hardware.SystemInformation;

internal static class SystemInfoConst
{
	public const string MANUFACTURER_KEY = "Manufacturer";

	public const string PRODUCT_NAME_KEY = "ProductName";

	public const string BIOS_VERSION_KEY = "BiosVersion";

	public const string EC_FIRMWARE_VERSION_KEY = "ECFirmwareVersion";

	public const string SYSTEM_SKU_KEY = "SystemSKU";

	public const string SYSTEM_VERSION_KEY = "SystemVersion";

	public const string BASEBOARDVERSION_KEY = "BaseBoardVersion";

	public const string WMI_ROOT = "root\\WMI";

	public const string SYSTEM_INFORMATION_QUERY = "SELECT * FROM MS_SystemInformation";

	public const string MANUFACTURER_WMI_KEY = "SystemManufacturer";

	public const string PRODUCT_NAME_WMI_KEY = "SystemProductName";

	public const string BIOS_VERSION_WMI_KEY = "BIOSVersion";

	public const string EC_FIRMWARE_VERSION_MAJOR_WMI_KEY = "ECFirmwareMajorRelease";

	public const string EC_FIRMWARE_VERSION_MINOR_WMI_KEY = "ECFirmwareMinorRelease";
}
