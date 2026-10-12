using System.Globalization;

namespace OpenHardwareMonitor.Hardware;

internal class Control : IControl
{
	private readonly Identifier identifier;

	private readonly ISettings settings;

	private ControlMode mode;

	private float softwareValue;

	private float minSoftwareValue;

	private float maxSoftwareValue;

	public Identifier Identifier => identifier;

	public ControlMode ControlMode
	{
		get
		{
			return mode;
		}
		private set
		{
			if (mode != value)
			{
				mode = value;
				if (ControlModeChanged != null)
				{
					ControlModeChanged(this);
				}
				ISettings settings = this.settings;
				string name = new Identifier(identifier, "mode").ToString();
				int num = (int)mode;
				settings.SetValue(name, num.ToString(CultureInfo.InvariantCulture));
			}
		}
	}

	public float SoftwareValue
	{
		get
		{
			return softwareValue;
		}
		private set
		{
			if (softwareValue != value)
			{
				softwareValue = value;
				if (SoftwareControlValueChanged != null)
				{
					SoftwareControlValueChanged(this);
				}
				settings.SetValue(new Identifier(identifier, "value").ToString(), value.ToString(CultureInfo.InvariantCulture));
			}
		}
	}

	public float MinSoftwareValue => minSoftwareValue;

	public float MaxSoftwareValue => maxSoftwareValue;

	internal event ControlEventHandler ControlModeChanged;

	internal event ControlEventHandler SoftwareControlValueChanged;

	public Control(ISensor sensor, ISettings settings, float minSoftwareValue, float maxSoftwareValue)
	{
		identifier = new Identifier(sensor.Identifier, "control");
		this.settings = settings;
		this.minSoftwareValue = minSoftwareValue;
		this.maxSoftwareValue = maxSoftwareValue;
		if (!float.TryParse(settings.GetValue(new Identifier(identifier, "value").ToString(), "0"), NumberStyles.Float, CultureInfo.InvariantCulture, out softwareValue))
		{
			softwareValue = 0f;
		}
		if (!int.TryParse(settings.GetValue(new Identifier(identifier, "mode").ToString(), 0.ToString(CultureInfo.InvariantCulture)), NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
		{
			mode = ControlMode.Undefined;
		}
		else
		{
			mode = (ControlMode)result;
		}
	}

	public void SetDefault()
	{
		ControlMode = ControlMode.Default;
	}

	public void SetSoftware(float value)
	{
		ControlMode = ControlMode.Software;
		SoftwareValue = value;
	}
}
