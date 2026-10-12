using System.ComponentModel;
using System.Windows.Converters;
using System.Windows.Markup;
using System.Windows.Media;

namespace System.Windows;

/// <summary>Represents a displacement in 2-D space. </summary>
[Serializable]
[TypeConverter(typeof(VectorConverter))]
[ValueSerializer(typeof(VectorValueSerializer))]
public struct Vector : IFormattable
{
	internal double _x;

	internal double _y;

	/// <summary>Gets or sets the <see cref="P:System.Windows.Vector.X" /> component of this vector. </summary>
	/// <returns>The <see cref="P:System.Windows.Vector.X" /> component of this vector. The default value is 0.</returns>
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

	/// <summary>Gets or sets the <see cref="P:System.Windows.Vector.Y" /> component of this vector. </summary>
	/// <returns>The <see cref="P:System.Windows.Vector.Y" /> component of this vector. The default value is 0.</returns>
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

	/// <summary>Gets the length of this vector. </summary>
	/// <returns>The length of this vector. </returns>
	public double Length
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the square of the length of this vector. </summary>
	/// <returns>The square of the <see cref="P:System.Windows.Vector.Length" /> of this vector.</returns>
	public double LengthSquared
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary> Compares two vectors for equality.</summary>
	/// <param name="vector1">The first vector to compare.</param>
	/// <param name="vector2">The second vector to compare.</param>
	/// <returns>
	///     <see langword="true" /> if the <see cref="P:System.Windows.Vector.X" /> and <see cref="P:System.Windows.Vector.Y" /> components of <paramref name="vector1" /> and <paramref name="vector2" /> are equal; otherwise, <see langword="false" />.</returns>
	public static bool operator ==(Vector vector1, Vector vector2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Compares two vectors for inequality.</summary>
	/// <param name="vector1">The first vector to compare.</param>
	/// <param name="vector2">The second vector to compare.</param>
	/// <returns>
	///     <see langword="true" /> if the <see cref="P:System.Windows.Vector.X" /> and <see cref="P:System.Windows.Vector.Y" /> components of <paramref name="vector1" /> and <paramref name="vector2" /> are different; otherwise, <see langword="false" />.</returns>
	public static bool operator !=(Vector vector1, Vector vector2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Compares the two specified vectors for equality.</summary>
	/// <param name="vector1">The first vector to compare.</param>
	/// <param name="vector2">The second vector to compare.</param>
	/// <returns>
	///     <see langword="true" /> if t he <see cref="P:System.Windows.Vector.X" /> and <see cref="P:System.Windows.Vector.Y" /> components of <paramref name="vector1" /> and <paramref name="vector2" /> are equal; otherwise, <see langword="false" />.</returns>
	public static bool Equals(Vector vector1, Vector vector2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Determines whether the specified <see cref="T:System.Object" /> is a <see cref="T:System.Windows.Vector" /> structure and, if it is, whether it has the same <see cref="P:System.Windows.Vector.X" /> and <see cref="P:System.Windows.Vector.Y" /> values as this vector.</summary>
	/// <param name="o">The vector to compare.</param>
	/// <returns>
	///     <see langword="true" /> if <paramref name="o" /> is a <see cref="T:System.Windows.Vector" /> and has the same <see cref="P:System.Windows.Vector.X" /> and <see cref="P:System.Windows.Vector.Y" /> values as this vector; otherwise, <see langword="false" />.</returns>
	public override bool Equals(object o)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary> Compares two vectors for equality.</summary>
	/// <param name="value">The vector to compare with this vector.</param>
	/// <returns>
	///     <see langword="true" /> if <paramref name="value" /> has the same <see cref="P:System.Windows.Vector.X" /> and <see cref="P:System.Windows.Vector.Y" /> values as this vector; otherwise, <see langword="false" />.</returns>
	public bool Equals(Vector value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns the hash code for this vector. </summary>
	/// <returns>The hash code for this instance.</returns>
	public override int GetHashCode()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Converts a string representation of a vector into the equivalent <see cref="T:System.Windows.Vector" /> structure. </summary>
	/// <param name="source">The string representation of the vector.</param>
	/// <returns>The equivalent <see cref="T:System.Windows.Vector" /> structure.</returns>
	public static Vector Parse(string source)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns the string representation of this <see cref="T:System.Windows.Vector" /> structure.</summary>
	/// <returns>A string that represents the <see cref="P:System.Windows.Vector.X" /> and <see cref="P:System.Windows.Vector.Y" /> values of this <see cref="T:System.Windows.Vector" />.</returns>
	public override string ToString()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns the string representation of this <see cref="T:System.Windows.Vector" /> structure with the specified formatting information. </summary>
	/// <param name="provider">The culture-specific formatting information.</param>
	/// <returns>A string that represents the <see cref="P:System.Windows.Vector.X" /> and <see cref="P:System.Windows.Vector.Y" /> values of this <see cref="T:System.Windows.Vector" />.</returns>
	public string ToString(IFormatProvider provider)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code. For a description of this member, see <see cref="M:System.IFormattable.ToString(System.String,System.IFormatProvider)" />.</summary>
	/// <param name="format">The string specifying the format to use. -or- 
	///       <see langword="null" /> to use the default format defined for the type of the <see cref="T:System.IFormattable" /> implementation.</param>
	/// <param name="provider">The <see langword="IFormatProvider" /> to use to format the value. -or- 
	///       <see langword="null" /> to obtain the numeric format information from the current locale setting of the operating system.</param>
	/// <returns>A string containing the value of the current instance in the specified format. </returns>
	string IFormattable.ToString(string format, IFormatProvider provider)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal string ConvertToString(string format, IFormatProvider provider)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Vector" /> structure. </summary>
	/// <param name="x">The <see cref="P:System.Windows.Vector.X" />-offset of the new <see cref="T:System.Windows.Vector" />.</param>
	/// <param name="y">The <see cref="P:System.Windows.Vector.Y" />-offset of the new <see cref="T:System.Windows.Vector" />.</param>
	public Vector(double x, double y)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary> Normalizes this vector. </summary>
	public void Normalize()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Calculates the cross product of two vectors. </summary>
	/// <param name="vector1">The first vector to evaluate.</param>
	/// <param name="vector2">The second vector to evaluate.</param>
	/// <returns>The cross product of <paramref name="vector1" /> and <paramref name="vector2" />. The following formula is used to calculate the cross product: 
	///     (Vector1.X * Vector2.Y) - (Vector1.Y * Vector2.X)
	///   </returns>
	public static double CrossProduct(Vector vector1, Vector vector2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Retrieves the angle, expressed in degrees, between the two specified vectors. </summary>
	/// <param name="vector1">The first vector to evaluate.</param>
	/// <param name="vector2">The second vector to evaluate.</param>
	/// <returns>The angle, in degrees, between <paramref name="vector1" /> and <paramref name="vector2" />.</returns>
	public static double AngleBetween(Vector vector1, Vector vector2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Negates the specified vector. </summary>
	/// <param name="vector">The vector to negate.</param>
	/// <returns>A vector with <see cref="P:System.Windows.Vector.X" /> and <see cref="P:System.Windows.Vector.Y" /> values opposite of the <see cref="P:System.Windows.Vector.X" /> and <see cref="P:System.Windows.Vector.Y" /> values of <paramref name="vector" />.</returns>
	public static Vector operator -(Vector vector)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Negates this vector. The vector has the same magnitude as before, but its direction is now opposite. </summary>
	public void Negate()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Adds two vectors and returns the result as a vector. </summary>
	/// <param name="vector1">The first vector to add.</param>
	/// <param name="vector2">The second vector to add.</param>
	/// <returns>The sum of <paramref name="vector1" /> and <paramref name="vector2" />. </returns>
	public static Vector operator +(Vector vector1, Vector vector2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Adds two vectors and returns the result as a <see cref="T:System.Windows.Vector" /> structure. </summary>
	/// <param name="vector1">The first vector to add.</param>
	/// <param name="vector2">The second vector to add.</param>
	/// <returns>The sum of <paramref name="vector1" /> and <paramref name="vector2" />.</returns>
	public static Vector Add(Vector vector1, Vector vector2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Subtracts one specified vector from another. </summary>
	/// <param name="vector1">The vector from which <paramref name="vector2" /> is subtracted. </param>
	/// <param name="vector2">The vector to subtract from <paramref name="vector1" />.</param>
	/// <returns>The difference between <paramref name="vector1" /> and <paramref name="vector2" />. </returns>
	public static Vector operator -(Vector vector1, Vector vector2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Subtracts the specified vector from another specified vector. </summary>
	/// <param name="vector1">The vector from which <paramref name="vector2" /> is subtracted.</param>
	/// <param name="vector2">The vector to subtract from <paramref name="vector1" />.</param>
	/// <returns>The difference between <paramref name="vector1" /> and <paramref name="vector2" />. </returns>
	public static Vector Subtract(Vector vector1, Vector vector2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary> Translates a point by the specified vector and returns the resulting point. </summary>
	/// <param name="vector">The vector used to translate <paramref name="point" />.</param>
	/// <param name="point">The point to translate.</param>
	/// <returns>The result of translating <paramref name="point" /> by <paramref name="vector" />.</returns>
	public static Point operator +(Vector vector, Point point)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Translates the specified point by the specified vector and returns the resulting point.</summary>
	/// <param name="vector">The amount to translate the specified point.</param>
	/// <param name="point">The point to translate.</param>
	/// <returns>The result of translating <paramref name="point" /> by <paramref name="vector" />.</returns>
	public static Point Add(Vector vector, Point point)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Multiplies the specified vector by the specified scalar and returns the resulting vector. </summary>
	/// <param name="vector">The vector to multiply.</param>
	/// <param name="scalar">The scalar to multiply.</param>
	/// <returns>The result of multiplying <paramref name="vector" /> and <paramref name="scalar" />.</returns>
	public static Vector operator *(Vector vector, double scalar)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary> Multiplies the specified vector by the specified scalar and returns the resulting <see cref="T:System.Windows.Vector" />. </summary>
	/// <param name="vector">The vector to multiply.</param>
	/// <param name="scalar">The scalar to multiply.</param>
	/// <returns>The result of multiplying <paramref name="vector" /> and <paramref name="scalar" />.</returns>
	public static Vector Multiply(Vector vector, double scalar)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary> Multiplies the specified scalar by the specified vector and returns the resulting vector. </summary>
	/// <param name="scalar">The scalar to multiply.</param>
	/// <param name="vector">The vector to multiply.</param>
	/// <returns>The result of multiplying <paramref name="scalar" /> and <paramref name="vector" />.</returns>
	public static Vector operator *(double scalar, Vector vector)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary> Multiplies the specified scalar by the specified vector and returns the resulting <see cref="T:System.Windows.Vector" />. </summary>
	/// <param name="scalar">The scalar to multiply.</param>
	/// <param name="vector">The vector to multiply.</param>
	/// <returns>The result of multiplying <paramref name="scalar" /> and <paramref name="vector" />.</returns>
	public static Vector Multiply(double scalar, Vector vector)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary> Divides the specified vector by the specified scalar and returns the resulting vector.</summary>
	/// <param name="vector">The vector to divide.</param>
	/// <param name="scalar">The scalar by which <paramref name="vector" /> will be divided.</param>
	/// <returns>The result of dividing <paramref name="vector" /> by <paramref name="scalar" />.</returns>
	public static Vector operator /(Vector vector, double scalar)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Divides the specified vector by the specified scalar and returns the result as a <see cref="T:System.Windows.Vector" />.</summary>
	/// <param name="vector">The vector structure to divide.</param>
	/// <param name="scalar">The amount by which <paramref name="vector" /> is divided.</param>
	/// <returns>The result of dividing <paramref name="vector" /> by <paramref name="scalar" />.</returns>
	public static Vector Divide(Vector vector, double scalar)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary> Transforms the coordinate space of the specified vector using the specified <see cref="T:System.Windows.Media.Matrix" />. </summary>
	/// <param name="vector">The vector to transform.</param>
	/// <param name="matrix">The transformation to apply to <paramref name="vector" />.</param>
	/// <returns>The result of transforming <paramref name="vector" /> by <paramref name="matrix" />.</returns>
	public static Vector operator *(Vector vector, Matrix matrix)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Transforms the coordinate space of the specified vector using the specified <see cref="T:System.Windows.Media.Matrix" />.</summary>
	/// <param name="vector">The vector structure to transform.</param>
	/// <param name="matrix">The transformation to apply to <paramref name="vector" />.</param>
	/// <returns>The result of transforming <paramref name="vector" /> by <paramref name="matrix" />.</returns>
	public static Vector Multiply(Vector vector, Matrix matrix)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary> Calculates the dot product of the two specified vector structures and returns the result as a <see cref="T:System.Double" />.</summary>
	/// <param name="vector1">The first vector to multiply.</param>
	/// <param name="vector2">The second vector to multiply.</param>
	/// <returns>Returns a <see cref="T:System.Double" /> containing the scalar dot product of <paramref name="vector1" /> and <paramref name="vector2" />, which is calculated using the following formula:
	///     vector1.X * vector2.X + vector1.Y * vector2.Y
	///   </returns>
	public static double operator *(Vector vector1, Vector vector2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Calculates the dot product of the two specified vectors and returns the result as a <see cref="T:System.Double" />.</summary>
	/// <param name="vector1">The first vector to multiply.</param>
	/// <param name="vector2">The second vector structure to multiply.</param>
	/// <returns>A <see cref="T:System.Double" /> containing the scalar dot product of <paramref name="vector1" /> and <paramref name="vector2" />, which is calculated using the following formula:
	///      (vector1.X * vector2.X) + (vector1.Y * vector2.Y) 
	///   </returns>
	public static double Multiply(Vector vector1, Vector vector2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Calculates the determinant of two vectors.</summary>
	/// <param name="vector1">The first vector to evaluate.</param>
	/// <param name="vector2">The second vector to evaluate.</param>
	/// <returns>The determinant of <paramref name="vector1" /> and <paramref name="vector2" />.</returns>
	public static double Determinant(Vector vector1, Vector vector2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates a <see cref="T:System.Windows.Size" /> from the offsets of this vector.</summary>
	/// <param name="vector">The vector to convert.</param>
	/// <returns>A <see cref="T:System.Windows.Size" /> with a <see cref="P:System.Windows.Size.Width" /> equal to the absolute value of this vector's <see cref="P:System.Windows.Vector.X" /> property and a <see cref="P:System.Windows.Size.Height" /> equal to the absolute value of this vector's <see cref="P:System.Windows.Vector.Y" /> property.</returns>
	public static explicit operator Size(Vector vector)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates a <see cref="T:System.Windows.Point" /> with the <see cref="P:System.Windows.Vector.X" /> and <see cref="P:System.Windows.Vector.Y" /> values of this vector. </summary>
	/// <param name="vector">The vector to convert.</param>
	/// <returns>A point with <see cref="P:System.Windows.Point.X" />- and <see cref="P:System.Windows.Point.Y" />-coordinate values equal to the <see cref="P:System.Windows.Vector.X" /> and <see cref="P:System.Windows.Vector.Y" /> offset values of <paramref name="vector" />.</returns>
	public static explicit operator Point(Vector vector)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
