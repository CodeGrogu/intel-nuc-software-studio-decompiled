using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace NucSoftwareStudio.UniwillService;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "FanTable", Namespace = "http://schemas.datacontract.org/2004/07/IntelSDK")]
public class FanTable : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private int DownTField;

	[OptionalField]
	private int DutyField;

	[OptionalField]
	private int IdField;

	[OptionalField]
	private int PostTimeField;

	[OptionalField]
	private int PreTimeField;

	[OptionalField]
	private int UpTField;

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
	public int DownT
	{
		get
		{
			return DownTField;
		}
		set
		{
			if (!DownTField.Equals(value))
			{
				DownTField = value;
				RaisePropertyChanged("DownT");
			}
		}
	}

	[DataMember]
	public int Duty
	{
		get
		{
			return DutyField;
		}
		set
		{
			if (!DutyField.Equals(value))
			{
				DutyField = value;
				RaisePropertyChanged("Duty");
			}
		}
	}

	[DataMember]
	public int Id
	{
		get
		{
			return IdField;
		}
		set
		{
			if (!IdField.Equals(value))
			{
				IdField = value;
				RaisePropertyChanged("Id");
			}
		}
	}

	[DataMember]
	public int PostTime
	{
		get
		{
			return PostTimeField;
		}
		set
		{
			if (!PostTimeField.Equals(value))
			{
				PostTimeField = value;
				RaisePropertyChanged("PostTime");
			}
		}
	}

	[DataMember]
	public int PreTime
	{
		get
		{
			return PreTimeField;
		}
		set
		{
			if (!PreTimeField.Equals(value))
			{
				PreTimeField = value;
				RaisePropertyChanged("PreTime");
			}
		}
	}

	[DataMember]
	public int UpT
	{
		get
		{
			return UpTField;
		}
		set
		{
			if (!UpTField.Equals(value))
			{
				UpTField = value;
				RaisePropertyChanged("UpT");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
