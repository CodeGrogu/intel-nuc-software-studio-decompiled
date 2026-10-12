using System.Collections.Generic;
using OpenHardwareMonitor.Collections;

namespace OpenHardwareMonitor.Hardware.HDD;

[NamePrefix("PLEXTOR")]
internal class SSDPlextor : AbstractHarddrive
{
	private static readonly IEnumerable<SmartAttribute> smartAttributes = new List<SmartAttribute>
	{
		new SmartAttribute(9, SmartNames.PowerOnHours, AbstractHarddrive.RawToInt),
		new SmartAttribute(12, SmartNames.PowerCycleCount, AbstractHarddrive.RawToInt),
		new SmartAttribute(241, SmartNames.HostWrites, RawToGb, SensorType.Data, 0, SmartNames.HostWrites),
		new SmartAttribute(242, SmartNames.HostReads, RawToGb, SensorType.Data, 1, SmartNames.HostReads)
	};

	public SSDPlextor(ISmart smart, string name, string firmwareRevision, int index, ISettings settings)
		: base(smart, name, firmwareRevision, index, smartAttributes, settings)
	{
	}

	private static float RawToGb(byte[] rawvalue, byte value, IReadOnlyArray<IParameter> parameters)
	{
		return AbstractHarddrive.RawToInt(rawvalue, value, parameters) / 32f;
	}
}
