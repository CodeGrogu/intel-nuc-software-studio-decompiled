using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace UniwillSdkClient.UniwillService;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "PerformanceState", Namespace = "http://schemas.datacontract.org/2004/07/IntelSDK")]
public class PerformanceState : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private bool BenchmarkModeField;

	[OptionalField]
	private bool PassiveCoolingField;

	[OptionalField]
	private ProfileIndex SelectedProfileIndexField;

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
	public bool BenchmarkMode
	{
		get
		{
			return BenchmarkModeField;
		}
		set
		{
			if (!BenchmarkModeField.Equals(value))
			{
				BenchmarkModeField = value;
				RaisePropertyChanged("BenchmarkMode");
			}
		}
	}

	[DataMember]
	public bool PassiveCooling
	{
		get
		{
			return PassiveCoolingField;
		}
		set
		{
			if (!PassiveCoolingField.Equals(value))
			{
				PassiveCoolingField = value;
				RaisePropertyChanged("PassiveCooling");
			}
		}
	}

	[DataMember]
	public ProfileIndex SelectedProfileIndex
	{
		get
		{
			return SelectedProfileIndexField;
		}
		set
		{
			if (!SelectedProfileIndexField.Equals(value))
			{
				SelectedProfileIndexField = value;
				RaisePropertyChanged("SelectedProfileIndex");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
