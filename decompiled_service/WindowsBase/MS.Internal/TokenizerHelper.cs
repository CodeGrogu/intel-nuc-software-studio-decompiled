using System;
using MS.Internal.WindowsBase;

namespace MS.Internal;

[FriendAccessAllowed]
internal class TokenizerHelper
{
	internal int _currentTokenIndex;

	internal int _currentTokenLength;

	internal bool FoundSeparator
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal TokenizerHelper(string str, IFormatProvider formatProvider)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal TokenizerHelper(string str, char quoteChar, char separator)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal string GetCurrentToken()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void LastTokenRequired()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal bool NextToken()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal string NextTokenRequired()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal string NextTokenRequired(bool allowQuotedToken)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal bool NextToken(bool allowQuotedToken)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal bool NextToken(bool allowQuotedToken, char separator)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static char GetNumericListSeparator(IFormatProvider provider)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
