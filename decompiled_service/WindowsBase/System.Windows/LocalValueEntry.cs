namespace System.Windows;

/// <summary>Represents a property identifier and the property value for a locally set dependency property.</summary>
public struct LocalValueEntry
{
	internal DependencyProperty _dp;

	internal object _value;

	/// <summary>Gets the identifier for the locally set dependency property that is represented by this entry. </summary>
	/// <returns>The identifier for the locally set dependency property.</returns>
	public DependencyProperty Property
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the value of the locally set dependency property. </summary>
	/// <returns>The value of the locally set dependency property as an object. </returns>
	public object Value
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Returns the hash code for this <see cref="T:System.Windows.LocalValueEntry" />.</summary>
	/// <returns>A signed 32-bit integer value.</returns>
	public override int GetHashCode()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Determines whether two <see cref="T:System.Windows.LocalValueEntry" /> instances are equal.</summary>
	/// <param name="obj">The <see cref="T:System.Windows.LocalValueEntry" /> to compare with the current <see cref="T:System.Windows.LocalValueEntry" />.</param>
	/// <returns>This <see cref="M:System.Windows.LocalValueEntry.op_Equality(System.Windows.LocalValueEntry,System.Windows.LocalValueEntry)" /> implementation compares the values of the <see cref="P:System.Windows.LocalValueEntry.Property" />, and potentially compares the values of <see cref="P:System.Windows.LocalValueEntry.Value" />. The <see cref="P:System.Windows.LocalValueEntry.Property" /> component of a <see cref="T:System.Windows.LocalValueEntry" /> is a value type, so will always be a bitwise comparison. For the <see cref="P:System.Windows.LocalValueEntry.Value" /> component, this implementation employs a bitwise comparison if it is a value type. For locally set properties that have reference types, the behavior is deferred to that type's equality determination mechanisms, because it just uses the == operator on the two values internally. By default, this would be a reference equality of the values and thus the equality of the entire <see cref="T:System.Windows.LocalValueEntry" /> would become a reference equality. </returns>
	public override bool Equals(object obj)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Compares the specified <see cref="T:System.Windows.LocalValueEntry" /> instances to determine whether they are the same.</summary>
	/// <param name="obj1">The first instance to compare.</param>
	/// <param name="obj2">The second instance to compare.</param>
	/// <returns>
	///     <see langword="true" /> if the <paramref name="obj1" /> <see cref="T:System.Windows.LocalValueEntry" /> is equal to the <paramref name="obj2" /> <see cref="T:System.Windows.LocalValueEntry" />; otherwise, <see langword="false" />.</returns>
	public static bool operator ==(LocalValueEntry obj1, LocalValueEntry obj2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Compares the specified <see cref="T:System.Windows.LocalValueEnumerator" /> instances to determine whether they are different.</summary>
	/// <param name="obj1">The first instance to compare.</param>
	/// <param name="obj2">The second instance to compare.</param>
	/// <returns>This implementation compares the values of the <see cref="P:System.Windows.LocalValueEntry.Property" /> and <see cref="P:System.Windows.LocalValueEntry.Value" /> components of a <see cref="T:System.Windows.LocalValueEntry" />. The <see cref="P:System.Windows.LocalValueEntry.Property" /> component of a <see cref="T:System.Windows.LocalValueEntry" /> is always a value type, so this comparison will always be a bitwise comparison. For the <see cref="P:System.Windows.LocalValueEntry.Value" /> component, this implementation employs a bitwise comparison if it is a value type. For locally set properties that have reference types, the behavior is deferred to that type's equality determination mechanisms, because it uses the <see langword="==" /> operator on the two values internally. By default, this is a reference equality of the values. </returns>
	public static bool operator !=(LocalValueEntry obj1, LocalValueEntry obj2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal LocalValueEntry(DependencyProperty dp, object value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
