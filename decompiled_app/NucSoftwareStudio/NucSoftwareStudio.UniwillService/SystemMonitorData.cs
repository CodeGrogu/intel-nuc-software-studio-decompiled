using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace NucSoftwareStudio.UniwillService;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "SystemMonitorData", Namespace = "http://schemas.datacontract.org/2004/07/IntelSDK")]
public class SystemMonitorData : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private Dictionary<string, string> DataField;

	[OptionalField]
	private SysMonType DataTypeField;

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
	public Dictionary<string, string> Data
	{
		get
		{
			return DataField;
		}
		set
		{
			if (DataField != value)
			{
				DataField = value;
				RaisePropertyChanged("Data");
			}
		}
	}

	[DataMember]
	public SysMonType DataType
	{
		get
		{
			return DataTypeField;
		}
		set
		{
			if (!DataTypeField.Equals(value))
			{
				DataTypeField = value;
				RaisePropertyChanged("DataType");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
