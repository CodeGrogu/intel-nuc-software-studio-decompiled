using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Ports;
using System.Security;
using System.Text;
using System.Threading;
using Microsoft.Win32;

namespace OpenHardwareMonitor.Hardware.Heatmaster;

internal class HeatmasterGroup : IGroup
{
	private readonly List<Heatmaster> hardware = new List<Heatmaster>();

	private readonly StringBuilder report = new StringBuilder();

	public IHardware[] Hardware => hardware.ToArray();

	private static string ReadLine(SerialPort port, int timeout)
	{
		int num = 0;
		StringBuilder stringBuilder = new StringBuilder();
		while (num < timeout)
		{
			while (port.BytesToRead > 0)
			{
				byte b = (byte)port.ReadByte();
				switch (b)
				{
				case 170:
				{
					char c = (char)b;
					return c.ToString();
				}
				case 13:
					return stringBuilder.ToString();
				}
				stringBuilder.Append((char)b);
			}
			num++;
			Thread.Sleep(1);
		}
		throw new TimeoutException();
	}

	private static string[] GetRegistryPortNames()
	{
		List<string> list = new List<string>();
		string[] array = new string[2] { "", "&MI_00" };
		try
		{
			string[] array2 = array;
			foreach (string text in array2)
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Enum\\USB\\VID_10C4&PID_EA60" + text);
				if (registryKey == null)
				{
					continue;
				}
				string[] subKeyNames = registryKey.GetSubKeyNames();
				foreach (string text2 in subKeyNames)
				{
					RegistryKey registryKey2 = registryKey.OpenSubKey(text2 + "\\Device Parameters");
					if (registryKey2 != null && registryKey2.GetValue("PortName") is string item && !list.Contains(item))
					{
						list.Add(item);
					}
				}
			}
		}
		catch (SecurityException)
		{
		}
		return list.ToArray();
	}

	public HeatmasterGroup(ISettings settings)
	{
		if (OperatingSystem.IsUnix)
		{
			return;
		}
		string[] registryPortNames = GetRegistryPortNames();
		for (int i = 0; i < registryPortNames.Length; i++)
		{
			bool flag = false;
			try
			{
				using SerialPort serialPort = new SerialPort(registryPortNames[i], 38400, Parity.None, 8, StopBits.One);
				serialPort.NewLine = '\r'.ToString();
				report.Append("Port Name: ");
				report.AppendLine(registryPortNames[i]);
				try
				{
					serialPort.Open();
				}
				catch (UnauthorizedAccessException)
				{
					report.AppendLine("Exception: Access Denied");
				}
				if (serialPort.IsOpen)
				{
					serialPort.DiscardInBuffer();
					serialPort.DiscardOutBuffer();
					serialPort.Write(new byte[1] { 170 }, 0, 1);
					int num = 0;
					while (serialPort.BytesToRead == 0 && num < 10)
					{
						Thread.Sleep(20);
						num++;
					}
					if (serialPort.BytesToRead > 0)
					{
						bool flag2 = false;
						while (serialPort.BytesToRead > 0 && !flag2)
						{
							flag2 |= serialPort.ReadByte() == 170;
						}
						if (flag2)
						{
							serialPort.WriteLine("[0:0]RH");
							try
							{
								int j = 0;
								int num2 = 0;
								for (; j < 5; j++)
								{
									string text = ReadLine(serialPort, 100);
									if (text.StartsWith("-[0:0]RH:", StringComparison.Ordinal))
									{
										num2 = int.Parse(text.Substring(9), CultureInfo.InvariantCulture);
										break;
									}
								}
								flag = num2 == 770;
								if (!flag)
								{
									report.Append("Status: Wrong Hardware Revision " + num2.ToString(CultureInfo.InvariantCulture));
								}
							}
							catch (TimeoutException)
							{
								report.AppendLine("Status: Timeout Reading Revision");
							}
						}
						else
						{
							report.AppendLine("Status: Wrong Startflag");
						}
					}
					else
					{
						report.AppendLine("Status: No Response");
					}
					serialPort.DiscardInBuffer();
				}
				else
				{
					report.AppendLine("Status: Port not Open");
				}
			}
			catch (Exception ex3)
			{
				report.AppendLine(ex3.ToString());
			}
			if (flag)
			{
				report.AppendLine("Status: OK");
				hardware.Add(new Heatmaster(registryPortNames[i], settings));
			}
			report.AppendLine();
		}
	}

	public string GetReport()
	{
		if (report.Length > 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Serial Port Heatmaster");
			stringBuilder.AppendLine();
			stringBuilder.Append(report);
			stringBuilder.AppendLine();
			return stringBuilder.ToString();
		}
		return null;
	}

	public void Close()
	{
		foreach (Heatmaster item in hardware)
		{
			item.Close();
		}
	}
}
