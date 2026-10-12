using System.Runtime.InteropServices;

namespace System.Threading;

[ComImport]
[Guid("603E88E4-A338-4FFE-A457-A5CFB9CEB899")]
internal interface IDispatcherQueue
{
	object CreateTimer();

	bool TryEnqueue(DispatcherQueueHandler callback);

	bool TryEnqueue(DispatcherQueuePriority priority, DispatcherQueueHandler callback);
}
