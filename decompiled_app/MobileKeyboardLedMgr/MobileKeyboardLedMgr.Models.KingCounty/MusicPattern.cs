using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using MobileKeyboardLedMgr.Constants;
using MobileKeyboardLedMgr.Models.Profiles;
using UniwillSdkClient.UniwillService;

namespace MobileKeyboardLedMgr.Models.KingCounty;

internal class MusicPattern
{
	internal static ObservableCollection<KeyColorData> Generate(RGBKeyboardCapabilities keyboardType, SortedList<int, Color> colorsList, RGBKeyboardDirection direction)
	{
		ObservableCollection<KeyColorData> keys = MonocolorPattern.Generate(keyboardType, DefaultColors.OFF_COLOR_UI, enabled: true);
		List<Color> list = new List<Color>(colorsList.Values.Take(6));
		if (list.Count < 6)
		{
			int num = 6 - list.Count;
			int num2 = 0;
			if (list.Count > 2)
			{
				for (int i = 0; i < num; i++)
				{
					list.Insert(i + num2, list[i + num2]);
					num2++;
				}
			}
			else if (list.Count == 2)
			{
				for (int j = 0; j <= 3; j += 3)
				{
					list.Insert(j, list[j]);
					list.Insert(j, list[j]);
				}
			}
			else if (list.Count == 1)
			{
				for (int k = 0; k < 5; k++)
				{
					list.Add(list[0]);
				}
			}
		}
		switch (direction)
		{
		case RGBKeyboardDirection.None:
			GenerateMusicUp(keyboardType, keys, list);
			break;
		case RGBKeyboardDirection.Right:
			GenerateMusicOut(keyboardType, keys, list);
			break;
		case RGBKeyboardDirection.Left:
			GenerateMusicIn(keyboardType, keys, list);
			break;
		case RGBKeyboardDirection.Up:
			GenerateMusicDown(keyboardType, keys, list);
			break;
		}
		return PresetPatternsKC.UpdateMultiRowKeys(keyboardType, keys);
	}

	private static void GenerateMusicUp(RGBKeyboardCapabilities keyboardType, ObservableCollection<KeyColorData> keys, List<Color> colors)
	{
		for (int i = 0; i < colors.Count; i++)
		{
			switch (i)
			{
			case 0:
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 5), KeyboardCapabilities.GetLastKeyInRow(keyboardType, 5));
				break;
			case 1:
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 4), KeyboardCapabilities.GetLastKeyInRow(keyboardType, 4));
				break;
			case 2:
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 3), KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 3) + 6);
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetLastKeyInRow(keyboardType, 3) - 4, KeyboardCapabilities.GetLastKeyInRow(keyboardType, 3));
				break;
			case 3:
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 2) + 1, KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 2) + 5);
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetLastKeyInRow(keyboardType, 2) - 4, KeyboardCapabilities.GetLastKeyInRow(keyboardType, 2));
				break;
			case 4:
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 1) + 2, KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 1) + 5);
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetLastKeyInRow(keyboardType, 1) - 3, KeyboardCapabilities.GetLastKeyInRow(keyboardType, 1) - 1);
				break;
			case 5:
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0) + 3, KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0) + 4);
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetLastKeyInRow(keyboardType, 0) - 3, KeyboardCapabilities.GetLastKeyInRow(keyboardType, 0) - 2);
				break;
			}
		}
	}

	private static void GenerateMusicOut(RGBKeyboardCapabilities keyboardType, ObservableCollection<KeyColorData> keys, List<Color> colors)
	{
		for (int num = new List<Color>(colors.Take(4)).Count - 1; num >= 0; num--)
		{
			switch (num)
			{
			case 0:
				SetColorToKeyRange(keys, colors[num], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 0, 7), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 0, 9));
				SetColorToKeyRange(keys, colors[num], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 1, 7), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 1, 9));
				SetColorToKeyRange(keys, colors[num], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 2, 6), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 2, 8));
				SetColorToKeyRange(keys, colors[num], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 3, 6), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 3, 8));
				SetColorToKeyRange(keys, colors[num], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 4, 5), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 4, 7));
				SetColorToKeyRange(keys, colors[num], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 5, 4), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 5, 4));
				break;
			case 1:
				SetColorToKeyRange(keys, colors[num], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 0, 5), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 0, 9));
				SetColorToKeyRange(keys, colors[num], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 1, 6), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 1, 10));
				SetColorToKeyRange(keys, colors[num], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 2, 5), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 2, 9));
				SetColorToKeyRange(keys, colors[num], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 3, 5), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 3, 9));
				SetColorToKeyRange(keys, colors[num], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 4, 4), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 4, 8));
				SetColorToKeyRange(keys, colors[num], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 5, 5), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 5, 5));
				break;
			case 2:
				SetColorToKeyRange(keys, colors[num], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 0, 5), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 0, 11));
				SetColorToKeyRange(keys, colors[num], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 1, 5), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 1, 11));
				SetColorToKeyRange(keys, colors[num], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 2, 4), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 2, 10));
				SetColorToKeyRange(keys, colors[num], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 3, 4), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 3, 10));
				SetColorToKeyRange(keys, colors[num], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 4, 3), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 4, 9));
				SetColorToKeyRange(keys, colors[num], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 5, 6), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 5, 6));
				break;
			case 3:
				SetColorToKeyRange(keys, colors[num], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 0, 4), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 0, 12));
				SetColorToKeyRange(keys, colors[num], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 1, 4), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 1, 12));
				SetColorToKeyRange(keys, colors[num], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 2, 3), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 2, 11));
				SetColorToKeyRange(keys, colors[num], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 3, 3), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 3, 11));
				SetColorToKeyRange(keys, colors[num], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 4, 2), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 4, 10));
				SetColorToKeyRange(keys, colors[num], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 5, 3), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 5, 7));
				break;
			}
		}
	}

	private static void GenerateMusicIn(RGBKeyboardCapabilities keyboardType, ObservableCollection<KeyColorData> keys, List<Color> colors)
	{
		List<Color> list = new List<Color>(colors.Take(5));
		for (int i = 0; i < list.Count; i++)
		{
			switch (i)
			{
			case 0:
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0), KeyboardCapabilities.GetLastKeyInRow(keyboardType, 0));
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 1), KeyboardCapabilities.GetLastKeyInRow(keyboardType, 1));
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 2), KeyboardCapabilities.GetLastKeyInRow(keyboardType, 2));
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 3), KeyboardCapabilities.GetLastKeyInRow(keyboardType, 3));
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 4), KeyboardCapabilities.GetLastKeyInRow(keyboardType, 4));
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 5), KeyboardCapabilities.GetLastKeyInRow(keyboardType, 5));
				break;
			case 1:
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 0, 2), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 0, 2, FromFront: false));
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 1, 2), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 1, 2, FromFront: false));
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 2, 1), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 2, 2, FromFront: false));
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 3, 1), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 3, 2, FromFront: false));
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 4, 1), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 4, 2, FromFront: false));
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 5, 1), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 5, 2, FromFront: false));
				break;
			case 2:
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 0, 3), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 0, 3, FromFront: false));
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 1, 3), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 1, 2, FromFront: false));
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 2, 2), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 2, 3, FromFront: false));
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 3, 2), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 3, 2, FromFront: false));
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 4, 1), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 4, 3, FromFront: false));
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 5, 2), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 5, 3, FromFront: false));
				break;
			case 3:
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 0, 4), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 0, 4, FromFront: false));
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 1, 4), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 1, 3, FromFront: false));
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 2, 3), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 2, 4, FromFront: false));
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 3, 3), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 3, 3, FromFront: false));
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 4, 2), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 4, 4, FromFront: false));
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 5, 3), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 5, 4, FromFront: false));
				break;
			case 4:
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 0, 5), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 0, 5, FromFront: false));
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 1, 5), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 1, 4, FromFront: false));
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 2, 4), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 2, 5, FromFront: false));
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 3, 4), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 3, 4, FromFront: false));
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 4, 3), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 4, 5, FromFront: false));
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 5, 5), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 5, 5, FromFront: false));
				break;
			}
		}
		SetColorToKeyRange(keys, DefaultColors.OFF_COLOR_UI, KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 0, 5), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 0, 6, FromFront: false));
		SetColorToKeyRange(keys, DefaultColors.OFF_COLOR_UI, KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 1, 6), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 1, 5, FromFront: false));
		SetColorToKeyRange(keys, DefaultColors.OFF_COLOR_UI, KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 2, 5), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 2, 6, FromFront: false));
		SetColorToKeyRange(keys, DefaultColors.OFF_COLOR_UI, KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 3, 5), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 3, 5, FromFront: false));
		SetColorToKeyRange(keys, DefaultColors.OFF_COLOR_UI, KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 4, 4), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 4, 6, FromFront: false));
		SetColorToKeyRange(keys, DefaultColors.OFF_COLOR_UI, KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 5, 4), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 5, 6, FromFront: false));
	}

	private static void GenerateMusicDown(RGBKeyboardCapabilities keyboardType, ObservableCollection<KeyColorData> keys, List<Color> colors)
	{
		for (int i = 0; i < colors.Count; i++)
		{
			switch (i)
			{
			case 0:
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 0), KeyboardCapabilities.GetLastKeyInRow(keyboardType, 0));
				break;
			case 1:
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 1), KeyboardCapabilities.GetLastKeyInRow(keyboardType, 1));
				break;
			case 2:
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetFirstKeyInRow(keyboardType, 2), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 2, 5));
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 2, 4, FromFront: false), KeyboardCapabilities.GetLastKeyInRow(keyboardType, 2));
				break;
			case 3:
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 3, 1), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 3, 4));
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 3, 3, FromFront: false), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 3, 1, FromFront: false));
				break;
			case 4:
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 4, 1), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 4, 3));
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 4, 3, FromFront: false), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 4, 2, FromFront: false));
				break;
			case 5:
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 5, 3), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 5, 3));
				SetColorToKeyRange(keys, colors[i], KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 5, 3, FromFront: false), KeyboardCapabilities.GetKeyFromRowPosition(keyboardType, 5, 3, FromFront: false));
				break;
			}
		}
	}

	private static void SetColorToKeyRange(ObservableCollection<KeyColorData> existingKeys, Color color, int startIndex, int endIndex)
	{
		for (int i = startIndex; i <= endIndex; i++)
		{
			existingKeys[i].SelectedColor = color;
		}
	}

	private ObservableCollection<KeyColorData> GenerateMusicRow(ObservableCollection<KeyColorData> keys, SortedList<int, Color> colorsList, int startIndex, int endIndex)
	{
		int num = endIndex - startIndex;
		int count = colorsList.Count;
		int num2 = startIndex + (endIndex - startIndex) / 2;
		int[] array = new int[2];
		if (num % 2 == 0)
		{
			array[0] = num / 2 - 1;
			array[1] = num / 2;
		}
		else
		{
			array[0] = num / 2;
			array[1] = num / 2;
		}
		double num3 = Math.Round((double)num / 2.0);
		double num4 = num3 / (double)count;
		int leftPointer = startIndex + array[0];
		int rightPointer = startIndex + array[1];
		int num5 = 0;
		double num6 = num4;
		int i;
		for (i = 0; (double)i < num3; i++)
		{
			if (num6 - (double)i >= 1.0)
			{
				keys[keys.IndexOf(keys.FirstOrDefault((KeyColorData x) => x.Index == leftPointer - i))].SelectedColor = colorsList.Values[num5];
				keys[keys.IndexOf(keys.FirstOrDefault((KeyColorData x) => x.Index == rightPointer + i))].SelectedColor = colorsList.Values[num5];
			}
			else
			{
				if (!(num6 - (double)i < 1.0))
				{
					continue;
				}
				if (num6 - (double)i > 0.5)
				{
					keys[keys.IndexOf(keys.FirstOrDefault((KeyColorData x) => x.Index == leftPointer - i))].SelectedColor = colorsList.Values[num5];
					keys[keys.IndexOf(keys.FirstOrDefault((KeyColorData x) => x.Index == rightPointer + i))].SelectedColor = colorsList.Values[num5];
					num5++;
				}
				else
				{
					num5++;
					keys[keys.IndexOf(keys.FirstOrDefault((KeyColorData x) => x.Index == leftPointer - i))].SelectedColor = colorsList.Values[num5];
					keys[keys.IndexOf(keys.FirstOrDefault((KeyColorData x) => x.Index == rightPointer + i))].SelectedColor = colorsList.Values[num5];
				}
				num6 += num4;
			}
		}
		int j;
		for (j = startIndex; j <= endIndex; j++)
		{
			int value = Convert.ToInt32((double)(255 - Math.Abs(num2 - j) * 25));
			Color color = keys.FirstOrDefault((KeyColorData x) => x.Index == j)?.SelectedColor ?? default(Color);
			Color selectedColor = Color.FromArgb(Convert.ToByte(value), color.R, color.G, color.B);
			keys[keys.IndexOf(keys.FirstOrDefault((KeyColorData x) => x.Index == j))].SelectedColor = selectedColor;
		}
		return keys;
	}
}
