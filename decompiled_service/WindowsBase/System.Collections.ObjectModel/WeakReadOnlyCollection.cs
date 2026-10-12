using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using MS.Internal.WindowsBase;

namespace System.Collections.ObjectModel;

[Serializable]
[ComVisible(false)]
[DebuggerDisplay("Count = {Count}")]
[FriendAccessAllowed]
internal class WeakReadOnlyCollection<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IList, ICollection
{
	public int Count
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public T this[int index]
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	bool ICollection<T>.IsReadOnly
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	T IList<T>.this[int index]
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	bool ICollection.IsSynchronized
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	object ICollection.SyncRoot
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	bool IList.IsFixedSize
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	bool IList.IsReadOnly
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	object IList.this[int index]
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public WeakReadOnlyCollection(IList<WeakReference> list)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public bool Contains(T value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public void CopyTo(T[] array, int index)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public IEnumerator<T> GetEnumerator()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public int IndexOf(T value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	void ICollection<T>.Add(T value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	void ICollection<T>.Clear()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	void IList<T>.Insert(int index, T value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	bool ICollection<T>.Remove(T value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	void IList<T>.RemoveAt(int index)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	void ICollection.CopyTo(Array array, int index)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	int IList.Add(object value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	void IList.Clear()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	bool IList.Contains(object value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	int IList.IndexOf(object value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	void IList.Insert(int index, object value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	void IList.Remove(object value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	void IList.RemoveAt(int index)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
