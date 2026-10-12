namespace System.Windows;

/// <summary>Specifies that an attached property is only browsable on an element that also has another specific  .NET Framework attribute applied to its class definition.</summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class AttachedPropertyBrowsableWhenAttributePresentAttribute : AttachedPropertyBrowsableAttribute
{
	/// <summary>Gets the type of the  .NET Framework attribute that must also be applied on a class.</summary>
	/// <returns>The  .NET Framework attribute type.</returns>
	public Type AttributeType
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.AttachedPropertyBrowsableWhenAttributePresentAttribute" /> class.</summary>
	/// <param name="attributeType">The <see cref="T:System.Type" /> of the  .NET Framework attribute that must also be applied on a class in order for the attached property to be browsable on the class where <see cref="T:System.Windows.AttachedPropertyBrowsableWhenAttributePresentAttribute" /> is applied..</param>
	public AttachedPropertyBrowsableWhenAttributePresentAttribute(Type attributeType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Determines whether the current <see cref="T:System.Windows.AttachedPropertyBrowsableWhenAttributePresentAttribute" /> .NET Framework attribute is equal to a specified object.</summary>
	/// <param name="obj">The <see cref="T:System.Windows.AttachedPropertyBrowsableWhenAttributePresentAttribute" /> to compare to the current <see cref="T:System.Windows.AttachedPropertyBrowsableWhenAttributePresentAttribute" />.</param>
	/// <returns>
	///     <see langword="true" /> if the specified <see cref="T:System.Windows.AttachedPropertyBrowsableWhenAttributePresentAttribute" /> is equal to the current <see cref="T:System.Windows.AttachedPropertyBrowsableWhenAttributePresentAttribute" />; otherwise, <see langword="false" />.</returns>
	public override bool Equals(object obj)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns the hash code for this <see cref="T:System.Windows.AttachedPropertyBrowsableWhenAttributePresentAttribute" /> .NET Framework attribute.</summary>
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
