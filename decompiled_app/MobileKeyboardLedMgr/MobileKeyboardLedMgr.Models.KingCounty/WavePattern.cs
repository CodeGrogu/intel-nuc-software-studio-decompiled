using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using MobileKeyboardLedMgr.Models.Profiles;
using UniwillSdkClient.UniwillService;

namespace MobileKeyboardLedMgr.Models.KingCounty;

internal static class WavePattern
{
	internal static ObservableCollection<KeyColorData> Generate(RGBKeyboardCapabilities keyboardType, SortedList<int, Color> colors, RGBKeyboardDirection direction)
	{
		ObservableCollection<KeyColorData> observableCollection = new ObservableCollection<KeyColorData>();
		List<Color> colors2 = colors.Values.ToList();
		switch (direction)
		{
		case RGBKeyboardDirection.Right:
		case RGBKeyboardDirection.Left:
		{
			for (int i = 0; i < 5; i++)
			{
				observableCollection.AddRange(GenerateRightAndLeftWaveRow(colors2, KeyboardCapabilities.GetFirstKeyInRow(keyboardType, i), KeyboardCapabilities.GetLastKeyInRow(keyboardType, i)));
			}
			observableCollection.AddRange(GenerateRightAndLeftWaveFinalRow(keyboardType, KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 5), KeyboardCapabilities.GetLastKeyInRow(keyboardType, 5), observableCollection));
			break;
		}
		case RGBKeyboardDirection.Up:
		case RGBKeyboardDirection.Down:
			observableCollection.AddRange(GenerateUpAndDownWaveRows(keyboardType, colors2));
			break;
		}
		return PresetPatternsKC.UpdateMultiRowKeys(keyboardType, observableCollection);
	}

	private static ObservableCollection<KeyColorData> GenerateRightAndLeftWaveRow(List<Color> colors, int startIndex, int endIndex)
	{
		ObservableCollection<KeyColorData> observableCollection = new ObservableCollection<KeyColorData>();
		int num = endIndex - startIndex + 1;
		int num2 = (int)Math.Floor((double)num / (double)colors.Count);
		int num3 = num % (num2 * colors.Count);
		int num4 = startIndex;
		foreach (Color color in colors)
		{
			for (int num5 = num2; num5 > 0; num5--)
			{
				observableCollection.Add(new KeyColorData(num4, color, enabled: true));
				num4++;
			}
			if (num3 > 0)
			{
				observableCollection.Add(new KeyColorData(num4, color, enabled: true));
				num3--;
				num4++;
			}
		}
		return observableCollection;
	}

	private static ObservableCollection<KeyColorData> GenerateRightAndLeftWaveFinalRow(RGBKeyboardCapabilities keyboardType, int startIndex, int endIndex, ObservableCollection<KeyColorData> existingKeys)
	{
		ObservableCollection<KeyColorData> observableCollection = new ObservableCollection<KeyColorData>();
		Dictionary<int, int> dictionary = RightAndLeftWaveFinalRowKeyColorMap(keyboardType);
		for (int i = startIndex; i <= endIndex; i++)
		{
			if (dictionary.ContainsKey(i))
			{
				int index = dictionary[i];
				observableCollection.Add(new KeyColorData(i, existingKeys[index].SelectedColor, enabled: true));
			}
		}
		return observableCollection;
	}

	private static ObservableCollection<KeyColorData> GenerateUpAndDownWaveRows(RGBKeyboardCapabilities keyboardType, List<Color> colors)
	{
		ObservableCollection<KeyColorData> observableCollection = new ObservableCollection<KeyColorData>();
		List<Color> list = colors.Take(6).ToList();
		int num = (int)Math.Floor(6.0 / (double)list.Count);
		int num2 = 6 % (num * list.Count);
		int num3 = 0;
		foreach (Color color in colors)
		{
			for (int num4 = num; num4 > 0; num4--)
			{
				if (num3 == KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0))
				{
					observableCollection.AddRange(MonocolorPattern.GenerateRow(color, KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0), KeyboardCapabilities.GetLastKeyInRow(keyboardType, 0), enabled: true));
					num3 = KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 1);
				}
				else if (num3 == KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 1))
				{
					observableCollection.AddRange(MonocolorPattern.GenerateRow(color, KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 1), KeyboardCapabilities.GetLastKeyInRow(keyboardType, 1), enabled: true));
					num3 = KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 2);
				}
				else if (num3 == KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 2))
				{
					observableCollection.AddRange(MonocolorPattern.GenerateRow(color, KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 2), KeyboardCapabilities.GetLastKeyInRow(keyboardType, 2), enabled: true));
					num3 = KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 3);
				}
				else if (num3 == KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 3))
				{
					observableCollection.AddRange(MonocolorPattern.GenerateRow(color, KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 3), KeyboardCapabilities.GetLastKeyInRow(keyboardType, 3), enabled: true));
					num3 = KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 4);
				}
				else if (num3 == KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 4))
				{
					observableCollection.AddRange(MonocolorPattern.GenerateRow(color, KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 4), KeyboardCapabilities.GetLastKeyInRow(keyboardType, 4), enabled: true));
					num3 = KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 5);
				}
				else if (num3 == KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 5))
				{
					observableCollection.AddRange(MonocolorPattern.GenerateRow(color, KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 5), KeyboardCapabilities.GetLastKeyInRow(keyboardType, 5), enabled: true));
				}
			}
			if (num2 > 0)
			{
				if (num3 == KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 1))
				{
					observableCollection.AddRange(MonocolorPattern.GenerateRow(color, KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 1), KeyboardCapabilities.GetLastKeyInRow(keyboardType, 1), enabled: true));
					num3 = KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 2);
				}
				else if (num3 == KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 3))
				{
					observableCollection.AddRange(MonocolorPattern.GenerateRow(color, KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 3), KeyboardCapabilities.GetLastKeyInRow(keyboardType, 3), enabled: true));
					num3 = KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 4);
				}
				num2--;
			}
		}
		return observableCollection;
	}

	private static Dictionary<int, int> RightAndLeftWaveFinalRowKeyColorMap(RGBKeyboardCapabilities keyboardType)
	{
		return keyboardType switch
		{
			RGBKeyboardCapabilities.KEY85 => new Dictionary<int, int>
			{
				{ 74, 60 },
				{ 75, 47 },
				{ 76, 61 },
				{ 77, 62 },
				{ 78, 65 },
				{ 79, 68 },
				{ 80, 69 },
				{ 81, 70 },
				{ 82, 71 },
				{ 83, 72 },
				{ 84, 73 }
			}, 
			RGBKeyboardCapabilities.KEY86 => new Dictionary<int, int>
			{
				{ 76, 61 },
				{ 77, 47 },
				{ 78, 63 },
				{ 79, 64 },
				{ 80, 67 },
				{ 81, 70 },
				{ 82, 71 },
				{ 83, 72 },
				{ 84, 73 },
				{ 85, 74 },
				{ 86, 75 }
			}, 
			RGBKeyboardCapabilities.KEY87 => new Dictionary<int, int>
			{
				{ 77, 62 },
				{ 78, 63 },
				{ 79, 48 },
				{ 80, 64 },
				{ 81, 67 },
				{ 82, 70 },
				{ 83, 71 },
				{ 84, 72 },
				{ 85, 74 },
				{ 86, 75 },
				{ 87, 76 }
			}, 
			_ => new Dictionary<int, int>(), 
		};
	}
}
