using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using MobileKeyboardLedMgr.Constants;
using MobileKeyboardLedMgr.Models.Profiles;
using UniwillSdkClient.UniwillService;

namespace MobileKeyboardLedMgr.Models.KingCounty;

internal static class GamingPattern
{
	internal static ObservableCollection<KeyColorData> Generate(RGBKeyboardCapabilities keyboardType, SortedList<int, Color> colors)
	{
		ObservableCollection<KeyColorData> observableCollection = new ObservableCollection<KeyColorData>();
		observableCollection.AddRange(MonocolorPattern.Generate(keyboardType, DefaultColors.OFF_COLOR_UI, enabled: true));
		foreach (KeyColorData item in observableCollection)
		{
			if (KeyboardCapabilities.IsGamingKey(keyboardType, item.Index))
			{
				item.SelectedColor = colors[0];
			}
		}
		return PresetPatternsKC.UpdateMultiRowKeys(keyboardType, observableCollection);
	}
}
