using System.Security;

namespace System.Threading;

internal sealed class WinRTDispatcherQueueBasedSynchronizationContext : WinRTSynchronizationContextBase
{
	private readonly IDispatcherQueue _dispatcherQueue;

	internal WinRTDispatcherQueueBasedSynchronizationContext(IDispatcherQueue dispatcherQueue)
	{
		_dispatcherQueue = dispatcherQueue;
	}

	[SecuritySafeCritical]
	public override void Post(SendOrPostCallback d, object state)
	{
		if (d == null)
		{
			throw new ArgumentNullException("d");
		}
		bool flag = _dispatcherQueue.TryEnqueue(DispatcherQueuePriority.Normal, new Invoker(d, state).Invoke);
	}

	public override SynchronizationContext CreateCopy()
	{
		return new WinRTDispatcherQueueBasedSynchronizationContext(_dispatcherQueue);
	}
}
