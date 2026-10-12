using System;

namespace OpenHardwareMonitor.Hardware.HDD;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
internal class NamePrefixAttribute : Attribute
{
	public string Prefix { get; private set; }

	public NamePrefixAttribute(string namePrefix)
	{
		Prefix = namePrefix;
	}
}
