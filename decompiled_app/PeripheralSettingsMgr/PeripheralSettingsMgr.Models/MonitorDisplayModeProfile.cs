using Newtonsoft.Json;
using PeripheralSettingsMgr.Models.DisplayModes;
using Prism.Mvvm;

namespace PeripheralSettingsMgr.Models;

public class MonitorDisplayModeProfile : BindableBase
{
	public delegate bool MonitorSetDisplayDelegate(DisplaySetting setting);

	private MonitorCapabilities _monitorCapabilities;

	private DisplaySetting _ac;

	private DisplaySetting _dc;

	public string MonitorName { get; }

	public DisplaySetting AcSettings
	{
		get
		{
			return _ac;
		}
		set
		{
			SetProperty(ref _ac, value, "AcSettings");
		}
	}

	public DisplaySetting DcSettings
	{
		get
		{
			return _dc;
		}
		set
		{
			SetProperty(ref _dc, value, "DcSettings");
		}
	}

	public MonitorDisplayModeProfile(MonitorSetting monitor)
	{
		MonitorName = monitor.Name;
		_monitorCapabilities = monitor.Capabilities;
		DisplaySetting displaySetting = monitor.GetDisplaySetting();
		AcSettings = new DisplaySetting(_monitorCapabilities, displaySetting.Brightness, displaySetting.Contrast, displaySetting.ColorTempType, displaySetting.Red, displaySetting.Green, displaySetting.Blue, monitor.SetDisplaySetting);
		DcSettings = new DisplaySetting(_monitorCapabilities, displaySetting.Brightness, displaySetting.Contrast, displaySetting.ColorTempType, displaySetting.Red, displaySetting.Green, displaySetting.Blue, monitor.SetDisplaySetting);
	}

	public MonitorDisplayModeProfile(MonitorDisplayModeProfile monitorDisplayModeProfile)
	{
		MonitorName = monitorDisplayModeProfile.MonitorName;
		_monitorCapabilities = monitorDisplayModeProfile._monitorCapabilities;
		AcSettings = new DisplaySetting(_monitorCapabilities, monitorDisplayModeProfile.AcSettings.Brightness, monitorDisplayModeProfile.AcSettings.Contrast, monitorDisplayModeProfile.AcSettings.ColorTempType, monitorDisplayModeProfile.AcSettings.Red, monitorDisplayModeProfile.AcSettings.Green, monitorDisplayModeProfile.AcSettings.Blue, monitorDisplayModeProfile.AcSettings.SetDisplay);
		DcSettings = new DisplaySetting(_monitorCapabilities, monitorDisplayModeProfile.DcSettings.Brightness, monitorDisplayModeProfile.DcSettings.Contrast, monitorDisplayModeProfile.DcSettings.ColorTempType, monitorDisplayModeProfile.DcSettings.Red, monitorDisplayModeProfile.DcSettings.Green, monitorDisplayModeProfile.DcSettings.Blue, monitorDisplayModeProfile.DcSettings.SetDisplay);
	}

	[JsonConstructor]
	public MonitorDisplayModeProfile(string monitorName, MonitorCapabilities capabilities, DisplaySetting ac, DisplaySetting dc)
	{
		MonitorName = monitorName;
		_monitorCapabilities = capabilities;
		AcSettings = ac;
		DcSettings = dc;
	}
}
