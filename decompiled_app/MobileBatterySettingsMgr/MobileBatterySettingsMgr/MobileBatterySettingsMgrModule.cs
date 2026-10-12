using CustomizationMgr.Interfaces;
using MobileBatterySettingsMgr.Models;
using MobileBatterySettingsMgr.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace MobileBatterySettingsMgr;

public class MobileBatterySettingsMgrModule : IModule
{
	public void OnInitialized(IContainerProvider containerProvider)
	{
		ICustomResourceManager customResourceManager = containerProvider.Resolve<ICustomResourceManager>();
		customResourceManager?.LoadComponentStrings("MobileBatterySettingsMgr");
		customResourceManager?.AddKnownCustomSettingType(typeof(BatteryChargeLimitCustomSetting), BatteryChargeLimitCustomSetting.GetDefaultSettingKingCounty());
		containerProvider.Resolve<IRegionManager>().RegisterViewWithRegion("BatteryChargeLimitRegion", typeof(BatteryChargeLimitView));
	}

	public void RegisterTypes(IContainerRegistry containerRegistry)
	{
	}
}
