using System.Diagnostics.Tracing;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security;

namespace System.Threading;

internal abstract class WinRTSynchronizationContextBase : SynchronizationContext
{
	protected class Invoker
	{
		private delegate void DelEtwFireThreadTransferSendObj(object id, int kind, string info, bool multiDequeues);

		private delegate void DelEtwFireThreadTransferObj(object id, int kind, string info);

		private readonly ExecutionContext m_executionContext;

		private readonly SendOrPostCallback m_callback;

		private readonly object m_state;

		private static readonly ContextCallback s_contextCallback = InvokeInContext;

		private static DelEtwFireThreadTransferSendObj s_EtwFireThreadTransferSendObj;

		private static DelEtwFireThreadTransferObj s_EtwFireThreadTransferReceiveObj;

		private static DelEtwFireThreadTransferObj s_EtwFireThreadTransferReceiveHandledObj;

		private static volatile bool s_TriedGetEtwDelegates;

		[SecurityCritical]
		public Invoker(SendOrPostCallback callback, object state)
		{
			m_executionContext = ExecutionContext.FastCapture();
			m_callback = callback;
			m_state = state;
			if (FrameworkEventSource.Log.IsEnabled(EventLevel.Informational, (EventKeywords)16L))
			{
				EtwFireThreadTransferSendObj(this);
			}
		}

		[SecurityCritical]
		public void Invoke()
		{
			if (FrameworkEventSource.Log.IsEnabled(EventLevel.Informational, (EventKeywords)16L))
			{
				EtwFireThreadTransferReceiveObj(this);
			}
			if (m_executionContext == null)
			{
				InvokeCore();
			}
			else
			{
				ExecutionContext.Run(m_executionContext, s_contextCallback, this, preserveSyncCtx: true);
			}
			if (FrameworkEventSource.Log.IsEnabled(EventLevel.Informational, (EventKeywords)16L))
			{
				EtwFireThreadTransferReceiveHandledObj(this);
			}
		}

		private static void InvokeInContext(object thisObj)
		{
			((Invoker)thisObj).InvokeCore();
		}

		private void InvokeCore()
		{
			try
			{
				m_callback(m_state);
			}
			catch (Exception ex)
			{
				if (!(ex is ThreadAbortException) && !(ex is AppDomainUnloadedException) && !WindowsRuntimeMarshal.ReportUnhandledError(ex))
				{
					ExceptionDispatchInfo state = ExceptionDispatchInfo.Capture(ex);
					ThreadPool.QueueUserWorkItem((object o) =>
					{
						((ExceptionDispatchInfo)o).Throw();
					}, state);
				}
			}
		}

		private static void InitEtwMethods()
		{
			Type typeFromHandle = typeof(FrameworkEventSource);
			MethodInfo method = typeFromHandle.GetMethod("ThreadTransferSendObj", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			MethodInfo method2 = typeFromHandle.GetMethod("ThreadTransferReceiveObj", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			MethodInfo method3 = typeFromHandle.GetMethod("ThreadTransferReceiveHandledObj", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (method != null && method2 != null && method3 != null)
			{
				s_EtwFireThreadTransferSendObj = (DelEtwFireThreadTransferSendObj)method.CreateDelegate(typeof(DelEtwFireThreadTransferSendObj), FrameworkEventSource.Log);
				s_EtwFireThreadTransferReceiveObj = (DelEtwFireThreadTransferObj)method2.CreateDelegate(typeof(DelEtwFireThreadTransferObj), FrameworkEventSource.Log);
				s_EtwFireThreadTransferReceiveHandledObj = (DelEtwFireThreadTransferObj)method3.CreateDelegate(typeof(DelEtwFireThreadTransferObj), FrameworkEventSource.Log);
			}
			s_TriedGetEtwDelegates = true;
		}

		private static void EtwFireThreadTransferSendObj(object id)
		{
			if (!s_TriedGetEtwDelegates)
			{
				InitEtwMethods();
			}
			if (s_EtwFireThreadTransferSendObj != null)
			{
				s_EtwFireThreadTransferSendObj(id, 3, string.Empty, multiDequeues: false);
			}
		}

		private static void EtwFireThreadTransferReceiveObj(object id)
		{
			if (!s_TriedGetEtwDelegates)
			{
				InitEtwMethods();
			}
			if (s_EtwFireThreadTransferReceiveObj != null)
			{
				s_EtwFireThreadTransferReceiveObj(id, 3, string.Empty);
			}
		}

		private static void EtwFireThreadTransferReceiveHandledObj(object id)
		{
			if (!s_TriedGetEtwDelegates)
			{
				InitEtwMethods();
			}
			if (s_EtwFireThreadTransferReceiveHandledObj != null)
			{
				s_EtwFireThreadTransferReceiveHandledObj(id, 3, string.Empty);
			}
		}
	}

	[SecuritySafeCritical]
	public override void Send(SendOrPostCallback d, object state)
	{
		throw new NotSupportedException(SR.GetString("InvalidOperation_SendNotSupportedOnWindowsRTSynchronizationContext"));
	}
}
