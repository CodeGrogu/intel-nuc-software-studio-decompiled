using System.ComponentModel;
using System.Windows.Markup;
using System.Windows.Media.Converters;
using MS.Internal;

namespace System.Windows.Media;

/// <summary> Represents a 3x3 affine transformation matrix used for transformations in 2-D space. </summary>
[Serializable]
[TypeConverter(typeof(MatrixConverter))]
[ValueSerializer(typeof(MatrixValueSerializer))]
public struct Matrix : IFormattable
{
	private const int c_identityHashCode = 0;

	internal double _m11;

	internal double _m12;

	internal double _m21;

	internal double _m22;

	internal double _offsetX;

	internal double _offsetY;

	internal MatrixTypes _type;

	internal int _padding;

	/// <summary> Gets an identity <see cref="T:System.Windows.Media.Matrix" />. </summary>
	/// <returns>An identity matrix.</returns>
	public static Matrix Identity
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary> Gets a value that indicates whether this <see cref="T:System.Windows.Media.Matrix" /> structure is an identity matrix. </summary>
	/// <returns>
	///     <see langword="true" /> if the <see cref="T:System.Windows.Media.Matrix" /> structure is an identity matrix; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
	public bool IsIdentity
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary> Gets the determinant of this <see cref="T:System.Windows.Media.Matrix" /> structure. </summary>
	/// <returns>The determinant of this <see cref="T:System.Windows.Media.Matrix" />.</returns>
	public double Determinant
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary> Gets a value that indicates whether this <see cref="T:System.Windows.Media.Matrix" /> structure is invertible. </summary>
	/// <returns>
	///     <see langword="true" /> if the <see cref="T:System.Windows.Media.Matrix" /> has an inverse; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
	public bool HasInverse
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets or sets the value of the first row and first column of this <see cref="T:System.Windows.Media.Matrix" /> structure. </summary>
	/// <returns>The value of the first row and first column of this <see cref="T:System.Windows.Media.Matrix" />. The default value is 1.</returns>
	public double M11
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

	/// <summary> Gets or sets the value of the first row and second column of this <see cref="T:System.Windows.Media.Matrix" /> structure. </summary>
	/// <returns>The value of the first row and second column of this <see cref="T:System.Windows.Media.Matrix" />. The default value is 0.</returns>
	public double M12
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

	/// <summary> Gets or sets the value of the second row and first column of this <see cref="T:System.Windows.Media.Matrix" /> structure.</summary>
	/// <returns>The value of the second row and first column of this <see cref="T:System.Windows.Media.Matrix" />. The default value is 0.</returns>
	public double M21
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

	/// <summary>Gets or sets the value of the second row and second column of this <see cref="T:System.Windows.Media.Matrix" /> structure. </summary>
	/// <returns>The value of the second row and second column of this <see cref="T:System.Windows.Media.Matrix" /> structure. The default value is 1.</returns>
	public double M22
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

	/// <summary>Gets or sets the value of the third row and first column of this <see cref="T:System.Windows.Media.Matrix" /> structure.  </summary>
	/// <returns>The value of the third row and first column of this <see cref="T:System.Windows.Media.Matrix" /> structure. The default value is 0.</returns>
	public double OffsetX
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

	/// <summary>Gets or sets the value of the third row and second column of this <see cref="T:System.Windows.Media.Matrix" /> structure. </summary>
	/// <returns>The value of the third row and second column of this <see cref="T:System.Windows.Media.Matrix" /> structure. The default value is 0.</returns>
	public double OffsetY
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

	/// <summary> Initializes a new instance of the <see cref="T:System.Windows.Media.Matrix" /> structure. </summary>
	/// <param name="m11">The new <see cref="T:System.Windows.Media.Matrix" /> structure's <see cref="P:System.Windows.Media.Matrix.M11" /> coefficient.</param>
	/// <param name="m12">The new <see cref="T:System.Windows.Media.Matrix" /> structure's <see cref="P:System.Windows.Media.Matrix.M12" /> coefficient.</param>
	/// <param name="m21">The new <see cref="T:System.Windows.Media.Matrix" /> structure's <see cref="P:System.Windows.Media.Matrix.M21" /> coefficient.</param>
	/// <param name="m22">The new <see cref="T:System.Windows.Media.Matrix" /> structure's <see cref="P:System.Windows.Media.Matrix.M22" /> coefficient.</param>
	/// <param name="offsetX">The new <see cref="T:System.Windows.Media.Matrix" /> structure's <see cref="P:System.Windows.Media.Matrix.OffsetX" /> coefficient.</param>
	/// <param name="offsetY">The new <see cref="T:System.Windows.Media.Matrix" /> structure's <see cref="P:System.Windows.Media.Matrix.OffsetY" /> coefficient.</param>
	public Matrix(double m11, double m12, double m21, double m22, double offsetX, double offsetY)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary> Changes this <see cref="T:System.Windows.Media.Matrix" /> structure into an identity matrix. </summary>
	public void SetIdentity()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary> Multiplies a <see cref="T:System.Windows.Media.Matrix" /> structure by another <see cref="T:System.Windows.Media.Matrix" /> structure. </summary>
	/// <param name="trans1">The first <see cref="T:System.Windows.Media.Matrix" /> structure to multiply.</param>
	/// <param name="trans2">The second <see cref="T:System.Windows.Media.Matrix" /> structure to multiply.</param>
	/// <returns>The result of multiplying <paramref name="trans1" /> by <paramref name="trans2" />.</returns>
	public static Matrix operator *(Matrix trans1, Matrix trans2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary> Multiplies a <see cref="T:System.Windows.Media.Matrix" /> structure by another <see cref="T:System.Windows.Media.Matrix" /> structure. </summary>
	/// <param name="trans1">The first <see cref="T:System.Windows.Media.Matrix" /> structure to multiply.</param>
	/// <param name="trans2">The second <see cref="T:System.Windows.Media.Matrix" /> structure to multiply.</param>
	/// <returns>The result of multiplying <paramref name="trans1" /> by <paramref name="trans2" />.</returns>
	public static Matrix Multiply(Matrix trans1, Matrix trans2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary> Appends the specified <see cref="T:System.Windows.Media.Matrix" /> structure to this <see cref="T:System.Windows.Media.Matrix" /> structure. </summary>
	/// <param name="matrix">The <see cref="T:System.Windows.Media.Matrix" /> structure to append to this <see cref="T:System.Windows.Media.Matrix" /> structure.</param>
	public void Append(Matrix matrix)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary> Prepends the specified <see cref="T:System.Windows.Media.Matrix" /> structure onto this <see cref="T:System.Windows.Media.Matrix" /> structure. </summary>
	/// <param name="matrix">The <see cref="T:System.Windows.Media.Matrix" /> structure to prepend to this <see cref="T:System.Windows.Media.Matrix" /> structure.</param>
	public void Prepend(Matrix matrix)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary> Applies a rotation of the specified angle about the origin of this <see cref="T:System.Windows.Media.Matrix" /> structure. </summary>
	/// <param name="angle">The angle of rotation.</param>
	public void Rotate(double angle)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary> Prepends a rotation of the specified angle to this <see cref="T:System.Windows.Media.Matrix" /> structure. </summary>
	/// <param name="angle">The angle of rotation to prepend.</param>
	public void RotatePrepend(double angle)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Rotates this matrix about the specified point.</summary>
	/// <param name="angle">The angle, in degrees, by which to rotate this matrix. </param>
	/// <param name="centerX">The x-coordinate of the point about which to rotate this matrix.</param>
	/// <param name="centerY">The y-coordinate of the point about which to rotate this matrix.</param>
	public void RotateAt(double angle, double centerX, double centerY)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Prepends a rotation of the specified angle at the specified point to this <see cref="T:System.Windows.Media.Matrix" /> structure.</summary>
	/// <param name="angle">The rotation angle, in degrees.</param>
	/// <param name="centerX">The x-coordinate of the rotation center.</param>
	/// <param name="centerY">The y-coordinate of the rotation center.</param>
	public void RotateAtPrepend(double angle, double centerX, double centerY)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary> Appends the specified scale vector to this <see cref="T:System.Windows.Media.Matrix" /> structure. </summary>
	/// <param name="scaleX">The value by which to scale this <see cref="T:System.Windows.Media.Matrix" /> along the x-axis.</param>
	/// <param name="scaleY">The value by which to scale this <see cref="T:System.Windows.Media.Matrix" /> along the y-axis.</param>
	public void Scale(double scaleX, double scaleY)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary> Prepends the specified scale vector to this <see cref="T:System.Windows.Media.Matrix" /> structure. </summary>
	/// <param name="scaleX">The value by which to scale this <see cref="T:System.Windows.Media.Matrix" /> structure along the x-axis.</param>
	/// <param name="scaleY">The value by which to scale this <see cref="T:System.Windows.Media.Matrix" /> structure along the y-axis.</param>
	public void ScalePrepend(double scaleX, double scaleY)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Scales this <see cref="T:System.Windows.Media.Matrix" /> by the specified amount about the specified point.</summary>
	/// <param name="scaleX">The amount by which to scale this <see cref="T:System.Windows.Media.Matrix" /> along the x-axis. </param>
	/// <param name="scaleY">The amount by which to scale this <see cref="T:System.Windows.Media.Matrix" /> along the y-axis.</param>
	/// <param name="centerX">The x-coordinate of the scale operation's center point.</param>
	/// <param name="centerY">The y-coordinate of the scale operation's center point.</param>
	public void ScaleAt(double scaleX, double scaleY, double centerX, double centerY)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Prepends the specified scale about the specified point of this <see cref="T:System.Windows.Media.Matrix" />.</summary>
	/// <param name="scaleX">The x-axis scale factor.</param>
	/// <param name="scaleY">The y-axis scale factor.</param>
	/// <param name="centerX">The x-coordinate of the point about which the scale operation is performed.</param>
	/// <param name="centerY">The y-coordinate of the point about which the scale operation is performed.</param>
	public void ScaleAtPrepend(double scaleX, double scaleY, double centerX, double centerY)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary> Appends a skew of the specified degrees in the x and y dimensions to this <see cref="T:System.Windows.Media.Matrix" /> structure. </summary>
	/// <param name="skewX">The angle in the x dimension by which to skew this <see cref="T:System.Windows.Media.Matrix" />.</param>
	/// <param name="skewY">The angle in the y dimension by which to skew this <see cref="T:System.Windows.Media.Matrix" />.</param>
	public void Skew(double skewX, double skewY)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary> Prepends a skew of the specified degrees in the x and y dimensions to this <see cref="T:System.Windows.Media.Matrix" /> structure. </summary>
	/// <param name="skewX">The angle in the x dimension by which to skew this <see cref="T:System.Windows.Media.Matrix" />.</param>
	/// <param name="skewY">The angle in the y dimension by which to skew this <see cref="T:System.Windows.Media.Matrix" />.</param>
	public void SkewPrepend(double skewX, double skewY)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary> Appends a translation of the specified offsets to this <see cref="T:System.Windows.Media.Matrix" /> structure. </summary>
	/// <param name="offsetX">The amount to offset this <see cref="T:System.Windows.Media.Matrix" /> along the x-axis.</param>
	/// <param name="offsetY">The amount to offset this <see cref="T:System.Windows.Media.Matrix" /> along the y-axis.</param>
	public void Translate(double offsetX, double offsetY)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary> Prepends a translation of the specified offsets to this <see cref="T:System.Windows.Media.Matrix" /> structure. </summary>
	/// <param name="offsetX">The amount to offset this <see cref="T:System.Windows.Media.Matrix" /> along the x-axis.</param>
	/// <param name="offsetY">The amount to offset this <see cref="T:System.Windows.Media.Matrix" /> along the y-axis.</param>
	public void TranslatePrepend(double offsetX, double offsetY)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Transforms the specified point by the <see cref="T:System.Windows.Media.Matrix" /> and returns the result.</summary>
	/// <param name="point">The point to transform.</param>
	/// <returns>The result of transforming <paramref name="point" /> by this <see cref="T:System.Windows.Media.Matrix" />.</returns>
	public Point Transform(Point point)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Transforms the specified points by this <see cref="T:System.Windows.Media.Matrix" />. </summary>
	/// <param name="points">The points to transform. The original points in the array are replaced by their transformed values.</param>
	public void Transform(Point[] points)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Transforms the specified vector by this <see cref="T:System.Windows.Media.Matrix" />.</summary>
	/// <param name="vector">The vector to transform.</param>
	/// <returns>The result of transforming <paramref name="vector" /> by this <see cref="T:System.Windows.Media.Matrix" />.</returns>
	public Vector Transform(Vector vector)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Transforms the specified vectors by this <see cref="T:System.Windows.Media.Matrix" />.</summary>
	/// <param name="vectors">The vectors to transform. The original vectors in the array are replaced by their transformed values.</param>
	public void Transform(Vector[] vectors)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary> Inverts this <see cref="T:System.Windows.Media.Matrix" /> structure. </summary>
	/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Windows.Media.Matrix" /> structure is not invertible.</exception>
	public void Invert()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void MultiplyVector(ref double x, ref double y)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void MultiplyPoint(ref double x, ref double y)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static Matrix CreateRotationRadians(double angle)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static Matrix CreateRotationRadians(double angle, double centerX, double centerY)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static Matrix CreateScaling(double scaleX, double scaleY, double centerX, double centerY)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static Matrix CreateScaling(double scaleX, double scaleY)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static Matrix CreateSkewRadians(double skewX, double skewY)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static Matrix CreateTranslation(double offsetX, double offsetY)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary> Determines whether the two specified <see cref="T:System.Windows.Media.Matrix" /> structures are identical.</summary>
	/// <param name="matrix1">The first <see cref="T:System.Windows.Media.Matrix" /> structure to compare.</param>
	/// <param name="matrix2">The second <see cref="T:System.Windows.Media.Matrix" /> structure to compare.</param>
	/// <returns>
	///     <see langword="true" /> if <paramref name="matrix1" /> and <paramref name="matrix2" /> are identical; otherwise, <see langword="false" />.</returns>
	public static bool operator ==(Matrix matrix1, Matrix matrix2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary> Determines whether the two specified <see cref="T:System.Windows.Media.Matrix" /> structures are not identical.</summary>
	/// <param name="matrix1">The first <see cref="T:System.Windows.Media.Matrix" /> structure to compare.</param>
	/// <param name="matrix2">The second <see cref="T:System.Windows.Media.Matrix" /> structure to compare.</param>
	/// <returns>
	///     <see langword="true" /> if <paramref name="matrix1" /> and <paramref name="matrix2" /> are not identical; otherwise, <see langword="false" />.</returns>
	public static bool operator !=(Matrix matrix1, Matrix matrix2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary> Determines whether the two specified <see cref="T:System.Windows.Media.Matrix" /> structures are identical.</summary>
	/// <param name="matrix1">The first <see cref="T:System.Windows.Media.Matrix" /> structure to compare.</param>
	/// <param name="matrix2">The second <see cref="T:System.Windows.Media.Matrix" /> structure to compare.</param>
	/// <returns>
	///     <see langword="true" /> if <paramref name="matrix1" /> and <paramref name="matrix2" /> are identical; otherwise, <see langword="false" />.</returns>
	public static bool Equals(Matrix matrix1, Matrix matrix2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary> Determines whether the specified <see cref="T:System.Object" /> is a <see cref="T:System.Windows.Media.Matrix" /> structure that is identical to this <see cref="T:System.Windows.Media.Matrix" />. </summary>
	/// <param name="o">The <see cref="T:System.Object" /> to compare.</param>
	/// <returns>
	///     <see langword="true" /> if <paramref name="o" /> is a <see cref="T:System.Windows.Media.Matrix" /> structure that is identical to this <see cref="T:System.Windows.Media.Matrix" /> structure; otherwise, <see langword="false" />.</returns>
	public override bool Equals(object o)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary> Determines whether the specified <see cref="T:System.Windows.Media.Matrix" /> structure is identical to this instance. </summary>
	/// <param name="value">The instance of <see cref="T:System.Windows.Media.Matrix" /> to compare to this instance.</param>
	/// <returns>
	///     <see langword="true" /> if instances are equal; otherwise, <see langword="false" />. </returns>
	public bool Equals(Matrix value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary> Returns the hash code for this <see cref="T:System.Windows.Media.Matrix" /> structure. </summary>
	/// <returns>The hash code for this instance.</returns>
	public override int GetHashCode()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary> Converts a <see cref="T:System.String" /> representation of a matrix into the equivalent <see cref="T:System.Windows.Media.Matrix" /> structure. </summary>
	/// <param name="source">The <see cref="T:System.String" /> representation of the matrix.</param>
	/// <returns>The equivalent <see cref="T:System.Windows.Media.Matrix" /> structure.</returns>
	public static Matrix Parse(string source)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary> Creates a <see cref="T:System.String" /> representation of this <see cref="T:System.Windows.Media.Matrix" /> structure. </summary>
	/// <returns>A <see cref="T:System.String" /> containing the <see cref="P:System.Windows.Media.Matrix.M11" />, <see cref="P:System.Windows.Media.Matrix.M12" />, <see cref="P:System.Windows.Media.Matrix.M21" />, <see cref="P:System.Windows.Media.Matrix.M22" />, <see cref="P:System.Windows.Media.Matrix.OffsetX" />, and <see cref="P:System.Windows.Media.Matrix.OffsetY" /> values of this <see cref="T:System.Windows.Media.Matrix" />.</returns>
	public override string ToString()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary> Creates a <see cref="T:System.String" /> representation of this <see cref="T:System.Windows.Media.Matrix" /> structure with culture-specific formatting information. </summary>
	/// <param name="provider">The culture-specific formatting information.</param>
	/// <returns>A <see cref="T:System.String" /> containing the <see cref="P:System.Windows.Media.Matrix.M11" />, <see cref="P:System.Windows.Media.Matrix.M12" />, <see cref="P:System.Windows.Media.Matrix.M21" />, <see cref="P:System.Windows.Media.Matrix.M22" />, <see cref="P:System.Windows.Media.Matrix.OffsetX" />, and <see cref="P:System.Windows.Media.Matrix.OffsetY" /> values of this <see cref="T:System.Windows.Media.Matrix" />.</returns>
	public string ToString(IFormatProvider provider)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Formats the value of the current instance using the specified format.</summary>
	/// <param name="format">The format to use.-or- A null reference (<see langword="Nothing" /> in Visual Basic) to use the default format defined for the type of the <see cref="T:System.IFormattable" /> implementation. </param>
	/// <param name="provider">The provider to use to format the value.-or- A null reference (<see langword="Nothing" /> in Visual Basic) to obtain the numeric format information from the current locale setting of the operating system. </param>
	/// <returns>The value of the current instance in the specified format.</returns>
	string IFormattable.ToString(string format, IFormatProvider provider)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal string ConvertToString(string format, IFormatProvider provider)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static Matrix()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
