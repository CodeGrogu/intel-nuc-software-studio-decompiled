using System.Collections;
using System.Collections.Generic;

namespace OpenHardwareMonitor.Collections;

public class ReadOnlyArray<T> : IReadOnlyArray<T>, IEnumerable<T>, IEnumerable
{
	private readonly T[] array;

	public T this[int index] => array[index];

	public int Length => array.Length;

	public ReadOnlyArray(T[] array)
	{
		this.array = array;
	}

	public IEnumerator<T> GetEnumerator()
	{
		return ((IEnumerable<T>)array).GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return array.GetEnumerator();
	}

	public static implicit operator ReadOnlyArray<T>(T[] array)
	{
		return new ReadOnlyArray<T>(array);
	}

	public T[] ToArray()
	{
		return (T[])array.Clone();
	}
}
