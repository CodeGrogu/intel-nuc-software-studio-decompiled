namespace System.Windows.Threading;

/// <summary>Represents a delegate to use for dispatcher operations.</summary>
/// <param name="arg">An argument passed to the callback.</param>
/// <returns>The object returned by the callback.</returns>
public delegate object DispatcherOperationCallback(object arg);
