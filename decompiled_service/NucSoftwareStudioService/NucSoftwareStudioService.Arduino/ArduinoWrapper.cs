using System;
using SerialDeviceAccessLibrary;

namespace NucSoftwareStudioService.Arduino;

internal static class ArduinoWrapper
{
	private static string MONSTER_COVE_VID = "067B";

	private static string MONSTER_COVE_PID = "2323";

	public static bool Connected { get; set; }

	public static string SerialPortError { get; set; }

	public static void ArduinoCall(string CommandName)
	{
		if (!Connected)
		{
			Connect();
		}
		ParseCommandName(CommandName);
	}

	public static byte[] ParseResults(string results)
	{
		return new byte[0];
	}

	public static void ParseCommandName(string CommandName)
	{
		switch (CommandName)
		{
		case "RESET_ALL_PATTERN":
			SerialDeviceAccess.ResetAllPattern();
			break;
		case "ENABLE_ALL":
			SerialDeviceAccess.EnableAll();
			break;
		case "DISABLE_ALL":
			SerialDeviceAccess.DisableAll();
			break;
		case "COMMAND_DELAY":
			SerialDeviceAccess.SendCommandDelayTime();
			break;
		case "DISCONNECT":
			Disconnect();
			break;
		}
	}

	public static void Connect()
	{
		SerialDeviceAccess.SetDeviceInfo(MONSTER_COVE_VID, MONSTER_COVE_PID);
		try
		{
			Connected = SerialDeviceAccess.ConnectToDevice();
			SerialPortError = SerialDeviceAccess.SerialPortError;
		}
		catch (Exception)
		{
		}
	}

	public static void Disconnect()
	{
		if (Connected)
		{
			try
			{
				Connected = false;
				SerialDeviceAccess.DisconnectFromDevice();
			}
			catch (Exception)
			{
			}
		}
	}
}
