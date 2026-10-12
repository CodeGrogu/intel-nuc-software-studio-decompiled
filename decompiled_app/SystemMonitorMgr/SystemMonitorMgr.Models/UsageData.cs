using SystemMonitorMgr.Enums;
using Prism.Mvvm;

namespace SystemMonitorMgr.Models;

public class UsageData : BindableBase
{
	private int? _temp;

	private int _usage;

	public SystemDataType Type { get; private set; }

	public int? Temp
	{
		get
		{
			return _temp;
		}
		set
		{
			SetProperty(ref _temp, value, "Temp");
		}
	}

	public int Usage
	{
		get
		{
			return _usage;
		}
		set
		{
			SetProperty(ref _usage, value, "Usage");
		}
	}

	public UsageData(SystemDataType type)
	{
		Type = type;
		_temp = 0;
		_usage = 0;
	}

	public UsageData(SystemDataType type, int? temp, int usage)
	{
		Type = type;
		if (temp == -1)
		{
			_temp = null;
		}
		else
		{
			_temp = temp;
		}
		_usage = usage;
	}
}
