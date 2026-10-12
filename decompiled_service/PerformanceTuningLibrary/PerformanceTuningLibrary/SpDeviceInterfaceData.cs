using System;

namespace PerformanceTuningLibrary;

internal struct SpDeviceInterfaceData
{
	public uint cbSize;

	public Guid InterfaceClassGuid;

	public uint Flags;

	public IntPtr Reserved;
}
