using System.Windows.Media;

namespace NucLedMgr.WMI.Models;

public static class WmiColor
{
	public delegate byte ColorToWmiValueConverter(Color Color);

	public delegate Color WmiValueToColorConverter(byte Value);
}
