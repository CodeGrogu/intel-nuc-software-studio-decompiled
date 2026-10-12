using System.Security;

namespace System.Windows.Interop;

internal class ComponentDispatcherThread
{
	public bool IsThreadModal
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public MSG CurrentKeyboardMessage
	{
		[SecurityCritical]
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		[SecurityCritical]
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public event EventHandler ThreadIdle
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

	public event ThreadMessageEventHandler ThreadFilterMessage
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

	public event ThreadMessageEventHandler ThreadPreprocessMessage
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

	public event EventHandler EnterThreadModal
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

	public event EventHandler LeaveThreadModal
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

	[SecurityCritical]
	public void PushModal()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	public void PopModal()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	public void RaiseIdle()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	public bool RaiseThreadMessage(ref MSG msg)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	public void AddThreadPreprocessMessageHandlerFirst(ThreadMessageEventHandler handler)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	public void RemoveThreadPreprocessMessageHandlerFirst(ThreadMessageEventHandler handler)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public ComponentDispatcherThread()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
