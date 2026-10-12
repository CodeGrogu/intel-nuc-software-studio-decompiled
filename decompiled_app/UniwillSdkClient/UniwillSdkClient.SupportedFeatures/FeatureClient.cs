using System.Collections.Generic;

namespace UniwillSdkClient.SupportedFeatures;

public class FeatureClient
{
	public List<Feature> GetSupportedFeatures()
	{
		return new List<Feature>
		{
			new Feature(FeatureType.KeyboardLedIndividualKeyControl, new List<FeatureInterfaceType> { FeatureInterfaceType.OdmSdk }, 2, 1),
			new Feature(FeatureType.LightbarSingleColor, new List<FeatureInterfaceType> { FeatureInterfaceType.OdmSdk }, 2, 2),
			new Feature(FeatureType.CpuPowerMode, new List<FeatureInterfaceType> { FeatureInterfaceType.OdmSdk }, 1, 1),
			new Feature(FeatureType.FanPowerMode, new List<FeatureInterfaceType> { FeatureInterfaceType.OdmSdk }, 1, 1),
			new Feature(FeatureType.SystemMonitor, new List<FeatureInterfaceType> { FeatureInterfaceType.OdmSdk }, 0, 0),
			new Feature(FeatureType.DisplaySettings, new List<FeatureInterfaceType> { FeatureInterfaceType.OdmSdk }, 2, 0),
			new Feature(FeatureType.KeyWinLock, new List<FeatureInterfaceType> { FeatureInterfaceType.OdmSdk }, 1, 0),
			new Feature(FeatureType.KeyFnLock, new List<FeatureInterfaceType> { FeatureInterfaceType.OdmSdk }, 1, 0),
			new Feature(FeatureType.KeyNumPad, new List<FeatureInterfaceType> { FeatureInterfaceType.OdmSdk }, 1, 0),
			new Feature(FeatureType.KeyOSD, new List<FeatureInterfaceType> { FeatureInterfaceType.OdmSdk }, 0, 0),
			new Feature(FeatureType.BatteryChargeLimit, new List<FeatureInterfaceType> { FeatureInterfaceType.OdmSdk }, 0, 0)
		};
	}
}
