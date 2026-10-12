using MS.Internal.WindowsBase;

namespace System.IO.Packaging;

/// <summary>Represents a container that can store multiple data objects.</summary>
public abstract class Package : IDisposable
{
	internal delegate bool PartOperation(PackagePart p);

	/// <summary>Gets the file access setting for the package.</summary>
	/// <returns>One of the <see cref="T:System.IO.FileAccess" /> values: <see cref="F:System.IO.FileAccess.Read" />, <see cref="F:System.IO.FileAccess.Write" />, or <see cref="F:System.IO.FileAccess.ReadWrite" />.</returns>
	/// <exception cref="T:System.ObjectDisposedException">The package is not open (<see cref="M:System.IO.Packaging.Package.Dispose(System.Boolean)" /> or <see cref="M:System.IO.Packaging.Package.Close" /> has been called).</exception>
	public FileAccess FileOpenAccess
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the core properties of the package.</summary>
	/// <returns>The core properties of the package.</returns>
	/// <exception cref="T:System.ObjectDisposedException">The package is not open (<see cref="M:System.IO.Packaging.Package.Dispose(System.Boolean)" /> or <see cref="M:System.IO.Packaging.Package.Close" /> has been called).</exception>
	public PackageProperties PackageProperties
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool InStreamingCreation
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.IO.Packaging.Package" /> class that uses a given <see cref="T:System.IO.FileAccess" />.</summary>
	/// <param name="openFileAccess">The file IO permissions for the package.</param>
	/// <exception cref="T:System.ArgumentOutOfRangeException">The value for <paramref name="openFileAccess" /> is not valid.</exception>
	protected Package(FileAccess openFileAccess)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.IO.Packaging.Package" /> class that uses a given <see cref="T:System.IO.FileAccess" /> and streaming option.</summary>
	/// <param name="openFileAccess">The file IO permissions for the package.</param>
	/// <param name="streaming">
	///       <see langword="true" /> to open the package for streaming; otherwise, <see langword="false" />.</param>
	/// <exception cref="T:System.ArgumentOutOfRangeException">The value for <paramref name="openFileAccess" /> is not valid.</exception>
	protected Package(FileAccess openFileAccess, bool streaming)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Opens a package at a given path and file name.</summary>
	/// <param name="path">The path and file name of the package.</param>
	/// <returns>The opened package.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="path" /> is <see langword="null" />.</exception>
	public static Package Open(string path)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Opens a package at a given path using a given file mode. </summary>
	/// <param name="path">The path and file name of the package.</param>
	/// <param name="packageMode">The file mode in which to open the package.</param>
	/// <returns>The opened package.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="path" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.ArgumentOutOfRangeException">Value for <paramref name="packageMode" /> is not valid.</exception>
	public static Package Open(string path, FileMode packageMode)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Opens a package at a given path using a given file mode and file access setting.</summary>
	/// <param name="path">The path and file name of the package.</param>
	/// <param name="packageMode">The file mode in which to open the package.</param>
	/// <param name="packageAccess">The file access in which to open the package.</param>
	/// <returns>The opened package.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="path" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.ArgumentOutOfRangeException">Value for <paramref name="packageMode" /> or <paramref name="packageAccess" /> is not valid.</exception>
	public static Package Open(string path, FileMode packageMode, FileAccess packageAccess)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Opens a package at a given path using a given file mode, file access, and file share setting.</summary>
	/// <param name="path">The path and file name of the package.</param>
	/// <param name="packageMode">The file mode in which to open the package.</param>
	/// <param name="packageAccess">The file access in which to open the package.</param>
	/// <param name="packageShare">The file sharing mode in which to open the package.</param>
	/// <returns>The opened package.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="path" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.ArgumentOutOfRangeException">The value for <paramref name="packageMode" />, <paramref name="packageAccess" />, or <paramref name="packageShare" /> is not valid.</exception>
	public static Package Open(string path, FileMode packageMode, FileAccess packageAccess, FileShare packageShare)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Opens a package on a given IO stream.</summary>
	/// <param name="stream">The IO stream on which to open the package.</param>
	/// <returns>The opened package.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="stream" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.IO.IOException">The package to open requires read or read/write permission and the specified <paramref name="stream" /> is write-only; or, the package to open requires write or read/write permission and the specified <paramref name="stream" /> is read-only.</exception>
	public static Package Open(Stream stream)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Opens a package with a given IO stream and file mode.</summary>
	/// <param name="stream">The IO stream on which to open the package.</param>
	/// <param name="packageMode">The file mode in which to open the package.</param>
	/// <returns>The opened package.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="stream" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.ArgumentOutOfRangeException">
	///         <paramref name="packageMode" /> value is not valid.</exception>
	/// <exception cref="T:System.IO.IOException">The package to open requires read or read/write permission and the specified <paramref name="stream" /> is write-only; or, the package to open requires write or read/write permission and the specified <paramref name="stream" /> is read-only.</exception>
	public static Package Open(Stream stream, FileMode packageMode)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Opens a package with a given IO stream, file mode, and file access setting.</summary>
	/// <param name="stream">The IO stream on which to open the package.</param>
	/// <param name="packageMode">The file mode in which to open the package.</param>
	/// <param name="packageAccess">The file access in which to open the package.</param>
	/// <returns>The opened package.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="stream" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.ArgumentOutOfRangeException">The value for <paramref name="packageMode" /> or <paramref name="packageAccess" /> is not valid.</exception>
	/// <exception cref="T:System.IO.IOException">The package to open requires read or read/write permission and the specified <paramref name="stream" /> is write-only; or the package to open requires write or read/write permission and the specified <paramref name="stream" /> is read-only.</exception>
	public static Package Open(Stream stream, FileMode packageMode, FileAccess packageAccess)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates a new uncompressed part with a given URI and content type.</summary>
	/// <param name="partUri">The uniform resource identifier (URI) of the new part.</param>
	/// <param name="contentType">The content type of the data stream.</param>
	/// <returns>The new created part.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="partUri" /> or <paramref name="contentType" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.ArgumentException">
	///         <paramref name="partUri" /> is not a valid <see cref="T:System.IO.Packaging.PackagePart" /> URI.</exception>
	/// <exception cref="T:System.InvalidOperationException">A part with the specified <paramref name="partUri" /> is already present in the package.</exception>
	/// <exception cref="T:System.ObjectDisposedException">The package is not open (<see cref="M:System.IO.Packaging.Package.Dispose(System.Boolean)" /> or <see cref="M:System.IO.Packaging.Package.Close" /> has been called).</exception>
	/// <exception cref="T:System.IO.IOException">The package is read-only (a new part cannot be added).</exception>
	public PackagePart CreatePart(Uri partUri, string contentType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates a new part with a given URI, content type, and compression option.</summary>
	/// <param name="partUri">The URI of the new part.</param>
	/// <param name="contentType">The content type of the data stream.</param>
	/// <param name="compressionOption">The compression option for the data stream, <see cref="F:System.IO.Packaging.CompressionOption.NotCompressed" /> or <see cref="F:System.IO.Packaging.CompressionOption.Normal" /> compression.</param>
	/// <returns>The new created part.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="partUri" /> or <paramref name="contentType" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.ArgumentException">
	///         <paramref name="partUri" /> is not a valid <see cref="T:System.IO.Packaging.PackagePart" /> uniform resource identifier (URI).</exception>
	/// <exception cref="T:System.InvalidOperationException">A part with the specified <paramref name="partUri" /> is already present in the package.</exception>
	/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="compressionOption" /> value is not valid.</exception>
	/// <exception cref="T:System.ObjectDisposedException">The package is not open (<see cref="M:System.IO.Packaging.Package.Dispose(System.Boolean)" /> or <see cref="M:System.IO.Packaging.Package.Close" /> has been called).</exception>
	/// <exception cref="T:System.IO.IOException">The package is read-only (a new part cannot be added).</exception>
	public PackagePart CreatePart(Uri partUri, string contentType, CompressionOption compressionOption)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns the part with a given URI.</summary>
	/// <param name="partUri">The uniform resource identifier (URI) of the part to return.</param>
	/// <returns>The part with the specified <paramref name="partUri" />.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="partUri" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.ArgumentException">
	///         <paramref name="partUri" /> is not a valid <see cref="T:System.IO.Packaging.PackagePart" /> uniform resource identifier (URI).</exception>
	/// <exception cref="T:System.InvalidOperationException">A part with the specified <paramref name="partUri" /> is not in the package.</exception>
	/// <exception cref="T:System.ObjectDisposedException">The package is not open (<see cref="M:System.IO.Packaging.Package.Dispose(System.Boolean)" /> or <see cref="M:System.IO.Packaging.Package.Close" /> has been called).</exception>
	/// <exception cref="T:System.IO.IOException">The package is write-only.</exception>
	public PackagePart GetPart(Uri partUri)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Indicates whether a part with a given URI is in the package.</summary>
	/// <param name="partUri">The <see cref="T:System.Uri" /> of the part to check for.</param>
	/// <returns>
	///     <see langword="true" /> if a part with the specified <paramref name="partUri" /> is in the package; otherwise, <see langword="false" />.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="partUri" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.ArgumentException">
	///         <paramref name="partUri" /> is not a valid <see cref="T:System.IO.Packaging.PackagePart" /> uniform resource identifier (URI).</exception>
	/// <exception cref="T:System.ObjectDisposedException">The package is not open (<see cref="M:System.IO.Packaging.Package.Dispose(System.Boolean)" /> or <see cref="M:System.IO.Packaging.Package.Close" /> has been called).</exception>
	/// <exception cref="T:System.IO.IOException">The package is write-only (information cannot be read).</exception>
	public virtual bool PartExists(Uri partUri)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Deletes a part with a given URI from the package.</summary>
	/// <param name="partUri">The URI of the part to delete.</param>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="partUri" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.ArgumentException">
	///         <paramref name="partUri" /> is not a valid <see cref="T:System.IO.Packaging.PackagePart" /> URI.</exception>
	/// <exception cref="T:System.ObjectDisposedException">The package is not open (<see cref="M:System.IO.Packaging.Package.Dispose(System.Boolean)" /> or <see cref="M:System.IO.Packaging.Package.Close" /> has been called).</exception>
	/// <exception cref="T:System.IO.IOException">The package is read-only.</exception>
	public void DeletePart(Uri partUri)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns a collection of all the parts in the package.</summary>
	/// <returns>A collection of all the <see cref="T:System.IO.Packaging.PackagePart" /> elements that are contained in the package.</returns>
	/// <exception cref="T:System.ObjectDisposedException">The package is not open (<see cref="M:System.IO.Packaging.Package.Dispose(System.Boolean)" /> or <see cref="M:System.IO.Packaging.Package.Close" /> has been called).</exception>
	/// <exception cref="T:System.IO.IOException">The package is write-only.</exception>
	public PackagePartCollection GetParts()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended for application use.  Use the type-safe <see cref="M:System.IO.Packaging.Package.Dispose(System.Boolean)" /> method instead. </summary>
	void IDisposable.Dispose()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Saves and closes the package plus all underlying part streams.</summary>
	public void Close()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Saves the contents of all parts and relationships that are contained in the package.</summary>
	/// <exception cref="T:System.ObjectDisposedException">The package is not open (<see cref="M:System.IO.Packaging.Package.Dispose(System.Boolean)" /> or <see cref="M:System.IO.Packaging.Package.Close" /> has been called).</exception>
	/// <exception cref="T:System.IO.IOException">The package is read-only and cannot be modified.</exception>
	public void Flush()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates a package-level relationship to a part with a given URI, target mode, and relationship type. </summary>
	/// <param name="targetUri">The uniform resource identifier (URI) of the target part.</param>
	/// <param name="targetMode">Indicates if the target part is <see cref="F:System.IO.Packaging.TargetMode.Internal" /> or <see cref="F:System.IO.Packaging.TargetMode.External" /> to the package.</param>
	/// <param name="relationshipType">A URI that uniquely defines the role of the relationship.</param>
	/// <returns>The package-level relationship to the specified part.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="targetUri" /> or <paramref name="relationshipType" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.ArgumentException">The <paramref name="targetUri" /> part is a <see cref="T:System.IO.Packaging.PackageRelationship" />, or <paramref name="targetMode" /> is <see cref="F:System.IO.Packaging.TargetMode.Internal" /> and <paramref name="targetUri" /> is an absolute URI.</exception>
	/// <exception cref="T:System.ArgumentOutOfRangeException">The value for <paramref name="targetMode" /> is not valid.</exception>
	/// <exception cref="T:System.ObjectDisposedException">The package is not open (<see cref="M:System.IO.Packaging.Package.Dispose(System.Boolean)" /> or <see cref="M:System.IO.Packaging.Package.Close" /> has been called).</exception>
	/// <exception cref="T:System.IO.IOException">The package is read-only.</exception>
	public PackageRelationship CreateRelationship(Uri targetUri, TargetMode targetMode, string relationshipType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates a package-level relationship to a part with a given URI, target mode, relationship type, and identifier (ID).</summary>
	/// <param name="targetUri">The uniform resource identifier (URI) of the target part.</param>
	/// <param name="targetMode">Indicates if the target part is <see cref="F:System.IO.Packaging.TargetMode.Internal" /> or <see cref="F:System.IO.Packaging.TargetMode.External" /> to the package.</param>
	/// <param name="relationshipType">A URI that uniquely defines the role of the relationship.</param>
	/// <param name="id">A unique XML identifier.</param>
	/// <returns>The package-level relationship to the specified part.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="targetUri" /> or <paramref name="relationshipType" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.ArgumentException">The <paramref name="targetUri" /> part is a <see cref="T:System.IO.Packaging.PackageRelationship" />, or <paramref name="targetMode" /> is <see cref="F:System.IO.Packaging.TargetMode.Internal" /> and <paramref name="targetUri" /> is an absolute URI.</exception>
	/// <exception cref="T:System.ArgumentOutOfRangeException">The value for <paramref name="targetMode" /> is not valid.</exception>
	/// <exception cref="T:System.ObjectDisposedException">The package is not open (<see cref="M:System.IO.Packaging.Package.Dispose(System.Boolean)" /> or <see cref="M:System.IO.Packaging.Package.Close" /> has been called).</exception>
	/// <exception cref="T:System.IO.IOException">The package is read-only.</exception>
	/// <exception cref="T:System.Xml.XmlException">
	///         <paramref name="id" /> is not a valid XML identifier; or a part with the specified <paramref name="id" /> already occurs in the package.</exception>
	public PackageRelationship CreateRelationship(Uri targetUri, TargetMode targetMode, string relationshipType, string id)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Deletes a package-level relationship.</summary>
	/// <param name="id">The <see cref="P:System.IO.Packaging.PackageRelationship.Id" /> of the <see cref="T:System.IO.Packaging.PackageRelationship" /> to delete.</param>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="id" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.ObjectDisposedException">The package is not open (<see cref="M:System.IO.Packaging.Package.Dispose(System.Boolean)" /> or <see cref="M:System.IO.Packaging.Package.Close" /> has been called).</exception>
	/// <exception cref="T:System.IO.IOException">The package is read-only.</exception>
	/// <exception cref="T:System.Xml.XmlException">
	///         <paramref name="id" /> is not a valid XML identifier.</exception>
	public void DeleteRelationship(string id)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns a collection of all the package-level relationships.</summary>
	/// <returns>A collection of all the package-level relationships that are contained in the package.</returns>
	/// <exception cref="T:System.ObjectDisposedException">The package is not open (<see cref="M:System.IO.Packaging.Package.Dispose(System.Boolean)" /> or <see cref="M:System.IO.Packaging.Package.Close" /> has been called).</exception>
	/// <exception cref="T:System.IO.IOException">The package is write-only.</exception>
	public PackageRelationshipCollection GetRelationships()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns a collection of all the package-level relationships that match a given <see cref="P:System.IO.Packaging.PackageRelationship.RelationshipType" />. </summary>
	/// <param name="relationshipType">The <see cref="P:System.IO.Packaging.PackageRelationship.RelationshipType" />  to match and return in the collection.</param>
	/// <returns>A collection of package-level relationships that match the specified <paramref name="relationshipType" />.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="relationshipType" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.ArgumentException">
	///         <paramref name="relationshipType" /> is an empty string.</exception>
	/// <exception cref="T:System.ObjectDisposedException">The package is not open (<see cref="M:System.IO.Packaging.Package.Dispose(System.Boolean)" /> or <see cref="M:System.IO.Packaging.Package.Close" /> has been called).</exception>
	/// <exception cref="T:System.IO.IOException">The package is write-only.</exception>
	public PackageRelationshipCollection GetRelationshipsByType(string relationshipType)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns the package-level relationship with a given identifier.</summary>
	/// <param name="id">The <see cref="P:System.IO.Packaging.PackageRelationship.Id" /> of the relationship to return.</param>
	/// <returns>The package-level relationship with the specified <paramref name="id" />.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="id" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.Xml.XmlException">
	///         <paramref name="id" /> is not a valid XML identifier.</exception>
	/// <exception cref="T:System.InvalidOperationException">A relationship with the specified <paramref name="id" /> is not in the package.</exception>
	/// <exception cref="T:System.ObjectDisposedException">The package is not open (<see cref="M:System.IO.Packaging.Package.Dispose(System.Boolean)" /> or <see cref="M:System.IO.Packaging.Package.Close" /> has been called).</exception>
	/// <exception cref="T:System.IO.IOException">The package is write-only.</exception>
	public PackageRelationship GetRelationship(string id)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Indicates whether a package-level relationship with a given ID is contained in the package.</summary>
	/// <param name="id">The <see cref="P:System.IO.Packaging.PackageRelationship.Id" /> of the relationship to check for.</param>
	/// <returns>
	///     <see langword="true" /> if a package-level relationship with the specified <paramref name="id" /> is in the package; otherwise, <see langword="false" />.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="id" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.Xml.XmlException">
	///         <paramref name="id" /> is not a valid XML identifier.</exception>
	/// <exception cref="T:System.ObjectDisposedException">The package is not open (<see cref="M:System.IO.Packaging.Package.Dispose(System.Boolean)" /> or <see cref="M:System.IO.Packaging.Package.Close" /> has been called).</exception>
	/// <exception cref="T:System.IO.IOException">The package is write-only.</exception>
	public bool RelationshipExists(string id)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>When overridden in a derived class, creates a new part in the package.</summary>
	/// <param name="partUri">The uniform resource identifier (URI) for the part being created.</param>
	/// <param name="contentType">The content type of the data stream.</param>
	/// <param name="compressionOption">The compression option for the data stream.</param>
	/// <returns>The created part.</returns>
	protected abstract PackagePart CreatePartCore(Uri partUri, string contentType, CompressionOption compressionOption);

	/// <summary>When overridden in a derived class, returns the part addressed by a given URI.</summary>
	/// <param name="partUri">The uniform resource identifier (URI) of the part to retrieve.</param>
	/// <returns>The requested part; or <see langword="null" />, if a part with the specified <paramref name="partUri" /> is not in the package.</returns>
	protected abstract PackagePart GetPartCore(Uri partUri);

	/// <summary>When overridden in a derived class, deletes a part with a given URI. </summary>
	/// <param name="partUri">The <see cref="P:System.IO.Packaging.PackagePart.Uri" /> of the <see cref="T:System.IO.Packaging.PackagePart" /> to delete.</param>
	protected abstract void DeletePartCore(Uri partUri);

	/// <summary>When overridden in a derived class, returns an array of all the parts in the package. </summary>
	/// <returns>An array of all the parts that are contained in the package.</returns>
	protected abstract PackagePart[] GetPartsCore();

	/// <summary>Flushes and saves the content of all parts and relationships, closes the package, and releases all resources.</summary>
	/// <param name="disposing">
	///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
	protected virtual void Dispose(bool disposing)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>When overridden in a derived class, saves the content of all parts and relationships to the derived class store.</summary>
	protected abstract void FlushCore();

	internal void ThrowIfInStreamingCreation(string methodName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void ThrowIfNotInStreamingCreation(string methodName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void ThrowIfReadOnly()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void ThrowIfWriteOnly()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static void ThrowIfFileModeInvalid(FileMode mode)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static void ThrowIfFileAccessInvalid(FileAccess access)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static void ThrowIfCompressionOptionInvalid(CompressionOption compressionOption)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static Package Open(string path, FileMode packageMode, FileAccess packageAccess, FileShare packageShare, bool streaming)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[FriendAccessAllowed]
	internal static Package Open(Stream stream, FileMode packageMode, FileAccess packageAccess, bool streaming)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void FlushPackageRelationships()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void ClosePackageRelationships()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static Package()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
