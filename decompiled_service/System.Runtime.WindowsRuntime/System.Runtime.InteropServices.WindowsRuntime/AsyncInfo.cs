using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;

namespace System.Runtime.InteropServices.WindowsRuntime;

[CLSCompliant(false)]
[__DynamicallyInvokable]
public static class AsyncInfo
{
	[__DynamicallyInvokable]
	public static IAsyncAction Run(Func<CancellationToken, Task> taskProvider)
	{
		if (taskProvider == null)
		{
			throw new ArgumentNullException("taskProvider");
		}
		return (IAsyncAction)(object)new TaskToAsyncActionAdapter(taskProvider);
	}

	[__DynamicallyInvokable]
	public static IAsyncActionWithProgress<TProgress> Run<TProgress>(Func<CancellationToken, IProgress<TProgress>, Task> taskProvider)
	{
		if (taskProvider == null)
		{
			throw new ArgumentNullException("taskProvider");
		}
		return new TaskToAsyncActionWithProgressAdapter<TProgress>(taskProvider);
	}

	[__DynamicallyInvokable]
	public static IAsyncOperation<TResult> Run<TResult>(Func<CancellationToken, Task<TResult>> taskProvider)
	{
		if (taskProvider == null)
		{
			throw new ArgumentNullException("taskProvider");
		}
		return new TaskToAsyncOperationAdapter<TResult>(taskProvider);
	}

	[__DynamicallyInvokable]
	public static IAsyncOperationWithProgress<TResult, TProgress> Run<TResult, TProgress>(Func<CancellationToken, IProgress<TProgress>, Task<TResult>> taskProvider)
	{
		if (taskProvider == null)
		{
			throw new ArgumentNullException("taskProvider");
		}
		return new TaskToAsyncOperationWithProgressAdapter<TResult, TProgress>(taskProvider);
	}

	internal static IAsyncAction CreateCompletedAction()
	{
		return (IAsyncAction)(object)new TaskToAsyncActionAdapter(isCanceled: false);
	}

	internal static IAsyncActionWithProgress<TProgress> CreateCompletedAction<TProgress>()
	{
		return new TaskToAsyncActionWithProgressAdapter<TProgress>(isCanceled: false);
	}

	internal static IAsyncOperation<TResult> CreateCompletedOperation<TResult>(TResult synchronousResult)
	{
		return new TaskToAsyncOperationAdapter<TResult>(synchronousResult);
	}

	internal static IAsyncOperationWithProgress<TResult, TProgress> CreateCompletedOperation<TResult, TProgress>(TResult synchronousResult)
	{
		return new TaskToAsyncOperationWithProgressAdapter<TResult, TProgress>(synchronousResult);
	}

	internal static IAsyncAction CreateFaultedAction(Exception error)
	{
		if (error == null)
		{
			throw new ArgumentNullException("error");
		}
		TaskToAsyncActionAdapter taskToAsyncActionAdapter = new TaskToAsyncActionAdapter(isCanceled: false);
		taskToAsyncActionAdapter.DangerousSetError(error);
		return (IAsyncAction)(object)taskToAsyncActionAdapter;
	}

	internal static IAsyncActionWithProgress<TProgress> CreateFaultedAction<TProgress>(Exception error)
	{
		if (error == null)
		{
			throw new ArgumentNullException("error");
		}
		TaskToAsyncActionWithProgressAdapter<TProgress> taskToAsyncActionWithProgressAdapter = new TaskToAsyncActionWithProgressAdapter<TProgress>(isCanceled: false);
		taskToAsyncActionWithProgressAdapter.DangerousSetError(error);
		return taskToAsyncActionWithProgressAdapter;
	}

	internal static IAsyncOperation<TResult> CreateFaultedOperation<TResult>(Exception error)
	{
		if (error == null)
		{
			throw new ArgumentNullException("error");
		}
		TaskToAsyncOperationAdapter<TResult> taskToAsyncOperationAdapter = new TaskToAsyncOperationAdapter<TResult>(default(TResult));
		taskToAsyncOperationAdapter.DangerousSetError(error);
		return taskToAsyncOperationAdapter;
	}

	internal static IAsyncOperationWithProgress<TResult, TProgress> CreateFaultedOperation<TResult, TProgress>(Exception error)
	{
		if (error == null)
		{
			throw new ArgumentNullException("error");
		}
		TaskToAsyncOperationWithProgressAdapter<TResult, TProgress> taskToAsyncOperationWithProgressAdapter = new TaskToAsyncOperationWithProgressAdapter<TResult, TProgress>(default(TResult));
		taskToAsyncOperationWithProgressAdapter.DangerousSetError(error);
		return taskToAsyncOperationWithProgressAdapter;
	}
}
