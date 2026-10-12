using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;

namespace System.ComponentModel;

/// <summary>Represents a collection of <see cref="T:System.ComponentModel.SortDescription" /> objects.</summary>
public class SortDescriptionCollection : Collection<SortDescription>, INotifyCollectionChanged
{
	/// <summary>Gets an empty and non-modifiable instance of <see cref="T:System.ComponentModel.SortDescriptionCollection" />. </summary>
	public static readonly SortDescriptionCollection Empty;

	/// <summary>Occurs when an item is added or removed.</summary>
	event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
	{
		add
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		remove
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Occurs when an item is added or removed.</summary>
	protected event NotifyCollectionChangedEventHandler CollectionChanged
	{
		[CompilerGenerated]
		add
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		[CompilerGenerated]
		remove
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Removes all items from the collection.</summary>
	protected override void ClearItems()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Removes the item at the specified index in the collection.</summary>
	/// <param name="index">The zero-based index of the element to remove.</param>
	protected override void RemoveItem(int index)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Inserts an item into the collection at the specified index.</summary>
	/// <param name="index">The zero-based index where the <paramref name="item" /> is inserted.</param>
	/// <param name="item">The object to insert.</param>
	protected override void InsertItem(int index, SortDescription item)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Replaces the element at the specified index.</summary>
	/// <param name="index">The zero-based index of the element to replace.</param>
	/// <param name="item">The new value for the element at the specified index.</param>
	protected override void SetItem(int index, SortDescription item)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.SortDescriptionCollection" /> class.</summary>
	public SortDescriptionCollection()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static SortDescriptionCollection()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
