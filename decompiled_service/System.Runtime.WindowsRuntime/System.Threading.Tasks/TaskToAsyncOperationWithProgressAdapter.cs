using System.Runtime.CompilerServices;
using Windows.Foundation;

namespace System.Threading.Tasks;

internal class TaskToAsyncOperationWithProgressAdapter<TResult, TProgress> : TaskToAsyncInfoAdapter<AsyncOperationWithProgressCompletedHandler<TResult, TProgress>, AsyncOperationProgressHandler<TResult, TProgress>, TResult, TProgress>, IAsyncOperationWithProgress<TResult, TProgress>, IAsyncInfo
{
	internal TaskToAsyncOperationWithProgressAdapter(Delegate taskGenerator)
		: base(taskGenerator)
	{
	}

	internal TaskToAsyncOperationWithProgressAdapter(TResult synchronousResult)
		: base(synchronousResult)
	{
	}

	public virtual TResult GetResults()
	{
		return GetResultsInternal();
	}

	internal override void OnCompleted(AsyncOperationWithProgressCompletedHandler<TResult, TProgress> userCompletionHandler, AsyncStatus asyncStatus)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		userCompletionHandler.Invoke((IAsyncOperationWithProgress<TResult, TProgress>)this, asyncStatus);
	}

	internal override void OnProgress(AsyncOperationProgressHandler<TResult, TProgress> userProgressHandler, TProgress progressInfo)
	{
		userProgressHandler.Invoke((IAsyncOperationWithProgress<TResult, TProgress>)this, progressInfo);
	}

	[SpecialName]
	void IAsyncOperationWithProgress<TResult, TProgress>.put_Progress(AsyncOperationProgressHandler<TResult, TProgress> handler)
	{
		Progress = handler;
	}

	[SpecialName]
	void IAsyncOperationWithProgress<TResult, TProgress>.put_Completed(AsyncOperationWithProgressCompletedHandler<TResult, TProgress> handler)
	{
		Completed = handler;
	}
}
