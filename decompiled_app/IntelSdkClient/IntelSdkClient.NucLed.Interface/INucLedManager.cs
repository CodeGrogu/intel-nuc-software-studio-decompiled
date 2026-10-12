namespace IntelSdkClient.NucLed.Interface;

public interface INucLedManager
{
	byte[] Wmi(string CommandName, byte[] Parameters);
}
