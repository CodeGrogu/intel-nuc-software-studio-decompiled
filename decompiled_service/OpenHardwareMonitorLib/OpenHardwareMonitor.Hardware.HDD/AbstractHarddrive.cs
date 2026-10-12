using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using OpenHardwareMonitor.Collections;

namespace OpenHardwareMonitor.Hardware.HDD;

internal abstract class AbstractHarddrive : Hardware
{
	private const int UPDATE_DIVIDER = 30;

	private static Type[] hddTypes = new Type[7]
	{
		typeof(SSDPlextor),
		typeof(SSDIntel),
		typeof(SSDSandforce),
		typeof(SSDIndilinx),
		typeof(SSDSamsung),
		typeof(SSDMicron),
		typeof(GenericHarddisk)
	};

	private string firmwareRevision;

	private readonly ISmart smart;

	private readonly IntPtr handle;

	private readonly int index;

	private int count;

	private IList<SmartAttribute> smartAttributes;

	private IDictionary<SmartAttribute, Sensor> sensors;

	private DriveInfo[] driveInfos;

	private Sensor usageSensor;

	public override HardwareType HardwareType => HardwareType.HDD;

	protected AbstractHarddrive(ISmart smart, string name, string firmwareRevision, int index, IEnumerable<SmartAttribute> smartAttributes, ISettings settings)
		: base(name, new Identifier("hdd", index.ToString(CultureInfo.InvariantCulture)), settings)
	{
		this.firmwareRevision = firmwareRevision;
		this.smart = smart;
		handle = smart.OpenDrive(index);
		if (handle != smart.InvalidHandle)
		{
			smart.EnableSmart(handle, index);
		}
		this.index = index;
		count = 0;
		this.smartAttributes = new List<SmartAttribute>(smartAttributes);
		string[] logicalDrives = smart.GetLogicalDrives(index);
		List<DriveInfo> list = new List<DriveInfo>(logicalDrives.Length);
		string[] array = logicalDrives;
		foreach (string driveName in array)
		{
			try
			{
				if (new DriveInfo(driveName).TotalSize > 0)
				{
					list.Add(new DriveInfo(driveName));
				}
			}
			catch (ArgumentException)
			{
			}
			catch (IOException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
		}
		driveInfos = list.ToArray();
		CreateSensors();
	}

	public static AbstractHarddrive CreateInstance(ISmart smart, int driveIndex, ISettings settings)
	{
		IntPtr intPtr = smart.OpenDrive(driveIndex);
		string text = null;
		string text2 = null;
		DriveAttributeValue[] array = new DriveAttributeValue[0];
		if (intPtr != smart.InvalidHandle)
		{
			bool flag = smart.ReadNameAndFirmwareRevision(intPtr, driveIndex, out text, out text2);
			if (smart.EnableSmart(intPtr, driveIndex))
			{
				array = smart.ReadSmartData(intPtr, driveIndex);
			}
			smart.CloseHandle(intPtr);
			if (!flag)
			{
				text = null;
				text2 = null;
			}
		}
		else
		{
			string[] logicalDrives = smart.GetLogicalDrives(driveIndex);
			if (logicalDrives == null || logicalDrives.Length == 0)
			{
				return null;
			}
			bool flag2 = false;
			string[] array2 = logicalDrives;
			foreach (string driveName in array2)
			{
				try
				{
					if (new DriveInfo(driveName).TotalSize > 0)
					{
						flag2 = true;
						break;
					}
				}
				catch (ArgumentException)
				{
				}
				catch (IOException)
				{
				}
				catch (UnauthorizedAccessException)
				{
				}
			}
			if (!flag2)
			{
				return null;
			}
		}
		if (string.IsNullOrEmpty(text))
		{
			text = "Generic Hard Disk";
		}
		if (string.IsNullOrEmpty(text2))
		{
			text2 = "Unknown";
		}
		Type[] array3 = hddTypes;
		foreach (Type type in array3)
		{
			NamePrefixAttribute[] array4 = type.GetCustomAttributes(typeof(NamePrefixAttribute), inherit: true) as NamePrefixAttribute[];
			RequireSmartAttribute[] obj = type.GetCustomAttributes(typeof(RequireSmartAttribute), inherit: true) as RequireSmartAttribute[];
			bool flag3 = true;
			RequireSmartAttribute[] array5 = obj;
			foreach (RequireSmartAttribute requireSmartAttribute in array5)
			{
				bool flag4 = false;
				DriveAttributeValue[] array6 = array;
				for (int k = 0; k < array6.Length; k++)
				{
					if (array6[k].Identifier == requireSmartAttribute.AttributeId)
					{
						flag4 = true;
						break;
					}
				}
				if (!flag4)
				{
					flag3 = false;
					break;
				}
			}
			if (!flag3)
			{
				continue;
			}
			NamePrefixAttribute[] array7 = array4;
			foreach (NamePrefixAttribute namePrefixAttribute in array7)
			{
				if (text.StartsWith(namePrefixAttribute.Prefix, StringComparison.InvariantCulture))
				{
					return Activator.CreateInstance(type, smart, text, text2, driveIndex, settings) as AbstractHarddrive;
				}
			}
		}
		return null;
	}

	private void CreateSensors()
	{
		sensors = new Dictionary<SmartAttribute, Sensor>();
		if (handle != smart.InvalidHandle)
		{
			IList<Pair<SensorType, int>> list = new List<Pair<SensorType, int>>();
			DriveAttributeValue[] array = smart.ReadSmartData(handle, index);
			foreach (SmartAttribute smartAttribute in smartAttributes)
			{
				if (!smartAttribute.SensorType.HasValue)
				{
					continue;
				}
				bool flag = false;
				DriveAttributeValue[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					if (array2[i].Identifier == smartAttribute.Identifier)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					Pair<SensorType, int> item = new Pair<SensorType, int>(smartAttribute.SensorType.Value, smartAttribute.SensorChannel);
					if (!list.Contains(item))
					{
						Sensor sensor = new Sensor(smartAttribute.SensorName, smartAttribute.SensorChannel, smartAttribute.DefaultHiddenSensor, smartAttribute.SensorType.Value, this, smartAttribute.ParameterDescriptions, settings);
						sensors.Add(smartAttribute, sensor);
						ActivateSensor(sensor);
						list.Add(item);
					}
				}
			}
		}
		if (driveInfos.Length != 0)
		{
			usageSensor = new Sensor("Used Space", 0, SensorType.Load, this, settings);
			ActivateSensor(usageSensor);
		}
	}

	public virtual void UpdateAdditionalSensors(DriveAttributeValue[] values)
	{
	}

	public override void Update()
	{
		if (count == 0)
		{
			if (handle != smart.InvalidHandle)
			{
				DriveAttributeValue[] array = smart.ReadSmartData(handle, index);
				foreach (KeyValuePair<SmartAttribute, Sensor> sensor in sensors)
				{
					SmartAttribute key = sensor.Key;
					DriveAttributeValue[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						DriveAttributeValue value = array2[i];
						if (value.Identifier == key.Identifier)
						{
							Sensor value2 = sensor.Value;
							value2.Value = key.ConvertValue(value, value2.Parameters);
						}
					}
				}
				UpdateAdditionalSensors(array);
			}
			if (usageSensor != null)
			{
				long num = 0L;
				long num2 = 0L;
				for (int j = 0; j < driveInfos.Length; j++)
				{
					if (driveInfos[j].IsReady)
					{
						try
						{
							num += driveInfos[j].TotalSize;
							num2 += driveInfos[j].TotalFreeSpace;
						}
						catch (IOException)
						{
						}
						catch (UnauthorizedAccessException)
						{
						}
					}
				}
				if (num > 0)
				{
					usageSensor.Value = 100f - 100f * (float)num2 / (float)num;
				}
				else
				{
					usageSensor.Value = null;
				}
			}
		}
		count++;
		count %= 30;
	}

	public override string GetReport()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine(GetType().Name);
		stringBuilder.AppendLine();
		stringBuilder.AppendLine("Drive name: " + name);
		stringBuilder.AppendLine("Firmware version: " + firmwareRevision);
		stringBuilder.AppendLine();
		if (handle != smart.InvalidHandle)
		{
			DriveAttributeValue[] array = smart.ReadSmartData(handle, index);
			DriveThresholdValue[] array2 = smart.ReadSmartThresholds(handle, index);
			if (array.Length != 0)
			{
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, " {0}{1}{2}{3}{4}{5}{6}{7}", "ID".PadRight(3), "Description".PadRight(35), "Raw Value".PadRight(13), "Worst".PadRight(6), "Value".PadRight(6), "Thres".PadRight(6), "Physical".PadRight(8), Environment.NewLine);
				DriveAttributeValue[] array3 = array;
				for (int i = 0; i < array3.Length; i++)
				{
					DriveAttributeValue value = array3[i];
					if (value.Identifier == 0)
					{
						break;
					}
					byte? b = null;
					DriveThresholdValue[] array4 = array2;
					for (int j = 0; j < array4.Length; j++)
					{
						DriveThresholdValue driveThresholdValue = array4[j];
						if (driveThresholdValue.Identifier == value.Identifier)
						{
							b = driveThresholdValue.Threshold;
						}
					}
					string text = "Unknown";
					float? num = null;
					foreach (SmartAttribute smartAttribute in smartAttributes)
					{
						if (smartAttribute.Identifier == value.Identifier)
						{
							text = smartAttribute.Name;
							num = ((!(smartAttribute.HasRawValueConversion | smartAttribute.SensorType.HasValue)) ? ((float?)null) : new float?(smartAttribute.ConvertValue(value, null)));
						}
					}
					string text2 = BitConverter.ToString(value.RawValue);
					CultureInfo invariantCulture = CultureInfo.InvariantCulture;
					object[] array5 = new object[8];
					byte b2 = value.Identifier;
					array5[0] = b2.ToString("X2").PadRight(3);
					array5[1] = text.PadRight(35);
					array5[2] = text2.Replace("-", "").PadRight(13);
					b2 = value.WorstValue;
					array5[3] = b2.ToString(CultureInfo.InvariantCulture).PadRight(6);
					b2 = value.AttrValue;
					array5[4] = b2.ToString(CultureInfo.InvariantCulture).PadRight(6);
					array5[5] = (b.HasValue ? b.Value.ToString(CultureInfo.InvariantCulture) : "-").PadRight(6);
					array5[6] = (num.HasValue ? num.Value.ToString(CultureInfo.InvariantCulture) : "-").PadRight(8);
					array5[7] = Environment.NewLine;
					stringBuilder.AppendFormat(invariantCulture, " {0}{1}{2}{3}{4}{5}{6}{7}", array5);
				}
				stringBuilder.AppendLine();
			}
		}
		DriveInfo[] array6 = driveInfos;
		foreach (DriveInfo driveInfo in array6)
		{
			if (driveInfo.IsReady)
			{
				try
				{
					stringBuilder.AppendLine("Logical drive name: " + driveInfo.Name);
					stringBuilder.AppendLine("Format: " + driveInfo.DriveFormat);
					stringBuilder.AppendLine("Total size: " + driveInfo.TotalSize);
					stringBuilder.AppendLine("Total free space: " + driveInfo.TotalFreeSpace);
					stringBuilder.AppendLine();
				}
				catch (IOException)
				{
				}
				catch (UnauthorizedAccessException)
				{
				}
			}
		}
		return stringBuilder.ToString();
	}

	protected static float RawToInt(byte[] raw, byte value, IReadOnlyArray<IParameter> parameters)
	{
		return (raw[3] << 24) | (raw[2] << 16) | (raw[1] << 8) | raw[0];
	}

	public override void Close()
	{
		if (handle != smart.InvalidHandle)
		{
			smart.CloseHandle(handle);
		}
		base.Close();
	}

	public override void Traverse(IVisitor visitor)
	{
		ISensor[] array = Sensors;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Accept(visitor);
		}
	}
}
