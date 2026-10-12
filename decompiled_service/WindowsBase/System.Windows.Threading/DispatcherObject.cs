using System.ComponentModel;
using MS.Internal.WindowsBase;

namespace System.Windows.Threading;

/// <summary>Represents an object that is associated with a <see cref="T:System.Windows.Threading.Dispatcher" />. </summary>
public abstract class DispatcherObject
{
	/// <summary>Gets the <see cref="T:System.Windows.Threading.Dispatcher" /> this <see cref="T:System.Windows.Threading.DispatcherObject" /> is associated with. </summary>
	/// <returns>The dispatcher.</returns>
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public Dispatcher Dispatcher
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	[FriendAccessAllowed]
	internal void DetachFromDispatcher()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Determines whether the calling thread has access to this <see cref="T:System.Windows.Threading.DispatcherObject" />.</summary>
	/// <returns>
	///     <see langword="true" /> if the calling thread has access to this object; otherwise, <see langword="false" />.</returns>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool CheckAccess()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Enforces that the calling thread has access to this <see cref="T:System.Windows.Threading.DispatcherObject" />.</summary>
	/// <exception cref="T:System.InvalidOperationException">the calling thread does not have access to this <see cref="T:System.Windows.Threading.DispatcherObject" />.</exception>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void VerifyAccess()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Threading.DispatcherObject" /> class. </summary>
	protected DispatcherObject()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
