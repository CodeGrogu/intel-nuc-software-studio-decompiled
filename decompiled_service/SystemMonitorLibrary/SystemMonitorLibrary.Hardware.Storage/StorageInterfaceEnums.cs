namespace SystemMonitorLibrary.Hardware.Storage;

public static class StorageInterfaceEnums
{
	public enum MediaType : ushort
	{
		Unspecified = 1,
		HDD = 3,
		SSD = 4,
		SCM = 5
	}

	public enum BusType : ushort
	{
		Unknown,
		SCSI,
		ATAPI,
		ATA,
		IEEE1394,
		SSA,
		FibreChannel,
		USB,
		RAID,
		iSCSI,
		SAS,
		SATA,
		SD,
		MMC,
		MAX,
		FileBackedVirtual,
		StorageSpaces,
		NVMe
	}
}
