using System.ComponentModel;
using System.Globalization;

namespace System.Windows;

/// <summary>Converts instances of other types to and from instances of the <see cref="T:System.Windows.Size" /> class. </summary>
public sealed class SizeConverter : TypeConverter
{
	/// <summary>Determines whether a class can be converted from a given type to an instance of <see cref="T:System.Windows.Size" />. </summary>
	/// <param name="context">Provides contextual information about a component.</param>
	/// <param name="sourceType">Identifies the data type to evaluate for conversion.</param>
	/// <returns>
	///     <see langword="true" /> if the <paramref name="sourceType" /> can be converted to an instance of <see cref="T:System.Windows.Size" />; otherwise, <see langword="false" />.</returns>
	public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Determines whether an instance of <see cref="T:System.Windows.Size" /> can be converted to a different type. </summary>
	/// <param name="context">Provides contextual information about a component.</param>
	/// <param name="destinationType">Identifies the data type to evaluate for conversion.</param>
	/// <returns>
	///     <see langword="true" /> if this instance of <see cref="T:System.Windows.Size" /> can be converted to the <paramref name="destinationType" />; otherwise, <see langword="false" />.</returns>
	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Attempts to convert a specified object to an instance of <see cref="T:System.Windows.Size" />.</summary>
	/// <param name="context">Provides contextual information about a component.</param>
	/// <param name="culture">Culture-specific information that should be respected during conversion.</param>
	/// <param name="value">The source object that is being converted.</param>
	/// <returns>The instance of <see cref="T:System.Windows.Size" /> that is created from the converted <paramref name="source" />.</returns>
	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Attempts to convert an instance of <see cref="T:System.Windows.Size" /> to a specified type. </summary>
	/// <param name="context">Provides contextual information about a component.</param>
	/// <param name="culture">Culture-specific information that should be respected during conversion.</param>
	/// <param name="value">The instance of <see cref="T:System.Windows.Size" /> to convert.</param>
	/// <param name="destinationType">The type that this instance of <see cref="T:System.Windows.Size" /> is converted to.</param>
	/// <returns>The object that is created from the converted instance of <see cref="T:System.Windows.Size" />.</returns>
	/// <exception cref="T:System.NotSupportedException">
	///         <paramref name="value" /> is <see langword="null" /> or is not an instance of <see cref="T:System.Windows.Size" />, or if the <paramref name="destinationType" /> is not one of the valid destination types.</exception>
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.SizeConverter" /> class. </summary>
	public SizeConverter()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
