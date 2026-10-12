namespace System.Windows;

/// <summary>Specifies that an attached property is browsable only for elements that derive from a specified type.</summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class AttachedPropertyBrowsableForTypeAttribute : AttachedPropertyBrowsableAttribute
{
	/// <summary>Gets the base type that scopes the use of the attached property where this .NET Framework attribute applies.</summary>
	/// <returns>The requested <see cref="T:System.Type" />.</returns>
	public Type TargetType
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets a unique type identifier for this <see cref="T:System.Windows.AttachedPropertyBrowsableForTypeAttribute" /> .NET Framework attribute.</summary>
	/// <returns>An object that is a unique identifier for the <see cref="T:System.Windows.AttachedPropertyBrowsableForTypeAttribute" />.</returns>
	public override object TypeId
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal override bool UnionResults
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.AttachedPropertyBrowsableForTypeAttribute" /> class, using the provided <paramref name="targetType" />.</summary>
	/// <param name="targetType">The intended type that scopes the use of the attached property where this .NET Framework attribute applies.</param>
	public AttachedPropertyBrowsableForTypeAttribute(Type targetType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Determines whether the current <see cref="T:System.Windows.AttachedPropertyBrowsableForTypeAttribute" /> .NET Framework attribute is equal to a specified object.</summary>
	/// <param name="obj">The <see cref="T:System.Windows.AttachedPropertyBrowsableForTypeAttribute" /> to compare to the current <see cref="T:System.Windows.AttachedPropertyBrowsableForTypeAttribute" />.</param>
	/// <returns>
	///     <see langword="true" /> if the specified <see cref="T:System.Windows.AttachedPropertyBrowsableForTypeAttribute" /> is equal to the current <see cref="T:System.Windows.AttachedPropertyBrowsableForTypeAttribute" />; otherwise, <see langword="false" />.</returns>
	public override bool Equals(object obj)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns the hash code for this <see cref="T:System.Windows.AttachedPropertyBrowsableForTypeAttribute" /> .NET Framework attribute.</summary>
	/// <returns>An unsigned 32-bit integer value.</returns>
	public override int GetHashCode()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal override bool IsBrowsable(DependencyObject d, DependencyProperty dp)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
