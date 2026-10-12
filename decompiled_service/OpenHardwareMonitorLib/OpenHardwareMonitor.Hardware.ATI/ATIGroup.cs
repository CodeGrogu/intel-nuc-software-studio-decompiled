using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace OpenHardwareMonitor.Hardware.ATI;

internal class ATIGroup : IGroup
{
	private readonly List<ATIGPU> hardware = new List<ATIGPU>();

	private readonly StringBuilder report = new StringBuilder();

	private IntPtr context = IntPtr.Zero;

	public IHardware[] Hardware => hardware.ToArray();

	public ATIGroup(ISettings settings)
	{
		try
		{
			ADLStatus aDLStatus = ADL.ADL_Main_Control_Create(1);
			ADLStatus aDLStatus2 = ADL.ADL2_Main_Control_Create(1, out context);
			report.AppendLine("AMD Display Library");
			report.AppendLine();
			report.Append("ADL Status: ");
			report.AppendLine(aDLStatus.ToString());
			report.Append("ADL2 Status: ");
			report.AppendLine(aDLStatus2.ToString());
			report.AppendLine();
			report.AppendLine("Graphics Versions");
			report.AppendLine();
			try
			{
				ADLStatus aDLStatus3 = ADL.ADL_Graphics_Versions_Get(out var versionInfo);
				report.Append(" Status: ");
				report.AppendLine(aDLStatus3.ToString());
				report.Append(" DriverVersion: ");
				report.AppendLine(versionInfo.DriverVersion);
				report.Append(" CatalystVersion: ");
				report.AppendLine(versionInfo.CatalystVersion);
				report.Append(" CatalystWebLink: ");
				report.AppendLine(versionInfo.CatalystWebLink);
			}
			catch (DllNotFoundException)
			{
				report.AppendLine(" Status: DLL not found");
			}
			catch (Exception ex2)
			{
				report.AppendLine(" Status: " + ex2.Message);
			}
			report.AppendLine();
			if (aDLStatus != ADLStatus.OK)
			{
				return;
			}
			int numAdapters = 0;
			ADL.ADL_Adapter_NumberOfAdapters_Get(ref numAdapters);
			report.Append("Number of adapters: ");
			report.AppendLine(numAdapters.ToString(CultureInfo.InvariantCulture));
			report.AppendLine();
			if (numAdapters <= 0)
			{
				return;
			}
			ADLAdapterInfo[] array = new ADLAdapterInfo[numAdapters];
			if (ADL.ADL_Adapter_AdapterInfo_Get(array) != ADLStatus.OK)
			{
				return;
			}
			for (int i = 0; i < numAdapters; i++)
			{
				ADL.ADL_Adapter_Active_Get(array[i].AdapterIndex, out var status);
				ADL.ADL_Adapter_ID_Get(array[i].AdapterIndex, out var adapterID);
				report.Append("AdapterIndex: ");
				report.AppendLine(i.ToString(CultureInfo.InvariantCulture));
				report.Append("isActive: ");
				report.AppendLine(status.ToString(CultureInfo.InvariantCulture));
				report.Append("AdapterName: ");
				report.AppendLine(array[i].AdapterName);
				report.Append("UDID: ");
				report.AppendLine(array[i].UDID);
				report.Append("Present: ");
				report.AppendLine(array[i].Present.ToString(CultureInfo.InvariantCulture));
				report.Append("VendorID: 0x");
				report.AppendLine(array[i].VendorID.ToString("X", CultureInfo.InvariantCulture));
				report.Append("BusNumber: ");
				report.AppendLine(array[i].BusNumber.ToString(CultureInfo.InvariantCulture));
				report.Append("DeviceNumber: ");
				report.AppendLine(array[i].DeviceNumber.ToString(CultureInfo.InvariantCulture));
				report.Append("FunctionNumber: ");
				report.AppendLine(array[i].FunctionNumber.ToString(CultureInfo.InvariantCulture));
				report.Append("AdapterID: 0x");
				report.AppendLine(adapterID.ToString("X", CultureInfo.InvariantCulture));
				if (!string.IsNullOrEmpty(array[i].UDID) && array[i].VendorID == 4098)
				{
					bool flag = false;
					foreach (ATIGPU item in hardware)
					{
						if (item.BusNumber == array[i].BusNumber && item.DeviceNumber == array[i].DeviceNumber)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						StringBuilder stringBuilder = new StringBuilder(array[i].AdapterName);
						stringBuilder.Replace("(TM)", " ");
						for (int j = 0; j < 10; j++)
						{
							stringBuilder.Replace("  ", " ");
						}
						string name = stringBuilder.ToString().Trim();
						hardware.Add(new ATIGPU(name, array[i].AdapterIndex, array[i].BusNumber, array[i].DeviceNumber, context, settings));
					}
				}
				report.AppendLine();
			}
		}
		catch (DllNotFoundException)
		{
		}
		catch (EntryPointNotFoundException ex4)
		{
			report.AppendLine();
			report.AppendLine(ex4.ToString());
			report.AppendLine();
		}
	}

	public string GetReport()
	{
		return report.ToString();
	}

	public void Close()
	{
		try
		{
			foreach (ATIGPU item in hardware)
			{
				item.Close();
			}
			if (context != IntPtr.Zero)
			{
				ADL.ADL2_Main_Control_Destroy(context);
				context = IntPtr.Zero;
			}
			ADL.ADL_Main_Control_Destroy();
		}
		catch (Exception)
		{
		}
	}
}
