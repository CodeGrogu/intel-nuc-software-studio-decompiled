using System.Runtime.ConstrainedExecution;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.Windows.Threading;

/// <summary>Provides a synchronization context for Windows Presentation Foundation (WPF).</summary>
public sealed class DispatcherSynchronizationContext : SynchronizationContext
{
	internal Dispatcher _dispatcher;

	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Threading.DispatcherSynchronizationContext" /> class by using the current <see cref="T:System.Windows.Threading.Dispatcher" />.</summary>
	public DispatcherSynchronizationContext()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Threading.DispatcherSynchronizationContext" /> class by using the specified <see cref="T:System.Windows.Threading.Dispatcher" />.</summary>
	/// <param name="dispatcher">The <see cref="T:System.Windows.Threading.Dispatcher" /> to associate this <see cref="T:System.Windows.Threading.DispatcherSynchronizationContext" /> with.</param>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="dispatcher" /> is <see langword="null" />.</exception>
	public DispatcherSynchronizationContext(Dispatcher dispatcher)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Threading.DispatcherSynchronizationContext" /> class by using the specified <see cref="T:System.Windows.Threading.Dispatcher" />.</summary>
	/// <param name="dispatcher">The <see cref="T:System.Windows.Threading.Dispatcher" /> to associate this <see cref="T:System.Windows.Threading.DispatcherSynchronizationContext" /> with.</param>
	/// <param name="priority">The priority used to send and post callback methods.</param>
	public DispatcherSynchronizationContext(Dispatcher dispatcher, DispatcherPriority priority)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Invokes the callback in the synchronization context synchronously.</summary>
	/// <param name="d">The delegate to call.</param>
	/// <param name="state">The object passed to the delegate.</param>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="d" /> is <see langword="null" />.</exception>
	public override void Send(SendOrPostCallback d, object state)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Invokes the callback in the synchronization context asynchronously. </summary>
	/// <param name="d">The delegate to call.</param>
	/// <param name="state">The object passed to the delegate.</param>
	/// <exception cref="T:System.ArgumentNullException">
	///         <paramref name="d" /> is <see langword="null" />.</exception>
	public override void Post(SendOrPostCallback d, object state)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Waits for any or all the elements in the specified array to receive a signal.</summary>
	/// <param name="waitHandles">An array that contains the native operating system handles.</param>
	/// <param name="waitAll">
	///       <see langword="true" /> to wait for all handles; <see langword="false" /> to wait for any handle.</param>
	/// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely.</param>
	/// <returns>The array index of the object that satisfied the wait.</returns>
	[SecurityCritical]
	[PrePrepareMethod]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = (SecurityPermissionFlag.ControlEvidence | SecurityPermissionFlag.ControlPolicy))]
	public override int Wait(IntPtr[] waitHandles, bool waitAll, int millisecondsTimeout)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Creates a copy of this <see cref="T:System.Windows.Threading.DispatcherSynchronizationContext" />. </summary>
	/// <returns>The copy of this synchronization context.</returns>
	public override SynchronizationContext CreateCopy()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
