using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using MobileKeyboardLedMgr.Constants;
using MobileKeyboardLedMgr.Models.Profiles;
using UniwillSdkClient.UniwillService;

namespace MobileKeyboardLedMgr.Models.KingCounty;

internal static class MarqueePattern
{
	internal static ObservableCollection<KeyColorData> Generate(RGBKeyboardCapabilities keyboardType, SortedList<int, Color> colorsList)
	{
		ObservableCollection<KeyColorData> observableCollection = new ObservableCollection<KeyColorData>();
		observableCollection.AddRange(MonocolorPattern.Generate(keyboardType, DefaultColors.OFF_COLOR_UI, enabled: true));
		switch (colorsList.Count)
		{
		case 1:
			observableCollection[observableCollection.IndexOf(observableCollection.FirstOrDefault((KeyColorData x) => x.Index == KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0) + 6))].SelectedColor = colorsList.Values[0];
			break;
		case 2:
			observableCollection[observableCollection.IndexOf(observableCollection.FirstOrDefault((KeyColorData x) => x.Index == KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0) + 5))].SelectedColor = colorsList.Values[1];
			observableCollection[observableCollection.IndexOf(observableCollection.FirstOrDefault((KeyColorData x) => x.Index == KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0) + 6))].SelectedColor = colorsList.Values[0];
			break;
		case 3:
			observableCollection[observableCollection.IndexOf(observableCollection.FirstOrDefault((KeyColorData x) => x.Index == KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0) + 4))].SelectedColor = colorsList.Values[2];
			observableCollection[observableCollection.IndexOf(observableCollection.FirstOrDefault((KeyColorData x) => x.Index == KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0) + 5))].SelectedColor = colorsList.Values[1];
			observableCollection[observableCollection.IndexOf(observableCollection.FirstOrDefault((KeyColorData x) => x.Index == KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0) + 6))].SelectedColor = colorsList.Values[0];
			break;
		case 4:
			observableCollection[observableCollection.IndexOf(observableCollection.FirstOrDefault((KeyColorData x) => x.Index == KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0) + 3))].SelectedColor = colorsList.Values[3];
			observableCollection[observableCollection.IndexOf(observableCollection.FirstOrDefault((KeyColorData x) => x.Index == KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0) + 4))].SelectedColor = colorsList.Values[2];
			observableCollection[observableCollection.IndexOf(observableCollection.FirstOrDefault((KeyColorData x) => x.Index == KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0) + 5))].SelectedColor = colorsList.Values[1];
			observableCollection[observableCollection.IndexOf(observableCollection.FirstOrDefault((KeyColorData x) => x.Index == KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0) + 6))].SelectedColor = colorsList.Values[0];
			break;
		case 5:
			observableCollection[observableCollection.IndexOf(observableCollection.FirstOrDefault((KeyColorData x) => x.Index == KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0) + 2))].SelectedColor = colorsList.Values[4];
			observableCollection[observableCollection.IndexOf(observableCollection.FirstOrDefault((KeyColorData x) => x.Index == KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0) + 3))].SelectedColor = colorsList.Values[3];
			observableCollection[observableCollection.IndexOf(observableCollection.FirstOrDefault((KeyColorData x) => x.Index == KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0) + 4))].SelectedColor = colorsList.Values[2];
			observableCollection[observableCollection.IndexOf(observableCollection.FirstOrDefault((KeyColorData x) => x.Index == KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0) + 5))].SelectedColor = colorsList.Values[1];
			observableCollection[observableCollection.IndexOf(observableCollection.FirstOrDefault((KeyColorData x) => x.Index == KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0) + 6))].SelectedColor = colorsList.Values[0];
			break;
		case 6:
			observableCollection[observableCollection.IndexOf(observableCollection.FirstOrDefault((KeyColorData x) => x.Index == KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0) + 1))].SelectedColor = colorsList.Values[5];
			observableCollection[observableCollection.IndexOf(observableCollection.FirstOrDefault((KeyColorData x) => x.Index == KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0) + 2))].SelectedColor = colorsList.Values[4];
			observableCollection[observableCollection.IndexOf(observableCollection.FirstOrDefault((KeyColorData x) => x.Index == KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0) + 3))].SelectedColor = colorsList.Values[3];
			observableCollection[observableCollection.IndexOf(observableCollection.FirstOrDefault((KeyColorData x) => x.Index == KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0) + 4))].SelectedColor = colorsList.Values[2];
			observableCollection[observableCollection.IndexOf(observableCollection.FirstOrDefault((KeyColorData x) => x.Index == KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0) + 5))].SelectedColor = colorsList.Values[1];
			observableCollection[observableCollection.IndexOf(observableCollection.FirstOrDefault((KeyColorData x) => x.Index == KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0) + 6))].SelectedColor = colorsList.Values[0];
			break;
		case 7:
			observableCollection[observableCollection.IndexOf(observableCollection.FirstOrDefault((KeyColorData x) => x.Index == KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0)))].SelectedColor = colorsList.Values[6];
			observableCollection[observableCollection.IndexOf(observableCollection.FirstOrDefault((KeyColorData x) => x.Index == KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0) + 1))].SelectedColor = colorsList.Values[5];
			observableCollection[observableCollection.IndexOf(observableCollection.FirstOrDefault((KeyColorData x) => x.Index == KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0) + 2))].SelectedColor = colorsList.Values[4];
			observableCollection[observableCollection.IndexOf(observableCollection.FirstOrDefault((KeyColorData x) => x.Index == KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0) + 3))].SelectedColor = colorsList.Values[3];
			observableCollection[observableCollection.IndexOf(observableCollection.FirstOrDefault((KeyColorData x) => x.Index == KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0) + 4))].SelectedColor = colorsList.Values[2];
			observableCollection[observableCollection.IndexOf(observableCollection.FirstOrDefault((KeyColorData x) => x.Index == KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0) + 5))].SelectedColor = colorsList.Values[1];
			observableCollection[observableCollection.IndexOf(observableCollection.FirstOrDefault((KeyColorData x) => x.Index == KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0) + 6))].SelectedColor = colorsList.Values[0];
			break;
		}
		return PresetPatternsKC.UpdateMultiRowKeys(keyboardType, observableCollection);
	}
}
