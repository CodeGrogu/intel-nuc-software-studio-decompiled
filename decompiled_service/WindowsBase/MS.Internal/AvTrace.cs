using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security;
using MS.Internal.WindowsBase;

namespace MS.Internal;

internal class AvTrace
{
	public bool IsEnabled
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public bool SuppressGeneratedParameters
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

	public bool IsEnabledOverride
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public bool EnabledByDebugger
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

	public event AvTraceEventHandler TraceExtraMessages
	{
		[CompilerGenerated]
		add
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		[CompilerGenerated]
		remove
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public AvTrace(GetTraceSourceDelegate getTraceSourceDelegate, ClearTraceSourceDelegate clearTraceSourceDelegate)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public void Refresh()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public static void OnRefresh()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	[MS.Internal.WindowsBase.FriendAccessAllowed]
	internal static bool IsWpfTracingEnabledInRegistry()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static bool IsDebuggerAttached()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public void Trace(TraceEventType type, int eventId, string message, string[] labels, object[] parameters)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public void TraceStartStop(int eventID, string message, string[] labels, object[] parameters)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public static string ToStringHelper(object value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public static string AntiFormat(string s)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public static string TypeName(object value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public static int GetHashCodeHelper(object value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public static Type GetTypeHelper(object value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static AvTrace()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
