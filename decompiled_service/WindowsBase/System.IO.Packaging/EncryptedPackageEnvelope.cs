using System.Security.RightsManagement;

namespace System.IO.Packaging;

/// <summary>Represents an OLE compound file that contains an encrypted package.</summary>
public class EncryptedPackageEnvelope : IDisposable
{
	/// <summary>Gets the rights management information stored in the <see cref="T:System.IO.Packaging.EncryptedPackageEnvelope" />; specifically, the <see cref="T:System.Security.RightsManagement.PublishLicense" /> and the <see cref="T:System.Security.RightsManagement.UseLicense" />s stored in the OLE compound file that embodies the rights-management protected package</summary>
	/// <returns>The <see cref="T:System.IO.Packaging.RightsManagementInformation" /> for the package.</returns>
	public RightsManagementInformation RightsManagementInformation
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the core package properties (such as Title and Subject) of the rights managed document.</summary>
	/// <returns>The <see cref="T:System.IO.Packaging.PackageProperties" /> for the package.</returns>
	public PackageProperties PackageProperties
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets a value that specifies whether the file was opened with access to read, write, or both.</summary>
	/// <returns>The <see cref="T:System.IO.FileAccess" /> value that was used to open the file. </returns>
	public FileAccess FileOpenAccess
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets an object that provides access to the compound file streams outside the encrypted package.</summary>
	/// <returns>A <see cref="T:System.IO.Packaging.StorageInfo" /> representing streams in the OLE compound file that are outside the encrypted package.</returns>
	public StorageInfo StorageInfo
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal static string EncryptionTransformName
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal static string PackageStreamName
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal static string DataspaceLabelRMEncryptionNoCompression
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal EncryptedPackageEnvelope(string envelopeFileName, PublishLicense publishLicense, CryptoProvider cryptoProvider)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal EncryptedPackageEnvelope(Stream envelopeStream, PublishLicense publishLicense, CryptoProvider cryptoProvider)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal EncryptedPackageEnvelope(string envelopeFileName, Stream packageStream, PublishLicense publishLicense, CryptoProvider cryptoProvider)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal EncryptedPackageEnvelope(Stream envelopeStream, Stream packageStream, PublishLicense publishLicense, CryptoProvider cryptoProvider)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal EncryptedPackageEnvelope(string envelopeFileName, FileAccess access, FileShare sharing)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal EncryptedPackageEnvelope(Stream envelopeStream)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates and returns an <see cref="T:System.IO.Packaging.EncryptedPackageEnvelope" /> and gives it the specified file name.</summary>
	/// <param name="envelopeFileName">The name of the OLE compound file.</param>
	/// <param name="publishLicense">The publish license that is embedded in the compound file.</param>
	/// <param name="cryptoProvider">An object that determines what operations the current user is allowed to perform on the encrypted content.</param>
	/// <returns>The newly created <see cref="T:System.IO.Packaging.EncryptedPackageEnvelope" />. </returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="envelopeFileName" /> is <see langword="null" />.</exception>
	public static EncryptedPackageEnvelope Create(string envelopeFileName, PublishLicense publishLicense, CryptoProvider cryptoProvider)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates and returns an <see cref="T:System.IO.Packaging.EncryptedPackageEnvelope" /> on the specified stream.</summary>
	/// <param name="envelopeStream">The stream on which to create the file.</param>
	/// <param name="publishLicense">The publish license that is embedded in the compound file.</param>
	/// <param name="cryptoProvider">An object that determines what operations the current user is allowed to perform on the encrypted content.</param>
	/// <returns>The newly created <see cref="T:System.IO.Packaging.EncryptedPackageEnvelope" />. </returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="envelopeStream" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.NotSupportedException">
	///         <paramref name="envelopeStream" /> does not provide read/write access.</exception>
	public static EncryptedPackageEnvelope Create(Stream envelopeStream, PublishLicense publishLicense, CryptoProvider cryptoProvider)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates and returns an <see cref="T:System.IO.Packaging.EncryptedPackageEnvelope" /> that uses the specified unencrypted package as its content, and gives it the specified file name.</summary>
	/// <param name="envelopeFileName">The name of the OLE compound file.</param>
	/// <param name="packageStream">The stream representing the existing unencrypted package.</param>
	/// <param name="publishLicense">The publish license that is embedded in the compound file.</param>
	/// <param name="cryptoProvider">An object that determines what operations the current user is allowed to perform on the encrypted content.</param>
	/// <returns>The newly created <see cref="T:System.IO.Packaging.EncryptedPackageEnvelope" />.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="envelopeFileName" /> or <paramref name="packageStream" /> is <see langword="null" />.</exception>
	public static EncryptedPackageEnvelope CreateFromPackage(string envelopeFileName, Stream packageStream, PublishLicense publishLicense, CryptoProvider cryptoProvider)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates and returns an <see cref="T:System.IO.Packaging.EncryptedPackageEnvelope" /> (on the specified stream) that uses the specified unencrypted package as its content.</summary>
	/// <param name="envelopeStream">The stream on which to create the file.</param>
	/// <param name="packageStream">The stream representing the existing unencrypted package.</param>
	/// <param name="publishLicense">The publish license that is embedded in the compound file.</param>
	/// <param name="cryptoProvider">An object that determines what operations the current user is allowed to perform on the encrypted content.</param>
	/// <returns>The newly created <see cref="T:System.IO.Packaging.EncryptedPackageEnvelope" />. </returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="envelopeStream" /> or <paramref name="packageStream" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.NotSupportedException">
	///         <paramref name="envelopeStream" /> does not provide read/write access.</exception>
	public static EncryptedPackageEnvelope CreateFromPackage(Stream envelopeStream, Stream packageStream, PublishLicense publishLicense, CryptoProvider cryptoProvider)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Open the encrypted package in the specified file as read only and unshared.</summary>
	/// <param name="envelopeFileName">The file containing the encrypted package.</param>
	/// <returns>The <see cref="T:System.IO.Packaging.EncryptedPackageEnvelope" /> that is opened.</returns>
	public static EncryptedPackageEnvelope Open(string envelopeFileName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Open the encrypted package in the specified file as unshared and with the specified access.</summary>
	/// <param name="envelopeFileName">The file containing the encrypted package.</param>
	/// <param name="access">The type of access.</param>
	/// <returns>The <see cref="T:System.IO.Packaging.EncryptedPackageEnvelope" /> that is opened.</returns>
	public static EncryptedPackageEnvelope Open(string envelopeFileName, FileAccess access)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Open the encrypted package in the specified file and gives it the specified access and sharing.</summary>
	/// <param name="envelopeFileName">The file containing the encrypted package.</param>
	/// <param name="access">The type of access.</param>
	/// <param name="sharing">The type of sharing.</param>
	/// <returns>The <see cref="T:System.IO.Packaging.EncryptedPackageEnvelope" /> that is opened.</returns>
	public static EncryptedPackageEnvelope Open(string envelopeFileName, FileAccess access, FileShare sharing)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Open the encrypted package in the specified stream.</summary>
	/// <param name="envelopeStream">The stream containing the encrypted package.</param>
	/// <returns>The <see cref="T:System.IO.Packaging.EncryptedPackageEnvelope" /> that is opened.</returns>
	public static EncryptedPackageEnvelope Open(Stream envelopeStream)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Gets a value specifying whether the specified file is an OLE compound file with an encrypted package stream.</summary>
	/// <param name="fileName">The file that is tested.</param>
	/// <returns>
	///     <see langword="true" /> if the specified file is an OLE compound file with an encrypted package stream; otherwise, <see langword="false" />. </returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="fileName" /> is <see langword="null" />.</exception>
	public static bool IsEncryptedPackageEnvelope(string fileName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Gets a value specifying whether the specified stream is an OLE compound file with an encrypted package stream.</summary>
	/// <param name="stream">The stream that is tested.</param>
	/// <returns>
	///     <see langword="true" /> if the specified stream is an OLE compound file with an encrypted package stream; otherwise, <see langword="false" />. </returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="stream" /> is <see langword="null" />.</exception>
	public static bool IsEncryptedPackageEnvelope(Stream stream)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Flush the stream for both the <see cref="T:System.IO.Packaging.EncryptedPackageEnvelope" /> and its package content.</summary>
	public void Flush()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Closes the encrypted package and the OLE compound file that holds it.</summary>
	public void Close()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Releases all resources used by the <see cref="T:System.IO.Packaging.EncryptedPackageEnvelope" />. </summary>
	public void Dispose()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Gets the encrypted package inside the <see cref="T:System.IO.Packaging.EncryptedPackageEnvelope" />.</summary>
	/// <returns>The <see cref="T:System.IO.Packaging.Package" /> in the envelope. </returns>
	public Package GetPackage()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal Stream GetPackageStream()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Releases the unmanaged resources used by the <see cref="T:System.IO.Packaging.EncryptedPackageEnvelope" /> and optionally releases the managed resources. </summary>
	/// <param name="disposing">
	///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
	protected virtual void Dispose(bool disposing)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
