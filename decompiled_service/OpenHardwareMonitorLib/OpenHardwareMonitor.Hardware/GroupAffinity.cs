namespace OpenHardwareMonitor.Hardware;

internal struct GroupAffinity(ushort group, ulong mask)
{
	public static GroupAffinity Undefined = new GroupAffinity(ushort.MaxValue, 0uL);

	public ushort Group { get; } = group;

	public ulong Mask { get; } = mask;

	public static GroupAffinity Single(ushort group, int index)
	{
		return new GroupAffinity(group, (ulong)(1L << index));
	}

	public override bool Equals(object o)
	{
		if (o == null || GetType() != o.GetType())
		{
			return false;
		}
		GroupAffinity groupAffinity = (GroupAffinity)o;
		if (Group == groupAffinity.Group)
		{
			return Mask == groupAffinity.Mask;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return Group.GetHashCode() ^ Mask.GetHashCode();
	}

	public static bool operator ==(GroupAffinity a1, GroupAffinity a2)
	{
		if (a1.Group == a2.Group)
		{
			return a1.Mask == a2.Mask;
		}
		return false;
	}

	public static bool operator !=(GroupAffinity a1, GroupAffinity a2)
	{
		if (a1.Group == a2.Group)
		{
			return a1.Mask != a2.Mask;
		}
		return true;
	}
}
