using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace IntelSdkClient.PerformanceTuningService;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "PowerSetting", Namespace = "http://schemas.datacontract.org/2004/07/PerformanceTuningLibrary")]
public struct PowerSetting : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private uint PL1Field;

	[OptionalField]
	private uint PL2Field;

	[OptionalField]
	private int TauField;

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
	public uint PL1
	{
		get
		{
			return PL1Field;
		}
		set
		{
			if (!PL1Field.Equals(value))
			{
				PL1Field = value;
				RaisePropertyChanged("PL1");
			}
		}
	}

	[DataMember]
	public uint PL2
	{
		get
		{
			return PL2Field;
		}
		set
		{
			if (!PL2Field.Equals(value))
			{
				PL2Field = value;
				RaisePropertyChanged("PL2");
			}
		}
	}

	[DataMember]
	public int Tau
	{
		get
		{
			return TauField;
		}
		set
		{
			if (!TauField.Equals(value))
			{
				TauField = value;
				RaisePropertyChanged("Tau");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	private void RaisePropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
