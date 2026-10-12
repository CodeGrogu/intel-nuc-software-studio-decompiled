namespace System.Windows.Threading;

/// <summary>Provides data for the <see cref="T:System.Windows.Threading.Dispatcher" /> <see cref="E:System.Windows.Threading.Dispatcher.UnhandledExceptionFilter" /> event.</summary>
public sealed class DispatcherUnhandledExceptionFilterEventArgs : DispatcherEventArgs
{
	/// <summary>Gets the exception that was raised when executing code by way of the dispatcher.</summary>
	/// <returns>The exception.</returns>
	public Exception Exception
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets or sets whether the exception should be caught and the event handlers called. </summary>
	/// <returns>
	///     <see langword="true" /> if the <see cref="E:System.Windows.Threading.Dispatcher.UnhandledException" /> should be raised; otherwise; <see langword="false" />.  The default value is <see langword="true" />.</returns>
	public bool RequestCatch
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

	internal DispatcherUnhandledExceptionFilterEventArgs(Dispatcher dispatcher)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void Initialize(Exception exception, bool requestCatch)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
