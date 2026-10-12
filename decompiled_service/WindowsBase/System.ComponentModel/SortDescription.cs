namespace System.ComponentModel;

/// <summary>Defines the direction and the property name to be used as the criteria for sorting a collection.</summary>
public struct SortDescription
{
	private string _propertyName;

	private ListSortDirection _direction;

	private bool _sealed;

	/// <summary>Gets or sets the property name being used as the sorting criteria.</summary>
	/// <returns>The default value is null.</returns>
	public string PropertyName
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

	/// <summary>Gets or sets a value that indicates whether to sort in ascending or descending order.</summary>
	/// <returns>A <see cref="T:System.ComponentModel.ListSortDirection" /> value to indicate whether to sort in ascending or descending order.</returns>
	public ListSortDirection Direction
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

	/// <summary>Gets a value that indicates whether this object is in an immutable state.</summary>
	/// <returns>
	///     <see langword="true" /> if this object is in use; otherwise, <see langword="false" />.</returns>
	public bool IsSealed
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.SortDescription" /> structure.</summary>
	/// <param name="propertyName">The name of the property to sort the list by.</param>
	/// <param name="direction">The sort order.</param>
	/// <exception cref="T:System.ArgumentNullException">The <paramref name="propertyName" /> parameter cannot be null.</exception>
	/// <exception cref="T:System.ArgumentException">The <paramref name="propertyName" /> parameter cannot be empty</exception>
	/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The <paramref name="direction" /> parameter does not specify a valid value.</exception>
	public SortDescription(string propertyName, ListSortDirection direction)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Compares the specified instance and the current instance of <see cref="T:System.ComponentModel.SortDescription" /> for value equality.</summary>
	/// <param name="obj">The <see cref="T:System.ComponentModel.SortDescription" /> instance to compare.</param>
	/// <returns>
	///     <see langword="true" /> if <paramref name="obj" /> and this instance of <see cref="T:System.ComponentModel.SortDescription" /> have the same values.</returns>
	public override bool Equals(object obj)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Compares two <see cref="T:System.ComponentModel.SortDescription" /> objects for value equality.</summary>
	/// <param name="sd1">The first instance to compare.</param>
	/// <param name="sd2">The second instance to compare.</param>
	/// <returns>
	///     <see langword="true" /> if the two objects are equal; otherwise, <see langword="false" />.</returns>
	public static bool operator ==(SortDescription sd1, SortDescription sd2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Compares two <see cref="T:System.ComponentModel.SortDescription" /> objects for value inequality.</summary>
	/// <param name="sd1">The first instance to compare.</param>
	/// <param name="sd2">The second instance to compare.</param>
	/// <returns>
	///     <see langword="true" /> if the values are not equal; otherwise, <see langword="false" />.</returns>
	public static bool operator !=(SortDescription sd1, SortDescription sd2)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns the hash code for this instance of <see cref="T:System.ComponentModel.SortDescription" />.</summary>
	/// <returns>The hash code for this instance of <see cref="T:System.ComponentModel.SortDescription" />.</returns>
	public override int GetHashCode()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void Seal()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
