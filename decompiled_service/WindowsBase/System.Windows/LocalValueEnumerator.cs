using System.Collections;

namespace System.Windows;

/// <summary>Provides enumeration support for the local values of any dependency properties that exist on a <see cref="T:System.Windows.DependencyObject" />.</summary>
public struct LocalValueEnumerator : IEnumerator
{
	private int _index;

	private LocalValueEntry[] _snapshot;

	private int _count;

	/// <summary>Gets the current element in the collection.</summary>
	/// <returns>The current <see cref="T:System.Windows.LocalValueEntry" /> in the collection.</returns>
	public LocalValueEntry Current
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>For a description of this members, see <see cref="P:System.Collections.IEnumerator.Current" />.</summary>
	/// <returns>The current element in the collection.</returns>
	object IEnumerator.Current
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the number of items that are represented in the collection. </summary>
	/// <returns>The number of items in the collection.</returns>
	public int Count
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Returns a hash code for the current <see cref="T:System.Windows.LocalValueEnumerator" />.</summary>
	/// <returns>A 32-bit integer hash code.</returns>
	public override int GetHashCode()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Determines whether the provided <see cref="T:System.Windows.LocalValueEnumerator" /> is equivalent to this <see cref="T:System.Windows.LocalValueEnumerator" />.</summary>
	/// <param name="obj">The <see cref="T:System.Windows.LocalValueEnumerator" /> to compare with the current <see cref="T:System.Windows.LocalValueEnumerator" />.</param>
	/// <returns>
	///     <see langword="true" /> if the specified <see cref="T:System.Windows.LocalValueEnumerator" /> is equal to the current <see cref="T:System.Windows.LocalValueEnumerator" />; otherwise, <see langword="false" />.</returns>
	public override bool Equals(object obj)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Compares whether two specified <see cref="T:System.Windows.LocalValueEnumerator" /> objects are the same.</summary>
	/// <param name="obj1">The first object to compare.</param>
	/// <param name="obj2">The second object to compare.</param>
	/// <returns>
	///     <see langword="true" /> if the <paramref name="obj1" /><see cref="T:System.Windows.LocalValueEnumerator" /> is equal to the <paramref name="obj2" /><see cref="T:System.Windows.LocalValueEnumerator" />; otherwise, <see langword="false" />.</returns>
	public static bool operator ==(LocalValueEnumerator obj1, LocalValueEnumerator obj2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Compares two specified <see cref="T:System.Windows.LocalValueEnumerator" /> objects to determine whether they are not the same.</summary>
	/// <param name="obj1">The first object to compare.</param>
	/// <param name="obj2">The second object to compare.</param>
	/// <returns>
	///     <see langword="true" /> if the instances are not equal; otherwise, <see langword="false" />.</returns>
	public static bool operator !=(LocalValueEnumerator obj1, LocalValueEnumerator obj2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Advances the enumerator to the next element of the collection.</summary>
	/// <returns>
	///     <see langword="true" /> if the enumerator was successfully advanced to the next element; <see langword="false" /> if the enumerator has passed the end of the collection.</returns>
	public bool MoveNext()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
	public void Reset()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal LocalValueEnumerator(LocalValueEntry[] snapshot, int count)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
