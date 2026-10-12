using System.ComponentModel;
using System.Globalization;

namespace System.Windows;

/// <summary> Converts instances of other types to and from a <see cref="T:System.Windows.Vector" />. </summary>
public sealed class VectorConverter : TypeConverter
{
	/// <summary>Indicates whether an object can be converted from a given type to an instance of a <see cref="T:System.Windows.Vector" />.</summary>
	/// <param name="context">Describes the context information of a type.</param>
	/// <param name="sourceType">The source <see cref="T:System.Type" /> that is being queried for conversion support.</param>
	/// <returns>
	///     <see langword="true" /> if objects of the specified type can be converted to a <see cref="T:System.Windows.Vector" />; otherwise, <see langword="false" />.</returns>
	public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Determines whether instances of <see cref="T:System.Windows.Vector" /> can be converted to the specified type. </summary>
	/// <param name="context">Describes the context information of a type.</param>
	/// <param name="destinationType">The desired type this <see cref="T:System.Windows.Vector" /> is being evaluated for conversion.</param>
	/// <returns>
	///     <see langword="true" /> if instances of <see cref="T:System.Windows.Vector" /> can be converted to <paramref name="destinationType" />; otherwise, <see langword="false" />.</returns>
	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Converts the specified object to a <see cref="T:System.Windows.Vector" />. </summary>
	/// <param name="context">Describes the context information of a type.</param>
	/// <param name="culture">Describes the <see cref="T:System.Globalization.CultureInfo" /> of the type being converted.</param>
	/// <param name="value">The object being converted.</param>
	/// <returns>The <see cref="T:System.Windows.Vector" /> created from converting <paramref name="value" />.</returns>
	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary> Converts the specified <see cref="T:System.Windows.Vector" /> to the specified type. </summary>
	/// <param name="context">Describes the context information of a type.</param>
	/// <param name="culture">Describes the <see cref="T:System.Globalization.CultureInfo" /> of the type being converted.</param>
	/// <param name="value">The <see cref="T:System.Windows.Vector" /> to convert.</param>
	/// <param name="destinationType">The type to convert this <see cref="T:System.Windows.Vector" /> to.</param>
	/// <returns>The object created from converting this <see cref="T:System.Windows.Vector" />.</returns>
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Vector" /> structure. </summary>
	public VectorConverter()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
