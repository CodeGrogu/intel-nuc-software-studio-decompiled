using System.Collections.Generic;

namespace MobileKeyboardLedMgr.Models.KeyMaps.JIS;

internal static class KeyMapJis
{
	public static readonly Dictionary<PhysicalKeyJis, List<int>> KeyMap = new Dictionary<PhysicalKeyJis, List<int>>
	{
		{
			PhysicalKeyJis.ESC,
			new List<int> { 0 }
		},
		{
			PhysicalKeyJis.F1,
			new List<int> { 1 }
		},
		{
			PhysicalKeyJis.F2,
			new List<int> { 2 }
		},
		{
			PhysicalKeyJis.F3,
			new List<int> { 3 }
		},
		{
			PhysicalKeyJis.F4,
			new List<int> { 4 }
		},
		{
			PhysicalKeyJis.F5,
			new List<int> { 5 }
		},
		{
			PhysicalKeyJis.F6,
			new List<int> { 6 }
		},
		{
			PhysicalKeyJis.F7,
			new List<int> { 7 }
		},
		{
			PhysicalKeyJis.F8,
			new List<int> { 8 }
		},
		{
			PhysicalKeyJis.F9,
			new List<int> { 9 }
		},
		{
			PhysicalKeyJis.F10,
			new List<int> { 10 }
		},
		{
			PhysicalKeyJis.F11,
			new List<int> { 11 }
		},
		{
			PhysicalKeyJis.F12,
			new List<int> { 12 }
		},
		{
			PhysicalKeyJis.INSERT,
			new List<int> { 13 }
		},
		{
			PhysicalKeyJis.SCRLK,
			new List<int> { 14 }
		},
		{
			PhysicalKeyJis.DEL,
			new List<int> { 15 }
		},
		{
			PhysicalKeyJis.HAN_ZEN,
			new List<int> { 21 }
		},
		{
			PhysicalKeyJis.ONE,
			new List<int> { 22 }
		},
		{
			PhysicalKeyJis.TWO,
			new List<int> { 23 }
		},
		{
			PhysicalKeyJis.THREE,
			new List<int> { 24 }
		},
		{
			PhysicalKeyJis.FOUR,
			new List<int> { 25 }
		},
		{
			PhysicalKeyJis.FIVE,
			new List<int> { 26 }
		},
		{
			PhysicalKeyJis.SIX,
			new List<int> { 27 }
		},
		{
			PhysicalKeyJis.SEVEN,
			new List<int> { 28 }
		},
		{
			PhysicalKeyJis.EIGHT,
			new List<int> { 29 }
		},
		{
			PhysicalKeyJis.NINE,
			new List<int> { 30 }
		},
		{
			PhysicalKeyJis.ZERO,
			new List<int> { 31 }
		},
		{
			PhysicalKeyJis.MINUS,
			new List<int> { 32 }
		},
		{
			PhysicalKeyJis.CARET,
			new List<int> { 33 }
		},
		{
			PhysicalKeyJis.YEN,
			new List<int> { 34 }
		},
		{
			PhysicalKeyJis.BACKSPACE,
			new List<int> { 35 }
		},
		{
			PhysicalKeyJis.HOME,
			new List<int> { 36 }
		},
		{
			PhysicalKeyJis.TAB,
			new List<int> { 42 }
		},
		{
			PhysicalKeyJis.Q,
			new List<int> { 44 }
		},
		{
			PhysicalKeyJis.W,
			new List<int> { 45 }
		},
		{
			PhysicalKeyJis.E,
			new List<int> { 46 }
		},
		{
			PhysicalKeyJis.R,
			new List<int> { 47 }
		},
		{
			PhysicalKeyJis.T,
			new List<int> { 48 }
		},
		{
			PhysicalKeyJis.Y,
			new List<int> { 49 }
		},
		{
			PhysicalKeyJis.U,
			new List<int> { 50 }
		},
		{
			PhysicalKeyJis.I,
			new List<int> { 51 }
		},
		{
			PhysicalKeyJis.O,
			new List<int> { 52 }
		},
		{
			PhysicalKeyJis.P,
			new List<int> { 53 }
		},
		{
			PhysicalKeyJis.QUOTE,
			new List<int> { 54 }
		},
		{
			PhysicalKeyJis.BRACKET_OPEN,
			new List<int> { 55 }
		},
		{
			PhysicalKeyJis.ENTER,
			new List<int> { 56, 77 }
		},
		{
			PhysicalKeyJis.PAGE_UP,
			new List<int> { 57 }
		},
		{
			PhysicalKeyJis.CAPS,
			new List<int> { 63 }
		},
		{
			PhysicalKeyJis.A,
			new List<int> { 65 }
		},
		{
			PhysicalKeyJis.S,
			new List<int> { 66 }
		},
		{
			PhysicalKeyJis.D,
			new List<int> { 67 }
		},
		{
			PhysicalKeyJis.F,
			new List<int> { 68 }
		},
		{
			PhysicalKeyJis.G,
			new List<int> { 69 }
		},
		{
			PhysicalKeyJis.H,
			new List<int> { 70 }
		},
		{
			PhysicalKeyJis.J,
			new List<int> { 71 }
		},
		{
			PhysicalKeyJis.K,
			new List<int> { 72 }
		},
		{
			PhysicalKeyJis.L,
			new List<int> { 73 }
		},
		{
			PhysicalKeyJis.SEMICOLON,
			new List<int> { 74 }
		},
		{
			PhysicalKeyJis.COLON,
			new List<int> { 75 }
		},
		{
			PhysicalKeyJis.BRACKET_CLOSE,
			new List<int> { 76 }
		},
		{
			PhysicalKeyJis.ENTER_SECOND_ROW,
			new List<int> { 56, 77 }
		},
		{
			PhysicalKeyJis.PAGE_DOWN,
			new List<int> { 78 }
		},
		{
			PhysicalKeyJis.SHIFT_LEFT,
			new List<int> { 84, 85, 86 }
		},
		{
			PhysicalKeyJis.Z,
			new List<int> { 87 }
		},
		{
			PhysicalKeyJis.X,
			new List<int> { 88 }
		},
		{
			PhysicalKeyJis.C,
			new List<int> { 89 }
		},
		{
			PhysicalKeyJis.V,
			new List<int> { 90 }
		},
		{
			PhysicalKeyJis.B,
			new List<int> { 91 }
		},
		{
			PhysicalKeyJis.N,
			new List<int> { 92 }
		},
		{
			PhysicalKeyJis.M,
			new List<int> { 93 }
		},
		{
			PhysicalKeyJis.COMMA,
			new List<int> { 94 }
		},
		{
			PhysicalKeyJis.PERIOD,
			new List<int> { 95 }
		},
		{
			PhysicalKeyJis.SLASH,
			new List<int> { 96 }
		},
		{
			PhysicalKeyJis.BACKSLASH,
			new List<int> { 97 }
		},
		{
			PhysicalKeyJis.SHIFT_RIGHT,
			new List<int> { 98 }
		},
		{
			PhysicalKeyJis.ARROW_UP,
			new List<int> { 99 }
		},
		{
			PhysicalKeyJis.END,
			new List<int> { 100 }
		},
		{
			PhysicalKeyJis.CTRL_LEFT,
			new List<int> { 105 }
		},
		{
			PhysicalKeyJis.FN,
			new List<int> { 107 }
		},
		{
			PhysicalKeyJis.WINDOWS,
			new List<int> { 108 }
		},
		{
			PhysicalKeyJis.ALT_LEFT,
			new List<int> { 109 }
		},
		{
			PhysicalKeyJis.SPACE,
			new List<int> { 112 }
		},
		{
			PhysicalKeyJis.HEN_CAN,
			new List<int> { 115 }
		},
		{
			PhysicalKeyJis.KATAKANA,
			new List<int> { 116 }
		},
		{
			PhysicalKeyJis.CTRL_RIGHT,
			new List<int> { 117 }
		},
		{
			PhysicalKeyJis.ARROW_LEFT,
			new List<int> { 118 }
		},
		{
			PhysicalKeyJis.ARROW_DOWN,
			new List<int> { 119 }
		},
		{
			PhysicalKeyJis.ARROW_RIGHT,
			new List<int> { 120 }
		}
	};

	public static List<int> GetIndicesForKey(PhysicalKeyJis key)
	{
		return KeyMap[key];
	}

	public static List<int> GetIndicesForKey(int key)
	{
		return GetIndicesForKey((PhysicalKeyJis)key);
	}
}
