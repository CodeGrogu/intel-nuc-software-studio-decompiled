using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace IntelSdkClient.EneService;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "LedPattern", Namespace = "http://schemas.datacontract.org/2004/07/NucSoftwareStudioService.LedControl.Models")]
public class LedPattern : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private byte ChannelField;

	[OptionalField]
	private byte MaxBrightnessField;

	[OptionalField]
	private byte[] PaletteField;

	[OptionalField]
	private byte PatternIndexField;

	[OptionalField]
	private byte RainBowField;

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
	public byte Channel
	{
		get
		{
			return ChannelField;
		}
		set
		{
			if (!ChannelField.Equals(value))
			{
				ChannelField = value;
				RaisePropertyChanged("Channel");
			}
		}
	}

	[DataMember]
	public byte MaxBrightness
	{
		get
		{
			return MaxBrightnessField;
		}
		set
		{
			if (!MaxBrightnessField.Equals(value))
			{
				MaxBrightnessField = value;
				RaisePropertyChanged("MaxBrightness");
			}
		}
	}

	[DataMember]
	public byte[] Palette
	{
		get
		{
			return PaletteField;
		}
		set
		{
			if (PaletteField != value)
			{
				PaletteField = value;
				RaisePropertyChanged("Palette");
			}
		}
	}

	[DataMember]
	public byte PatternIndex
	{
		get
		{
			return PatternIndexField;
		}
		set
		{
			if (!PatternIndexField.Equals(value))
			{
				PatternIndexField = value;
				RaisePropertyChanged("PatternIndex");
			}
		}
	}

	[DataMember]
	public byte RainBow
	{
		get
		{
			return RainBowField;
		}
		set
		{
			if (!RainBowField.Equals(value))
			{
				RainBowField = value;
				RaisePropertyChanged("RainBow");
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
