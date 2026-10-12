using System.Runtime.CompilerServices;
using Windows.Foundation;

namespace System.Threading.Tasks;

internal class TaskToAsyncActionWithProgressAdapter<TProgress> : TaskToAsyncInfoAdapter<AsyncActionWithProgressCompletedHandler<TProgress>, AsyncActionProgressHandler<TProgress>, VoidValueTypeParameter, TProgress>, IAsyncActionWithProgress<TProgress>, IAsyncInfo
{
	internal TaskToAsyncActionWithProgressAdapter(Delegate taskGenerator)
		: base(taskGenerator)
	{
	}

	internal TaskToAsyncActionWithProgressAdapter(bool isCanceled)
		: base(default(VoidValueTypeParameter))
	{
		if (isCanceled)
		{
			DangerousSetCanceled();
		}
	}

	public virtual void GetResults()
	{
		GetResultsInternal();
	}

	internal override void OnCompleted(AsyncActionWithProgressCompletedHandler<TProgress> userCompletionHandler, AsyncStatus asyncStatus)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		userCompletionHandler.Invoke((IAsyncActionWithProgress<TProgress>)this, asyncStatus);
	}

	internal override void OnProgress(AsyncActionProgressHandler<TProgress> userProgressHandler, TProgress progressInfo)
	{
		userProgressHandler.Invoke((IAsyncActionWithProgress<TProgress>)this, progressInfo);
	}

	[SpecialName]
	void IAsyncActionWithProgress<TProgress>.put_Progress(AsyncActionProgressHandler<TProgress> handler)
	{
		Progress = handler;
	}

	[SpecialName]
	void IAsyncActionWithProgress<TProgress>.put_Completed(AsyncActionWithProgressCompletedHandler<TProgress> handler)
	{
		Completed = handler;
	}
}
