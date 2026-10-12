using Windows.Foundation;

namespace System.IO;

internal class StreamFlushAsyncResult : StreamOperationAsyncResult
{
	internal StreamFlushAsyncResult(IAsyncOperation<bool> asyncStreamFlushOperation, bool processCompletedOperationInCallback)
		: base((IAsyncInfo)(object)asyncStreamFlushOperation, null, null, processCompletedOperationInCallback)
	{
		asyncStreamFlushOperation.put_Completed((AsyncOperationCompletedHandler<bool>)StreamOperationCompletedCallback);
	}

	internal override void ProcessConcreteCompletedOperation(IAsyncInfo completedOperation, out long bytesCompleted)
	{
		ProcessConcreteCompletedOperation((IAsyncOperation<bool>)(object)completedOperation, out bytesCompleted);
	}

	private void ProcessConcreteCompletedOperation(IAsyncOperation<bool> completedOperation, out long bytesCompleted)
	{
		bool results = completedOperation.GetResults();
		bytesCompleted = ((!results) ? (-1) : 0);
	}
}
