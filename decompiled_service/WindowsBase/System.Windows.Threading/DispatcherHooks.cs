using System.Security;
using System.Security.Permissions;

namespace System.Windows.Threading;

/// <summary>Provides additional event information about <see cref="T:System.Windows.Threading.Dispatcher" /> processing.</summary>
public sealed class DispatcherHooks
{
	/// <summary>Occurs when the dispatcher has no more operations to process. </summary>
	public event EventHandler DispatcherInactive
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

	/// <summary>Occurs when an operation is posted to the dispatcher. </summary>
	public event DispatcherHookEventHandler OperationPosted
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

	/// <summary>Occurs when an operation is invoked.</summary>
	public event DispatcherHookEventHandler OperationStarted
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

	/// <summary>Occurs when an operation completes. </summary>
	public event DispatcherHookEventHandler OperationCompleted
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

	/// <summary>Occurs when the priority of an operation is changed. </summary>
	public event DispatcherHookEventHandler OperationPriorityChanged
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

	/// <summary>Occurs when an operation is aborted. </summary>
	public event DispatcherHookEventHandler OperationAborted
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

	internal DispatcherHooks()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal void RaiseDispatcherInactive(Dispatcher dispatcher)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal void RaiseOperationPosted(Dispatcher dispatcher, DispatcherOperation operation)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal void RaiseOperationStarted(Dispatcher dispatcher, DispatcherOperation operation)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal void RaiseOperationCompleted(Dispatcher dispatcher, DispatcherOperation operation)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal void RaiseOperationPriorityChanged(Dispatcher dispatcher, DispatcherOperation operation)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal void RaiseOperationAborted(Dispatcher dispatcher, DispatcherOperation operation)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
