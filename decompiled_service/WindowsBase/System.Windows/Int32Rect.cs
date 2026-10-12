using System.ComponentModel;
using System.Windows.Converters;
using System.Windows.Markup;

namespace System.Windows;

/// <summary>Describes the width, height, and location of an integer rectangle.</summary>
[Serializable]
[TypeConverter(typeof(Int32RectConverter))]
[ValueSerializer(typeof(Int32RectValueSerializer))]
public struct Int32Rect : IFormattable
{
	internal int _x;

	internal int _y;

	internal int _width;

	internal int _height;

	/// <summary>Gets or sets the x-coordinate of the top-left corner of the rectangle.</summary>
	/// <returns>The x-coordinate of the top-left corner of the rectangle. The default value is 0.</returns>
	public int X
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets or sets the y-coordinate of the top-left corner of the rectangle.</summary>
	/// <returns>The y-coordinate of the top-left corner of the rectangle. The default value is 0.</returns>
	public int Y
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets or sets the width of the rectangle.</summary>
	/// <returns>The width of the rectangle. The default value is 0.</returns>
	public int Width
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets or sets the height of the rectangle.</summary>
	/// <returns>The height of the rectangle. The default value is 0.</returns>
	public int Height
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the empty rectangle, a special value that represents a rectangle with no position or area. </summary>
	/// <returns>An empty rectangle with no position or area.</returns>
	public static Int32Rect Empty
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets a value indicating whether the rectangle is empty.</summary>
	/// <returns>
	///     true if the rectangle is empty; otherwise, false. The default value is true.</returns>
	public bool IsEmpty
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets a value that indicates whether the <see cref="P:System.Windows.Int32Rect.Width" /> and <see cref="P:System.Windows.Int32Rect.Height" /> properties of the <see cref="T:System.Windows.Int32Rect" /> are greater than 0.</summary>
	/// <returns>true if the <see cref="P:System.Windows.Int32Rect.Width" /> and <see cref="P:System.Windows.Int32Rect.Height" /> properties of the <see cref="T:System.Windows.Int32Rect" /> are greater than 0; otherwise, <paramref name="false" />.</returns>
	public bool HasArea
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Compares two rectangles for exact equality.</summary>
	/// <param name="int32Rect1">The first rectangle to compare.</param>
	/// <param name="int32Rect2">The second rectangle to compare.</param>
	/// <returns>
	///     true if int32Rect1 and int32Rect2 have the same <see cref="P:System.Windows.Int32Rect.X" />, <see cref="P:System.Windows.Int32Rect.Y" />, <see cref="P:System.Windows.Int32Rect.Width" />, and <see cref="P:System.Windows.Int32Rect.Height" />; otherwise, false.</returns>
	public static bool operator ==(Int32Rect int32Rect1, Int32Rect int32Rect2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Compares two rectangles for inequality.</summary>
	/// <param name="int32Rect1">The first rectangle to compare.</param>
	/// <param name="int32Rect2">The second rectangle to compare.</param>
	/// <returns>
	///     false if int32Rect1 and int32Rect2 have the same <see cref="P:System.Windows.Int32Rect.X" />, <see cref="P:System.Windows.Int32Rect.Y" />, <see cref="P:System.Windows.Int32Rect.Width" />, and <see cref="P:System.Windows.Int32Rect.Height" />; otherwise, if all of these values are the same, then true.</returns>
	public static bool operator !=(Int32Rect int32Rect1, Int32Rect int32Rect2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Determines whether the specified rectangles are equal.</summary>
	/// <param name="int32Rect1">The first rectangle to compare.</param>
	/// <param name="int32Rect2">The second rectangle to compare.</param>
	/// <returns>
	///     true if int32Rect1 and int32Rect2 have the same <see cref="P:System.Windows.Int32Rect.X" />, <see cref="P:System.Windows.Int32Rect.Y" />, <see cref="P:System.Windows.Int32Rect.Width" />, and <see cref="P:System.Windows.Int32Rect.Height" />; otherwise, false.</returns>
	public static bool Equals(Int32Rect int32Rect1, Int32Rect int32Rect2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Determines whether the specified rectangle is equal to this rectangle.</summary>
	/// <param name="o">The object to compare to the current rectangle.</param>
	/// <returns>
	///     true if o is an <see cref="T:System.Windows.Int32Rect" /> and the same <see cref="P:System.Windows.Int32Rect.X" />, <see cref="P:System.Windows.Int32Rect.Y" />, <see cref="P:System.Windows.Int32Rect.Width" />, and <see cref="P:System.Windows.Int32Rect.Height" /> as this rectangle; otherwise, false.</returns>
	public override bool Equals(object o)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Determines whether the specified rectangle is equal to this rectangle.</summary>
	/// <param name="value">The rectangle to compare to the current rectangle.</param>
	/// <returns>
	///     true if both rectangles have the same <see cref="P:System.Windows.Int32Rect.X" />, <see cref="P:System.Windows.Int32Rect.Y" />, <see cref="P:System.Windows.Int32Rect.Width" />, and <see cref="P:System.Windows.Int32Rect.Height" /> as this rectangle; otherwise, false.</returns>
	public bool Equals(Int32Rect value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates a hash code from this rectangle's <see cref="P:System.Windows.Int32Rect.X" />, <see cref="P:System.Windows.Int32Rect.Y" />, <see cref="P:System.Windows.Int32Rect.Width" />, and <see cref="P:System.Windows.Int32Rect.Height" /> values.</summary>
	/// <returns>This rectangle's hash code.</returns>
	public override int GetHashCode()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates an <see cref="T:System.Windows.Int32Rect" /> structure from the specified <see cref="T:System.String" /> representation.</summary>
	/// <param name="source">A string representation of an <see cref="T:System.Windows.Int32Rect" />.</param>
	/// <returns>The equivalent <see cref="T:System.Windows.Int32Rect" /> structure.</returns>
	public static Int32Rect Parse(string source)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates a string representation of this <see cref="T:System.Windows.Int32Rect" />.</summary>
	/// <returns>A string containing the same <see cref="P:System.Windows.Int32Rect.X" />, <see cref="P:System.Windows.Int32Rect.Y" />, <see cref="P:System.Windows.Int32Rect.Width" />, and <see cref="P:System.Windows.Int32Rect.Height" /> values of this <see cref="T:System.Windows.Int32Rect" /> structure.</returns>
	public override string ToString()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates a string representation of this <see cref="T:System.Windows.Int32Rect" /> based on the supplied <see cref="T:System.IFormatProvider" />.</summary>
	/// <param name="provider">The format provider to use. If provider is <see langword="null" />, the current culture is used.</param>
	/// <returns>A string representation of this instance of <see cref="T:System.Windows.Int32Rect" />.</returns>
	public string ToString(IFormatProvider provider)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Formats the value of the current instance using the specified format.</summary>
	/// <param name="format">The format to use.</param>
	/// <param name="provider">The provider to use to format the value</param>
	/// <returns>The value of the current instance in the specified format.</returns>
	string IFormattable.ToString(string format, IFormatProvider provider)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal string ConvertToString(string format, IFormatProvider provider)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of an <see cref="T:System.Windows.Int32Rect" /> with the specified <see cref="P:System.Windows.Int32Rect.X" /> and <see cref="P:System.Windows.Int32Rect.Y" /> coordinates and the specified <see cref="P:System.Windows.Int32Rect.Width" /> and <see cref="P:System.Windows.Int32Rect.Height" />. </summary>
	/// <param name="x">The <see cref="P:System.Windows.Int32Rect.X" /> of the new <see cref="T:System.Windows.Int32Rect" /> instance which specifies the x-coordinate of the top-left corner of the rectangle.</param>
	/// <param name="y">The <see cref="P:System.Windows.Int32Rect.Y" /> of the new <see cref="T:System.Windows.Int32Rect" /> instance which specifies the y-coordinate of the top-left corner of the rectangle.</param>
	/// <param name="width">The <see cref="P:System.Windows.Int32Rect.Width" /> of the new <see cref="T:System.Windows.Int32Rect" /> instance which specifies the width of the rectangle.</param>
	/// <param name="height">The <see cref="P:System.Windows.Int32Rect.Height" /> of the new <see cref="T:System.Windows.Int32Rect" /> instance which specifies the height of the rectangle.</param>
	public Int32Rect(int x, int y, int width, int height)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void ValidateForDirtyRect(string paramName, int width, int height)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static Int32Rect()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
