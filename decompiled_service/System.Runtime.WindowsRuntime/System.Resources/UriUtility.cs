using System.Text;

namespace System.Resources;

internal static class UriUtility
{
	public static string UriEncode(string str)
	{
		if (str == null)
		{
			return null;
		}
		byte[] bytes = Encoding.UTF8.GetBytes(str);
		int num = bytes.Length;
		if (num == 0)
		{
			return null;
		}
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			if (!IsUriUnreservedChar((char)bytes[i]))
			{
				num2++;
			}
		}
		byte[] array = null;
		if (num2 == 0)
		{
			array = bytes;
		}
		else
		{
			array = new byte[num + num2 * 2];
			int num3 = 0;
			for (int j = 0; j < num; j++)
			{
				byte b = bytes[j];
				if (IsUriUnreservedChar((char)b))
				{
					array[num3++] = b;
					continue;
				}
				array[num3++] = 37;
				array[num3++] = (byte)IntToHex((b >> 4) & 0xF);
				array[num3++] = (byte)IntToHex(b & 0xF);
			}
		}
		return Encoding.UTF8.GetString(array);
	}

	private static char IntToHex(int n)
	{
		if (n <= 9)
		{
			return (char)(n + 48);
		}
		return (char)(n - 10 + 97);
	}

	private static bool IsUriUnreservedChar(char ch)
	{
		if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z') || (ch >= '0' && ch <= '9'))
		{
			return true;
		}
		switch (ch)
		{
		case '-':
		case '.':
		case '_':
		case '~':
			return true;
		default:
			return false;
		}
	}
}
