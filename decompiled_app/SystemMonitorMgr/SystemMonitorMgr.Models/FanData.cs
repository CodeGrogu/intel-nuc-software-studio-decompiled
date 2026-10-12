using IntelSdkClient.SystemMonitorService;
using Prism.Mvvm;

namespace SystemMonitorMgr.Models;

public class FanData : BindableBase
{
	private int _rpm;

	public FanType Type { get; private set; }

	public int Rpm
	{
		get
		{
			return _rpm;
		}
		set
		{
			SetProperty(ref _rpm, value, "Rpm");
		}
	}

	public FanData(FanType type, int rpm = 0)
	{
		Type = type;
		Rpm = rpm;
	}
}
