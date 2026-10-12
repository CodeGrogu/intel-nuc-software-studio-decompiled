using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace UniwillSdkClient.UniwillService;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "LightbarSetting", Namespace = "http://schemas.datacontract.org/2004/07/IntelSDK")]
public class LightbarSetting : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private int B_BrightnessField;

	[OptionalField]
	private LightBarEffect EffectField;

	[OptionalField]
	private int G_BrightnessField;

	[OptionalField]
	private int R_BrightnessField;

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
	public int B_Brightness
	{
		get
		{
			return B_BrightnessField;
		}
		set
		{
			if (!B_BrightnessField.Equals(value))
			{
				B_BrightnessField = value;
				RaisePropertyChanged("B_Brightness");
			}
		}
	}

	[DataMember]
	public LightBarEffect Effect
	{
		get
		{
			return EffectField;
		}
		set
		{
			if (!EffectField.Equals(value))
			{
				EffectField = value;
				RaisePropertyChanged("Effect");
			}
		}
	}

	[DataMember]
	public int G_Brightness
	{
		get
		{
			return G_BrightnessField;
		}
		set
		{
			if (!G_BrightnessField.Equals(value))
			{
				G_BrightnessField = value;
				RaisePropertyChanged("G_Brightness");
			}
		}
	}

	[DataMember]
	public int R_Brightness
	{
		get
		{
			return R_BrightnessField;
		}
		set
		{
			if (!R_BrightnessField.Equals(value))
			{
				R_BrightnessField = value;
				RaisePropertyChanged("R_Brightness");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
