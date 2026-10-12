using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace NucLedMgr.WMI.Models;

[JsonObject]
public class ControlItem : EventArgs
{
	private byte _value;

	[JsonProperty]
	public byte Index { get; set; }

	[JsonProperty]
	public byte Value
	{
		get
		{
			return _value;
		}
		set
		{
			_value = value;
			OnPropertyChanged("Value");
		}
	}

	[JsonProperty]
	public bool Supported { get; private set; }

	public event PropertyChangedEventHandler PropertyChanged;

	public ControlItem(byte index, bool supported)
	{
		Index = index;
		Supported = supported;
	}

	public ControlItem(byte index, byte value, bool supported)
	{
		Index = index;
		Value = value;
		Supported = supported;
	}

	[JsonConstructor]
	public ControlItem()
	{
	}

	private void OnPropertyChanged(string v)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
	}
}
