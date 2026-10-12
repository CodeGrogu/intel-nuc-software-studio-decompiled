using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using MS.Internal.WindowsBase;

namespace System.Windows.Threading;

/// <summary>Provides services for managing the queue of work items for a thread.</summary>
public sealed class Dispatcher
{
	internal delegate void ShutdownCallback();

	internal bool _exitAllFrames;

	internal bool _hasShutdownStarted;

	internal int _disableProcessingCount;

	internal DispatcherSynchronizationContext _defaultDispatcherSynchronizationContext;

	internal object _instanceLock;

	/// <summary>Gets the <see cref="T:System.Windows.Threading.Dispatcher" /> for the thread currently executing and creates a new <see cref="T:System.Windows.Threading.Dispatcher" /> if one is not already associated with the thread. </summary>
	/// <returns>The dispatcher associated with the current thread.</returns>
	public static Dispatcher CurrentDispatcher
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the thread this <see cref="T:System.Windows.Threading.Dispatcher" /> is associated with.</summary>
	/// <returns>The thread.</returns>
	public Thread Thread
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Determines whether the <see cref="T:System.Windows.Threading.Dispatcher" /> is shutting down. </summary>
	/// <returns>
	///     <see langword="true" /> if the <see cref="T:System.Windows.Threading.Dispatcher" /> has started shutting down; otherwise, <see langword="false" />.</returns>
	public bool HasShutdownStarted
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Determines whether the <see cref="T:System.Windows.Threading.Dispatcher" /> has finished shutting down.</summary>
	/// <returns>
	///     <see langword="true" /> if the dispatcher has finished shutting down; otherwise, <see langword="false" />.</returns>
	public bool HasShutdownFinished
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the collection of hooks that provide additional event information about the <see cref="T:System.Windows.Threading.Dispatcher" />. </summary>
	/// <returns>The hooks associated with this <see cref="T:System.Windows.Threading.Dispatcher" />. </returns>
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public DispatcherHooks Hooks
	{
		[SecurityCritical]
		[UIPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal object Reserved0
	{
		[MS.Internal.WindowsBase.FriendAccessAllowed]
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		[MS.Internal.WindowsBase.FriendAccessAllowed]
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal object Reserved1
	{
		[MS.Internal.WindowsBase.FriendAccessAllowed]
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		[MS.Internal.WindowsBase.FriendAccessAllowed]
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal object Reserved2
	{
		[MS.Internal.WindowsBase.FriendAccessAllowed]
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		[MS.Internal.WindowsBase.FriendAccessAllowed]
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal object Reserved3
	{
		[MS.Internal.WindowsBase.FriendAccessAllowed]
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		[MS.Internal.WindowsBase.FriendAccessAllowed]
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal object Reserved4
	{
		[MS.Internal.WindowsBase.FriendAccessAllowed]
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		[MS.Internal.WindowsBase.FriendAccessAllowed]
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal object PtsCache
	{
		[MS.Internal.WindowsBase.FriendAccessAllowed]
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		[MS.Internal.WindowsBase.FriendAccessAllowed]
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal object InputMethod
	{
		[MS.Internal.WindowsBase.FriendAccessAllowed]
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		[MS.Internal.WindowsBase.FriendAccessAllowed]
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal object InputManager
	{
		[MS.Internal.WindowsBase.FriendAccessAllowed]
		[SecurityCritical]
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		[MS.Internal.WindowsBase.FriendAccessAllowed]
		[SecurityCritical]
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	[MS.Internal.WindowsBase.FriendAccessAllowed]
	internal bool IsTSFMessagePumpEnabled
	{
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Occurs when the <see cref="T:System.Windows.Threading.Dispatcher" /> begins to shut down. </summary>
	public event EventHandler ShutdownStarted
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

	/// <summary>Occurs when the <see cref="T:System.Windows.Threading.Dispatcher" /> finishes shutting down. </summary>
	public event EventHandler ShutdownFinished
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

	/// <summary>Occurs when a thread exception is thrown and uncaught during execution of a delegate by way of <see cref="Overload:System.Windows.Threading.Dispatcher.Invoke" /> or <see cref="Overload:System.Windows.Threading.Dispatcher.BeginInvoke" /> when in the filter stage. </summary>
	public event DispatcherUnhandledExceptionFilterEventHandler UnhandledExceptionFilter
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

	/// <summary>Occurs when a thread exception is thrown and uncaught during execution of a delegate by way of <see cref="Overload:System.Windows.Threading.Dispatcher.Invoke" /> or <see cref="Overload:System.Windows.Threading.Dispatcher.BeginInvoke" />.</summary>
	public event DispatcherUnhandledExceptionEventHandler UnhandledException
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

	[SecurityCritical]
	[SecurityTreatAsSafe]
	static Dispatcher()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Gets the <see cref="T:System.Windows.Threading.Dispatcher" /> for the specified thread. </summary>
	/// <param name="thread">The thread to obtain the <see cref="T:System.Windows.Threading.Dispatcher" /> from.</param>
	/// <returns>The dispatcher for <paramref name="thread" />.</returns>
	public static Dispatcher FromThread(Thread thread)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Determines whether the calling thread is the thread associated with this <see cref="T:System.Windows.Threading.Dispatcher" />. </summary>
	/// <returns>
	///     <see langword="true" /> if the calling thread is the thread associated with this <see cref="T:System.Windows.Threading.Dispatcher" />; otherwise, <see langword="false" />.</returns>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool CheckAccess()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Determines whether the calling thread has access to this <see cref="T:System.Windows.Threading.Dispatcher" />. </summary>
	/// <exception cref="T:System.InvalidOperationException">The calling thread does not have access to this <see cref="T:System.Windows.Threading.Dispatcher" />.</exception>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void VerifyAccess()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initiates shutdown of the <see cref="T:System.Windows.Threading.Dispatcher" /> asynchronously. </summary>
	/// <param name="priority">The priority at which to begin shutting down the dispatcher.</param>
	[SecurityCritical]
	public void BeginInvokeShutdown(DispatcherPriority priority)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initiates the shutdown process of the <see cref="T:System.Windows.Threading.Dispatcher" /> synchronously.</summary>
	[SecurityCritical]
	public void InvokeShutdown()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[MS.Internal.WindowsBase.FriendAccessAllowed]
	internal void CriticalInvokeShutdown()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Pushes the main execution frame on the event queue of the <see cref="T:System.Windows.Threading.Dispatcher" />. </summary>
	[SecurityCritical]
	[UIPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	public static void Run()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Enters an execute loop.</summary>
	/// <param name="frame">The frame for the dispatcher to process.</param>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="frame" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.InvalidOperationException">
	///         <see cref="P:System.Windows.Threading.Dispatcher.HasShutdownFinished" /> is <see langword="true" />-or-
	///         <paramref name="frame" /> is running on a different <see cref="T:System.Windows.Threading.Dispatcher" />.-or-Dispatcher processing has been disabled.</exception>
	[SecurityCritical]
	[UIPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	public static void PushFrame(DispatcherFrame frame)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Requests that all frames exit, including nested frames.</summary>
	[SecurityCritical]
	public static void ExitAllFrames()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates an awaitable object that asynchronously yields control back to the current dispatcher and provides an opportunity for the dispatcher to process other events.</summary>
	/// <returns>An awaitable object that asynchronously yields control back to the current dispatcher and provides an opportunity for the dispatcher to process other events.</returns>
	public static DispatcherPriorityAwaitable Yield()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates an awaitable object that asynchronously yields control back to the current dispatcher and provides an opportunity for the dispatcher to process other events.  The work that occurs when control returns to the code awaiting the result of this method is scheduled with the specified priority.</summary>
	/// <param name="priority">The priority at which to schedule the continuation.</param>
	/// <returns>An awaitable object that asynchronously yields control back to the current dispatcher and provides an opportunity for the dispatcher to process other events.</returns>
	public static DispatcherPriorityAwaitable Yield(DispatcherPriority priority)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Executes the specified delegate asynchronously at the specified priority on the thread the <see cref="T:System.Windows.Threading.Dispatcher" /> is associated with.</summary>
	/// <param name="priority">The priority, relative to the other pending operations in the <see cref="T:System.Windows.Threading.Dispatcher" /> event queue, the specified method is invoked.</param>
	/// <param name="method">The delegate to a method that takes no arguments, which is pushed onto the <see cref="T:System.Windows.Threading.Dispatcher" /> event queue.</param>
	/// <returns>An object, which is returned immediately after <see cref="Overload:System.Windows.Threading.Dispatcher.BeginInvoke" /> is called, that can be used to interact with the delegate as it is pending execution in the event queue.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="method" /> is <see langword="null" />. </exception>
	/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
	///         <paramref name="priority" /> is not a valid <see cref="T:System.Windows.Threading.DispatcherPriority" />.</exception>
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public DispatcherOperation BeginInvoke(DispatcherPriority priority, Delegate method)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Executes the specified delegate asynchronously at the specified priority and with the specified argument on the thread the <see cref="T:System.Windows.Threading.Dispatcher" /> is associated with.</summary>
	/// <param name="priority">The priority, relative to the other pending operations in the <see cref="T:System.Windows.Threading.Dispatcher" /> event queue, the specified method is invoked.</param>
	/// <param name="method">A delegate to a method that takes one argument, which is pushed onto the <see cref="T:System.Windows.Threading.Dispatcher" /> event queue.</param>
	/// <param name="arg">The object to pass as an argument to the specified method.</param>
	/// <returns>An object, which is returned immediately after <see cref="Overload:System.Windows.Threading.Dispatcher.BeginInvoke" /> is called, that can be used to interact with the delegate as it is pending execution in the event queue.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="method" /> is <see langword="null" />. </exception>
	/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
	///         <paramref name="priority" /> is not a valid <see cref="T:System.Windows.Threading.DispatcherPriority" />.</exception>
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public DispatcherOperation BeginInvoke(DispatcherPriority priority, Delegate method, object arg)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Executes the specified delegate asynchronously at the specified priority and with the specified array of arguments on the thread the <see cref="T:System.Windows.Threading.Dispatcher" /> is associated with.</summary>
	/// <param name="priority">The priority, relative to the other pending operations in the <see cref="T:System.Windows.Threading.Dispatcher" /> event queue, the specified method is invoked.</param>
	/// <param name="method">A delegate to a method that takes multiple arguments, which is pushed onto the <see cref="T:System.Windows.Threading.Dispatcher" /> event queue.</param>
	/// <param name="arg">The object to pass as an argument to the specified method.</param>
	/// <param name="args">An array of objects to pass as arguments to the specified method.</param>
	/// <returns>An object, which is returned immediately after <see cref="Overload:System.Windows.Threading.Dispatcher.BeginInvoke" /> is called, that can be used to interact with the delegate as it is pending execution in the <see cref="T:System.Windows.Threading.Dispatcher" /> queue.</returns>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="method" /> is <see langword="null" />. </exception>
	/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
	///         <see cref="T:System.Windows.Threading.DispatcherPriority" /> is not a valid priority.</exception>
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public DispatcherOperation BeginInvoke(DispatcherPriority priority, Delegate method, object arg, params object[] args)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Executes the specified delegate asynchronously with the specified arguments on the thread that the <see cref="T:System.Windows.Threading.Dispatcher" /> was created on.</summary>
	/// <param name="method">The delegate to a method that takes parameters specified in <paramref name="args" />, which is pushed onto the <see cref="T:System.Windows.Threading.Dispatcher" /> event queue.</param>
	/// <param name="args">An array of objects to pass as arguments to the given method. Can be <see langword="null" />.</param>
	/// <returns>An object, which is returned immediately after <see cref="Overload:System.Windows.Threading.Dispatcher.BeginInvoke" /> is called, that can be used to interact with the delegate as it is pending execution in the event queue.</returns>
	public DispatcherOperation BeginInvoke(Delegate method, params object[] args)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Executes the specified delegate asynchronously with the specified arguments, at the specified priority, on the thread that the <see cref="T:System.Windows.Threading.Dispatcher" /> was created on.</summary>
	/// <param name="method">The delegate to a method that takes parameters specified in <paramref name="args" />, which is pushed onto the <see cref="T:System.Windows.Threading.Dispatcher" /> event queue.</param>
	/// <param name="priority">The priority, relative to the other pending operations in the <see cref="T:System.Windows.Threading.Dispatcher" /> event queue, the specified method is invoked.</param>
	/// <param name="args">An array of objects to pass as arguments to the given method. Can be <see langword="null" />.</param>
	/// <returns>An object, which is returned immediately after <see cref="Overload:System.Windows.Threading.Dispatcher.BeginInvoke" /> is called, that can be used to interact with the delegate as it is pending execution in the event queue.</returns>
	public DispatcherOperation BeginInvoke(Delegate method, DispatcherPriority priority, params object[] args)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Executes the specified <see cref="T:System.Action" /> synchronously on the thread the <see cref="T:System.Windows.Threading.Dispatcher" /> is associated with.</summary>
	/// <param name="callback">A delegate to invoke through the dispatcher.</param>
	public void Invoke(Action callback)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Executes the specified <see cref="T:System.Action" /> synchronously at the specified priority on the thread the <see cref="T:System.Windows.Threading.Dispatcher" /> is associated with.</summary>
	/// <param name="callback">A delegate to invoke through the dispatcher.</param>
	/// <param name="priority">The priority that determines in what order the specified callback is invoked relative to the other pending operations in the <see cref="T:System.Windows.Threading.Dispatcher" />.</param>
	public void Invoke(Action callback, DispatcherPriority priority)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Executes the specified <see cref="T:System.Action" /> synchronously at the specified priority on the thread the <see cref="T:System.Windows.Threading.Dispatcher" /> is associated with.</summary>
	/// <param name="callback">A delegate to invoke through the dispatcher.</param>
	/// <param name="priority">The priority that determines in what order the specified callback is invoked relative to the other pending operations in the <see cref="T:System.Windows.Threading.Dispatcher" />.</param>
	/// <param name="cancellationToken">An object that indicates whether to cancel the action.</param>
	public void Invoke(Action callback, DispatcherPriority priority, CancellationToken cancellationToken)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Executes the specified <see cref="T:System.Action" /> synchronously at the specified priority on the thread the <see cref="T:System.Windows.Threading.Dispatcher" /> is associated with.</summary>
	/// <param name="callback">A delegate to invoke through the dispatcher.</param>
	/// <param name="priority">The priority that determines in what order the specified callback is invoked relative to the other pending operations in the <see cref="T:System.Windows.Threading.Dispatcher" />.</param>
	/// <param name="cancellationToken">An object that indicates whether to cancel the action.</param>
	/// <param name="timeout">The minimum amount of time to wait for the operation to start.</param>
	[SecuritySafeCritical]
	public void Invoke(Action callback, DispatcherPriority priority, CancellationToken cancellationToken, TimeSpan timeout)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Executes the specified <see cref="T:System.Func`1" /> synchronously on the thread the <see cref="T:System.Windows.Threading.Dispatcher" /> is associated with.</summary>
	/// <param name="callback">A delegate to invoke through the dispatcher.</param>
	/// <typeparam name="TResult">The return value type of the specified delegate.</typeparam>
	public TResult Invoke<TResult>(Func<TResult> callback)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Executes the specified <see cref="T:System.Func`1" /> synchronously at the specified priority on the thread the <see cref="T:System.Windows.Threading.Dispatcher" /> is associated with.</summary>
	/// <param name="callback">A delegate to invoke through the dispatcher.</param>
	/// <param name="priority">The priority that determines in what order the specified callback is invoked relative to the other pending operations in the <see cref="T:System.Windows.Threading.Dispatcher" />.</param>
	/// <typeparam name="TResult">The return value type of the specified delegate.</typeparam>
	public TResult Invoke<TResult>(Func<TResult> callback, DispatcherPriority priority)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Executes the specified <see cref="T:System.Func`1" /> synchronously at the specified priority on the thread the <see cref="T:System.Windows.Threading.Dispatcher" /> is associated with.</summary>
	/// <param name="callback">A delegate to invoke through the dispatcher.</param>
	/// <param name="priority">The priority that determines in what order the specified callback is invoked relative to the other pending operations in the <see cref="T:System.Windows.Threading.Dispatcher" />.</param>
	/// <param name="cancellationToken">An object that indicates whether to cancel the operation.</param>
	/// <typeparam name="TResult">The return value type of the specified delegate.</typeparam>
	public TResult Invoke<TResult>(Func<TResult> callback, DispatcherPriority priority, CancellationToken cancellationToken)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Executes the specified <see cref="T:System.Func`1" /> synchronously at the specified priority on the thread the <see cref="T:System.Windows.Threading.Dispatcher" /> is associated with.</summary>
	/// <param name="callback">A delegate to invoke through the dispatcher.</param>
	/// <param name="priority">The priority that determines in what order the specified callback is invoked relative to the other pending operations in the <see cref="T:System.Windows.Threading.Dispatcher" />.</param>
	/// <param name="cancellationToken">An object that indicates whether to cancel the operation.</param>
	/// <param name="timeout">The minimum amount of time to wait for the operation to start.</param>
	/// <typeparam name="TResult">The return value type of the specified delegate.</typeparam>
	[SecuritySafeCritical]
	public TResult Invoke<TResult>(Func<TResult> callback, DispatcherPriority priority, CancellationToken cancellationToken, TimeSpan timeout)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Executes the specified <see cref="T:System.Action" /> asynchronously on the thread the <see cref="T:System.Windows.Threading.Dispatcher" /> is associated with.</summary>
	/// <param name="callback">A delegate to invoke through the dispatcher.</param>
	/// <returns>An object, which is returned immediately after <see cref="M:System.Windows.Threading.Dispatcher.InvokeAsync(System.Action)" /> is called, that can be used to interact with the delegate as it is pending execution in the event queue.</returns>
	public DispatcherOperation InvokeAsync(Action callback)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Executes the specified <see cref="T:System.Action" /> asynchronously at the specified priority on the thread the <see cref="T:System.Windows.Threading.Dispatcher" /> is associated with.</summary>
	/// <param name="callback">A delegate to invoke through the dispatcher.</param>
	/// <param name="priority">The priority that determines in what order the specified callback is invoked relative to the other pending operations in the <see cref="T:System.Windows.Threading.Dispatcher" />.</param>
	/// <returns>An object, which is returned immediately after <see cref="M:System.Windows.Threading.Dispatcher.InvokeAsync(System.Action,System.Windows.Threading.DispatcherPriority)" /> is called, that can be used to interact with the delegate as it is pending execution in the event queue.</returns>
	public DispatcherOperation InvokeAsync(Action callback, DispatcherPriority priority)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Executes the specified <see cref="T:System.Action" /> asynchronously at the specified priority on the thread the <see cref="T:System.Windows.Threading.Dispatcher" /> is associated with.</summary>
	/// <param name="callback">A delegate to invoke through the dispatcher.</param>
	/// <param name="priority">The priority that determines in what order the specified callback is invoked relative to the other pending operations in the <see cref="T:System.Windows.Threading.Dispatcher" />.</param>
	/// <param name="cancellationToken">An object that indicates whether to cancel the action.</param>
	/// <returns>An object, which is returned immediately after <see cref="M:System.Windows.Threading.Dispatcher.InvokeAsync(System.Action,System.Windows.Threading.DispatcherPriority,System.Threading.CancellationToken)" /> is called, that can be used to interact with the delegate as it is pending execution in the event queue.</returns>
	[SecuritySafeCritical]
	public DispatcherOperation InvokeAsync(Action callback, DispatcherPriority priority, CancellationToken cancellationToken)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Executes the specified <see cref="T:System.Func`1" /> asynchronously on the thread the <see cref="T:System.Windows.Threading.Dispatcher" /> is associated with.</summary>
	/// <param name="callback">A delegate to invoke through the dispatcher.</param>
	/// <typeparam name="TResult">The return value type of the specified delegate.</typeparam>
	/// <returns>An object, which is returned immediately after <see cref="M:System.Windows.Threading.Dispatcher.InvokeAsync``1(System.Func{``0})" /> is called, that can be used to interact with the delegate as it is pending execution in the event queue.</returns>
	public DispatcherOperation<TResult> InvokeAsync<TResult>(Func<TResult> callback)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Executes the specified <see cref="T:System.Func`1" /> asynchronously at the specified priority on the thread the <see cref="T:System.Windows.Threading.Dispatcher" /> is associated with.</summary>
	/// <param name="callback">A delegate to invoke through the dispatcher.</param>
	/// <param name="priority">The priority that determines in what order the specified callback is invoked relative to the other pending operations in the <see cref="T:System.Windows.Threading.Dispatcher" />.</param>
	/// <typeparam name="TResult">The return value type of the specified delegate.</typeparam>
	/// <returns>An object, which is returned immediately after <see cref="M:System.Windows.Threading.Dispatcher.InvokeAsync``1(System.Func{``0},System.Windows.Threading.DispatcherPriority)" /> is called, that can be used to interact with the delegate as it is pending execution in the event queue.</returns>
	public DispatcherOperation<TResult> InvokeAsync<TResult>(Func<TResult> callback, DispatcherPriority priority)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Executes the specified <see cref="T:System.Func`1" /> synchronously at the specified priority on the thread the <see cref="T:System.Windows.Threading.Dispatcher" /> is associated with.</summary>
	/// <param name="callback">A delegate to invoke through the dispatcher.</param>
	/// <param name="priority">The priority that determines in what order the specified callback is invoked relative to the other pending operations in the <see cref="T:System.Windows.Threading.Dispatcher" />.</param>
	/// <param name="cancellationToken">An object that indicates whether to cancel the operation.</param>
	/// <typeparam name="TResult">The return value type of the specified delegate.</typeparam>
	/// <returns>An object, which is returned immediately after <see cref="M:System.Windows.Threading.Dispatcher.InvokeAsync``1(System.Func{``0},System.Windows.Threading.DispatcherPriority,System.Threading.CancellationToken)" /> is called, that can be used to interact with the delegate as it is pending execution in the event queue.</returns>
	[SecuritySafeCritical]
	public DispatcherOperation<TResult> InvokeAsync<TResult>(Func<TResult> callback, DispatcherPriority priority, CancellationToken cancellationToken)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Executes the specified delegate synchronously at the specified priority on the thread on which the <see cref="T:System.Windows.Threading.Dispatcher" /> is associated with. </summary>
	/// <param name="priority">The priority, relative to the other pending operations in the <see cref="T:System.Windows.Threading.Dispatcher" /> event queue, the specified method is invoked.</param>
	/// <param name="method">A delegate to a method that takes no arguments, which is pushed onto the <see cref="T:System.Windows.Threading.Dispatcher" /> event queue.</param>
	/// <returns>The return value from the delegate being invoked or <see langword="null" /> if the delegate has no return value.</returns>
	/// <exception cref="T:System.ArgumentException">
	///         <paramref name="priority" /> is equal to <see cref="F:System.Windows.Threading.DispatcherPriority.Inactive" />.</exception>
	/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
	///         <paramref name="priority" /> is not a valid priority.</exception>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="method" /> is <see langword="null" />. </exception>
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public object Invoke(DispatcherPriority priority, Delegate method)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Executes the specified delegate at the specified priority with the specified argument synchronously on the thread the <see cref="T:System.Windows.Threading.Dispatcher" /> is associated with.</summary>
	/// <param name="priority">The priority, relative to the other pending operations in the <see cref="T:System.Windows.Threading.Dispatcher" /> event queue, the specified method is invoked.</param>
	/// <param name="method">A delegate to a method that takes one argument, which is pushed onto the <see cref="T:System.Windows.Threading.Dispatcher" /> event queue.</param>
	/// <param name="arg">An object to pass as an argument to the given method.</param>
	/// <returns>The return value from the delegate being invoked or <see langword="null" /> if the delegate has no return value.</returns>
	/// <exception cref="T:System.ArgumentException">
	///         <paramref name="priority" /> is equal to <see cref="F:System.Windows.Threading.DispatcherPriority.Inactive" />.</exception>
	/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
	///         <paramref name="priority" /> is not a valid priority.</exception>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="method" /> is <see langword="null" />. </exception>
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public object Invoke(DispatcherPriority priority, Delegate method, object arg)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Executes the specified delegate at the specified priority with the specified arguments synchronously on the thread the <see cref="T:System.Windows.Threading.Dispatcher" /> is associated with.</summary>
	/// <param name="priority">The priority, relative to the other pending operations in the <see cref="T:System.Windows.Threading.Dispatcher" /> event queue, the specified method is invoked.</param>
	/// <param name="method">A delegate to a method that takes multiple arguments, which is pushed onto the <see cref="T:System.Windows.Threading.Dispatcher" /> event queue.</param>
	/// <param name="arg">An object to pass as an argument to the given method.</param>
	/// <param name="args">An array of objects to pass as arguments to the given method.</param>
	/// <returns>The return value from the delegate being invoked or <see langword="null" /> if the delegate has no return value.</returns>
	/// <exception cref="T:System.ArgumentException">
	///         <paramref name="priority" /> is equal to <see cref="F:System.Windows.Threading.DispatcherPriority.Inactive" />.</exception>
	/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
	///         <paramref name="priority" /> is not a valid priority.</exception>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="method" /> is <see langword="null" />. </exception>
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public object Invoke(DispatcherPriority priority, Delegate method, object arg, params object[] args)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Executes the specified delegate synchronously at the specified priority and with the specified time-out value on the thread the <see cref="T:System.Windows.Threading.Dispatcher" /> was created.</summary>
	/// <param name="priority">The priority, relative to the other pending operations in the <see cref="T:System.Windows.Threading.Dispatcher" /> event queue, the specified method is invoked.</param>
	/// <param name="timeout">The maximum time to wait for the operation to finish.</param>
	/// <param name="method">The delegate to a method that takes no arguments, which is pushed onto the <see cref="T:System.Windows.Threading.Dispatcher" /> event queue.</param>
	/// <returns>The return value from the delegate being invoked or <see langword="null" /> if the delegate has no return value.</returns>
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public object Invoke(DispatcherPriority priority, TimeSpan timeout, Delegate method)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Executes the specified delegate at the specified priority with the specified argument synchronously on the thread the <see cref="T:System.Windows.Threading.Dispatcher" /> is associated with.</summary>
	/// <param name="priority">The priority, relative to the other pending operations in the <see cref="T:System.Windows.Threading.Dispatcher" /> event queue, the specified method is invoked.</param>
	/// <param name="timeout">The maximum time to wait for the operation to finish.</param>
	/// <param name="method">A delegate to a method that takes multiple arguments, which is pushed onto the <see cref="T:System.Windows.Threading.Dispatcher" /> event queue.</param>
	/// <param name="arg">An object to pass as an argument to the given method. This can be <see langword="null" /> if no arguments are needed.</param>
	/// <returns>The return value from the delegate being invoked or <see langword="null" /> if the delegate has no return value.</returns>
	/// <exception cref="T:System.ArgumentException">
	///         <paramref name="priority" /> is equal to <see cref="F:System.Windows.Threading.DispatcherPriority.Inactive" />.</exception>
	/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
	///         <paramref name="priority" /> is not a valid priority.</exception>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="method" /> is <see langword="null" />. </exception>
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public object Invoke(DispatcherPriority priority, TimeSpan timeout, Delegate method, object arg)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Executes the specified delegate at the specified priority with the specified arguments synchronously on the thread the <see cref="T:System.Windows.Threading.Dispatcher" /> is associated with.</summary>
	/// <param name="priority">The priority, relative to the other pending operations in the <see cref="T:System.Windows.Threading.Dispatcher" /> event queue, the specified method is invoked.</param>
	/// <param name="timeout">The maximum time to wait for the operation to finish.</param>
	/// <param name="method">A delegate to a method that takes multiple arguments, which is pushed onto the <see cref="T:System.Windows.Threading.Dispatcher" /> event queue.</param>
	/// <param name="arg">An object to pass as an argument to the specified method.</param>
	/// <param name="args">An array of objects to pass as arguments to the specified method. </param>
	/// <returns>The return value from the delegate being invoked or <see langword="null" /> if the delegate has no return value.</returns>
	/// <exception cref="T:System.ArgumentException">
	///         <paramref name="priority" /> is equal to <see cref="F:System.Windows.Threading.DispatcherPriority.Inactive" />.</exception>
	/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
	///         <paramref name="priority" /> is not a valid <see cref="T:System.Windows.Threading.DispatcherPriority" />.</exception>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="method" /> is <see langword="null" />. </exception>
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public object Invoke(DispatcherPriority priority, TimeSpan timeout, Delegate method, object arg, params object[] args)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Executes the specified delegate with the specified arguments synchronously on the thread the <see cref="T:System.Windows.Threading.Dispatcher" /> is associated with.</summary>
	/// <param name="method">A delegate to a method that takes parameters specified in <paramref name="args" />, which is pushed onto the <see cref="T:System.Windows.Threading.Dispatcher" /> event queue.</param>
	/// <param name="args">An array of objects to pass as arguments to the given method. Can be <see langword="null" />.</param>
	/// <returns>The return value from the delegate being invoked or <see langword="null" /> if the delegate has no return value.</returns>
	public object Invoke(Delegate method, params object[] args)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Executes the specified delegate at the specified priority with the specified arguments synchronously on the thread the <see cref="T:System.Windows.Threading.Dispatcher" /> is associated with.</summary>
	/// <param name="method">A delegate to a method that takes parameters specified in <paramref name="args" />, which is pushed onto the <see cref="T:System.Windows.Threading.Dispatcher" /> event queue.</param>
	/// <param name="priority">The priority, relative to the other pending operations in the <see cref="T:System.Windows.Threading.Dispatcher" /> event queue, the specified method is invoked.</param>
	/// <param name="args">An array of objects to pass as arguments to the given method. Can be <see langword="null" />.</param>
	/// <returns>The return value from the delegate being invoked or <see langword="null" /> if the delegate has no return value.</returns>
	public object Invoke(Delegate method, DispatcherPriority priority, params object[] args)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Executes the specified delegate within the designated time span at the specified priority with the specified arguments synchronously on the thread the <see cref="T:System.Windows.Threading.Dispatcher" /> is associated with.</summary>
	/// <param name="method">A delegate to a method that takes parameters specified in <paramref name="args" />, which is pushed onto the <see cref="T:System.Windows.Threading.Dispatcher" /> event queue.</param>
	/// <param name="timeout">The maximum amount of time to wait for the operation to complete.</param>
	/// <param name="args">An array of objects to pass as arguments to the given method. Can be <see langword="null" />.</param>
	/// <returns>The return value from the delegate being invoked or <see langword="null" /> if the delegate has no return value.</returns>
	public object Invoke(Delegate method, TimeSpan timeout, params object[] args)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Executes the specified delegate within the designated time span at the specified priority with the specified arguments synchronously on the thread the <see cref="T:System.Windows.Threading.Dispatcher" /> is associated with.</summary>
	/// <param name="method">A delegate to a method that takes parameters specified in <paramref name="args" />, which is pushed onto the <see cref="T:System.Windows.Threading.Dispatcher" /> event queue.</param>
	/// <param name="timeout">The maximum amount of time to wait for the operation to complete.</param>
	/// <param name="priority">The priority, relative to the other pending operations in the <see cref="T:System.Windows.Threading.Dispatcher" /> event queue, the specified method is invoked.</param>
	/// <param name="args">An array of objects to pass as arguments to the given method. Can be <see langword="null" />.</param>
	/// <returns>The return value from the delegate being invoked or <see langword="null" /> if the delegate has no return value.</returns>
	public object Invoke(Delegate method, TimeSpan timeout, DispatcherPriority priority, params object[] args)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecuritySafeCritical]
	internal object LegacyInvokeImpl(DispatcherPriority priority, TimeSpan timeout, Delegate method, object args, int numArgs)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Disables processing of the <see cref="T:System.Windows.Threading.Dispatcher" /> queue.</summary>
	/// <returns>A structure used to re-enable dispatcher processing.</returns>
	public DispatcherProcessingDisabled DisableProcessing()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Determines whether the specified <see cref="T:System.Windows.Threading.DispatcherPriority" /> is a valid priority. </summary>
	/// <param name="priority">The priority to check.</param>
	/// <param name="parameterName">A string that will be returned by the exception that occurs if the priority is invalid.</param>
	/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
	///         <paramref name="priority" /> is not a valid <see cref="T:System.Windows.Threading.DispatcherPriority" />.</exception>
	public static void ValidatePriority(DispatcherPriority priority, string parameterName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	private Dispatcher()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	internal bool SetPriority(DispatcherOperation operation, DispatcherPriority priority)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	internal bool Abort(DispatcherOperation operation)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal bool CriticalRequestProcessing(bool force)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void PromoteTimers(int currentTimeInTicks)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void AddTimer(DispatcherTimer timer)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void RemoveTimer(DispatcherTimer timer)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void UpdateWin32Timer()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[MS.Internal.WindowsBase.FriendAccessAllowed]
	internal object WrappedInvoke(Delegate callback, object args, int numArgs, Delegate catchHandler)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
