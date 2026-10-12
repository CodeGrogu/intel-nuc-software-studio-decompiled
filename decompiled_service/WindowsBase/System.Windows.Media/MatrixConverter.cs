using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Media;

/// <summary>Converts instances of other types to and from a <see cref="T:System.Windows.Media.Matrix" />. </summary>
public sealed class MatrixConverter : TypeConverter
{
	/// <summary>Determines whether an object can be converted from a specific type to an instance of a <see cref="T:System.Windows.Media.Matrix" />.  </summary>
	/// <param name="context">The context information of a type.</param>
	/// <param name="sourceType">The type of the source that is being evaluated for conversion.</param>
	/// <returns>
	///     <see langword="true" /> if the type can be converted to a <see cref="T:System.Windows.Media.Matrix" />; otherwise, <see langword="false" />.</returns>
	public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Determines whether an instance of a <see cref="T:System.Windows.Media.Matrix" /> can be converted to a different type. </summary>
	/// <param name="context">The context information of a type.</param>
	/// <param name="destinationType">The desired type this <see cref="T:System.Windows.Media.Matrix" /> is being evaluated for conversion.</param>
	/// <returns>
	///     <see langword="true" /> if this <see cref="T:System.Windows.Media.Matrix" /> can be converted to <paramref name="destinationType" />; otherwise, <see langword="false" />.</returns>
	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Attempts to convert the specified object to a <see cref="T:System.Windows.Media.Matrix" />. </summary>
	/// <param name="context">The context information of a type.</param>
	/// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> of the type being converted.</param>
	/// <param name="value">The object being converted.</param>
	/// <returns>The <see cref="T:System.Windows.Media.Matrix" /> created from converting <paramref name="value" />.</returns>
	/// <exception cref="T:System.NotSupportedException">The specified object is null or is a type that cannot be converted to a <see cref="T:System.Windows.Media.Matrix" />.</exception>
	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Attempts to convert a <see cref="T:System.Windows.Media.Matrix" /> to a specified type. </summary>
	/// <param name="context">The context information of a type.</param>
	/// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> of the type being converted.</param>
	/// <param name="value">The <see cref="T:System.Windows.Media.Matrix" /> to convert.</param>
	/// <param name="destinationType">The type to convert this <see cref="T:System.Windows.Media.Matrix" /> to.</param>
	/// <returns>The object created from converting this <see cref="T:System.Windows.Media.Matrix" />.</returns>
	/// <exception cref="T:System.NotSupportedException">The <paramref name="value" /> is null or is not a <see cref="T:System.Windows.Media.Matrix" />, or the <paramref name="destinationType" /> is not a valid conversion type.</exception>
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of <see cref="T:System.Windows.Media.MatrixConverter" />.</summary>
	public MatrixConverter()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
