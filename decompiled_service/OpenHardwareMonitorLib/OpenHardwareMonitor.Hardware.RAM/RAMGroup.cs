namespace OpenHardwareMonitor.Hardware.RAM;

internal class RAMGroup : IGroup
{
	private Hardware[] hardware;

	public IHardware[] Hardware => hardware;

	public RAMGroup(SMBIOS smbios, ISettings settings)
	{
		if (OperatingSystem.IsUnix)
		{
			hardware = new Hardware[0];
			return;
		}
		hardware = new Hardware[1]
		{
			new GenericRAM("Generic Memory", settings)
		};
	}

	public string GetReport()
	{
		return null;
	}

	public void Close()
	{
		Hardware[] array = hardware;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Close();
		}
	}
}
