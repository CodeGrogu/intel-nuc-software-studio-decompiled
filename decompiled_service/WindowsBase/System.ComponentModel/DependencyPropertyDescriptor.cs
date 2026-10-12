using System.Windows;

namespace System.ComponentModel;

/// <summary>Provides an extension of <see cref="T:System.ComponentModel.PropertyDescriptor" /> that accounts for the additional property characteristics of a dependency property.</summary>
public sealed class DependencyPropertyDescriptor : PropertyDescriptor
{
	/// <summary>Returns the dependency property identifier.</summary>
	/// <returns>The dependency property identifier.</returns>
	public DependencyProperty DependencyProperty
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets a value that indicates whether the property is registered as an attached property and is being used through an attached usage.</summary>
	/// <returns>
	///     <see langword="true" /> if the property is an attached property; otherwise, <see langword="false" />.</returns>
	public bool IsAttached
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the metadata associated with the dependency property.</summary>
	/// <returns>The dependency property metadata.</returns>
	public PropertyMetadata Metadata
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the type of the component this property is bound to.</summary>
	/// <returns>A <see cref="T:System.Type" /> that represents the type of component this property is bound to. When <see cref="M:System.ComponentModel.DependencyPropertyDescriptor.GetValue(System.Object)" /> or <see cref="M:System.ComponentModel.DependencyPropertyDescriptor.SetValue(System.Object,System.Object)" /> are invoked, the object specified might be an instance of this type.</returns>
	public override Type ComponentType
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets a value indicating whether this property is read-only.</summary>
	/// <returns>
	///     <see langword="true" /> if the property is read-only; otherwise, <see langword="false" />.</returns>
	public override bool IsReadOnly
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the represented <see cref="T:System.Type" /> of the dependency property.</summary>
	/// <returns>The <see cref="T:System.Type" /> of the dependency property.</returns>
	public override Type PropertyType
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the collection of attributes for this member.</summary>
	/// <returns>The <see cref="T:System.ComponentModel.AttributeCollection" /> collection of attributes.</returns>
	public override AttributeCollection Attributes
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the name of the category that the member belongs to, as specified in the <see cref="T:System.ComponentModel.CategoryAttribute" />.</summary>
	/// <returns>The name of the category to which the member belongs. If there is no <see cref="T:System.ComponentModel.CategoryAttribute" />, the category name is set to the default category, <see langword="Misc" />.</returns>
	public override string Category
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the description of the member, as specified in the <see cref="T:System.ComponentModel.DescriptionAttribute" />.</summary>
	/// <returns>The description of the member. If there is no <see cref="T:System.ComponentModel.DescriptionAttribute" />, the property value is set to the default, which is an empty string ("").</returns>
	public override string Description
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets whether this member should be set only at design time, as specified in the <see cref="T:System.ComponentModel.DesignOnlyAttribute" />.</summary>
	/// <returns>
	///     <see langword="true" /> if this member should be set only at design time; <see langword="false" /> if the member can be set during run time. If there is no <see cref="T:System.ComponentModel.DesignOnlyAttribute" />, the return value is the default, which is <see langword="false" />.</returns>
	public override bool DesignTimeOnly
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the name that can be displayed in a window, such as a Properties window.</summary>
	/// <returns>The name to display for the property.</returns>
	public override string DisplayName
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the type converter for this property.</summary>
	/// <returns>A <see cref="T:System.ComponentModel.TypeConverter" /> that is used to convert the <see cref="T:System.Type" /> of this property.</returns>
	public override TypeConverter Converter
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets a value that indicates the value of the <see cref="T:System.ComponentModel.BrowsableAttribute" /> on the property.</summary>
	/// <returns>
	///     <see langword="true" /> if the <see cref="T:System.ComponentModel.BrowsableAttribute" /> was specified on the property; otherwise, <see langword="false" />.</returns>
	public override bool IsBrowsable
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets a value indicating whether this property should be localized, as specified in the <see cref="T:System.ComponentModel.LocalizableAttribute" />.</summary>
	/// <returns>
	///     <see langword="true" /> if the member is marked with the <see cref="T:System.ComponentModel.LocalizableAttribute" /> constructor of the value true; otherwise, <see langword="false" />.</returns>
	public override bool IsLocalizable
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Indicates whether value change notifications for this property may originate from outside the property descriptor, such as from the component itself, or whether notifications will only originate from direct calls made to <see cref="M:System.ComponentModel.DependencyPropertyDescriptor.SetValue(System.Object,System.Object)" />. </summary>
	/// <returns>
	///     <see langword="true" /> if notifications for this property may originate from outside the property descriptor, such as from the component itself. <see langword="false" /> if notifications will only originate from direct calls made to <see cref="M:System.ComponentModel.DependencyPropertyDescriptor.SetValue(System.Object,System.Object)" />.</returns>
	public override bool SupportsChangeEvents
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets or sets a callback that designers use to modify the effective value of a dependency property before the dependency property value is stored in the dependency property engine.</summary>
	/// <returns>A callback that designers use to modify the effective value of a dependency property before the dependency property value is stored in the dependency property engine.</returns>
	public CoerceValueCallback DesignerCoerceValueCallback
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

	/// <summary>Returns a <see cref="T:System.ComponentModel.DependencyPropertyDescriptor" /> for a provided <see cref="T:System.ComponentModel.PropertyDescriptor" />.</summary>
	/// <param name="property">The <see cref="T:System.ComponentModel.PropertyDescriptor" /> to check.</param>
	/// <returns>If the property described by <paramref name="property" /> is a dependency property, returns a valid <see cref="T:System.ComponentModel.DependencyPropertyDescriptor" />. Otherwise, returns a <see langword="null" /><see cref="T:System.ComponentModel.DependencyPropertyDescriptor" />.</returns>
	public static DependencyPropertyDescriptor FromProperty(PropertyDescriptor property)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static DependencyPropertyDescriptor FromProperty(DependencyProperty dependencyProperty, Type ownerType, Type targetType, bool ignorePropertyType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns a <see cref="T:System.ComponentModel.DependencyPropertyDescriptor" /> for a provided dependency property and target type.</summary>
	/// <param name="dependencyProperty">The identifier for a dependency property.</param>
	/// <param name="targetType">The type of the object where the property is set.</param>
	/// <returns>A <see cref="T:System.ComponentModel.DependencyPropertyDescriptor" /> for the provided dependency property.</returns>
	public static DependencyPropertyDescriptor FromProperty(DependencyProperty dependencyProperty, Type targetType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns a <see cref="T:System.ComponentModel.DependencyPropertyDescriptor" /> for a provided property name.</summary>
	/// <param name="name">The registered name of a dependency property or an attached property.</param>
	/// <param name="ownerType">The <see cref="T:System.Type" /> of the object that owns the property definition.</param>
	/// <param name="targetType">The <see cref="T:System.Type" /> of the object you want to set the property for.</param>
	/// <returns>The requested <see cref="T:System.ComponentModel.DependencyPropertyDescriptor" />.</returns>
	public static DependencyPropertyDescriptor FromName(string name, Type ownerType, Type targetType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns a <see cref="T:System.ComponentModel.DependencyPropertyDescriptor" /> for a provided property name.</summary>
	/// <param name="name">The registered name of a dependency property or an attached property.</param>
	/// <param name="ownerType">The <see cref="T:System.Type" /> of the object that owns the property definition.</param>
	/// <param name="targetType">The <see cref="T:System.Type" /> of the object you want to set the property for.</param>
	/// <param name="ignorePropertyType">Specifies to ignore the property type.</param>
	/// <returns>The requested <see cref="T:System.ComponentModel.DependencyPropertyDescriptor" />.</returns>
	public static DependencyPropertyDescriptor FromName(string name, Type ownerType, Type targetType, bool ignorePropertyType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Compares two <see cref="T:System.ComponentModel.DependencyPropertyDescriptor" /> instances for equality.</summary>
	/// <param name="obj">The <see cref="T:System.ComponentModel.DependencyPropertyDescriptor" />  to compare with the current instance. </param>
	/// <returns>
	///     <see langword="true" /> if the values are equivalent; otherwise, <see langword="false" />.</returns>
	public override bool Equals(object obj)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns the hash code for this <see cref="T:System.ComponentModel.DependencyPropertyDescriptor" />.</summary>
	/// <returns>A 32-bit signed integer hash code. </returns>
	public override int GetHashCode()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Converts the value of this instance to its equivalent string representation.</summary>
	/// <returns>Returns the <see cref="P:System.ComponentModel.MemberDescriptor.Name" /> value.</returns>
	public override string ToString()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns whether resetting an object changes its value.</summary>
	/// <param name="component">The component to test for reset capability. </param>
	/// <returns>
	///     <see langword="true" /> if resetting the component changes its value; otherwise, <see langword="false" />.</returns>
	public override bool CanResetValue(object component)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Resturns the current value of the property on a component.</summary>
	/// <param name="component">The component instance.</param>
	/// <returns>The requested value.</returns>
	public override object GetValue(object component)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Resets the value for this property of the component to the default value.</summary>
	/// <param name="component">The component with the property value that is to be reset to the default value. </param>
	public override void ResetValue(object component)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Sets the value of the component to a different value.</summary>
	/// <param name="component">The component with the property value that is to be set. </param>
	/// <param name="value">The new value.</param>
	public override void SetValue(object component, object value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Indicates whether the value of this property needs to be persisted by serialization processes.</summary>
	/// <param name="component">The component with the property to be examined for persistence.</param>
	/// <returns>
	///     <see langword="true" /> if the property should be persisted; otherwise, <see langword="false" />.</returns>
	public override bool ShouldSerializeValue(object component)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Enables other objects to be notified when this property changes. </summary>
	/// <param name="component">The component to add the handler for.</param>
	/// <param name="handler">The delegate to add as a listener.</param>
	public override void AddValueChanged(object component, EventHandler handler)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Enables other objects to be notified when this property changes.</summary>
	/// <param name="component">The component to add the handler for. </param>
	/// <param name="handler">The delegate to add as a listener. </param>
	public override void RemoveValueChanged(object component, EventHandler handler)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns a <see cref="T:System.ComponentModel.PropertyDescriptorCollection" />.</summary>
	/// <param name="instance">A component to get the properties for.</param>
	/// <param name="filter">An array of type <see cref="T:System.Attribute" /> to use as a filter. </param>
	/// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> with the properties that match the specified attributes for the specified component.</returns>
	public override PropertyDescriptorCollection GetChildProperties(object instance, Attribute[] filter)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Gets an editor of the specified type.</summary>
	/// <param name="editorBaseType">The base type of editor, which is used to differentiate between multiple editors that a property supports. </param>
	/// <returns>An instance of the requested editor type, or <see langword="null" /> if an editor cannot be found.</returns>
	public override object GetEditor(Type editorBaseType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static void ClearCache()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static DependencyPropertyDescriptor()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	private DependencyPropertyDescriptor()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
