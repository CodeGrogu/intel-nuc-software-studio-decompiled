using System.ComponentModel;
using System.Windows.Converters;
using System.Windows.Markup;

namespace System.Windows;

/// <summary>Implements a structure that is used to describe the <see cref="T:System.Windows.Size" /> of an object. </summary>
[Serializable]
[TypeConverter(typeof(SizeConverter))]
[ValueSerializer(typeof(SizeValueSerializer))]
public struct Size : IFormattable
{
	internal double _width;

	internal double _height;

	/// <summary>Gets a value that represents a static empty <see cref="T:System.Windows.Size" />. </summary>
	/// <returns>An empty instance of <see cref="T:System.Windows.Size" />.</returns>
	public static Size Empty
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets a value that indicates whether this instance of <see cref="T:System.Windows.Size" /> is <see cref="P:System.Windows.Size.Empty" />. </summary>
	/// <returns>
	///     <see langword="true" /> if this instance of size is <see cref="P:System.Windows.Size.Empty" />; otherwise <see langword="false" />.</returns>
	public bool IsEmpty
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets or sets the <see cref="P:System.Windows.Size.Width" /> of this instance of <see cref="T:System.Windows.Size" />. </summary>
	/// <returns>The <see cref="P:System.Windows.Size.Width" /> of this instance of <see cref="T:System.Windows.Size" />. The default value is 0. The value cannot be negative.</returns>
	public double Width
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

	/// <summary>Gets or sets the <see cref="P:System.Windows.Size.Height" /> of this instance of <see cref="T:System.Windows.Size" />. </summary>
	/// <returns>The <see cref="P:System.Windows.Size.Height" /> of this instance of <see cref="T:System.Windows.Size" />. The default is 0. The value cannot be negative.</returns>
	public double Height
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

	/// <summary>Compares two instances of <see cref="T:System.Windows.Size" /> for equality. </summary>
	/// <param name="size1">The first instance of <see cref="T:System.Windows.Size" /> to compare.</param>
	/// <param name="size2">The second instance of <see cref="T:System.Windows.Size" /> to compare.</param>
	/// <returns>true if the two instances of <see cref="T:System.Windows.Size" /> are equal; otherwise <see langword="false" />.</returns>
	public static bool operator ==(Size size1, Size size2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Compares two instances of <see cref="T:System.Windows.Size" /> for inequality. </summary>
	/// <param name="size1">The first instance of <see cref="T:System.Windows.Size" /> to compare.</param>
	/// <param name="size2">The second instance of <see cref="T:System.Windows.Size" /> to compare.</param>
	/// <returns>
	///     <see langword="true" /> if the instances of <see cref="T:System.Windows.Size" /> are not equal; otherwise <see langword="false" />.</returns>
	public static bool operator !=(Size size1, Size size2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Compares two instances of <see cref="T:System.Windows.Size" /> for equality. </summary>
	/// <param name="size1">The first instance of <see cref="T:System.Windows.Size" /> to compare.</param>
	/// <param name="size2">The second instance of <see cref="T:System.Windows.Size" /> to compare.</param>
	/// <returns>
	///     <see langword="true" /> if the instances of <see cref="T:System.Windows.Size" /> are equal; otherwise, <see langword="false" />.</returns>
	public static bool Equals(Size size1, Size size2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Compares an object to an instance of <see cref="T:System.Windows.Size" /> for equality. </summary>
	/// <param name="o">The <see cref="T:System.Object" /> to compare.</param>
	/// <returns>
	///     <see langword="true" /> if the sizes are equal; otherwise, <see langword="false" />.</returns>
	public override bool Equals(object o)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Compares a value to an instance of <see cref="T:System.Windows.Size" /> for equality. </summary>
	/// <param name="value">The size to compare to this current instance of <see cref="T:System.Windows.Size" />.</param>
	/// <returns>
	///     <see langword="true" /> if the instances of <see cref="T:System.Windows.Size" /> are equal; otherwise, <see langword="false" />.</returns>
	public bool Equals(Size value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Gets the hash code for this instance of <see cref="T:System.Windows.Size" />. </summary>
	/// <returns>The hash code for this instance of <see cref="T:System.Windows.Size" />.</returns>
	public override int GetHashCode()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns an instance of <see cref="T:System.Windows.Size" /> from a converted <see cref="T:System.String" />. </summary>
	/// <param name="source">A <see cref="T:System.String" /> value to parse to a <see cref="T:System.Windows.Size" /> value.</param>
	/// <returns>An instance of <see cref="T:System.Windows.Size" />.</returns>
	public static Size Parse(string source)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns a <see cref="T:System.String" /> that represents this <see cref="T:System.Windows.Size" /> object. </summary>
	/// <returns>A <see cref="T:System.String" /> that specifies the width followed by the height.</returns>
	public override string ToString()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns a <see cref="T:System.String" /> that represents this instance of <see cref="T:System.Windows.Size" />. </summary>
	/// <param name="provider">An object that provides a way to control formatting.</param>
	/// <returns>A <see cref="T:System.String" /> that represents this <see cref="T:System.Windows.Size" /> object.</returns>
	public string ToString(IFormatProvider provider)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
	/// <param name="format"> The format to use.</param>
	/// <param name="provider"> The provider to use to format the value.</param>
	/// <returns> The value of the current instance in the specified format.</returns>
	string IFormattable.ToString(string format, IFormatProvider provider)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal string ConvertToString(string format, IFormatProvider provider)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Size" /> structure and assigns it an initial <paramref name="width" /> and <paramref name="height" />.</summary>
	/// <param name="width">The initial width of the instance of <see cref="T:System.Windows.Size" />.</param>
	/// <param name="height">The initial height of the instance of <see cref="T:System.Windows.Size" />.</param>
	public Size(double width, double height)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Explicitly converts an instance of <see cref="T:System.Windows.Size" /> to an instance of <see cref="T:System.Windows.Vector" />. </summary>
	/// <param name="size">The <see cref="T:System.Windows.Size" /> value to be converted.</param>
	/// <returns>A <see cref="T:System.Windows.Vector" /> equal in value to this instance of <see cref="T:System.Windows.Size" />.</returns>
	public static explicit operator Vector(Size size)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Explicitly converts an instance of <see cref="T:System.Windows.Size" /> to an instance of <see cref="T:System.Windows.Point" />. </summary>
	/// <param name="size">The <see cref="T:System.Windows.Size" /> value to be converted.</param>
	/// <returns>A <see cref="T:System.Windows.Point" /> equal in value to this instance of <see cref="T:System.Windows.Size" />.</returns>
	public static explicit operator Point(Size size)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static Size()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
