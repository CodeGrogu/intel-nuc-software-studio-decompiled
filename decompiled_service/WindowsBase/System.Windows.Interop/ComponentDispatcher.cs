using System.Security;
using System.Security.Permissions;
using MS.Internal.WindowsBase;

namespace System.Windows.Interop;

/// <summary>Enables shared control of the message pump between Win32 and WPF in interoperation scenarios. </summary>
public static class ComponentDispatcher
{
	/// <summary>Gets a value that indicates whether the thread is modal. </summary>
	/// <returns>
	///     <see langword="true" /> if the thread is modal; otherwise, <see langword="false" />.</returns>
	public static bool IsThreadModal
	{
		[SecurityCritical]
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the last message that was raised. </summary>
	/// <returns>The last message.</returns>
	public static MSG CurrentKeyboardMessage
	{
		[SecurityCritical]
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal static MSG UnsecureCurrentKeyboardMessage
	{
		[FriendAccessAllowed]
		[SecurityCritical]
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		[FriendAccessAllowed]
		[SecurityCritical]
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Occurs when the thread is idle. </summary>
	public static event EventHandler ThreadIdle
	{
		[SecurityCritical]
		add
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		[SecurityCritical]
		remove
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Occurs when the message pump receives a keyboard message.  </summary>
	public static event ThreadMessageEventHandler ThreadFilterMessage
	{
		[SecurityCritical]
		add
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		[SecurityCritical]
		remove
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Occurs when the message pump receives a keyboard message.</summary>
	public static event ThreadMessageEventHandler ThreadPreprocessMessage
	{
		[SecurityCritical]
		[UIPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		add
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		[SecurityCritical]
		[UIPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		remove
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Occurs when a thread becomes modal. </summary>
	public static event EventHandler EnterThreadModal
	{
		[SecurityCritical]
		add
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		[SecurityCritical]
		remove
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Occurs when a modal thread becomes nonmodal. </summary>
	public static event EventHandler LeaveThreadModal
	{
		[SecurityCritical]
		add
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		[SecurityCritical]
		remove
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	static ComponentDispatcher()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Called to indicate that the thread is modal. </summary>
	[SecurityCritical]
	public static void PushModal()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal static void CriticalPushModal()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Called to indicate that a modal thread is no longer modal.</summary>
	[SecurityCritical]
	public static void PopModal()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal static void CriticalPopModal()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Called to indicate that a thread is idle. </summary>
	[SecurityCritical]
	[UIPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	public static void RaiseIdle()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Indicates that a new message is available for possible handling. </summary>
	/// <param name="msg">The message and its associated data.</param>
	/// <returns>
	///     <see langword="true" />, if one of the modules listening to the message loop processed the message. The owner of the message loop should ignore the message. <see langword="false" />, if the message was not processed. In this case, the owner of the message pump should call the Win32 function TranslateMessage followed by DispatchMessage. </returns>
	[SecurityCritical]
	[UIPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	public static bool RaiseThreadMessage(ref MSG msg)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal static void CriticalAddThreadPreprocessMessageHandlerFirst(ThreadMessageEventHandler handler)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal static void CriticalRemoveThreadPreprocessMessageHandlerFirst(ThreadMessageEventHandler handler)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
