using System;

namespace NucLedMgr.WMI.Models.Profiles;

public sealed class SerializableControlItem : IEquatable<SerializableControlItem>
{
	public byte Index { get; set; }

	public byte Value { get; set; }

	public override bool Equals(object obj)
	{
		if (obj is SerializableControlItem serializableControlItem && Index == serializableControlItem.Index)
		{
			return Value == serializableControlItem.Value;
		}
		return false;
	}

	public bool Equals(SerializableControlItem other)
	{
		if (Index == other.Index)
		{
			return Value == other.Value;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (unchecked(1376307821 * -1521134295) + Index.GetHashCode()) * -1521134295 + Value.GetHashCode();
	}
}
