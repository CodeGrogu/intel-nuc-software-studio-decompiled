using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace NucSoftwareStudio.UniwillService;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "RGB", Namespace = "http://schemas.datacontract.org/2004/07/IntelSDK")]
public struct RGB : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private byte BField;

	[OptionalField]
	private byte GField;

	[OptionalField]
	private byte RField;

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
	public byte B
	{
		get
		{
			return BField;
		}
		set
		{
			if (!BField.Equals(value))
			{
				BField = value;
				RaisePropertyChanged("B");
			}
		}
	}

	[DataMember]
	public byte G
	{
		get
		{
			return GField;
		}
		set
		{
			if (!GField.Equals(value))
			{
				GField = value;
				RaisePropertyChanged("G");
			}
		}
	}

	[DataMember]
	public byte R
	{
		get
		{
			return RField;
		}
		set
		{
			if (!RField.Equals(value))
			{
				RField = value;
				RaisePropertyChanged("R");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	private void RaisePropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
