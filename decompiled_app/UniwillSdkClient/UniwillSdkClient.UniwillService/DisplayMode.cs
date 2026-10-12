using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace UniwillSdkClient.UniwillService;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "DisplayMode", Namespace = "http://schemas.datacontract.org/2004/07/IntelSDK")]
public class DisplayMode : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private int BlueField;

	[OptionalField]
	private int BrightnessACField;

	[OptionalField]
	private int BrightnessDCField;

	[OptionalField]
	private int ColorTempField;

	[OptionalField]
	private float ContrastField;

	[OptionalField]
	private int GreenField;

	[OptionalField]
	private int IndexField;

	[OptionalField]
	private int RedField;

	[OptionalField]
	private DisplayModeType TypeField;

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
	public int Blue
	{
		get
		{
			return BlueField;
		}
		set
		{
			if (!BlueField.Equals(value))
			{
				BlueField = value;
				RaisePropertyChanged("Blue");
			}
		}
	}

	[DataMember]
	public int BrightnessAC
	{
		get
		{
			return BrightnessACField;
		}
		set
		{
			if (!BrightnessACField.Equals(value))
			{
				BrightnessACField = value;
				RaisePropertyChanged("BrightnessAC");
			}
		}
	}

	[DataMember]
	public int BrightnessDC
	{
		get
		{
			return BrightnessDCField;
		}
		set
		{
			if (!BrightnessDCField.Equals(value))
			{
				BrightnessDCField = value;
				RaisePropertyChanged("BrightnessDC");
			}
		}
	}

	[DataMember]
	public int ColorTemp
	{
		get
		{
			return ColorTempField;
		}
		set
		{
			if (!ColorTempField.Equals(value))
			{
				ColorTempField = value;
				RaisePropertyChanged("ColorTemp");
			}
		}
	}

	[DataMember]
	public float Contrast
	{
		get
		{
			return ContrastField;
		}
		set
		{
			if (!ContrastField.Equals(value))
			{
				ContrastField = value;
				RaisePropertyChanged("Contrast");
			}
		}
	}

	[DataMember]
	public int Green
	{
		get
		{
			return GreenField;
		}
		set
		{
			if (!GreenField.Equals(value))
			{
				GreenField = value;
				RaisePropertyChanged("Green");
			}
		}
	}

	[DataMember]
	public int Index
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

	[DataMember]
	public int Red
	{
		get
		{
			return RedField;
		}
		set
		{
			if (!RedField.Equals(value))
			{
				RedField = value;
				RaisePropertyChanged("Red");
			}
		}
	}

	[DataMember]
	public DisplayModeType Type
	{
		get
		{
			return TypeField;
		}
		set
		{
			if (!TypeField.Equals(value))
			{
				TypeField = value;
				RaisePropertyChanged("Type");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
