using System.Collections.Generic;
using System.Collections.ObjectModel;
using MS.Internal.Security.RightsManagement;

namespace System.Security.RightsManagement;

/// <summary>Provides digital rights management services for encrypting and decrypting protected content. </summary>
[SecurityCritical(SecurityCriticalScope.Everything)]
public class CryptoProvider : IDisposable
{
	/// <summary>Gets the cipher block size, in bytes. </summary>
	/// <returns>The cipher block size, in bytes.  The default block size for Advanced Encryption Standard (AES) is 8.</returns>
	public int BlockSize
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets a value that indicates whether <see cref="M:System.Security.RightsManagement.CryptoProvider.Encrypt(System.Byte[])" /> and <see cref="M:System.Security.RightsManagement.CryptoProvider.Decrypt(System.Byte[])" /> can accept buffers that are different block sizes in length.</summary>
	/// <returns>
	///     true if the buffer passed to <see cref="M:System.Security.RightsManagement.CryptoProvider.Encrypt(System.Byte[])" /> can be a different length from the buffer passed to <see cref="M:System.Security.RightsManagement.CryptoProvider.Decrypt(System.Byte[])" />; otherwise, false if the buffers passed to <see cref="M:System.Security.RightsManagement.CryptoProvider.Encrypt(System.Byte[])" /> and <see cref="M:System.Security.RightsManagement.CryptoProvider.Decrypt(System.Byte[])" /> must be the exact same length.  For Advanced Encryption Standard (AES) the default is true.</returns>
	public bool CanMergeBlocks
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets a collection listing the rights that passed verification and that are granted to the user.</summary>
	/// <returns>A collection enumerating the rights that passed verification and that are granted to the user.</returns>
	public ReadOnlyCollection<ContentGrant> BoundGrants
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets a value that indicates whether the user has rights to encrypt. </summary>
	/// <returns>
	///     true if the <see cref="T:System.Security.RightsManagement.UseLicense" /> grants the user rights to encrypt; otherwise, false.</returns>
	public bool CanEncrypt
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets a value that indicates whether the user has rights to decrypt. </summary>
	/// <returns>
	///     true if the <see cref="T:System.Security.RightsManagement.UseLicense" /> grants the user rights to decrypt; otherwise, false.</returns>
	public bool CanDecrypt
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Frees resources and performs internal cleanup before the instance is reclaimed by garbage collection.</summary>
	~CryptoProvider()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Releases all resources used by the <see cref="T:System.Security.RightsManagement.CryptoProvider" />. </summary>
	public void Dispose()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Encrypts clear text to cipher text. </summary>
	/// <param name="clearText">The clear text content to encrypt.</param>
	/// <returns>Encrypted cipher text of the given <paramref name="clearText" />.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="clearText" /> is null.</exception>
	/// <exception cref="T:System.Security.RightsManagement.RightsManagementException">Encryption is not permitted.</exception>
	public byte[] Encrypt(byte[] clearText)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Decrypts cipher text to clear text. </summary>
	/// <param name="cryptoText">The cipher text to decrypt.</param>
	/// <returns>The decrypted clear text of <paramref name="cryptoText" />.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="cipherText" /> is null.</exception>
	/// <exception cref="T:System.Security.RightsManagement.RightsManagementException">Decryption right not granted.</exception>
	public byte[] Decrypt(byte[] cryptoText)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Security.RightsManagement.CryptoProvider" /> and optionally releases the managed resources.  </summary>
	/// <param name="disposing">
	///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
	protected virtual void Dispose(bool disposing)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal CryptoProvider(List<SafeRightsManagementHandle> boundLicenseHandleList, List<RightNameExpirationInfoPair> rightsInfoList, ContentUser owner)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal UnsignedPublishLicense DecryptPublishLicense(string serializedPublishLicense)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
