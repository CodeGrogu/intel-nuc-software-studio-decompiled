using System.Collections.Generic;
using OpenHardwareMonitor.Collections;

namespace OpenHardwareMonitor.Hardware.HDD;

[NamePrefix("")]
[RequireSmart(177)]
[RequireSmart(179)]
[RequireSmart(181)]
[RequireSmart(182)]
[RequireSmart(183)]
[RequireSmart(187)]
[RequireSmart(195)]
[RequireSmart(199)]
internal class SSDSamsung : AbstractHarddrive
{
	private static readonly IEnumerable<SmartAttribute> smartAttributes = new List<SmartAttribute>
	{
		new SmartAttribute(5, SmartNames.ReallocatedSectorsCount),
		new SmartAttribute(9, SmartNames.PowerOnHours, AbstractHarddrive.RawToInt),
		new SmartAttribute(12, SmartNames.PowerCycleCount, AbstractHarddrive.RawToInt),
		new SmartAttribute(175, SmartNames.ProgramFailCountChip, AbstractHarddrive.RawToInt),
		new SmartAttribute(176, SmartNames.EraseFailCountChip, AbstractHarddrive.RawToInt),
		new SmartAttribute(177, SmartNames.WearLevelingCount, AbstractHarddrive.RawToInt),
		new SmartAttribute(178, SmartNames.UsedReservedBlockCountChip, AbstractHarddrive.RawToInt),
		new SmartAttribute(179, SmartNames.UsedReservedBlockCountTotal, AbstractHarddrive.RawToInt),
		new SmartAttribute(180, SmartNames.RemainingLife, null, SensorType.Level, 0, SmartNames.RemainingLife),
		new SmartAttribute(181, SmartNames.ProgramFailCountTotal, AbstractHarddrive.RawToInt),
		new SmartAttribute(182, SmartNames.EraseFailCountTotal, AbstractHarddrive.RawToInt),
		new SmartAttribute(183, SmartNames.RuntimeBadBlockTotal, AbstractHarddrive.RawToInt),
		new SmartAttribute(187, SmartNames.UncorrectableErrorCount, AbstractHarddrive.RawToInt),
		new SmartAttribute(190, SmartNames.Temperature, (byte[] r, byte v, IReadOnlyArray<IParameter> p) => (float)(int)r[0] + (p?[0].Value ?? 0f), SensorType.Temperature, 0, SmartNames.Temperature, defaultHiddenSensor: false, new ParameterDescription[1]
		{
			new ParameterDescription("Offset [°C]", "Temperature offset of the thermal sensor.\nTemperature = Value + Offset.", 0f)
		}),
		new SmartAttribute(194, SmartNames.AirflowTemperature),
		new SmartAttribute(195, SmartNames.ECCRate),
		new SmartAttribute(198, SmartNames.OffLineUncorrectableErrorCount, AbstractHarddrive.RawToInt),
		new SmartAttribute(199, SmartNames.CRCErrorCount, AbstractHarddrive.RawToInt),
		new SmartAttribute(201, SmartNames.SupercapStatus),
		new SmartAttribute(202, SmartNames.ExceptionModeStatus),
		new SmartAttribute(235, SmartNames.PowerRecoveryCount),
		new SmartAttribute(241, SmartNames.TotalLbasWritten, (byte[] r, byte v, IReadOnlyArray<IParameter> p) => (float)(long)(((ulong)r[5] << 40) | ((ulong)r[4] << 32) | ((ulong)r[3] << 24) | ((ulong)r[2] << 16) | ((ulong)r[1] << 8) | r[0]) * 4.7683716E-07f, SensorType.Data, 0, "Total Bytes Written")
	};

	public SSDSamsung(ISmart smart, string name, string firmwareRevision, int index, ISettings settings)
		: base(smart, name, firmwareRevision, index, smartAttributes, settings)
	{
	}
}
