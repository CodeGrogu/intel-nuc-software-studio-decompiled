using System.Runtime.InteropServices;

namespace OpenHardwareMonitor.Hardware;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct IOControlCode
{
	public enum Method : uint
	{
		Buffered,
		InDirect,
		OutDirect,
		Neither
	}

	public enum Access : uint
	{
		Any,
		Read,
		Write
	}

	private uint code;

	public IOControlCode(uint deviceType, uint function, Access access)
		: this(deviceType, function, Method.Buffered, access)
	{
	}

	public IOControlCode(uint deviceType, uint function, Method method, Access access)
	{
		code = (deviceType << 16) | ((uint)access << 14) | (function << 2) | (uint)method;
	}
}
