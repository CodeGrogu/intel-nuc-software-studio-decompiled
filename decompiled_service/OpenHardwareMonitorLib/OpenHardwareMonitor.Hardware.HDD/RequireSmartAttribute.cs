using System;

namespace OpenHardwareMonitor.Hardware.HDD;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
internal class RequireSmartAttribute : Attribute
{
	public byte AttributeId { get; private set; }

	public RequireSmartAttribute(byte attributeId)
	{
		AttributeId = attributeId;
	}
}
