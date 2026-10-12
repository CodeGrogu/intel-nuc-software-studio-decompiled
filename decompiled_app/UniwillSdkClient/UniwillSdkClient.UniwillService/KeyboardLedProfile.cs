using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace UniwillSdkClient.UniwillService;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "KeyboardLedProfile", Namespace = "http://schemas.datacontract.org/2004/07/IntelSDK")]
public class KeyboardLedProfile : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private int Brightness_ACField;

	[OptionalField]
	private int Brightness_DCField;

	[OptionalField]
	private RGB[] ColorField;

	[OptionalField]
	private int ColorLengthField;

	[OptionalField]
	private RGBKeyboardDirection DirectionField;

	[OptionalField]
	private RGBKeyboardEffect EffectField;

	[OptionalField]
	private RGB[] KeyColorField;

	[OptionalField]
	private int SpeedField;

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
	public int Brightness_AC
	{
		get
		{
			return Brightness_ACField;
		}
		set
		{
			if (!Brightness_ACField.Equals(value))
			{
				Brightness_ACField = value;
				RaisePropertyChanged("Brightness_AC");
			}
		}
	}

	[DataMember]
	public int Brightness_DC
	{
		get
		{
			return Brightness_DCField;
		}
		set
		{
			if (!Brightness_DCField.Equals(value))
			{
				Brightness_DCField = value;
				RaisePropertyChanged("Brightness_DC");
			}
		}
	}

	[DataMember]
	public RGB[] Color
	{
		get
		{
			return ColorField;
		}
		set
		{
			if (ColorField != value)
			{
				ColorField = value;
				RaisePropertyChanged("Color");
			}
		}
	}

	[DataMember]
	public int ColorLength
	{
		get
		{
			return ColorLengthField;
		}
		set
		{
			if (!ColorLengthField.Equals(value))
			{
				ColorLengthField = value;
				RaisePropertyChanged("ColorLength");
			}
		}
	}

	[DataMember]
	public RGBKeyboardDirection Direction
	{
		get
		{
			return DirectionField;
		}
		set
		{
			if (!DirectionField.Equals(value))
			{
				DirectionField = value;
				RaisePropertyChanged("Direction");
			}
		}
	}

	[DataMember]
	public RGBKeyboardEffect Effect
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
	public RGB[] KeyColor
	{
		get
		{
			return KeyColorField;
		}
		set
		{
			if (KeyColorField != value)
			{
				KeyColorField = value;
				RaisePropertyChanged("KeyColor");
			}
		}
	}

	[DataMember]
	public int Speed
	{
		get
		{
			return SpeedField;
		}
		set
		{
			if (!SpeedField.Equals(value))
			{
				SpeedField = value;
				RaisePropertyChanged("Speed");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
