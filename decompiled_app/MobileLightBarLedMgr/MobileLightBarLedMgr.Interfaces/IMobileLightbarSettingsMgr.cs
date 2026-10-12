using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Prism.Commands;

namespace MobileLightBarLedMgr.Interfaces;

public interface IMobileLightbarSettingsMgr
{
	int RgbMin { get; }

	int RgbMax { get; }

	bool LightbarEnabled { get; set; }

	bool AcEnabled { get; set; }

	bool DcEnabled { get; set; }

	bool ErrorMessage { get; set; }

	ObservableCollection<Color> RecentColors { get; set; }

	string CurrentAcTimerValue { get; set; }

	ObservableCollection<string> AcTimerValues { get; }

	string CurrentDcTimerValue { get; }

	ObservableCollection<string> DcTimerValues { get; }

	DelegateCommand<Tuple<Color, int>> SelectColor { get; set; }

	DelegateCommand SendToSdk { get; set; }

	DelegateCommand ResetAc { get; set; }

	int AcRed { get; set; }

	int AcGreen { get; set; }

	int AcBlue { get; set; }

	Color AcColor { get; set; }

	Color AcNewColor { get; set; }

	bool AcRainbow { get; set; }

	bool BreathingEnabled { get; set; }

	DelegateCommand ResetDc { get; set; }

	int DcRed { get; set; }

	int DcGreen { get; set; }

	int DcBlue { get; set; }

	Color DcColor { get; set; }

	Color DcNewColor { get; set; }

	bool DcRainbow { get; set; }

	void SendProfileToSDK();
}
