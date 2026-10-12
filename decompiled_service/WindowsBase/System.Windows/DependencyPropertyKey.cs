namespace System.Windows;

/// <summary>Provides a dependency property identifier for limited write access to a read-only dependency property.</summary>
public sealed class DependencyPropertyKey
{
	/// <summary>Gets the dependency property identifier associated with this specialized read-only dependency property identifier. </summary>
	/// <returns>The relevant dependency property identifier.</returns>
	public DependencyProperty DependencyProperty
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal DependencyPropertyKey(DependencyProperty dp)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Overrides the metadata of a read-only dependency property that is represented by this dependency property identifier.</summary>
	/// <param name="forType">The type on which this dependency property exists and metadata should be overridden.</param>
	/// <param name="typeMetadata">Metadata supplied for this type.</param>
	/// <exception cref="T:System.InvalidOperationException">Attempted metadata override on a read-write dependency property (cannot be done using this signature).</exception>
	/// <exception cref="T:System.ArgumentException">Metadata was already established for the property as it exists on the provided type.</exception>
	public void OverrideMetadata(Type forType, PropertyMetadata typeMetadata)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void SetDependencyProperty(DependencyProperty dp)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
