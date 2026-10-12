using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace IntelSdkClient.PerformanceTuningService;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "FanProfile23", Namespace = "http://schemas.datacontract.org/2004/07/PerformanceTuningLibrary")]
public struct FanProfile23 : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private FanSetting23[] fanSettingsField;

	[OptionalField]
	private string nameField;

	[OptionalField]
	private byte[] profileField;

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
	public FanSetting23[] fanSettings
	{
		get
		{
			return fanSettingsField;
		}
		set
		{
			if (fanSettingsField != value)
			{
				fanSettingsField = value;
				RaisePropertyChanged("fanSettings");
			}
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
	public byte[] profile
	{
		get
		{
			return profileField;
		}
		set
		{
			if (profileField != value)
			{
				profileField = value;
				RaisePropertyChanged("profile");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	private void RaisePropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
