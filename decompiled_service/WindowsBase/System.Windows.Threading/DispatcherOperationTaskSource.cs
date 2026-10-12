using System.Threading.Tasks;

namespace System.Windows.Threading;

internal abstract class DispatcherOperationTaskSource
{
	public abstract void Initialize(DispatcherOperation operation);

	public abstract Task GetTask();

	public abstract void SetCanceled();

	public abstract void SetResult(object result);

	public abstract void SetException(Exception exception);

	protected DispatcherOperationTaskSource()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
internal class DispatcherOperationTaskSource<TResult> : DispatcherOperationTaskSource
{
	public override void Initialize(DispatcherOperation operation)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override Task GetTask()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override void SetCanceled()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override void SetResult(object result)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override void SetException(Exception exception)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public DispatcherOperationTaskSource()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
