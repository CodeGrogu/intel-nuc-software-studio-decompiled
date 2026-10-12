using System;
using System.Globalization;

namespace Windows.Foundation;

internal static class TokenizerHelper
{
	internal static char GetNumericListSeparator(IFormatProvider provider)
	{
		char c = ',';
		NumberFormatInfo instance = NumberFormatInfo.GetInstance(provider);
		if (instance.NumberDecimalSeparator.Length > 0 && c == instance.NumberDecimalSeparator[0])
		{
			c = ';';
		}
		return c;
	}
}
