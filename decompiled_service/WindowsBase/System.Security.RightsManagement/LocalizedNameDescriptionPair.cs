namespace System.Security.RightsManagement;

/// <summary>Represents an immutable (read-only) pair of "Name" and "Description" strings.  </summary>
[SecurityCritical(SecurityCriticalScope.Everything)]
public class LocalizedNameDescriptionPair
{
	/// <summary>Gets the locale name.</summary>
	/// <returns>The locale name.</returns>
	public string Name
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the locale description.</summary>
	/// <returns>The locale description.</returns>
	public string Description
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Security.RightsManagement.LocalizedNameDescriptionPair" /> class.</summary>
	/// <param name="name">The value for the locale <see cref="P:System.Security.RightsManagement.LocalizedNameDescriptionPair.Name" /> property.</param>
	/// <param name="description">The value for the locale <see cref="P:System.Security.RightsManagement.LocalizedNameDescriptionPair.Description" /> property.</param>
	/// <exception cref="T:System.ArgumentNullException">Either the <paramref name="name" /> parameter or <paramref name="description" /> parameter is <see langword="null" />.</exception>
	public LocalizedNameDescriptionPair(string name, string description)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Indicates whether the <see cref="P:System.Security.RightsManagement.LocalizedNameDescriptionPair.Name" /> and <see cref="P:System.Security.RightsManagement.LocalizedNameDescriptionPair.Description" /> properties of a given object match those of this <see cref="T:System.Security.RightsManagement.LocalizedNameDescriptionPair" />.</summary>
	/// <param name="obj">The object to compare the <see cref="P:System.Security.RightsManagement.LocalizedNameDescriptionPair.Name" /> and <see cref="P:System.Security.RightsManagement.LocalizedNameDescriptionPair.Description" /> properties of.</param>
	/// <returns>
	///     <see langword="true" /> if the <see cref="P:System.Security.RightsManagement.LocalizedNameDescriptionPair.Name" /> and <see cref="P:System.Security.RightsManagement.LocalizedNameDescriptionPair.Description" /> properties of the given object match those of this instance; otherwise, <see langword="false" />.</returns>
	public override bool Equals(object obj)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Gets a computed hash code based on the <see cref="P:System.Security.RightsManagement.LocalizedNameDescriptionPair.Name" /> and <see cref="P:System.Security.RightsManagement.LocalizedNameDescriptionPair.Description" /> properties.</summary>
	/// <returns>A computed hash code based on the <see cref="P:System.Security.RightsManagement.LocalizedNameDescriptionPair.Name" /> and <see cref="P:System.Security.RightsManagement.LocalizedNameDescriptionPair.Description" /> properties of this <see cref="T:System.Security.RightsManagement.LocalizedNameDescriptionPair" />.</returns>
	public override int GetHashCode()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
