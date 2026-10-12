using System;
using System.Runtime.InteropServices;
using System.Security;

namespace MS.Internal.Interop;

[StructLayout(LayoutKind.Explicit)]
internal struct HRESULT
{
	[FieldOffset(0)]
	private readonly uint _value;

	public static readonly HRESULT S_OK;

	public static readonly HRESULT S_FALSE;

	public static readonly HRESULT E_NOTIMPL;

	public static readonly HRESULT E_NOINTERFACE;

	public static readonly HRESULT E_POINTER;

	public static readonly HRESULT E_ABORT;

	public static readonly HRESULT E_FAIL;

	public static readonly HRESULT E_UNEXPECTED;

	public static readonly HRESULT DISP_E_MEMBERNOTFOUND;

	public static readonly HRESULT DISP_E_TYPEMISMATCH;

	public static readonly HRESULT DISP_E_UNKNOWNNAME;

	public static readonly HRESULT DISP_E_EXCEPTION;

	public static readonly HRESULT DISP_E_OVERFLOW;

	public static readonly HRESULT DISP_E_BADINDEX;

	public static readonly HRESULT DISP_E_BADPARAMCOUNT;

	public static readonly HRESULT DISP_E_PARAMNOTOPTIONAL;

	public static readonly HRESULT SCRIPT_E_REPORTED;

	public static readonly HRESULT STG_E_INVALIDFUNCTION;

	public static readonly HRESULT DESTS_E_NO_MATCHING_ASSOC_HANDLER;

	public static readonly HRESULT E_ACCESSDENIED;

	public static readonly HRESULT E_OUTOFMEMORY;

	public static readonly HRESULT E_INVALIDARG;

	public static readonly HRESULT COR_E_OBJECTDISPOSED;

	public static readonly HRESULT WC_E_GREATERTHAN;

	public static readonly HRESULT WC_E_SYNTAX;

	public Facility Facility
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public int Code
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public bool Succeeded
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public bool Failed
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public HRESULT(uint i)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public static HRESULT Make(bool severe, Facility facility, int code)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public static Facility GetFacility(int errorCode)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public static int GetCode(int error)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override string ToString()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override bool Equals(object obj)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public override int GetHashCode()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public static bool operator ==(HRESULT hrLeft, HRESULT hrRight)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public static bool operator !=(HRESULT hrLeft, HRESULT hrRight)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public void ThrowIfFailed()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public void ThrowIfFailed(string message)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public Exception GetException()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	public Exception GetException(string message)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static HRESULT()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
