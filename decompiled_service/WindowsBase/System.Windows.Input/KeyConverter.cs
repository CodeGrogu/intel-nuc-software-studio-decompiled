using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Input;

/// <summary>Converts a <see cref="T:System.Windows.Input.Key" /> object to and from other types.</summary>
public class KeyConverter : TypeConverter
{
	/// <summary>Determines whether an object of the specified type can be converted to an instance of <see cref="T:System.Windows.Input.Key" />, using the specified context. </summary>
	/// <param name="context">A format context that provides information about the environment from which this converter is being invoked.</param>
	/// <param name="sourceType">The type being evaluated for conversion.</param>
	/// <returns>
	///     <see langword="true" /> if <paramref name="sourceType" /> is of type <see cref="T:System.String" />; otherwise, <see langword="false" />.</returns>
	public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Determines whether an instance of <see cref="T:System.Windows.Input.Key" /> can be converted to the specified type, using the specified context.</summary>
	/// <param name="context">A format context that provides information about the environment from which this converter is being invoked.</param>
	/// <param name="destinationType">The type being evaluated for conversion.</param>
	/// <returns>
	///     <see langword="true" /> if <paramref name="destinationType" /> is of type <see cref="T:System.String" />; otherwise, <see langword="false" />.</returns>
	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Attempts to convert the specified object to a <see cref="T:System.Windows.Input.Key" />, using the specified context.</summary>
	/// <param name="context">A format context that provides information about the environment from which this converter is being invoked.</param>
	/// <param name="culture">Culture specific information.</param>
	/// <param name="source">The object to convert.</param>
	/// <returns>The converted object.</returns>
	/// <exception cref="T:System.NotSupportedException">
	///         <paramref name="source" /> cannot be converted.</exception>
	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object source)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Attempts to convert a <see cref="T:System.Windows.Input.Key" /> to the specified type, using the specified context.</summary>
	/// <param name="context">A format context that provides information about the environment from which this converter is being invoked.</param>
	/// <param name="culture">Culture specific information.</param>
	/// <param name="value">The object to convert.</param>
	/// <param name="destinationType">The type to convert the object to.</param>
	/// <returns>The converted object.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="destinationType" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.NotSupportedException">
	///         <paramref name="value" /> cannot be converted to <paramref name="destinationType" />.</exception>
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Input.Key" /> class. </summary>
	public KeyConverter()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
