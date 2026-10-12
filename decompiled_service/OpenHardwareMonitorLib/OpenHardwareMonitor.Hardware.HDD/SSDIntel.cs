using System.Collections.Generic;
using OpenHardwareMonitor.Collections;

namespace OpenHardwareMonitor.Hardware.HDD;

[NamePrefix("INTEL SSD")]
[RequireSmart(225)]
[RequireSmart(232)]
[RequireSmart(233)]
internal class SSDIntel : AbstractHarddrive
{
	private static readonly IEnumerable<SmartAttribute> smartAttributes = new List<SmartAttribute>
	{
		new SmartAttribute(1, SmartNames.ReadErrorRate),
		new SmartAttribute(3, SmartNames.SpinUpTime),
		new SmartAttribute(4, SmartNames.StartStopCount, AbstractHarddrive.RawToInt),
		new SmartAttribute(5, SmartNames.ReallocatedSectorsCount),
		new SmartAttribute(9, SmartNames.PowerOnHours, AbstractHarddrive.RawToInt),
		new SmartAttribute(12, SmartNames.PowerCycleCount, AbstractHarddrive.RawToInt),
		new SmartAttribute(170, SmartNames.AvailableReservedSpace),
		new SmartAttribute(171, SmartNames.ProgramFailCount),
		new SmartAttribute(172, SmartNames.EraseFailCount),
		new SmartAttribute(174, SmartNames.UnexpectedPowerLossCount, AbstractHarddrive.RawToInt),
		new SmartAttribute(183, SmartNames.SataDownshiftErrorCount, AbstractHarddrive.RawToInt),
		new SmartAttribute(187, SmartNames.UncorrectableErrorCount, AbstractHarddrive.RawToInt),
		new SmartAttribute(184, SmartNames.EndToEndError),
		new SmartAttribute(190, SmartNames.Temperature, (byte[] r, byte v, IReadOnlyArray<IParameter> p) => (float)(int)r[0] + (p?[0].Value ?? 0f), SensorType.Temperature, 0, SmartNames.Temperature, defaultHiddenSensor: false, new ParameterDescription[1]
		{
			new ParameterDescription("Offset [°C]", "Temperature offset of the thermal sensor.\nTemperature = Value + Offset.", 0f)
		}),
		new SmartAttribute(192, SmartNames.UnsafeShutdownCount),
		new SmartAttribute(199, SmartNames.CRCErrorCount, AbstractHarddrive.RawToInt),
		new SmartAttribute(225, SmartNames.HostWrites, (byte[] r, byte v, IReadOnlyArray<IParameter> p) => AbstractHarddrive.RawToInt(r, v, p) / 32f, SensorType.Data, 0, SmartNames.HostWrites),
		new SmartAttribute(232, SmartNames.RemainingLife, null, SensorType.Level, 0, SmartNames.RemainingLife),
		new SmartAttribute(233, SmartNames.MediaWearOutIndicator),
		new SmartAttribute(241, SmartNames.HostWrites, (byte[] r, byte v, IReadOnlyArray<IParameter> p) => AbstractHarddrive.RawToInt(r, v, p) / 32f, SensorType.Data, 0, SmartNames.HostWrites),
		new SmartAttribute(242, SmartNames.HostReads, (byte[] r, byte v, IReadOnlyArray<IParameter> p) => AbstractHarddrive.RawToInt(r, v, p) / 32f, SensorType.Data, 1, SmartNames.HostReads)
	};

	public SSDIntel(ISmart smart, string name, string firmwareRevision, int index, ISettings settings)
		: base(smart, name, firmwareRevision, index, smartAttributes, settings)
	{
	}
}
