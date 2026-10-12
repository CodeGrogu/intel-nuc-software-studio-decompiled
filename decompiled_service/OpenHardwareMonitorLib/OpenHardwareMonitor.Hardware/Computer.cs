using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Permissions;
using OpenHardwareMonitor.Hardware.ATI;
using OpenHardwareMonitor.Hardware.CPU;
using OpenHardwareMonitor.Hardware.HDD;
using OpenHardwareMonitor.Hardware.Heatmaster;
using OpenHardwareMonitor.Hardware.Mainboard;
using OpenHardwareMonitor.Hardware.Nvidia;
using OpenHardwareMonitor.Hardware.RAM;
using OpenHardwareMonitor.Hardware.TBalancer;

namespace OpenHardwareMonitor.Hardware;

public class Computer : IComputer, IElement
{
	private class Settings : ISettings
	{
		public bool Contains(string name)
		{
			return false;
		}

		public void SetValue(string name, string value)
		{
		}

		public string GetValue(string name, string value)
		{
			return value;
		}

		public void Remove(string name)
		{
		}
	}

	private readonly List<IGroup> groups = new List<IGroup>();

	private readonly ISettings settings;

	private SMBIOS smbios;

	private bool open;

	private bool mainboardEnabled;

	private bool cpuEnabled;

	private bool ramEnabled;

	private bool gpuEnabled;

	private bool fanControllerEnabled;

	private bool hddEnabled;

	public bool MainboardEnabled
	{
		get
		{
			return mainboardEnabled;
		}
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		set
		{
			if (open && value != mainboardEnabled)
			{
				if (value)
				{
					Add(new MainboardGroup(smbios, settings));
				}
				else
				{
					RemoveType<MainboardGroup>();
				}
			}
			mainboardEnabled = value;
		}
	}

	public bool CPUEnabled
	{
		get
		{
			return cpuEnabled;
		}
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		set
		{
			if (open && value != cpuEnabled)
			{
				if (value)
				{
					Add(new CPUGroup(settings));
				}
				else
				{
					RemoveType<CPUGroup>();
				}
			}
			cpuEnabled = value;
		}
	}

	public bool RAMEnabled
	{
		get
		{
			return ramEnabled;
		}
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		set
		{
			if (open && value != ramEnabled)
			{
				if (value)
				{
					Add(new RAMGroup(smbios, settings));
				}
				else
				{
					RemoveType<RAMGroup>();
				}
			}
			ramEnabled = value;
		}
	}

	public bool GPUEnabled
	{
		get
		{
			return gpuEnabled;
		}
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		set
		{
			if (open && value != gpuEnabled)
			{
				if (value)
				{
					Add(new ATIGroup(settings));
					Add(new NvidiaGroup(settings));
				}
				else
				{
					RemoveType<ATIGroup>();
					RemoveType<NvidiaGroup>();
				}
			}
			gpuEnabled = value;
		}
	}

	public bool FanControllerEnabled
	{
		get
		{
			return fanControllerEnabled;
		}
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		set
		{
			if (open && value != fanControllerEnabled)
			{
				if (value)
				{
					Add(new TBalancerGroup(settings));
					Add(new HeatmasterGroup(settings));
				}
				else
				{
					RemoveType<TBalancerGroup>();
					RemoveType<HeatmasterGroup>();
				}
			}
			fanControllerEnabled = value;
		}
	}

	public bool HDDEnabled
	{
		get
		{
			return hddEnabled;
		}
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		set
		{
			if (open && value != hddEnabled)
			{
				if (value)
				{
					Add(new HarddriveGroup(settings));
				}
				else
				{
					RemoveType<HarddriveGroup>();
				}
			}
			hddEnabled = value;
		}
	}

	public IHardware[] Hardware
	{
		get
		{
			List<IHardware> list = new List<IHardware>();
			foreach (IGroup group in groups)
			{
				IHardware[] hardware = group.Hardware;
				foreach (IHardware item in hardware)
				{
					list.Add(item);
				}
			}
			return list.ToArray();
		}
	}

	public event HardwareEventHandler HardwareAdded;

	public event HardwareEventHandler HardwareRemoved;

	public Computer()
	{
		settings = new Settings();
	}

	public Computer(ISettings settings)
	{
		this.settings = settings ?? new Settings();
	}

	private void Add(IGroup group)
	{
		if (groups.Contains(group))
		{
			return;
		}
		groups.Add(group);
		if (HardwareAdded != null)
		{
			IHardware[] hardware = group.Hardware;
			foreach (IHardware hardware2 in hardware)
			{
				HardwareAdded(hardware2);
			}
		}
	}

	private void Remove(IGroup group)
	{
		if (!groups.Contains(group))
		{
			return;
		}
		groups.Remove(group);
		if (HardwareRemoved != null)
		{
			IHardware[] hardware = group.Hardware;
			foreach (IHardware hardware2 in hardware)
			{
				HardwareRemoved(hardware2);
			}
		}
		group.Close();
	}

	private void RemoveType<T>() where T : IGroup
	{
		List<IGroup> list = new List<IGroup>();
		foreach (IGroup group in groups)
		{
			if (group is T)
			{
				list.Add(group);
			}
		}
		foreach (IGroup item in list)
		{
			Remove(item);
		}
	}

	[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
	public void Open()
	{
		if (!open)
		{
			smbios = new SMBIOS();
			Ring0.Open();
			Opcode.Open();
			AddGroups();
			open = true;
		}
	}

	private void AddGroups()
	{
		if (mainboardEnabled)
		{
			Add(new MainboardGroup(smbios, settings));
		}
		if (cpuEnabled)
		{
			Add(new CPUGroup(settings));
		}
		if (ramEnabled)
		{
			Add(new RAMGroup(smbios, settings));
		}
		if (gpuEnabled)
		{
			Add(new ATIGroup(settings));
			Add(new NvidiaGroup(settings));
		}
		if (fanControllerEnabled)
		{
			Add(new TBalancerGroup(settings));
			Add(new HeatmasterGroup(settings));
		}
		if (hddEnabled)
		{
			Add(new HarddriveGroup(settings));
		}
	}

	public void Reset()
	{
		if (open)
		{
			RemoveGroups();
			AddGroups();
		}
	}

	private static void NewSection(TextWriter writer)
	{
		for (int i = 0; i < 8; i++)
		{
			writer.Write("----------");
		}
		writer.WriteLine();
		writer.WriteLine();
	}

	private static int CompareSensor(ISensor a, ISensor b)
	{
		int num = a.SensorType.CompareTo(b.SensorType);
		if (num == 0)
		{
			return a.Index.CompareTo(b.Index);
		}
		return num;
	}

	private static void ReportHardwareSensorTree(IHardware hardware, TextWriter w, string space)
	{
		w.WriteLine("{0}|", space);
		w.WriteLine("{0}+- {1} ({2})", space, hardware.Name, hardware.Identifier);
		ISensor[] sensors = hardware.Sensors;
		Array.Sort(sensors, CompareSensor);
		ISensor[] array = sensors;
		foreach (ISensor sensor in array)
		{
			w.WriteLine("{0}|  +- {1,-14} : {2,8:G6} {3,8:G6} {4,8:G6} ({5})", space, sensor.Name, sensor.Value, sensor.Min, sensor.Max, sensor.Identifier);
		}
		IHardware[] subHardware = hardware.SubHardware;
		for (int i = 0; i < subHardware.Length; i++)
		{
			ReportHardwareSensorTree(subHardware[i], w, "|  ");
		}
	}

	private static void ReportHardwareParameterTree(IHardware hardware, TextWriter w, string space)
	{
		w.WriteLine("{0}|", space);
		w.WriteLine("{0}+- {1} ({2})", space, hardware.Name, hardware.Identifier);
		ISensor[] sensors = hardware.Sensors;
		Array.Sort(sensors, CompareSensor);
		ISensor[] array = sensors;
		foreach (ISensor sensor in array)
		{
			string text = space + "|  ";
			if (sensor.Parameters.Length <= 0)
			{
				continue;
			}
			w.WriteLine("{0}|", text);
			w.WriteLine("{0}+- {1} ({2})", text, sensor.Name, sensor.Identifier);
			foreach (IParameter parameter in sensor.Parameters)
			{
				string arg = text + "|  ";
				w.WriteLine("{0}+- {1} : {2}", arg, parameter.Name, string.Format(CultureInfo.InvariantCulture, "{0} : {1}", new object[2] { parameter.DefaultValue, parameter.Value }));
			}
		}
		IHardware[] subHardware = hardware.SubHardware;
		for (int i = 0; i < subHardware.Length; i++)
		{
			ReportHardwareParameterTree(subHardware[i], w, "|  ");
		}
	}

	private static void ReportHardware(IHardware hardware, TextWriter w)
	{
		string report = hardware.GetReport();
		if (!string.IsNullOrEmpty(report))
		{
			NewSection(w);
			w.Write(report);
		}
		IHardware[] subHardware = hardware.SubHardware;
		for (int i = 0; i < subHardware.Length; i++)
		{
			ReportHardware(subHardware[i], w);
		}
	}

	public string GetReport()
	{
		using StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
		stringWriter.WriteLine();
		stringWriter.WriteLine("Open Hardware Monitor Report");
		stringWriter.WriteLine();
		Version version = typeof(Computer).Assembly.GetName().Version;
		NewSection(stringWriter);
		stringWriter.Write("Version: ");
		stringWriter.WriteLine(version.ToString());
		stringWriter.WriteLine();
		NewSection(stringWriter);
		stringWriter.Write("Common Language Runtime: ");
		stringWriter.WriteLine(Environment.Version.ToString());
		stringWriter.Write("Operating System: ");
		stringWriter.WriteLine(Environment.OSVersion.ToString());
		stringWriter.Write("Process Type: ");
		stringWriter.WriteLine((IntPtr.Size == 4) ? "32-Bit" : "64-Bit");
		stringWriter.WriteLine();
		string report = Ring0.GetReport();
		if (report != null)
		{
			NewSection(stringWriter);
			stringWriter.Write(report);
			stringWriter.WriteLine();
		}
		NewSection(stringWriter);
		stringWriter.WriteLine("Sensors");
		stringWriter.WriteLine();
		foreach (IGroup group in groups)
		{
			IHardware[] hardware = group.Hardware;
			for (int i = 0; i < hardware.Length; i++)
			{
				ReportHardwareSensorTree(hardware[i], stringWriter, "");
			}
		}
		stringWriter.WriteLine();
		NewSection(stringWriter);
		stringWriter.WriteLine("Parameters");
		stringWriter.WriteLine();
		foreach (IGroup group2 in groups)
		{
			IHardware[] hardware = group2.Hardware;
			for (int i = 0; i < hardware.Length; i++)
			{
				ReportHardwareParameterTree(hardware[i], stringWriter, "");
			}
		}
		stringWriter.WriteLine();
		foreach (IGroup group3 in groups)
		{
			string report2 = group3.GetReport();
			if (!string.IsNullOrEmpty(report2))
			{
				NewSection(stringWriter);
				stringWriter.Write(report2);
			}
			IHardware[] hardware = group3.Hardware;
			for (int i = 0; i < hardware.Length; i++)
			{
				ReportHardware(hardware[i], stringWriter);
			}
		}
		return stringWriter.ToString();
	}

	public void Close()
	{
		if (open)
		{
			RemoveGroups();
			Opcode.Close();
			Ring0.Close();
			smbios = null;
			open = false;
		}
	}

	private void RemoveGroups()
	{
		while (groups.Count > 0)
		{
			IGroup obj = groups[groups.Count - 1];
			Remove(obj);
		}
	}

	public void Accept(IVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.VisitComputer(this);
	}

	public void Traverse(IVisitor visitor)
	{
		foreach (IGroup group in groups)
		{
			IHardware[] hardware = group.Hardware;
			for (int i = 0; i < hardware.Length; i++)
			{
				hardware[i].Accept(visitor);
			}
		}
	}
}
