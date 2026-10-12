using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace IntelSdkClient.EneService;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "LedStatus", Namespace = "http://schemas.datacontract.org/2004/07/NucSoftwareStudioService.LedControl.Models")]
public class LedStatus : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private BntStatus ButtonStatusField;

	[OptionalField]
	private LedPattern[] LedPatternListField;

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
	public BntStatus ButtonStatus
	{
		get
		{
			return ButtonStatusField;
		}
		set
		{
			if (!ButtonStatusField.Equals(value))
			{
				ButtonStatusField = value;
				RaisePropertyChanged("ButtonStatus");
			}
		}
	}

	[DataMember]
	public LedPattern[] LedPatternList
	{
		get
		{
			return LedPatternListField;
		}
		set
		{
			if (LedPatternListField != value)
			{
				LedPatternListField = value;
				RaisePropertyChanged("LedPatternList");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
