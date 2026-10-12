using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using OpenHardwareMonitor.Collections;

namespace OpenHardwareMonitor.Hardware;

internal class Sensor : ISensor, IElement
{
	private readonly string defaultName;

	private string name;

	private readonly int index;

	private readonly bool defaultHidden;

	private readonly SensorType sensorType;

	private readonly Hardware hardware;

	private readonly ReadOnlyArray<IParameter> parameters;

	private float? currentValue;

	private float? minValue;

	private float? maxValue;

	private readonly RingCollection<SensorValue> values = new RingCollection<SensorValue>();

	private readonly ISettings settings;

	private IControl control;

	private float sum;

	private int count;

	public IHardware Hardware => hardware;

	public SensorType SensorType => sensorType;

	public Identifier Identifier => new Identifier(hardware.Identifier, sensorType.ToString().ToLowerInvariant(), index.ToString(CultureInfo.InvariantCulture));

	public string Name
	{
		get
		{
			return name;
		}
		set
		{
			if (!string.IsNullOrEmpty(value))
			{
				name = value;
			}
			else
			{
				name = defaultName;
			}
			settings.SetValue(new Identifier(Identifier, "name").ToString(), name);
		}
	}

	public int Index => index;

	public bool IsDefaultHidden => defaultHidden;

	public IReadOnlyArray<IParameter> Parameters => parameters;

	public float? Value
	{
		get
		{
			return currentValue;
		}
		set
		{
			DateTime utcNow = DateTime.UtcNow;
			while (values.Count > 0 && (utcNow - values.First.Time).TotalDays > 1.0)
			{
				values.Remove();
			}
			if (value.HasValue)
			{
				sum += value.Value;
				count++;
				if (count == 4)
				{
					AppendValue(sum / (float)count, utcNow);
					sum = 0f;
					count = 0;
				}
			}
			currentValue = value;
			if (minValue > value || !minValue.HasValue)
			{
				minValue = value;
			}
			if (maxValue < value || !maxValue.HasValue)
			{
				maxValue = value;
			}
		}
	}

	public float? Min => minValue;

	public float? Max => maxValue;

	public IEnumerable<SensorValue> Values => values;

	public IControl Control
	{
		get
		{
			return control;
		}
		internal set
		{
			control = value;
		}
	}

	public Sensor(string name, int index, SensorType sensorType, Hardware hardware, ISettings settings)
		: this(name, index, sensorType, hardware, null, settings)
	{
	}

	public Sensor(string name, int index, SensorType sensorType, Hardware hardware, ParameterDescription[] parameterDescriptions, ISettings settings)
		: this(name, index, defaultHidden: false, sensorType, hardware, parameterDescriptions, settings)
	{
	}

	public Sensor(string name, int index, bool defaultHidden, SensorType sensorType, Hardware hardware, ParameterDescription[] parameterDescriptions, ISettings settings)
	{
		this.index = index;
		this.defaultHidden = defaultHidden;
		this.sensorType = sensorType;
		this.hardware = hardware;
		Parameter[] array = new Parameter[parameterDescriptions?.Length ?? 0];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = new Parameter(parameterDescriptions[i], this, settings);
		}
		IParameter[] array2 = array;
		parameters = array2;
		this.settings = settings;
		defaultName = name;
		this.name = settings.GetValue(new Identifier(Identifier, "name").ToString(), name);
		GetSensorValuesFromSettings();
		hardware.Closing += (IHardware h) =>
		{
			SetSensorValuesToSettings();
		};
	}

	private void SetSensorValuesToSettings()
	{
		using MemoryStream memoryStream = new MemoryStream();
		using (GZipStream stream = new GZipStream(memoryStream, CompressionMode.Compress))
		{
			using BufferedStream output = new BufferedStream(stream, 65536);
			using BinaryWriter binaryWriter = new BinaryWriter(output);
			long num = 0L;
			foreach (SensorValue item in (IEnumerable<SensorValue>)values)
			{
				long num2 = item.Time.ToBinary();
				binaryWriter.Write(num2 - num);
				num = num2;
				binaryWriter.Write(item.Value);
			}
			binaryWriter.Flush();
		}
		settings.SetValue(new Identifier(Identifier, "values").ToString(), Convert.ToBase64String(memoryStream.ToArray()));
	}

	private void GetSensorValuesFromSettings()
	{
		string text = new Identifier(Identifier, "values").ToString();
		string value = settings.GetValue(text, null);
		try
		{
			byte[] buffer = Convert.FromBase64String(value);
			value = null;
			DateTime utcNow = DateTime.UtcNow;
			using MemoryStream stream = new MemoryStream(buffer);
			using GZipStream input = new GZipStream(stream, CompressionMode.Decompress);
			using BinaryReader binaryReader = new BinaryReader(input);
			try
			{
				long num = 0L;
				while (true)
				{
					num += binaryReader.ReadInt64();
					DateTime dateTime = DateTime.FromBinary(num);
					if (!(dateTime > utcNow))
					{
						float value2 = binaryReader.ReadSingle();
						AppendValue(value2, dateTime);
						continue;
					}
					break;
				}
			}
			catch (EndOfStreamException)
			{
			}
		}
		catch
		{
		}
		if (values.Count > 0)
		{
			AppendValue(float.NaN, DateTime.UtcNow);
		}
		settings.Remove(text);
	}

	private void AppendValue(float value, DateTime time)
	{
		if (values.Count >= 2 && values.Last.Value == value && values[values.Count - 2].Value == value)
		{
			values.Last = new SensorValue(value, time);
		}
		else
		{
			values.Append(new SensorValue(value, time));
		}
	}

	public void ResetMin()
	{
		minValue = null;
	}

	public void ResetMax()
	{
		maxValue = null;
	}

	public void Accept(IVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.VisitSensor(this);
	}

	public void Traverse(IVisitor visitor)
	{
		foreach (IParameter parameter in parameters)
		{
			parameter.Accept(visitor);
		}
	}
}
