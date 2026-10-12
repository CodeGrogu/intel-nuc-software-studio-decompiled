using MS.Internal;

namespace System.IO.Packaging;

/// <summary>Provides a base class for parts stored in a <see cref="T:System.IO.Packaging.Package" />.  This class is abstract.</summary>
public abstract class PackagePart
{
	/// <summary>Gets the URI of the part.</summary>
	/// <returns>The URI of the part relative to the package root.</returns>
	/// <exception cref="T:System.InvalidOperationException">The part has been deleted.-or-The <see cref="P:System.IO.Packaging.PackagePart.Package" /> is not open (<see cref="M:System.IO.Packaging.Package.Dispose(System.Boolean)" /> or <see cref="M:System.IO.Packaging.Package.Close" /> has been called).</exception>
	public Uri Uri
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the MIME type of the content stream.</summary>
	/// <returns>The MIME type of the content data stream for the part.</returns>
	/// <exception cref="T:System.InvalidOperationException">The part has been deleted.-or-The <see cref="P:System.IO.Packaging.PackagePart.Package" /> is not open (<see cref="M:System.IO.Packaging.Package.Dispose(System.Boolean)" /> or <see cref="M:System.IO.Packaging.Package.Close" /> has been called).-or-The string returned by the derived class <see cref="M:System.IO.Packaging.PackagePart.GetContentTypeCore" /> method is empty.</exception>
	public string ContentType
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the parent <see cref="T:System.IO.Packaging.Package" /> of the part.</summary>
	/// <returns>The parent package of the part.</returns>
	/// <exception cref="T:System.InvalidOperationException">The part has been deleted.-or-The <see cref="P:System.IO.Packaging.PackagePart.Package" /> is not open (<see cref="M:System.IO.Packaging.Package.Dispose(System.Boolean)" /> or <see cref="M:System.IO.Packaging.Package.Close" /> has been called).</exception>
	public Package Package
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the compression option of the part content stream.</summary>
	/// <returns>The compression option of the part content stream.</returns>
	/// <exception cref="T:System.InvalidOperationException">The part has been deleted.-or-The <see cref="P:System.IO.Packaging.PackagePart.Package" /> is not open (<see cref="M:System.IO.Packaging.Package.Dispose(System.Boolean)" /> or <see cref="M:System.IO.Packaging.Package.Close" /> has been called).</exception>
	public CompressionOption CompressionOption
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool IsRelationshipPart
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool IsDeleted
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

	internal bool IsClosed
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal ContentType ValidatedContentType
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.IO.Packaging.PackagePart" /> class with a specified parent <see cref="P:System.IO.Packaging.PackagePart.Package" /> and part URI.</summary>
	/// <param name="package">The parent <see cref="T:System.IO.Packaging.Package" /> of the part.</param>
	/// <param name="partUri">The URI of the part, relative to the parent <see cref="T:System.IO.Packaging.Package" /> root.</param>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="package" /> or <paramref name="partUri" /> is <see langword="null" />.</exception>
	protected PackagePart(Package package, Uri partUri)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.IO.Packaging.PackagePart" /> class with a specified parent <see cref="P:System.IO.Packaging.PackagePart.Package" />, part URI, and MIME content type.</summary>
	/// <param name="package">The parent <see cref="T:System.IO.Packaging.Package" /> of the part.</param>
	/// <param name="partUri">The URI of the part, relative to the parent <see cref="T:System.IO.Packaging.Package" /> root.</param>
	/// <param name="contentType">The MIME content type of the part data stream.</param>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="package" /> or <paramref name="partUri" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.ArgumentException">
	///         <paramref name="partUri" /> is not a valid <see cref="T:System.IO.Packaging.PackagePart" /> URI.</exception>
	protected PackagePart(Package package, Uri partUri, string contentType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.IO.Packaging.PackagePart" /> class with a specified parent <see cref="P:System.IO.Packaging.PackagePart.Package" />, part URI, MIME content type, and <see cref="T:System.IO.Packaging.CompressionOption" />.</summary>
	/// <param name="package">The parent <see cref="T:System.IO.Packaging.Package" /> of the part.</param>
	/// <param name="partUri">The URI of the part, relative to the parent <see cref="T:System.IO.Packaging.Package" /> root.</param>
	/// <param name="contentType">The MIME content type of the part's data stream.</param>
	/// <param name="compressionOption">The compression option of the part data stream.</param>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="package" /> or <paramref name="partUri" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.ArgumentException">
	///         <paramref name="partUri" /> is not a valid <see cref="T:System.IO.Packaging.PackagePart" /> URI.</exception>
	/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="compressionOption" /> value is not valid.</exception>
	protected PackagePart(Package package, Uri partUri, string contentType, CompressionOption compressionOption)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>When overridden in a derived class, returns the MIME type of the part content.</summary>
	/// <returns>The MIME type of the part content.</returns>
	/// <exception cref="T:System.NotSupportedException">The derived class does not provide an override implementation required for the <see cref="M:System.IO.Packaging.PackagePart.GetContentTypeCore" /> method.</exception>
	protected virtual string GetContentTypeCore()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns the part content data stream.</summary>
	/// <returns>The content data stream for the part.</returns>
	/// <exception cref="T:System.InvalidOperationException">The part has been deleted.-or-The <see cref="P:System.IO.Packaging.PackagePart.Package" /> is not open (<see cref="M:System.IO.Packaging.Package.Dispose(System.Boolean)" /> or <see cref="M:System.IO.Packaging.Package.Close" /> has been called).</exception>
	/// <exception cref="T:System.IO.IOException">The stream object returned by the <see cref="M:System.IO.Packaging.PackagePart.GetStreamCore(System.IO.FileMode,System.IO.FileAccess)" /> method of the derived subclass is <see langword="null" />.</exception>
	public Stream GetStream()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns the content stream opened in a specified I/O <see cref="T:System.IO.FileMode" />.</summary>
	/// <param name="mode">The I/O mode in which to open the content stream.</param>
	/// <returns>The content stream of the part.</returns>
	/// <exception cref="T:System.InvalidOperationException">The part has been deleted.-or-The <see cref="P:System.IO.Packaging.PackagePart.Package" /> is not open (<see cref="M:System.IO.Packaging.Package.Dispose(System.Boolean)" /> or <see cref="M:System.IO.Packaging.Package.Close" /> has been called).</exception>
	/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="mode" /> parameter is not a valid <see cref="T:System.IO.FileMode" /> enumeration value.</exception>
	/// <exception cref="T:System.IO.IOException">The <paramref name="mode" /> parameter is not compatible with the package and part stream.-or-The stream object returned by the <see cref="M:System.IO.Packaging.PackagePart.GetStreamCore(System.IO.FileMode,System.IO.FileAccess)" /> method of the derived subclass is <see langword="null" />.</exception>
	public Stream GetStream(FileMode mode)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns the part content stream opened with a specified <see cref="T:System.IO.FileMode" /> and <see cref="T:System.IO.FileAccess" />.</summary>
	/// <param name="mode">The I/O mode in which to open the content stream.</param>
	/// <param name="access">The access permissions to use in opening the content stream.</param>
	/// <returns>The content stream for the part.</returns>
	/// <exception cref="T:System.InvalidOperationException">The part has been deleted.-or-The <see cref="P:System.IO.Packaging.PackagePart.Package" /> is not open (<see cref="M:System.IO.Packaging.Package.Dispose(System.Boolean)" /> or <see cref="M:System.IO.Packaging.Package.Close" /> has been called).</exception>
	/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="mode" /> parameter is not a valid <see cref="T:System.IO.FileMode" /> enumeration value.-or-The <paramref name="access" /> parameter is not a valid <see cref="T:System.IO.FileAccess" /> enumeration value.</exception>
	/// <exception cref="T:System.IO.IOException">
	///         <paramref name="mode" /> or <paramref name="access" /> is not compatible with the package and part stream.-or-The <paramref name="access" /> parameter is specified as <see cref="F:System.IO.FileAccess.Read" /> but the <paramref name="mode" /> parameter requires write access.  (<see cref="T:System.IO.FileMode" /> values of <see cref="F:System.IO.FileMode.Create" />, <see cref="F:System.IO.FileMode.CreateNew" />, <see cref="F:System.IO.FileMode.Truncate" />, and <see cref="F:System.IO.FileMode.Append" /> require <see cref="F:System.IO.FileAccess.Write" /> or <see cref="F:System.IO.FileAccess.ReadWrite" /> access.)-or-The stream object returned by the <see cref="M:System.IO.Packaging.PackagePart.GetStreamCore(System.IO.FileMode,System.IO.FileAccess)" /> method of the derived subclass is <see langword="null" />.</exception>
	public Stream GetStream(FileMode mode, FileAccess access)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>When overridden in a derived class, returns the part content stream opened with a specified <see cref="T:System.IO.FileMode" /> and <see cref="T:System.IO.FileAccess" />.</summary>
	/// <param name="mode">The I/O mode in which to open the content stream.</param>
	/// <param name="access">The access permissions to use in opening the content stream.</param>
	/// <returns>The content data stream of the part.</returns>
	protected abstract Stream GetStreamCore(FileMode mode, FileAccess access);

	/// <summary>Creates a part-level relationship between this <see cref="T:System.IO.Packaging.PackagePart" /> to a specified target <see cref="T:System.IO.Packaging.PackagePart" /> or external resource.</summary>
	/// <param name="targetUri">The URI of the target part.</param>
	/// <param name="targetMode">One of the enumeration values. For example, <see cref="F:System.IO.Packaging.TargetMode.Internal" /> if the target part is inside the <see cref="T:System.IO.Packaging.Package" />; or <see cref="F:System.IO.Packaging.TargetMode.External" /> if the target is a resource outside the <see cref="T:System.IO.Packaging.Package" />.</param>
	/// <param name="relationshipType">The role of the relationship.</param>
	/// <returns>The part-level relationship between this <see cref="T:System.IO.Packaging.PackagePart" /> to the target <see cref="T:System.IO.Packaging.PackagePart" /> or external resource.</returns>
	/// <exception cref="T:System.InvalidOperationException">The part has been deleted.-or-The <see cref="P:System.IO.Packaging.PackagePart.Package" /> is not open (<see cref="M:System.IO.Packaging.Package.Dispose(System.Boolean)" /> or <see cref="M:System.IO.Packaging.Package.Close" /> has been called).</exception>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="targetUri" /> or <paramref name="relationshipType" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="targetMode" /> parameter is not a valid <see cref="T:System.IO.Packaging.TargetMode" /> enumeration value.</exception>
	/// <exception cref="T:System.ArgumentException">The part identified by the <paramref name="targetUri" /> is a relationship (the target of a relationship cannot be another relationship).-or-
	///         <paramref name="targetMode" /> is specified as <see cref="F:System.IO.Packaging.TargetMode.Internal" /> but <paramref name="targetUri" /> is an absolute external URI.</exception>
	/// <exception cref="T:System.IO.IOException">The package is read-only (a new relationship cannot be added).</exception>
	public PackageRelationship CreateRelationship(Uri targetUri, TargetMode targetMode, string relationshipType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates a part-level relationship between this <see cref="T:System.IO.Packaging.PackagePart" /> to a specified target <see cref="T:System.IO.Packaging.PackagePart" /> or external resource.</summary>
	/// <param name="targetUri">The URI of the target part.</param>
	/// <param name="targetMode">One of the enumeration values. For example, <see cref="F:System.IO.Packaging.TargetMode.Internal" /> if the target part is inside the <see cref="T:System.IO.Packaging.Package" />; or <see cref="F:System.IO.Packaging.TargetMode.External" /> if the target is a resource outside the <see cref="T:System.IO.Packaging.Package" />.</param>
	/// <param name="relationshipType">The role of the relationship.</param>
	/// <param name="id">A unique ID for the relationship.</param>
	/// <returns>The part-level relationship between this <see cref="T:System.IO.Packaging.PackagePart" /> to the target <see cref="T:System.IO.Packaging.PackagePart" /> or external resource.</returns>
	/// <exception cref="T:System.InvalidOperationException">The part has been deleted.-or-The <see cref="P:System.IO.Packaging.PackagePart.Package" /> is not open (<see cref="M:System.IO.Packaging.Package.Dispose(System.Boolean)" /> or <see cref="M:System.IO.Packaging.Package.Close" /> has been called).</exception>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="targetUri" /> or <paramref name="relationshipType" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="targetMode" /> parameter is not a valid <see cref="T:System.IO.Packaging.TargetMode" /> enumeration value.</exception>
	/// <exception cref="T:System.ArgumentException">The part identified by the <paramref name="targetUri" /> is a relationship (the target of a relationship cannot be another relationship).-or-
	///         <paramref name="targetMode" /> is specified as <see cref="F:System.IO.Packaging.TargetMode.Internal" /> but <paramref name="targetUri" /> is an absolute external URI.</exception>
	/// <exception cref="T:System.IO.IOException">The package is read-only (a new relationship cannot be added).</exception>
	/// <exception cref="T:System.Xml.XmlException">
	///         <paramref name="id" /> is not a valid XML identifier.-or-A part with the specified <paramref name="id" /> already exists.</exception>
	public PackageRelationship CreateRelationship(Uri targetUri, TargetMode targetMode, string relationshipType, string id)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Deletes a specified part-level <see cref="T:System.IO.Packaging.PackageRelationship" />.</summary>
	/// <param name="id">The <see cref="P:System.IO.Packaging.PackageRelationship.Id" /> of the relationship to delete.</param>
	/// <exception cref="T:System.InvalidOperationException">The part has been deleted.-or-The <see cref="P:System.IO.Packaging.PackagePart.Package" /> is not open (<see cref="M:System.IO.Packaging.Package.Dispose(System.Boolean)" /> or <see cref="M:System.IO.Packaging.Package.Close" /> has been called).</exception>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="id" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.IO.IOException">The package is read-only (relationships cannot be deleted).</exception>
	/// <exception cref="T:System.Xml.XmlException">
	///         <paramref name="id" /> is not a valid XML identifier.</exception>
	public void DeleteRelationship(string id)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns a collection of all the relationships that are owned by this part.</summary>
	/// <returns>A collection of all the relationships that are owned by the part.</returns>
	/// <exception cref="T:System.InvalidOperationException">The part has been deleted.-or-The <see cref="P:System.IO.Packaging.PackagePart.Package" /> is not open (<see cref="M:System.IO.Packaging.Package.Dispose(System.Boolean)" /> or <see cref="M:System.IO.Packaging.Package.Close" /> has been called).</exception>
	/// <exception cref="T:System.IO.IOException">The package is write-only (relationship information cannot be read).</exception>
	public PackageRelationshipCollection GetRelationships()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns a collection of the relationships that match a specified <see cref="P:System.IO.Packaging.PackageRelationship.RelationshipType" />.</summary>
	/// <param name="relationshipType">The <see cref="P:System.IO.Packaging.PackageRelationship.RelationshipType" /> of the relationships to locate and return in the collection.</param>
	/// <returns>A collection of the relationships that match the specified <paramref name="relationshipType" />.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="relationshipType" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.ArgumentException">
	///         <paramref name="relationshipType" /> is an empty string.</exception>
	/// <exception cref="T:System.InvalidOperationException">The part has been deleted.-or-The <see cref="P:System.IO.Packaging.PackagePart.Package" /> is not open (<see cref="M:System.IO.Packaging.Package.Dispose(System.Boolean)" /> or <see cref="M:System.IO.Packaging.Package.Close" /> has been called).</exception>
	/// <exception cref="T:System.IO.IOException">The package is write-only (relationship information cannot be read).</exception>
	public PackageRelationshipCollection GetRelationshipsByType(string relationshipType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns the relationship that has a specified <see cref="P:System.IO.Packaging.PackageRelationship.Id" />. </summary>
	/// <param name="id">The <see cref="P:System.IO.Packaging.PackageRelationship.Id" /> of the relationship to return.</param>
	/// <returns>The relationship that matches the specified <paramref name="id" />.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="id" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.Xml.XmlException">
	///         <paramref name="id" /> is not a valid XML identifier.</exception>
	/// <exception cref="T:System.ArgumentException">
	///         <paramref name="relationshipType" /> is an empty string.</exception>
	/// <exception cref="T:System.InvalidOperationException">The part has been deleted.-or-The <see cref="P:System.IO.Packaging.PackagePart.Package" /> is not open (<see cref="M:System.IO.Packaging.Package.Dispose(System.Boolean)" /> or <see cref="M:System.IO.Packaging.Package.Close" /> has been called).-or-A relationship with the specified <paramref name="id" /> does not exist in the package.</exception>
	/// <exception cref="T:System.IO.IOException">The package is write-only (relationship information cannot be read).</exception>
	public PackageRelationship GetRelationship(string id)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns a value that indicates whether this part owns a relationship with a specified <see cref="P:System.IO.Packaging.PackageRelationship.Id" />.</summary>
	/// <param name="id">The <see cref="P:System.IO.Packaging.PackageRelationship.Id" /> of the relationship to check for.</param>
	/// <returns>
	///     <see langword="true" /> if this part owns a relationship with the specified <paramref name="id" />; otherwise, <see langword="false" />.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="id" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.Xml.XmlException">
	///         <paramref name="id" /> is not a valid XML identifier.</exception>
	/// <exception cref="T:System.InvalidOperationException">The part has been deleted.-or-The <see cref="P:System.IO.Packaging.PackagePart.Package" /> is not open (<see cref="M:System.IO.Packaging.Package.Dispose(System.Boolean)" /> or <see cref="M:System.IO.Packaging.Package.Close" /> has been called).</exception>
	/// <exception cref="T:System.IO.IOException">The package is write-only (relationship information cannot be read).</exception>
	public bool RelationshipExists(string id)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void ClearRelationships()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void Flush()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void Close()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void FlushRelationships()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void CloseRelationships()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
