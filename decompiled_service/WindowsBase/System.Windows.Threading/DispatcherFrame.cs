using System.Security;

namespace System.Windows.Threading;

/// <summary>Represents an execution loop in the <see cref="T:System.Windows.Threading.Dispatcher" />. </summary>
public class DispatcherFrame : DispatcherObject
{
	/// <summary>Gets or sets a value that indicates whether this <see cref="T:System.Windows.Threading.DispatcherFrame" /> should continue.</summary>
	/// <returns>
	///     <see langword="true" /> if the frame should continue; otherwise, <see langword="false" />.  The default value is <see langword="true" />.</returns>
	public bool Continue
	{
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
	[SecurityTreatAsSafe]
	static DispatcherFrame()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Threading.DispatcherFrame" /> class.</summary>
	public DispatcherFrame()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Threading.DispatcherFrame" /> class, by using the specified exit request flag.</summary>
	/// <param name="exitWhenRequested">Indicates whether this frame will exit when all frames are requested to exit.</param>
	public DispatcherFrame(bool exitWhenRequested)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
