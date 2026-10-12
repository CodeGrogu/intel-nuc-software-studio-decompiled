using System;

namespace PerformanceTuningLibrary;

public struct PowerProfile : IEquatable<PowerProfile>
{
	public string Name { get; set; }

	public uint PL1 { get; set; }

	public uint PL2 { get; set; }

	public uint Tau { get; set; }

	public int TauHt { get; set; }

	public CpuMode CpuMode { get; set; }

	public bool IsSelected { get; set; }

	public override bool Equals(object obj)
	{
		if (!(obj is PowerProfile other))
		{
			return false;
		}
		return Equals(other);
	}

	public bool Equals(PowerProfile other)
	{
		if (PL1 == other.PL1 && PL2 == other.PL2)
		{
			return Tau == other.Tau;
		}
		return false;
	}

	public bool Equals(PowerSetting other)
	{
		if (PL1 == other.PL1 && PL2 == other.PL2)
		{
			return Tau == other.Tau;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return ((unchecked(494222874 * -1521134295) + PL1.GetHashCode()) * -1521134295 + PL2.GetHashCode()) * -1521134295 + Tau.GetHashCode();
	}
}
