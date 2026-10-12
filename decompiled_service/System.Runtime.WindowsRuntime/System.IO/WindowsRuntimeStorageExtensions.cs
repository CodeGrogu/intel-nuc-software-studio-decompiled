using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;

namespace System.IO;

[__DynamicallyInvokable]
public static class WindowsRuntimeStorageExtensions
{
	[CLSCompliant(false)]
	public static SafeFileHandle CreateSafeFileHandle(this IStorageFile windowsRuntimeFile, FileAccess access = FileAccess.ReadWrite, FileShare share = FileShare.Read, FileOptions options = FileOptions.None)
	{
		if (windowsRuntimeFile == null)
		{
			throw new ArgumentNullException("windowsRuntimeFile");
		}
		HANDLE_ACCESS_OPTIONS accessOptions = FileAccessToHandleAccessOptions(access);
		HANDLE_SHARING_OPTIONS sharingOptions = FileShareToHandleSharingOptions(share);
		HANDLE_OPTIONS options2 = FileOptionsToHandleOptions(options);
		if (!(windowsRuntimeFile is IStorageItemHandleAccess storageItemHandleAccess))
		{
			return null;
		}
		int num = storageItemHandleAccess.Create(accessOptions, sharingOptions, options2, IntPtr.Zero, out var interopHandle);
		if (num != 0)
		{
			throw Win32Marshal.GetExceptionForWin32Error(Win32Marshal.TryMakeWin32ErrorCodeFromHR(num), ((IStorageItem)windowsRuntimeFile).Name);
		}
		return interopHandle;
	}

	[CLSCompliant(false)]
	public static SafeFileHandle CreateSafeFileHandle(this IStorageFolder rootDirectory, string relativePath, FileMode mode)
	{
		return rootDirectory.CreateSafeFileHandle(relativePath, mode, (mode == FileMode.Append) ? FileAccess.Write : FileAccess.ReadWrite);
	}

	[CLSCompliant(false)]
	public static SafeFileHandle CreateSafeFileHandle(this IStorageFolder rootDirectory, string relativePath, FileMode mode, FileAccess access, FileShare share = FileShare.Read, FileOptions options = FileOptions.None)
	{
		if (rootDirectory == null)
		{
			throw new ArgumentNullException("rootDirectory");
		}
		if (relativePath == null)
		{
			throw new ArgumentNullException("relativePath");
		}
		HANDLE_CREATION_OPTIONS creationOptions = FileModeToCreationOptions(mode);
		HANDLE_ACCESS_OPTIONS accessOptions = FileAccessToHandleAccessOptions(access);
		HANDLE_SHARING_OPTIONS sharingOptions = FileShareToHandleSharingOptions(share);
		HANDLE_OPTIONS options2 = FileOptionsToHandleOptions(options);
		if (!(rootDirectory is IStorageFolderHandleAccess storageFolderHandleAccess))
		{
			return null;
		}
		int num = storageFolderHandleAccess.Create(relativePath, creationOptions, accessOptions, sharingOptions, options2, IntPtr.Zero, out var interopHandle);
		if (num != 0)
		{
			throw Win32Marshal.GetExceptionForWin32Error(Win32Marshal.TryMakeWin32ErrorCodeFromHR(num), relativePath);
		}
		return interopHandle;
	}

	private static HANDLE_ACCESS_OPTIONS FileAccessToHandleAccessOptions(FileAccess access)
	{
		return access switch
		{
			FileAccess.ReadWrite => HANDLE_ACCESS_OPTIONS.HAO_WRITE | HANDLE_ACCESS_OPTIONS.HAO_READ_ATTRIBUTES | HANDLE_ACCESS_OPTIONS.FILE_READ_DATA | HANDLE_ACCESS_OPTIONS.FILE_READ_EA, 
			FileAccess.Read => HANDLE_ACCESS_OPTIONS.HAO_READ, 
			FileAccess.Write => HANDLE_ACCESS_OPTIONS.HAO_WRITE, 
			_ => throw new ArgumentOutOfRangeException("access", access, null), 
		};
	}

	private static HANDLE_SHARING_OPTIONS FileShareToHandleSharingOptions(FileShare share)
	{
		if ((share & FileShare.Inheritable) != 0)
		{
			throw new NotSupportedException("NotSupported_Inheritable");
		}
		if ((share < FileShare.None) || share > (FileShare.ReadWrite | FileShare.Delete))
		{
			throw new ArgumentOutOfRangeException("share", share, null);
		}
		HANDLE_SHARING_OPTIONS hANDLE_SHARING_OPTIONS = HANDLE_SHARING_OPTIONS.HSO_SHARE_NONE;
		if ((share & FileShare.Read) != 0)
		{
			hANDLE_SHARING_OPTIONS |= HANDLE_SHARING_OPTIONS.HSO_SHARE_READ;
		}
		if ((share & FileShare.Write) != 0)
		{
			hANDLE_SHARING_OPTIONS |= HANDLE_SHARING_OPTIONS.HSO_SHARE_WRITE;
		}
		if ((share & FileShare.Delete) != 0)
		{
			hANDLE_SHARING_OPTIONS |= HANDLE_SHARING_OPTIONS.HSO_SHARE_DELETE;
		}
		return hANDLE_SHARING_OPTIONS;
	}

	private static HANDLE_OPTIONS FileOptionsToHandleOptions(FileOptions options)
	{
		if ((options & FileOptions.Encrypted) != 0)
		{
			throw new NotSupportedException("NotSupported_Encrypted");
		}
		if (options != FileOptions.None && (options & (FileOptions)0x3FFFFFF) != 0)
		{
			throw new ArgumentOutOfRangeException("options", options, null);
		}
		return (HANDLE_OPTIONS)options;
	}

	private static HANDLE_CREATION_OPTIONS FileModeToCreationOptions(FileMode mode)
	{
		switch (mode)
		{
		default:
			throw new ArgumentOutOfRangeException("mode", mode, null);
		case FileMode.Append:
			return HANDLE_CREATION_OPTIONS.HCO_CREATE_ALWAYS;
		case FileMode.CreateNew:
		case FileMode.Create:
		case FileMode.Open:
		case FileMode.OpenOrCreate:
		case FileMode.Truncate:
			return (HANDLE_CREATION_OPTIONS)mode;
		}
	}

	[CLSCompliant(false)]
	[__DynamicallyInvokable]
	public static Task<Stream> OpenStreamForReadAsync(this IStorageFile windowsRuntimeFile)
	{
		if (windowsRuntimeFile == null)
		{
			throw new ArgumentNullException("windowsRuntimeFile");
		}
		return windowsRuntimeFile.OpenStreamForReadAsyncCore();
	}

	private static async Task<Stream> OpenStreamForReadAsyncCore(this IStorageFile windowsRuntimeFile)
	{
		try
		{
			return ((IInputStream)(object)(await windowsRuntimeFile.OpenAsync((FileAccessMode)0).AsTask<IRandomAccessStream>().ConfigureAwait(continueOnCapturedContext: false))).AsStreamForRead();
		}
		catch (Exception nativeException)
		{
			WinRtIOHelper.NativeExceptionToIOExceptionInfo(nativeException).Throw();
			return null;
		}
	}

	[CLSCompliant(false)]
	[__DynamicallyInvokable]
	public static Task<Stream> OpenStreamForWriteAsync(this IStorageFile windowsRuntimeFile)
	{
		if (windowsRuntimeFile == null)
		{
			throw new ArgumentNullException("windowsRuntimeFile");
		}
		return windowsRuntimeFile.OpenStreamForWriteAsyncCore(0L);
	}

	private static async Task<Stream> OpenStreamForWriteAsyncCore(this IStorageFile windowsRuntimeFile, long offset)
	{
		try
		{
			Stream stream = ((IOutputStream)(object)(await windowsRuntimeFile.OpenAsync((FileAccessMode)1).AsTask<IRandomAccessStream>().ConfigureAwait(continueOnCapturedContext: false))).AsStreamForWrite();
			stream.Position = offset;
			return stream;
		}
		catch (Exception nativeException)
		{
			WinRtIOHelper.NativeExceptionToIOExceptionInfo(nativeException).Throw();
			return null;
		}
	}

	[CLSCompliant(false)]
	[__DynamicallyInvokable]
	public static Task<Stream> OpenStreamForReadAsync(this IStorageFolder rootDirectory, string relativePath)
	{
		if (rootDirectory == null)
		{
			throw new ArgumentNullException("rootDirectory");
		}
		if (relativePath == null)
		{
			throw new ArgumentNullException("relativePath");
		}
		if (string.IsNullOrWhiteSpace(relativePath))
		{
			throw new ArgumentException(SR.GetString("Argument_RelativePathMayNotBeWhitespaceOnly"), "relativePath");
		}
		return rootDirectory.OpenStreamForReadAsyncCore(relativePath);
	}

	private static async Task<Stream> OpenStreamForReadAsyncCore(this IStorageFolder rootDirectory, string relativePath)
	{
		_ = 1;
		try
		{
			return await ((IStorageFile)(object)(await rootDirectory.GetFileAsync(relativePath).AsTask<StorageFile>().ConfigureAwait(continueOnCapturedContext: false))).OpenStreamForReadAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (Exception nativeException)
		{
			WinRtIOHelper.NativeExceptionToIOExceptionInfo(nativeException).Throw();
			return null;
		}
	}

	[CLSCompliant(false)]
	[__DynamicallyInvokable]
	public static Task<Stream> OpenStreamForWriteAsync(this IStorageFolder rootDirectory, string relativePath, CreationCollisionOption creationCollisionOption)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		if (rootDirectory == null)
		{
			throw new ArgumentNullException("rootDirectory");
		}
		if (relativePath == null)
		{
			throw new ArgumentNullException("relativePath");
		}
		if (string.IsNullOrWhiteSpace(relativePath))
		{
			throw new ArgumentException(SR.GetString("Argument_RelativePathMayNotBeWhitespaceOnly"), "relativePath");
		}
		return rootDirectory.OpenStreamForWriteAsyncCore(relativePath, creationCollisionOption);
	}

	private static async Task<Stream> OpenStreamForWriteAsyncCore(this IStorageFolder rootDirectory, string relativePath, CreationCollisionOption creationCollisionOption)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			IStorageFile windowsRuntimeFile = (IStorageFile)(object)(await rootDirectory.CreateFileAsync(relativePath, creationCollisionOption).AsTask<StorageFile>().ConfigureAwait(continueOnCapturedContext: false));
			long offset = 0L;
			if ((int)creationCollisionOption == 3)
			{
				ulong size = (await ((IStorageItem)windowsRuntimeFile).GetBasicPropertiesAsync().AsTask<BasicProperties>().ConfigureAwait(continueOnCapturedContext: false)).Size;
				offset = checked((long)size);
			}
			return await windowsRuntimeFile.OpenStreamForWriteAsyncCore(offset).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (Exception nativeException)
		{
			WinRtIOHelper.NativeExceptionToIOExceptionInfo(nativeException).Throw();
			return null;
		}
	}
}
