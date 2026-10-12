using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Markup;

namespace System.Windows;

/// <summary>Implements base WPF support for the <see cref="T:System.Windows.Markup.INameScope" /> methods that store or retrieve name-object mappings into a particular XAML namescope. Adds attached property support to make it simpler to get or set XAML namescope names dynamically at the element level..</summary>
[TypeForwardedFrom("PresentationFramework, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
public class NameScope : INameScopeDictionary, INameScope, IDictionary<string, object>, ICollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable
{
	/// <summary>Identifies the <see cref="P:System.Windows.NameScope.NameScope" />  attached property. </summary>
	/// <returns>The identifier for the <see cref="P:System.Windows.NameScope.NameScope" /> attached property.</returns>
	public static readonly DependencyProperty NameScopeProperty;

	/// <summary>Returns the number of items in the collection of mapped names in this <see cref="T:System.Windows.NameScope" />.</summary>
	/// <returns>The number of items in the collection.</returns>
	public int Count
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets a value indicating whether the collection is read-only.</summary>
	/// <returns>Always returns <see langword="false" />.</returns>
	public bool IsReadOnly
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets or sets the item with the specified key. </summary>
	/// <param name="key">The string name for the XAML name mapping to get or set.</param>
	/// <returns>The value of the object mapped by the XAML name provided as <paramref name="key" />.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="key" /> is provided as <see langword="null" />.-or-
	///         <paramref name="value" /> is provided as <see langword="null" /> for a set operation.</exception>
	public object this[string key]
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

	/// <summary>Gets a collection of the keys in the <see cref="T:System.Windows.NameScope" /> dictionary.</summary>
	/// <returns>A collection of the keys in the <see cref="T:System.Windows.NameScope" /> dictionary.</returns>
	public ICollection<string> Keys
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets a collection of the values in the <see cref="T:System.Windows.NameScope" /> dictionary.</summary>
	/// <returns>A collection of the values in the <see cref="T:System.Windows.NameScope" /> dictionary.</returns>
	public ICollection<object> Values
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Registers a new name-object pair into the current XAML namescope.</summary>
	/// <param name="name">The name to use for mapping the given object.</param>
	/// <param name="scopedElement">The object to be mapped to the provided name.</param>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="name" /> or <paramref name="scopedElement" /> was provided as <see langword="null" />.</exception>
	/// <exception cref="T:System.ArgumentException">
	///         <paramref name="name" /> was provided as empty string- or -
	///         <paramref name="name" /> provided was rejected by the parser, because it contained characters that are invalid for a XAML name- or -
	///         <paramref name="name" /> provided would result in a duplicate name registration.</exception>
	public void RegisterName(string name, object scopedElement)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Removes a name-object mapping from the XAML namescope.</summary>
	/// <param name="name">The name of the mapping to remove.</param>
	/// <exception cref="T:System.ArgumentException">
	///         <paramref name="name" /> was provided as empty string.- or -
	///         <paramref name="name" /> provided had not been registered.</exception>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="name" /> is <see langword="null" />.</exception>
	public void UnregisterName(string name)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns the corresponding object in the XAML namescope maintained by this <see cref="T:System.Windows.NameScope" />, based on a provided name string.</summary>
	/// <param name="name">Name portion of an existing mapping to retrieve the object portion for.</param>
	/// <returns>The requested object that is mapped with <paramref name="name" />. Can return <see langword="null" /> if <paramref name="name" /> was provided as <see langword="null" /> or empty string, or if no matching object was found.</returns>
	public object FindName(string name)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static INameScope NameScopeFromObject(object obj)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Provides the attached property set accessor for the <see cref="P:System.Windows.NameScope.NameScope" /> attached property.</summary>
	/// <param name="dependencyObject">Object to change XAML namescope for.</param>
	/// <param name="value">The new XAML namescope, using an interface cast.</param>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="dependencyObject" /> is <see langword="null" />.</exception>
	public static void SetNameScope(DependencyObject dependencyObject, INameScope value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Provides the attached property get accessor for the <see cref="P:System.Windows.NameScope.NameScope" /> attached property.</summary>
	/// <param name="dependencyObject">The object to get the XAML namescope from.</param>
	/// <returns>A XAML namescope, as an <see cref="T:System.Windows.Markup.INameScope" /> instance.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="dependencyObject" /> is <see langword="null" />.</exception>
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public static INameScope GetNameScope(DependencyObject dependencyObject)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	private IEnumerator<KeyValuePair<string, object>> GetEnumerator()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns an enumerator that iterates through a collection.</summary>
	/// <returns>An enumerator that iterates through a collection.</returns>
	IEnumerator IEnumerable.GetEnumerator()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Removes all items from the collection.</summary>
	public void Clear()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Copies the elements of the collection to an array, starting at a particular array index.</summary>
	/// <param name="array">The one-dimensional array that is the destination of the elements copied from the collection The array must have zero-based indexing.</param>
	/// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
	public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Removes the specific object from the collection.</summary>
	/// <param name="item">The object to remove from the collection, specified as a <see cref="T:System.Collections.Generic.KeyValuePair`2" />  (key is <see cref="T:System.String" />, value is <see cref="T:System.Object" />).</param>
	/// <returns>
	///     <see langword="true" /> if item was successfully removed from the collection, otherwise <see langword="false" />. Also returns <see langword="false" /> if the item was not found in the collection.</returns>
	public bool Remove(KeyValuePair<string, object> item)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Adds an item to the collection.</summary>
	/// <param name="item">A <see cref="T:System.Collections.Generic.KeyValuePair`2" />  (key is <see cref="T:System.String" />, value is <see cref="T:System.Object" />) that represents the name mapping to add to the XAML namescope.</param>
	/// <exception cref="T:System.ArgumentException">Either or both components of <paramref name="item" /> are <see langword="null" />.</exception>
	public void Add(KeyValuePair<string, object> item)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Determines whether the collection contains a specified item. </summary>
	/// <param name="item">The item to find in the collection, specified as a <see cref="T:System.Collections.Generic.KeyValuePair`2" />  (key is <see cref="T:System.String" />, value is <see cref="T:System.Object" />).</param>
	/// <returns>
	///     <see langword="true" /> if the specified <see cref="T:System.Collections.Generic.KeyValuePair`2" /> identifies an existing mapping in this <see cref="T:System.Windows.NameScope" /> . <see langword="false" /> if the specified <see cref="T:System.Collections.Generic.KeyValuePair`2" /> does not exist in the current <see cref="T:System.Windows.NameScope" />.</returns>
	/// <exception cref="T:System.ArgumentException">
	///         <paramref name="key" /> is <see langword="null" />.</exception>
	public bool Contains(KeyValuePair<string, object> item)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Adds an item to the collection.</summary>
	/// <param name="key">The string key, which is the name of the XAML namescope mapping to add.</param>
	/// <param name="value">The object value, which is the object reference of the XAML namescope mapping to add.</param>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="key" /> or <paramref name="value" /> is <see langword="null" />.</exception>
	public void Add(string key, object value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns whether a provided name already exists in this <see cref="T:System.Windows.NameScope" />.</summary>
	/// <param name="key">The string key to find.</param>
	/// <returns>
	///     <see langword="true" /> if the specified <paramref name="key" /> identifies a name for an existing mapping in this <see cref="T:System.Windows.NameScope" />. <see langword="false" /> if the specified <paramref name="key" /> does not exist in the current <see cref="T:System.Windows.NameScope" />.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="key" /> is <see langword="null" />.</exception>
	public bool ContainsKey(string key)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Removes a mapping for a specified name from the collection.</summary>
	/// <param name="key">The string key, which is the name of the XAML namescope mapping to remove.</param>
	/// <returns>
	///     <see langword="true" /> if item was successfully removed from the collection, otherwise <see langword="false" />. Also returns <see langword="false" /> if the item was not found in the collection.</returns>
	public bool Remove(string key)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Gets the value associated with the specified key.</summary>
	/// <param name="key">The key of the value to get.</param>
	/// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, a null object. This parameter is passed uninitialized.</param>
	/// <returns>true if the <see cref="T:System.Windows.NameScope" /> contains a mapping for the name provided as <paramref name="key" />. Otherwise, false.</returns>
	public bool TryGetValue(string key, out object value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.NameScope" /> class.</summary>
	public NameScope()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static NameScope()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
