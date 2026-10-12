using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace IntelSdkClient.SystemMonitorService;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "FanSpeed", Namespace = "http://schemas.datacontract.org/2004/07/PerformanceTuningLibrary")]
public struct FanSpeed : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private string nameField;

	[OptionalField]
	private uint speedField;

	[OptionalField]
	private FanType typeField;

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
	public string name
	{
		get
		{
			return nameField;
		}
		set
		{
			if ((object)nameField != value)
			{
				nameField = value;
				RaisePropertyChanged("name");
			}
		}
	}

	[DataMember]
	public uint speed
	{
		get
		{
			return speedField;
		}
		set
		{
			if (!speedField.Equals(value))
			{
				speedField = value;
				RaisePropertyChanged("speed");
			}
		}
	}

	[DataMember]
	public FanType type
	{
		get
		{
			return typeField;
		}
		set
		{
			if (!typeField.Equals(value))
			{
				typeField = value;
				RaisePropertyChanged("type");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	private void RaisePropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
