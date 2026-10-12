using System;
using System.Text;
using Windows.Foundation;

namespace Windows.UI;

[__DynamicallyInvokable]
public struct Color : IFormattable
{
	private byte _A;

	private byte _R;

	private byte _G;

	private byte _B;

	[__DynamicallyInvokable]
	public byte A
	{
		[__DynamicallyInvokable]
		get
		{
			return _A;
		}
		[__DynamicallyInvokable]
		set
		{
			_A = value;
		}
	}

	[__DynamicallyInvokable]
	public byte R
	{
		[__DynamicallyInvokable]
		get
		{
			return _R;
		}
		[__DynamicallyInvokable]
		set
		{
			_R = value;
		}
	}

	[__DynamicallyInvokable]
	public byte G
	{
		[__DynamicallyInvokable]
		get
		{
			return _G;
		}
		[__DynamicallyInvokable]
		set
		{
			_G = value;
		}
	}

	[__DynamicallyInvokable]
	public byte B
	{
		[__DynamicallyInvokable]
		get
		{
			return _B;
		}
		[__DynamicallyInvokable]
		set
		{
			_B = value;
		}
	}

	[__DynamicallyInvokable]
	public static Color FromArgb(byte a, byte r, byte g, byte b)
	{
		return new Color
		{
			A = a,
			R = r,
			G = g,
			B = b
		};
	}

	[__DynamicallyInvokable]
	public override string ToString()
	{
		return ConvertToString(null, null);
	}

	[__DynamicallyInvokable]
	public string ToString(IFormatProvider provider)
	{
		return ConvertToString(null, provider);
	}

	[__DynamicallyInvokable]
	string IFormattable.ToString(string format, IFormatProvider provider)
	{
		return ConvertToString(format, provider);
	}

	internal string ConvertToString(string format, IFormatProvider provider)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (format == null)
		{
			stringBuilder.AppendFormat(provider, "#{0:X2}", new object[1] { _A });
			stringBuilder.AppendFormat(provider, "{0:X2}", new object[1] { _R });
			stringBuilder.AppendFormat(provider, "{0:X2}", new object[1] { _G });
			stringBuilder.AppendFormat(provider, "{0:X2}", new object[1] { _B });
		}
		else
		{
			char numericListSeparator = TokenizerHelper.GetNumericListSeparator(provider);
			stringBuilder.AppendFormat(provider, "sc#{1:" + format + "}{0} {2:" + format + "}{0} {3:" + format + "}{0} {4:" + format + "}", numericListSeparator, _A, _R, _G, _B);
		}
		return stringBuilder.ToString();
	}

	[__DynamicallyInvokable]
	public override int GetHashCode()
	{
		return _A.GetHashCode() ^ _R.GetHashCode() ^ _G.GetHashCode() ^ _B.GetHashCode();
	}

	[__DynamicallyInvokable]
	public override bool Equals(object o)
	{
		if (o is Color color)
		{
			return this == color;
		}
		return false;
	}

	[__DynamicallyInvokable]
	public bool Equals(Color color)
	{
		return this == color;
	}

	[__DynamicallyInvokable]
	public static bool operator ==(Color color1, Color color2)
	{
		if (color1.R != color2.R)
		{
			return false;
		}
		if (color1.G != color2.G)
		{
			return false;
		}
		if (color1.B != color2.B)
		{
			return false;
		}
		if (color1.A != color2.A)
		{
			return false;
		}
		return true;
	}

	[__DynamicallyInvokable]
	public static bool operator !=(Color color1, Color color2)
	{
		return !(color1 == color2);
	}
}
