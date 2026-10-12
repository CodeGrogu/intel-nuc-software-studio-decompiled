using System.Runtime.CompilerServices;
using System.Security;
using Windows.UI.Core;

namespace System.Threading;

[FriendAccessAllowed]
[SecurityCritical]
internal sealed class WinRTSynchronizationContextFactory : WinRTSynchronizationContextFactoryBase
{
	private static readonly ConditionalWeakTable<CoreDispatcher, WinRTCoreDispatcherBasedSynchronizationContext> s_coreDispatcherContextCache = new ConditionalWeakTable<CoreDispatcher, WinRTCoreDispatcherBasedSynchronizationContext>();

	private static readonly ConditionalWeakTable<IDispatcherQueue, WinRTDispatcherQueueBasedSynchronizationContext> s_dispatcherQueueContextCache = new ConditionalWeakTable<IDispatcherQueue, WinRTDispatcherQueueBasedSynchronizationContext>();

	public override SynchronizationContext Create(object dispatcherObj)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Expected Obj, but got Unknown
		if (dispatcherObj is CoreDispatcher)
		{
			CoreDispatcher key = (CoreDispatcher)dispatcherObj;
			return s_coreDispatcherContextCache.GetValue(key, (CoreDispatcher _dispatcher) => new WinRTCoreDispatcherBasedSynchronizationContext(_dispatcher));
		}
		IDispatcherQueue key2 = (IDispatcherQueue)dispatcherObj;
		return s_dispatcherQueueContextCache.GetValue(key2, (IDispatcherQueue _dispatcherQueue) => new WinRTDispatcherQueueBasedSynchronizationContext(_dispatcherQueue));
	}
}
