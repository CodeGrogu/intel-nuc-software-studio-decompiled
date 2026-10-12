using System.Runtime.Serialization;
using System.Windows.Media;
using NucLedMgr.WMI.Enums;

namespace NucLedMgr.WMI.Models.Indicators;

[DataContract]
public class CpuIndicator
{
	[DataMember]
	public LedType LedType { get; set; }

	[DataMember]
	public bool? GreenToRed { get; set; }

	[DataMember]
	public Color CustomColor { get; set; }

	public CpuIndicator(LedType ledType, bool? greenToRed)
	{
		LedType = ledType;
		GreenToRed = greenToRed;
	}
}
