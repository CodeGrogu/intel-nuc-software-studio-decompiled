using System.ServiceModel;
using NucLedMgr.WMI.Models.Indicators;

namespace NucLedMgr.WMI.CpuUsage;

[ServiceContract]
public interface ICpuService
{
	[OperationContract]
	void AddSetting(CpuIndicator setting);

	[OperationContract]
	void RemoveSetting(CpuIndicator setting);
}
