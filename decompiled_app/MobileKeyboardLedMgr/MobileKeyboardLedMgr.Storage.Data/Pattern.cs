using System.Collections.Generic;
using System.Windows.Media;

namespace MobileKeyboardLedMgr.Storage.Data;

public class Pattern
{
	public string Effect { get; set; }

	public List<Color> Colors { get; set; }

	public List<Key> Keys { get; set; }

	public int BrightnessAc { get; set; } = 100;

	public int BrightnessDc { get; set; } = 50;

	public int Speed { get; set; }

	public string Behavior { get; set; } = "None";
}
