using System.Windows.Media;

namespace MobileKeyboardLedMgr.Models.Profiles;

public class KeyColorData
{
	public int Index;

	public Color SelectedColor;

	public bool Enabled;

	public KeyColorData(int index, Color selectedColor, bool enabled)
	{
		Index = index;
		SelectedColor = selectedColor;
		Enabled = enabled;
	}

	public KeyColorData()
	{
	}

	public KeyColorData DeepCopy()
	{
		return new KeyColorData(Index, Color.FromArgb(SelectedColor.A, SelectedColor.R, SelectedColor.G, SelectedColor.B), Enabled);
	}
}
