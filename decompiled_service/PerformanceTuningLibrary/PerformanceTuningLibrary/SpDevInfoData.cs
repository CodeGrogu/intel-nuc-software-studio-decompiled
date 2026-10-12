using System;

namespace PerformanceTuningLibrary;

internal struct SpDevInfoData
{
	public uint cbSize;

	public Guid ClassGuid;

	public uint DevInst;

	public IntPtr Reserved;
}
