using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace IntelSdkClient.PerformanceTuningService;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "FanProfileRN", Namespace = "http://schemas.datacontract.org/2004/07/PerformanceTuningLibrary")]
public struct FanProfileRN : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private string CurrentBiosFanModeField;

	[OptionalField]
	private FanSettingRN[] fanSettingsField;

	[OptionalField]
	private string fanTypeField;

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
	public string CurrentBiosFanMode
	{
		get
		{
			return CurrentBiosFanModeField;
		}
		set
		{
			if ((object)CurrentBiosFanModeField != value)
			{
				CurrentBiosFanModeField = value;
				RaisePropertyChanged("CurrentBiosFanMode");
			}
		}
	}

	[DataMember]
	public FanSettingRN[] fanSettings
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
	public string fanType
	{
		get
		{
			return fanTypeField;
		}
		set
		{
			if ((object)fanTypeField != value)
			{
				fanTypeField = value;
				RaisePropertyChanged("fanType");
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
