using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace IntelSdkClient.PerformanceTuningService;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "FanSettingRN", Namespace = "http://schemas.datacontract.org/2004/07/PerformanceTuningLibrary")]
public struct FanSettingRN : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private byte FanControlModeAndSpeedRampRateLimitField;

	[OptionalField]
	private byte[] FanModeBytesField;

	[OptionalField]
	private string FanModeNameField;

	[OptionalField]
	private byte MaximumDutyCycleField;

	[OptionalField]
	private byte PrimaryDutyCycleIncrementAndSamplingPeriodField;

	[OptionalField]
	private byte PrimaryFanCutOffTemperatureOffsetAndRampDownRateField;

	[OptionalField]
	private byte PrimaryMinimumDutyCycleAndRampAccelerationField;

	[OptionalField]
	private byte PrimaryMinimumTemperatureField;

	[OptionalField]
	private byte PrimaryTemperatureSourceAndTemperatureHystersisField;

	[OptionalField]
	private byte RampAccelerationTemperatureField;

	[OptionalField]
	private byte SecondaryDutyCycleIncrementAndSamplingPeriodField;

	[OptionalField]
	private byte SecondaryFanCutOffTemperatureOffsetAndRampDownRateField;

	[OptionalField]
	private byte SecondaryMinimumDutyCycleAndRampAccelerationField;

	[OptionalField]
	private byte SecondaryMinimumTemperatureField;

	[OptionalField]
	private byte SecondaryRampAccelerationTemperatureField;

	[OptionalField]
	private byte SecondaryTemperatureSourceAndTemperatureHystersisField;

	public ExtensionDataObject ExtensionData
	{
		get
		{
			return extensionDataField;
		}
		set
		{
			extensionDataField = value;
		}
	}

	[DataMember]
	public byte FanControlModeAndSpeedRampRateLimit
	{
		get
		{
			return FanControlModeAndSpeedRampRateLimitField;
		}
		set
		{
			if (!FanControlModeAndSpeedRampRateLimitField.Equals(value))
			{
				FanControlModeAndSpeedRampRateLimitField = value;
				RaisePropertyChanged("FanControlModeAndSpeedRampRateLimit");
			}
		}
	}

	[DataMember]
	public byte[] FanModeBytes
	{
		get
		{
			return FanModeBytesField;
		}
		set
		{
			if (FanModeBytesField != value)
			{
				FanModeBytesField = value;
				RaisePropertyChanged("FanModeBytes");
			}
		}
	}

	[DataMember]
	public string FanModeName
	{
		get
		{
			return FanModeNameField;
		}
		set
		{
			if ((object)FanModeNameField != value)
			{
				FanModeNameField = value;
				RaisePropertyChanged("FanModeName");
			}
		}
	}

	[DataMember]
	public byte MaximumDutyCycle
	{
		get
		{
			return MaximumDutyCycleField;
		}
		set
		{
			if (!MaximumDutyCycleField.Equals(value))
			{
				MaximumDutyCycleField = value;
				RaisePropertyChanged("MaximumDutyCycle");
			}
		}
	}

	[DataMember]
	public byte PrimaryDutyCycleIncrementAndSamplingPeriod
	{
		get
		{
			return PrimaryDutyCycleIncrementAndSamplingPeriodField;
		}
		set
		{
			if (!PrimaryDutyCycleIncrementAndSamplingPeriodField.Equals(value))
			{
				PrimaryDutyCycleIncrementAndSamplingPeriodField = value;
				RaisePropertyChanged("PrimaryDutyCycleIncrementAndSamplingPeriod");
			}
		}
	}

	[DataMember]
	public byte PrimaryFanCutOffTemperatureOffsetAndRampDownRate
	{
		get
		{
			return PrimaryFanCutOffTemperatureOffsetAndRampDownRateField;
		}
		set
		{
			if (!PrimaryFanCutOffTemperatureOffsetAndRampDownRateField.Equals(value))
			{
				PrimaryFanCutOffTemperatureOffsetAndRampDownRateField = value;
				RaisePropertyChanged("PrimaryFanCutOffTemperatureOffsetAndRampDownRate");
			}
		}
	}

	[DataMember]
	public byte PrimaryMinimumDutyCycleAndRampAcceleration
	{
		get
		{
			return PrimaryMinimumDutyCycleAndRampAccelerationField;
		}
		set
		{
			if (!PrimaryMinimumDutyCycleAndRampAccelerationField.Equals(value))
			{
				PrimaryMinimumDutyCycleAndRampAccelerationField = value;
				RaisePropertyChanged("PrimaryMinimumDutyCycleAndRampAcceleration");
			}
		}
	}

	[DataMember]
	public byte PrimaryMinimumTemperature
	{
		get
		{
			return PrimaryMinimumTemperatureField;
		}
		set
		{
			if (!PrimaryMinimumTemperatureField.Equals(value))
			{
				PrimaryMinimumTemperatureField = value;
				RaisePropertyChanged("PrimaryMinimumTemperature");
			}
		}
	}

	[DataMember]
	public byte PrimaryTemperatureSourceAndTemperatureHystersis
	{
		get
		{
			return PrimaryTemperatureSourceAndTemperatureHystersisField;
		}
		set
		{
			if (!PrimaryTemperatureSourceAndTemperatureHystersisField.Equals(value))
			{
				PrimaryTemperatureSourceAndTemperatureHystersisField = value;
				RaisePropertyChanged("PrimaryTemperatureSourceAndTemperatureHystersis");
			}
		}
	}

	[DataMember]
	public byte RampAccelerationTemperature
	{
		get
		{
			return RampAccelerationTemperatureField;
		}
		set
		{
			if (!RampAccelerationTemperatureField.Equals(value))
			{
				RampAccelerationTemperatureField = value;
				RaisePropertyChanged("RampAccelerationTemperature");
			}
		}
	}

	[DataMember]
	public byte SecondaryDutyCycleIncrementAndSamplingPeriod
	{
		get
		{
			return SecondaryDutyCycleIncrementAndSamplingPeriodField;
		}
		set
		{
			if (!SecondaryDutyCycleIncrementAndSamplingPeriodField.Equals(value))
			{
				SecondaryDutyCycleIncrementAndSamplingPeriodField = value;
				RaisePropertyChanged("SecondaryDutyCycleIncrementAndSamplingPeriod");
			}
		}
	}

	[DataMember]
	public byte SecondaryFanCutOffTemperatureOffsetAndRampDownRate
	{
		get
		{
			return SecondaryFanCutOffTemperatureOffsetAndRampDownRateField;
		}
		set
		{
			if (!SecondaryFanCutOffTemperatureOffsetAndRampDownRateField.Equals(value))
			{
				SecondaryFanCutOffTemperatureOffsetAndRampDownRateField = value;
				RaisePropertyChanged("SecondaryFanCutOffTemperatureOffsetAndRampDownRate");
			}
		}
	}

	[DataMember]
	public byte SecondaryMinimumDutyCycleAndRampAcceleration
	{
		get
		{
			return SecondaryMinimumDutyCycleAndRampAccelerationField;
		}
		set
		{
			if (!SecondaryMinimumDutyCycleAndRampAccelerationField.Equals(value))
			{
				SecondaryMinimumDutyCycleAndRampAccelerationField = value;
				RaisePropertyChanged("SecondaryMinimumDutyCycleAndRampAcceleration");
			}
		}
	}

	[DataMember]
	public byte SecondaryMinimumTemperature
	{
		get
		{
			return SecondaryMinimumTemperatureField;
		}
		set
		{
			if (!SecondaryMinimumTemperatureField.Equals(value))
			{
				SecondaryMinimumTemperatureField = value;
				RaisePropertyChanged("SecondaryMinimumTemperature");
			}
		}
	}

	[DataMember]
	public byte SecondaryRampAccelerationTemperature
	{
		get
		{
			return SecondaryRampAccelerationTemperatureField;
		}
		set
		{
			if (!SecondaryRampAccelerationTemperatureField.Equals(value))
			{
				SecondaryRampAccelerationTemperatureField = value;
				RaisePropertyChanged("SecondaryRampAccelerationTemperature");
			}
		}
	}

	[DataMember]
	public byte SecondaryTemperatureSourceAndTemperatureHystersis
	{
		get
		{
			return SecondaryTemperatureSourceAndTemperatureHystersisField;
		}
		set
		{
			if (!SecondaryTemperatureSourceAndTemperatureHystersisField.Equals(value))
			{
				SecondaryTemperatureSourceAndTemperatureHystersisField = value;
				RaisePropertyChanged("SecondaryTemperatureSourceAndTemperatureHystersis");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	private void RaisePropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
