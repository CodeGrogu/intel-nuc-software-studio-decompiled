using System;
using System.Runtime.CompilerServices;
using System.Security;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.WindowsBase;

namespace MS.Win32;

[MS.Internal.WindowsBase.FriendAccessAllowed]
internal class HwndWrapper : DispatcherObject, IDisposable
{
	internal class DestroyWindowArgs
	{
		public SecurityCriticalDataClass<IntPtr> Handle
		{
			get
			{
				/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
			}
		}

		public ushort ClassAtom
		{
			get
			{
				/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
			}
		}

		public DestroyWindowArgs(SecurityCriticalDataClass<IntPtr> handle, ushort classAtom)
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public IntPtr Handle
	{
		[SecurityCritical]
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public event EventHandler Disposed
	{
		[CompilerGenerated]
		add
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		[CompilerGenerated]
		remove
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	static HwndWrapper()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	public HwndWrapper(int classStyle, int style, int exStyle, int x, int y, int width, int height, string name, IntPtr parent, HwndWrapperHook[] hooks)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	~HwndWrapper()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public virtual void Dispose()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	public void AddHook(HwndWrapperHook hook)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal void AddHookLast(HwndWrapperHook hook)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	public void RemoveHook(HwndWrapperHook hook)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal static object DestroyWindow(object args)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	internal static object UnregisterClass(object arg)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
