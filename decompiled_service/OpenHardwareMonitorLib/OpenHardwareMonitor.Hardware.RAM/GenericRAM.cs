using System.Runtime.InteropServices;

namespace OpenHardwareMonitor.Hardware.RAM;

internal class GenericRAM : Hardware
{
	private class NativeMethods
	{
		public struct MemoryStatusEx
		{
			public uint Length;

			public uint MemoryLoad;

			public ulong TotalPhysicalMemory;

			public ulong AvailablePhysicalMemory;

			public ulong TotalPageFile;

			public ulong AvailPageFile;

			public ulong TotalVirtual;

			public ulong AvailVirtual;

			public ulong AvailExtendedVirtual;
		}

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool GlobalMemoryStatusEx(ref MemoryStatusEx buffer);
	}

	private Sensor loadSensor;

	private Sensor usedMemory;

	private Sensor availableMemory;

	public override HardwareType HardwareType => HardwareType.RAM;

	public GenericRAM(string name, ISettings settings)
		: base(name, new Identifier("ram"), settings)
	{
		loadSensor = new Sensor("Memory", 0, SensorType.Load, this, settings);
		ActivateSensor(loadSensor);
		usedMemory = new Sensor("Used Memory", 0, SensorType.Data, this, settings);
		ActivateSensor(usedMemory);
		availableMemory = new Sensor("Available Memory", 1, SensorType.Data, this, settings);
		ActivateSensor(availableMemory);
	}

	public override void Update()
	{
		NativeMethods.MemoryStatusEx buffer = new NativeMethods.MemoryStatusEx
		{
			Length = checked((uint)Marshal.SizeOf(typeof(NativeMethods.MemoryStatusEx)))
		};
		if (NativeMethods.GlobalMemoryStatusEx(ref buffer))
		{
			loadSensor.Value = 100f - 100f * (float)buffer.AvailablePhysicalMemory / (float)buffer.TotalPhysicalMemory;
			usedMemory.Value = (float)(buffer.TotalPhysicalMemory - buffer.AvailablePhysicalMemory) / 1.0737418E+09f;
			availableMemory.Value = (float)buffer.AvailablePhysicalMemory / 1.0737418E+09f;
		}
	}
}
