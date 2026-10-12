using System.Runtime.CompilerServices;
using Windows.Foundation;

namespace System.Threading.Tasks;

internal class TaskToAsyncActionAdapter : TaskToAsyncInfoAdapter<AsyncActionCompletedHandler, VoidReferenceTypeParameter, VoidValueTypeParameter, VoidValueTypeParameter>, IAsyncAction, IAsyncInfo
{
	internal TaskToAsyncActionAdapter(Delegate taskGenerator)
		: base(taskGenerator)
	{
	}

	internal TaskToAsyncActionAdapter(Task underlyingTask, CancellationTokenSource underlyingCancelTokenSource)
		: base(underlyingTask, underlyingCancelTokenSource, (Progress<VoidValueTypeParameter>)null)
	{
	}

	internal TaskToAsyncActionAdapter(bool isCanceled)
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

	internal override void OnCompleted(AsyncActionCompletedHandler userCompletionHandler, AsyncStatus asyncStatus)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		userCompletionHandler.Invoke((IAsyncAction)(object)this, asyncStatus);
	}

	[SpecialName]
	void IAsyncAction.put_Completed(AsyncActionCompletedHandler handler)
	{
		Completed = handler;
	}
}
