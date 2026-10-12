using MS.Internal.WindowsBase;

namespace System.Windows;

/// <summary>Defines certain behavior aspects of a dependency property as it is applied to a specific type, including conditions it was registered with. </summary>
public class PropertyMetadata
{
	[FriendAccessAllowed]
	internal enum MetadataFlags : uint
	{
		DefaultValueModifiedID = 1u,
		SealedID = 2u,
		Inherited = 0x10u,
		UI_IsAnimationProhibitedID = 0x20u,
		FW_AffectsMeasureID = 0x40u,
		FW_AffectsArrangeID = 0x80u,
		FW_AffectsParentMeasureID = 0x100u,
		FW_AffectsParentArrangeID = 0x200u,
		FW_AffectsRenderID = 0x400u,
		FW_OverridesInheritanceBehaviorID = 0x800u,
		FW_IsNotDataBindableID = 0x1000u,
		FW_BindsTwoWayByDefaultID = 0x2000u,
		FW_ShouldBeJournaledID = 0x4000u,
		FW_SubPropertiesDoNotAffectRenderID = 0x8000u,
		FW_SubPropertiesDoNotAffectRenderModifiedID = 0x10000u,
		FW_InheritsModifiedID = 0x100000u,
		FW_OverridesInheritanceBehaviorModifiedID = 0x200000u,
		FW_ShouldBeJournaledModifiedID = 0x1000000u,
		FW_UpdatesSourceOnLostFocusByDefaultID = 0x2000000u,
		FW_DefaultUpdateSourceTriggerModifiedID = 0x4000000u,
		FW_ReadOnlyID = 0x8000000u,
		FW_DefaultUpdateSourceTriggerEnumBit1 = 0x40000000u,
		FW_DefaultUpdateSourceTriggerEnumBit2 = 0x80000000u
	}

	[FriendAccessAllowed]
	internal MetadataFlags _flags;

	/// <summary> Gets or sets the default value of the dependency property. </summary>
	/// <returns>The default value of the property. The default value on a <see cref="T:System.Windows.PropertyMetadata" /> instance created with the parameterless constructor will be <see cref="F:System.Windows.DependencyProperty.UnsetValue" />; see Remarks.</returns>
	/// <exception cref="T:System.ArgumentException">Cannot be set to the value <see cref="F:System.Windows.DependencyProperty.UnsetValue" /> once created; see Remarks.</exception>
	/// <exception cref="T:System.InvalidOperationException">Cannot set a metadata property once it is applied to a dependency property operation.</exception>
	public object DefaultValue
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

	internal bool UsingDefaultValueFactory
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets or sets a reference to a <see cref="T:System.Windows.PropertyChangedCallback" /> implementation specified in this metadata.</summary>
	/// <returns>A <see cref="T:System.Windows.PropertyChangedCallback" /> implementation reference.</returns>
	/// <exception cref="T:System.InvalidOperationException">Cannot set a metadata property once it is applied to a dependency property operation.</exception>
	public PropertyChangedCallback PropertyChangedCallback
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

	/// <summary>Gets or sets a reference to a <see cref="T:System.Windows.CoerceValueCallback" /> implementation specified in this metadata.</summary>
	/// <returns>A <see cref="T:System.Windows.CoerceValueCallback" /> implementation reference.</returns>
	/// <exception cref="T:System.InvalidOperationException">Cannot set a metadata property once it is applied to a dependency property operation.</exception>
	public CoerceValueCallback CoerceValueCallback
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
	internal virtual GetReadOnlyValueCallback GetReadOnlyValueCallback
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	[FriendAccessAllowed]
	internal FreezeValueCallback FreezeValueCallback
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

	/// <summary>Gets a value that determines whether the metadata has been applied to a property in some way, resulting in the immutable state of that metadata instance. </summary>
	/// <returns>
	///     <see langword="true" /> if the metadata instance is immutable; otherwise, <see langword="false" />.</returns>
	protected bool IsSealed
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool IsDefaultValueModified
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool IsInherited
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

	internal bool Sealed
	{
		[FriendAccessAllowed]
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary> Initializes a new instance of the <see cref="T:System.Windows.PropertyMetadata" /> class. </summary>
	public PropertyMetadata()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary> Initializes a new instance of the <see cref="T:System.Windows.PropertyMetadata" /> class with a specified default value for the dependency property that this metadata will be applied to. </summary>
	/// <param name="defaultValue">The default value to specify for a dependency property, usually provided as a value of some specific type.</param>
	/// <exception cref="T:System.ArgumentException">
	///         <paramref name="defaultValue" /> cannot be set to the value <see cref="F:System.Windows.DependencyProperty.UnsetValue" />; see Remarks.</exception>
	public PropertyMetadata(object defaultValue)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary> Initializes a new instance of the <see cref="T:System.Windows.PropertyMetadata" /> class with the specified <see cref="T:System.Windows.PropertyChangedCallback" /> implementation reference. </summary>
	/// <param name="propertyChangedCallback">Reference to a handler implementation that is to be called by the property system whenever the effective value of the property changes.</param>
	public PropertyMetadata(PropertyChangedCallback propertyChangedCallback)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary> Initializes a new instance of the <see cref="T:System.Windows.PropertyMetadata" /> class with the specified default value and <see cref="T:System.Windows.PropertyChangedCallback" /> implementation reference. </summary>
	/// <param name="defaultValue">The default value of the dependency property, usually provided as a value of some specific type.</param>
	/// <param name="propertyChangedCallback">Reference to a handler implementation that is to be called by the property system whenever the effective value of the property changes.</param>
	/// <exception cref="T:System.ArgumentException">
	///         <paramref name="defaultValue" /> cannot be set to the value <see cref="F:System.Windows.DependencyProperty.UnsetValue" />; see Remarks.</exception>
	public PropertyMetadata(object defaultValue, PropertyChangedCallback propertyChangedCallback)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.PropertyMetadata" /> class with the specified default value and callbacks. </summary>
	/// <param name="defaultValue">The default value of the dependency property, usually provided as a value of some specific type.</param>
	/// <param name="propertyChangedCallback">Reference to a handler implementation that is to be called by the property system whenever the effective value of the property changes.</param>
	/// <param name="coerceValueCallback">Reference to a handler implementation that is to be called whenever the property system calls <see cref="M:System.Windows.DependencyObject.CoerceValue(System.Windows.DependencyProperty)" /> against this property.</param>
	/// <exception cref="T:System.ArgumentException">
	///         <paramref name="defaultValue" /> cannot be set to the value <see cref="F:System.Windows.DependencyProperty.UnsetValue" />; see Remarks.</exception>
	public PropertyMetadata(object defaultValue, PropertyChangedCallback propertyChangedCallback, CoerceValueCallback coerceValueCallback)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal object GetDefaultValue(DependencyObject owner, DependencyProperty property)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void ClearCachedDefaultValue(DependencyObject owner, DependencyProperty property)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static void PromoteAllCachedDefaultValues(DependencyObject owner)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static void RemoveAllCachedDefaultValues(Freezable owner)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal bool DefaultValueWasSet()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal virtual PropertyMetadata CreateInstance()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal PropertyMetadata Copy(DependencyProperty dp)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Merges this metadata with the base metadata. </summary>
	/// <param name="baseMetadata">The base metadata to merge with this instance's values.</param>
	/// <param name="dp">The dependency property to which this metadata is being applied.</param>
	protected virtual void Merge(PropertyMetadata baseMetadata, DependencyProperty dp)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void InvokeMerge(PropertyMetadata baseMetadata, DependencyProperty dp)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Called when this metadata has been applied to a property, which indicates that the metadata is being sealed. </summary>
	/// <param name="dp">The dependency property to which the metadata has been applied.</param>
	/// <param name="targetType">The type associated with this metadata if this is type-specific metadata. If this is default metadata, this value is a null reference.</param>
	protected virtual void OnApply(DependencyProperty dp, Type targetType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void Seal(DependencyProperty dp, Type targetType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal void WriteFlag(MetadataFlags id, bool value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal bool ReadFlag(MetadataFlags id)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static PropertyMetadata()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
