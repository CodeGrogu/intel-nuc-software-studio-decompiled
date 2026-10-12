using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;

namespace MS.Internal.IO.Packaging.CompoundFile;

[SecurityCritical(SecurityCriticalScope.Everything)]
internal static class UnsafeNativeCompoundFileMethods
{
	internal class UnsafeLockBytesOnStream : UnsafeNativeILockBytes, IDisposable
	{
		internal UnsafeLockBytesOnStream(Stream underlyingStream)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		public void Dispose()
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		protected virtual void Dispose(bool disposing)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		void UnsafeNativeILockBytes.ReadAt(ulong offset, [Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] pv, int cb, out int pcbRead)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		void UnsafeNativeILockBytes.WriteAt(ulong offset, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] pv, int cb, out int pcbWritten)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		void UnsafeNativeILockBytes.Flush()
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		void UnsafeNativeILockBytes.SetSize(ulong cb)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		void UnsafeNativeILockBytes.LockRegion(ulong libOffset, ulong cb, int dwLockType)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		void UnsafeNativeILockBytes.UnlockRegion(ulong libOffset, ulong cb, int dwLockType)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		void UnsafeNativeILockBytes.Stat(out System.Runtime.InteropServices.ComTypes.STATSTG pstatstg, int grfStatFlag)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	[ComImport]
	[Guid("0000000a-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface UnsafeNativeILockBytes
	{
		void ReadAt(ulong offset, [Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] pv, int cb, out int pcbRead);

		void WriteAt(ulong offset, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] pv, int cb, out int pcbWritten);

		void Flush();

		void SetSize(ulong cb);

		void LockRegion(ulong libOffset, ulong cb, int dwLockType);

		void UnlockRegion(ulong libOffset, ulong cb, int dwLockType);

		void Stat(out System.Runtime.InteropServices.ComTypes.STATSTG pstatstg, int grfStatFlag);
	}

	[ComImport]
	[Guid("0000000b-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[SuppressUnmanagedCodeSecurity]
	internal interface UnsafeNativeIStorage
	{
		[PreserveSig]
		int CreateStream([In][MarshalAs(UnmanagedType.LPWStr)] string pwcsName, int grfMode, int reserved1, int reserved2, out UnsafeNativeIStream ppstm);

		[PreserveSig]
		int OpenStream([In][MarshalAs(UnmanagedType.LPWStr)] string pwcsName, int reserved1, int grfMode, int reserved2, out UnsafeNativeIStream ppstm);

		[PreserveSig]
		int CreateStorage([In][MarshalAs(UnmanagedType.LPWStr)] string pwcsName, int grfMode, int reserved1, int reserved2, out UnsafeNativeIStorage ppstg);

		[PreserveSig]
		int OpenStorage([In][MarshalAs(UnmanagedType.LPWStr)] string pwcsName, UnsafeNativeIStorage pstgPriority, int grfMode, IntPtr snbExclude, int reserved, out UnsafeNativeIStorage ppstg);

		void CopyTo(int ciidExclude, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] Guid[] rgiidExclude, IntPtr snbExclude, UnsafeNativeIStorage ppstg);

		void MoveElementTo([In][MarshalAs(UnmanagedType.LPWStr)] string pwcsName, UnsafeNativeIStorage pstgDest, [In][MarshalAs(UnmanagedType.LPWStr)] string pwcsNewName, int grfFlags);

		void Commit(int grfCommitFlags);

		void Revert();

		void EnumElements(int reserved1, IntPtr reserved2, int reserved3, out UnsafeNativeIEnumSTATSTG ppEnum);

		void DestroyElement([In][MarshalAs(UnmanagedType.LPWStr)] string pwcsName);

		void RenameElement([In][MarshalAs(UnmanagedType.LPWStr)] string pwcsOldName, [In][MarshalAs(UnmanagedType.LPWStr)] string pwcsNewName);

		void SetElementTimes([In][MarshalAs(UnmanagedType.LPWStr)] string pwcsName, System.Runtime.InteropServices.ComTypes.FILETIME pctime, System.Runtime.InteropServices.ComTypes.FILETIME patime, System.Runtime.InteropServices.ComTypes.FILETIME pmtime);

		void SetClass(ref Guid clsid);

		void SetStateBits(int grfStateBits, int grfMask);

		void Stat(out System.Runtime.InteropServices.ComTypes.STATSTG pstatstg, int grfStatFlag);
	}

	[ComImport]
	[Guid("0000000c-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[SuppressUnmanagedCodeSecurity]
	internal interface UnsafeNativeIStream
	{
		void Read([Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] pv, int cb, out int pcbRead);

		void Write([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] pv, int cb, out int pcbWritten);

		void Seek(long dlibMove, int dwOrigin, out long plibNewPosition);

		void SetSize(long libNewSize);

		void CopyTo(UnsafeNativeIStream pstm, long cb, out long pcbRead, out long pcbWritten);

		void Commit(int grfCommitFlags);

		void Revert();

		void LockRegion(long libOffset, long cb, int dwLockType);

		void UnlockRegion(long libOffset, long cb, int dwLockType);

		void Stat(out System.Runtime.InteropServices.ComTypes.STATSTG pstatstg, int grfStatFlag);

		void Clone(out UnsafeNativeIStream ppstm);
	}

	[ComImport]
	[Guid("0000013A-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[SuppressUnmanagedCodeSecurity]
	internal interface UnsafeNativeIPropertySetStorage
	{
		void Create(ref Guid rfmtid, ref Guid pclsid, uint grfFlags, uint grfMode, out UnsafeNativeIPropertyStorage ppprstg);

		[PreserveSig]
		int Open(ref Guid rfmtid, uint grfMode, out UnsafeNativeIPropertyStorage ppprstg);

		void Delete(ref Guid rfmtid);

		void Enum(out UnsafeNativeIEnumSTATPROPSETSTG ppenum);
	}

	[ComImport]
	[Guid("0000013B-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[SuppressUnmanagedCodeSecurity]
	internal interface UnsafeNativeIEnumSTATPROPSETSTG
	{
		[PreserveSig]
		int Next(uint celt, [Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] STATPROPSETSTG rgelt, out uint pceltFetched);

		void Skip(uint celt);

		void Reset();

		void Clone(out UnsafeNativeIEnumSTATPROPSETSTG ppenum);
	}

	[ComImport]
	[Guid("00000138-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[SuppressUnmanagedCodeSecurity]
	internal interface UnsafeNativeIPropertyStorage
	{
		[PreserveSig]
		int ReadMultiple(uint cpspec, [In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] PROPSPEC[] rgpspec, [Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] PROPVARIANT[] rgpropvar);

		void WriteMultiple(uint cpspec, [In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] PROPSPEC[] rgpspec, [In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] PROPVARIANT[] rgpropvar, uint propidNameFirst);

		void DeleteMultiple(uint cpspec, [In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] PROPSPEC[] rgpspec);

		void ReadPropertyNames(uint cpropid, [In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] uint[] rgpropid, [Out][MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 0)] string[] rglpwstrName);

		void WritePropertyNames(uint cpropid, [In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] uint[] rgpropid, [In][MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 0)] string[] rglpwstrName);

		void DeletePropertyNames(uint cpropid, [In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] uint[] rgpropid);

		void Commit(uint grfCommitFlags);

		void Revert();

		void Enum(out UnsafeNativeIEnumSTATPROPSTG ppenum);

		void SetTimes(ref System.Runtime.InteropServices.ComTypes.FILETIME pctime, ref System.Runtime.InteropServices.ComTypes.FILETIME patime, ref System.Runtime.InteropServices.ComTypes.FILETIME pmtime);

		void SetClass(ref Guid clsid);

		void Stat(out STATPROPSETSTG pstatpsstg);
	}

	[ComImport]
	[Guid("00000139-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[SuppressUnmanagedCodeSecurity]
	internal interface UnsafeNativeIEnumSTATPROPSTG
	{
		[PreserveSig]
		int Next(uint celt, [Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] STATPROPSTG rgelt, out uint pceltFetched);

		void Skip(uint celt);

		void Reset();

		void Clone(out UnsafeNativeIEnumSTATPROPSTG ppenum);
	}

	[ComImport]
	[Guid("0000000d-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[SuppressUnmanagedCodeSecurity]
	internal interface UnsafeNativeIEnumSTATSTG
	{
		void Next(uint celt, out System.Runtime.InteropServices.ComTypes.STATSTG rgelt, out uint pceltFetched);

		void Skip(uint celt);

		void Reset();

		void Clone(out UnsafeNativeIEnumSTATSTG ppenum);
	}

	[DllImport("ole32.dll")]
	[SuppressUnmanagedCodeSecurity]
	internal static extern int StgCreateDocfileOnILockBytes(UnsafeNativeILockBytes plkbyt, int grfMode, int reserved, out UnsafeNativeIStorage ppstgOpen);

	[DllImport("ole32.dll")]
	[SuppressUnmanagedCodeSecurity]
	internal static extern int StgOpenStorageOnILockBytes(UnsafeNativeILockBytes plkbyt, UnsafeNativeIStorage pStgPriority, int grfMode, IntPtr snbExclude, int reserved, out UnsafeNativeIStorage ppstgOpen);

	[DllImport("ole32.dll")]
	[SuppressUnmanagedCodeSecurity]
	internal static extern int StgCreateStorageEx([In][MarshalAs(UnmanagedType.LPWStr)] string pwcsName, int grfMode, int stgfmt, int grfAttrs, IntPtr pStgOptions, IntPtr reserved2, ref Guid riid, out UnsafeNativeIStorage ppObjectOpen);

	[DllImport("ole32.dll")]
	[SuppressUnmanagedCodeSecurity]
	internal static extern int StgOpenStorageEx([In][MarshalAs(UnmanagedType.LPWStr)] string pwcsName, int grfMode, int stgfmt, int grfAttrs, IntPtr pStgOptions, IntPtr reserved2, ref Guid riid, out UnsafeNativeIStorage ppObjectOpen);

	[DllImport("ole32.dll")]
	[SuppressUnmanagedCodeSecurity]
	internal static extern int PropVariantClear(ref PROPVARIANT pvar);
}
