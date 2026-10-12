using System.Security;
using MS.Internal.WindowsBase;

namespace System.Windows.Interop;

/// <summary>Contains message information from a thread's message queue. </summary>
[Serializable]
public struct MSG
{
	[SecurityCritical]
	private IntPtr _hwnd;

	[SecurityCritical]
	private int _message;

	[SecurityCritical]
	private IntPtr _wParam;

	[SecurityCritical]
	private IntPtr _lParam;

	[SecurityCritical]
	private int _time;

	[SecurityCritical]
	private int _pt_x;

	[SecurityCritical]
	private int _pt_y;

	/// <summary>Gets or sets the window handle (HWND) to the window whose window procedure receives the message. </summary>
	/// <returns>The window handle (HWND).</returns>
	public IntPtr hwnd
	{
		[SecurityCritical]
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		[SecurityCritical]
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets or sets the message identifier. </summary>
	/// <returns>The message identifier.</returns>
	public int message
	{
		[SecurityCritical]
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		[SecurityCritical]
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets or sets the <paramref name="wParam" /> value for the message, which specifies additional information about the message. The exact meaning depends on the value of the message. </summary>
	/// <returns>The <paramref name="wParam" /> value for the message.</returns>
	public IntPtr wParam
	{
		[SecurityCritical]
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		[SecurityCritical]
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets or sets the <paramref name="lParam" /> value that specifies additional information about the message. The exact meaning depends on the value of the <see cref="P:System.Windows.Interop.MSG.message" /> member.</summary>
	/// <returns>The <paramref name="lParam" /> value for the message.</returns>
	public IntPtr lParam
	{
		[SecurityCritical]
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		[SecurityCritical]
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets or sets the time at which the message was posted.</summary>
	/// <returns>The time that the message was posted.</returns>
	public int time
	{
		[SecurityCritical]
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		[SecurityCritical]
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets or sets the x coordinate of the cursor position on the screen, when the message was posted. </summary>
	/// <returns>The x coordinate of the cursor position.</returns>
	public int pt_x
	{
		[SecurityCritical]
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		[SecurityCritical]
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	/// <summary>Gets or sets the y coordinate of the cursor position on the screen, when the message was posted. </summary>
	/// <returns>The y coordinate of the cursor position.</returns>
	public int pt_y
	{
		[SecurityCritical]
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		[SecurityCritical]
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	[SecurityCritical]
	[FriendAccessAllowed]
	internal MSG(IntPtr hwnd, int message, IntPtr wParam, IntPtr lParam, int time, int pt_x, int pt_y)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
