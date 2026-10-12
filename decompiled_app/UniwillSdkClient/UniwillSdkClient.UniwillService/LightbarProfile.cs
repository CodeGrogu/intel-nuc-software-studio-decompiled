using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace UniwillSdkClient.UniwillService;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "LightbarProfile", Namespace = "http://schemas.datacontract.org/2004/07/IntelSDK")]
public class LightbarProfile : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private LightbarSetting AcSettingField;

	[OptionalField]
	private bool BreathingEnableField;

	[OptionalField]
	private LightbarSetting DcSettingField;

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
	public LightbarSetting AcSetting
	{
		get
		{
			return AcSettingField;
		}
		set
		{
			if (AcSettingField != value)
			{
				AcSettingField = value;
				RaisePropertyChanged("AcSetting");
			}
		}
	}

	[DataMember]
	public bool BreathingEnable
	{
		get
		{
			return BreathingEnableField;
		}
		set
		{
			if (!BreathingEnableField.Equals(value))
			{
				BreathingEnableField = value;
				RaisePropertyChanged("BreathingEnable");
			}
		}
	}

	[DataMember]
	public LightbarSetting DcSetting
	{
		get
		{
			return DcSettingField;
		}
		set
		{
			if (DcSettingField != value)
			{
				DcSettingField = value;
				RaisePropertyChanged("DcSetting");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
