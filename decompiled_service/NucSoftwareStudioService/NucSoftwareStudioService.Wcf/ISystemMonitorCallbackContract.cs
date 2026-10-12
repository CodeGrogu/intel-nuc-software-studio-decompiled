using System.Collections.Generic;
using System.ServiceModel;
using SystemMonitorLibrary.Hardware;
using PerformanceTuningLibrary;

namespace NucSoftwareStudioService.Wcf;

public interface ISystemMonitorCallbackContract
{
	[OperationContract(IsOneWay = true)]
	void SystemMonitorDataCallback(List<FanSpeed> FanData, SystemMonitorData SystemData);
}
