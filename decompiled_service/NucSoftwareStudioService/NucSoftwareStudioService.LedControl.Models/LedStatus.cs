using System.Collections.Generic;
using System.Runtime.Serialization;
using SerialDeviceAccessLibrary;

namespace NucSoftwareStudioService.LedControl.Models;

[DataContract]
public class LedStatus
{
	[DataMember]
	public List<LedPattern> LedPatternList { get; set; } = new List<LedPattern>();

	[DataMember]
	public BntStatus ButtonStatus { get; set; }
}
