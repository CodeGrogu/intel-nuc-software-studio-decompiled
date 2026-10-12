using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;

namespace OpenHardwareMonitor.Hardware.TBalancer;

internal class TBalancer : Hardware
{
	private delegate void MethodDelegate();

	private readonly int portIndex;

	private readonly byte protocolVersion;

	private readonly Sensor[] digitalTemperatures = new Sensor[8];

	private readonly Sensor[] analogTemperatures = new Sensor[4];

	private readonly Sensor[] sensorhubTemperatures = new Sensor[6];

	private readonly Sensor[] sensorhubFlows = new Sensor[2];

	private readonly Sensor[] fans = new Sensor[4];

	private readonly Sensor[] controls = new Sensor[4];

	private readonly Sensor[] miniNGTemperatures = new Sensor[4];

	private readonly Sensor[] miniNGFans = new Sensor[4];

	private readonly Sensor[] miniNGControls = new Sensor[4];

	private readonly List<ISensor> deactivating = new List<ISensor>();

	private FT_HANDLE handle;

	private byte[] data = new byte[285];

	private byte[] primaryData = new byte[0];

	private byte[] alternativeData = new byte[0];

	public const byte STARTFLAG = 100;

	public const byte ENDFLAG = 254;

	private readonly MethodDelegate alternativeRequest;

	public override HardwareType HardwareType => HardwareType.TBalancer;

	public TBalancer(int portIndex, byte protocolVersion, ISettings settings)
		: base("T-Balancer bigNG", new Identifier("bigng", portIndex.ToString(CultureInfo.InvariantCulture)), settings)
	{
		this.portIndex = portIndex;
		this.protocolVersion = protocolVersion;
		ParameterDescription[] parameterDescriptions = new ParameterDescription[1]
		{
			new ParameterDescription("Offset [°C]", "Temperature offset.", 0f)
		};
		int num = 0;
		for (int i = 0; i < digitalTemperatures.Length; i++)
		{
			digitalTemperatures[i] = new Sensor("Digital Sensor " + i, num + i, SensorType.Temperature, this, parameterDescriptions, settings);
		}
		num += digitalTemperatures.Length;
		for (int j = 0; j < analogTemperatures.Length; j++)
		{
			analogTemperatures[j] = new Sensor("Analog Sensor " + (j + 1), num + j, SensorType.Temperature, this, parameterDescriptions, settings);
		}
		num += analogTemperatures.Length;
		for (int k = 0; k < sensorhubTemperatures.Length; k++)
		{
			sensorhubTemperatures[k] = new Sensor("Sensorhub Sensor " + k, num + k, SensorType.Temperature, this, parameterDescriptions, settings);
		}
		num += sensorhubTemperatures.Length;
		for (int l = 0; l < miniNGTemperatures.Length; l++)
		{
			miniNGTemperatures[l] = new Sensor("miniNG #" + (l / 2 + 1) + " Sensor " + (l % 2 + 1), num + l, SensorType.Temperature, this, parameterDescriptions, settings);
		}
		num += miniNGTemperatures.Length;
		for (int m = 0; m < sensorhubFlows.Length; m++)
		{
			sensorhubFlows[m] = new Sensor("Flowmeter " + (m + 1), m, SensorType.Flow, this, new ParameterDescription[1]
			{
				new ParameterDescription("Impulse Rate", "The impulse rate of the flowmeter in pulses/L", 509f)
			}, settings);
		}
		for (int n = 0; n < controls.Length; n++)
		{
			controls[n] = new Sensor("Fan Channel " + n, n, SensorType.Control, this, settings);
		}
		for (int num2 = 0; num2 < miniNGControls.Length; num2++)
		{
			miniNGControls[num2] = new Sensor("miniNG #" + (num2 / 2 + 1) + " Fan Channel " + (num2 % 2 + 1), 4 + num2, SensorType.Control, this, settings);
		}
		alternativeRequest = DelayedAlternativeRequest;
		Open();
		Update();
	}

	protected override void ActivateSensor(ISensor sensor)
	{
		deactivating.Remove(sensor);
		base.ActivateSensor(sensor);
	}

	protected override void DeactivateSensor(ISensor sensor)
	{
		if (deactivating.Contains(sensor))
		{
			deactivating.Remove(sensor);
			base.DeactivateSensor(sensor);
		}
		else if (active.Contains(sensor))
		{
			deactivating.Add(sensor);
		}
	}

	private void ReadminiNG(int number)
	{
		int num = 1 + number * 65;
		if (data[num + 61] != 254)
		{
			return;
		}
		for (int i = 0; i < 2; i++)
		{
			Sensor sensor = miniNGTemperatures[number * 2 + i];
			if (data[num + 7 + i] > 0)
			{
				sensor.Value = 0.5f * (float)(int)data[num + 7 + i] + sensor.Parameters[0].Value;
				ActivateSensor(sensor);
			}
			else
			{
				DeactivateSensor(sensor);
			}
		}
		for (int j = 0; j < 2; j++)
		{
			if (miniNGFans[number * 2 + j] == null)
			{
				miniNGFans[number * 2 + j] = new Sensor("miniNG #" + (number + 1) + " Fan Channel " + (j + 1), 4 + number * 2 + j, SensorType.Fan, this, settings);
			}
			Sensor sensor2 = miniNGFans[number * 2 + j];
			sensor2.Value = 20f * (float)(int)data[num + 43 + 2 * j];
			ActivateSensor(sensor2);
		}
		for (int k = 0; k < 2; k++)
		{
			Sensor sensor3 = miniNGControls[number * 2 + k];
			sensor3.Value = (int)data[num + 15 + k];
			ActivateSensor(sensor3);
		}
	}

	private void ReadData()
	{
		FTD2XX.Read(handle, data);
		if (data[0] != 100)
		{
			FTD2XX.FT_Purge(handle, FT_PURGE.FT_PURGE_RX);
		}
		else if (data[1] == byte.MaxValue || data[1] == 88)
		{
			if (data[274] != protocolVersion)
			{
				return;
			}
			if (primaryData.Length == 0)
			{
				primaryData = new byte[data.Length];
			}
			data.CopyTo(primaryData, 0);
			for (int i = 0; i < digitalTemperatures.Length; i++)
			{
				if (data[238 + i] > 0)
				{
					digitalTemperatures[i].Value = 0.5f * (float)(int)data[238 + i] + digitalTemperatures[i].Parameters[0].Value;
					ActivateSensor(digitalTemperatures[i]);
				}
				else
				{
					DeactivateSensor(digitalTemperatures[i]);
				}
			}
			for (int j = 0; j < analogTemperatures.Length; j++)
			{
				if (data[260 + j] > 0)
				{
					analogTemperatures[j].Value = 0.5f * (float)(int)data[260 + j] + analogTemperatures[j].Parameters[0].Value;
					ActivateSensor(analogTemperatures[j]);
				}
				else
				{
					DeactivateSensor(analogTemperatures[j]);
				}
			}
			for (int k = 0; k < sensorhubTemperatures.Length; k++)
			{
				if (data[246 + k] > 0)
				{
					sensorhubTemperatures[k].Value = 0.5f * (float)(int)data[246 + k] + sensorhubTemperatures[k].Parameters[0].Value;
					ActivateSensor(sensorhubTemperatures[k]);
				}
				else
				{
					DeactivateSensor(sensorhubTemperatures[k]);
				}
			}
			for (int l = 0; l < sensorhubFlows.Length; l++)
			{
				if (data[231 + l] > 0 && data[234] > 0)
				{
					float num = (float)(int)data[231 + l] * 4f / (float)(int)data[234];
					float value = sensorhubFlows[l].Parameters[0].Value;
					sensorhubFlows[l].Value = num * 3600f / value;
					ActivateSensor(sensorhubFlows[l]);
				}
				else
				{
					DeactivateSensor(sensorhubFlows[l]);
				}
			}
			for (int m = 0; m < fans.Length; m++)
			{
				float defaultValue = 11.5f * (float)((data[149 + 2 * m] << 8) | data[148 + 2 * m]);
				if (fans[m] == null)
				{
					fans[m] = new Sensor("Fan Channel " + m, m, SensorType.Fan, this, new ParameterDescription[1]
					{
						new ParameterDescription("MaxRPM", "Maximum revolutions per minute (RPM) of the fan.", defaultValue)
					}, settings);
				}
				float num2 = (((data[136] & (1 << m)) != 0) ? (0.01f * (float)(int)data[141 + m]) : (0.02f * (float)(int)data[137 + m]));
				fans[m].Value = fans[m].Parameters[0].Value * num2;
				ActivateSensor(fans[m]);
				controls[m].Value = 100f * num2;
				ActivateSensor(controls[m]);
			}
		}
		else if (data[1] == 253)
		{
			if (alternativeData.Length == 0)
			{
				alternativeData = new byte[data.Length];
			}
			data.CopyTo(alternativeData, 0);
			ReadminiNG(0);
			if (data[66] == 253)
			{
				ReadminiNG(1);
			}
		}
	}

	public override string GetReport()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("T-Balancer bigNG");
		stringBuilder.AppendLine();
		stringBuilder.Append("Port Index: ");
		stringBuilder.AppendLine(portIndex.ToString(CultureInfo.InvariantCulture));
		stringBuilder.AppendLine();
		stringBuilder.AppendLine("Primary System Information Answer");
		stringBuilder.AppendLine();
		stringBuilder.AppendLine("       00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F");
		stringBuilder.AppendLine();
		for (int i = 0; i <= 17; i++)
		{
			stringBuilder.Append(" ");
			stringBuilder.Append((i << 4).ToString("X3", CultureInfo.InvariantCulture));
			stringBuilder.Append("  ");
			for (int j = 0; j <= 15; j++)
			{
				int num = (i << 4) | j;
				if (num < primaryData.Length)
				{
					stringBuilder.Append(" ");
					stringBuilder.Append(primaryData[num].ToString("X2", CultureInfo.InvariantCulture));
				}
			}
			stringBuilder.AppendLine();
		}
		stringBuilder.AppendLine();
		if (alternativeData.Length != 0)
		{
			stringBuilder.AppendLine("Alternative System Information Answer");
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("       00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F");
			stringBuilder.AppendLine();
			for (int k = 0; k <= 17; k++)
			{
				stringBuilder.Append(" ");
				stringBuilder.Append((k << 4).ToString("X3", CultureInfo.InvariantCulture));
				stringBuilder.Append("  ");
				for (int l = 0; l <= 15; l++)
				{
					int num2 = (k << 4) | l;
					if (num2 < alternativeData.Length)
					{
						stringBuilder.Append(" ");
						stringBuilder.Append(alternativeData[num2].ToString("X2", CultureInfo.InvariantCulture));
					}
				}
				stringBuilder.AppendLine();
			}
			stringBuilder.AppendLine();
		}
		return stringBuilder.ToString();
	}

	private void DelayedAlternativeRequest()
	{
		Thread.Sleep(500);
		FTD2XX.Write(handle, new byte[1] { 55 });
	}

	public void Open()
	{
		FTD2XX.FT_Open(portIndex, out handle);
		FTD2XX.FT_SetBaudRate(handle, 19200u);
		FTD2XX.FT_SetDataCharacteristics(handle, 8, 1, 0);
		FTD2XX.FT_SetFlowControl(handle, FT_FLOW_CONTROL.FT_FLOW_RTS_CTS, 17, 19);
		FTD2XX.FT_SetTimeouts(handle, 1000u, 1000u);
		FTD2XX.FT_Purge(handle, FT_PURGE.FT_PURGE_ALL);
	}

	public override void Update()
	{
		while (FTD2XX.BytesToRead(handle) >= 285)
		{
			ReadData();
		}
		if (FTD2XX.BytesToRead(handle) == 1)
		{
			FTD2XX.ReadByte(handle);
		}
		FTD2XX.Write(handle, new byte[1] { 56 });
		alternativeRequest.BeginInvoke(null, null);
	}

	public override void Close()
	{
		FTD2XX.FT_Close(handle);
		base.Close();
	}
}
