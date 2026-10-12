using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using MobileKeyboardLedMgr.Models.Profiles;
using UniwillSdkClient.UniwillService;

namespace MobileKeyboardLedMgr.Models.KingCounty;

internal static class BreathingPattern
{
	internal static ObservableCollection<KeyColorData> Generate(RGBKeyboardCapabilities keyboardType, SortedList<int, Color> colorsList)
	{
		ObservableCollection<KeyColorData> keys = MonocolorPattern.Generate(keyboardType, colorsList.Values[0], enabled: true);
		ObservableCollection<KeyColorData> observableCollection = new ObservableCollection<KeyColorData>();
		for (int i = 0; i < 6; i++)
		{
			observableCollection.AddRange(LightenKeysAwayFromCenter(keys, KeyboardCapabilities.GetFirstKeyInRow(keyboardType, i), KeyboardCapabilities.GetLastKeyInRow(keyboardType, i)));
		}
		return PresetPatternsKC.UpdateMultiRowKeys(keyboardType, observableCollection);
	}

	private static ObservableCollection<KeyColorData> LightenKeysAwayFromCenter(ObservableCollection<KeyColorData> keys, int startIndex, int endIndex)
	{
		int num = startIndex + (endIndex - startIndex) / 2;
		int j;
		for (j = startIndex; j <= endIndex; j++)
		{
			int value = Convert.ToInt32((double)(255 - Math.Abs(num - j) * 31));
			Color color = keys.FirstOrDefault((KeyColorData x) => x.Index == j)?.SelectedColor ?? default(Color);
			Color selectedColor = Color.FromArgb(Convert.ToByte(value), color.R, color.G, color.B);
			keys[keys.IndexOf(keys.FirstOrDefault((KeyColorData x) => x.Index == j))].SelectedColor = selectedColor;
		}
		return keys;
	}
}
