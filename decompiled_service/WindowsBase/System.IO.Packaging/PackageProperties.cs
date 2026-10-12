namespace System.IO.Packaging;

/// <summary>Represents the core properties of a <see cref="T:System.IO.Packaging.Package" />.  </summary>
public abstract class PackageProperties : IDisposable
{
	/// <summary>When overridden in a derived class, gets or sets the name given to the <see cref="T:System.IO.Packaging.Package" /> and its content.</summary>
	/// <returns>The name given to the <see cref="T:System.IO.Packaging.Package" /> and its content.</returns>
	public abstract string Title { get; set; }

	/// <summary>When overridden in a derived class, gets or sets the topic of the <see cref="T:System.IO.Packaging.Package" /> content.</summary>
	/// <returns>The topic of the <see cref="T:System.IO.Packaging.Package" /> content.</returns>
	public abstract string Subject { get; set; }

	/// <summary>When overridden in a derived class, gets or sets a value that identifies the individual or entity that created the <see cref="T:System.IO.Packaging.Package" /> and its content.</summary>
	/// <returns>The individual or entity that created the <see cref="T:System.IO.Packaging.Package" /> and its content.</returns>
	public abstract string Creator { get; set; }

	/// <summary>When overridden in a derived class, gets or sets a value that define a delimited set of keywords to support searching and indexing the <see cref="T:System.IO.Packaging.Package" /> and its content.</summary>
	/// <returns>A delimited set of keywords to support searching and indexing the <see cref="T:System.IO.Packaging.Package" /> and content.</returns>
	public abstract string Keywords { get; set; }

	/// <summary>When overridden in a derived class, gets or sets a description of the content contained in the <see cref="T:System.IO.Packaging.Package" />.</summary>
	/// <returns>A description of the content contained in the <see cref="T:System.IO.Packaging.Package" />.</returns>
	public abstract string Description { get; set; }

	/// <summary>When overridden in a derived class, gets or sets a value that identifies the user who last modified the <see cref="T:System.IO.Packaging.Package" /> content.</summary>
	/// <returns>The user who last modified the <see cref="T:System.IO.Packaging.Package" /> content.</returns>
	public abstract string LastModifiedBy { get; set; }

	/// <summary>When overridden in a derived class, gets or sets the revision number of the <see cref="T:System.IO.Packaging.Package" />.</summary>
	/// <returns>The revision number of the <see cref="T:System.IO.Packaging.Package" />.</returns>
	public abstract string Revision { get; set; }

	/// <summary>When overridden in a derived class, gets or sets the date and time the <see cref="T:System.IO.Packaging.Package" /> content was last printed.</summary>
	/// <returns>The date and time the <see cref="T:System.IO.Packaging.Package" /> content was last printed.</returns>
	public abstract DateTime? LastPrinted { get; set; }

	/// <summary>When overridden in a derived class, gets or sets the date and time the <see cref="T:System.IO.Packaging.Package" /> was created.</summary>
	/// <returns>The date and time the <see cref="T:System.IO.Packaging.Package" /> was initially created.</returns>
	public abstract DateTime? Created { get; set; }

	/// <summary>When overridden in a derived class, gets or sets the date and time the <see cref="T:System.IO.Packaging.Package" /> was last changed.</summary>
	/// <returns>The date and time the <see cref="T:System.IO.Packaging.Package" /> was last changed.</returns>
	public abstract DateTime? Modified { get; set; }

	/// <summary>When overridden in a derived class, gets or sets the category of the <see cref="T:System.IO.Packaging.Package" />.</summary>
	/// <returns>The category of the content that is contained in the <see cref="T:System.IO.Packaging.Package" />.</returns>
	public abstract string Category { get; set; }

	/// <summary>When overridden in a derived class, gets or sets a value that unambiguously identifies the <see cref="T:System.IO.Packaging.Package" /> and its content.</summary>
	/// <returns>A value that unambiguously identifies the <see cref="T:System.IO.Packaging.Package" /> and its content.</returns>
	public abstract string Identifier { get; set; }

	/// <summary>When overridden in a derived class, gets or sets a value that represents the type of content that is contained in the <see cref="T:System.IO.Packaging.Package" />.</summary>
	/// <returns>The type of content that is contained in the <see cref="T:System.IO.Packaging.Package" />.</returns>
	public abstract string ContentType { get; set; }

	/// <summary>When overridden in a derived class, gets or sets a value that identifies the language of the <see cref="T:System.IO.Packaging.Package" /> content.</summary>
	/// <returns>A value that identifies the <see cref="T:System.IO.Packaging.Package" /> content language.</returns>
	public abstract string Language { get; set; }

	/// <summary>When overridden in a derived class, gets or sets the version number of the <see cref="T:System.IO.Packaging.Package" />.</summary>
	/// <returns>The version number of the <see cref="T:System.IO.Packaging.Package" />.</returns>
	public abstract string Version { get; set; }

	/// <summary>When overridden in a derived class, gets or sets a value that represents the status of the <see cref="T:System.IO.Packaging.Package" />.</summary>
	/// <returns>The status of the <see cref="T:System.IO.Packaging.Package" /> content.</returns>
	public abstract string ContentStatus { get; set; }

	/// <summary>Releases all resources used by the <see cref="T:System.IO.Packaging.PackageProperties" /> instance.</summary>
	public void Dispose()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Releases the unmanaged resources used by the <see cref="T:System.IO.Packaging.PackageProperties" /> instance and optionally releases the managed resources. </summary>
	/// <param name="disposing">
	///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
	protected virtual void Dispose(bool disposing)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.IO.Packaging.PackageProperties" /> class. </summary>
	protected PackageProperties()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
