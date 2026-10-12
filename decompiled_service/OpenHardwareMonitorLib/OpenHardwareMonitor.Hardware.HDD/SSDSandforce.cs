using System.Collections.Generic;
using OpenHardwareMonitor.Collections;

namespace OpenHardwareMonitor.Hardware.HDD;

[NamePrefix("")]
[RequireSmart(171)]
[RequireSmart(177)]
internal class SSDSandforce : AbstractHarddrive
{
	private static readonly IEnumerable<SmartAttribute> smartAttributes = new List<SmartAttribute>
	{
		new SmartAttribute(1, SmartNames.RawReadErrorRate),
		new SmartAttribute(5, SmartNames.RetiredBlockCount, AbstractHarddrive.RawToInt),
		new SmartAttribute(9, SmartNames.PowerOnHours, AbstractHarddrive.RawToInt),
		new SmartAttribute(12, SmartNames.PowerCycleCount, AbstractHarddrive.RawToInt),
		new SmartAttribute(171, SmartNames.ProgramFailCount, AbstractHarddrive.RawToInt),
		new SmartAttribute(172, SmartNames.EraseFailCount, AbstractHarddrive.RawToInt),
		new SmartAttribute(174, SmartNames.UnexpectedPowerLossCount, AbstractHarddrive.RawToInt),
		new SmartAttribute(177, SmartNames.WearRangeDelta, AbstractHarddrive.RawToInt),
		new SmartAttribute(181, SmartNames.AlternativeProgramFailCount, AbstractHarddrive.RawToInt),
		new SmartAttribute(182, SmartNames.AlternativeEraseFailCount, AbstractHarddrive.RawToInt),
		new SmartAttribute(187, SmartNames.UncorrectableErrorCount, AbstractHarddrive.RawToInt),
		new SmartAttribute(194, SmartNames.Temperature, (byte[] raw, byte value, IReadOnlyArray<IParameter> p) => (float)(int)value + (p?[0].Value ?? 0f), SensorType.Temperature, 0, SmartNames.Temperature, defaultHiddenSensor: true, new ParameterDescription[1]
		{
			new ParameterDescription("Offset [°C]", "Temperature offset of the thermal sensor.\nTemperature = Value + Offset.", 0f)
		}),
		new SmartAttribute(195, SmartNames.UnrecoverableEcc),
		new SmartAttribute(196, SmartNames.ReallocationEventCount, AbstractHarddrive.RawToInt),
		new SmartAttribute(231, SmartNames.RemainingLife, null, SensorType.Level, 0, SmartNames.RemainingLife),
		new SmartAttribute(233, SmartNames.ControllerWritesToNAND, AbstractHarddrive.RawToInt, SensorType.Data, 0, SmartNames.ControllerWritesToNAND),
		new SmartAttribute(234, SmartNames.HostWritesToController, AbstractHarddrive.RawToInt, SensorType.Data, 1, SmartNames.HostWritesToController),
		new SmartAttribute(241, SmartNames.HostWrites, AbstractHarddrive.RawToInt, SensorType.Data, 1, SmartNames.HostWrites),
		new SmartAttribute(242, SmartNames.HostReads, AbstractHarddrive.RawToInt, SensorType.Data, 2, SmartNames.HostReads)
	};

	private Sensor writeAmplification;

	public SSDSandforce(ISmart smart, string name, string firmwareRevision, int index, ISettings settings)
		: base(smart, name, firmwareRevision, index, smartAttributes, settings)
	{
		writeAmplification = new Sensor("Write Amplification", 1, SensorType.Factor, this, settings);
	}

	public override void UpdateAdditionalSensors(DriveAttributeValue[] values)
	{
		float? num = null;
		float? num2 = null;
		for (int i = 0; i < values.Length; i++)
		{
			DriveAttributeValue driveAttributeValue = values[i];
			if (driveAttributeValue.Identifier == 233)
			{
				num = AbstractHarddrive.RawToInt(driveAttributeValue.RawValue, driveAttributeValue.AttrValue, null);
			}
			if (driveAttributeValue.Identifier == 234)
			{
				num2 = AbstractHarddrive.RawToInt(driveAttributeValue.RawValue, driveAttributeValue.AttrValue, null);
			}
		}
		if (num.HasValue && num2.HasValue)
		{
			if (num2.Value > 0f)
			{
				writeAmplification.Value = num.Value / num2.Value;
			}
			else
			{
				writeAmplification.Value = 0f;
			}
			ActivateSensor(writeAmplification);
		}
	}
}
