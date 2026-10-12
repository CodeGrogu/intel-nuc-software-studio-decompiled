using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace OpenHardwareMonitor.Hardware.Mainboard;

internal class GigabyteTAMG
{
	private struct Sensor
	{
		public string Name;

		public SensorType Type;

		public int Channel;

		public float Value;
	}

	private enum SensorType
	{
		Voltage = 1,
		Temperature = 2,
		Fan = 4,
		Case = 8
	}

	private byte[] table;

	private Sensor[] sensors;

	public GigabyteTAMG(byte[] table)
	{
		if (table == null)
		{
			throw new ArgumentNullException("table");
		}
		this.table = table;
		int num = IndexOf(table, Encoding.ASCII.GetBytes("$HEALTH$"), 0);
		if (num >= 0)
		{
			num += 8;
			using MemoryStream input = new MemoryStream(table, num, table.Length - num);
			using BinaryReader binaryReader = new BinaryReader(input);
			try
			{
				binaryReader.ReadInt64();
				int num2 = binaryReader.ReadInt32();
				binaryReader.ReadInt64();
				binaryReader.ReadInt32();
				sensors = new Sensor[num2];
				for (int i = 0; i < sensors.Length; i++)
				{
					sensors[i].Name = new string(binaryReader.ReadChars(32)).TrimEnd(default(char));
					sensors[i].Type = (SensorType)binaryReader.ReadByte();
					sensors[i].Channel = binaryReader.ReadInt16();
					sensors[i].Channel |= binaryReader.ReadByte() << 24;
					binaryReader.ReadInt64();
					int num3 = binaryReader.ReadInt32();
					if (sensors[i].Type == SensorType.Voltage)
					{
						sensors[i].Value = 0.001f * (float)num3;
					}
					else
					{
						sensors[i].Value = num3;
					}
					binaryReader.ReadInt64();
				}
				return;
			}
			catch (IOException)
			{
				sensors = new Sensor[0];
				return;
			}
		}
		sensors = new Sensor[0];
	}

	public static int IndexOf(byte[] array, byte[] pattern, int startIndex)
	{
		if (array == null || pattern == null || pattern.Length > array.Length)
		{
			return -1;
		}
		for (int i = startIndex; i < array.Length - pattern.Length; i++)
		{
			bool flag = true;
			for (int j = 0; j < pattern.Length; j++)
			{
				if (array[i + j] != pattern[j])
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				return i;
			}
		}
		return -1;
	}

	private string GetCompressedAndEncodedTable()
	{
		string text;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress))
			{
				gZipStream.Write(table, 0, table.Length);
			}
			text = Convert.ToBase64String(memoryStream.ToArray());
		}
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; (double)i < Math.Ceiling((double)text.Length / 64.0); i++)
		{
			stringBuilder.Append(" ");
			for (int j = 0; j < 64; j++)
			{
				int num = (i << 6) | j;
				if (num < text.Length)
				{
					stringBuilder.Append(text[num]);
				}
			}
			stringBuilder.AppendLine();
		}
		return stringBuilder.ToString();
	}

	public string GetReport()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (sensors.Length != 0)
		{
			stringBuilder.AppendLine("Gigabyte TAMG Sensors");
			stringBuilder.AppendLine();
			Sensor[] array = sensors;
			for (int i = 0; i < array.Length; i++)
			{
				Sensor sensor = array[i];
				stringBuilder.AppendFormat(" {0,-10}: {1,8:G6} ({2})", sensor.Name, sensor.Value, sensor.Type);
				stringBuilder.AppendLine();
			}
			stringBuilder.AppendLine();
		}
		if (table.Length != 0)
		{
			stringBuilder.AppendLine("Gigabyte TAMG Table");
			stringBuilder.AppendLine();
			stringBuilder.Append(GetCompressedAndEncodedTable());
			stringBuilder.AppendLine();
		}
		return stringBuilder.ToString();
	}
}
