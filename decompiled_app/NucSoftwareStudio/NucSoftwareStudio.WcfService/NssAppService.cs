using System;
using System.ServiceModel;
using System.Windows;
using System.Windows.Threading;

namespace NucSoftwareStudio.WcfService;

[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
public class NssAppService : INssAppServiceContract
{
	public const string URI = "net.pipe://localhost/NssTrayApp/service";

	public const string ENDPOINT = "NssTrayAppEndpoint";

	public const string ADDRESS = "net.pipe://localhost/NssTrayApp/service/NssTrayAppEndpoint";

	public void ShowMainWindow()
	{
		Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, (Action)(() =>
		{
			Application.Current.MainWindow.WindowState = WindowState.Normal;
			Application.Current.MainWindow.Show();
			Application.Current.MainWindow.Activate();
		}));
	}
}
