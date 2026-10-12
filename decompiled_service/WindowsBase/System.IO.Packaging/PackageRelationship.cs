namespace System.IO.Packaging;

/// <summary>Represents an association between a source <see cref="T:System.IO.Packaging.Package" /> or <see cref="T:System.IO.Packaging.PackagePart" />, and a target object which can be a <see cref="T:System.IO.Packaging.PackagePart" /> or external resource.</summary>
public class PackageRelationship
{
	/// <summary>Gets the URI of the package or part that owns the relationship.</summary>
	/// <returns>The URI of the <see cref="T:System.IO.Packaging.Package" /> or <see cref="T:System.IO.Packaging.PackagePart" /> that owns the relationship.</returns>
	public Uri SourceUri
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the URI of the target resource of the relationship.</summary>
	/// <returns>The URI of the target resource.</returns>
	public Uri TargetUri
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the qualified type name of the relationship. </summary>
	/// <returns>The qualified type name of the relationship.</returns>
	public string RelationshipType
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets a value that indicates whether the target of the relationship is <see cref="F:System.IO.Packaging.TargetMode.Internal" /> or <see cref="F:System.IO.Packaging.TargetMode.External" /> to the <see cref="T:System.IO.Packaging.Package" />.</summary>
	/// <returns>An enumeration value that indicates whether <see cref="P:System.IO.Packaging.PackageRelationship.TargetUri" /> references a resource <see cref="F:System.IO.Packaging.TargetMode.Internal" /> or <see cref="F:System.IO.Packaging.TargetMode.External" /> to the <see cref="T:System.IO.Packaging.Package" />.</returns>
	public TargetMode TargetMode
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets a string that identifies the relationship.</summary>
	/// <returns>A string that identifies the relationship.</returns>
	public string Id
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the <see cref="T:System.IO.Packaging.Package" /> that contains this relationship.</summary>
	/// <returns>The package that contains this relationship.</returns>
	public Package Package
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal static Uri ContainerRelationshipPartName
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool Saved
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

	internal PackageRelationship(Package package, PackagePart sourcePart, Uri targetUri, TargetMode targetMode, string relationshipType, string id)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static PackageRelationship()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
