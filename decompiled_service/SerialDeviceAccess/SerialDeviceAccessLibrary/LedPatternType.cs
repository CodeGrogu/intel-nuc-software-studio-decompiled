using System.Collections.Generic;

namespace SerialDeviceAccessLibrary;

public class LedPatternType
{
	public Dictionary<char, string> LedPatternTypeDictory { get; }

	public uint LedPatternTypeZeroIndex { get; }

	public uint LedPatternTypeOneMaxIndex { get; }

	public uint LedPatternTypeTwoMaxIndex { get; }

	public uint LedPatternTypeThreeMaxIndex { get; }

	public uint LedPatternTypeOnePaletteNums { get; }

	public uint LedPatternTypeTwoPaletteNums { get; }

	public LedPatternType()
	{
		LedPatternTypeDictory = new Dictionary<char, string>
		{
			{ '0', "None" },
			{ '1', "C1 , Vx , Rtr5B , Dx" },
			{ '2', "C1 , Vx , RB , Dx" },
			{ '3', "C1 , Vx , RB , Dx" },
			{ '4', "C1 , Vx , RB , Dx" },
			{ '5', "C2 , C3 , C4 , Vx , Dx" },
			{ '6', "C2 , C3 , C4 , Vx , Dx" },
			{ '7', "C2 , C3 , C4 , Vx , Dx" },
			{ '8', "V1" },
			{ '9', "C1 , V1 , RB" }
		};
		LedPatternTypeZeroIndex = 0u;
		LedPatternTypeOneMaxIndex = 4u;
		LedPatternTypeTwoMaxIndex = 7u;
		LedPatternTypeThreeMaxIndex = 9u;
		LedPatternTypeOnePaletteNums = 1u;
		LedPatternTypeTwoPaletteNums = 3u;
	}
}
