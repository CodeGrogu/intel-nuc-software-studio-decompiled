using System.Collections.ObjectModel;
using System.Windows.Media;
using MobileKeyboardLedMgr.Constants;

namespace MobileKeyboardLedMgr.Models.Profiles;

public class RecentColorsData
{
	public ObservableCollection<Color> Recent { get; set; }

	public RecentColorsData(ObservableCollection<Color> recentColors)
	{
		Recent = recentColors;
	}

	public RecentColorsData()
	{
	}

	public void Initialize()
	{
		Recent = new ObservableCollection<Color>(DefaultColors.COLORS);
	}
}
