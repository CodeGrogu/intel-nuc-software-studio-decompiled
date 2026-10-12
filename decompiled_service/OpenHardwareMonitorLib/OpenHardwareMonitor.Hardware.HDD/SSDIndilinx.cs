using System.Collections.Generic;

namespace OpenHardwareMonitor.Hardware.HDD;

[NamePrefix("")]
[RequireSmart(1)]
[RequireSmart(9)]
[RequireSmart(12)]
[RequireSmart(209)]
[RequireSmart(206)]
[RequireSmart(207)]
internal class SSDIndilinx : AbstractHarddrive
{
	private static readonly IEnumerable<SmartAttribute> smartAttributes = new List<SmartAttribute>
	{
		new SmartAttribute(1, SmartNames.ReadErrorRate),
		new SmartAttribute(9, SmartNames.PowerOnHours),
		new SmartAttribute(12, SmartNames.PowerCycleCount),
		new SmartAttribute(184, SmartNames.InitialBadBlockCount),
		new SmartAttribute(195, SmartNames.ProgramFailure),
		new SmartAttribute(196, SmartNames.EraseFailure),
		new SmartAttribute(197, SmartNames.ReadFailure),
		new SmartAttribute(198, SmartNames.SectorsRead),
		new SmartAttribute(199, SmartNames.SectorsWritten),
		new SmartAttribute(200, SmartNames.ReadCommands),
		new SmartAttribute(201, SmartNames.WriteCommands),
		new SmartAttribute(202, SmartNames.BitErrors),
		new SmartAttribute(203, SmartNames.CorrectedErrors),
		new SmartAttribute(204, SmartNames.BadBlockFullFlag),
		new SmartAttribute(205, SmartNames.MaxCellCycles),
		new SmartAttribute(206, SmartNames.MinErase),
		new SmartAttribute(207, SmartNames.MaxErase),
		new SmartAttribute(208, SmartNames.AverageEraseCount),
		new SmartAttribute(209, SmartNames.RemainingLife, null, SensorType.Level, 0, SmartNames.RemainingLife),
		new SmartAttribute(210, SmartNames.UnknownUnique),
		new SmartAttribute(211, SmartNames.SataErrorCountCrc),
		new SmartAttribute(212, SmartNames.SataErrorCountHandshake)
	};

	public SSDIndilinx(ISmart smart, string name, string firmwareRevision, int index, ISettings settings)
		: base(smart, name, firmwareRevision, index, smartAttributes, settings)
	{
	}
}
