using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace IntelSdkClient.PerformanceTuningService;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "FanControl.AllFanSettings", Namespace = "http://schemas.datacontract.org/2004/07/PerformanceTuningLibrary")]
public struct FanControlAllFanSettings : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private FanSetting[] fanSettingsField;

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
	public FanSetting[] fanSettings
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

	public event PropertyChangedEventHandler PropertyChanged;

	private void RaisePropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
