using Windows.Foundation;
using Windows.Storage.Streams;

namespace System.IO;

internal class StreamReadAsyncResult : StreamOperationAsyncResult
{
	private IBuffer userBuffer;

	internal StreamReadAsyncResult(IAsyncOperationWithProgress<IBuffer, uint> asyncStreamReadOperation, IBuffer buffer, AsyncCallback userCompletionCallback, object userAsyncStateInfo, bool processCompletedOperationInCallback)
		: base((IAsyncInfo)(object)asyncStreamReadOperation, userCompletionCallback, userAsyncStateInfo, processCompletedOperationInCallback)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		userBuffer = buffer;
		asyncStreamReadOperation.put_Completed((AsyncOperationWithProgressCompletedHandler<IBuffer, uint>)StreamOperationCompletedCallback);
	}

	internal override void ProcessConcreteCompletedOperation(IAsyncInfo completedOperation, out long bytesCompleted)
	{
		ProcessConcreteCompletedOperation((IAsyncOperationWithProgress<IBuffer, uint>)(object)completedOperation, out bytesCompleted);
	}

	private void ProcessConcreteCompletedOperation(IAsyncOperationWithProgress<IBuffer, uint> completedOperation, out long bytesCompleted)
	{
		IBuffer results = completedOperation.GetResults();
		WinRtIOHelper.EnsureResultsInUserBuffer(userBuffer, results);
		bytesCompleted = userBuffer.Length;
	}
}
