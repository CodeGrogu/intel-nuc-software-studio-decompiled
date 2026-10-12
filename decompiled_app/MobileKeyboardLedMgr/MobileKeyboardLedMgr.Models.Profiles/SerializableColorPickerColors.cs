using System.Windows.Media;

namespace MobileKeyboardLedMgr.Models.Profiles;

public class SerializableColorPickerColors
{
	public int Index { get; set; }

	public Color SelectedColor { get; set; }

	public bool Enabled { get; set; }

	public SerializableColorPickerColors(ColorPickerColors color)
	{
		if (color != null)
		{
			Index = color.Index;
			SelectedColor = color.SelectedColor;
			Enabled = color.Enabled;
		}
	}

	public override bool Equals(object obj)
	{
		if (obj is SerializableColorPickerColors serializableColorPickerColors && Index == serializableColorPickerColors.Index && SelectedColor == serializableColorPickerColors.SelectedColor)
		{
			return Enabled == serializableColorPickerColors.Enabled;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return ((unchecked(2144091145 * -1521134295) + Index.GetHashCode()) * -1521134295 + SelectedColor.GetHashCode()) * -1521134295 + Enabled.GetHashCode();
	}
}
