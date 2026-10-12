using System;
using SerialDeviceAccessLibrary;

namespace NucSoftwareStudioService.ENE;

public static class EneWrapper
{
	private static string ENE_VID = "067B";

	private static string ENE_PID = "23A3";

	private const string ResetCommand = "RESET_ALL_PATTERN";

	private const string EnableCommand = "ENABLE_ALL";

	private const string DisableCommand = "DISABLE_ALL";

	private const string DelayCommand = "COMMAND_DELAY";

	private const string DisconnectCommand = "DISCONNECT";

	public static bool Connected { get; set; }

	public static string SerialPortError { get; set; }

	public static void Connect()
	{
		SerialDeviceAccess.SetDeviceInfo(ENE_VID, ENE_PID);
		try
		{
			Connected = SerialDeviceAccess.ConnectToDevice();
			SerialPortError = SerialDeviceAccess.SerialPortError;
		}
		catch (Exception)
		{
		}
	}

	public static void EneCall(string CommandName)
	{
		if (!Connected)
		{
			Connect();
		}
		ParseCommandName(CommandName);
	}

	private static void ParseCommandName(string CommandName)
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

	internal static byte[] LedConnectionStatus()
	{
		try
		{
			if (!Connected)
			{
				Connect();
			}
			return SerialDeviceAccess.GetLedConnectionStatus();
		}
		catch (Exception)
		{
			return new byte[4];
		}
	}
}
