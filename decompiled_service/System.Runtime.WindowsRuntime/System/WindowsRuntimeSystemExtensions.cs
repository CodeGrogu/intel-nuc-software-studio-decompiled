using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;

namespace System;

[CLSCompliant(false)]
[__DynamicallyInvokable]
public static class WindowsRuntimeSystemExtensions
{
	private static void ConcatenateCancelTokens(CancellationToken source, CancellationTokenSource sink, Task concatenationLifetime)
	{
		CancellationTokenRegistration cancellationTokenRegistration = source.Register((object state) =>
		{
			((CancellationTokenSource)state).Cancel();
		}, sink);
		concatenationLifetime.ContinueWith((Task _, object state) =>
		{
			((CancellationTokenRegistration)state).Dispose();
		}, cancellationTokenRegistration, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
	}

	private static void ConcatenateProgress<TProgress>(IAsyncActionWithProgress<TProgress> source, IProgress<TProgress> sink)
	{
		source.put_Progress((AsyncActionProgressHandler<TProgress>)(object)Delegate.Combine((Delegate)(object)source.Progress, (Delegate)(object)(AsyncActionProgressHandler<TProgress>)((IAsyncActionWithProgress<TProgress> _, TProgress info) =>
		{
			sink.Report(info);
		})));
	}

	private static void ConcatenateProgress<TResult, TProgress>(IAsyncOperationWithProgress<TResult, TProgress> source, IProgress<TProgress> sink)
	{
		source.put_Progress((AsyncOperationProgressHandler<TResult, TProgress>)(object)Delegate.Combine((Delegate)(object)source.Progress, (Delegate)(object)(AsyncOperationProgressHandler<TResult, TProgress>)((IAsyncOperationWithProgress<TResult, TProgress> _, TProgress info) =>
		{
			sink.Report(info);
		})));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[__DynamicallyInvokable]
	public static TaskAwaiter GetAwaiter(this IAsyncAction source)
	{
		return source.AsTask().GetAwaiter();
	}

	[__DynamicallyInvokable]
	public static Task AsTask(this IAsyncAction source)
	{
		return source.AsTask(CancellationToken.None);
	}

	[__DynamicallyInvokable]
	public static Task AsTask(this IAsyncAction source, CancellationToken cancellationToken)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected I4, but got Unknown
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Expected Obj, but got Unknown
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (source is TaskToAsyncActionAdapter { CompletedSynchronously: false, Task: var task } taskToAsyncActionAdapter)
		{
			if (!task.IsCompleted && cancellationToken.CanBeCanceled && taskToAsyncActionAdapter.CancelTokenSource != null)
			{
				ConcatenateCancelTokens(cancellationToken, taskToAsyncActionAdapter.CancelTokenSource, task);
			}
			return task;
		}
		AsyncStatus status = ((IAsyncInfo)source).Status;
		switch (status - 1)
		{
		case 0:
			return Task.CompletedTask;
		case 2:
			return Task.FromException(RestrictedErrorInfoHelper.AttachRestrictedErrorInfo(((IAsyncInfo)source).ErrorCode));
		case 1:
			return Task.FromCancellation(cancellationToken.IsCancellationRequested ? cancellationToken : new CancellationToken(canceled: true));
		default:
		{
			AsyncInfoToTaskBridge<VoidValueTypeParameter, VoidValueTypeParameter> asyncInfoToTaskBridge = new AsyncInfoToTaskBridge<VoidValueTypeParameter, VoidValueTypeParameter>();
			try
			{
				source.put_Completed((AsyncActionCompletedHandler)asyncInfoToTaskBridge.CompleteFromAsyncAction);
				asyncInfoToTaskBridge.RegisterForCancellation((IAsyncInfo)(object)source, cancellationToken);
			}
			catch
			{
				if (Task.s_asyncDebuggingEnabled)
				{
					Task.RemoveFromActiveTasks(asyncInfoToTaskBridge.Task.Id);
				}
				throw;
			}
			return asyncInfoToTaskBridge.Task;
		}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[__DynamicallyInvokable]
	public static TaskAwaiter<TResult> GetAwaiter<TResult>(this IAsyncOperation<TResult> source)
	{
		return source.AsTask<TResult>().GetAwaiter();
	}

	[__DynamicallyInvokable]
	public static Task<TResult> AsTask<TResult>(this IAsyncOperation<TResult> source)
	{
		return source.AsTask<TResult>(CancellationToken.None);
	}

	[__DynamicallyInvokable]
	public static Task<TResult> AsTask<TResult>(this IAsyncOperation<TResult> source, CancellationToken cancellationToken)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected I4, but got Unknown
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (source is TaskToAsyncOperationAdapter<TResult> { CompletedSynchronously: false } taskToAsyncOperationAdapter)
		{
			Task<TResult> task = taskToAsyncOperationAdapter.Task as Task<TResult>;
			if (!task.IsCompleted && cancellationToken.CanBeCanceled && taskToAsyncOperationAdapter.CancelTokenSource != null)
			{
				ConcatenateCancelTokens(cancellationToken, taskToAsyncOperationAdapter.CancelTokenSource, task);
			}
			return task;
		}
		AsyncStatus status = ((IAsyncInfo)source).Status;
		switch (status - 1)
		{
		case 0:
			return Task.FromResult(source.GetResults());
		case 2:
			return Task.FromException<TResult>(RestrictedErrorInfoHelper.AttachRestrictedErrorInfo(((IAsyncInfo)source).ErrorCode));
		case 1:
			return Task.FromCancellation<TResult>(cancellationToken.IsCancellationRequested ? cancellationToken : new CancellationToken(canceled: true));
		default:
		{
			AsyncInfoToTaskBridge<TResult, VoidValueTypeParameter> asyncInfoToTaskBridge = new AsyncInfoToTaskBridge<TResult, VoidValueTypeParameter>();
			try
			{
				source.put_Completed((AsyncOperationCompletedHandler<TResult>)asyncInfoToTaskBridge.CompleteFromAsyncOperation);
				asyncInfoToTaskBridge.RegisterForCancellation((IAsyncInfo)(object)source, cancellationToken);
			}
			catch
			{
				if (Task.s_asyncDebuggingEnabled)
				{
					Task.RemoveFromActiveTasks(asyncInfoToTaskBridge.Task.Id);
				}
				throw;
			}
			return asyncInfoToTaskBridge.Task;
		}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[__DynamicallyInvokable]
	public static TaskAwaiter GetAwaiter<TProgress>(this IAsyncActionWithProgress<TProgress> source)
	{
		return source.AsTask<TProgress>().GetAwaiter();
	}

	[__DynamicallyInvokable]
	public static Task AsTask<TProgress>(this IAsyncActionWithProgress<TProgress> source)
	{
		return source.AsTask<TProgress>(CancellationToken.None, null);
	}

	[__DynamicallyInvokable]
	public static Task AsTask<TProgress>(this IAsyncActionWithProgress<TProgress> source, CancellationToken cancellationToken)
	{
		return source.AsTask<TProgress>(cancellationToken, null);
	}

	[__DynamicallyInvokable]
	public static Task AsTask<TProgress>(this IAsyncActionWithProgress<TProgress> source, IProgress<TProgress> progress)
	{
		return source.AsTask(CancellationToken.None, progress);
	}

	[__DynamicallyInvokable]
	public static Task AsTask<TProgress>(this IAsyncActionWithProgress<TProgress> source, CancellationToken cancellationToken, IProgress<TProgress> progress)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected I4, but got Unknown
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (source is TaskToAsyncActionWithProgressAdapter<TProgress> { CompletedSynchronously: false, Task: var task } taskToAsyncActionWithProgressAdapter)
		{
			if (!task.IsCompleted)
			{
				if (cancellationToken.CanBeCanceled && taskToAsyncActionWithProgressAdapter.CancelTokenSource != null)
				{
					ConcatenateCancelTokens(cancellationToken, taskToAsyncActionWithProgressAdapter.CancelTokenSource, task);
				}
				if (progress != null)
				{
					ConcatenateProgress(source, progress);
				}
			}
			return task;
		}
		AsyncStatus status = ((IAsyncInfo)source).Status;
		switch (status - 1)
		{
		case 0:
			return Task.CompletedTask;
		case 2:
			return Task.FromException(RestrictedErrorInfoHelper.AttachRestrictedErrorInfo(((IAsyncInfo)source).ErrorCode));
		case 1:
			return Task.FromCancellation(cancellationToken.IsCancellationRequested ? cancellationToken : new CancellationToken(canceled: true));
		default:
		{
			if (progress != null)
			{
				ConcatenateProgress(source, progress);
			}
			AsyncInfoToTaskBridge<VoidValueTypeParameter, TProgress> asyncInfoToTaskBridge = new AsyncInfoToTaskBridge<VoidValueTypeParameter, TProgress>();
			try
			{
				source.put_Completed((AsyncActionWithProgressCompletedHandler<TProgress>)asyncInfoToTaskBridge.CompleteFromAsyncActionWithProgress);
				asyncInfoToTaskBridge.RegisterForCancellation((IAsyncInfo)(object)source, cancellationToken);
			}
			catch
			{
				if (Task.s_asyncDebuggingEnabled)
				{
					Task.RemoveFromActiveTasks(asyncInfoToTaskBridge.Task.Id);
				}
				throw;
			}
			return asyncInfoToTaskBridge.Task;
		}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[__DynamicallyInvokable]
	public static TaskAwaiter<TResult> GetAwaiter<TResult, TProgress>(this IAsyncOperationWithProgress<TResult, TProgress> source)
	{
		return source.AsTask<TResult, TProgress>().GetAwaiter();
	}

	[__DynamicallyInvokable]
	public static Task<TResult> AsTask<TResult, TProgress>(this IAsyncOperationWithProgress<TResult, TProgress> source)
	{
		return source.AsTask<TResult, TProgress>(CancellationToken.None, null);
	}

	[__DynamicallyInvokable]
	public static Task<TResult> AsTask<TResult, TProgress>(this IAsyncOperationWithProgress<TResult, TProgress> source, CancellationToken cancellationToken)
	{
		return source.AsTask<TResult, TProgress>(cancellationToken, null);
	}

	[__DynamicallyInvokable]
	public static Task<TResult> AsTask<TResult, TProgress>(this IAsyncOperationWithProgress<TResult, TProgress> source, IProgress<TProgress> progress)
	{
		return source.AsTask<TResult, TProgress>(CancellationToken.None, progress);
	}

	[__DynamicallyInvokable]
	public static Task<TResult> AsTask<TResult, TProgress>(this IAsyncOperationWithProgress<TResult, TProgress> source, CancellationToken cancellationToken, IProgress<TProgress> progress)
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected I4, but got Unknown
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (source is TaskToAsyncOperationWithProgressAdapter<TResult, TProgress> { CompletedSynchronously: false } taskToAsyncOperationWithProgressAdapter)
		{
			Task<TResult> task = taskToAsyncOperationWithProgressAdapter.Task as Task<TResult>;
			if (!task.IsCompleted)
			{
				if (cancellationToken.CanBeCanceled && taskToAsyncOperationWithProgressAdapter.CancelTokenSource != null)
				{
					ConcatenateCancelTokens(cancellationToken, taskToAsyncOperationWithProgressAdapter.CancelTokenSource, task);
				}
				if (progress != null)
				{
					ConcatenateProgress<TResult, TProgress>(source, progress);
				}
			}
			return task;
		}
		AsyncStatus status = ((IAsyncInfo)source).Status;
		switch (status - 1)
		{
		case 0:
			return Task.FromResult(source.GetResults());
		case 2:
			return Task.FromException<TResult>(RestrictedErrorInfoHelper.AttachRestrictedErrorInfo(((IAsyncInfo)source).ErrorCode));
		case 1:
			return Task.FromCancellation<TResult>(cancellationToken.IsCancellationRequested ? cancellationToken : new CancellationToken(canceled: true));
		default:
		{
			if (progress != null)
			{
				ConcatenateProgress<TResult, TProgress>(source, progress);
			}
			AsyncInfoToTaskBridge<TResult, TProgress> asyncInfoToTaskBridge = new AsyncInfoToTaskBridge<TResult, TProgress>();
			try
			{
				source.put_Completed((AsyncOperationWithProgressCompletedHandler<TResult, TProgress>)asyncInfoToTaskBridge.CompleteFromAsyncOperationWithProgress);
				asyncInfoToTaskBridge.RegisterForCancellation((IAsyncInfo)(object)source, cancellationToken);
			}
			catch
			{
				if (Task.s_asyncDebuggingEnabled)
				{
					Task.RemoveFromActiveTasks(asyncInfoToTaskBridge.Task.Id);
				}
				throw;
			}
			return asyncInfoToTaskBridge.Task;
		}
		}
	}

	[__DynamicallyInvokable]
	public static IAsyncAction AsAsyncAction(this Task source)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		return (IAsyncAction)(object)new TaskToAsyncActionAdapter(source, null);
	}

	[__DynamicallyInvokable]
	public static IAsyncOperation<TResult> AsAsyncOperation<TResult>(this Task<TResult> source)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		return new TaskToAsyncOperationAdapter<TResult>(source, null);
	}

	private static void CommonlyUsedGenericInstantiations()
	{
		((IAsyncOperation<bool>)null).AsTask<bool>();
		((IAsyncOperation<string>)null).AsTask<string>();
		((IAsyncOperation<object>)null).AsTask<object>();
		((IAsyncOperation<uint>)null).AsTask<uint>();
		((IAsyncOperationWithProgress<uint, uint>)null).AsTask<uint, uint>();
		((IAsyncOperationWithProgress<ulong, ulong>)null).AsTask<ulong, ulong>();
		((IAsyncOperationWithProgress<string, ulong>)null).AsTask<string, ulong>();
	}
}
