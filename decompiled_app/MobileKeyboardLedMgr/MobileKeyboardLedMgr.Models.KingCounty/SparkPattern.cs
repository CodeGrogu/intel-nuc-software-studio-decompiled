using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using MobileKeyboardLedMgr.Models.Profiles;
using UniwillSdkClient.UniwillService;

namespace MobileKeyboardLedMgr.Models.KingCounty;

internal static class SparkPattern
{
	internal static ObservableCollection<KeyColorData> Generate(RGBKeyboardCapabilities keyboardType, SortedList<int, Color> colorsList)
	{
		ObservableCollection<KeyColorData> observableCollection = new ObservableCollection<KeyColorData>();
		observableCollection.AddRange(MonocolorPattern.Generate(keyboardType, Colors.DimGray, enabled: false));
		ObservableCollection<KeyColorData> observableCollection2 = new ObservableCollection<KeyColorData>();
		switch (colorsList.Count)
		{
		case 1:
			observableCollection2.AddRange(GenerateSparks1(keyboardType, colorsList.Values[0]));
			observableCollection2.AddRange(GenerateSparks2(keyboardType, colorsList.Values[0]));
			break;
		case 2:
			observableCollection2.AddRange(GenerateSparks1(keyboardType, colorsList.Values[0]));
			observableCollection2.AddRange(GenerateSparks2(keyboardType, colorsList.Values[1]));
			break;
		case 3:
			observableCollection2.AddRange(GenerateSparks1(keyboardType, colorsList.Values[0]));
			observableCollection2.AddRange(GenerateSparks2(keyboardType, colorsList.Values[1]));
			observableCollection2.AddRange(GenerateSparks3(keyboardType, colorsList.Values[2]));
			break;
		case 4:
			observableCollection2.AddRange(GenerateSparks1(keyboardType, colorsList.Values[0]));
			observableCollection2.AddRange(GenerateSparks2(keyboardType, colorsList.Values[1]));
			observableCollection2.AddRange(GenerateSparks3(keyboardType, colorsList.Values[2]));
			observableCollection2.AddRange(GenerateSparks4(keyboardType, colorsList.Values[3]));
			break;
		case 5:
			observableCollection2.AddRange(GenerateSparks1(keyboardType, colorsList.Values[0]));
			observableCollection2.AddRange(GenerateSparks2(keyboardType, colorsList.Values[2]));
			observableCollection2.AddRange(GenerateSparks3(keyboardType, colorsList.Values[3]));
			observableCollection2.AddRange(GenerateSparks4(keyboardType, colorsList.Values[4]));
			break;
		case 6:
			observableCollection2.AddRange(GenerateSparks1(keyboardType, colorsList.Values[0]));
			observableCollection2.AddRange(GenerateSparks2(keyboardType, colorsList.Values[2]));
			observableCollection2.AddRange(GenerateSparks3(keyboardType, colorsList.Values[4]));
			observableCollection2.AddRange(GenerateSparks4(keyboardType, colorsList.Values[5]));
			break;
		case 7:
			observableCollection2.AddRange(GenerateSparks1(keyboardType, colorsList.Values[0]));
			observableCollection2.AddRange(GenerateSparks2(keyboardType, colorsList.Values[2]));
			observableCollection2.AddRange(GenerateSparks3(keyboardType, colorsList.Values[4]));
			observableCollection2.AddRange(GenerateSparks4(keyboardType, colorsList.Values[6]));
			break;
		}
		foreach (KeyColorData k in observableCollection2)
		{
			observableCollection[observableCollection.IndexOf(observableCollection.FirstOrDefault((KeyColorData x) => x.Index == k.Index))].SelectedColor = k.SelectedColor;
		}
		return PresetPatternsKC.UpdateMultiRowKeys(keyboardType, observableCollection);
	}

	private static ObservableCollection<KeyColorData> GenerateSparks1(RGBKeyboardCapabilities keyboardType, Color color)
	{
		return new ObservableCollection<KeyColorData>
		{
			new KeyColorData(KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0) + 1, color, enabled: true),
			new KeyColorData(KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0) + 2, color, enabled: true),
			new KeyColorData(KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0) + 3, color, enabled: true),
			new KeyColorData(KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 1) + 1, color, enabled: true),
			new KeyColorData(KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 1) + 2, color, enabled: true),
			new KeyColorData(KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0) + 3, color, enabled: true),
			new KeyColorData(KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 2) + 2, color, enabled: true)
		};
	}

	private static ObservableCollection<KeyColorData> GenerateSparks2(RGBKeyboardCapabilities keyboardType, Color color)
	{
		return new ObservableCollection<KeyColorData>
		{
			new KeyColorData(KeyboardCapabilities.GetLastKeyInRow(keyboardType, 5) - 1, color, enabled: true),
			new KeyColorData(KeyboardCapabilities.GetLastKeyInRow(keyboardType, 5) - 2, color, enabled: true),
			new KeyColorData(KeyboardCapabilities.GetLastKeyInRow(keyboardType, 5) - 3, color, enabled: true),
			new KeyColorData(KeyboardCapabilities.GetLastKeyInRow(keyboardType, 4) - 1, color, enabled: true),
			new KeyColorData(KeyboardCapabilities.GetLastKeyInRow(keyboardType, 4) - 2, color, enabled: true),
			new KeyColorData(KeyboardCapabilities.GetLastKeyInRow(keyboardType, 4) - 3, color, enabled: true),
			new KeyColorData(KeyboardCapabilities.GetLastKeyInRow(keyboardType, 3) - 2, color, enabled: true)
		};
	}

	private static ObservableCollection<KeyColorData> GenerateSparks3(RGBKeyboardCapabilities keyboardType, Color color)
	{
		return new ObservableCollection<KeyColorData>
		{
			new KeyColorData(KeyboardCapabilities.GetLastKeyInRow(keyboardType, 0) - 3, color, enabled: true),
			new KeyColorData(KeyboardCapabilities.GetLastKeyInRow(keyboardType, 0) - 4, color, enabled: true),
			new KeyColorData(KeyboardCapabilities.GetLastKeyInRow(keyboardType, 0) - 5, color, enabled: true),
			new KeyColorData(KeyboardCapabilities.GetLastKeyInRow(keyboardType, 1) - 3, color, enabled: true),
			new KeyColorData(KeyboardCapabilities.GetLastKeyInRow(keyboardType, 1) - 4, color, enabled: true),
			new KeyColorData(KeyboardCapabilities.GetLastKeyInRow(keyboardType, 1) - 5, color, enabled: true),
			new KeyColorData(KeyboardCapabilities.GetLastKeyInRow(keyboardType, 2) - 4, color, enabled: true)
		};
	}

	private static ObservableCollection<KeyColorData> GenerateSparks4(RGBKeyboardCapabilities keyboardType, Color color)
	{
		return new ObservableCollection<KeyColorData>
		{
			new KeyColorData(KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 4) + 4, color, enabled: true),
			new KeyColorData(KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 4) + 5, color, enabled: true),
			new KeyColorData(KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 4) + 6, color, enabled: true),
			new KeyColorData(KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 3) + 4, color, enabled: true),
			new KeyColorData(KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 3) + 5, color, enabled: true),
			new KeyColorData(KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 3) + 6, color, enabled: true),
			new KeyColorData(KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 2) + 5, color, enabled: true)
		};
	}
}
