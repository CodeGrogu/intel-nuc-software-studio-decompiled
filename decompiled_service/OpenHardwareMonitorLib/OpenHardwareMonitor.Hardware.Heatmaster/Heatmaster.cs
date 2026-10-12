using System;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace OpenHardwareMonitor.Hardware.Heatmaster;

internal class Heatmaster : Hardware, IDisposable
{
	private readonly string portName;

	private SerialPort serialPort;

	private readonly int hardwareRevision;

	private readonly int firmwareRevision;

	private readonly int firmwareCRC;

	private readonly Sensor[] fans;

	private readonly Sensor[] controls;

	private readonly Sensor[] temperatures;

	private readonly Sensor[] flows;

	private readonly Sensor[] relays;

	private readonly bool available;

	private readonly StringBuilder buffer = new StringBuilder();

	public override HardwareType HardwareType => HardwareType.Heatmaster;

	private string ReadLine(int timeout)
	{
		int num = 0;
		StringBuilder stringBuilder = new StringBuilder();
		while (num <= timeout)
		{
			while (serialPort.BytesToRead > 0)
			{
				byte b = (byte)serialPort.ReadByte();
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

	private string ReadField(int device, char field)
	{
		serialPort.WriteLine("[0:" + device + "]R" + field);
		for (int i = 0; i < 5; i++)
		{
			Match match = Regex.Match(ReadLine(200), "-\\[0:" + device.ToString(CultureInfo.InvariantCulture) + "\\]R" + Regex.Escape(field.ToString(CultureInfo.InvariantCulture)) + ":(.*)");
			if (match.Success)
			{
				return match.Groups[1].Value;
			}
		}
		return null;
	}

	protected string ReadString(int device, char field)
	{
		string text = ReadField(device, field);
		if (text != null && text[0] == '"' && text[text.Length - 1] == '"')
		{
			return text.Substring(1, text.Length - 2);
		}
		return null;
	}

	protected int ReadInteger(int device, char field)
	{
		if (int.TryParse(ReadField(device, field), out var result))
		{
			return result;
		}
		return 0;
	}

	private bool WriteField(int device, char field, string value)
	{
		serialPort.WriteLine("[0:" + device + "]W" + field + ":" + value);
		for (int i = 0; i < 5; i++)
		{
			if (Regex.Match(ReadLine(200), "-\\[0:" + device.ToString(CultureInfo.InvariantCulture) + "\\]W" + Regex.Escape(field.ToString(CultureInfo.InvariantCulture)) + ":" + value).Success)
			{
				return true;
			}
		}
		return false;
	}

	protected bool WriteInteger(int device, char field, int value)
	{
		return WriteField(device, field, value.ToString(CultureInfo.InvariantCulture));
	}

	protected bool WriteString(int device, char field, string value)
	{
		return WriteField(device, field, "\"" + value + "\"");
	}

	public Heatmaster(string portName, ISettings settings)
		: base("Heatmaster", new Identifier("heatmaster", portName.TrimStart('/').ToLowerInvariant()), settings)
	{
		this.portName = portName;
		try
		{
			serialPort = new SerialPort(portName, 38400, Parity.None, 8, StopBits.One);
			serialPort.Open();
			serialPort.NewLine = '\r'.ToString();
			hardwareRevision = ReadInteger(0, 'H');
			firmwareRevision = ReadInteger(0, 'V');
			firmwareCRC = ReadInteger(0, 'C');
			int num = Math.Min(ReadInteger(32, '?'), 4);
			int num2 = Math.Min(ReadInteger(48, '?'), 6);
			int num3 = Math.Min(ReadInteger(64, '?'), 1);
			int num4 = Math.Min(ReadInteger(80, '?'), 1);
			fans = new Sensor[num];
			controls = new Sensor[num];
			for (int i = 0; i < num; i++)
			{
				int num5 = 33 + i;
				string text = ReadString(num5, 'C');
				fans[i] = new Sensor(text, num5, SensorType.Fan, this, settings);
				fans[i].Value = ReadInteger(num5, 'R');
				ActivateSensor(fans[i]);
				controls[i] = new Sensor(text, num5, SensorType.Control, this, settings);
				controls[i].Value = 100f / 255f * (float)ReadInteger(num5, 'P');
				ActivateSensor(controls[i]);
			}
			temperatures = new Sensor[num2];
			for (int j = 0; j < num2; j++)
			{
				int num6 = 49 + j;
				string text2 = ReadString(num6, 'C');
				temperatures[j] = new Sensor(text2, num6, SensorType.Temperature, this, settings);
				int num7 = ReadInteger(num6, 'T');
				temperatures[j].Value = 0.1f * (float)num7;
				if (num7 != -32768)
				{
					ActivateSensor(temperatures[j]);
				}
			}
			flows = new Sensor[num3];
			for (int k = 0; k < num3; k++)
			{
				int num8 = 65 + k;
				string text3 = ReadString(num8, 'C');
				flows[k] = new Sensor(text3, num8, SensorType.Flow, this, settings);
				flows[k].Value = 0.1f * (float)ReadInteger(num8, 'L');
				ActivateSensor(flows[k]);
			}
			relays = new Sensor[num4];
			for (int l = 0; l < num4; l++)
			{
				int num9 = 81 + l;
				string text4 = ReadString(num9, 'C');
				relays[l] = new Sensor(text4, num9, SensorType.Control, this, settings);
				relays[l].Value = 100 * ReadInteger(num9, 'S');
				ActivateSensor(relays[l]);
			}
			WriteInteger(0, 'L', 2);
			available = true;
		}
		catch (IOException)
		{
		}
		catch (TimeoutException)
		{
		}
	}

	private void ProcessUpdateLine(string line)
	{
		Match match = Regex.Match(line, ">\\[0:(\\d+)\\]([0-9:\\|-]+)");
		if (!match.Success || !int.TryParse(match.Groups[1].Value, out var result))
		{
			return;
		}
		string[] array = match.Groups[2].Value.Split('|');
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Split(':');
			int[] array3 = new int[array2.Length];
			bool flag = true;
			for (int j = 0; j < array3.Length; j++)
			{
				if (!int.TryParse(array2[j], out array3[j]))
				{
					flag = false;
					break;
				}
			}
			if (!flag)
			{
				continue;
			}
			switch (result)
			{
			case 32:
				if (array3.Length == 3 && array3[0] <= fans.Length)
				{
					fans[array3[0] - 1].Value = array3[1];
					controls[array3[0] - 1].Value = 100f / 255f * (float)array3[2];
				}
				break;
			case 48:
				if (array3.Length == 2 && array3[0] <= temperatures.Length)
				{
					temperatures[array3[0] - 1].Value = 0.1f * (float)array3[1];
				}
				break;
			case 64:
				if (array3.Length == 3 && array3[0] <= flows.Length)
				{
					flows[array3[0] - 1].Value = 0.1f * (float)array3[1];
				}
				break;
			case 80:
				if (array3.Length == 2 && array3[0] <= relays.Length)
				{
					relays[array3[0] - 1].Value = 100 * array3[1];
				}
				break;
			}
		}
	}

	public override void Update()
	{
		if (!available)
		{
			return;
		}
		while (serialPort.IsOpen && serialPort.BytesToRead > 0)
		{
			byte b = (byte)serialPort.ReadByte();
			if (b == 13)
			{
				ProcessUpdateLine(buffer.ToString());
				buffer.Length = 0;
			}
			else
			{
				buffer.Append((char)b);
			}
		}
	}

	public override string GetReport()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("Heatmaster");
		stringBuilder.AppendLine();
		stringBuilder.Append("Port: ");
		stringBuilder.AppendLine(portName);
		stringBuilder.Append("Hardware Revision: ");
		stringBuilder.AppendLine(hardwareRevision.ToString(CultureInfo.InvariantCulture));
		stringBuilder.Append("Firmware Revision: ");
		stringBuilder.AppendLine(firmwareRevision.ToString(CultureInfo.InvariantCulture));
		stringBuilder.Append("Firmware CRC: ");
		stringBuilder.AppendLine(firmwareCRC.ToString(CultureInfo.InvariantCulture));
		stringBuilder.AppendLine();
		return stringBuilder.ToString();
	}

	public override void Close()
	{
		serialPort.Close();
		serialPort.Dispose();
		serialPort = null;
		base.Close();
	}

	public void Dispose()
	{
		if (serialPort != null)
		{
			serialPort.Dispose();
			serialPort = null;
		}
	}
}
