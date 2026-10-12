namespace HidDeviceAccessLibrary;

public class FwVersion
{
	public uint HighVer { get; }

	public uint LowVer { get; }

	public uint TestVer { get; }

	public uint CustomVer { get; }

	public FwVersion(byte[] msg)
	{
		HighVer = msg[2];
		LowVer = msg[3];
		TestVer = msg[4];
		CustomVer = msg[5];
	}
}
