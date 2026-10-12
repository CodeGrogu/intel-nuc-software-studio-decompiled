using System.Collections.Generic;
using System.ComponentModel;
using MS.Internal.WindowsBase;

namespace System.Windows.Markup.Primitives;

/// <summary>Abstract class that provides a property description to be used while writing to markup which encapsulates access to properties and their values. </summary>
public abstract class MarkupProperty
{
	/// <summary>When overridden in a derived class, gets a name that is used for diagnostics and error reporting. </summary>
	/// <returns>The identifier property name.</returns>
	public abstract string Name { get; }

	/// <summary>When overridden in a derived class, gets the CLR type of the property.</summary>
	/// <returns>The CLR type.</returns>
	public abstract Type PropertyType { get; }

	internal bool IsCollectionProperty
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>When overridden in a derived class, gets the <see cref="T:System.ComponentModel.PropertyDescriptor" /> for the markup property. </summary>
	/// <returns>The property descriptor. </returns>
	public virtual PropertyDescriptor PropertyDescriptor
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>When overridden in a derived class, gets the <see cref="T:System.Windows.DependencyProperty" /> identifier for the markup property if the property is implemented as a dependency property</summary>
	/// <returns>The dependency property identifier.</returns>
	public virtual DependencyProperty DependencyProperty
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>When overridden in a derived class, determines whether this <see cref="T:System.Windows.Markup.Primitives.MarkupProperty" /> is an attached <see cref="T:System.Windows.DependencyProperty" />. </summary>
	/// <returns>
	///     <see langword="true" /> if the property is an attached <see cref="T:System.Windows.DependencyProperty" />; otherwise, <see langword="false" />.</returns>
	public virtual bool IsAttached
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>When overridden in a derived class, determines whether this <see cref="T:System.Windows.Markup.Primitives.MarkupProperty" /> represents a constructor argument.</summary>
	/// <returns>
	///     <see langword="true" /> if this property represents a constructor argument; otherwise, <see langword="false" />.</returns>
	public virtual bool IsConstructorArgument
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>When overridden in a derived class, determines whether this <see cref="T:System.Windows.Markup.Primitives.MarkupProperty" /> represents text which is passed to a type converter to create an instance of the property or if a constructor should be used.</summary>
	/// <returns>
	///     <see langword="true" />, if this <see cref="T:System.Windows.Markup.Primitives.MarkupProperty" /> represents a string; otherwise, <see langword="false" />.</returns>
	public virtual bool IsValueAsString
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>When overridden in a derived class, determines whether this <see cref="T:System.Windows.Markup.Primitives.MarkupProperty" /> represents direct content of a collection.</summary>
	/// <returns>
	///     <see langword="true" /> if the property represents direct content; otherwise, <see langword="false" />.</returns>
	public virtual bool IsContent
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>When overridden in a derived class, determines whether this <see cref="T:System.Windows.Markup.Primitives.MarkupProperty" /> represents the key used by the <see cref="T:System.Windows.Markup.Primitives.MarkupObject" /> to store the item in a dictionary.</summary>
	/// <returns>
	///     <see langword="true" /> if this property represents a key; otherwise, <see langword="false" />.</returns>
	public virtual bool IsKey
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>When overridden in a derived class, determines whether this <see cref="T:System.Windows.Markup.Primitives.MarkupProperty" /> is a composite property. </summary>
	/// <returns>
	///     <see langword="true" /> is this property is a composite property; otherwise, <see langword="false" />.</returns>
	public virtual bool IsComposite
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>When overridden in a derived class, gets the current value of this <see cref="T:System.Windows.Markup.Primitives.MarkupProperty" />.</summary>
	/// <returns>The current value.</returns>
	public abstract object Value { get; }

	/// <summary>When overridden in a derived class, gets the string value of this <see cref="T:System.Windows.Markup.Primitives.MarkupProperty" />.</summary>
	/// <returns>The string value.</returns>
	public abstract string StringValue { get; }

	/// <summary>When overridden in a derived class, gets the set of types that this <see cref="T:System.Windows.Markup.Primitives.MarkupProperty" /> will reference when it serializes its value as a string.</summary>
	/// <returns>The set of types.</returns>
	public abstract IEnumerable<Type> TypeReferences { get; }

	/// <summary>When overridden in a derived class, gets the items that make up the value of this property. </summary>
	/// <returns>The items that make up the value of this property.</returns>
	public abstract IEnumerable<MarkupObject> Items { get; }

	/// <summary>When overridden in a derived class, gets the attributes associated with this <see cref="T:System.Windows.Markup.Primitives.MarkupProperty" />.</summary>
	/// <returns>The collection of attributes.</returns>
	public abstract AttributeCollection Attributes { get; }

	[FriendAccessAllowed]
	internal MarkupProperty()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal virtual void VerifyOnlySerializableTypes()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
