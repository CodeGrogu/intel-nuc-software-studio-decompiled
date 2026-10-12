namespace OpenHardwareMonitor.Hardware.Mainboard;

internal class MainboardGroup : IGroup
{
	private readonly Mainboard[] mainboards;

	public IHardware[] Hardware => mainboards;

	public MainboardGroup(SMBIOS smbios, ISettings settings)
	{
		mainboards = new Mainboard[1];
		mainboards[0] = new Mainboard(smbios, settings);
	}

	public void Close()
	{
		Mainboard[] array = mainboards;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Close();
		}
	}

	public string GetReport()
	{
		return null;
	}
}
