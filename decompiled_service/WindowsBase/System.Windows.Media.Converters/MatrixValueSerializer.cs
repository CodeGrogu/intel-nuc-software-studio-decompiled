using System.Windows.Markup;

namespace System.Windows.Media.Converters;

/// <summary>Converts instances of <see cref="T:System.String" /> to and from instances of <see cref="T:System.Windows.Media.Matrix" />.</summary>
public class MatrixValueSerializer : ValueSerializer
{
	/// <summary>Determines if conversion from a given <see cref="T:System.String" /> to an instance of <see cref="T:System.Windows.Media.Matrix" /> is possible.</summary>
	/// <param name="value">String to evaluate for conversion.</param>
	/// <param name="context">Context information used for conversion.</param>
	/// <returns>
	///     <see langword="true" /> if the value can be converted; otherwise, <see langword="false" />.</returns>
	public override bool CanConvertFromString(string value, IValueSerializerContext context)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Determines if an instance of <see cref="T:System.Windows.Media.Matrix" /> can be converted to a <see cref="T:System.String" />.</summary>
	/// <param name="value">Instance of <see cref="T:System.Windows.Media.Matrix" /> to evaluate for conversion.</param>
	/// <param name="context">Context information used for conversion.</param>
	/// <returns>
	///     <see langword="true" /> if <paramref name="value" /> can be converted into a <see cref="T:System.String" />; otherwise, <see langword="false" />.</returns>
	/// <exception cref="T:System.ArgumentException">Occurs when <paramref name="value" /> is not a <see cref="T:System.Windows.Media.Int32Collection" />.</exception>
	public override bool CanConvertToString(object value, IValueSerializerContext context)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Converts a <see cref="T:System.String" /> into a <see cref="T:System.Windows.Media.Matrix" />.</summary>
	/// <param name="value">
	///       <see cref="T:System.String" /> value to convert into a <see cref="T:System.Windows.Media.Matrix" />.</param>
	/// <param name="context">Context information used for conversion.</param>
	/// <returns>A new instance of <see cref="T:System.Windows.Media.Matrix" /> based on the supplied <paramref name="value" />.</returns>
	public override object ConvertFromString(string value, IValueSerializerContext context)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Converts an instance of <see cref="T:System.Windows.Media.Matrix" /> to a <see cref="T:System.String" />.</summary>
	/// <param name="value">Instance of <see cref="T:System.Windows.Media.Matrix" /> to evaluate for conversion.</param>
	/// <param name="context">Context information used for conversion.</param>
	/// <returns>A <see cref="T:System.String" /> representation of the supplied <see cref="T:System.Windows.Media.Matrix" /> object.</returns>
	public override string ConvertToString(object value, IValueSerializerContext context)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Media.Converters.MatrixValueSerializer" /> class.</summary>
	public MatrixValueSerializer()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
