using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using MobileKeyboardLedMgr.Models.Profiles;
using UniwillSdkClient.UniwillService;

namespace MobileKeyboardLedMgr.Models.KingCounty;

internal static class RainbowPattern
{
	internal static ObservableCollection<KeyColorData> Generate(RGBKeyboardCapabilities keyboardType, List<Color> colors)
	{
		ObservableCollection<KeyColorData> observableCollection = new ObservableCollection<KeyColorData>();
		observableCollection.AddRange(GenerateRainbowRow(KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0), KeyboardCapabilities.GetLastKeyInRow(keyboardType, 0), 3, 2, colors));
		observableCollection.AddRange(GenerateRainbowRow(KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 1), KeyboardCapabilities.GetLastKeyInRow(keyboardType, 1), 3, 2, colors));
		observableCollection.AddRange(GenerateRainbowRow(KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 2), KeyboardCapabilities.GetLastKeyInRow(keyboardType, 2), 2, 2, colors));
		observableCollection.AddRange(GenerateRainbowRow(KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 3), KeyboardCapabilities.GetLastKeyInRow(keyboardType, 3), 2, 2, colors));
		observableCollection.AddRange(GenerateRainbowRow(KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 4), KeyboardCapabilities.GetLastKeyInRow(keyboardType, 4), 1, 2, colors));
		observableCollection.AddRange(GenerateFinalRainbowRow(KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 5), KeyboardCapabilities.GetLastKeyInRow(keyboardType, 5), 2, 2, colors));
		return PresetPatternsKC.UpdateMultiRowKeys(keyboardType, observableCollection);
	}

	private static ObservableCollection<KeyColorData> GenerateRainbowRow(int StartIndex, int EndIndex, int FirstColorCount, int OtherColorCount, List<Color> colors)
	{
		ObservableCollection<KeyColorData> observableCollection = new ObservableCollection<KeyColorData>();
		for (int i = StartIndex; i <= EndIndex; i++)
		{
			Color selectedColor;
			if (i >= StartIndex + FirstColorCount)
			{
				if (i >= StartIndex + FirstColorCount + OtherColorCount)
				{
					if (i >= StartIndex + FirstColorCount + 2 * OtherColorCount)
					{
						if (i >= StartIndex + FirstColorCount + 3 * OtherColorCount)
						{
							if (i >= StartIndex + FirstColorCount + 4 * OtherColorCount)
							{
								selectedColor = ((i >= StartIndex + FirstColorCount + 5 * OtherColorCount) ? colors[6] : colors[5]);
							}
							else
							{
								selectedColor = colors[4];
							}
						}
						else
						{
							selectedColor = colors[3];
						}
					}
					else
					{
						selectedColor = colors[2];
					}
				}
				else
				{
					selectedColor = colors[1];
				}
			}
			else
			{
				selectedColor = colors[0];
			}
			observableCollection.Add(new KeyColorData(i, selectedColor, enabled: false));
		}
		return observableCollection;
	}

	private static ObservableCollection<KeyColorData> GenerateFinalRainbowRow(int StartIndex, int EndIndex, int FirstColorCount, int OtherColorCount, List<Color> colors)
	{
		ObservableCollection<KeyColorData> observableCollection = new ObservableCollection<KeyColorData>();
		for (int i = StartIndex; i <= EndIndex; i++)
		{
			Color selectedColor;
			if (i >= StartIndex + FirstColorCount)
			{
				if (i >= StartIndex + FirstColorCount + OtherColorCount)
				{
					if (i >= 0)
					{
						if (i >= StartIndex + FirstColorCount + OtherColorCount + 1)
						{
							if (i >= StartIndex + FirstColorCount + OtherColorCount + 2)
							{
								selectedColor = ((i >= StartIndex + FirstColorCount + OtherColorCount + 2 + OtherColorCount) ? colors[6] : colors[5]);
							}
							else
							{
								selectedColor = colors[4];
							}
						}
						else
						{
							selectedColor = colors[3];
						}
					}
					else
					{
						selectedColor = colors[2];
					}
				}
				else
				{
					selectedColor = colors[1];
				}
			}
			else
			{
				selectedColor = colors[0];
			}
			observableCollection.Add(new KeyColorData(i, selectedColor, enabled: false));
		}
		return observableCollection;
	}
}
