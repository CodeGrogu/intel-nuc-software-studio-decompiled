using System.Collections.Generic;
using OpenHardwareMonitor.Collections;

namespace OpenHardwareMonitor.Hardware.HDD;

[NamePrefix("")]
internal class GenericHarddisk : AbstractHarddrive
{
	private static readonly List<SmartAttribute> smartAttributes = new List<SmartAttribute>
	{
		new SmartAttribute(1, SmartNames.ReadErrorRate),
		new SmartAttribute(2, SmartNames.ThroughputPerformance),
		new SmartAttribute(3, SmartNames.SpinUpTime),
		new SmartAttribute(4, SmartNames.StartStopCount, AbstractHarddrive.RawToInt),
		new SmartAttribute(5, SmartNames.ReallocatedSectorsCount),
		new SmartAttribute(6, SmartNames.ReadChannelMargin),
		new SmartAttribute(7, SmartNames.SeekErrorRate),
		new SmartAttribute(8, SmartNames.SeekTimePerformance),
		new SmartAttribute(9, SmartNames.PowerOnHours, AbstractHarddrive.RawToInt),
		new SmartAttribute(10, SmartNames.SpinRetryCount),
		new SmartAttribute(11, SmartNames.RecalibrationRetries),
		new SmartAttribute(12, SmartNames.PowerCycleCount, AbstractHarddrive.RawToInt),
		new SmartAttribute(13, SmartNames.SoftReadErrorRate),
		new SmartAttribute(170, SmartNames.Unknown),
		new SmartAttribute(171, SmartNames.Unknown),
		new SmartAttribute(172, SmartNames.Unknown),
		new SmartAttribute(183, SmartNames.SataDownshiftErrorCount, AbstractHarddrive.RawToInt),
		new SmartAttribute(184, SmartNames.EndToEndError),
		new SmartAttribute(185, SmartNames.HeadStability),
		new SmartAttribute(186, SmartNames.InducedOpVibrationDetection),
		new SmartAttribute(187, SmartNames.ReportedUncorrectableErrors, AbstractHarddrive.RawToInt),
		new SmartAttribute(188, SmartNames.CommandTimeout, AbstractHarddrive.RawToInt),
		new SmartAttribute(189, SmartNames.HighFlyWrites),
		new SmartAttribute(191, SmartNames.GSenseErrorRate),
		new SmartAttribute(192, SmartNames.EmergencyRetractCycleCount),
		new SmartAttribute(193, SmartNames.LoadCycleCount),
		new SmartAttribute(195, SmartNames.HardwareEccRecovered),
		new SmartAttribute(196, SmartNames.ReallocationEventCount),
		new SmartAttribute(197, SmartNames.CurrentPendingSectorCount),
		new SmartAttribute(198, SmartNames.UncorrectableSectorCount),
		new SmartAttribute(199, SmartNames.UltraDmaCrcErrorCount),
		new SmartAttribute(200, SmartNames.WriteErrorRate),
		new SmartAttribute(202, SmartNames.DataAddressMarkErrors),
		new SmartAttribute(203, SmartNames.RunOutCancel),
		new SmartAttribute(204, SmartNames.SoftEccCorrection),
		new SmartAttribute(205, SmartNames.ThermalAsperityRate),
		new SmartAttribute(206, SmartNames.FlyingHeight),
		new SmartAttribute(207, SmartNames.SpinHighCurrent),
		new SmartAttribute(208, SmartNames.SpinBuzz),
		new SmartAttribute(209, SmartNames.OfflineSeekPerformance),
		new SmartAttribute(211, SmartNames.VibrationDuringWrite),
		new SmartAttribute(212, SmartNames.ShockDuringWrite),
		new SmartAttribute(220, SmartNames.DiskShift),
		new SmartAttribute(221, SmartNames.AlternativeGSenseErrorRate),
		new SmartAttribute(222, SmartNames.LoadedHours),
		new SmartAttribute(223, SmartNames.LoadUnloadRetryCount),
		new SmartAttribute(224, SmartNames.LoadFriction),
		new SmartAttribute(225, SmartNames.LoadUnloadCycleCount),
		new SmartAttribute(226, SmartNames.LoadInTime),
		new SmartAttribute(227, SmartNames.TorqueAmplificationCount),
		new SmartAttribute(228, SmartNames.PowerOffRetractCycle),
		new SmartAttribute(230, SmartNames.GmrHeadAmplitude),
		new SmartAttribute(232, SmartNames.EnduranceRemaining),
		new SmartAttribute(233, SmartNames.PowerOnHours),
		new SmartAttribute(240, SmartNames.HeadFlyingHours),
		new SmartAttribute(241, SmartNames.TotalLbasWritten),
		new SmartAttribute(242, SmartNames.TotalLbasRead),
		new SmartAttribute(250, SmartNames.ReadErrorRetryRate),
		new SmartAttribute(254, SmartNames.FreeFallProtection),
		new SmartAttribute(194, SmartNames.Temperature, (byte[] r, byte v, IReadOnlyArray<IParameter> p) => (float)(int)r[0] + (p?[0].Value ?? 0f), SensorType.Temperature, 0, SmartNames.Temperature, defaultHiddenSensor: false, new ParameterDescription[1]
		{
			new ParameterDescription("Offset [°C]", "Temperature offset of the thermal sensor.\nTemperature = Value + Offset.", 0f)
		}),
		new SmartAttribute(231, SmartNames.Temperature, (byte[] r, byte v, IReadOnlyArray<IParameter> p) => (float)(int)r[0] + (p?[0].Value ?? 0f), SensorType.Temperature, 0, SmartNames.Temperature, defaultHiddenSensor: false, new ParameterDescription[1]
		{
			new ParameterDescription("Offset [°C]", "Temperature offset of the thermal sensor.\nTemperature = Value + Offset.", 0f)
		}),
		new SmartAttribute(190, SmartNames.TemperatureDifferenceFrom100, (byte[] r, byte v, IReadOnlyArray<IParameter> p) => (float)(int)r[0] + (p?[0].Value ?? 0f), SensorType.Temperature, 0, "Temperature", defaultHiddenSensor: false, new ParameterDescription[1]
		{
			new ParameterDescription("Offset [°C]", "Temperature offset of the thermal sensor.\nTemperature = Value + Offset.", 0f)
		})
	};

	public GenericHarddisk(ISmart smart, string name, string firmwareRevision, int index, ISettings settings)
		: base(smart, name, firmwareRevision, index, smartAttributes, settings)
	{
	}
}
