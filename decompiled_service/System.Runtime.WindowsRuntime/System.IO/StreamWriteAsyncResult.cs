using Windows.Foundation;

namespace System.IO;

internal class StreamWriteAsyncResult : StreamOperationAsyncResult
{
	internal StreamWriteAsyncResult(IAsyncOperationWithProgress<uint, uint> asyncStreamWriteOperation, AsyncCallback userCompletionCallback, object userAsyncStateInfo, bool processCompletedOperationInCallback)
		: base((IAsyncInfo)(object)asyncStreamWriteOperation, userCompletionCallback, userAsyncStateInfo, processCompletedOperationInCallback)
	{
		asyncStreamWriteOperation.put_Completed((AsyncOperationWithProgressCompletedHandler<uint, uint>)StreamOperationCompletedCallback);
	}

	internal override void ProcessConcreteCompletedOperation(IAsyncInfo completedOperation, out long bytesCompleted)
	{
		ProcessConcreteCompletedOperation((IAsyncOperationWithProgress<uint, uint>)(object)completedOperation, out bytesCompleted);
	}

	private void ProcessConcreteCompletedOperation(IAsyncOperationWithProgress<uint, uint> completedOperation, out long bytesCompleted)
	{
		uint results = completedOperation.GetResults();
		bytesCompleted = results;
	}
}
