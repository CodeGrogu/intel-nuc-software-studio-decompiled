using System.Collections;
using System.Runtime.InteropServices.ComTypes;
using MS.Internal.IO.Packaging.CompoundFile;

namespace System.IO.Packaging;

/// <summary>Provides access and information for manipulating data stores in a <see cref="T:System.IO.Packaging.Package" />.  </summary>
public class StorageInfo
{
	internal StorageInfoCore core;

	/// <summary>Gets the name of the store.</summary>
	/// <returns>The name of this store.</returns>
	public string Name
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal string FullNameInternal
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal StorageRoot Root
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool Exists
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal IStorage SafeIStorage
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal bool StorageDisposed
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal StorageInfo(IStorage safeIStorage)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal StorageInfo(StorageInfo parent, string fileName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal StreamInfoCore CoreForChildStream(string streamName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates a new stream with a given name, <see cref="T:System.IO.Packaging.CompressionOption" />, and <see cref="T:System.IO.Packaging.EncryptionOption" />.</summary>
	/// <param name="name">The name for the new stream.</param>
	/// <param name="compressionOption">The compression option for the data stream.</param>
	/// <param name="encryptionOption">The encryption option for the data stream.</param>
	/// <returns>The new stream with the specified <paramref name="name" />, <paramref name="compressionOption" />, and <paramref name="encryptionOption" />.</returns>
	public StreamInfo CreateStream(string name, CompressionOption compressionOption, EncryptionOption encryptionOption)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates a new stream with a given name.</summary>
	/// <param name="name">The name for the new stream.</param>
	/// <returns>The new stream with the specified <paramref name="name" />.</returns>
	public StreamInfo CreateStream(string name)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns the <see cref="T:System.IO.Packaging.StreamInfo" /> instance with the given name.</summary>
	/// <param name="name">The name of the <see cref="T:System.IO.Packaging.StreamInfo" /> instance to return.</param>
	/// <returns>The stream with the specified <paramref name="name" />.</returns>
	/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is <see langword="null" />.</exception>
	public StreamInfo GetStreamInfo(string name)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns a value that indicates whether a given stream exists.</summary>
	/// <param name="name">The <see cref="T:System.IO.Packaging.StreamInfo" /> name to check for.</param>
	/// <returns>
	///     <see langword="true" /> if a stream with the specified <paramref name="name" /> exists; otherwise, <see langword="false" />.</returns>
	public bool StreamExists(string name)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Deletes the stream with a specified name. </summary>
	/// <param name="name">The <see cref="T:System.IO.Packaging.StreamInfo" /> name of the stream to delete.</param>
	/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is <see langword="null" />.</exception>
	public void DeleteStream(string name)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates a new child <see cref="T:System.IO.Packaging.StorageInfo" /> with this <see cref="T:System.IO.Packaging.StorageInfo" /> as the parent.</summary>
	/// <param name="name">The name for the new child data store.</param>
	/// <returns>The new child data store.</returns>
	/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is <see langword="null" />.</exception>
	public StorageInfo CreateSubStorage(string name)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns the child sub-store with a given name.</summary>
	/// <param name="name">The name of the sub-store to return.</param>
	/// <returns>The child sub-store with the specified <paramref name="name" />.</returns>
	/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is <see langword="null" />.</exception>
	public StorageInfo GetSubStorageInfo(string name)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns a value that indicates whether a given child sub-store exists.</summary>
	/// <param name="name">The child <see cref="T:System.IO.Packaging.StorageInfo" /> name to check for.</param>
	/// <returns>
	///     <see langword="true" /> if a child sub-store with the specified <paramref name="name" /> exists; otherwise, <see langword="false" />.</returns>
	public bool SubStorageExists(string name)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Deletes a specified sub-store.</summary>
	/// <param name="name">The name of the sub-store to delete.</param>
	/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is <see langword="null" />.</exception>
	public void DeleteSubStorage(string name)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns an array of the <see cref="T:System.IO.Packaging.StreamInfo" /> instances that are currently contained in this store.</summary>
	/// <returns>An array of the <see cref="T:System.IO.Packaging.StreamInfo" /> objects, each pointing to an I/O steam defined within this store.</returns>
	public StreamInfo[] GetStreams()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns an array of the child sub-stores that are currently contained in this store.</summary>
	/// <returns>An array of <see cref="T:System.IO.Packaging.StorageInfo" /> objects, each pointing to a sub-store defined within this store.</returns>
	public StorageInfo[] GetSubStorages()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void Create()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal bool Delete(bool recursive, string name)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void RemoveSubStorageEntryFromDataSpaceMap(StorageInfo storageInfo)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void DestroyElement(string elementNameInternal)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal bool FindStatStgOfName(string streamName, out STATSTG statStg)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal bool StorageIsEmpty()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void InvalidateEnumerators()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal ArrayList BuildFullNameFromParentName()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal ArrayList BuildFullNameInternalFromParentNameInternal()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static void RecursiveStorageInfoCoreRelease(StorageInfoCore startCore)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void CheckDisposedStatus()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static StorageInfo()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
