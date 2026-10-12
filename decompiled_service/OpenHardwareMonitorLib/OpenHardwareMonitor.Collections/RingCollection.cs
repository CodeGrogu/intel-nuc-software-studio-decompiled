using System;
using System.Collections;
using System.Collections.Generic;

namespace OpenHardwareMonitor.Collections;

public class RingCollection<T> : IEnumerable<T>, IEnumerable
{
	private struct Enumerator(RingCollection<T> collection) : IEnumerator<T>, IDisposable, IEnumerator
	{
		private RingCollection<T> collection = collection;

		private int index = -1;

		public T Current
		{
			get
			{
				if (index < 0)
				{
					throw new InvalidOperationException();
				}
				return collection[index];
			}
		}

		object IEnumerator.Current
		{
			get
			{
				if (index < 0)
				{
					throw new InvalidOperationException();
				}
				return collection[index];
			}
		}

		public void Dispose()
		{
			index = -2;
		}

		public void Reset()
		{
			index = -1;
		}

		public bool MoveNext()
		{
			if (index == -2)
			{
				return false;
			}
			index++;
			if (index == collection.size)
			{
				index = -2;
				return false;
			}
			return true;
		}
	}

	private T[] array;

	private int head;

	private int tail;

	private int size;

	public int Capacity
	{
		get
		{
			return array.Length;
		}
		set
		{
			T[] destinationArray = new T[value];
			if (size > 0)
			{
				if (head < tail)
				{
					Array.Copy(array, head, destinationArray, 0, size);
				}
				else
				{
					Array.Copy(array, head, destinationArray, 0, array.Length - head);
					Array.Copy(array, 0, destinationArray, array.Length - head, tail);
				}
			}
			array = destinationArray;
			head = 0;
			tail = ((size != value) ? size : 0);
		}
	}

	public int Count => size;

	public T this[int index]
	{
		get
		{
			if (index < 0 || index >= size)
			{
				throw new IndexOutOfRangeException();
			}
			int num = head + index;
			if (num >= array.Length)
			{
				num -= array.Length;
			}
			return array[num];
		}
		set
		{
			if (index < 0 || index >= size)
			{
				throw new IndexOutOfRangeException();
			}
			int num = head + index;
			if (num >= array.Length)
			{
				num -= array.Length;
			}
			array[num] = value;
		}
	}

	public T First
	{
		get
		{
			if (size == 0)
			{
				throw new InvalidOperationException();
			}
			return array[head];
		}
		set
		{
			if (size == 0)
			{
				throw new InvalidOperationException();
			}
			array[head] = value;
		}
	}

	public T Last
	{
		get
		{
			if (size == 0)
			{
				throw new InvalidOperationException();
			}
			return array[(tail == 0) ? (array.Length - 1) : (tail - 1)];
		}
		set
		{
			if (size == 0)
			{
				throw new InvalidOperationException();
			}
			array[(tail == 0) ? (array.Length - 1) : (tail - 1)] = value;
		}
	}

	public RingCollection()
		: this(0)
	{
	}

	public RingCollection(int capacity)
	{
		if (capacity < 0)
		{
			throw new ArgumentOutOfRangeException("capacity");
		}
		array = new T[capacity];
		head = 0;
		tail = 0;
		size = 0;
	}

	public void Clear()
	{
		if (head < tail)
		{
			Array.Clear(array, head, size);
		}
		else
		{
			Array.Clear(array, 0, tail);
			Array.Clear(array, head, array.Length - head);
		}
		head = 0;
		tail = 0;
		size = 0;
	}

	public void Append(T item)
	{
		if (size == array.Length)
		{
			int num = array.Length * 3 / 2;
			if (num < array.Length + 8)
			{
				num = array.Length + 8;
			}
			Capacity = num;
		}
		array[tail] = item;
		tail = ((tail + 1 != array.Length) ? (tail + 1) : 0);
		size++;
	}

	public T Remove()
	{
		if (size == 0)
		{
			throw new InvalidOperationException();
		}
		T result = array[head];
		array[head] = default;
		head = ((head + 1 != array.Length) ? (head + 1) : 0);
		size--;
		return result;
	}

	IEnumerator<T> IEnumerable<T>.GetEnumerator()
	{
		return new Enumerator(this);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return new Enumerator(this);
	}
}
