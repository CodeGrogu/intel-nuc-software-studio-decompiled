using System.Security;
using Windows.Foundation;
using Windows.UI.Core;

namespace System.Threading;

internal sealed class WinRTCoreDispatcherBasedSynchronizationContext : WinRTSynchronizationContextBase
{
	private readonly CoreDispatcher _dispatcher;

	internal WinRTCoreDispatcherBasedSynchronizationContext(CoreDispatcher dispatcher)
	{
		_dispatcher = dispatcher;
	}

	[SecuritySafeCritical]
	public override void Post(SendOrPostCallback d, object state)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected Obj, but got Unknown
		if (d == null)
		{
			throw new ArgumentNullException("d");
		}
		IAsyncAction val = _dispatcher.RunAsync((CoreDispatcherPriority)0, (DispatchedHandler)new Invoker(d, state).Invoke);
	}

	public override SynchronizationContext CreateCopy()
	{
		return new WinRTCoreDispatcherBasedSynchronizationContext(_dispatcher);
	}
}
