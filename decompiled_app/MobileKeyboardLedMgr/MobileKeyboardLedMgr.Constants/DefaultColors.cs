using System.Collections.Generic;
using System.Windows.Media;

namespace MobileKeyboardLedMgr.Constants;

public static class DefaultColors
{
	public static readonly Color SELECTED = Colors.White;

	public static readonly Color OFF_COLOR_LED = Colors.Black;

	public static readonly Color OFF_COLOR_UI = Colors.Gray;

	public static readonly List<Color> COLORS = new List<Color>
	{
		Colors.Red,
		Colors.Orange,
		Colors.Yellow,
		Colors.Green,
		Colors.Blue,
		Colors.Indigo,
		Colors.Violet
	};

	public static readonly Color MONOCOLOR = Colors.White;

	public static readonly Color GAMING = Colors.Red;
}
