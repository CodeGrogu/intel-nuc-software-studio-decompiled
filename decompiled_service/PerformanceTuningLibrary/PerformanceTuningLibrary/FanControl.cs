using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Infrastructure.WMI;

namespace PerformanceTuningLibrary;

public static class FanControl
{
	public struct AllFanSettings
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
		public FanSetting[] fanSettings;
	}

	public struct AllFanSettings23
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
		public FanSetting23[] fanSettings;
	}

	private enum SubFunctionNumber : byte
	{
		QueryFanControlCapability = 1,
		FanProfileReport
	}

	private const byte FAN_PROFILE_QUERY = 2;

	private const byte PLATFORM_CAPABILITY_QUERY = 1;

	private const string FIXED_FAN = "Fixed";

	private const string CUSTOM_FAN = "Custom";

	public static readonly byte NO_ERROR = 0;

	public static readonly byte FUNCTION_NOT_SUPPORTED = 225;

	public static readonly byte UNDEFINED_DEVICE = 226;

	public static readonly byte EC_NO_RESPONSE = 227;

	public static readonly byte INVALID_PARAMETER = 228;

	public static readonly byte UNEXPECTED_ERROR = 239;

	public static readonly string FAN_PROFILE_REPORT = "PlatformConfigurationProfileReport";

	public static readonly string FAN_PROFILE_REPORT_V1 = "PlatformConfigurationAccessV1";

	public static readonly string PLATFORM_CAPABILITY_REPORT = "PlatformCapabilityReport";

	private const byte MAX_FAN_DUTY = 100;

	private const uint RETURN_BYTE_NUM_SUPPORT = 6u;

	private const uint FAN_MAX_NUMBER = 6u;

	private static int FAN_START_BYTE = 3;

	private static int RN_FAN_START_BYTE = 5;

	private const int DATASET_COUNT = 14;

	private const int DATASET_COUNT_RN = 15;

	private static byte _fanModeSupport = 0;

	private static ECVersion _currentECVersion = ECVersion.Reserve;

	private static bool? _fanEcSupported = null;

	private const int SUPPORTED_FAN_MODE_COUNT_RN = 3;

	private static bool? _fanProfilesSupported = null;

	private static List<string> _supportedFanModes = new List<string>();

	private static List<FanProfile> _fanProfileList = new List<FanProfile>();

	private static List<FanProfile23> _fanProfileList23 = new List<FanProfile23>();

	private static List<FanProfileRN> _fanProfileListRN = new List<FanProfileRN>();

	public static List<string> SupportedFanModes
	{
		get
		{
			if (!FanProfilesSupported.HasValue && _supportedFanModes.Count == 0)
			{
				_supportedFanModes = GetSupportedFanModes();
				_fanProfilesSupported = _supportedFanModes.Count != 0;
			}
			return _supportedFanModes;
		}
	}

	public static ECVersion CurrentEcVersion
	{
		get
		{
			if (!FanEcSupported.HasValue && _currentECVersion == ECVersion.Reserve)
			{
				_currentECVersion = GetCurrentECVersion();
				_fanEcSupported = _currentECVersion != ECVersion.Reserve;
			}
			return _currentECVersion;
		}
	}

	public static bool? FanEcSupported
	{
		get
		{
			if (!_fanEcSupported.HasValue)
			{
				GetSupportedCurrentECVersion();
			}
			return _fanEcSupported;
		}
	}

	public static bool? FanProfilesSupported
	{
		get
		{
			if (!_fanProfilesSupported.HasValue)
			{
				GetSupportedFanFeatures();
			}
			return _fanProfilesSupported;
		}
	}

	public static uint CurrentFanDuty { get; private set; } = 0u;

	public static NucFanMode CurrentMode { get; private set; } = NucFanMode.Reserve;

	public static uint FanCount { get; private set; }

	public static string GetCurrentBiosFanModeName()
	{
		if (FanProfilesSupported == false)
		{
			return null;
		}
		byte[] fanCapabilityParameter = GetFanCapabilityParameter();
		byte[] array = WmiWrapper.WmiCall(FAN_PROFILE_REPORT, fanCapabilityParameter);
		if (array.Length == 0)
		{
			array = WmiWrapper.WmiCall(FAN_PROFILE_REPORT_V1, fanCapabilityParameter);
		}
		if (array.Length >= 6 && array[0] == NO_ERROR)
		{
			CurrentMode = (NucFanMode)array[5];
			if ((CurrentMode == NucFanMode.Fixed && array[1] == 2) || (CurrentMode == NucFanMode.Custom && array[1] == 2))
			{
				FanProfile23 newModeProfile = CreateFixedandCustomProfile();
				if (!_fanProfileList23.Any((FanProfile23 x) => x.name == newModeProfile.name))
				{
					_fanProfileList23.Add(newModeProfile);
				}
			}
			return CurrentMode.ToString();
		}
		_fanProfilesSupported = false;
		return null;
	}

	private static FanProfile23 CreateFixedandCustomProfile()
	{
		AllFanSettings23 currentFanSettings = GetCurrentFanSettings23();
		byte[] sourceArray = ECReadProfile();
		FanProfile23 result = new FanProfile23
		{
			name = CurrentMode.ToString(),
			profile = new byte[90],
			fanSettings = currentFanSettings.fanSettings
		};
		result.profile[0] = 0;
		result.profile[1] = 2;
		result.profile[2] = 0;
		for (int i = 0; i < 90; i++)
		{
			switch (i)
			{
			case 0:
				result.profile[i] = 0;
				break;
			case 1:
				result.profile[i] = 2;
				break;
			case 2:
				result.profile[i] = 0;
				break;
			default:
				Array.Copy(sourceArray, 0, result.profile, 3, 14);
				break;
			}
		}
		return result;
	}

	public static AllFanSettings GetCurrentFanSettings()
	{
		byte[] array = ECReadProfile();
		switch (CurrentEcVersion)
		{
		case ECVersion.ECNUC21:
			if (array[12] == 1)
			{
				CurrentFanDuty = array[13];
				CurrentMode = NucFanMode.Fixed;
			}
			break;
		case ECVersion.ECNUC22:
			if ((array[12] & 0x80) == 128)
			{
				CurrentFanDuty = (uint)(array[12] & 0x7F);
				CurrentMode = NucFanMode.Fixed;
			}
			break;
		}
		return FillFanSetting(array);
	}

	public static AllFanSettings23 GetCurrentFanSettings23()
	{
		byte[] array = ECReadProfile();
		switch (CurrentEcVersion)
		{
		case ECVersion.ECNUC21:
			if (array[12] == 1)
			{
				CurrentFanDuty = array[13];
				CurrentMode = NucFanMode.Fixed;
			}
			break;
		case ECVersion.ECNUC22:
			if ((array[12] & 0x80) == 128)
			{
				CurrentFanDuty = (uint)(array[12] & 0x7F);
				CurrentMode = NucFanMode.Fixed;
			}
			break;
		}
		return FillFanSetting23(array);
	}

	public static bool SetFanMode(string fanModeName)
	{
		NucFanMode nucFanMode = NucFanMode.Reserve;
		foreach (NucFanMode value in Enum.GetValues(typeof(NucFanMode)))
		{
			if (value.ToString() == fanModeName)
			{
				nucFanMode = value;
			}
		}
		if (nucFanMode == NucFanMode.Reserve)
		{
			return false;
		}
		if (SupportedFanModes.Count == 0)
		{
			return false;
		}
		bool flag = false;
		if ((_fanModeSupport & (1 << (int)nucFanMode)) != 0)
		{
			if (CurrentEcVersion == ECVersion.ECNUC21)
			{
				IEnumerable<FanProfile> enumerable = _fanProfileList.Where((FanProfile profile) => profile.name == fanModeName);
				byte[] array = new byte[90];
				foreach (FanProfile item in enumerable)
				{
					array = item.profile;
					flag = true;
				}
				if (!flag)
				{
					return false;
				}
				if (array.Length >= 90 && array[0] == NO_ERROR)
				{
					FanProfileWmi fanProfile = new FanProfileWmi
					{
						Data = array
					};
					CurrentMode = nucFanMode;
					return ReadWriteControl.WriteFanProfile21(fanProfile);
				}
			}
			else if (CurrentEcVersion == ECVersion.ECNUC22)
			{
				IEnumerable<FanProfile> enumerable2 = _fanProfileList.Where((FanProfile profile) => profile.name == fanModeName);
				byte[] array2 = new byte[90];
				foreach (FanProfile item2 in enumerable2)
				{
					array2 = item2.profile;
					flag = true;
				}
				if (!flag)
				{
					return false;
				}
				if (array2.Length >= 90 && array2[0] == NO_ERROR)
				{
					FanProfileWmi fanProfile2 = new FanProfileWmi
					{
						Data = array2
					};
					CurrentMode = nucFanMode;
					return ReadWriteControl.WriteFanProfile22(fanProfile2);
				}
			}
			else if (CurrentEcVersion == ECVersion.ECNUC23)
			{
				if (fanModeName == "Custom")
				{
					FanProfile23 newModeProfile = CreateFixedandCustomProfile();
					newModeProfile.name = fanModeName;
					if (!_fanProfileList23.Any((FanProfile23 x) => x.name == newModeProfile.name))
					{
						_fanProfileList23.Add(newModeProfile);
					}
					else
					{
						_fanProfileList23[_fanProfileList23.FindIndex((FanProfile23 x) => string.Equals(x.name, fanModeName, StringComparison.OrdinalIgnoreCase))] = newModeProfile;
					}
				}
				IEnumerable<FanProfile23> enumerable3 = _fanProfileList23.Where((FanProfile23 profile) => profile.name == fanModeName);
				byte[] array3 = new byte[90];
				NucFanModeSN nucFanModeSN = NucFanModeSN.Cool;
				foreach (NucFanModeSN value2 in Enum.GetValues(typeof(NucFanModeSN)))
				{
					if (value2.ToString() == fanModeName)
					{
						nucFanModeSN = value2;
						break;
					}
				}
				foreach (FanProfile23 item3 in enumerable3)
				{
					array3 = item3.profile;
					flag = true;
				}
				if (!flag)
				{
					return false;
				}
				if (array3.Length >= 90 && array3[0] == NO_ERROR)
				{
					byte[] array4 = new byte[21];
					for (int num = 0; num < 21; num++)
					{
						array4[num] = array3[num];
						if (num == 17)
						{
							array4[num] = 1;
						}
						if (num == 18)
						{
							array4[num] = (byte)nucFanModeSN;
						}
					}
					FanProfileWmi23 fanProfile3 = new FanProfileWmi23
					{
						Data = array4
					};
					CurrentMode = nucFanMode;
					return ReadWriteControl.WriteFanProfile23(fanProfile3);
				}
			}
		}
		return false;
	}

	public static bool SetFanOffConfiguration(string fanModeName, bool isFanOff)
	{
		NucFanMode nucFanMode = NucFanMode.Reserve;
		foreach (NucFanMode value3 in Enum.GetValues(typeof(NucFanMode)))
		{
			if (value3.ToString() == fanModeName)
			{
				nucFanMode = value3;
			}
		}
		if (nucFanMode == NucFanMode.Reserve || nucFanMode == NucFanMode.Fixed)
		{
			return false;
		}
		if (SupportedFanModes.Count == 0)
		{
			return false;
		}
		bool flag = false;
		if ((_fanModeSupport & (1 << (int)nucFanMode)) != 0)
		{
			if (CurrentEcVersion == ECVersion.ECNUC21)
			{
				IEnumerable<FanProfile> enumerable = _fanProfileList.Where((FanProfile profile) => profile.name == fanModeName);
				byte[] array = new byte[90];
				foreach (FanProfile item in enumerable)
				{
					array = item.profile;
					flag = true;
				}
				if (!flag)
				{
					return false;
				}
				if (array.Length >= 90 && array[0] == NO_ERROR)
				{
					FanProfileWmi fanProfile = new FanProfileWmi
					{
						Data = array
					};
					CurrentMode = nucFanMode;
					return ReadWriteControl.WriteFanProfile21(fanProfile);
				}
			}
			else if (CurrentEcVersion == ECVersion.ECNUC22)
			{
				IEnumerable<FanProfile> enumerable2 = _fanProfileList.Where((FanProfile profile) => profile.name == fanModeName);
				byte[] array2 = new byte[90];
				foreach (FanProfile item2 in enumerable2)
				{
					array2 = item2.profile;
					flag = true;
				}
				if (!flag)
				{
					return false;
				}
				byte b = array2[16];
				BitArray bitArray = new BitArray(new byte[1] { b });
				bitArray[7] = isFanOff;
				byte[] array3 = new byte[1];
				bitArray.CopyTo(array3, 0);
				array2[16] = array3[0];
				if (array2.Length >= 90 && array2[0] == NO_ERROR)
				{
					FanProfileWmi fanProfile2 = new FanProfileWmi
					{
						Data = array2
					};
					CurrentMode = nucFanMode;
					bool result = ReadWriteControl.WriteFanProfile22(fanProfile2);
					AllFanSettings currentFanSettings = GetCurrentFanSettings();
					FanProfile value = _fanProfileList.First((FanProfile x) => string.Equals(x.name, fanModeName, StringComparison.OrdinalIgnoreCase));
					value.fanSettings = currentFanSettings.fanSettings;
					_fanProfileList[_fanProfileList.FindIndex((FanProfile x) => string.Equals(x.name, fanModeName, StringComparison.OrdinalIgnoreCase))] = value;
					return result;
				}
			}
			else if (CurrentEcVersion == ECVersion.ECNUC23)
			{
				IEnumerable<FanProfile23> enumerable3 = _fanProfileList23.Where((FanProfile23 profile) => profile.name == fanModeName);
				byte[] array4 = new byte[90];
				foreach (FanProfile23 item3 in enumerable3)
				{
					array4 = item3.profile;
					flag = true;
				}
				if (!flag)
				{
					return false;
				}
				byte b2 = array4[10];
				BitArray bitArray2 = new BitArray(new byte[1] { b2 });
				bitArray2[7] = isFanOff;
				byte[] array5 = new byte[1];
				bitArray2.CopyTo(array5, 0);
				array4[10] = array5[0];
				NucFanModeSN nucFanModeSN = NucFanModeSN.Cool;
				foreach (NucFanModeSN value4 in Enum.GetValues(typeof(NucFanModeSN)))
				{
					if (value4.ToString() == fanModeName)
					{
						nucFanModeSN = value4;
					}
				}
				if (array4.Length >= 90 && array4[0] == NO_ERROR)
				{
					byte[] array6 = new byte[21];
					for (int num = 0; num < 21; num++)
					{
						array6[num] = array4[num];
						if (num == 17)
						{
							array6[num] = 1;
						}
						if (num == 18)
						{
							array6[num] = (byte)nucFanModeSN;
						}
					}
					FanProfileWmi23 fanProfile3 = new FanProfileWmi23
					{
						Data = array6
					};
					CurrentMode = nucFanMode;
					bool result2 = ReadWriteControl.WriteFanProfile23(fanProfile3);
					AllFanSettings23 currentFanSettings2 = GetCurrentFanSettings23();
					FanProfile23 value2 = _fanProfileList23.First((FanProfile23 x) => string.Equals(x.name, fanModeName, StringComparison.OrdinalIgnoreCase));
					value2.fanSettings = currentFanSettings2.fanSettings;
					_fanProfileList23[_fanProfileList23.FindIndex((FanProfile23 x) => string.Equals(x.name, fanModeName, StringComparison.OrdinalIgnoreCase))] = value2;
					return result2;
				}
			}
		}
		return false;
	}

	public static FanProfile GetSelectedFanProfile(string profileName)
	{
		return _fanProfileList.FirstOrDefault((FanProfile profile) => profile.name == profileName);
	}

	public static FanProfile23 GetSelectedFanProfile23(string fanProfileName)
	{
		if (fanProfileName != null)
		{
			CurrentMode = (NucFanMode)Enum.Parse(typeof(NucFanMode), fanProfileName);
			if (fanProfileName == "Fixed" || fanProfileName == "Custom")
			{
				FanProfile23 newModeProfile = CreateFixedandCustomProfile();
				if (!_fanProfileList23.Any((FanProfile23 x) => x.name == newModeProfile.name))
				{
					_fanProfileList23.Add(newModeProfile);
				}
			}
		}
		return _fanProfileList23.FirstOrDefault((FanProfile23 profile) => profile.name == fanProfileName);
	}

	public static List<FanProfileRN> GetSelectedFanProfileRN()
	{
		return _fanProfileListRN;
	}

	public static bool SetFanCustom(AllFanSettings FanSettings)
	{
		byte[] array = new byte[90];
		byte[] array2 = ECReadProfile();
		array[0] = 0;
		array[1] = 1;
		array[2] = 27;
		int num = FAN_START_BYTE;
		for (int i = 0; i < 90; i++)
		{
			array[num] = array2[i];
			if (num % 14 == 2)
			{
				i += 2;
			}
			num++;
		}
		for (int j = 0; j < FanCount; j++)
		{
			array[FAN_START_BYTE + j * 14] = (byte)FanSettings.fanSettings[j].primaryInputSourceConfiguration;
			array[FAN_START_BYTE + j * 14 + 1] = FanSettings.fanSettings[j].PrimaryMaximumTemperature;
			array[FAN_START_BYTE + j * 14 + 2] = FanSettings.fanSettings[j].PrimaryMinimumTemperature;
			array[FAN_START_BYTE + j * 14 + 3] = FanSettings.fanSettings[j].PrimaryMinimumDuty;
			array[FAN_START_BYTE + j * 14 + 4] = FanSettings.fanSettings[j].PrimaryDutyIncrement;
			array[FAN_START_BYTE + j * 14 + 5] = FanSettings.fanSettings[j].PrimaryFanOffTemperature;
			array[FAN_START_BYTE + j * 14 + 6] = (byte)FanSettings.fanSettings[j].secondaryInputSourceConfiguration;
			array[FAN_START_BYTE + j * 14 + 7] = FanSettings.fanSettings[j].SecondaryMaximumTemperature;
			array[FAN_START_BYTE + j * 14 + 8] = FanSettings.fanSettings[j].SecondaryMinimumTemperature;
			array[FAN_START_BYTE + j * 14 + 9] = FanSettings.fanSettings[j].SecondaryMinimumDuty;
			array[FAN_START_BYTE + j * 14 + 10] = FanSettings.fanSettings[j].SecondaryDutyIncrement;
			array[FAN_START_BYTE + j * 14 + 11] = FanSettings.fanSettings[j].SecondaryFanOffTemperature;
		}
		FanProfileWmi fanProfile = new FanProfileWmi
		{
			Data = array
		};
		return CurrentEcVersion switch
		{
			ECVersion.ECNUC21 => ReadWriteControl.WriteFanProfile21(fanProfile), 
			ECVersion.ECNUC22 => ReadWriteControl.WriteFanProfile22(fanProfile), 
			_ => false, 
		};
	}

	public static bool SetFixFanSpeed(uint fanNumber, uint duty)
	{
		if (duty >= 100)
		{
			return false;
		}
		if (fanNumber >= FanCount)
		{
			return false;
		}
		return CurrentEcVersion switch
		{
			ECVersion.ECNUC21 => ReadWriteControl.ControlFanDuty((byte)fanNumber, (byte)duty, NucEcSection.UseEcSection21), 
			ECVersion.ECNUC22 => ReadWriteControl.ControlFanDuty((byte)fanNumber, (byte)duty, NucEcSection.UseEcSection22), 
			_ => false, 
		};
	}

	private static List<string> GetSupportedFanModes()
	{
		byte[] fanCapabilityParameter = GetFanCapabilityParameter();
		byte[] array = WmiWrapper.WmiCall(FAN_PROFILE_REPORT_V1, fanCapabilityParameter);
		if (array.Length == 0)
		{
			array = WmiWrapper.WmiCall(FAN_PROFILE_REPORT, fanCapabilityParameter);
		}
		if (array.Length >= 6 && array[0] == NO_ERROR)
		{
			FanCount = array[3];
			_fanModeSupport = array[4];
			for (int i = 0; i < 4; i++)
			{
				if ((_fanModeSupport & (1 << i)) != 0)
				{
					List<string> supportedFanModes = _supportedFanModes;
					NucFanMode nucFanMode = (NucFanMode)i;
					if (!supportedFanModes.Contains(nucFanMode.ToString()))
					{
						List<string> supportedFanModes2 = _supportedFanModes;
						nucFanMode = (NucFanMode)i;
						supportedFanModes2.Add(nucFanMode.ToString());
					}
				}
			}
			byte b = array[1];
			byte b2 = array[2];
			_fanProfilesSupported = _supportedFanModes != null && _supportedFanModes.Count != 0;
			_fanProfileList = new List<FanProfile>();
			_fanProfileList23 = new List<FanProfile23>();
			if (b == 2 && b2 == 1)
			{
				for (int j = 0; j < FanCount; j++)
				{
					CreateFanProfileRN(array, j, RN_FAN_START_BYTE);
					RN_FAN_START_BYTE += 15;
				}
			}
			else
			{
				for (int k = 0; k < 4; k++)
				{
					if ((_fanModeSupport & (1 << k)) == 0)
					{
						continue;
					}
					byte[] modeProfile = GetModeProfile((NucFanMode)k);
					FanSetting[] fanSettings = new FanSetting[6];
					FanSetting23[] fanSettings2 = new FanSetting23[6];
					if (modeProfile.Length >= 90 && modeProfile[0] == NO_ERROR)
					{
						if (_currentECVersion == ECVersion.ECNUC21 || _currentECVersion == ECVersion.ECNUC22)
						{
							CreateFanProfile(k, fanSettings, modeProfile);
						}
						if (_currentECVersion == ECVersion.ECNUC23)
						{
							CreateFanProfile23(k, fanSettings2, modeProfile);
						}
					}
				}
			}
			return _supportedFanModes;
		}
		return new List<string>();
	}

	public static bool SetFanModeRN(string fanModeName, string fanType, List<FanProfileRN> fanProfiles)
	{
		List<FanProfileRN> list = ChangeSequence(fanProfiles);
		byte[] array = new byte[90];
		int num = 0;
		foreach (FanProfileRN fanProfile in list)
		{
			FAN_START_BYTE = num * 14 + 3;
			Array.Copy(fanProfile.fanSettings.FirstOrDefault((FanSettingRN x) => x.FanModeName == fanProfile.CurrentBiosFanMode).FanModeBytes, 0, array, FAN_START_BYTE, 14);
			num++;
		}
		return ReadWriteControl.WriteFanProfile22(new FanProfileWmi
		{
			Data = array
		});
	}

	private static List<FanProfileRN> ChangeSequence(List<FanProfileRN> fanProfiles)
	{
		int index = fanProfiles.FindIndex((FanProfileRN x) => x.fanType.Equals(FanTypeRN.CPUFan2.ToString()));
		FanProfileRN item = fanProfiles.FirstOrDefault((FanProfileRN x) => x.fanType.Equals(FanTypeRN.CPUFan2.ToString()));
		fanProfiles.RemoveAt(index);
		fanProfiles.Insert(3, item);
		return fanProfiles;
	}

	private static void CreateFanProfileRN(byte[] outResult, int count, int currentModeByte)
	{
		FanSettingRN[] fanSettings = new FanSettingRN[5];
		FanProfileRN fanProfileRN = default;
		FanTypeRN fanTypeRN = (FanTypeRN)count;
		fanProfileRN.fanType = fanTypeRN.ToString();
		NucFanMode nucFanMode = (NucFanMode)outResult[currentModeByte];
		fanProfileRN.CurrentBiosFanMode = nucFanMode.ToString();
		fanProfileRN.profile = new byte[90];
		fanProfileRN.fanSettings = fanSettings;
		FanProfileRN item = fanProfileRN;
		FAN_START_BYTE = count * 14 + 3;
		for (int i = 0; i < 3; i++)
		{
			if ((_fanModeSupport & (1 << i)) != 0)
			{
				NucFanMode fanMode = (NucFanMode)i;
				byte[] modeProfile = GetModeProfile(fanMode);
				item.fanSettings[i].FanModeName = fanMode.ToString();
				item.fanSettings[i].PrimaryTemperatureSourceAndTemperatureHystersis = modeProfile[FAN_START_BYTE];
				item.fanSettings[i].RampAccelerationTemperature = modeProfile[FAN_START_BYTE + 1];
				item.fanSettings[i].PrimaryMinimumTemperature = modeProfile[FAN_START_BYTE + 2];
				item.fanSettings[i].PrimaryMinimumDutyCycleAndRampAcceleration = modeProfile[FAN_START_BYTE + 3];
				item.fanSettings[i].PrimaryDutyCycleIncrementAndSamplingPeriod = modeProfile[FAN_START_BYTE + 4];
				item.fanSettings[i].PrimaryFanCutOffTemperatureOffsetAndRampDownRate = modeProfile[FAN_START_BYTE + 5];
				item.fanSettings[i].SecondaryTemperatureSourceAndTemperatureHystersis = modeProfile[FAN_START_BYTE + 6];
				item.fanSettings[i].SecondaryRampAccelerationTemperature = modeProfile[FAN_START_BYTE + 7];
				item.fanSettings[i].SecondaryMinimumTemperature = modeProfile[FAN_START_BYTE + 8];
				item.fanSettings[i].SecondaryMinimumDutyCycleAndRampAcceleration = modeProfile[FAN_START_BYTE + 9];
				item.fanSettings[i].SecondaryDutyCycleIncrementAndSamplingPeriod = modeProfile[FAN_START_BYTE + 10];
				item.fanSettings[i].SecondaryFanCutOffTemperatureOffsetAndRampDownRate = modeProfile[FAN_START_BYTE + 11];
				item.fanSettings[i].FanControlModeAndSpeedRampRateLimit = modeProfile[FAN_START_BYTE + 12];
				item.fanSettings[i].MaximumDutyCycle = modeProfile[FAN_START_BYTE + 13];
				item.fanSettings[i].FanModeBytes = new byte[14];
				for (int j = 0; j < 14; j++)
				{
					item.fanSettings[i].FanModeBytes[j] = modeProfile[FAN_START_BYTE + j];
				}
			}
		}
		if (item.CurrentBiosFanMode == "Custom" || item.CurrentBiosFanMode == "Fixed")
		{
			int num = 0;
			if (item.CurrentBiosFanMode == "Custom")
			{
				num = 3;
			}
			else if (item.CurrentBiosFanMode == "Fixed")
			{
				num = 4;
			}
			byte[] array = ECReadProfile();
			item.fanSettings[num].FanModeBytes = new byte[14];
			int num2 = GetFanDataMultiplier(count) * 16;
			for (int k = 0; k < 14; k++)
			{
				item.fanSettings[num].FanModeBytes[k] = array[num2 + k];
				if (FAN_START_BYTE % 14 == 2)
				{
					k += 2;
				}
			}
			item.fanSettings[num].FanModeName = item.CurrentBiosFanMode;
			item.fanSettings[num].PrimaryTemperatureSourceAndTemperatureHystersis = array[num2];
			item.fanSettings[num].RampAccelerationTemperature = array[num2 + 1];
			item.fanSettings[num].PrimaryMinimumTemperature = array[num2 + 2];
			item.fanSettings[num].PrimaryMinimumDutyCycleAndRampAcceleration = array[num2 + 3];
			item.fanSettings[num].PrimaryDutyCycleIncrementAndSamplingPeriod = array[num2 + 4];
			item.fanSettings[num].PrimaryFanCutOffTemperatureOffsetAndRampDownRate = array[num2 + 5];
			item.fanSettings[num].SecondaryTemperatureSourceAndTemperatureHystersis = array[num2 + 6];
			item.fanSettings[num].SecondaryRampAccelerationTemperature = array[num2 + 7];
			item.fanSettings[num].SecondaryMinimumTemperature = array[num2 + 8];
			item.fanSettings[num].SecondaryMinimumDutyCycleAndRampAcceleration = array[num2 + 9];
			item.fanSettings[num].SecondaryDutyCycleIncrementAndSamplingPeriod = array[num2 + 10];
			item.fanSettings[num].SecondaryFanCutOffTemperatureOffsetAndRampDownRate = array[num2 + 11];
			item.fanSettings[num].FanControlModeAndSpeedRampRateLimit = array[num2 + 12];
			item.fanSettings[num].MaximumDutyCycle = array[num2 + 13];
		}
		_fanProfileListRN.Add(item);
	}

	private static int GetFanDataMultiplier(int count)
	{
		return count switch
		{
			0 => 0, 
			1 => 3, 
			2 => 1, 
			3 => 2, 
			_ => 0, 
		};
	}

	private static void CreateFanProfile23(int i, FanSetting23[] fanSettings, byte[] result)
	{
		FanProfile23 fanProfile = default;
		NucFanMode nucFanMode = (NucFanMode)i;
		fanProfile.name = nucFanMode.ToString();
		fanProfile.profile = new byte[90];
		fanProfile.fanSettings = fanSettings;
		FanProfile23 newModeProfile = fanProfile;
		for (int j = 0; j < 90; j++)
		{
			newModeProfile.profile[j] = result[j];
		}
		for (int k = 0; k < FanCount; k++)
		{
			newModeProfile.fanSettings[k].PrimaryTemperatureSourceAndTemperatureHystersis = result[FAN_START_BYTE + k * 14];
			newModeProfile.fanSettings[k].RampAccelerationTemperature = result[FAN_START_BYTE + k * 14 + 1];
			newModeProfile.fanSettings[k].PrimaryMinimumTemperature = result[FAN_START_BYTE + k * 14 + 2];
			newModeProfile.fanSettings[k].PrimaryMinimumDutyCycleAndRampAcceleration = result[FAN_START_BYTE + k * 14 + 3];
			newModeProfile.fanSettings[k].PrimaryDutyCycleIncrementAndSamplingPeriod = result[FAN_START_BYTE + k * 14 + 4];
			newModeProfile.fanSettings[k].PrimaryFanCutOffTemperatureOffsetAndRampDownRate = result[FAN_START_BYTE + k * 14 + 5];
			newModeProfile.fanSettings[k].SecondaryTemperatureSourceAndTemperatureHystersis = result[FAN_START_BYTE + k * 14 + 6];
			newModeProfile.fanSettings[k].SecondaryRampAccelerationTemperature = result[FAN_START_BYTE + k * 14 + 7];
			newModeProfile.fanSettings[k].SecondaryMinimumTemperature = result[FAN_START_BYTE + k * 14 + 8];
			newModeProfile.fanSettings[k].SecondaryMinimumDutyCycleAndRampAcceleration = result[FAN_START_BYTE + k * 14 + 9];
			newModeProfile.fanSettings[k].SecondaryDutyCycleIncrementAndSamplingPeriod = result[FAN_START_BYTE + k * 14 + 10];
			newModeProfile.fanSettings[k].SecondaryFanCutOffTemperatureOffsetAndRampDownRate = result[FAN_START_BYTE + k * 14 + 11];
			newModeProfile.fanSettings[k].FanControlModeAndSpeedRampRateLimit = result[FAN_START_BYTE + k * 14 + 12];
			newModeProfile.fanSettings[k].MaximumDutyCycle = result[FAN_START_BYTE + k * 14 + 13];
		}
		if (!_fanProfileList23.Any((FanProfile23 x) => x.name == newModeProfile.name))
		{
			_fanProfileList23.Add(newModeProfile);
			GetCurrentBiosFanModeName();
		}
	}

	private static void CreateFanProfile(int i, FanSetting[] fanSettings, byte[] result)
	{
		FanProfile fanProfile = default;
		NucFanMode nucFanMode = (NucFanMode)i;
		fanProfile.name = nucFanMode.ToString();
		fanProfile.profile = new byte[90];
		fanProfile.fanSettings = fanSettings;
		FanProfile item = fanProfile;
		for (int j = 0; j < 90; j++)
		{
			item.profile[j] = result[j];
		}
		for (int k = 0; k < FanCount; k++)
		{
			item.fanSettings[k].primaryInputSourceConfiguration = (PrimaryInputSourceConfiguration)result[FAN_START_BYTE + k * 14];
			item.fanSettings[k].PrimaryMaximumTemperature = result[FAN_START_BYTE + k * 14 + 1];
			item.fanSettings[k].PrimaryMinimumTemperature = result[FAN_START_BYTE + k * 14 + 2];
			item.fanSettings[k].PrimaryMinimumDuty = result[FAN_START_BYTE + k * 14 + 3];
			item.fanSettings[k].PrimaryDutyIncrement = result[FAN_START_BYTE + k * 14 + 4];
			item.fanSettings[k].PrimaryFanOffTemperature = result[FAN_START_BYTE + k * 14 + 5];
			item.fanSettings[k].secondaryInputSourceConfiguration = (SecondaryInputSourceConfiguration)result[FAN_START_BYTE + k * 14 + 6];
			item.fanSettings[k].SecondaryMaximumTemperature = result[FAN_START_BYTE + k * 14 + 7];
			item.fanSettings[k].SecondaryMinimumTemperature = result[FAN_START_BYTE + k * 14 + 8];
			item.fanSettings[k].SecondaryMinimumDuty = result[FAN_START_BYTE + k * 14 + 9];
			item.fanSettings[k].SecondaryDutyIncrement = result[FAN_START_BYTE + k * 14 + 10];
			item.fanSettings[k].SecondaryFanOffTemperature = result[FAN_START_BYTE + k * 14 + 11];
			if (_currentECVersion == ECVersion.ECNUC21)
			{
				item.fanSettings[k].Configuration1 = (byte)((result[FAN_START_BYTE + k * 14 + 12] << 7) | result[FAN_START_BYTE + k * 14 + 13]);
				item.fanSettings[k].Configuration2 = 0;
			}
			else
			{
				item.fanSettings[k].Configuration1 = result[FAN_START_BYTE + k * 14 + 12];
				item.fanSettings[k].Configuration2 = result[FAN_START_BYTE + k * 14 + 13];
			}
		}
		if (!_fanProfileList.Contains(item))
		{
			_fanProfileList.Add(item);
		}
	}

	private static void GetSupportedFanFeatures()
	{
		if (!_fanProfilesSupported.HasValue)
		{
			GetSupportedFanModes();
		}
	}

	private static void GetSupportedCurrentECVersion()
	{
		if (!_fanEcSupported.HasValue)
		{
			_currentECVersion = GetCurrentECVersion();
			_fanEcSupported = _currentECVersion != ECVersion.Reserve;
		}
	}

	public static List<FanSpeed> GetFanSpeed()
	{
		return CurrentEcVersion switch
		{
			ECVersion.ECNUC21 => ReadWriteControl.ReadFanSpeed(NucEcSection.UseEcSection21), 
			ECVersion.ECNUC22 => ReadWriteControl.ReadFanSpeed(NucEcSection.UseEcSection22), 
			ECVersion.ECNUC23 => ReadWriteControl.ReadFanSpeed(NucEcSection.UseEcSection22), 
			ECVersion.ECBC => ReadWriteControl.ReadFanSpeed(NucEcSection.UseEcSectionBC), 
			_ => new List<FanSpeed>(), 
		};
	}

	public static int GetCPUTemprature()
	{
		return ECReadTempratureProfile()[1];
	}

	public static int GetIntegratedGPUTemprature()
	{
		return ECReadTempratureProfile()[2];
	}

	public static int GetDiscreteGPUTemprature()
	{
		return ECReadTempratureProfile()[0];
	}

	public static bool SetFanDutyCycleFixedMode(string selectedFanMode, byte[] FanBytes)
	{
		FanProfile23 fanProfile = default;
		if (_fanProfileList23.Any((FanProfile23 x) => x.name == selectedFanMode))
		{
			fanProfile = _fanProfileList23.First((FanProfile23 x) => string.Equals(x.name, selectedFanMode, StringComparison.OrdinalIgnoreCase));
		}
		else
		{
			fanProfile.name = selectedFanMode;
			_fanProfileList23.Add(fanProfile);
		}
		byte[] array = FanBytes;
		FanBytes = FanBytes.Skip(3).ToArray();
		AllFanSettings23 allFanSettings = FillFanSetting23(FanBytes);
		FanBytes = array;
		fanProfile.profile = FanBytes;
		fanProfile.fanSettings = allFanSettings.fanSettings;
		_fanProfileList23[_fanProfileList23.FindIndex((FanProfile23 x) => string.Equals(x.name, selectedFanMode, StringComparison.OrdinalIgnoreCase))] = fanProfile;
		return SetFanMode(selectedFanMode);
	}

	private static AllFanSettings FillFanSetting(byte[] EcData)
	{
		FanSetting[] fanSettings = new FanSetting[FanCount];
		AllFanSettings result = new AllFanSettings
		{
			fanSettings = fanSettings
		};
		byte[] array;
		if (CurrentEcVersion != ECVersion.ECNUC21)
		{
			array = ((CurrentEcVersion != ECVersion.ECNUC22) ? new byte[FanCount * 14] : EcData);
		}
		else
		{
			byte[] first = EcData.Take(14).ToArray();
			byte[] second = EcData.Skip(17).ToArray();
			array = first.Concat(second).ToArray();
		}
		for (int i = 0; i < FanCount; i++)
		{
			result.fanSettings[i].primaryInputSourceConfiguration = (PrimaryInputSourceConfiguration)array[14 * i];
			result.fanSettings[i].PrimaryMaximumTemperature = array[14 * i + 1];
			result.fanSettings[i].PrimaryMinimumTemperature = array[14 * i + 2];
			result.fanSettings[i].PrimaryMinimumDuty = array[14 * i + 3];
			result.fanSettings[i].PrimaryDutyIncrement = array[14 * i + 4];
			result.fanSettings[i].PrimaryFanOffTemperature = array[14 * i + 5];
			result.fanSettings[i].secondaryInputSourceConfiguration = (SecondaryInputSourceConfiguration)array[14 * i + 6];
			result.fanSettings[i].SecondaryMaximumTemperature = array[14 * i + 7];
			result.fanSettings[i].SecondaryMinimumTemperature = array[14 * i + 8];
			result.fanSettings[i].SecondaryMinimumDuty = array[14 * i + 9];
			result.fanSettings[i].SecondaryDutyIncrement = array[14 * i + 10];
			result.fanSettings[i].SecondaryFanOffTemperature = array[14 * i + 11];
			if (_currentECVersion == ECVersion.ECNUC21)
			{
				result.fanSettings[i].Configuration1 = (byte)((array[14 * i + 12] << 7) | array[14 * i + 13]);
				result.fanSettings[i].Configuration2 = 0;
			}
			else
			{
				result.fanSettings[i].Configuration1 = array[14 * i + 12];
				result.fanSettings[i].Configuration2 = array[14 * i + 13];
			}
		}
		return result;
	}

	private static AllFanSettings23 FillFanSetting23(byte[] EcData)
	{
		FanSetting23[] fanSettings = new FanSetting23[FanCount];
		AllFanSettings23 result = new AllFanSettings23
		{
			fanSettings = fanSettings
		};
		byte[] array = ((CurrentEcVersion != ECVersion.ECNUC23) ? new byte[FanCount * 14] : EcData);
		for (int i = 0; i < FanCount; i++)
		{
			result.fanSettings[i].PrimaryTemperatureSourceAndTemperatureHystersis = array[14 * i];
			result.fanSettings[i].RampAccelerationTemperature = array[14 * i + 1];
			result.fanSettings[i].PrimaryMinimumTemperature = array[14 * i + 2];
			result.fanSettings[i].PrimaryMinimumDutyCycleAndRampAcceleration = array[14 * i + 3];
			result.fanSettings[i].PrimaryDutyCycleIncrementAndSamplingPeriod = array[14 * i + 4];
			result.fanSettings[i].PrimaryFanCutOffTemperatureOffsetAndRampDownRate = array[14 * i + 5];
			result.fanSettings[i].SecondaryTemperatureSourceAndTemperatureHystersis = array[14 * i + 6];
			result.fanSettings[i].SecondaryRampAccelerationTemperature = array[14 * i + 7];
			result.fanSettings[i].SecondaryMinimumTemperature = array[14 * i + 8];
			result.fanSettings[i].SecondaryMinimumDutyCycleAndRampAcceleration = array[14 * i + 9];
			result.fanSettings[i].SecondaryDutyCycleIncrementAndSamplingPeriod = array[14 * i + 10];
			result.fanSettings[i].SecondaryFanCutOffTemperatureOffsetAndRampDownRate = array[14 * i + 11];
			result.fanSettings[i].FanControlModeAndSpeedRampRateLimit = array[14 * i + 12];
			result.fanSettings[i].MaximumDutyCycle = array[14 * i + 13];
		}
		return result;
	}

	private static byte[] ECReadProfile()
	{
		return ReadWriteControl.ReadFanProfile().Data;
	}

	private static byte[] ECReadTempratureProfile()
	{
		_ = new byte[15];
		return ReadWriteControl.ReadTempratureProfile().Data;
	}

	private static ECVersion GetCurrentECVersion()
	{
		ECVersion? eCVersion = CamdenBishopBios();
		if (eCVersion.HasValue)
		{
			return eCVersion.Value;
		}
		byte[] platformCapabilityReport = GetPlatformCapabilityReport();
		byte[] array = WmiWrapper.WmiCall(PLATFORM_CAPABILITY_REPORT, platformCapabilityReport);
		if (array.Length >= 54 && array[0] == NO_ERROR)
		{
			BitArray bitArray = new BitArray(new byte[1] { array[1] });
			if (bitArray[0] && bitArray[1])
			{
				return ECVersion.ECNUC23;
			}
			if (bitArray[0])
			{
				return ECVersion.ECNUC22;
			}
			if (!bitArray[0] && !bitArray[1])
			{
				return ECVersion.ECNUC21;
			}
			if (bitArray[2])
			{
				return ECVersion.ECElement;
			}
		}
		return ECVersion.Reserve;
	}

	public static ECVersion? CamdenBishopBios()
	{
		string biosString = WmiWrapper.GetBiosString();
		if (!string.IsNullOrEmpty(biosString))
		{
			if (biosString.Contains("EBTGL"))
			{
				return ECVersion.ECNUC21;
			}
			if (biosString.Contains("BCTGL"))
			{
				return ECVersion.ECBC;
			}
			if (biosString.Contains("HBADL"))
			{
				return ECVersion.ECNUC21;
			}
		}
		return null;
	}

	public static List<string> SupportedFanTypes()
	{
		return CurrentEcVersion switch
		{
			ECVersion.ECNUC21 => ReadWriteControl.GetFanType(NucEcSection.UseEcSection21), 
			ECVersion.ECNUC22 => ReadWriteControl.GetFanType(NucEcSection.UseEcSection22), 
			ECVersion.ECNUC23 => ReadWriteControl.GetFanType(NucEcSection.UseEcSection22), 
			ECVersion.ECBC => ReadWriteControl.GetFanType(NucEcSection.UseEcSectionBC), 
			_ => ReadWriteControl.FanTypes, 
		};
	}

	private static byte[] GetModeProfile(NucFanMode fanMode)
	{
		byte[] fanReportParameter = GetFanReportParameter(fanMode);
		byte[] array = WmiWrapper.WmiCall(FAN_PROFILE_REPORT_V1, fanReportParameter);
		if (array.Length == 0)
		{
			array = WmiWrapper.WmiCall(FAN_PROFILE_REPORT, fanReportParameter);
		}
		if (array.Length >= 82 && array[0] == NO_ERROR)
		{
			return array;
		}
		return new byte[0];
	}

	private static byte[] GetFanReportParameter(NucFanMode fanMode)
	{
		return new byte[4]
		{
			2,
			2,
			(byte)fanMode,
			0
		};
	}

	private static byte[] GetFanCapabilityParameter()
	{
		return new byte[4] { 2, 1, 0, 0 };
	}

	private static byte[] GetPlatformCapabilityReport()
	{
		return new byte[4] { 1, 0, 0, 0 };
	}
}
