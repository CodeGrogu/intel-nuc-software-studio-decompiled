using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using SystemMonitorMgr;
using SystemPerformanceMgr;
using AlexaLightBarMgr;
using CustomizationMgr.Interfaces;
using CustomizationMgr.Models;
using FeatureMgr;
using Infrastructure.ErrorHandling;
using Infrastructure.Models;
using MobileBatterySettingsMgr;
using MobileDisplayModeMgr;
using MobileKeyboardLedMgr;
using MobileKeyboardSettingsMgr;
using MobileLightBarLedMgr;
using MobilePerformanceMgr;
using NucLedMgr;
using NucSoftwareStudio.Interfaces;
using NucSoftwareStudio.Models;
using NucSoftwareStudio.Views;
using NucSoftwareStudio.WcfService;
using Prism.Events;
using Prism.Ioc;
using Prism.Logging;
using Prism.Modularity;
using Prism.Unity;
using UniwillSdkClient.Events;
using UserPresenceCstMgr;
using UserPresenceMgr;
using Windows.Storage;

namespace NucSoftwareStudio;

public class App : PrismApplication
{
	private const string MUTEX_NAME = "NucSoftwareStudio";

	private const string APP_NAME = "Intel® NUC Software Studio";

	private const string TRAY_COMMANDLINE_ARG = "tray";

	private const string LOEM_CUSTOMIZATION_DEFAULTS_COMMANDLINE_ARG = "GenerateDefaultCustomizationFile";

	private Mutex _mutex;

	private IEventAggregator _events;

	protected override Window CreateShell()
	{
		_mutex = new Mutex(initiallyOwned: true, "NucSoftwareStudio", out var createdNew);
		if (!createdNew)
		{
			if (!MainWindowLauncher.SetRunningAppToForeground("Intel® NUC Software Studio"))
			{
				NssAppServiceManager.SendTrayAppLaunchMessage();
			}
			Environment.Exit(0);
		}
		return Container.Resolve<MainWindow>();
	}

	protected override void OnInitialized()
	{
		ErrorHandler.SetPrompt(((MainWindow)MainWindow).ErrorPrompt);
		Container.Resolve<INssAppServiceManager>();
		Container.Resolve<ICustomResourceManager>().LoadCustomResources();
		Container.Resolve<ITrayApp>().SetMainWindow(MainWindow);
		_events = Container.Resolve<IEventAggregator>();
		string[] commandLineArgs = Environment.GetCommandLineArgs();
		if (commandLineArgs.Length > 1 && commandLineArgs[1].Equals("tray"))
		{
			_events.GetEvent<SubscribeEvent>().Publish(payload: false);
		}
		else if (commandLineArgs.Length > 1 && commandLineArgs[1].Equals("GenerateDefaultCustomizationFile"))
		{
			Container.Resolve<ICustomResourceManager>().GenerateDefaultFile();
			Environment.Exit(0);
		}
		else
		{
			base.OnInitialized();
		}
	}

	protected override void OnSessionEnding(SessionEndingCancelEventArgs e)
	{
		try
		{
			_events.GetEvent<SubscribeEvent>().Publish(payload: false);
			ApplicationData.Current.LocalSettings.Values["FirstLaunch"] = false;
		}
		catch (Exception)
		{
		}
		base.OnSessionEnding(e);
	}

	protected override void OnExit(ExitEventArgs e)
	{
		try
		{
			_events.GetEvent<SubscribeEvent>().Publish(payload: false);
			ApplicationData.Current.LocalSettings.Values["FirstLaunch"] = false;
			ApplicationData.Current.LocalSettings.Values["isFreshLaunch"] = false;
		}
		catch (Exception)
		{
		}
		base.OnExit(e);
	}

	protected override void RegisterTypes(IContainerRegistry containerRegistry)
	{
		containerRegistry.RegisterSingleton<INssAppServiceManager, NssAppServiceManager>();
		containerRegistry.RegisterSingleton<ICustomResourceManager, CustomResourceManager>();
		containerRegistry.RegisterSingleton<ILoggerFacade, NssDebugLogger>();
		containerRegistry.RegisterSingleton<ITrayApp, TrayApp>();
	}

	protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
	{
		moduleCatalog.AddModule<FeatureMgrModule>();
		Type typeFromHandle = typeof(AlexaLightBarMgrModule);
		moduleCatalog.AddModule(new ModuleInfo
		{
			ModuleName = typeFromHandle.Name,
			ModuleType = typeFromHandle.AssemblyQualifiedName,
			InitializationMode = InitializationMode.OnDemand
		});
		Type typeFromHandle2 = typeof(MobileBatterySettingsMgrModule);
		moduleCatalog.AddModule(new ModuleInfo
		{
			ModuleName = typeFromHandle2.Name,
			ModuleType = typeFromHandle2.AssemblyQualifiedName,
			InitializationMode = InitializationMode.OnDemand
		});
		Type typeFromHandle3 = typeof(MobileDisplayModeMgrModule);
		moduleCatalog.AddModule(new ModuleInfo
		{
			ModuleName = typeFromHandle3.Name,
			ModuleType = typeFromHandle3.AssemblyQualifiedName,
			InitializationMode = InitializationMode.OnDemand
		});
		Type typeFromHandle4 = typeof(MobileKeyboardLedMgrModule);
		moduleCatalog.AddModule(new ModuleInfo
		{
			ModuleName = typeFromHandle4.Name,
			ModuleType = typeFromHandle4.AssemblyQualifiedName,
			InitializationMode = InitializationMode.OnDemand
		});
		Type typeFromHandle5 = typeof(MobileKeyboardSettingsMgrModule);
		moduleCatalog.AddModule(new ModuleInfo
		{
			ModuleName = typeFromHandle5.Name,
			ModuleType = typeFromHandle5.AssemblyQualifiedName,
			InitializationMode = InitializationMode.OnDemand
		});
		Type typeFromHandle6 = typeof(MobileLightBarLedMgrModule);
		moduleCatalog.AddModule(new ModuleInfo
		{
			ModuleName = typeFromHandle6.Name,
			ModuleType = typeFromHandle6.AssemblyQualifiedName,
			InitializationMode = InitializationMode.OnDemand
		});
		Type typeFromHandle7 = typeof(MobilePerformanceMgrModule);
		moduleCatalog.AddModule(new ModuleInfo
		{
			ModuleName = typeFromHandle7.Name,
			ModuleType = typeFromHandle7.AssemblyQualifiedName,
			InitializationMode = InitializationMode.OnDemand
		});
		Type typeFromHandle8 = typeof(NucLedMgrModule);
		moduleCatalog.AddModule(new ModuleInfo
		{
			ModuleName = typeFromHandle8.Name,
			ModuleType = typeFromHandle8.AssemblyQualifiedName,
			InitializationMode = InitializationMode.OnDemand
		});
		Type typeFromHandle9 = typeof(SystemMonitorMgrModule);
		moduleCatalog.AddModule(new ModuleInfo
		{
			ModuleName = typeFromHandle9.Name,
			ModuleType = typeFromHandle9.AssemblyQualifiedName,
			InitializationMode = InitializationMode.OnDemand
		});
		Type typeFromHandle10 = typeof(SystemPerformanceMgrModule);
		moduleCatalog.AddModule(new ModuleInfo
		{
			ModuleName = typeFromHandle10.Name,
			ModuleType = typeFromHandle10.AssemblyQualifiedName,
			InitializationMode = InitializationMode.OnDemand
		});
		Type typeFromHandle11 = typeof(UserPresenceMgrModule);
		moduleCatalog.AddModule(new ModuleInfo
		{
			ModuleName = typeFromHandle11.Name,
			ModuleType = typeFromHandle11.AssemblyQualifiedName,
			InitializationMode = InitializationMode.OnDemand
		});
		Type typeFromHandle12 = typeof(UserPresenceCstMgrModule);
		moduleCatalog.AddModule(new ModuleInfo
		{
			ModuleName = typeFromHandle12.Name,
			ModuleType = typeFromHandle12.AssemblyQualifiedName,
			InitializationMode = InitializationMode.OnDemand
		});
	}

	protected override void InitializeModules()
	{
		base.InitializeModules();
	}

	[STAThread]
	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public static void Main()
	{
		new App().Run();
	}
}
