using System.Collections.Generic;
using System.Collections.ObjectModel;
using SystemMonitorMgr.Models;
using Infrastructure.Models;

namespace SystemMonitorMgr.Interfaces;

public interface ISystemMonitor
{
	ClientState Subscribed { get; set; }

	ObservableCollection<SystemMonitorData> Data { get; set; }

	List<FanData> FanData { get; set; }

	string CpuName { get; set; }

	string GpuName { get; set; }
}
