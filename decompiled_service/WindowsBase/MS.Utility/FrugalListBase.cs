namespace MS.Utility;

internal abstract class FrugalListBase<T>
{
	internal class Compacter
	{
		protected FrugalListBase<T> _store;

		protected int _validItemCount;

		protected int _previousEnd;

		public Compacter(FrugalListBase<T> store, int newCount)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		public void Include(int start, int end)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		protected virtual void IncludeOverride(int start, int end)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}

		public virtual FrugalListBase<T> Finish()
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	protected int _count;

	public int Count
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public abstract int Capacity { get; }

	internal void TrustedSetCount(int newCount)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public abstract FrugalListStoreState Add(T value);

	public abstract void Clear();

	public abstract bool Contains(T value);

	public abstract int IndexOf(T value);

	public abstract void Insert(int index, T value);

	public abstract void SetAt(int index, T value);

	public abstract bool Remove(T value);

	public abstract void RemoveAt(int index);

	public abstract T EntryAt(int index);

	public abstract void Promote(FrugalListBase<T> newList);

	public abstract T[] ToArray();

	public abstract void CopyTo(T[] array, int index);

	public abstract object Clone();

	public virtual Compacter NewCompacter(int newCount)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	protected FrugalListBase()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
