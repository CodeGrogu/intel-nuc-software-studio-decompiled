using System.Collections.Generic;

namespace OpenHardwareMonitor.Hardware.HDD;

internal class HarddriveGroup : IGroup
{
	private const int MAX_DRIVES = 32;

	private readonly List<AbstractHarddrive> hardware = new List<AbstractHarddrive>();

	public IHardware[] Hardware => hardware.ToArray();

	public HarddriveGroup(ISettings settings)
	{
		if (OperatingSystem.IsUnix)
		{
			return;
		}
		ISmart smart = new WindowsSmart();
		for (int i = 0; i < 32; i++)
		{
			AbstractHarddrive abstractHarddrive = AbstractHarddrive.CreateInstance(smart, i, settings);
			if (abstractHarddrive != null)
			{
				hardware.Add(abstractHarddrive);
			}
		}
	}

	public string GetReport()
	{
		return null;
	}

	public void Close()
	{
		foreach (AbstractHarddrive item in hardware)
		{
			item.Close();
		}
	}
}
