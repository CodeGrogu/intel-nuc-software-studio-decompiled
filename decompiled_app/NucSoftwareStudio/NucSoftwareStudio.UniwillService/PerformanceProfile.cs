using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace NucSoftwareStudio.UniwillService;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "PerformanceProfile", Namespace = "http://schemas.datacontract.org/2004/07/IntelSDK")]
public class PerformanceProfile : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private PerformanceMode AcField;

	[OptionalField]
	private PerformanceMode DcField;

	[OptionalField]
	private ProfileIndex IndexField;

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
	public PerformanceMode Ac
	{
		get
		{
			return AcField;
		}
		set
		{
			if (AcField != value)
			{
				AcField = value;
				RaisePropertyChanged("Ac");
			}
		}
	}

	[DataMember]
	public PerformanceMode Dc
	{
		get
		{
			return DcField;
		}
		set
		{
			if (DcField != value)
			{
				DcField = value;
				RaisePropertyChanged("Dc");
			}
		}
	}

	[DataMember]
	public ProfileIndex Index
	{
		get
		{
			return IndexField;
		}
		set
		{
			if (!IndexField.Equals(value))
			{
				IndexField = value;
				RaisePropertyChanged("Index");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
