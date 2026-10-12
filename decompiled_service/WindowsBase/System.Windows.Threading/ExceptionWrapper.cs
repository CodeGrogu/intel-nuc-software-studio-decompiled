using System.Runtime.CompilerServices;

namespace System.Windows.Threading;

internal class ExceptionWrapper
{
	public delegate bool CatchHandler(object source, Exception e);

	public delegate bool FilterHandler(object source, Exception e);

	public event CatchHandler Catch
	{
		[CompilerGenerated]
		add
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		[CompilerGenerated]
		remove
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public event FilterHandler Filter
	{
		[CompilerGenerated]
		add
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		[CompilerGenerated]
		remove
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal ExceptionWrapper()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public object TryCatchWhen(object source, Delegate callback, object args, int numArgs, Delegate catchHandler)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
