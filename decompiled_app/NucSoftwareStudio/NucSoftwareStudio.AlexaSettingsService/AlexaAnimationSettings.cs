using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace NucSoftwareStudio.AlexaSettingsService;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "AlexaAnimationSettings", Namespace = "http://schemas.datacontract.org/2004/07/NucSoftwareStudioService.LedControl.Models")]
public class AlexaAnimationSettings : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private byte BrightnessField;

	[OptionalField]
	private byte DirectionField;

	[OptionalField]
	private byte SpeedField;

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
	public byte Brightness
	{
		get
		{
			return BrightnessField;
		}
		set
		{
			if (!BrightnessField.Equals(value))
			{
				BrightnessField = value;
				RaisePropertyChanged("Brightness");
			}
		}
	}

	[DataMember]
	public byte Direction
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
	public byte Speed
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
