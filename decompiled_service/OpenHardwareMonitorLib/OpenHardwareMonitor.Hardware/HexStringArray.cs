using System;

namespace OpenHardwareMonitor.Hardware;

internal static class HexStringArray
{
	public static byte Read(string s, ushort address)
	{
		string[] array = s.Split(new char[2] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Split(new char[2] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
			if (array2.Length != 0 && Convert.ToInt32(array2[0], 16) == (address & 0xFFF0))
			{
				return Convert.ToByte(array2[(address & 0xF) + 1], 16);
			}
		}
		throw new ArgumentException();
	}
}
