using System.Collections.ObjectModel;
using Prism.Commands;
using UniwillSdkClient.UniwillService;

namespace MobileDisplayModeMgr.Interfaces;

public interface IMobileDisplayModeSettingsMgr
{
	int BrightnessContrastMin { get; }

	int BrightnessContrastMax { get; }

	int ColorMin { get; }

	int ColorMax { get; }

	int ColorTempMin { get; }

	int ColorTempMax { get; }

	int ColorTempInterval { get; }

	bool BrightnessSupported { get; set; }

	bool ContrastSupported { get; set; }

	bool ColorTemperatureSupported { get; set; }

	bool RedSupported { get; set; }

	bool GreenSupported { get; set; }

	bool BlueSupported { get; set; }

	ObservableCollection<DisplayMode> Modes { get; set; }

	DisplayMode CurrentMode { get; set; }

	int AcBrightness { get; set; }

	int DcBrightness { get; set; }

	float CurrentContrast { get; set; }

	int CurrentTemp { get; set; }

	int CurrentRed { get; set; }

	int CurrentGreen { get; set; }

	int CurrentBlue { get; set; }

	bool ErrorMessage { get; set; }

	DelegateCommand<DisplayMode> SelectMode { get; set; }

	DelegateCommand ResetMode { get; set; }
}
