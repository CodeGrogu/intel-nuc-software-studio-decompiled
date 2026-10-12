using System;
using System.Collections.Generic;
using System.Configuration;
using System.Management;
using System.ServiceProcess;
using FeatureMgr.Enums;
using FeatureMgr.Interfaces;
using Infrastructure.ErrorHandling;
using Infrastructure.Models;
using Infrastructure.Models.FeatureMgr;
using Infrastructure.Utilities;
using IntelSdkClient.AppSettingsService;
using IntelSdkClient.SupportedFeatures;
using Prism.Events;
using Prism.Logging;
using UniwillSdkClient.Interfaces;
using UniwillSdkClient.UniwillService;

namespace FeatureMgr.Models;

public class SdkManager : ISdkManager
{
	private const string UNIWILL_SERVICE_NAME = "UniwillService";

	private const string NSS_SERVICE_NAME = "NucSoftwareStudioService";

	private readonly ILoggerFacade _logger;

	private readonly IUniwillClient _uniwillClient;

	private static readonly TimeSpan SERVICE_WAIT_TIMEOUT = TimeSpan.FromMinutes(1.0);

	public SdkType Sdk { get; }

	public List<Infrastructure.Models.FeatureMgr.Feature> SupportedFeatures { get; }

	public SdkManager(IUniwillClient uniwillClient, IEventAggregator events, ILoggerFacade logger)
	{
		_logger = logger;
		Sdk = GetSdkType(events);
		if (Sdk == SdkType.Uniwill)
		{
			_uniwillClient = uniwillClient;
			_uniwillClient.Initialize();
		}
		SupportedFeatures = GetFeaturesFromSdk(Sdk);
	}

	private SdkType GetSdkType(IEventAggregator events)
	{
		ErrorHandler.SetEvent(events, _logger);
		SdkType supportedSdk = GetSupportedSdk();
		string text;
		switch (supportedSdk)
		{
		case SdkType.Unknown:
			ErrorHandler.PublishError("DeviceNotSupported", ErrorType.DEVICE_NOT_SUPPORTED, critical: true, msgIsKey: true);
			return SdkType.Unknown;
		default:
			text = "NucSoftwareStudioService";
			break;
		case SdkType.Uniwill:
			text = "UniwillService";
			break;
		}
		string text2 = text;
		if (ServiceInstalled(text2, out var svcCtl))
		{
			if (ServiceRunning(svcCtl))
			{
				if (!(text2 == "NucSoftwareStudioService"))
				{
					return supportedSdk;
				}
				if (PerformanceDriverInstalled())
				{
					return supportedSdk;
				}
				ErrorHandler.PublishError("InstallPerformanceDriver", ErrorType.PERFORMNACEDRIVER_NOT_INSTALLED, critical: true, msgIsKey: true);
			}
			ErrorHandler.PublishError("Run" + text2, ErrorType.SERVICE_NOT_RUNNING, critical: true, msgIsKey: true);
		}
		else
		{
			ErrorHandler.PublishError("Install" + text2, ErrorType.SERVICE_NOT_INSTALLED, critical: true, msgIsKey: true);
		}
		return SdkType.Unknown;
	}

	private bool PerformanceDriverInstalled()
	{
		return new ManagementObjectSearcher(new SelectQuery("Win32_SystemDriver")
		{
			Condition = "Name = 'PerformanceDriver'"
		}).Get().Count > 0;
	}

	private SdkType GetSupportedSdk()
	{
		HashSet<string> hashSet = new HashSet<string>(Array.ConvertAll(ConfigurationManager.AppSettings.Get("NssServiceSupportedBiosIds").Split(','), (string p) => p.Trim()));
		HashSet<string> hashSet2 = new HashSet<string>(Array.ConvertAll(ConfigurationManager.AppSettings.Get("UniwillSupportedBiosIds").Split(','), (string p) => p.Trim()));
		string item = SysInfo.GetItem(SysItemName.BIOS).Substring(0, 2);
		if (hashSet.Contains(item))
		{
			return SdkType.Intel;
		}
		if (hashSet2.Contains(item))
		{
			return SdkType.Uniwill;
		}
		return SdkType.Unknown;
	}

	private bool ServiceInstalled(string serviceName, out ServiceController svcCtl)
	{
		svcCtl = Array.Find(ServiceController.GetServices(), (ServiceController s) => s.ServiceName == serviceName);
		return svcCtl != null;
	}

	private bool ServiceRunning(ServiceController svcCtl)
	{
		if (svcCtl == null)
		{
			return false;
		}
		if (svcCtl.Status == ServiceControllerStatus.Running)
		{
			return true;
		}
		try
		{
			svcCtl.WaitForStatus(ServiceControllerStatus.Running, SERVICE_WAIT_TIMEOUT);
			return true;
		}
		catch (System.ServiceProcess.TimeoutException exception)
		{
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
			return false;
		}
	}

	private List<Infrastructure.Models.FeatureMgr.Feature> GetFeaturesFromSdk(SdkType sdk)
	{
		return sdk switch
		{
			SdkType.Intel => IntelFeatures(), 
			SdkType.Uniwill => UniwillFeatures(), 
			_ => new List<Infrastructure.Models.FeatureMgr.Feature>(), 
		};
	}

	private List<Infrastructure.Models.FeatureMgr.Feature> IntelFeatures()
	{
		List<Infrastructure.Models.FeatureMgr.Feature> list = new List<Infrastructure.Models.FeatureMgr.Feature>();
		foreach (IntelSdkClient.AppSettingsService.Feature supportedFeature in new FeatureClient().GetSupportedFeatures())
		{
			List<Infrastructure.Models.FeatureMgr.FeatureInterfaceType> list2 = new List<Infrastructure.Models.FeatureMgr.FeatureInterfaceType>();
			IntelSdkClient.AppSettingsService.FeatureInterfaceType[] supportedInterfaces = supportedFeature.SupportedInterfaces;
			foreach (IntelSdkClient.AppSettingsService.FeatureInterfaceType item in supportedInterfaces)
			{
				list2.Add((Infrastructure.Models.FeatureMgr.FeatureInterfaceType)item);
			}
			Infrastructure.Models.FeatureMgr.Feature item2 = new Infrastructure.Models.FeatureMgr.Feature((Infrastructure.Models.FeatureMgr.FeatureType)supportedFeature.Type, list2, supportedFeature.PriorityLevel, supportedFeature.Index);
			list.Add(item2);
		}
		return list;
	}

	private List<Infrastructure.Models.FeatureMgr.Feature> UniwillFeatures()
	{
		List<Infrastructure.Models.FeatureMgr.Feature> list = new List<Infrastructure.Models.FeatureMgr.Feature>();
		UniwillSdkClient.UniwillService.Feature[] supportedFeatures = _uniwillClient.Client.GetSupportedFeatures();
		foreach (UniwillSdkClient.UniwillService.Feature feature in supportedFeatures)
		{
			List<Infrastructure.Models.FeatureMgr.FeatureInterfaceType> list2 = new List<Infrastructure.Models.FeatureMgr.FeatureInterfaceType>();
			UniwillSdkClient.UniwillService.FeatureInterfaceType[] supportedInterfaces = feature.SupportedInterfaces;
			foreach (UniwillSdkClient.UniwillService.FeatureInterfaceType item in supportedInterfaces)
			{
				list2.Add((Infrastructure.Models.FeatureMgr.FeatureInterfaceType)item);
			}
			Infrastructure.Models.FeatureMgr.Feature item2 = new Infrastructure.Models.FeatureMgr.Feature((Infrastructure.Models.FeatureMgr.FeatureType)feature.Type, list2, feature.PriorityLevel, feature.Index);
			list.Add(item2);
		}
		return list;
	}
}
