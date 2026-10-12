using System;
using System.Runtime.InteropServices;
using System.Security;

namespace MS.Internal.Interop;

[StructLayout(LayoutKind.Explicit)]
[SecurityCritical]
internal class PROPVARIANT : IDisposable
{
	public VarEnum VarType
	{
		[SecurityCritical]
		[SecurityTreatAsSafe]
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public string GetValue()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public void SetValue(bool f)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public void SetValue(string val)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public void Clear()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public void Dispose()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	~PROPVARIANT()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public PROPVARIANT()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
