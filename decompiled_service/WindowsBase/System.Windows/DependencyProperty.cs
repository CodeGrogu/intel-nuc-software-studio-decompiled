using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Markup;
using MS.Internal.WindowsBase;
using MS.Utility;

namespace System.Windows;

/// <summary>Represents a property that can be set through methods such as, styling, data binding, animation, and inheritance.</summary>
[TypeConverter("System.Windows.Markup.DependencyPropertyConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
[ValueSerializer(typeof(DependencyPropertyValueSerializer))]
public sealed class DependencyProperty
{
	[CompilerGenerated]
	private sealed class _003Cget_RegisteredProperties_003Ed__91 : IEnumerable<object>, IEnumerable, IEnumerator<object>, IDisposable, IEnumerator
	{
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
			}
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		private bool MoveNext()
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		[DebuggerHidden]
		IEnumerator<object> IEnumerable<object>.GetEnumerator()
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		private _003Cget_RegisteredProperties_003Ed__91()
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Specifies a static value that is used by the WPF property system rather than <see langword="null" /> to indicate that the property exists, but does not have its value set by the property system.</summary>
	/// <returns>An unset value. This is effectively the result of a call to the <see cref="T:System.Object" /> constructor.</returns>
	public static readonly object UnsetValue;

	internal InsertionSortMap _metadataMap;

	internal static ItemStructList<DependencyProperty> RegisteredPropertyList;

	internal static object Synchronized;

	/// <summary>Gets the name of the dependency property. </summary>
	/// <returns>The name of the property.</returns>
	public string Name
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the type that the dependency property uses for its value. </summary>
	/// <returns>The <see cref="T:System.Type" /> of the property value.</returns>
	public Type PropertyType
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the type of the object that registered the dependency property with the property system, or added itself as owner of the property. </summary>
	/// <returns>The type of the object that registered the property or added itself as owner of the property.</returns>
	public Type OwnerType
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the default metadata of the dependency property. </summary>
	/// <returns>The default metadata of the dependency property.</returns>
	public PropertyMetadata DefaultMetadata
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the value validation callback for the dependency property. </summary>
	/// <returns>The value validation callback for this dependency property, as provided for the <paramref name="validateValueCallback" /> parameter in the original dependency property registration.</returns>
	public ValidateValueCallback ValidateValueCallback
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets an internally generated value that uniquely identifies the dependency property.</summary>
	/// <returns>A unique numeric identifier.</returns>
	public int GlobalIndex
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool IsObjectType
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool IsValueType
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool IsFreezableType
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool IsStringType
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool IsPotentiallyInherited
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool IsDefaultValueChanged
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool IsPotentiallyUsingDefaultValueFactory
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets a value that indicates whether the dependency property identified by this <see cref="T:System.Windows.DependencyProperty" /> instance is a read-only dependency property.</summary>
	/// <returns>
	///     <see langword="true" /> if the dependency property is read-only; otherwise, <see langword="false" />.</returns>
	public bool ReadOnly
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal DependencyPropertyKey DependencyPropertyKey
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal CoerceValueCallback DesignerCoerceValueCallback
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

	internal static int RegisteredPropertyCount
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal static IEnumerable RegisteredProperties
	{
		[IteratorStateMachine(typeof(_003Cget_RegisteredProperties_003Ed__91))]
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Registers a dependency property with the specified property name, property type, and owner type. </summary>
	/// <param name="name">The name of the dependency property to register. The name must be unique within the registration namespace of the owner type.</param>
	/// <param name="propertyType">The type of the property.</param>
	/// <param name="ownerType">The owner type that is registering the dependency property.</param>
	/// <returns>A dependency property identifier that should be used to set the value of a <see langword="public static readonly" /> field in your class. That identifier is then used to reference the dependency property later, for operations such as setting its value programmatically or obtaining metadata.</returns>
	public static DependencyProperty Register(string name, Type propertyType, Type ownerType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Registers a dependency property with the specified property name, property type, owner type, and property metadata. </summary>
	/// <param name="name">The name of the dependency property to register.</param>
	/// <param name="propertyType">The type of the property.</param>
	/// <param name="ownerType">The owner type that is registering the dependency property.</param>
	/// <param name="typeMetadata">Property metadata for the dependency property.</param>
	/// <returns>A dependency property identifier that should be used to set the value of a <see langword="public static readonly" /> field in your class. That identifier is then used to reference the dependency property later, for operations such as setting its value programmatically or obtaining metadata.</returns>
	public static DependencyProperty Register(string name, Type propertyType, Type ownerType, PropertyMetadata typeMetadata)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Registers a dependency property with the specified property name, property type, owner type, property metadata, and a value validation callback for the property. </summary>
	/// <param name="name">The name of the dependency property to register.</param>
	/// <param name="propertyType">The type of the property.</param>
	/// <param name="ownerType">The owner type that is registering the dependency property.</param>
	/// <param name="typeMetadata">Property metadata for the dependency property.</param>
	/// <param name="validateValueCallback">A reference to a callback that should perform any custom validation of the dependency property value beyond typical type validation.</param>
	/// <returns>A dependency property identifier that should be used to set the value of a <see langword="public static readonly" /> field in your class. That identifier is then used to reference the dependency property later, for operations such as setting its value programmatically or obtaining metadata.</returns>
	public static DependencyProperty Register(string name, Type propertyType, Type ownerType, PropertyMetadata typeMetadata, ValidateValueCallback validateValueCallback)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary> Registers a read-only dependency property, with the specified property type, owner type, and property metadata. </summary>
	/// <param name="name">The name of the dependency property to register.</param>
	/// <param name="propertyType">The type of the property.</param>
	/// <param name="ownerType">The owner type that is registering the dependency property.</param>
	/// <param name="typeMetadata">Property metadata for the dependency property.</param>
	/// <returns>A dependency property key that should be used to set the value of a static read-only field in your class, which is then used to reference the dependency property.</returns>
	public static DependencyPropertyKey RegisterReadOnly(string name, Type propertyType, Type ownerType, PropertyMetadata typeMetadata)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Registers a read-only dependency property, with the specified property type, owner type, property metadata, and a validation callback. </summary>
	/// <param name="name">The name of the dependency property to register.</param>
	/// <param name="propertyType">The type of the property.</param>
	/// <param name="ownerType">The owner type that is registering the dependency property.</param>
	/// <param name="typeMetadata">Property metadata for the dependency property.</param>
	/// <param name="validateValueCallback">A reference to a user-created callback that should perform any custom validation of the dependency property value beyond typical type validation.</param>
	/// <returns>A dependency property key that should be used to set the value of a static read-only field in your class, which is then used to reference the dependency property later.</returns>
	public static DependencyPropertyKey RegisterReadOnly(string name, Type propertyType, Type ownerType, PropertyMetadata typeMetadata, ValidateValueCallback validateValueCallback)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Registers a read-only attached property, with the specified property type, owner type, and property metadata. </summary>
	/// <param name="name">The name of the dependency property to register.</param>
	/// <param name="propertyType">The type of the property.</param>
	/// <param name="ownerType">The owner type that is registering the dependency property.</param>
	/// <param name="defaultMetadata">Property metadata for the dependency property.</param>
	/// <returns>A dependency property key that should be used to set the value of a static read-only field in your class, which is then used to reference the dependency property later.</returns>
	public static DependencyPropertyKey RegisterAttachedReadOnly(string name, Type propertyType, Type ownerType, PropertyMetadata defaultMetadata)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Registers a read-only attached property, with the specified property type, owner type, property metadata, and a validation callback. </summary>
	/// <param name="name">The name of the dependency property to register.</param>
	/// <param name="propertyType">The type of the property.</param>
	/// <param name="ownerType">The owner type that is registering the dependency property.</param>
	/// <param name="defaultMetadata">Property metadata for the dependency property.</param>
	/// <param name="validateValueCallback">A reference to a user-created callback that should perform any custom validation of the dependency property value beyond typical type validation.</param>
	/// <returns>A dependency property key that should be used to set the value of a static read-only field in your class, which is then used to reference the dependency property.</returns>
	public static DependencyPropertyKey RegisterAttachedReadOnly(string name, Type propertyType, Type ownerType, PropertyMetadata defaultMetadata, ValidateValueCallback validateValueCallback)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Registers an attached property with the specified property name, property type, and owner type. </summary>
	/// <param name="name">The name of the dependency property to register.</param>
	/// <param name="propertyType">The type of the property.</param>
	/// <param name="ownerType">The owner type that is registering the dependency property.</param>
	/// <returns>A dependency property identifier that should be used to set the value of a <see langword="public static readonly" /> field in your class. That identifier is then used to reference the dependency property later, for operations such as setting its value programmatically or obtaining metadata.</returns>
	public static DependencyProperty RegisterAttached(string name, Type propertyType, Type ownerType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Registers an attached property with the specified property name, property type, owner type, and property metadata. </summary>
	/// <param name="name">The name of the dependency property to register.</param>
	/// <param name="propertyType">The type of the property.</param>
	/// <param name="ownerType">The owner type that is registering the dependency property.</param>
	/// <param name="defaultMetadata">Property metadata for the dependency property. This can include the default value as well as other characteristics.</param>
	/// <returns>A dependency property identifier that should be used to set the value of a <see langword="public static readonly" /> field in your class. That identifier is then used to reference the dependency property later, for operations such as setting its value programmatically or obtaining metadata.</returns>
	public static DependencyProperty RegisterAttached(string name, Type propertyType, Type ownerType, PropertyMetadata defaultMetadata)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Registers an attached property with the specified property type, owner type, property metadata, and value validation callback for the property. </summary>
	/// <param name="name">The name of the dependency property to register.</param>
	/// <param name="propertyType">The type of the property.</param>
	/// <param name="ownerType">The owner type that is registering the dependency property.</param>
	/// <param name="defaultMetadata">Property metadata for the dependency property. This can include the default value as well as other characteristics.</param>
	/// <param name="validateValueCallback">A reference to a callback that should perform any custom validation of the dependency property value beyond typical type validation.</param>
	/// <returns>A dependency property identifier that should be used to set the value of a <see langword="public static readonly" /> field in your class. That identifier is then used to reference the dependency property later, for operations such as setting its value programmatically or obtaining metadata.</returns>
	public static DependencyProperty RegisterAttached(string name, Type propertyType, Type ownerType, PropertyMetadata defaultMetadata, ValidateValueCallback validateValueCallback)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void ValidateFactoryDefaultValue(object defaultValue)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Specifies alternate metadata for this dependency property when it is present on instances of a specified type, overriding the metadata that existed for the dependency property as it was inherited from base types.</summary>
	/// <param name="forType">The type where this dependency property is inherited and where the provided alternate metadata will be applied.</param>
	/// <param name="typeMetadata">The metadata to apply to the dependency property on the overriding type.</param>
	/// <exception cref="T:System.InvalidOperationException">An attempt was made to override metadata on a read-only dependency property (that operation cannot be done using this signature).</exception>
	/// <exception cref="T:System.ArgumentException">Metadata was already established for the dependency property as it exists on the provided type.</exception>
	public void OverrideMetadata(Type forType, PropertyMetadata typeMetadata)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Supplies alternate metadata for a read-only dependency property when it is present on instances of a specified type, overriding the metadata that was provided in the initial dependency property registration. You must pass the <see cref="T:System.Windows.DependencyPropertyKey" /> for the read-only dependency property to avoid raising an exception.</summary>
	/// <param name="forType">The type where this dependency property is inherited and where the provided alternate metadata will be applied.</param>
	/// <param name="typeMetadata">The metadata to apply to the dependency property on the overriding type.</param>
	/// <param name="key">The access key for a read-only dependency property. </param>
	public void OverrideMetadata(Type forType, PropertyMetadata typeMetadata, DependencyPropertyKey key)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[MS.Internal.WindowsBase.FriendAccessAllowed]
	internal object GetDefaultValue(DependencyObjectType dependencyObjectType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[MS.Internal.WindowsBase.FriendAccessAllowed]
	internal object GetDefaultValue(Type forType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns the metadata for this dependency property as it exists on a specified existing type. </summary>
	/// <param name="forType">The specific type from which to retrieve the dependency property metadata.</param>
	/// <returns>A property metadata object.</returns>
	public PropertyMetadata GetMetadata(Type forType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns the metadata for this dependency property as it exists on the specified object instance. </summary>
	/// <param name="dependencyObject">A dependency object that is checked for type, to determine which type-specific version of the dependency property the metadata should come from.</param>
	/// <returns>A property metadata object.</returns>
	public PropertyMetadata GetMetadata(DependencyObject dependencyObject)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary> Returns the metadata for this dependency property as it exists on a specified type. </summary>
	/// <param name="dependencyObjectType">A specific object that records the dependency object type from which the dependency property metadata is desired.</param>
	/// <returns>A property metadata object.</returns>
	public PropertyMetadata GetMetadata(DependencyObjectType dependencyObjectType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Adds another type as an owner of a dependency property that has already been registered.</summary>
	/// <param name="ownerType">The type to add as an owner of this dependency property.</param>
	/// <returns>A reference to the original <see cref="T:System.Windows.DependencyProperty" /> identifier that identifies the dependency property. This identifier should be exposed by the adding class as a <see langword="public static readonly" /> field.</returns>
	public DependencyProperty AddOwner(Type ownerType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Adds another type as an owner of a dependency property that has already been registered, providing dependency property metadata for the dependency property as it will exist on the provided owner type. </summary>
	/// <param name="ownerType">The type to add as owner of this dependency property.</param>
	/// <param name="typeMetadata">The metadata that qualifies the dependency property as it exists on the provided type.</param>
	/// <returns>A reference to the original <see cref="T:System.Windows.DependencyProperty" /> identifier that identifies the dependency property. This identifier should be exposed by the adding class as a <see langword="public static readonly" /> field.</returns>
	public DependencyProperty AddOwner(Type ownerType, PropertyMetadata typeMetadata)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns a hash code for this <see cref="T:System.Windows.DependencyProperty" />.</summary>
	/// <returns>The hash code for this <see cref="T:System.Windows.DependencyProperty" />.</returns>
	public override int GetHashCode()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Determines whether a specified value is acceptable for this dependency property's type, as checked against the property type provided in the original dependency property registration. </summary>
	/// <param name="value">The value to check.</param>
	/// <returns>
	///     <see langword="true" /> if the specified value is the registered property type or an acceptable derived type; otherwise, <see langword="false" />.</returns>
	public bool IsValidType(object value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Determines whether the provided value is accepted for the type of property through basic type checking, and also potentially if it is within the allowed range of values for that type. </summary>
	/// <param name="value">The value to check.</param>
	/// <returns>
	///     <see langword="true" /> if the value is acceptable and is of the correct type or a derived type; otherwise, <see langword="false" />.</returns>
	public bool IsValidValue(object value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void VerifyReadOnlyKey(DependencyPropertyKey candidateKey)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal bool IsValidValueInternal(object value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[MS.Internal.WindowsBase.FriendAccessAllowed]
	internal static DependencyProperty FromName(string name, Type ownerType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary> Returns the string representation of the dependency property. </summary>
	/// <returns>The string representation of the dependency property.</returns>
	public override string ToString()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static bool IsValidType(object value, Type propertyType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static int GetUniqueGlobalIndex(Type ownerType, string name)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static DependencyProperty()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	private DependencyProperty()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
