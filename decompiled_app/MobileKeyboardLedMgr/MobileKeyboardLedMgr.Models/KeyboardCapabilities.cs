using System.Collections.Generic;
using UniwillSdkClient.UniwillService;

namespace MobileKeyboardLedMgr.Models;

public static class KeyboardCapabilities
{
	public const int NUMBER_OF_ROWS_ON_KEYBOARD = 6;

	public static bool IsGamingKey(RGBKeyboardCapabilities KeyboardType, int Index)
	{
		switch (KeyboardType)
		{
		case RGBKeyboardCapabilities.KEY85:
			switch (Index)
			{
			case 33:
			case 47:
			case 48:
			case 49:
			case 72:
			case 82:
			case 83:
			case 84:
				return true;
			}
			break;
		case RGBKeyboardCapabilities.KEY86:
			switch (Index)
			{
			case 33:
			case 47:
			case 48:
			case 49:
			case 74:
			case 84:
			case 85:
			case 86:
				return true;
			}
			break;
		case RGBKeyboardCapabilities.KEY87:
			switch (Index)
			{
			case 34:
			case 48:
			case 49:
			case 50:
			case 75:
			case 85:
			case 86:
			case 87:
				return true;
			}
			break;
		}
		return false;
	}

	public static double GetKeySpan(RGBKeyboardCapabilities KeyboardType, int Key)
	{
		switch (KeyboardType)
		{
		case RGBKeyboardCapabilities.KEY85:
			switch (Key)
			{
			case 31:
				return 1.5;
			case 44:
				return 1.46;
			case 46:
				return 1.8;
			case 58:
				return 2.12;
			case 29:
			case 60:
			case 71:
			case 81:
				return 1.96;
			case 78:
				return 4.85;
			default:
				return 1.0;
			}
		case RGBKeyboardCapabilities.KEY86:
			switch (Key)
			{
			case 31:
				return 1.5;
			case 46:
			case 73:
			case 83:
				return 1.8;
			case 44:
				return 1.47;
			case 59:
				return 1.17;
			case 29:
				return 1.96;
			case 61:
			case 76:
				return 1.16;
			case 80:
				return 4.85;
			default:
				return 1.0;
			}
		case RGBKeyboardCapabilities.KEY87:
			switch (Key)
			{
			case 32:
				return 1.5;
			case 47:
			case 84:
				return 1.8;
			case 45:
				return 1.47;
			case 60:
				return 1.17;
			case 62:
				return 1.96;
			case 77:
				return 1.16;
			case 81:
				return 4.85;
			default:
				return 1.0;
			}
		default:
			return 1.0;
		}
	}

	public static int GetKeyFromRowPosition(RGBKeyboardCapabilities KeyboardType, int Row, int Position, bool FromFront = true)
	{
		int firstKeyInRow = GetFirstKeyInRow(KeyboardType, Row);
		int lastKeyInRow = GetLastKeyInRow(KeyboardType, Row);
		if (FromFront)
		{
			if (firstKeyInRow + Position > lastKeyInRow)
			{
				return lastKeyInRow;
			}
			return firstKeyInRow + Position;
		}
		if (lastKeyInRow - Position < firstKeyInRow)
		{
			return firstKeyInRow;
		}
		return lastKeyInRow - Position;
	}

	public static int GetFirstKeyInRow(RGBKeyboardCapabilities KeyboardType, int Row)
	{
		switch (Row)
		{
		case 0:
			switch (KeyboardType)
			{
			case RGBKeyboardCapabilities.KEY85:
				return 0;
			case RGBKeyboardCapabilities.KEY86:
				return 0;
			case RGBKeyboardCapabilities.KEY87:
				return 0;
			}
			break;
		case 1:
			switch (KeyboardType)
			{
			case RGBKeyboardCapabilities.KEY85:
				return 16;
			case RGBKeyboardCapabilities.KEY86:
				return 16;
			case RGBKeyboardCapabilities.KEY87:
				return 16;
			}
			break;
		case 2:
			switch (KeyboardType)
			{
			case RGBKeyboardCapabilities.KEY85:
				return 31;
			case RGBKeyboardCapabilities.KEY86:
				return 31;
			case RGBKeyboardCapabilities.KEY87:
				return 32;
			}
			break;
		case 3:
			switch (KeyboardType)
			{
			case RGBKeyboardCapabilities.KEY85:
				return 46;
			case RGBKeyboardCapabilities.KEY86:
				return 46;
			case RGBKeyboardCapabilities.KEY87:
				return 47;
			}
			break;
		case 4:
			switch (KeyboardType)
			{
			case RGBKeyboardCapabilities.KEY85:
				return 60;
			case RGBKeyboardCapabilities.KEY86:
				return 61;
			case RGBKeyboardCapabilities.KEY87:
				return 62;
			}
			break;
		case 5:
			switch (KeyboardType)
			{
			case RGBKeyboardCapabilities.KEY85:
				return 74;
			case RGBKeyboardCapabilities.KEY86:
				return 76;
			case RGBKeyboardCapabilities.KEY87:
				return 77;
			}
			break;
		}
		return -1;
	}

	internal static bool IsSecondRow(RGBKeyboardCapabilities KeyboardType, int Key)
	{
		if (KeyboardType == RGBKeyboardCapabilities.KEY86 && Key == 59)
		{
			return true;
		}
		if (KeyboardType == RGBKeyboardCapabilities.KEY87 && Key == 60)
		{
			return true;
		}
		return false;
	}

	internal static Dictionary<int, List<int>> GetMultiRowKeys(RGBKeyboardCapabilities KeyboardType)
	{
		Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
		switch (KeyboardType)
		{
		case RGBKeyboardCapabilities.KEY86:
			dictionary.Add(44, new List<int> { 59 });
			break;
		case RGBKeyboardCapabilities.KEY87:
			dictionary.Add(45, new List<int> { 60 });
			break;
		}
		return dictionary;
	}

	internal static List<int> MultiRowKeyIds(RGBKeyboardCapabilities KeyboardType, int Key)
	{
		if (KeyboardType == RGBKeyboardCapabilities.KEY86 && (Key == 44 || Key == 59))
		{
			return new List<int> { 59, 44 };
		}
		if (KeyboardType == RGBKeyboardCapabilities.KEY87 && (Key == 45 || Key == 60))
		{
			return new List<int> { 60, 45 };
		}
		return new List<int>();
	}

	public static int GetLastKeyInRow(RGBKeyboardCapabilities KeyboardType, int Row)
	{
		switch (Row)
		{
		case 0:
			switch (KeyboardType)
			{
			case RGBKeyboardCapabilities.KEY85:
				return 15;
			case RGBKeyboardCapabilities.KEY86:
				return 15;
			case RGBKeyboardCapabilities.KEY87:
				return 15;
			}
			break;
		case 1:
			switch (KeyboardType)
			{
			case RGBKeyboardCapabilities.KEY85:
				return 30;
			case RGBKeyboardCapabilities.KEY86:
				return 30;
			case RGBKeyboardCapabilities.KEY87:
				return 31;
			}
			break;
		case 2:
			switch (KeyboardType)
			{
			case RGBKeyboardCapabilities.KEY85:
				return 45;
			case RGBKeyboardCapabilities.KEY86:
				return 45;
			case RGBKeyboardCapabilities.KEY87:
				return 46;
			}
			break;
		case 3:
			switch (KeyboardType)
			{
			case RGBKeyboardCapabilities.KEY85:
				return 59;
			case RGBKeyboardCapabilities.KEY86:
				return 60;
			case RGBKeyboardCapabilities.KEY87:
				return 61;
			}
			break;
		case 4:
			switch (KeyboardType)
			{
			case RGBKeyboardCapabilities.KEY85:
				return 73;
			case RGBKeyboardCapabilities.KEY86:
				return 75;
			case RGBKeyboardCapabilities.KEY87:
				return 76;
			}
			break;
		case 5:
			switch (KeyboardType)
			{
			case RGBKeyboardCapabilities.KEY85:
				return 84;
			case RGBKeyboardCapabilities.KEY86:
				return 86;
			case RGBKeyboardCapabilities.KEY87:
				return 87;
			}
			break;
		}
		return -1;
	}
}
