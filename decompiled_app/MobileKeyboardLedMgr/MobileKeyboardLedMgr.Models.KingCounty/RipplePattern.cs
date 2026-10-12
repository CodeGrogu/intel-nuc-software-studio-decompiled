using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using MobileKeyboardLedMgr.Constants;
using MobileKeyboardLedMgr.Models.Profiles;
using UniwillSdkClient.UniwillService;

namespace MobileKeyboardLedMgr.Models.KingCounty;

internal static class RipplePattern
{
	internal static ObservableCollection<KeyColorData> Generate(RGBKeyboardCapabilities keyboardType, SortedList<int, Color> colorsList)
	{
		ObservableCollection<KeyColorData> observableCollection = new ObservableCollection<KeyColorData>();
		observableCollection.AddRange(MonocolorPattern.Generate(keyboardType, DefaultColors.OFF_COLOR_UI, enabled: true));
		List<Color> list = new List<Color>(colorsList.Values.Take(4));
		if (list.Count < 4)
		{
			int num = 4 - list.Count;
			for (int i = 0; i < num; i++)
			{
				list.Add(list[i]);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			switch (j)
			{
			case 0:
				observableCollection[KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 0, 7)].SelectedColor = list[j];
				break;
			case 1:
				observableCollection[KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 0, 6)].SelectedColor = list[j];
				observableCollection[KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 1, 6)].SelectedColor = list[j];
				observableCollection[KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 1, 7)].SelectedColor = list[j];
				observableCollection[KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 1, 8)].SelectedColor = list[j];
				observableCollection[KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 0, 8)].SelectedColor = list[j];
				break;
			case 2:
				observableCollection[KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 0, 5)].SelectedColor = list[j];
				observableCollection[KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 1, 5)].SelectedColor = list[j];
				observableCollection[KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 2, 5)].SelectedColor = list[j];
				observableCollection[KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 2, 6)].SelectedColor = list[j];
				observableCollection[KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 2, 7)].SelectedColor = list[j];
				observableCollection[KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 2, 8)].SelectedColor = list[j];
				observableCollection[KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 1, 9)].SelectedColor = list[j];
				observableCollection[KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 0, 9)].SelectedColor = list[j];
				break;
			case 3:
				observableCollection[KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 0, 4)].SelectedColor = list[j];
				observableCollection[KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 1, 4)].SelectedColor = list[j];
				observableCollection[KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 2, 4)].SelectedColor = list[j];
				observableCollection[KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 3, 4)].SelectedColor = list[j];
				observableCollection[KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 3, 5)].SelectedColor = list[j];
				observableCollection[KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 3, 6)].SelectedColor = list[j];
				observableCollection[KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 3, 7)].SelectedColor = list[j];
				observableCollection[KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 3, 8)].SelectedColor = list[j];
				observableCollection[KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 2, 9)].SelectedColor = list[j];
				observableCollection[KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 1, 10)].SelectedColor = list[j];
				observableCollection[KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 1, 11)].SelectedColor = list[j];
				break;
			}
		}
		return PresetPatternsKC.UpdateMultiRowKeys(keyboardType, observableCollection);
	}
}
