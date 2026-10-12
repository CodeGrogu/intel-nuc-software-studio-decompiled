namespace System.Windows.Threading;

/// <summary>Provides data for the <see cref="T:System.Windows.Threading.Dispatcher" /> <see cref="E:System.Windows.Threading.Dispatcher.UnhandledException" /> event.</summary>
public sealed class DispatcherUnhandledExceptionEventArgs : DispatcherEventArgs
{
	/// <summary>Gets the exception that was raised when executing code by way of the dispatcher. </summary>
	/// <returns>The exception. </returns>
	public Exception Exception
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets or sets whether the exception event has been handled. </summary>
	/// <returns>
	///     <see langword="true" /> if the exception was handled; otherwise, <see langword="false" />.</returns>
	public bool Handled
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

	internal DispatcherUnhandledExceptionEventArgs(Dispatcher dispatcher)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void Initialize(Exception exception, bool handled)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
