using System.Windows.Markup;

namespace System.Windows.Converters;

/// <summary>Converts instances of <see cref="T:System.String" /> to and from instances of <see cref="T:System.Windows.Size" />.</summary>
public class SizeValueSerializer : ValueSerializer
{
	/// <summary>Determines whether the specified <see cref="T:System.String" /> can be converted to an instance of <see cref="T:System.Windows.Size" />.</summary>
	/// <param name="value">String to evaluate for conversion.</param>
	/// <param name="context">Context information that is used for conversion.</param>
	/// <returns>Always returns <see langword="true" />.</returns>
	public override bool CanConvertFromString(string value, IValueSerializerContext context)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Determines whether the specified <see cref="T:System.Windows.Size" /> can be converted to a <see cref="T:System.String" />.</summary>
	/// <param name="value">The object to evaluate for conversion.</param>
	/// <param name="context">Context information that is used for conversion.</param>
	/// <returns>
	///     <see langword="true" /> if <paramref name="value" /> can be converted into a <see cref="T:System.String" />; otherwise, <see langword="false" />.</returns>
	public override bool CanConvertToString(object value, IValueSerializerContext context)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Converts a <see cref="T:System.String" /> into a <see cref="T:System.Windows.Size" />.</summary>
	/// <param name="value">The string to convert.</param>
	/// <param name="context">Context information that is used for conversion.</param>
	/// <returns>A new instance of <see cref="T:System.Windows.Size" /> based on the supplied <paramref name="value" />.</returns>
	public override object ConvertFromString(string value, IValueSerializerContext context)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Converts an instance of <see cref="T:System.Windows.Size" /> to a <see cref="T:System.String" />.</summary>
	/// <param name="value">The object to convert into a string.</param>
	/// <param name="context">Context information that is used for conversion.</param>
	/// <returns>A string representation of the specified <see cref="T:System.Windows.Size" />.</returns>
	public override string ConvertToString(object value, IValueSerializerContext context)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Converters.SizeValueSerializer" /> class.</summary>
	public SizeValueSerializer()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
