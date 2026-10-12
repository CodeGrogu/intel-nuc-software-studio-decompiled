using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;

namespace System.IO;

internal abstract class StreamOperationAsyncResult : IAsyncResult
{
	private AsyncCallback userCompletionCallback;

	private object userAsyncStateInfo;

	private IAsyncInfo asyncStreamOperation;

	private volatile bool completed;

	private volatile bool callbackInvoked;

	private volatile ManualResetEvent waitHandle;

	private long bytesCompleted;

	private ExceptionDispatchInfo errorInfo;

	private readonly bool processCompletedOperationInCallback;

	private IAsyncInfo completedOperation;

	public object AsyncState => userAsyncStateInfo;

	internal bool ProcessCompletedOperationInCallback => processCompletedOperationInCallback;

	public WaitHandle AsyncWaitHandle
	{
		get
		{
			ManualResetEvent manualResetEvent = waitHandle;
			if (manualResetEvent != null)
			{
				return manualResetEvent;
			}
			manualResetEvent = new ManualResetEvent(completed);
			ManualResetEvent manualResetEvent2 = Interlocked.CompareExchange(ref waitHandle, manualResetEvent, null);
			if (manualResetEvent2 != null)
			{
				manualResetEvent.Dispose();
				return manualResetEvent2;
			}
			return manualResetEvent;
		}
	}

	public bool CompletedSynchronously => false;

	public bool IsCompleted => completed;

	internal long BytesCompleted => bytesCompleted;

	internal bool HasError => errorInfo != null;

	protected internal StreamOperationAsyncResult(IAsyncInfo asyncStreamOperation, AsyncCallback userCompletionCallback, object userAsyncStateInfo, bool processCompletedOperationInCallback)
	{
		if (asyncStreamOperation == null)
		{
			throw new ArgumentNullException("asyncReadOperation");
		}
		this.userCompletionCallback = userCompletionCallback;
		this.userAsyncStateInfo = userAsyncStateInfo;
		this.asyncStreamOperation = asyncStreamOperation;
		completed = false;
		callbackInvoked = false;
		bytesCompleted = 0L;
		errorInfo = null;
		this.processCompletedOperationInCallback = processCompletedOperationInCallback;
	}

	internal void Wait()
	{
		if (!completed)
		{
			WaitHandle asyncWaitHandle = AsyncWaitHandle;
			while (!completed)
			{
				asyncWaitHandle.WaitOne();
			}
		}
	}

	internal void ThrowCachedError()
	{
		if (errorInfo != null)
		{
			errorInfo.Throw();
		}
	}

	internal bool CancelStreamOperation()
	{
		if (callbackInvoked)
		{
			return false;
		}
		asyncStreamOperation.Cancel();
		return true;
	}

	internal void CloseStreamOperation()
	{
		try
		{
			if (asyncStreamOperation != null)
			{
				asyncStreamOperation.Close();
			}
		}
		catch
		{
		}
		asyncStreamOperation = null;
	}

	~StreamOperationAsyncResult()
	{
		CancelStreamOperation();
	}

	internal abstract void ProcessConcreteCompletedOperation(IAsyncInfo completedOperation, out long bytesCompleted);

	private static void ProcessCompletedOperation_InvalidOperationThrowHelper(ExceptionDispatchInfo errInfo, string errMsg)
	{
		Exception ex = errInfo?.SourceException;
		if (ex == null)
		{
			throw new InvalidOperationException(errMsg);
		}
		throw new InvalidOperationException(errMsg, ex);
	}

	internal void ProcessCompletedOperation()
	{
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Invalid comparison between Unknown and I4
		if (!callbackInvoked)
		{
			ProcessCompletedOperation_InvalidOperationThrowHelper(errorInfo, SR.GetString("InvalidOperation_CannotCallThisMethodInCurrentState"));
		}
		if (!processCompletedOperationInCallback && !completed)
		{
			ProcessCompletedOperation_InvalidOperationThrowHelper(errorInfo, SR.GetString("InvalidOperation_CannotCallThisMethodInCurrentState"));
		}
		if (completedOperation == null)
		{
			ExceptionDispatchInfo exceptionDispatchInfo = errorInfo;
			Exception ex = exceptionDispatchInfo?.SourceException;
			if (ex == null || !(ex is NullReferenceException) || !SR.GetString("NullReference_IOCompletionCallbackCannotProcessNullAsyncInfo").Equals(ex.Message))
			{
				throw new InvalidOperationException(SR.GetString("InvalidOperation_CannotCallThisMethodInCurrentState"));
			}
			exceptionDispatchInfo.Throw();
		}
		if (completedOperation.Id != asyncStreamOperation.Id)
		{
			ProcessCompletedOperation_InvalidOperationThrowHelper(errorInfo, SR.GetString("InvalidOperation_UnexpectedAsyncOperationID"));
		}
		if ((int)completedOperation.Status == 3)
		{
			bytesCompleted = 0L;
			WinRtIOHelper.NativeExceptionToIOExceptionInfo(RestrictedErrorInfoHelper.AttachRestrictedErrorInfo(completedOperation.ErrorCode)).Throw();
		}
		ProcessConcreteCompletedOperation(completedOperation, out bytesCompleted);
	}

	internal void StreamOperationCompletedCallback(IAsyncInfo completedOperation, AsyncStatus unusedCompletionStatus)
	{
		try
		{
			if (callbackInvoked)
			{
				throw new InvalidOperationException(SR.GetString("InvalidOperation_MultipleIOCompletionCallbackInvocation"));
			}
			callbackInvoked = true;
			if (completedOperation == null)
			{
				throw new NullReferenceException(SR.GetString("NullReference_IOCompletionCallbackCannotProcessNullAsyncInfo"));
			}
			this.completedOperation = completedOperation;
			if (processCompletedOperationInCallback)
			{
				ProcessCompletedOperation();
			}
		}
		catch (Exception source)
		{
			bytesCompleted = 0L;
			errorInfo = ExceptionDispatchInfo.Capture(source);
		}
		finally
		{
			completed = true;
			Thread.MemoryBarrier();
			if (waitHandle != null)
			{
				waitHandle.Set();
			}
		}
		if (userCompletionCallback != null)
		{
			userCompletionCallback(this);
		}
	}
}
