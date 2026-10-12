using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security;
using System.Threading.Tasks;

namespace System.Windows.Threading;

/// <summary>Represents an object that is used to interact with an operation that has been posted to the <see cref="T:System.Windows.Threading.Dispatcher" /> queue.</summary>
public class DispatcherOperation
{
	internal readonly Delegate _method;

	internal DispatcherOperationStatus _status;

	internal PriorityItem<DispatcherOperation> _item;

	internal readonly DispatcherOperationTaskSource _taskSource;

	/// <summary>Gets the <see cref="T:System.Windows.Threading.Dispatcher" /> that the operation was posted to. </summary>
	/// <returns>The dispatcher.</returns>
	public Dispatcher Dispatcher
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets or sets the priority of the operation in the <see cref="T:System.Windows.Threading.Dispatcher" /> queue. </summary>
	/// <returns>The priority of the delegate on the queue.</returns>
	public DispatcherPriority Priority
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

	/// <summary>Gets the current status of the operation..</summary>
	/// <returns>The status of the operation.</returns>
	public DispatcherOperationStatus Status
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets a <see cref="T:System.Threading.Tasks.Task`1" /> that represents the current operation.</summary>
	/// <returns>An object that represents the current operation.</returns>
	public Task Task
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal string Name
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal long Id
	{
		[SecurityCritical]
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the result of the operation after it has completed. </summary>
	/// <returns>
	///     The result of the operation -or- <see langword="null" /> if the operation has not completed.</returns>
	public object Result
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Occurs when the operation is aborted.</summary>
	public event EventHandler Aborted
	{
		add
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		remove
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Occurs when the operation has completed. </summary>
	public event EventHandler Completed
	{
		add
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		remove
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	[SecuritySafeCritical]
	static DispatcherOperation()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal DispatcherOperation(Dispatcher dispatcher, Delegate method, DispatcherPriority priority, object args, int numArgs, DispatcherOperationTaskSource taskSource, bool useAsyncSemantics)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal DispatcherOperation(Dispatcher dispatcher, Delegate method, DispatcherPriority priority, object args, int numArgs)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal DispatcherOperation(Dispatcher dispatcher, DispatcherPriority priority, Action action)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal DispatcherOperation(Dispatcher dispatcher, DispatcherPriority priority, Delegate method, object[] args)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns an object that is notified when the asynchronous operation is finished.</summary>
	/// <returns>An object that is notified when the asynchronous operation is finished.</returns>
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public TaskAwaiter GetAwaiter()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Waits for the operation to complete</summary>
	/// <returns>The status of the operation. </returns>
	/// <exception cref="T:System.InvalidOperationException">
	///         <see cref="P:System.Windows.Threading.DispatcherOperation.Status" /> is equal to <see cref="F:System.Windows.Threading.DispatcherOperationStatus.Executing" />.  This can occur when waiting for an operation that is already executing on the same thread.</exception>
	public DispatcherOperationStatus Wait()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Waits for the operation to complete in the specified period of time.</summary>
	/// <param name="timeout">The maximum period of time to wait.</param>
	/// <returns>The status of the operation. </returns>
	/// <exception cref="T:System.InvalidOperationException">
	///         <see cref="P:System.Windows.Threading.DispatcherOperation.Status" /> is equal to <see cref="F:System.Windows.Threading.DispatcherOperationStatus.Executing" />.  This can occur when waiting for an operation that is already executing on the same thread.</exception>
	[SecurityCritical]
	public DispatcherOperationStatus Wait(TimeSpan timeout)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Aborts the operation. </summary>
	/// <returns>
	///     <see langword="true" /> if the operation was aborted; otherwise, <see langword="false" />.</returns>
	public bool Abort()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal void Invoke()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal void InvokeCompletions()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Begins the operation that is associated with this <see cref="T:System.Windows.Threading.DispatcherOperation" />.</summary>
	/// <returns>
	///     <see langword="null" /> in all cases.</returns>
	protected virtual object InvokeDelegateCore()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
/// <summary>Represents an object that is used to interact with an operation that has been posted to the <see cref="T:System.Windows.Threading.Dispatcher" /> queue and contains a <see cref="T:System.Threading.Tasks.Task`1" />. </summary>
/// <typeparam name="TResult">The type that is returned by the <see cref="T:System.Threading.Tasks.Task`1" />.</typeparam>
public class DispatcherOperation<TResult> : DispatcherOperation
{
	/// <summary>Gets a <see cref="T:System.Threading.Tasks.Task`1" /> that represents the current operation.</summary>
	/// <returns>An object that represents the current operation.</returns>
	public new Task<TResult> Task
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the result of the operation after it has completed.</summary>
	/// <returns>The result of the operation.</returns>
	public new TResult Result
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	[SecurityCritical]
	internal DispatcherOperation(Dispatcher dispatcher, DispatcherPriority priority, Func<TResult> func)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Returns an object that awaits an asynchronous operation that returns a result.</summary>
	/// <returns>An object that awaits an asynchronous operation that returns a result.</returns>
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public new TaskAwaiter<TResult> GetAwaiter()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Begins the operation that is associated with this <see cref="T:System.Windows.Threading.DispatcherOperation" />.</summary>
	/// <returns>The result of the operation.</returns>
	protected override object InvokeDelegateCore()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
