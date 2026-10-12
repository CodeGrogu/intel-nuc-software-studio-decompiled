using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.WMI;

namespace PerformanceTuningLibrary;

public static class PowerLevelControl
{
	public static readonly byte NO_ERROR = 0;

	public static readonly byte FUNCTION_NOT_SUPPORTED = 225;

	public static readonly byte UNDEFINED_DEVICE = 226;

	public static readonly byte EC_NO_RESPONSE = 227;

	public static readonly byte INVALID_PARAMETER = 228;

	public static readonly byte UNEXPECTED_ERROR = 239;

	public static readonly string TURBO_BOOST_PROFILE = "PlatformConfigurationProfileReport";

	public static readonly string TURBO_BOOST_PROFILE_V1 = "PlatformConfigurationAccessV1";

	private const string MAX_PERFORMANCE_MODE_KEY = "MaxPerformanceMode";

	private const string BALANCED_MODE_KEY = "BalancedMode";

	private const string LOW_POWER_MODE_KEY = "LowPowerMode";

	private const string MAX_PERFORMANCE_BIOS_NAME = "MAX PERFORMANCE";

	private const string BALANCED_BIOS_NAME = "BALANCED";

	private const string LOW_POWER_BIOS_NAME = "LOW POWER";

	private const byte TURBO_BOOST_PROFILE_READ = 1;

	private const int STRING_START_INDEX = 4;

	private const int PROFILE_LENGTH = 35;

	private const int TOTAL_PROFILE_LENGTH = 256;

	private const int PROFILE_NAME_LENGTH = 31;

	private const int PROFILE_NUMBER_INDEX = 3;

	private const int PROFILE_OFFSET_INDEX = 4;

	private const int PL1_OFFSET_INDEX = 32;

	private const int PL2_OFFSET_INDEX = 33;

	private const int TAU_OFFSET_INDEX = 34;

	private const int CURRENT_BIOS_SETTING = 0;

	private static readonly ulong _pl1BitMask = 16383uL;

	private static readonly ulong _pl2BitMask = 70364449210368uL;

	private static readonly ulong _tauBitMask = 16646144uL;

	private static readonly Hashtable _tauTable = BuildTauTable();

	private static readonly Hashtable _tauTableInv = BuildTauTableInverse();

	private static bool _gotTurboProfile = false;

	private static readonly List<PowerProfile> _powerProfiles = new List<PowerProfile>();

	private static string _currentBiosPowerLevel;

	private static PowerProfile _currentPowerProfile;

	private static bool? _powerLevelControlSupported = null;

	public static string CurrentLevel { get; private set; } = "none";

	public static bool? PowerLevelControlSupported
	{
		get
		{
			if (!_powerLevelControlSupported.HasValue)
			{
				GetTurboBoostProfile();
			}
			return _powerLevelControlSupported;
		}
	}

	public static bool ApplyBiosPowerProfile(string PowerProfileName)
	{
		if (PowerLevelControlSupported == false)
		{
			return false;
		}
		if (!_gotTurboProfile)
		{
			GetTurboBoostProfile();
		}
		ulong num = 0uL;
		ulong num2 = 0uL;
		ulong num3 = 0uL;
		bool flag = false;
		foreach (PowerProfile item in _powerProfiles.Where((PowerProfile profile) =>
		{
			PowerProfile powerProfile = profile;
			return powerProfile.Name == PowerProfileName;
		}))
		{
			num = item.PL1;
			num2 = item.PL2;
			num3 = item.Tau;
			flag = true;
		}
		if (!flag)
		{
			return false;
		}
		CurrentLevel = PowerProfileName;
		ulong num4 = (((ReadWriteControl.ReadPowerLimit() & ~_pl1BitMask) | (num << 3)) & ~_pl2BitMask) | (num2 << 35);
		object value = ((!_tauTable.ContainsKey((int)num3)) ? ((object)14) : _tauTable[(int)num3]);
		int num5 = Convert.ToInt32(value);
		return ReadWriteControl.WritePowerLimit((num4 & ~_tauBitMask) | (ulong)((long)num5 << 17));
	}

	public static bool SetCustomPowerSetting(PowerSetting powerSetting)
	{
		if (PowerLevelControlSupported == false)
		{
			return false;
		}
		ulong num = powerSetting.PL1;
		ulong num2 = powerSetting.PL2;
		ulong num3 = (ulong)powerSetting.Tau;
		CurrentLevel = "Custom";
		ulong num4 = (((ReadWriteControl.ReadPowerLimit() & ~_pl1BitMask) | (num << 3)) & ~_pl2BitMask) | (num2 << 35);
		object value = ((!_tauTable.ContainsKey((int)num3)) ? ((object)14) : _tauTable[(int)num3]);
		int num5 = Convert.ToInt32(value);
		return ReadWriteControl.WritePowerLimit((num4 & ~_tauBitMask) | (ulong)((long)num5 << 17));
	}

	public static string GetCurrentBiosModeName()
	{
		if (PowerLevelControlSupported == false)
		{
			return "";
		}
		if (!_gotTurboProfile)
		{
			GetTurboBoostProfile();
		}
		return _currentBiosPowerLevel;
	}

	public static PowerSetting GetCurrentPowerSettings()
	{
		PowerSetting result = default;
		if (!_gotTurboProfile)
		{
			GetTurboBoostProfile();
		}
		ulong num = ReadWriteControl.ReadPowerLimit();
		result.PL1 = (uint)(num & _pl1BitMask) >> 3;
		result.PL2 = (uint)((num & _pl2BitMask) >> 32) >> 3;
		uint num2 = (uint)((num & _tauBitMask) >> 17);
		if (_tauTableInv.ContainsKey((int)num2))
		{
			result.Tau = (int)_tauTableInv[(int)num2];
		}
		else
		{
			result.Tau = 0;
		}
		return result;
	}

	public static List<PowerProfile> GetSupportedPowerProfiles()
	{
		if (PowerLevelControlSupported == false)
		{
			return new List<PowerProfile>();
		}
		if (!_gotTurboProfile)
		{
			GetTurboBoostProfile();
		}
		return _powerProfiles;
	}

	private static byte[] GetTurboBoostProfileParameter()
	{
		return new byte[4] { 1, 0, 0, 0 };
	}

	private static void GetTurboBoostProfile()
	{
		byte[] turboBoostProfileParameter = GetTurboBoostProfileParameter();
		byte[] array = WmiWrapper.WmiCall(TURBO_BOOST_PROFILE, turboBoostProfileParameter);
		if (array.Length == 0)
		{
			array = WmiWrapper.WmiCall(TURBO_BOOST_PROFILE_V1, turboBoostProfileParameter);
		}
		if (array.Length == 0)
		{
			_powerLevelControlSupported = false;
			return;
		}
		_powerLevelControlSupported = true;
		int num = array[3];
		if (array.Length <= 256 && array[0] == NO_ERROR)
		{
			for (int i = 4; i < num * 35 + 4 - 1; i += 35)
			{
				string text = Encoding.ASCII.GetString(array.Skip(i).Take(31).ToArray());
				text = text.Replace("\u0001", "");
				text = text.Replace("\u0002", "");
				text = text.Replace("\0", "");
				text = text.Replace("?", "");
				text = text.Replace("ENABLED", "").TrimEnd();
				if (text != "")
				{
					PowerProfile powerProfile = new PowerProfile
					{
						Name = text,
						PL1 = array[32 + i],
						PL2 = array[33 + i],
						Tau = array[34 + i],
						CpuMode = BiosNameToCpuMode(text)
					};
					powerProfile.TauHt = (int)_tauTable[(int)powerProfile.Tau];
					_powerProfiles.Add(powerProfile);
					if (array[i] == 128)
					{
						_currentBiosPowerLevel = text;
						_currentPowerProfile = powerProfile;
					}
				}
			}
		}
		if (_powerProfiles.TrueForAll((PowerProfile x) => x.Equals(_powerProfiles.FirstOrDefault())) && _powerProfiles.Count > 1)
		{
			_powerProfiles.RemoveRange(1, _powerProfiles.Count - 1);
		}
		_gotTurboProfile = true;
	}

	private static CpuMode BiosNameToCpuMode(string BiosModeName)
	{
		return BiosModeName.ToUpper() switch
		{
			"MAX PERFORMANCE" => CpuMode.Performance, 
			"BALANCED" => CpuMode.Balanced, 
			"LOW POWER" => CpuMode.BatterySaver, 
			_ => CpuMode.Unknown, 
		};
	}

	private static Hashtable BuildTauTable()
	{
		return new Hashtable
		{
			{ 1, 10 },
			{ 2, 11 },
			{ 3, 75 },
			{ 4, 12 },
			{ 5, 43 },
			{ 6, 76 },
			{ 7, 108 },
			{ 8, 13 },
			{ 10, 45 },
			{ 12, 77 },
			{ 14, 109 },
			{ 16, 14 },
			{ 20, 46 },
			{ 24, 78 },
			{ 28, 110 },
			{ 32, 15 },
			{ 40, 47 },
			{ 48, 79 },
			{ 56, 111 },
			{ 64, 16 },
			{ 80, 48 },
			{ 96, 80 },
			{ 112, 112 },
			{ 128, 17 },
			{ 160, 49 },
			{ 192, 81 },
			{ 224, 113 }
		};
	}

	private static Hashtable BuildTauTableInverse()
	{
		return new Hashtable
		{
			{ 10, 1 },
			{ 11, 2 },
			{ 75, 3 },
			{ 12, 4 },
			{ 43, 5 },
			{ 76, 6 },
			{ 108, 7 },
			{ 13, 8 },
			{ 45, 10 },
			{ 77, 12 },
			{ 109, 14 },
			{ 14, 16 },
			{ 46, 20 },
			{ 78, 24 },
			{ 110, 28 },
			{ 15, 32 },
			{ 47, 40 },
			{ 79, 48 },
			{ 111, 56 },
			{ 16, 64 },
			{ 48, 80 },
			{ 80, 96 },
			{ 112, 112 },
			{ 17, 128 },
			{ 49, 160 },
			{ 81, 192 },
			{ 113, 224 }
		};
	}
}
