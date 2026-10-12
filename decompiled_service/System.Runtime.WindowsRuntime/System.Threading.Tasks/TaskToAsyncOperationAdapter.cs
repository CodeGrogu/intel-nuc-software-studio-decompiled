using System.Runtime.CompilerServices;
using Windows.Foundation;

namespace System.Threading.Tasks;

internal class TaskToAsyncOperationAdapter<TResult> : TaskToAsyncInfoAdapter<AsyncOperationCompletedHandler<TResult>, VoidReferenceTypeParameter, TResult, VoidValueTypeParameter>, IAsyncOperation<TResult>, IAsyncInfo
{
	internal TaskToAsyncOperationAdapter(Delegate taskGenerator)
		: base(taskGenerator)
	{
	}

	internal TaskToAsyncOperationAdapter(Task underlyingTask, CancellationTokenSource underlyingCancelTokenSource)
		: base(underlyingTask, underlyingCancelTokenSource, (Progress<VoidValueTypeParameter>)null)
	{
	}

	internal TaskToAsyncOperationAdapter(TResult synchronousResult)
		: base(synchronousResult)
	{
	}

	public virtual TResult GetResults()
	{
		return GetResultsInternal();
	}

	internal override void OnCompleted(AsyncOperationCompletedHandler<TResult> userCompletionHandler, AsyncStatus asyncStatus)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		userCompletionHandler.Invoke((IAsyncOperation<TResult>)this, asyncStatus);
	}

	[SpecialName]
	void IAsyncOperation<TResult>.put_Completed(AsyncOperationCompletedHandler<TResult> handler)
	{
		Completed = handler;
	}
}
