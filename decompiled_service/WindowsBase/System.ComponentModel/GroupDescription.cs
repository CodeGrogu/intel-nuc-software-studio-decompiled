using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace System.ComponentModel;

/// <summary>Provides an abstract base class for types that describe how to divide the items in a collection into groups.</summary>
public abstract class GroupDescription : INotifyPropertyChanged
{
	/// <summary>Gets the collection of names that are used to initialize a group with a set of subgroups with the given names.</summary>
	/// <returns>The collection of names that are used to initialize a group with a set of subgroups with the given names.</returns>
	public ObservableCollection<object> GroupNames
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Occurs when a property value changes.</summary>
	event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
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

	/// <summary>Occurs when a property value changes.</summary>
	protected virtual event PropertyChangedEventHandler PropertyChanged
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

	/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.GroupDescription" /> class.</summary>
	protected GroupDescription()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Raises the <see cref="E:System.ComponentModel.GroupDescription.PropertyChanged" /> event.</summary>
	/// <param name="e">Arguments of the event being raised.</param>
	protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns whether serialization processes should serialize the effective value of the <see cref="P:System.ComponentModel.GroupDescription.GroupNames" /> property on instances of this class.</summary>
	/// <returns>Returns <see langword="true" /> if the <see cref="P:System.ComponentModel.GroupDescription.GroupNames" /> property value should be serialized; otherwise, false.</returns>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeGroupNames()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns the group name(s) for the given item.</summary>
	/// <param name="item">The item to return group names for.</param>
	/// <param name="level">The level of grouping.</param>
	/// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to supply to the converter.</param>
	/// <returns>The group name(s) for the given item.</returns>
	public abstract object GroupNameFromItem(object item, int level, CultureInfo culture);

	/// <summary>Returns a value that indicates whether the group name and the item name match such that the item belongs to the group.</summary>
	/// <param name="groupName">The name of the group to check.</param>
	/// <param name="itemName">The name of the item to check.</param>
	/// <returns>
	///     <see langword="true" /> if the names match and the item belongs to the group; otherwise, <see langword="false" />.</returns>
	public virtual bool NamesMatch(object groupName, object itemName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
