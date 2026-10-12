using System.Collections.ObjectModel;
using System.Windows.Media;

namespace NucLedMgr.Interfaces;

public interface IColorManager
{
	Color CurrentColor { get; set; }

	ObservableCollection<Color> StandardColors { get; }

	ObservableCollection<Color> RecentColors { get; }

	ObservableCollection<Color> PantherPowerColors { get; }

	Color First { get; set; }

	Color Second { get; set; }

	Color Third { get; set; }
}
