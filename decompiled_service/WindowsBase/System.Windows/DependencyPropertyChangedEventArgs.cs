using MS.Internal.WindowsBase;

namespace System.Windows;

/// <summary>Provides data for various property changed events. Typically these events report effective value changes in the value of a read-only dependency property. Another usage is as part of a <see cref="T:System.Windows.PropertyChangedCallback" /> implementation.</summary>
public struct DependencyPropertyChangedEventArgs
{
	private enum PrivateFlags : byte
	{
		IsAValueChange = 1,
		IsASubPropertyChange
	}

	private DependencyProperty _property;

	private PropertyMetadata _metadata;

	private PrivateFlags _flags;

	private EffectiveValueEntry _oldEntry;

	private EffectiveValueEntry _newEntry;

	private OperationType _operationType;

	/// <summary>Gets the identifier for the dependency property where the value change occurred.</summary>
	/// <returns>The identifier field of the dependency property where the value change occurred.</returns>
	public DependencyProperty Property
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	[FriendAccessAllowed]
	internal bool IsAValueChange
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

	[FriendAccessAllowed]
	internal bool IsASubPropertyChange
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

	[FriendAccessAllowed]
	internal PropertyMetadata Metadata
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	[FriendAccessAllowed]
	internal OperationType OperationType
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the value of the property before the change.</summary>
	/// <returns>The property value before the change.</returns>
	public object OldValue
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	[FriendAccessAllowed]
	internal EffectiveValueEntry OldEntry
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	[FriendAccessAllowed]
	internal BaseValueSourceInternal OldValueSource
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	[FriendAccessAllowed]
	internal bool IsOldValueModified
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	[FriendAccessAllowed]
	internal bool IsOldValueDeferred
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the value of the property after the change.</summary>
	/// <returns>The property value after the change.</returns>
	public object NewValue
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	[FriendAccessAllowed]
	internal EffectiveValueEntry NewEntry
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	[FriendAccessAllowed]
	internal BaseValueSourceInternal NewValueSource
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	[FriendAccessAllowed]
	internal bool IsNewValueModified
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	[FriendAccessAllowed]
	internal bool IsNewValueDeferred
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.DependencyPropertyChangedEventArgs" /> class.</summary>
	/// <param name="property">The identifier for the dependency property that changed.</param>
	/// <param name="oldValue">The value of the property before the change reported by the relevant event or state change.</param>
	/// <param name="newValue">The value of the property after the change reported by the relevant event or state change.</param>
	public DependencyPropertyChangedEventArgs(DependencyProperty property, object oldValue, object newValue)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal DependencyPropertyChangedEventArgs(DependencyProperty property, PropertyMetadata metadata, object oldValue, object newValue)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal DependencyPropertyChangedEventArgs(DependencyProperty property, PropertyMetadata metadata, object value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal DependencyPropertyChangedEventArgs(DependencyProperty property, PropertyMetadata metadata, bool isAValueChange, EffectiveValueEntry oldEntry, EffectiveValueEntry newEntry, OperationType operationType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Gets a hash code  for this <see cref="T:System.Windows.DependencyPropertyChangedEventArgs" />. </summary>
	/// <returns>A signed 32-bit integer hash code. </returns>
	public override int GetHashCode()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Determines whether the provided object is equivalent to the current <see cref="T:System.Windows.DependencyPropertyChangedEventArgs" />.</summary>
	/// <param name="obj">The object to compare to the current <see cref="T:System.Windows.DependencyPropertyChangedEventArgs" />.</param>
	/// <returns>
	///     <see langword="true" /> if the provided object is equivalent to the current <see cref="T:System.Windows.DependencyPropertyChangedEventArgs" />; otherwise, <see langword="false" />.</returns>
	public override bool Equals(object obj)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Determines whether the provided <see cref="T:System.Windows.DependencyPropertyChangedEventArgs" /> is equivalent to the current <see cref="T:System.Windows.DependencyPropertyChangedEventArgs" />.</summary>
	/// <param name="args">The <see cref="T:System.Windows.DependencyPropertyChangedEventArgs" /> to compare to the current <see cref="T:System.Windows.DependencyPropertyChangedEventArgs" /></param>
	/// <returns>
	///     <see langword="true" /> if the provided <see cref="T:System.Windows.DependencyPropertyChangedEventArgs" /> is equivalent to the current <see cref="T:System.Windows.DependencyPropertyChangedEventArgs" />; otherwise, <see langword="false" />.</returns>
	public bool Equals(DependencyPropertyChangedEventArgs args)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Determines whether two specified <see cref="T:System.Windows.DependencyPropertyChangedEventArgs" /> objects have the same value.</summary>
	/// <param name="left">The first <see cref="T:System.Windows.DependencyPropertyChangedEventArgs" /> to compare.</param>
	/// <param name="right">The second <see cref="T:System.Windows.DependencyPropertyChangedEventArgs" /> to compare.</param>
	/// <returns>
	///     <see langword="true" /> if the two <see cref="T:System.Windows.DependencyPropertyChangedEventArgs" /> instances are equivalent; otherwise, <see langword="false" />.</returns>
	public static bool operator ==(DependencyPropertyChangedEventArgs left, DependencyPropertyChangedEventArgs right)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Determines whether two specified <see cref="T:System.Windows.DependencyPropertyChangedEventArgs" /> objects are different.</summary>
	/// <param name="left">The first <see cref="T:System.Windows.DependencyPropertyChangedEventArgs" /> to compare.</param>
	/// <param name="right">The second <see cref="T:System.Windows.DependencyPropertyChangedEventArgs" /> to compare.</param>
	/// <returns>
	///     <see langword="true" /> if the two <see cref="T:System.Windows.DependencyPropertyChangedEventArgs" /> instances are different; otherwise, <see langword="false" />.</returns>
	public static bool operator !=(DependencyPropertyChangedEventArgs left, DependencyPropertyChangedEventArgs right)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
