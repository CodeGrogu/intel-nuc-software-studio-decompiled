using System.Collections.Generic;
using OpenHardwareMonitor.Collections;

namespace OpenHardwareMonitor.Hardware.HDD;

[NamePrefix("")]
[RequireSmart(171)]
[RequireSmart(172)]
[RequireSmart(173)]
[RequireSmart(174)]
[RequireSmart(196)]
[RequireSmart(202)]
[RequireSmart(206)]
internal class SSDMicron : AbstractHarddrive
{
	private static readonly IEnumerable<SmartAttribute> smartAttributes = new List<SmartAttribute>
	{
		new SmartAttribute(1, SmartNames.ReadErrorRate, AbstractHarddrive.RawToInt),
		new SmartAttribute(5, SmartNames.ReallocatedNANDBlockCount, AbstractHarddrive.RawToInt),
		new SmartAttribute(9, SmartNames.PowerOnHours, AbstractHarddrive.RawToInt),
		new SmartAttribute(12, SmartNames.PowerCycleCount, AbstractHarddrive.RawToInt),
		new SmartAttribute(170, SmartNames.NewFailingBlockCount, AbstractHarddrive.RawToInt),
		new SmartAttribute(171, SmartNames.ProgramFailCount, AbstractHarddrive.RawToInt),
		new SmartAttribute(172, SmartNames.EraseFailCount, AbstractHarddrive.RawToInt),
		new SmartAttribute(173, SmartNames.WearLevelingCount, AbstractHarddrive.RawToInt),
		new SmartAttribute(174, SmartNames.UnexpectedPowerLossCount, AbstractHarddrive.RawToInt),
		new SmartAttribute(180, SmartNames.UnusedReserveNANDBlocks, AbstractHarddrive.RawToInt),
		new SmartAttribute(181, SmartNames.Non4kAlignedAccess, (byte[] raw, byte value, IReadOnlyArray<IParameter> p) => 60000f * (float)((raw[5] << 8) | raw[4])),
		new SmartAttribute(183, SmartNames.SataDownshiftErrorCount, AbstractHarddrive.RawToInt),
		new SmartAttribute(184, SmartNames.ErrorCorrectionCount, AbstractHarddrive.RawToInt),
		new SmartAttribute(187, SmartNames.ReportedUncorrectableErrors, AbstractHarddrive.RawToInt),
		new SmartAttribute(188, SmartNames.CommandTimeout, AbstractHarddrive.RawToInt),
		new SmartAttribute(189, SmartNames.FactoryBadBlockCount, AbstractHarddrive.RawToInt),
		new SmartAttribute(194, SmartNames.Temperature, AbstractHarddrive.RawToInt),
		new SmartAttribute(196, SmartNames.ReallocationEventCount, AbstractHarddrive.RawToInt),
		new SmartAttribute(197, SmartNames.CurrentPendingSectorCount),
		new SmartAttribute(198, SmartNames.OffLineUncorrectableErrorCount, AbstractHarddrive.RawToInt),
		new SmartAttribute(199, SmartNames.UltraDmaCrcErrorCount, AbstractHarddrive.RawToInt),
		new SmartAttribute(202, SmartNames.RemainingLife, (byte[] raw, byte value, IReadOnlyArray<IParameter> p) => 100f - AbstractHarddrive.RawToInt(raw, value, p), SensorType.Level, 0, SmartNames.RemainingLife),
		new SmartAttribute(206, SmartNames.WriteErrorRate, (byte[] raw, byte value, IReadOnlyArray<IParameter> p) => 60000f * (float)((raw[1] << 8) | raw[0])),
		new SmartAttribute(210, SmartNames.SuccessfulRAINRecoveryCount, AbstractHarddrive.RawToInt),
		new SmartAttribute(246, SmartNames.TotalLbasWritten, (byte[] r, byte v, IReadOnlyArray<IParameter> p) => (float)(long)(((ulong)r[5] << 40) | ((ulong)r[4] << 32) | ((ulong)r[3] << 24) | ((ulong)r[2] << 16) | ((ulong)r[1] << 8) | r[0]) * 4.7683716E-07f, SensorType.Data, 0, "Total Bytes Written"),
		new SmartAttribute(247, SmartNames.HostProgramNANDPagesCount, AbstractHarddrive.RawToInt),
		new SmartAttribute(248, SmartNames.FTLProgramNANDPagesCount, AbstractHarddrive.RawToInt)
	};

	private Sensor temperature;

	private Sensor writeAmplification;

	public SSDMicron(ISmart smart, string name, string firmwareRevision, int index, ISettings settings)
		: base(smart, name, firmwareRevision, index, smartAttributes, settings)
	{
		temperature = new Sensor("Temperature", 0, defaultHidden: false, SensorType.Temperature, this, new ParameterDescription[1]
		{
			new ParameterDescription("Offset [°C]", "Temperature offset of the thermal sensor.\nTemperature = Value + Offset.", 0f)
		}, settings);
		writeAmplification = new Sensor("Write Amplification", 0, SensorType.Factor, this, settings);
	}

	public override void UpdateAdditionalSensors(DriveAttributeValue[] values)
	{
		float? num = null;
		float? num2 = null;
		for (int i = 0; i < values.Length; i++)
		{
			DriveAttributeValue driveAttributeValue = values[i];
			if (driveAttributeValue.Identifier == 247)
			{
				num = AbstractHarddrive.RawToInt(driveAttributeValue.RawValue, driveAttributeValue.AttrValue, null);
			}
			if (driveAttributeValue.Identifier == 248)
			{
				num2 = AbstractHarddrive.RawToInt(driveAttributeValue.RawValue, driveAttributeValue.AttrValue, null);
			}
			if (driveAttributeValue.Identifier == 194)
			{
				temperature.Value = (float)(int)driveAttributeValue.RawValue[0] + temperature.Parameters[0].Value;
				if (driveAttributeValue.RawValue[0] != 0)
				{
					ActivateSensor(temperature);
				}
			}
		}
		if (num.HasValue && num2.HasValue)
		{
			if (num.Value > 0f)
			{
				writeAmplification.Value = (num.Value + num2) / num.Value;
			}
			else
			{
				writeAmplification.Value = 0f;
			}
			ActivateSensor(writeAmplification);
		}
	}
}
