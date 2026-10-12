using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace IntelSdkClient.PerformanceTuningService;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "FanSetting", Namespace = "http://schemas.datacontract.org/2004/07/PerformanceTuningLibrary")]
public struct FanSetting : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private byte Configuration1Field;

	[OptionalField]
	private byte Configuration2Field;

	[OptionalField]
	private byte PrimaryDutyIncrementField;

	[OptionalField]
	private byte PrimaryFanOffTemperatureField;

	[OptionalField]
	private byte PrimaryMaximumTemperatureField;

	[OptionalField]
	private byte PrimaryMinimumDutyField;

	[OptionalField]
	private byte PrimaryMinimumTemperatureField;

	[OptionalField]
	private byte SecondaryDutyIncrementField;

	[OptionalField]
	private byte SecondaryFanOffTemperatureField;

	[OptionalField]
	private byte SecondaryMaximumTemperatureField;

	[OptionalField]
	private byte SecondaryMinimumDutyField;

	[OptionalField]
	private byte SecondaryMinimumTemperatureField;

	[OptionalField]
	private PrimaryInputSourceConfiguration primaryInputSourceConfigurationField;

	[OptionalField]
	private SecondaryInputSourceConfiguration secondaryInputSourceConfigurationField;

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
	public byte Configuration1
	{
		get
		{
			return Configuration1Field;
		}
		set
		{
			if (!Configuration1Field.Equals(value))
			{
				Configuration1Field = value;
				RaisePropertyChanged("Configuration1");
			}
		}
	}

	[DataMember]
	public byte Configuration2
	{
		get
		{
			return Configuration2Field;
		}
		set
		{
			if (!Configuration2Field.Equals(value))
			{
				Configuration2Field = value;
				RaisePropertyChanged("Configuration2");
			}
		}
	}

	[DataMember]
	public byte PrimaryDutyIncrement
	{
		get
		{
			return PrimaryDutyIncrementField;
		}
		set
		{
			if (!PrimaryDutyIncrementField.Equals(value))
			{
				PrimaryDutyIncrementField = value;
				RaisePropertyChanged("PrimaryDutyIncrement");
			}
		}
	}

	[DataMember]
	public byte PrimaryFanOffTemperature
	{
		get
		{
			return PrimaryFanOffTemperatureField;
		}
		set
		{
			if (!PrimaryFanOffTemperatureField.Equals(value))
			{
				PrimaryFanOffTemperatureField = value;
				RaisePropertyChanged("PrimaryFanOffTemperature");
			}
		}
	}

	[DataMember]
	public byte PrimaryMaximumTemperature
	{
		get
		{
			return PrimaryMaximumTemperatureField;
		}
		set
		{
			if (!PrimaryMaximumTemperatureField.Equals(value))
			{
				PrimaryMaximumTemperatureField = value;
				RaisePropertyChanged("PrimaryMaximumTemperature");
			}
		}
	}

	[DataMember]
	public byte PrimaryMinimumDuty
	{
		get
		{
			return PrimaryMinimumDutyField;
		}
		set
		{
			if (!PrimaryMinimumDutyField.Equals(value))
			{
				PrimaryMinimumDutyField = value;
				RaisePropertyChanged("PrimaryMinimumDuty");
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
	public byte SecondaryDutyIncrement
	{
		get
		{
			return SecondaryDutyIncrementField;
		}
		set
		{
			if (!SecondaryDutyIncrementField.Equals(value))
			{
				SecondaryDutyIncrementField = value;
				RaisePropertyChanged("SecondaryDutyIncrement");
			}
		}
	}

	[DataMember]
	public byte SecondaryFanOffTemperature
	{
		get
		{
			return SecondaryFanOffTemperatureField;
		}
		set
		{
			if (!SecondaryFanOffTemperatureField.Equals(value))
			{
				SecondaryFanOffTemperatureField = value;
				RaisePropertyChanged("SecondaryFanOffTemperature");
			}
		}
	}

	[DataMember]
	public byte SecondaryMaximumTemperature
	{
		get
		{
			return SecondaryMaximumTemperatureField;
		}
		set
		{
			if (!SecondaryMaximumTemperatureField.Equals(value))
			{
				SecondaryMaximumTemperatureField = value;
				RaisePropertyChanged("SecondaryMaximumTemperature");
			}
		}
	}

	[DataMember]
	public byte SecondaryMinimumDuty
	{
		get
		{
			return SecondaryMinimumDutyField;
		}
		set
		{
			if (!SecondaryMinimumDutyField.Equals(value))
			{
				SecondaryMinimumDutyField = value;
				RaisePropertyChanged("SecondaryMinimumDuty");
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
	public PrimaryInputSourceConfiguration primaryInputSourceConfiguration
	{
		get
		{
			return primaryInputSourceConfigurationField;
		}
		set
		{
			if (!primaryInputSourceConfigurationField.Equals(value))
			{
				primaryInputSourceConfigurationField = value;
				RaisePropertyChanged("primaryInputSourceConfiguration");
			}
		}
	}

	[DataMember]
	public SecondaryInputSourceConfiguration secondaryInputSourceConfiguration
	{
		get
		{
			return secondaryInputSourceConfigurationField;
		}
		set
		{
			if (!secondaryInputSourceConfigurationField.Equals(value))
			{
				secondaryInputSourceConfigurationField = value;
				RaisePropertyChanged("secondaryInputSourceConfiguration");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	private void RaisePropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
