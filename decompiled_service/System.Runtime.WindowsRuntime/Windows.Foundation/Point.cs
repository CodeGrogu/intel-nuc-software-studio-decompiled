using System;

namespace Windows.Foundation;

[__DynamicallyInvokable]
public struct Point(double x, double y) : IFormattable
{
	internal float _x = (float)x;

	internal float _y = (float)y;

	[__DynamicallyInvokable]
	public double X
	{
		[__DynamicallyInvokable]
		get
		{
			return _x;
		}
		[__DynamicallyInvokable]
		set
		{
			_x = (float)value;
		}
	}

	[__DynamicallyInvokable]
	public double Y
	{
		[__DynamicallyInvokable]
		get
		{
			return _y;
		}
		[__DynamicallyInvokable]
		set
		{
			_y = (float)value;
		}
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

	private string ConvertToString(string format, IFormatProvider provider)
	{
		char numericListSeparator = TokenizerHelper.GetNumericListSeparator(provider);
		return string.Format(provider, "{1:" + format + "}{0}{2:" + format + "}", new object[3] { numericListSeparator, _x, _y });
	}

	[__DynamicallyInvokable]
	public static bool operator ==(Point point1, Point point2)
	{
		if (point1.X == point2.X)
		{
			return point1.Y == point2.Y;
		}
		return false;
	}

	[__DynamicallyInvokable]
	public static bool operator !=(Point point1, Point point2)
	{
		return !(point1 == point2);
	}

	[__DynamicallyInvokable]
	public override bool Equals(object o)
	{
		if (o == null || !(o is Point point))
		{
			return false;
		}
		return this == point;
	}

	[__DynamicallyInvokable]
	public bool Equals(Point value)
	{
		return this == value;
	}

	[__DynamicallyInvokable]
	public override int GetHashCode()
	{
		return X.GetHashCode() ^ Y.GetHashCode();
	}
}
