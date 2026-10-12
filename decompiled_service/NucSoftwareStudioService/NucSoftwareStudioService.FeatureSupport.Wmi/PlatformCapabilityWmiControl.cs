using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Infrastructure.Logging;
using Infrastructure.WMI;
using NucSoftwareStudioService.FeatureSupport.Models;

namespace NucSoftwareStudioService.FeatureSupport.Wmi;

public static class PlatformCapabilityWmiControl
{
	public static List<Feature> GetSupportedFeatures(NssServiceLogger logger = null)
	{
		List<Feature> list = new List<Feature>();
		byte[] array = QueryPlatformCapability(logger);
		if (array.Length == 0)
		{
			Thread.Sleep(20000);
			array = QueryPlatformCapability(logger);
		}
		for (int i = 0; i < array.Length; i += 3)
		{
			Feature feature = BytesToFeatureConverter(i, array[i], array[i + 1], array[i + 2]);
			if (feature != null)
			{
				list.Add(feature);
			}
		}
		return list;
	}

	private static Feature BytesToFeatureConverter(int FeatureTypeIndex, byte FeatureSupported, byte Interfaces, byte DisplayPriority)
	{
		if (!SupportedBitsToBoolConverter(new BitArray(new byte[1] { FeatureSupported })))
		{
			return null;
		}
		FeatureType type = IndexToTypeConverter(FeatureTypeIndex);
		List<FeatureInterfaceType> interfaces = BitsToInterfacesConverter(new BitArray(new byte[1] { Interfaces }));
		int priority = DisplayPriority / 16;
		int index = DisplayPriority % 16;
		return new Feature(type, interfaces, priority, index);
	}

	private static bool SupportedBitsToBoolConverter(BitArray SupportedBits)
	{
		if (SupportedBits.Length < 8)
		{
			return false;
		}
		return SupportedBits[7];
	}

	private static FeatureType IndexToTypeConverter(int Index)
	{
		return Index switch
		{
			0 => FeatureType.ChassisLed, 
			3 => FeatureType.KeyboardLedSingleColor, 
			6 => FeatureType.KeyboardLedIndividualKeyControl, 
			9 => FeatureType.LightbarSingleColor, 
			12 => FeatureType.LightbarMultiColor, 
			15 => FeatureType.CpuPowerMode, 
			18 => FeatureType.dGpuPowerMode, 
			21 => FeatureType.FanPowerMode, 
			24 => FeatureType.SystemMonitor, 
			27 => FeatureType.DisplaySettings, 
			30 => FeatureType.KeyWinLock, 
			33 => FeatureType.KeyFnLock, 
			36 => FeatureType.KeyNumPad, 
			39 => FeatureType.KeyOSD, 
			42 => FeatureType.NucWakeSleepSettings, 
			45 => FeatureType.UserPresence, 
			_ => FeatureType.Error, 
		};
	}

	private static List<FeatureInterfaceType> BitsToInterfacesConverter(BitArray bits)
	{
		List<FeatureInterfaceType> list = new List<FeatureInterfaceType>();
		for (int i = 0; i < bits.Length; i++)
		{
			if (bits[i])
			{
				list.Add((FeatureInterfaceType)i);
			}
		}
		return list;
	}

	private static byte[] QueryPlatformCapability(NssServiceLogger logger = null)
	{
		byte b = 1;
		byte[] parameters = new byte[4] { b, 0, 0, 0 };
		byte[] array = WmiWrapper.WmiCall("PlatformCapabilityReport", parameters, logger);
		if (array != null && array.Length >= 51 && array[0] == 0)
		{
			byte[] array2 = new byte[48];
			Array.Copy(array, 3, array2, 0, 48);
			return array2;
		}
		return new byte[0];
	}
}
