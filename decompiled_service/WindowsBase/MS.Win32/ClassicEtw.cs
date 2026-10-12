using System;
using System.Runtime.InteropServices;
using System.Security;
using MS.Utility;

namespace MS.Win32;

[SuppressUnmanagedCodeSecurity]
internal static class ClassicEtw
{
	internal struct TRACE_GUID_REGISTRATION
	{
		[SecurityCritical]
		internal unsafe Guid* Guid;

		[SecurityCritical]
		internal unsafe void* RegHandle;
	}

	internal struct WNODE_HEADER
	{
		public uint BufferSize;

		public uint ProviderId;

		public ulong HistoricalContext;

		public ulong TimeStamp;

		public Guid Guid;

		public uint ClientContext;

		public uint Flags;
	}

	internal enum WMIDPREQUESTCODE
	{
		GetAllData,
		GetSingleInstance,
		SetSingleInstance,
		SetSingleItem,
		EnableEvents,
		DisableEvents,
		EnableCollection,
		DisableCollection,
		RegInfo,
		ExecuteMethod
	}

	[SecurityCritical]
	internal unsafe delegate uint ControlCallback(WMIDPREQUESTCODE requestCode, IntPtr requestContext, IntPtr reserved, WNODE_HEADER* data);

	internal struct EVENT_TRACE_HEADER
	{
		public ushort Size;

		public ushort FieldTypeFlags;

		public byte Type;

		public byte Level;

		public ushort Version;

		public int ThreadId;

		public int ProcessId;

		public long TimeStamp;

		public Guid Guid;

		public uint ClientContext;

		public uint Flags;
	}

	[StructLayout(LayoutKind.Explicit, Size = 304)]
	internal struct EVENT_HEADER
	{
		[FieldOffset(0)]
		public EVENT_TRACE_HEADER Header;

		[FieldOffset(48)]
		public EventData Data;
	}

	internal const int WNODE_FLAG_TRACED_GUID = 131072;

	internal const int WNODE_FLAG_USE_MOF_PTR = 1048576;

	internal const int MAX_MOF_FIELDS = 16;

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	internal static extern uint RegisterTraceGuidsW([In] ControlCallback cbFunc, [In] IntPtr context, [In] ref Guid providerGuid, [In] int taskGuidCount, [In][Out] ref TRACE_GUID_REGISTRATION taskGuids, [In] string mofImagePath, [In] string mofResourceName, out ulong regHandle);

	[DllImport("advapi32.dll")]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	internal static extern uint UnregisterTraceGuids(ulong regHandle);

	[DllImport("advapi32.dll")]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	internal static extern int GetTraceEnableFlags(ulong traceHandle);

	[DllImport("advapi32.dll")]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	internal static extern byte GetTraceEnableLevel(ulong traceHandle);

	[DllImport("advapi32.dll")]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	internal unsafe static extern long GetTraceLoggerHandle(WNODE_HEADER* data);

	[DllImport("advapi32.dll")]
	[SecurityCritical]
	[SuppressUnmanagedCodeSecurity]
	internal unsafe static extern uint TraceEvent(ulong traceHandle, EVENT_HEADER* header);
}
