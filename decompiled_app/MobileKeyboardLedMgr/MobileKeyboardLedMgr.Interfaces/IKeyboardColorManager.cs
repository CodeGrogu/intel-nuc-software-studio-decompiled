using System.Collections.ObjectModel;
using System.Windows.Media;

namespace MobileKeyboardLedMgr.Interfaces;

public interface IKeyboardColorManager
{
	Color CurrentColor { get; set; }

	ObservableCollection<Color> StandardColors { get; }

	ObservableCollection<Color> RecentColors { get; }

	Color First { get; set; }

	Color Second { get; set; }

	Color Third { get; set; }

	void SaveRecentColors(ObservableCollection<Color> recent);
}
