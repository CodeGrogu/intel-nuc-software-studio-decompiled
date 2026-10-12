using System.Collections.ObjectModel;
using System.Windows.Media;
using MobileKeyboardLedMgr.Models.Profiles;
using UniwillSdkClient.UniwillService;

namespace MobileKeyboardLedMgr.Models.KingCounty;

internal static class MonocolorPattern
{
	internal static ObservableCollection<KeyColorData> Generate(RGBKeyboardCapabilities keyboardType, Color color, bool enabled)
	{
		ObservableCollection<KeyColorData> observableCollection = new ObservableCollection<KeyColorData>();
		for (int i = 0; i < 6; i++)
		{
			observableCollection.AddRange(GenerateRow(color, KeyboardCapabilities.GetFirstKeyInRow(keyboardType, i), KeyboardCapabilities.GetLastKeyInRow(keyboardType, i), enabled));
		}
		return observableCollection;
	}

	internal static ObservableCollection<KeyColorData> GenerateRow(Color color, int startIndex, int endIndex, bool enabled)
	{
		ObservableCollection<KeyColorData> observableCollection = new ObservableCollection<KeyColorData>();
		for (int i = startIndex; i <= endIndex; i++)
		{
			observableCollection.Add(new KeyColorData(i, color, enabled));
		}
		return observableCollection;
	}
}
