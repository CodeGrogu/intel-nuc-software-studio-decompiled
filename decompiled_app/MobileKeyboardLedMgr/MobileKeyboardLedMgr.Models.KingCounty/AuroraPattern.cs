using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using MobileKeyboardLedMgr.Models.Profiles;
using UniwillSdkClient.UniwillService;

namespace MobileKeyboardLedMgr.Models.KingCounty;

internal static class AuroraPattern
{
	internal static ObservableCollection<KeyColorData> Generate(RGBKeyboardCapabilities keyboardType, SortedList<int, Color> colorsList)
	{
		ObservableCollection<KeyColorData> observableCollection = new ObservableCollection<KeyColorData>();
		List<Color> list = new List<Color>(colorsList.Values);
		if (list.Count < 6)
		{
			int num = 6 - list.Count;
			for (int i = 0; i < num; i++)
			{
				list.Add(list[i]);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			observableCollection.AddRange(MonocolorPattern.GenerateRow(list[j], KeyboardCapabilities.GetFirstKeyInRow(keyboardType, j), KeyboardCapabilities.GetLastKeyInRow(keyboardType, j), enabled: true));
		}
		return PresetPatternsKC.UpdateMultiRowKeys(keyboardType, observableCollection);
	}
}
