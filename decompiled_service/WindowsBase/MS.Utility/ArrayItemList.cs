using System.Collections;
using System.Collections.Generic;

namespace MS.Utility;

internal sealed class ArrayItemList<T> : FrugalListBase<T>
{
	internal class ArrayCompacter : Compacter
	{
		public ArrayCompacter(ArrayItemList<T> store, int newCount)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		protected override void IncludeOverride(int start, int end)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		public override FrugalListBase<T> Finish()
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public override int Capacity
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public ArrayItemList()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public ArrayItemList(int size)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public ArrayItemList(ICollection collection)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public ArrayItemList(ICollection<T> collection)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override FrugalListStoreState Add(T value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override void Clear()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override bool Contains(T value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override int IndexOf(T value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override void Insert(int index, T value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override void SetAt(int index, T value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override bool Remove(T value)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override void RemoveAt(int index)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override T EntryAt(int index)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override void Promote(FrugalListBase<T> oldList)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public void Promote(SixItemList<T> oldList)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public void Promote(ArrayItemList<T> oldList)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override T[] ToArray()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override void CopyTo(T[] array, int index)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override object Clone()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override Compacter NewCompacter(int newCount)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
