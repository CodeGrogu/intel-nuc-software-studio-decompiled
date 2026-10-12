using System;

namespace Windows.Foundation;

[__DynamicallyInvokable]
public struct Rect : IFormattable
{
	private float _x;

	private float _y;

	private float _width;

	private float _height;

	private const double EmptyX = double.PositiveInfinity;

	private const double EmptyY = double.PositiveInfinity;

	private const double EmptyWidth = double.NegativeInfinity;

	private const double EmptyHeight = double.NegativeInfinity;

	private static readonly Rect s_empty = CreateEmptyRect();

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
	public double Width
	{
		[__DynamicallyInvokable]
		get
		{
			return _width;
		}
		[__DynamicallyInvokable]
		set
		{
			if (value < 0.0)
			{
				throw new ArgumentException("Width");
			}
			_width = (float)value;
		}
	}

	[__DynamicallyInvokable]
	public double Height
	{
		[__DynamicallyInvokable]
		get
		{
			return _height;
		}
		[__DynamicallyInvokable]
		set
		{
			if (value < 0.0)
			{
				throw new ArgumentException("Height");
			}
			_height = (float)value;
		}
	}

	[__DynamicallyInvokable]
	public double Left
	{
		[__DynamicallyInvokable]
		get
		{
			return _x;
		}
	}

	[__DynamicallyInvokable]
	public double Top
	{
		[__DynamicallyInvokable]
		get
		{
			return _y;
		}
	}

	[__DynamicallyInvokable]
	public double Right
	{
		[__DynamicallyInvokable]
		get
		{
			if (IsEmpty)
			{
				return double.NegativeInfinity;
			}
			return _x + _width;
		}
	}

	[__DynamicallyInvokable]
	public double Bottom
	{
		[__DynamicallyInvokable]
		get
		{
			if (IsEmpty)
			{
				return double.NegativeInfinity;
			}
			return _y + _height;
		}
	}

	[__DynamicallyInvokable]
	public static Rect Empty
	{
		[__DynamicallyInvokable]
		get
		{
			return s_empty;
		}
	}

	[__DynamicallyInvokable]
	public bool IsEmpty
	{
		[__DynamicallyInvokable]
		get
		{
			return _width < 0f;
		}
	}

	[__DynamicallyInvokable]
	public Rect(double x, double y, double width, double height)
	{
		if (width < 0.0)
		{
			throw new ArgumentException("width");
		}
		if (height < 0.0)
		{
			throw new ArgumentException("height");
		}
		_x = (float)x;
		_y = (float)y;
		_width = (float)width;
		_height = (float)height;
	}

	[__DynamicallyInvokable]
	public Rect(Point point1, Point point2)
	{
		_x = (float)Math.Min(point1.X, point2.X);
		_y = (float)Math.Min(point1.Y, point2.Y);
		_width = (float)Math.Max(Math.Max(point1.X, point2.X) - (double)_x, 0.0);
		_height = (float)Math.Max(Math.Max(point1.Y, point2.Y) - (double)_y, 0.0);
	}

	[__DynamicallyInvokable]
	public Rect(Point location, Size size)
	{
		if (size.IsEmpty)
		{
			this = s_empty;
			return;
		}
		_x = (float)location.X;
		_y = (float)location.Y;
		_width = (float)size.Width;
		_height = (float)size.Height;
	}

	internal static Rect Create(double x, double y, double width, double height)
	{
		if (x == double.PositiveInfinity && y == double.PositiveInfinity && width == double.NegativeInfinity && height == double.NegativeInfinity)
		{
			return Empty;
		}
		return new Rect(x, y, width, height);
	}

	[__DynamicallyInvokable]
	public bool Contains(Point point)
	{
		return ContainsInternal(point.X, point.Y);
	}

	[__DynamicallyInvokable]
	public void Intersect(Rect rect)
	{
		if (!IntersectsWith(rect))
		{
			this = s_empty;
			return;
		}
		double num = Math.Max(X, rect.X);
		double num2 = Math.Max(Y, rect.Y);
		Width = Math.Max(Math.Min(X + Width, rect.X + rect.Width) - num, 0.0);
		Height = Math.Max(Math.Min(Y + Height, rect.Y + rect.Height) - num2, 0.0);
		X = num;
		Y = num2;
	}

	[__DynamicallyInvokable]
	public void Union(Rect rect)
	{
		if (IsEmpty)
		{
			this = rect;
		}
		else if (!rect.IsEmpty)
		{
			double num = Math.Min(Left, rect.Left);
			double num2 = Math.Min(Top, rect.Top);
			if (rect.Width == double.PositiveInfinity || Width == double.PositiveInfinity)
			{
				Width = double.PositiveInfinity;
			}
			else
			{
				double num3 = Math.Max(Right, rect.Right);
				Width = Math.Max(num3 - num, 0.0);
			}
			if (rect.Height == double.PositiveInfinity || Height == double.PositiveInfinity)
			{
				Height = double.PositiveInfinity;
			}
			else
			{
				double num4 = Math.Max(Bottom, rect.Bottom);
				Height = Math.Max(num4 - num2, 0.0);
			}
			X = num;
			Y = num2;
		}
	}

	[__DynamicallyInvokable]
	public void Union(Point point)
	{
		Union(new Rect(point, point));
	}

	private bool ContainsInternal(double x, double y)
	{
		if (x >= X && x - Width <= X && y >= Y)
		{
			return y - Height <= Y;
		}
		return false;
	}

	internal bool IntersectsWith(Rect rect)
	{
		if (Width < 0.0 || rect.Width < 0.0)
		{
			return false;
		}
		if (rect.X <= X + Width && rect.X + rect.Width >= X && rect.Y <= Y + Height)
		{
			return rect.Y + rect.Height >= Y;
		}
		return false;
	}

	private static Rect CreateEmptyRect()
	{
		return new Rect
		{
			X = double.PositiveInfinity,
			Y = double.PositiveInfinity,
			_width = float.NegativeInfinity,
			_height = float.NegativeInfinity
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
		if (IsEmpty)
		{
			return SR.GetString("DirectUI_Empty");
		}
		char numericListSeparator = TokenizerHelper.GetNumericListSeparator(provider);
		return string.Format(provider, "{1:" + format + "}{0}{2:" + format + "}{0}{3:" + format + "}{0}{4:" + format + "}", numericListSeparator, _x, _y, _width, _height);
	}

	[__DynamicallyInvokable]
	public bool Equals(Rect value)
	{
		return this == value;
	}

	[__DynamicallyInvokable]
	public static bool operator ==(Rect rect1, Rect rect2)
	{
		if (rect1.X == rect2.X && rect1.Y == rect2.Y && rect1.Width == rect2.Width)
		{
			return rect1.Height == rect2.Height;
		}
		return false;
	}

	[__DynamicallyInvokable]
	public static bool operator !=(Rect rect1, Rect rect2)
	{
		return !(rect1 == rect2);
	}

	[__DynamicallyInvokable]
	public override bool Equals(object o)
	{
		if (o == null || !(o is Rect rect))
		{
			return false;
		}
		return this == rect;
	}

	[__DynamicallyInvokable]
	public override int GetHashCode()
	{
		return X.GetHashCode() ^ Y.GetHashCode() ^ Width.GetHashCode() ^ Height.GetHashCode();
	}
}
