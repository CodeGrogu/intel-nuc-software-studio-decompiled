namespace OpenHardwareMonitor.Collections;

public struct Pair<F, S>(F first, S second)
{
	private F first = first;

	private S second = second;

	public F First
	{
		get
		{
			return first;
		}
		set
		{
			first = value;
		}
	}

	public S Second
	{
		get
		{
			return second;
		}
		set
		{
			second = value;
		}
	}

	public override int GetHashCode()
	{
		return ((first != null) ? first.GetHashCode() : 0) ^ ((second != null) ? second.GetHashCode() : 0);
	}
}
