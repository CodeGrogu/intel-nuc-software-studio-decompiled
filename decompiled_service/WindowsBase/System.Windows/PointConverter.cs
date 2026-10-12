using System.ComponentModel;
using System.Globalization;

namespace System.Windows;

/// <summary>Converts instances of other types to and from a <see cref="T:System.Windows.Point" />. </summary>
public sealed class PointConverter : TypeConverter
{
	/// <summary>Determines whether an object can be converted from a given type to an instance of a <see cref="T:System.Windows.Point" />.  </summary>
	/// <param name="context">Describes the context information of a type.</param>
	/// <param name="sourceType">The type of the source that is being evaluated for conversion.</param>
	/// <returns>
	///     <see langword="true" /> if the type can be converted to a <see cref="T:System.Windows.Point" />; otherwise, <see langword="false" />.</returns>
	public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Determines whether an instance of a <see cref="T:System.Windows.Point" /> can be converted to a different type. </summary>
	/// <param name="context">Describes the context information of a type.</param>
	/// <param name="destinationType">The desired type this <see cref="T:System.Windows.Point" /> is being evaluated for conversion.</param>
	/// <returns>
	///     <see langword="true" /> if this <see cref="T:System.Windows.Point" /> can be converted to <paramref name="destinationType" />; otherwise, <see langword="false" />.</returns>
	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Attempts to convert the specified object to a <see cref="T:System.Windows.Point" />. </summary>
	/// <param name="context">Provides contextual information required for conversion.</param>
	/// <param name="culture">Cultural information to respect during conversion.</param>
	/// <param name="value">The object being converted.</param>
	/// <returns>The <see cref="T:System.Windows.Point" /> created from converting <paramref name="value" />.</returns>
	/// <exception cref="T:System.NotSupportedException">Thrown if the specified object is NULL or is a type that cannot be converted to a <see cref="T:System.Windows.Point" />.</exception>
	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Attempts to convert a <see cref="T:System.Windows.Point" /> to a specified type. </summary>
	/// <param name="context">Provides contextual information required for conversion.</param>
	/// <param name="culture">Cultural information to respect during conversion.</param>
	/// <param name="value">The <see cref="T:System.Windows.Point" /> to convert.</param>
	/// <param name="destinationType">The type to convert this <see cref="T:System.Windows.Point" /> to.</param>
	/// <returns>The object created from converting this <see cref="T:System.Windows.Point" />.</returns>
	/// <exception cref="T:System.NotSupportedException">Thrown if <paramref name="value" /> is <see langword="null" /> or is not a <see cref="T:System.Windows.Point" />, or if the <paramref name="destinationType" /> is not one of the valid types for conversion.</exception>
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.PointConverter" /> class. </summary>
	public PointConverter()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
