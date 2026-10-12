using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace UniwillSdkClient.UniwillService;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "PerformanceMode", Namespace = "http://schemas.datacontract.org/2004/07/IntelSDK")]
public class PerformanceMode : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private int CpuModeField;

	[OptionalField]
	private int DGpuModeField;

	[OptionalField]
	private int FanModeField;

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
	public int CpuMode
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
	public int DGpuMode
	{
		get
		{
			return DGpuModeField;
		}
		set
		{
			if (!DGpuModeField.Equals(value))
			{
				DGpuModeField = value;
				RaisePropertyChanged("DGpuMode");
			}
		}
	}

	[DataMember]
	public int FanMode
	{
		get
		{
			return FanModeField;
		}
		set
		{
			if (!FanModeField.Equals(value))
			{
				FanModeField = value;
				RaisePropertyChanged("FanMode");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
