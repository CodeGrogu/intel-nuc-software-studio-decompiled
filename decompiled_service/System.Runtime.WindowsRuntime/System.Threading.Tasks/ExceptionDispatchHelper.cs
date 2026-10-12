using System.Runtime.ExceptionServices;

namespace System.Threading.Tasks;

internal static class ExceptionDispatchHelper
{
	internal static void ThrowAsync(Exception exception, SynchronizationContext targetContext)
	{
		if (exception == null || exception is ThreadAbortException)
		{
			return;
		}
		ExceptionDispatchInfo exceptionDispatchInfo = ExceptionDispatchInfo.Capture(exception);
		if (targetContext != null)
		{
			try
			{
				targetContext.Post((object edi) =>
				{
					((ExceptionDispatchInfo)edi).Throw();
				}, exceptionDispatchInfo);
				return;
			}
			catch
			{
				ThrowAsync(exception, null);
				return;
			}
		}
		bool flag;
		try
		{
			flag = ThreadPool.QueueUserWorkItem((object edi) =>
			{
				((ExceptionDispatchInfo)edi).Throw();
			}, exceptionDispatchInfo);
		}
		catch
		{
			flag = false;
		}
		if (!flag)
		{
			exceptionDispatchInfo.Throw();
		}
	}
}
