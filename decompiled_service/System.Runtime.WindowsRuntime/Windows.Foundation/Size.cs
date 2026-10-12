using System;

namespace Windows.Foundation;

[__DynamicallyInvokable]
public struct Size
{
	private float _width;

	private float _height;

	private static readonly Size s_empty = CreateEmptySize();

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
	public static Size Empty
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
			return Width < 0.0;
		}
	}

	[__DynamicallyInvokable]
	public Size(double width, double height)
	{
		if (width < 0.0)
		{
			throw new ArgumentException("width");
		}
		if (height < 0.0)
		{
			throw new ArgumentException("height");
		}
		_width = (float)width;
		_height = (float)height;
	}

	private static Size CreateEmptySize()
	{
		return new Size
		{
			_width = float.NegativeInfinity,
			_height = float.NegativeInfinity
		};
	}

	[__DynamicallyInvokable]
	public static bool operator ==(Size size1, Size size2)
	{
		if (size1.Width == size2.Width)
		{
			return size1.Height == size2.Height;
		}
		return false;
	}

	[__DynamicallyInvokable]
	public static bool operator !=(Size size1, Size size2)
	{
		return !(size1 == size2);
	}

	[__DynamicallyInvokable]
	public override bool Equals(object o)
	{
		if (o == null || !(o is Size size))
		{
			return false;
		}
		return Equals(this, size);
	}

	[__DynamicallyInvokable]
	public bool Equals(Size value)
	{
		return Equals(this, value);
	}

	[__DynamicallyInvokable]
	public override int GetHashCode()
	{
		if (IsEmpty)
		{
			return 0;
		}
		return Width.GetHashCode() ^ Height.GetHashCode();
	}

	private static bool Equals(Size size1, Size size2)
	{
		if (size1.IsEmpty)
		{
			return size2.IsEmpty;
		}
		if (size1.Width.Equals(size2.Width))
		{
			return size1.Height.Equals(size2.Height);
		}
		return false;
	}

	[__DynamicallyInvokable]
	public override string ToString()
	{
		if (IsEmpty)
		{
			return "Empty";
		}
		return $"{_width},{_height}";
	}
}
