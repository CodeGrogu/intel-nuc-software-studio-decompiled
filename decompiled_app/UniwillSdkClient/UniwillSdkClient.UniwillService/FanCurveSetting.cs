using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace UniwillSdkClient.UniwillService;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "FanCurveSetting", Namespace = "http://schemas.datacontract.org/2004/07/IntelSDK")]
public class FanCurveSetting : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private bool ACField;

	[OptionalField]
	private FanTable[] FanCurveField;

	[OptionalField]
	private int FanModeField;

	[OptionalField]
	private FanType TableTypeField;

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
	public bool AC
	{
		get
		{
			return ACField;
		}
		set
		{
			if (!ACField.Equals(value))
			{
				ACField = value;
				RaisePropertyChanged("AC");
			}
		}
	}

	[DataMember]
	public FanTable[] FanCurve
	{
		get
		{
			return FanCurveField;
		}
		set
		{
			if (FanCurveField != value)
			{
				FanCurveField = value;
				RaisePropertyChanged("FanCurve");
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

	[DataMember]
	public FanType TableType
	{
		get
		{
			return TableTypeField;
		}
		set
		{
			if (!TableTypeField.Equals(value))
			{
				TableTypeField = value;
				RaisePropertyChanged("TableType");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
