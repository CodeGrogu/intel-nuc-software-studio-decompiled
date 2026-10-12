using System.ComponentModel;
using System.Windows.Converters;
using System.Windows.Markup;
using System.Windows.Media;

namespace System.Windows;

/// <summary>Describes the width, height, and location of a rectangle. </summary>
[Serializable]
[TypeConverter(typeof(RectConverter))]
[ValueSerializer(typeof(RectValueSerializer))]
public struct Rect : IFormattable
{
	internal double _x;

	internal double _y;

	internal double _width;

	internal double _height;

	/// <summary>Gets a special value that represents a rectangle with no position or area. </summary>
	/// <returns>The empty rectangle, which has <see cref="P:System.Windows.Rect.X" /> and <see cref="P:System.Windows.Rect.Y" /> property values of <see cref="F:System.Double.PositiveInfinity" />, and has <see cref="P:System.Windows.Rect.Width" /> and <see cref="P:System.Windows.Rect.Height" /> property values of <see cref="F:System.Double.NegativeInfinity" />.</returns>
	public static Rect Empty
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets a value that indicates whether the rectangle is the <see cref="P:System.Windows.Rect.Empty" /> rectangle.</summary>
	/// <returns>
	///     <see langword="true" /> if the rectangle is the <see cref="P:System.Windows.Rect.Empty" /> rectangle; otherwise, <see langword="false" />.</returns>
	public bool IsEmpty
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets or sets the position of the top-left corner of the rectangle.</summary>
	/// <returns>The position of the top-left corner of the rectangle. The default is (0, 0). </returns>
	/// <exception cref="T:System.InvalidOperationException">
	///         <see cref="P:System.Windows.Rect.Location" /> is set on an <see cref="P:System.Windows.Rect.Empty" /> rectangle. </exception>
	public Point Location
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

	/// <summary>Gets or sets the width and height of the rectangle. </summary>
	/// <returns>A <see cref="T:System.Windows.Size" /> structure that specifies the width and height of the rectangle.</returns>
	/// <exception cref="T:System.InvalidOperationException">
	///         <see cref="P:System.Windows.Rect.Size" /> is set on an <see cref="P:System.Windows.Rect.Empty" /> rectangle. </exception>
	public Size Size
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

	/// <summary>Gets or sets the x-axis value of the left side of the rectangle. </summary>
	/// <returns>The x-axis value of the left side of the rectangle.</returns>
	/// <exception cref="T:System.InvalidOperationException">
	///         <see cref="P:System.Windows.Rect.X" /> is set on an <see cref="P:System.Windows.Rect.Empty" /> rectangle. </exception>
	public double X
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

	/// <summary>Gets or sets the y-axis value of the top side of the rectangle. </summary>
	/// <returns>The y-axis value of the top side of the rectangle.</returns>
	/// <exception cref="T:System.InvalidOperationException">
	///         <see cref="P:System.Windows.Rect.Y" /> is set on an <see cref="P:System.Windows.Rect.Empty" /> rectangle. </exception>
	public double Y
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

	/// <summary>Gets or sets the width of the rectangle.  </summary>
	/// <returns>A positive number that represents the width of the rectangle. The default is 0.</returns>
	/// <exception cref="T:System.ArgumentException">
	///         <see cref="P:System.Windows.Rect.Width" /> is set to a negative value.</exception>
	/// <exception cref="T:System.InvalidOperationException">
	///         <see cref="P:System.Windows.Rect.Width" /> is set on an <see cref="P:System.Windows.Rect.Empty" /> rectangle. </exception>
	public double Width
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

	/// <summary>Gets or sets the height of the rectangle. </summary>
	/// <returns>A positive number that represents the height of the rectangle. The default is 0.</returns>
	/// <exception cref="T:System.ArgumentException">
	///         <see cref="P:System.Windows.Rect.Height" /> is set to a negative value.</exception>
	/// <exception cref="T:System.InvalidOperationException">
	///         <see cref="P:System.Windows.Rect.Height" /> is set on an <see cref="P:System.Windows.Rect.Empty" /> rectangle. </exception>
	public double Height
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

	/// <summary>Gets the x-axis value of the left side of the rectangle. </summary>
	/// <returns>The x-axis value of the left side of the rectangle.</returns>
	public double Left
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the y-axis position of the top of the rectangle. </summary>
	/// <returns>The y-axis position of the top of the rectangle.</returns>
	public double Top
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the x-axis value of the right side of the rectangle.  </summary>
	/// <returns>The x-axis value of the right side of the rectangle.</returns>
	public double Right
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the y-axis value of the bottom of the rectangle. </summary>
	/// <returns>The y-axis value of the bottom of the rectangle. If the rectangle is empty, the value is <see cref="F:System.Double.NegativeInfinity" /> .</returns>
	public double Bottom
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the position of the top-left corner of the rectangle. </summary>
	/// <returns>The position of the top-left corner of the rectangle.</returns>
	public Point TopLeft
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the position of the top-right corner of the rectangle. </summary>
	/// <returns>The position of the top-right corner of the rectangle.</returns>
	public Point TopRight
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the position of the bottom-left corner of the rectangle </summary>
	/// <returns>The position of the bottom-left corner of the rectangle.</returns>
	public Point BottomLeft
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the position of the bottom-right corner of the rectangle. </summary>
	/// <returns>The position of the bottom-right corner of the rectangle.</returns>
	public Point BottomRight
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Compares two rectangles for exact equality.</summary>
	/// <param name="rect1">The first rectangle to compare.</param>
	/// <param name="rect2">The second rectangle to compare.</param>
	/// <returns>
	///     <see langword="true" /> if the rectangles have the same <see cref="P:System.Windows.Rect.Location" /> and <see cref="P:System.Windows.Rect.Size" /> values; otherwise, <see langword="false" />.</returns>
	public static bool operator ==(Rect rect1, Rect rect2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Compares two rectangles for inequality.  </summary>
	/// <param name="rect1">The first rectangle to compare.</param>
	/// <param name="rect2">The second rectangle to compare.</param>
	/// <returns>
	///     <see langword="true" /> if the rectangles do not have the same <see cref="P:System.Windows.Rect.Location" /> and <see cref="P:System.Windows.Rect.Size" /> values; otherwise, <see langword="false" />.</returns>
	public static bool operator !=(Rect rect1, Rect rect2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Indicates whether the specified rectangles are equal. </summary>
	/// <param name="rect1">The first rectangle to compare.</param>
	/// <param name="rect2">The second rectangle to compare.</param>
	/// <returns>
	///     <see langword="true" /> if the rectangles have the same <see cref="P:System.Windows.Rect.Location" /> and <see cref="P:System.Windows.Rect.Size" /> values; otherwise, <see langword="false" />.</returns>
	public static bool Equals(Rect rect1, Rect rect2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Indicates whether the specified object is equal to the current rectangle.</summary>
	/// <param name="o">The object to compare to the current rectangle.</param>
	/// <returns>
	///     <see langword="true" /> if <paramref name="o" /> is a <see cref="T:System.Windows.Rect" /> and has the same <see cref="P:System.Windows.Rect.Location" /> and <see cref="P:System.Windows.Rect.Size" /> values as the current rectangle; otherwise, <see langword="false" />.</returns>
	public override bool Equals(object o)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Indicates whether the specified rectangle is equal to the current rectangle. </summary>
	/// <param name="value">The rectangle to compare to the current rectangle.</param>
	/// <returns>
	///     <see langword="true" /> if the specified rectangle has the same <see cref="P:System.Windows.Rect.Location" /> and <see cref="P:System.Windows.Rect.Size" /> values as the current rectangle; otherwise, <see langword="false" />.</returns>
	public bool Equals(Rect value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates a hash code for the rectangle. </summary>
	/// <returns>A hash code for the current <see cref="T:System.Windows.Rect" /> structure.</returns>
	public override int GetHashCode()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates a new rectangle from the specified string representation. </summary>
	/// <param name="source">The string representation of the rectangle, in the form "x, y, width, height".</param>
	/// <returns>The resulting rectangle.</returns>
	public static Rect Parse(string source)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns a string representation of the rectangle. </summary>
	/// <returns>A string representation of the current rectangle. The string has the following form: "<see cref="P:System.Windows.Rect.X" />,<see cref="P:System.Windows.Rect.Y" />,<see cref="P:System.Windows.Rect.Width" />,<see cref="P:System.Windows.Rect.Height" />".</returns>
	public override string ToString()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns a string representation of the rectangle by using the specified format provider. </summary>
	/// <param name="provider">Culture-specific formatting information.</param>
	/// <returns>A string representation of the current rectangle that is determined by the specified format provider.</returns>
	public string ToString(IFormatProvider provider)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Formats the value of the current instance using the specified format.</summary>
	/// <param name="format">The format to use.-or- A null reference (<see langword="Nothing" /> in Visual Basic) to use the default format defined for the type of the <see cref="T:System.IFormattable" /> implementation. </param>
	/// <param name="provider">The provider to use to format the value.-or- A null reference (<see langword="Nothing" /> in Visual Basic) to obtain the numeric format information from the current locale setting of the operating system. </param>
	/// <returns>A string representation of the rectangle.</returns>
	string IFormattable.ToString(string format, IFormatProvider provider)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal string ConvertToString(string format, IFormatProvider provider)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Rect" /> structure that has the specified top-left corner location and the specified width and height. </summary>
	/// <param name="location">A point that specifies the location of the top-left corner of the rectangle.</param>
	/// <param name="size">A <see cref="T:System.Windows.Size" /> structure that specifies the width and height of the rectangle.</param>
	public Rect(Point location, Size size)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Rect" /> structure that has the specified x-coordinate, y-coordinate, width, and height. </summary>
	/// <param name="x">The x-coordinate of the top-left corner of the rectangle.</param>
	/// <param name="y">The y-coordinate of the top-left corner of the rectangle.</param>
	/// <param name="width">The width of the rectangle.</param>
	/// <param name="height">The height of the rectangle.</param>
	/// <exception cref="T:System.ArgumentException">
	///         <paramref name="width" /> is a negative value.-or-
	///         <paramref name="height" /> is a negative value.</exception>
	public Rect(double x, double y, double width, double height)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Rect" /> structure that is exactly large enough to contain the two specified points. </summary>
	/// <param name="point1">The first point that the new rectangle must contain.</param>
	/// <param name="point2">The second point that the new rectangle must contain.</param>
	public Rect(Point point1, Point point2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Rect" /> structure that is exactly large enough to contain the specified point and the sum of the specified point and the specified vector. </summary>
	/// <param name="point">The first point the rectangle must contain.</param>
	/// <param name="vector">The amount to offset the specified point. The resulting rectangle will be exactly large enough to contain both points.</param>
	public Rect(Point point, Vector vector)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Rect" /> structure that is of the specified size and is located at (0,0).  </summary>
	/// <param name="size">A <see cref="T:System.Windows.Size" /> structure that specifies the width and height of the rectangle.</param>
	public Rect(Size size)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Indicates whether the rectangle contains the specified point.</summary>
	/// <param name="point">The point to check.</param>
	/// <returns>
	///     <see langword="true" /> if the rectangle contains the specified point; otherwise, <see langword="false" />.</returns>
	public bool Contains(Point point)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Indicates whether the rectangle contains the specified x-coordinate and y-coordinate. </summary>
	/// <param name="x">The x-coordinate of the point to check.</param>
	/// <param name="y">The y-coordinate of the point to check.</param>
	/// <returns>
	///     <see langword="true" /> if (<paramref name="x" />, <paramref name="y" />) is contained by the rectangle; otherwise, <see langword="false" />.</returns>
	public bool Contains(double x, double y)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Indicates whether the rectangle contains the specified rectangle. </summary>
	/// <param name="rect">The rectangle to check.</param>
	/// <returns>
	///     <see langword="true" /> if <paramref name="rect" /> is entirely contained by the rectangle; otherwise, <see langword="false" />.</returns>
	public bool Contains(Rect rect)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Indicates whether the specified rectangle intersects with the current rectangle. </summary>
	/// <param name="rect">The rectangle to check.</param>
	/// <returns>
	///     <see langword="true" /> if the specified rectangle intersects with the current rectangle; otherwise, <see langword="false" />.</returns>
	public bool IntersectsWith(Rect rect)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Finds the intersection of the current rectangle and the specified rectangle, and stores the result as the current rectangle. </summary>
	/// <param name="rect">The rectangle to intersect with the current rectangle.</param>
	public void Intersect(Rect rect)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns the intersection of the specified rectangles. </summary>
	/// <param name="rect1">The first rectangle to compare.</param>
	/// <param name="rect2">The second rectangle to compare.</param>
	/// <returns>The intersection of the two rectangles, or <see cref="P:System.Windows.Rect.Empty" /> if no intersection exists.</returns>
	public static Rect Intersect(Rect rect1, Rect rect2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Expands the current rectangle exactly enough to contain the specified rectangle. </summary>
	/// <param name="rect">The rectangle to include.</param>
	public void Union(Rect rect)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates a rectangle that is exactly large enough to contain the two specified rectangles. </summary>
	/// <param name="rect1">The first rectangle to include.</param>
	/// <param name="rect2">The second rectangle to include.</param>
	/// <returns>The resulting rectangle.</returns>
	public static Rect Union(Rect rect1, Rect rect2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Expands the current rectangle exactly enough to contain the specified point. </summary>
	/// <param name="point">The point to include.</param>
	public void Union(Point point)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates a rectangle that is exactly large enough to include the specified rectangle and the specified point. </summary>
	/// <param name="rect">The rectangle to include.</param>
	/// <param name="point">The point to include.</param>
	/// <returns>A rectangle that is exactly large enough to contain the specified rectangle and the specified point.</returns>
	public static Rect Union(Rect rect, Point point)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Moves the rectangle by the specified vector. </summary>
	/// <param name="offsetVector">A vector that specifies the horizontal and vertical amounts to move the rectangle.</param>
	/// <exception cref="T:System.InvalidOperationException">This method is called on the <see cref="P:System.Windows.Rect.Empty" /> rectangle.</exception>
	public void Offset(Vector offsetVector)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Moves the rectangle by the specified horizontal and vertical amounts. </summary>
	/// <param name="offsetX">The amount to move the rectangle horizontally.</param>
	/// <param name="offsetY">The amount to move the rectangle vertically.</param>
	/// <exception cref="T:System.InvalidOperationException">This method is called on the <see cref="P:System.Windows.Rect.Empty" /> rectangle.</exception>
	public void Offset(double offsetX, double offsetY)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns a rectangle that is offset from the specified rectangle by using the specified vector. </summary>
	/// <param name="rect">The original rectangle.</param>
	/// <param name="offsetVector">A vector that specifies the horizontal and vertical offsets for the new rectangle.</param>
	/// <returns>The resulting rectangle.</returns>
	/// <exception cref="T:System.InvalidOperationException">
	///         <paramref name="rect" /> is <see cref="P:System.Windows.Rect.Empty" />.</exception>
	public static Rect Offset(Rect rect, Vector offsetVector)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns a rectangle that is offset from the specified rectangle by using the specified horizontal and vertical amounts. </summary>
	/// <param name="rect">The rectangle to move.</param>
	/// <param name="offsetX">The horizontal offset for the new rectangle.</param>
	/// <param name="offsetY">The vertical offset for the new rectangle.</param>
	/// <returns>The resulting rectangle.</returns>
	/// <exception cref="T:System.InvalidOperationException">
	///         <paramref name="rect" /> is <see cref="P:System.Windows.Rect.Empty" />.</exception>
	public static Rect Offset(Rect rect, double offsetX, double offsetY)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Expands the rectangle by using the specified <see cref="T:System.Windows.Size" />, in all directions. </summary>
	/// <param name="size">Specifies the amount to expand the rectangle. The <see cref="T:System.Windows.Size" /> structure's <see cref="P:System.Windows.Size.Width" /> property specifies the amount to increase the rectangle's <see cref="P:System.Windows.Rect.Left" /> and <see cref="P:System.Windows.Rect.Right" /> properties. The <see cref="T:System.Windows.Size" /> structure's <see cref="P:System.Windows.Size.Height" /> property specifies the amount to increase the rectangle's <see cref="P:System.Windows.Rect.Top" /> and <see cref="P:System.Windows.Rect.Bottom" /> properties. </param>
	/// <exception cref="T:System.InvalidOperationException">This method is called on the <see cref="P:System.Windows.Rect.Empty" /> rectangle.</exception>
	public void Inflate(Size size)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Expands or shrinks the rectangle by using the specified width and height amounts, in all directions. </summary>
	/// <param name="width">The amount by which to expand or shrink the left and right sides of the rectangle.</param>
	/// <param name="height">The amount by which to expand or shrink the top and bottom sides of the rectangle.</param>
	/// <exception cref="T:System.InvalidOperationException">This method is called on the <see cref="P:System.Windows.Rect.Empty" /> rectangle.</exception>
	public void Inflate(double width, double height)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns the rectangle that results from expanding the specified rectangle by the specified <see cref="T:System.Windows.Size" />, in all directions. </summary>
	/// <param name="rect">The <see cref="T:System.Windows.Rect" /> structure to modify.</param>
	/// <param name="size">Specifies the amount to expand the rectangle. The <see cref="T:System.Windows.Size" /> structure's <see cref="P:System.Windows.Size.Width" /> property specifies the amount to increase the rectangle's <see cref="P:System.Windows.Rect.Left" /> and <see cref="P:System.Windows.Rect.Right" /> properties. The <see cref="T:System.Windows.Size" /> structure's <see cref="P:System.Windows.Size.Height" /> property specifies the amount to increase the rectangle's <see cref="P:System.Windows.Rect.Top" /> and <see cref="P:System.Windows.Rect.Bottom" /> properties.</param>
	/// <returns>The resulting rectangle.</returns>
	/// <exception cref="T:System.InvalidOperationException">
	///         <paramref name="rect" /> is an <see cref="P:System.Windows.Rect.Empty" /> rectangle.</exception>
	public static Rect Inflate(Rect rect, Size size)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates a rectangle that results from expanding or shrinking the specified rectangle by the specified width and height amounts, in all directions. </summary>
	/// <param name="rect">The <see cref="T:System.Windows.Rect" /> structure to modify.</param>
	/// <param name="width">The amount by which to expand or shrink the left and right sides of the rectangle.</param>
	/// <param name="height">The amount by which to expand or shrink the top and bottom sides of the rectangle.</param>
	/// <returns>The resulting rectangle. </returns>
	/// <exception cref="T:System.InvalidOperationException">
	///         <paramref name="rect" /> is an <see cref="P:System.Windows.Rect.Empty" /> rectangle.</exception>
	public static Rect Inflate(Rect rect, double width, double height)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns the rectangle that results from applying the specified matrix to the specified rectangle. </summary>
	/// <param name="rect">A rectangle that is the basis for the transformation.</param>
	/// <param name="matrix">A matrix that specifies the transformation to apply.</param>
	/// <returns>The rectangle that results from the operation.</returns>
	public static Rect Transform(Rect rect, Matrix matrix)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Transforms the rectangle by applying the specified matrix. </summary>
	/// <param name="matrix">A matrix that specifies the transformation to apply.</param>
	public void Transform(Matrix matrix)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Multiplies the size of the current rectangle by the specified x and y values.</summary>
	/// <param name="scaleX">The scale factor in the x-direction.</param>
	/// <param name="scaleY">The scale factor in the y-direction.</param>
	public void Scale(double scaleX, double scaleY)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static Rect()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
