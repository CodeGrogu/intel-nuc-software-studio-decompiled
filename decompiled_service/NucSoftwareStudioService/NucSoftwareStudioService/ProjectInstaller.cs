using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace NucSoftwareStudioService;

[RunInstaller(true)]
public class ProjectInstaller : Installer
{
	private IContainer components;

	private ServiceProcessInstaller serviceProcessInstaller1;

	private ServiceInstaller serviceInstaller1;

	public ProjectInstaller()
	{
		InitializeComponent();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		serviceProcessInstaller1 = new ServiceProcessInstaller();
		serviceInstaller1 = new ServiceInstaller();
		serviceProcessInstaller1.Password = null;
		serviceProcessInstaller1.Username = null;
		serviceInstaller1.DisplayName = "Intel® NUC Software Studio Service";
		serviceInstaller1.ServiceName = "NssService";
		serviceInstaller1.StartType = ServiceStartMode.Automatic;
		Installers.AddRange(new Installer[2] { serviceProcessInstaller1, serviceInstaller1 });
	}
}
