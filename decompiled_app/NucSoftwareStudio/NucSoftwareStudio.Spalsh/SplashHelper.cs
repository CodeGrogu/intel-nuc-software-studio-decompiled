using System.Threading;
using System.Windows.Threading;

namespace NucSoftwareStudio.Spalsh;

public static class SplashHelper
{
	public static void AnimateSplash()
	{
		for (int i = 0; i < 3000; i++)
		{
			DispatcherFrame dispatcherFrame = new DispatcherFrame();
			Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(ExitFrames), dispatcherFrame);
			Dispatcher.PushFrame(dispatcherFrame);
			Thread.Sleep(1);
		}
	}

	private static object ExitFrames(object frame)
	{
		((DispatcherFrame)frame).Continue = false;
		return null;
	}
}
