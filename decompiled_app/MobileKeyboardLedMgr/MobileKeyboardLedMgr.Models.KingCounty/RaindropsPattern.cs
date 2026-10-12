using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using MobileKeyboardLedMgr.Models.Profiles;
using UniwillSdkClient.UniwillService;

namespace MobileKeyboardLedMgr.Models.KingCounty;

internal static class RaindropsPattern
{
	internal static ObservableCollection<KeyColorData> Generate(RGBKeyboardCapabilities keyboardType, SortedList<int, Color> colorsList)
	{
		return PresetPatternsKC.GenerateRandomKeysFromColors(keyboardType, colorsList.Values.ToList(), 3);
	}
}
