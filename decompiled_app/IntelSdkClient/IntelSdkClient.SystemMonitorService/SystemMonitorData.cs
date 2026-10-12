using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace IntelSdkClient.SystemMonitorService;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "SystemMonitorData", Namespace = "http://schemas.datacontract.org/2004/07/SystemMonitorLibrary.Hardware")]
public class SystemMonitorData : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private Dictionary<string, string> BatteryInformationField;

	[OptionalField]
	private Dictionary<string, string> DiscreteGpuInformationField;

	[OptionalField]
	private Dictionary<string, string> IntegratedGpuInformationField;

	[OptionalField]
	private Dictionary<string, string> MemoryInformationField;

	[OptionalField]
	private Dictionary<string, string> ProcessorInformationField;

	[OptionalField]
	private Dictionary<string, string>[] StorageInformationField;

	[OptionalField]
	private Dictionary<string, string> SystemInformationField;

	[Browsable(false)]
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
	public Dictionary<string, string> BatteryInformation
	{
		get
		{
			return BatteryInformationField;
		}
		set
		{
			if (BatteryInformationField != value)
			{
				BatteryInformationField = value;
				RaisePropertyChanged("BatteryInformation");
			}
		}
	}

	[DataMember]
	public Dictionary<string, string> DiscreteGpuInformation
	{
		get
		{
			return DiscreteGpuInformationField;
		}
		set
		{
			if (DiscreteGpuInformationField != value)
			{
				DiscreteGpuInformationField = value;
				RaisePropertyChanged("DiscreteGpuInformation");
			}
		}
	}

	[DataMember]
	public Dictionary<string, string> IntegratedGpuInformation
	{
		get
		{
			return IntegratedGpuInformationField;
		}
		set
		{
			if (IntegratedGpuInformationField != value)
			{
				IntegratedGpuInformationField = value;
				RaisePropertyChanged("IntegratedGpuInformation");
			}
		}
	}

	[DataMember]
	public Dictionary<string, string> MemoryInformation
	{
		get
		{
			return MemoryInformationField;
		}
		set
		{
			if (MemoryInformationField != value)
			{
				MemoryInformationField = value;
				RaisePropertyChanged("MemoryInformation");
			}
		}
	}

	[DataMember]
	public Dictionary<string, string> ProcessorInformation
	{
		get
		{
			return ProcessorInformationField;
		}
		set
		{
			if (ProcessorInformationField != value)
			{
				ProcessorInformationField = value;
				RaisePropertyChanged("ProcessorInformation");
			}
		}
	}

	[DataMember]
	public Dictionary<string, string>[] StorageInformation
	{
		get
		{
			return StorageInformationField;
		}
		set
		{
			if (StorageInformationField != value)
			{
				StorageInformationField = value;
				RaisePropertyChanged("StorageInformation");
			}
		}
	}

	[DataMember]
	public Dictionary<string, string> SystemInformation
	{
		get
		{
			return SystemInformationField;
		}
		set
		{
			if (SystemInformationField != value)
			{
				SystemInformationField = value;
				RaisePropertyChanged("SystemInformation");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
