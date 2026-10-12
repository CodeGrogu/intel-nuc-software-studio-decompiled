using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;

namespace OpenHardwareMonitor.Hardware.TBalancer;

internal class TBalancerGroup : IGroup
{
	private readonly List<TBalancer> hardware = new List<TBalancer>();

	private readonly StringBuilder report = new StringBuilder();

	public IHardware[] Hardware => hardware.ToArray();

	public TBalancerGroup(ISettings settings)
	{
		uint numDevices;
		try
		{
			if (FTD2XX.FT_CreateDeviceInfoList(out numDevices) != FT_STATUS.FT_OK)
			{
				report.AppendLine("Status: FT_CreateDeviceInfoList failed");
				return;
			}
		}
		catch (DllNotFoundException)
		{
			return;
		}
		catch (ArgumentNullException)
		{
			return;
		}
		catch (EntryPointNotFoundException)
		{
			return;
		}
		catch (BadImageFormatException)
		{
			return;
		}
		FT_DEVICE_INFO_NODE[] array = new FT_DEVICE_INFO_NODE[numDevices];
		if (FTD2XX.FT_GetDeviceInfoList(array, ref numDevices) != FT_STATUS.FT_OK)
		{
			report.AppendLine("Status: FT_GetDeviceInfoList failed");
			return;
		}
		if (numDevices > array.Length)
		{
			numDevices = (uint)array.Length;
		}
		for (int i = 0; i < numDevices; i++)
		{
			report.Append("Device Index: ");
			report.AppendLine(i.ToString(CultureInfo.InvariantCulture));
			report.Append("Device Type: ");
			report.AppendLine(array[i].Type.ToString());
			if (array[i].Type != FT_DEVICE.FT_DEVICE_232BM)
			{
				report.AppendLine("Status: Wrong device type");
				continue;
			}
			FT_STATUS fT_STATUS = FTD2XX.FT_Open(i, out var handle);
			if (fT_STATUS != FT_STATUS.FT_OK)
			{
				report.AppendLine("Open Status: " + fT_STATUS);
				continue;
			}
			FTD2XX.FT_SetBaudRate(handle, 19200u);
			FTD2XX.FT_SetDataCharacteristics(handle, 8, 1, 0);
			FTD2XX.FT_SetFlowControl(handle, FT_FLOW_CONTROL.FT_FLOW_RTS_CTS, 17, 19);
			FTD2XX.FT_SetTimeouts(handle, 1000u, 1000u);
			FTD2XX.FT_Purge(handle, FT_PURGE.FT_PURGE_ALL);
			fT_STATUS = FTD2XX.Write(handle, new byte[1] { 56 });
			if (fT_STATUS != FT_STATUS.FT_OK)
			{
				report.AppendLine("Write Status: " + fT_STATUS);
				FTD2XX.FT_Close(handle);
				continue;
			}
			bool flag = false;
			byte protocolVersion = 0;
			int num = 0;
			while (FTD2XX.BytesToRead(handle) == 0 && num < 2)
			{
				Thread.Sleep(100);
				num++;
			}
			if (FTD2XX.BytesToRead(handle) > 0)
			{
				if (FTD2XX.ReadByte(handle) == 100)
				{
					while (FTD2XX.BytesToRead(handle) < 284 && num < 5)
					{
						Thread.Sleep(100);
						num++;
					}
					int num2 = FTD2XX.BytesToRead(handle);
					if (num2 >= 284)
					{
						byte[] array2 = new byte[285];
						array2[0] = 100;
						for (int j = 1; j < array2.Length; j++)
						{
							array2[j] = FTD2XX.ReadByte(handle);
						}
						flag = (array2[274] & 0xF0) == 32;
						protocolVersion = array2[274];
						if (!flag)
						{
							report.Append("Status: Wrong Protocol Version: 0x");
							report.AppendLine(protocolVersion.ToString("X", CultureInfo.InvariantCulture));
						}
					}
					else
					{
						report.AppendLine("Status: Wrong Message Length: " + num2);
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
			FTD2XX.FT_Purge(handle, FT_PURGE.FT_PURGE_ALL);
			FTD2XX.FT_Close(handle);
			if (flag)
			{
				report.AppendLine("Status: OK");
				hardware.Add(new TBalancer(i, protocolVersion, settings));
			}
			if (i < numDevices - 1)
			{
				report.AppendLine();
			}
		}
	}

	public string GetReport()
	{
		if (report.Length > 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("FTD2XX");
			stringBuilder.AppendLine();
			stringBuilder.Append(report);
			stringBuilder.AppendLine();
			return stringBuilder.ToString();
		}
		return null;
	}

	public void Close()
	{
		foreach (TBalancer item in hardware)
		{
			item.Close();
		}
	}
}
