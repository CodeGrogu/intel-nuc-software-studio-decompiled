using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;

namespace System.Threading.Tasks;

internal sealed class AsyncInfoToTaskBridge<TResult, TProgress> : TaskCompletionSource<TResult>
{
	private CancellationTokenRegistration m_ctr;

	private bool m_completing;

	private object StateLock => this;

	internal AsyncInfoToTaskBridge()
	{
		if (AsyncCausalityTracer.LoggingOn)
		{
			AsyncCausalityTracer.TraceOperationCreation(CausalityTraceLevel.Required, Task.Id, "WinRT Operation as Task", 0uL);
		}
		if (System.Threading.Tasks.Task.s_asyncDebuggingEnabled)
		{
			System.Threading.Tasks.Task.AddToActiveTasks(Task);
		}
	}

	internal void RegisterForCancellation(IAsyncInfo asyncInfo, CancellationToken cancellationToken)
	{
		try
		{
			if (!cancellationToken.CanBeCanceled || m_completing)
			{
				return;
			}
			CancellationTokenRegistration ctr = cancellationToken.Register((object ai) =>
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				((IAsyncInfo)ai).Cancel();
			}, asyncInfo);
			bool flag = false;
			lock (StateLock)
			{
				if (m_completing)
				{
					flag = true;
				}
				else
				{
					m_ctr = ctr;
				}
			}
			if (flag)
			{
				ctr.TryDeregister();
			}
		}
		catch (Exception exception)
		{
			if (!Task.IsFaulted)
			{
				TrySetException(exception);
			}
		}
	}

	internal void CompleteFromAsyncAction(IAsyncAction asyncInfo, AsyncStatus asyncStatus)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		Complete((IAsyncInfo)(object)asyncInfo, null, asyncStatus);
	}

	internal void CompleteFromAsyncActionWithProgress(IAsyncActionWithProgress<TProgress> asyncInfo, AsyncStatus asyncStatus)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		Complete((IAsyncInfo)(object)asyncInfo, null, asyncStatus);
	}

	internal void CompleteFromAsyncOperation(IAsyncOperation<TResult> asyncInfo, AsyncStatus asyncStatus)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		Complete((IAsyncInfo)(object)asyncInfo, (IAsyncInfo ai) => ((IAsyncOperation<TResult>)(object)ai).GetResults(), asyncStatus);
	}

	internal void CompleteFromAsyncOperationWithProgress(IAsyncOperationWithProgress<TResult, TProgress> asyncInfo, AsyncStatus asyncStatus)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		Complete((IAsyncInfo)(object)asyncInfo, (IAsyncInfo ai) => ((IAsyncOperationWithProgress<TResult, TProgress>)(object)ai).GetResults(), asyncStatus);
	}

	private void Complete(IAsyncInfo asyncInfo, Func<IAsyncInfo, TResult> getResultsFunction, AsyncStatus asyncStatus)
	{
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Expected I4, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Invalid comparison between Unknown and I4
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Invalid comparison between Unknown and I4
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Invalid comparison between Unknown and I4
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Invalid comparison between Unknown and I4
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Invalid comparison between Unknown and I4
		if (asyncInfo == null)
		{
			throw new ArgumentNullException("asyncInfo");
		}
		if (System.Threading.Tasks.Task.s_asyncDebuggingEnabled)
		{
			System.Threading.Tasks.Task.RemoveFromActiveTasks(Task.Id);
		}
		try
		{
			if (Task.IsCompleted)
			{
				throw new InvalidOperationException(SR.GetString("InvalidOperation_InvalidAsyncCompletion"));
			}
			CancellationTokenRegistration ctr;
			lock (StateLock)
			{
				m_completing = true;
				ctr = m_ctr;
				m_ctr = default;
			}
			ctr.TryDeregister();
			try
			{
				if ((int)asyncStatus != 1 && (int)asyncStatus != 2 && (int)asyncStatus != 3)
				{
					throw new InvalidOperationException(SR.GetString("InvalidOperation_InvalidAsyncCompletion"));
				}
				TResult result = default;
				Exception exception = null;
				if ((int)asyncStatus == 3)
				{
					exception = asyncInfo.ErrorCode;
					exception = ((exception != null) ? RestrictedErrorInfoHelper.AttachRestrictedErrorInfo(asyncInfo.ErrorCode) : new InvalidOperationException(SR.GetString("InvalidOperation_InvalidAsyncCompletion")));
				}
				else if ((int)asyncStatus == 1 && getResultsFunction != null)
				{
					try
					{
						result = getResultsFunction(asyncInfo);
					}
					catch (Exception ex)
					{
						exception = ex;
						asyncStatus = (AsyncStatus)3;
					}
				}
				bool flag = false;
				switch (asyncStatus - 1)
				{
				case 0:
					if (AsyncCausalityTracer.LoggingOn)
					{
						AsyncCausalityTracer.TraceOperationCompletion(CausalityTraceLevel.Required, Task.Id, AsyncCausalityStatus.Completed);
					}
					flag = TrySetResult(result);
					break;
				case 2:
					flag = TrySetException(exception);
					break;
				case 1:
					flag = TrySetCanceled();
					break;
				}
			}
			catch (Exception exception2)
			{
				if (AsyncCausalityTracer.LoggingOn)
				{
					AsyncCausalityTracer.TraceOperationCompletion(CausalityTraceLevel.Required, Task.Id, AsyncCausalityStatus.Error);
				}
				if (!TrySetException(exception2))
				{
					throw;
				}
			}
		}
		finally
		{
			if (Marshal.IsComObject(asyncInfo))
			{
				Marshal.ReleaseComObject(asyncInfo);
			}
		}
	}
}
