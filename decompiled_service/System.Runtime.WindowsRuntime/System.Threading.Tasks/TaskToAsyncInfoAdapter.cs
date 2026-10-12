using System.Runtime.ExceptionServices;
using Windows.Foundation;

namespace System.Threading.Tasks;

internal class TaskToAsyncInfoAdapter<TCompletedHandler, TProgressHandler, TResult, TProgressInfo> : IAsyncInfo, IProgress<TProgressInfo> where TCompletedHandler : class where TProgressHandler : class
{
	private const int STATE_NOT_INITIALIZED = 0;

	private const int STATE_STARTED = 1;

	private const int STATE_RUN_TO_COMPLETION = 2;

	private const int STATE_CANCELLATION_REQUESTED = 4;

	private const int STATE_CANCELLATION_COMPLETED = 8;

	private const int STATE_ERROR = 16;

	private const int STATE_CLOSED = 32;

	private const int STATEFLAG_COMPLETED_SYNCHRONOUSLY = 536870912;

	private const int STATEFLAG_MUST_RUN_COMPLETION_HNDL_WHEN_SET = 268435456;

	private const int STATEFLAG_COMPLETION_HNDL_NOT_YET_INVOKED = 134217728;

	private const int STATEMASK_SELECT_ANY_ASYNC_STATE = 63;

	private const int STATEMASK_CLEAR_ALL_ASYNC_STATES = -64;

	private CancellationTokenSource m_cancelTokenSource;

	private uint m_id = 2147483647u;

	private Exception m_error;

	private volatile int m_state;

	private object m_dataContainer;

	private TCompletedHandler m_completedHandler;

	private TProgressHandler m_progressHandler;

	private SynchronizationContext m_startingContext;

	internal bool CompletedSynchronously => (m_state & 0x20000000) != 0;

	private bool IsInStartedState => (m_state & 1) != 0;

	private bool IsInRunToCompletionState => (m_state & 2) != 0;

	private bool IsInErrorState => (m_state & 0x10) != 0;

	private bool IsInClosedState => (m_state & 0x20) != 0;

	private bool IsInRunningState => (m_state & 5) != 0;

	private bool IsInTerminalState => (m_state & 0x1A) != 0;

	internal Task Task
	{
		get
		{
			EnsureNotClosed();
			if (CompletedSynchronously)
			{
				return null;
			}
			return (Task)m_dataContainer;
		}
	}

	internal CancellationTokenSource CancelTokenSource => m_cancelTokenSource;

	public virtual TCompletedHandler Completed
	{
		get
		{
			TCompletedHandler result = Volatile.Read(ref m_completedHandler);
			EnsureNotClosed();
			return result;
		}
		set
		{
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			EnsureNotClosed();
			TCompletedHandler val = Interlocked.CompareExchange(ref m_completedHandler, value, null);
			if (val != null)
			{
				InvalidOperationException ex = new InvalidOperationException(SR.GetString("InvalidOperation_CannotSetCompletionHanlderMoreThanOnce"));
				ex.SetErrorCode(-2147483624);
				throw ex;
			}
			if (value != null && (m_state & 0x10000000) != 0)
			{
				if (IsInClosedState)
				{
					Interlocked.Exchange(ref m_completedHandler, null);
				}
				else
				{
					OnCompletedInvoker(Status);
				}
			}
		}
	}

	public virtual TProgressHandler Progress
	{
		get
		{
			TProgressHandler result = Volatile.Read(ref m_progressHandler);
			EnsureNotClosed();
			return result;
		}
		set
		{
			EnsureNotClosed();
			Interlocked.Exchange(ref m_progressHandler, value);
			if (IsInClosedState)
			{
				Interlocked.Exchange(ref m_progressHandler, null);
			}
		}
	}

	public virtual Exception ErrorCode
	{
		get
		{
			EnsureNotClosed();
			if (!IsInErrorState)
			{
				return null;
			}
			Exception ex = Volatile.Read(ref m_error);
			if (ex != null || CompletedSynchronously)
			{
				return ex;
			}
			Task task = m_dataContainer as Task;
			AggregateException exception = task.Exception;
			if (exception == null)
			{
				ex = new Exception(SR.GetString("WinRtCOM_Error"));
				ex.SetErrorCode(-2147467259);
			}
			else
			{
				Exception innerException = exception.InnerException;
				ex = ((innerException == null) ? exception : innerException);
			}
			Exception ex2 = Interlocked.CompareExchange(ref m_error, ex, null);
			return ex2 ?? ex;
		}
	}

	public virtual uint Id
	{
		get
		{
			EnsureNotClosed();
			if (m_id != int.MaxValue)
			{
				return m_id;
			}
			return AsyncInfoIdGenerator.EnsureInitializedThreadsafe(ref m_id);
		}
	}

	public virtual AsyncStatus Status
	{
		get
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			EnsureNotClosed();
			return GetStatus(m_state);
		}
	}

	private static InvalidOperationException CreateCannotGetResultsFromIncompleteOperationException(Exception cause)
	{
		InvalidOperationException ex = ((cause == null) ? new InvalidOperationException(SR.GetString("InvalidOperation_CannotGetResultsFromIncompleteOperation")) : new InvalidOperationException(SR.GetString("InvalidOperation_CannotGetResultsFromIncompleteOperation"), cause));
		ex.SetErrorCode(-2147483634);
		return ex;
	}

	internal TaskToAsyncInfoAdapter(Delegate taskProvider)
	{
		m_startingContext = SynchronizationContext.CurrentNoFlow;
		Task task = InvokeTaskProvider(taskProvider);
		if (task == null)
		{
			throw new NullReferenceException(SR.GetString("NullReference_TaskProviderReturnedNull"));
		}
		if (task.Status == TaskStatus.Created)
		{
			throw new InvalidOperationException(SR.GetString("InvalidOperation_TaskProviderReturnedUnstartedTask"));
		}
		m_dataContainer = task;
		m_state = 134217729;
		task.ContinueWith((Task _, object this_) =>
		{
			((TaskToAsyncInfoAdapter<TCompletedHandler, TProgressHandler, TResult, TProgressInfo>)this_).TaskCompleted();
		}, this, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
	}

	internal TaskToAsyncInfoAdapter(Task underlyingTask, CancellationTokenSource underlyingCancelTokenSource, Progress<TProgressInfo> underlyingProgressDispatcher)
	{
		if (underlyingTask == null)
		{
			throw new ArgumentNullException("underlyingTask");
		}
		if (underlyingTask.Status == TaskStatus.Created)
		{
			throw new InvalidOperationException(SR.GetString("InvalidOperation_UnstartedTaskSpecified"));
		}
		m_startingContext = SynchronizationContext.CurrentNoFlow;
		m_dataContainer = underlyingTask;
		m_cancelTokenSource = underlyingCancelTokenSource;
		if (underlyingProgressDispatcher != null)
		{
			underlyingProgressDispatcher.ProgressChanged += OnReportChainedProgress;
		}
		m_state = 134217729;
		underlyingTask.ContinueWith((Task _, object this_) =>
		{
			((TaskToAsyncInfoAdapter<TCompletedHandler, TProgressHandler, TResult, TProgressInfo>)this_).TaskCompleted();
		}, this, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
	}

	internal TaskToAsyncInfoAdapter(TResult synchronousResult)
	{
		m_startingContext = null;
		m_dataContainer = synchronousResult;
		m_state = 939524098;
	}

	~TaskToAsyncInfoAdapter()
	{
		TransitionToClosed();
	}

	internal bool DangerousSetCompleted(TResult synchronousResult)
	{
		if (!CompletedSynchronously)
		{
			return false;
		}
		m_dataContainer = synchronousResult;
		m_error = null;
		m_state = 939524098;
		return true;
	}

	internal bool DangerousSetCanceled()
	{
		if (!CompletedSynchronously)
		{
			return false;
		}
		m_dataContainer = null;
		m_error = null;
		m_state = 939524104;
		return true;
	}

	internal bool DangerousSetError(Exception error)
	{
		if (!CompletedSynchronously)
		{
			return false;
		}
		if (error == null)
		{
			throw new ArgumentNullException("error");
		}
		m_dataContainer = null;
		m_error = error;
		m_state = 939524112;
		return true;
	}

	private bool CheckUniqueAsyncState(int state)
	{
		return (state & (~state + 1)) == state;
	}

	internal void EnsureNotClosed()
	{
		if (!IsInClosedState)
		{
			return;
		}
		ObjectDisposedException ex = new ObjectDisposedException(SR.GetString("ObjectDisposed_AsyncInfoIsClosed"));
		ex.SetErrorCode(-2147483634);
		throw ex;
	}

	internal virtual void OnCompleted(TCompletedHandler userCompletionHandler, AsyncStatus asyncStatus)
	{
	}

	internal virtual void OnProgress(TProgressHandler userProgressHandler, TProgressInfo progressInfo)
	{
	}

	private void OnCompletedInvoker(AsyncStatus status)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		TCompletedHandler val = Volatile.Read(ref m_completedHandler);
		bool conditionFailed;
		if (val == null)
		{
			SetState(268435456, -268435457, 0, useCondition: false, out conditionFailed);
			val = Volatile.Read(ref m_completedHandler);
			if (val == null)
			{
				return;
			}
		}
		SetState(0, -134217729, 134217728, useCondition: true, out conditionFailed);
		if (!conditionFailed)
		{
			OnCompleted(val, status);
		}
	}

	private void OnProgressInvokerCrossContext(TProgressHandler handler, TProgressInfo progressInfo)
	{
		m_startingContext.Post((object tupleObject) =>
		{
			Tuple<TaskToAsyncInfoAdapter<TCompletedHandler, TProgressHandler, TResult, TProgressInfo>, TProgressHandler, TProgressInfo> tuple = (Tuple<TaskToAsyncInfoAdapter<TCompletedHandler, TProgressHandler, TResult, TProgressInfo>, TProgressHandler, TProgressInfo>)tupleObject;
			tuple.Item1.OnProgress(tuple.Item2, tuple.Item3);
		}, Tuple.Create(this, handler, progressInfo));
	}

	void IProgress<TProgressInfo>.Report(TProgressInfo value)
	{
		TProgressHandler val = Volatile.Read(ref m_progressHandler);
		if (val != null)
		{
			if (m_startingContext == null)
			{
				OnProgress(val, value);
			}
			else
			{
				OnProgressInvokerCrossContext(val, value);
			}
		}
	}

	private void OnReportChainedProgress(object sender, TProgressInfo progressInfo)
	{
		((IProgress<TProgressInfo>)this).Report(progressInfo);
	}

	private int SetAsyncState(int newAsyncState, int conditionBitMask, bool useCondition, out bool conditionFailed)
	{
		return SetState(newAsyncState, -64, conditionBitMask, useCondition, out conditionFailed);
	}

	private int SetState(int newStateSetMask, int newStateIgnoreMask, int conditionBitMask, bool useCondition, out bool conditionFailed)
	{
		int state = m_state;
		if (useCondition && (state & conditionBitMask) == 0)
		{
			conditionFailed = true;
			return state;
		}
		int num = (state & newStateIgnoreMask) | newStateSetMask;
		int num2 = Interlocked.CompareExchange(ref m_state, num, state);
		while (true)
		{
			if (num2 == state)
			{
				conditionFailed = false;
				return num;
			}
			state = m_state;
			if (useCondition && (state & conditionBitMask) == 0)
			{
				break;
			}
			num = (state & newStateIgnoreMask) | newStateSetMask;
			num2 = Interlocked.CompareExchange(ref m_state, num, state);
		}
		conditionFailed = true;
		return state;
	}

	private int TransitionToTerminalState()
	{
		Task task = m_dataContainer as Task;
		int newAsyncState = 16;
		switch (task.Status)
		{
		case TaskStatus.RanToCompletion:
			newAsyncState = 2;
			break;
		case TaskStatus.Canceled:
			newAsyncState = 8;
			break;
		case TaskStatus.Faulted:
			newAsyncState = 16;
			break;
		}
		bool conditionFailed;
		return SetAsyncState(newAsyncState, 63, useCondition: true, out conditionFailed);
	}

	private void TaskCompleted()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		int state = TransitionToTerminalState();
		AsyncStatus status = GetStatus(state);
		try
		{
			if (m_startingContext == null)
			{
				OnCompletedInvoker(status);
				return;
			}
			m_startingContext.Post((object tupleObject) =>
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				Tuple<TaskToAsyncInfoAdapter<TCompletedHandler, TProgressHandler, TResult, TProgressInfo>, AsyncStatus> tuple = (Tuple<TaskToAsyncInfoAdapter<TCompletedHandler, TProgressHandler, TResult, TProgressInfo>, AsyncStatus>)tupleObject;
				tuple.Item1.OnCompletedInvoker(tuple.Item2);
			}, Tuple.Create<TaskToAsyncInfoAdapter<TCompletedHandler, TProgressHandler, TResult, TProgressInfo>, AsyncStatus>(this, status));
		}
		catch (Exception exception)
		{
			ExceptionDispatchHelper.ThrowAsync(exception, m_startingContext);
		}
	}

	private AsyncStatus GetStatus(int state)
	{
		switch (state & 0x3F)
		{
		case 0:
			return (AsyncStatus)3;
		case 1:
			return (AsyncStatus)0;
		case 2:
			return (AsyncStatus)1;
		case 4:
		case 8:
			return (AsyncStatus)2;
		case 16:
			return (AsyncStatus)3;
		case 32:
			return (AsyncStatus)3;
		default:
			return (AsyncStatus)3;
		}
	}

	internal TResult GetResultsInternal()
	{
		EnsureNotClosed();
		if (IsInErrorState)
		{
			Exception errorCode = ErrorCode;
			ExceptionDispatchInfo.Capture(errorCode).Throw();
		}
		if (!IsInRunToCompletionState)
		{
			throw CreateCannotGetResultsFromIncompleteOperationException(null);
		}
		if (CompletedSynchronously)
		{
			return (TResult)m_dataContainer;
		}
		if (!(m_dataContainer is Task<TResult> task))
		{
			return default;
		}
		try
		{
			return task.GetAwaiter().GetResult();
		}
		catch (TaskCanceledException cause)
		{
			throw CreateCannotGetResultsFromIncompleteOperationException(cause);
		}
	}

	private Task InvokeTaskProvider(Delegate taskProvider)
	{
		if (taskProvider is Func<Task> func)
		{
			return func();
		}
		if (taskProvider is Func<CancellationToken, Task> func2)
		{
			m_cancelTokenSource = new CancellationTokenSource();
			return func2(m_cancelTokenSource.Token);
		}
		if (taskProvider is Func<IProgress<TProgressInfo>, Task> func3)
		{
			return func3(this);
		}
		if (taskProvider is Func<CancellationToken, IProgress<TProgressInfo>, Task> func4)
		{
			m_cancelTokenSource = new CancellationTokenSource();
			return func4(m_cancelTokenSource.Token, this);
		}
		return null;
	}

	private void TransitionToClosed()
	{
		SetAsyncState(32, 0, useCondition: false, out var _);
		m_cancelTokenSource = null;
		m_dataContainer = null;
		m_error = null;
		m_completedHandler = null;
		m_progressHandler = null;
		m_startingContext = null;
	}

	public virtual void Cancel()
	{
		SetAsyncState(4, 1, useCondition: true, out var conditionFailed);
		if (!conditionFailed && m_cancelTokenSource != null)
		{
			m_cancelTokenSource.Cancel();
		}
	}

	public virtual void Close()
	{
		if (!IsInClosedState)
		{
			if (!IsInTerminalState && (m_state & 0x3F) != 0)
			{
				InvalidOperationException ex = new InvalidOperationException(SR.GetString("InvalidOperation_IllegalStateChange"));
				ex.SetErrorCode(-2147483635);
				throw ex;
			}
			TransitionToClosed();
		}
	}
}
