namespace System.Windows;

/// <summary>Contains properties that specify how an application should behave relative to new WPF features that are in the WindowsBase assembly.</summary>
public static class BaseCompatibilityPreferences
{
	/// <summary>Gets or sets a value that indicates whether a single <see cref="T:System.Windows.Threading.DispatcherSynchronizationContext" /> is used for multiple dispatcher operations.</summary>
	/// <returns>true if a single <see cref="T:System.Windows.Threading.DispatcherSynchronizationContext" /> is used for multiple dispatcher operations; otherwise, <see langword="false" />.</returns>
	public static bool ReuseDispatcherSynchronizationContextInstance
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

	/// <summary>Gets or sets a value that indicates whether information about the <see cref="P:System.Windows.Threading.DispatcherOperation.Priority" /> is saved to the <see cref="T:System.Windows.Threading.DispatcherSynchronizationContext" />.</summary>
	/// <returns>
	///     <see langword="true" /> if information about the <see cref="P:System.Windows.Threading.DispatcherOperation.Priority" /> is saved to the <see cref="T:System.Windows.Threading.DispatcherSynchronizationContext" />; otherwise, <see langword="false" />.</returns>
	public static bool FlowDispatcherSynchronizationContextPriority
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

	/// <summary>Gets or sets a value that indicates whether the <see cref="M:System.Windows.Threading.DispatcherSynchronizationContext.Send(System.Threading.SendOrPostCallback,System.Object)" /> method puts the delegates on the dispatcher queue or if the delegate is directly invoked.</summary>
	/// <returns>
	///     <see langword="true" /> if the <see cref="M:System.Windows.Threading.DispatcherSynchronizationContext.Send(System.Threading.SendOrPostCallback,System.Object)" /> method puts the delegates on the dispatcher queue or if the delegate is directly invoked; otherwise, <see langword="false" />.</returns>
	public static bool InlineDispatcherSynchronizationContextSend
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

	internal static bool GetReuseDispatcherSynchronizationContextInstance()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static bool GetFlowDispatcherSynchronizationContextPriority()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal static bool GetInlineDispatcherSynchronizationContextSend()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static BaseCompatibilityPreferences()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
