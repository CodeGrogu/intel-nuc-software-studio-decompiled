namespace System.Windows.Threading;

/// <summary>Provides event data for <see cref="T:System.Windows.Threading.DispatcherHooks" /> events.</summary>
public sealed class DispatcherHookEventArgs : EventArgs
{
	/// <summary>Gets the <see cref="T:System.Windows.Threading.Dispatcher" /> associated with this event. </summary>
	/// <returns>The <see cref="T:System.Windows.Threading.Dispatcher" /> associated with this event.</returns>
	public Dispatcher Dispatcher
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets the <see cref="T:System.Windows.Threading.DispatcherOperation" /> associated with this event. </summary>
	/// <returns>The operation.</returns>
	public DispatcherOperation Operation
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Threading.DispatcherHookEventArgs" /> class. </summary>
	/// <param name="operation">The operation associated with the event.</param>
	public DispatcherHookEventArgs(DispatcherOperation operation)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
