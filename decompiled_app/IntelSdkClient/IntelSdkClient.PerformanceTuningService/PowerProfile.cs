using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace IntelSdkClient.PerformanceTuningService;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "PowerProfile", Namespace = "http://schemas.datacontract.org/2004/07/PerformanceTuningLibrary")]
public struct PowerProfile : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private CpuMode CpuModeField;

	[OptionalField]
	private bool IsSelectedField;

	[OptionalField]
	private string NameField;

	[OptionalField]
	private uint PL1Field;

	[OptionalField]
	private uint PL2Field;

	[OptionalField]
	private uint TauField;

	[OptionalField]
	private int TauHtField;

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
	public CpuMode CpuMode
	{
		get
		{
			return CpuModeField;
		}
		set
		{
			if (!CpuModeField.Equals(value))
			{
				CpuModeField = value;
				RaisePropertyChanged("CpuMode");
			}
		}
	}

	[DataMember]
	public bool IsSelected
	{
		get
		{
			return IsSelectedField;
		}
		set
		{
			if (!IsSelectedField.Equals(value))
			{
				IsSelectedField = value;
				RaisePropertyChanged("IsSelected");
			}
		}
	}

	[DataMember]
	public string Name
	{
		get
		{
			return NameField;
		}
		set
		{
			if ((object)NameField != value)
			{
				NameField = value;
				RaisePropertyChanged("Name");
			}
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
	public uint Tau
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

	[DataMember]
	public int TauHt
	{
		get
		{
			return TauHtField;
		}
		set
		{
			if (!TauHtField.Equals(value))
			{
				TauHtField = value;
				RaisePropertyChanged("TauHt");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	private void RaisePropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
