namespace System.Runtime.InteropServices;

[ComImport]
[Guid("00000003-0000-0000-c000-000000000046")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IMarshal
{
	void GetUnmarshalClass(ref Guid riid, IntPtr pv, uint dwDestContext, IntPtr pvDestContext, uint mshlFlags, ref Guid pCid);

	void GetMarshalSizeMax(ref Guid riid, IntPtr pv, uint dwDestContext, IntPtr pvDestContext, uint mshlflags, out uint pSize);

	void MarshalInterface(IntPtr pStm, ref Guid riid, IntPtr pv, uint dwDestContext, IntPtr pvDestContext, uint mshlflags);

	void UnmarshalInterface(IntPtr pStm, ref Guid riid, out IntPtr ppv);

	void ReleaseMarshalData(IntPtr pStm);

	void DisconnectObject(uint dwReserved);
}
