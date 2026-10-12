using System.Collections;
using MS.Internal.WindowsBase;

namespace MS.Internal;

[FriendAccessAllowed]
internal class CopyOnWriteList
{
	public ArrayList List
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	protected object SyncRoot
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	protected ArrayList LiveList
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public CopyOnWriteList()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public CopyOnWriteList(object syncRoot)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public virtual bool Add(object obj)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public virtual bool Remove(object obj)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	protected bool Internal_Add(object obj)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	protected bool Internal_Insert(int index, object obj)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	protected bool RemoveAt(int index)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	protected void DoCopyOnWriteCheck()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
