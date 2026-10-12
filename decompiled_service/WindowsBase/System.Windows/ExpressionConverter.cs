using System.ComponentModel;
using System.Globalization;

namespace System.Windows;

/// <summary>Converts instances of <see cref="T:System.Windows.Expression" />  to and from other types. </summary>
public class ExpressionConverter : TypeConverter
{
	/// <summary>Returns whether this converter can convert from a source object to an <see cref="T:System.Windows.Expression" /> object. </summary>
	/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
	/// <param name="sourceType">A <see cref="T:System.Type" /> that represents the type you wish to convert from.</param>
	/// <returns>Always <see langword="false" />.</returns>
	public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns whether this converter can convert an <see cref="T:System.Windows.Expression" /> object to a specific destination type. </summary>
	/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
	/// <param name="destinationType">A <see cref="T:System.Type" /> that represents the type you wish to convert to.</param>
	/// <returns>Always <see langword="false" />.</returns>
	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Converts the provided value to the <see cref="T:System.Windows.Expression" /> type. </summary>
	/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
	/// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to use as the current culture. </param>
	/// <param name="value">The object to convert.</param>
	/// <returns>Always throws an exception and returns <see langword="null" />.</returns>
	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Converts the provided <see cref="T:System.Windows.Expression" /> object to the specified type.</summary>
	/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
	/// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to use as the current culture. </param>
	/// <param name="value">The object to convert.</param>
	/// <param name="destinationType">A <see cref="T:System.Type" /> that represents the type you wish to convert to.</param>
	/// <returns>Always throws an exception and returns <see langword="null" />.</returns>
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.ExpressionConverter" /> class. </summary>
	public ExpressionConverter()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
