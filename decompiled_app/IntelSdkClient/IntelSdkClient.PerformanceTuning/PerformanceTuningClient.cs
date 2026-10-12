using System;
using System.Collections.Generic;
using System.ServiceModel;
using IntelSdkClient.PerformanceTuning.Interface;
using IntelSdkClient.PerformanceTuningService;

namespace IntelSdkClient.PerformanceTuning;

public class PerformanceTuningClient : IPerformanceTuningClient
{
	private const string PERFORMANCE_TUNING_SERVICE_URI = "http://localhost:43568/NssService/service/PerformanceTuning";

	private const string INNER_EXCEPTION = "GetFanProfileData23' is invalid or unrecognized.";

	private WSHttpBinding _performanceTuningBinding;

	private EndpointAddress _performanceTuningEndpoint;

	private ChannelFactory<IPerformanceTuningServiceContract> _channelFactory;

	private IPerformanceTuningServiceContract _client;

	public PerformanceTuningClient()
	{
		CreateChannel();
	}

	private void CreateChannel()
	{
		_performanceTuningBinding = new WSHttpBinding();
		_performanceTuningBinding.MaxReceivedMessageSize = 10485760L;
		_performanceTuningEndpoint = new EndpointAddress("http://localhost:43568/NssService/service/PerformanceTuning");
		_channelFactory = new ChannelFactory<IPerformanceTuningServiceContract>(_performanceTuningBinding, _performanceTuningEndpoint);
		_client = _channelFactory.CreateChannel();
	}

	public bool? GetPowerLevelControlSupported()
	{
		try
		{
			if (_client == null)
			{
				CreateChannel();
			}
			return _client.GetPowerLevelControlSupported();
		}
		catch (CommunicationException)
		{
			_channelFactory.Abort();
			CreateChannel();
			return _client.GetPowerLevelControlSupported();
		}
		catch (Exception)
		{
			return false;
		}
	}

	public PowerSetting GetCurrentPowerSettings()
	{
		try
		{
			if (_client == null)
			{
				CreateChannel();
			}
			return _client.GetCurrentPowerSettings();
		}
		catch (CommunicationException)
		{
			_channelFactory.Abort();
			CreateChannel();
			return _client.GetCurrentPowerSettings();
		}
		catch (Exception)
		{
			return default;
		}
	}

	public PowerProfile[] GetSupportedPowerProfiles()
	{
		try
		{
			if (_client == null)
			{
				CreateChannel();
			}
			return _client.GetSupportedPowerProfiles();
		}
		catch (CommunicationException)
		{
			_channelFactory.Abort();
			CreateChannel();
			return _client.GetSupportedPowerProfiles();
		}
		catch (Exception)
		{
			return new PowerProfile[0];
		}
	}

	public bool? IsFanEcSupported()
	{
		try
		{
			return _client.IsFanEcSupported();
		}
		catch (CommunicationException)
		{
			_channelFactory.Abort();
			CreateChannel();
			return _client.IsFanEcSupported();
		}
		catch (Exception)
		{
			return false;
		}
	}

	public bool? IsFanProfilesSupported()
	{
		try
		{
			if (_client == null)
			{
				CreateChannel();
			}
			return _client.IsFanProfilesSupported();
		}
		catch (CommunicationException)
		{
			_channelFactory.Abort();
			CreateChannel();
			return _client.IsFanProfilesSupported();
		}
		catch (Exception)
		{
			return false;
		}
	}

	public string[] GetSupportedFanModes()
	{
		try
		{
			if (_client == null)
			{
				CreateChannel();
			}
			return _client.GetSupportedFanModes();
		}
		catch (CommunicationException)
		{
			_channelFactory.Abort();
			CreateChannel();
			return _client.GetSupportedFanModes();
		}
		catch (Exception)
		{
			return new string[0];
		}
	}

	public string GetCurrentBiosFanModeName()
	{
		try
		{
			if (_client == null)
			{
				CreateChannel();
			}
			return _client.GetCurrentBiosFanModeName();
		}
		catch (CommunicationException)
		{
			_channelFactory.Abort();
			CreateChannel();
			return _client.GetCurrentBiosFanModeName();
		}
		catch (Exception)
		{
			return string.Empty;
		}
	}

	public bool SetFanMode(string NewMode)
	{
		try
		{
			if (_client == null)
			{
				CreateChannel();
			}
			return _client.SetFanMode(NewMode);
		}
		catch (CommunicationException)
		{
			_channelFactory.Abort();
			CreateChannel();
			return _client.SetFanMode(NewMode);
		}
		catch (Exception)
		{
			return false;
		}
	}

	public bool SetFanModeRN(string NewMode, string fanType, List<FanProfileRN> FanProfiles)
	{
		try
		{
			if (_client == null)
			{
				CreateChannel();
			}
			return _client.SetFanModeRN(NewMode, fanType, FanProfiles.ToArray());
		}
		catch (CommunicationException)
		{
			_channelFactory.Abort();
			CreateChannel();
			return _client.SetFanModeRN(NewMode, fanType, FanProfiles.ToArray());
		}
		catch (Exception)
		{
			return false;
		}
	}

	public bool SetCustomPowerSetting(PowerSetting powerSetting)
	{
		try
		{
			if (_client == null)
			{
				CreateChannel();
			}
			return _client.SetCustomPowerSetting(powerSetting);
		}
		catch (CommunicationException)
		{
			_channelFactory.Abort();
			CreateChannel();
			return _client.SetCustomPowerSetting(powerSetting);
		}
		catch (Exception)
		{
			return false;
		}
	}

	public bool ApplyBiosPowerProfile(string powerSettingName)
	{
		try
		{
			if (_client == null)
			{
				CreateChannel();
			}
			return _client.ApplyBiosPowerProfile(powerSettingName);
		}
		catch (CommunicationException)
		{
			_channelFactory.Abort();
			CreateChannel();
			return _client.ApplyBiosPowerProfile(powerSettingName);
		}
		catch (Exception)
		{
			return false;
		}
	}

	public FanProfile GetFanProfileData(string currentFanMode)
	{
		try
		{
			if (_client == null)
			{
				CreateChannel();
			}
			return _client.GetFanProfileData(currentFanMode);
		}
		catch (CommunicationException)
		{
			_channelFactory.Abort();
			CreateChannel();
			return _client.GetFanProfileData(currentFanMode);
		}
		catch (Exception)
		{
			return default;
		}
	}

	public FanProfile23 GetFanProfileData23(string currentFanMode)
	{
		try
		{
			if (_client == null)
			{
				CreateChannel();
			}
			return _client.GetFanProfileData23(currentFanMode);
		}
		catch (CommunicationException ex)
		{
			if (!ex.InnerException.Message.Contains("GetFanProfileData23' is invalid or unrecognized."))
			{
				_channelFactory.Abort();
				CreateChannel();
				return _client.GetFanProfileData23(currentFanMode);
			}
			return default;
		}
		catch (Exception)
		{
			return default;
		}
	}

	public string[] SupportedFanTypes()
	{
		try
		{
			if (_client == null)
			{
				CreateChannel();
			}
			return _client.SupportedFanTypes();
		}
		catch (CommunicationException)
		{
			try
			{
				_channelFactory.Abort();
				CreateChannel();
				return _client.SupportedFanTypes();
			}
			catch (Exception)
			{
				return new string[0];
			}
		}
		catch (Exception)
		{
			return new string[0];
		}
	}

	public bool SetFanCustom(FanControlAllFanSettings allFanSettings)
	{
		try
		{
			if (_client == null)
			{
				CreateChannel();
			}
			return _client.SetFanCustom(allFanSettings);
		}
		catch (CommunicationException)
		{
			_channelFactory.Abort();
			CreateChannel();
			return _client.SetFanCustom(allFanSettings);
		}
		catch (Exception)
		{
			return false;
		}
	}

	public bool SetFanOffConfiguration(string selectedFanMode, bool isFanOff)
	{
		try
		{
			if (_client == null)
			{
				CreateChannel();
			}
			return _client.SetFanOffConfiguration(selectedFanMode, isFanOff);
		}
		catch (CommunicationException)
		{
			_channelFactory.Abort();
			CreateChannel();
			return _client.SetFanOffConfiguration(selectedFanMode, isFanOff);
		}
		catch (Exception)
		{
			return false;
		}
	}

	public bool SetFanDutyCycleFixedMode(string selectedFanMode, byte[] FanBytes)
	{
		try
		{
			if (_client == null)
			{
				CreateChannel();
			}
			return _client.SetFanDutyCycleFixedMode(selectedFanMode, FanBytes);
		}
		catch (CommunicationException)
		{
			_channelFactory.Abort();
			CreateChannel();
			return _client.SetFanDutyCycleFixedMode(selectedFanMode, FanBytes);
		}
		catch (Exception)
		{
			return false;
		}
	}

	public List<FanProfileRN> GetFanProfileDataRN()
	{
		try
		{
			if (_client == null)
			{
				CreateChannel();
			}
			return new List<FanProfileRN>(_client.GetFanProfileDataRN());
		}
		catch (CommunicationException)
		{
			_channelFactory.Abort();
			CreateChannel();
			return new List<FanProfileRN>(_client.GetFanProfileDataRN());
		}
		catch (Exception)
		{
			return new List<FanProfileRN>();
		}
	}
}
