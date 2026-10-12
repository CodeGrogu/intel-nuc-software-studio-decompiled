using System;
using System.Globalization;

namespace OpenHardwareMonitor.Hardware;

internal class Parameter : IParameter, IElement
{
	private readonly ISensor sensor;

	private ParameterDescription description;

	private float value;

	private bool isDefault;

	private readonly ISettings settings;

	public ISensor Sensor => sensor;

	public Identifier Identifier => new Identifier(sensor.Identifier, "parameter", Name.Replace(" ", "").ToLowerInvariant());

	public string Name => description.Name;

	public string Description => description.Description;

	public float Value
	{
		get
		{
			return value;
		}
		set
		{
			isDefault = false;
			this.value = value;
			settings.SetValue(Identifier.ToString(), value.ToString(CultureInfo.InvariantCulture));
		}
	}

	public float DefaultValue => description.DefaultValue;

	public bool IsDefault
	{
		get
		{
			return isDefault;
		}
		set
		{
			isDefault = value;
			if (value)
			{
				this.value = description.DefaultValue;
				settings.Remove(Identifier.ToString());
			}
		}
	}

	public Parameter(ParameterDescription description, ISensor sensor, ISettings settings)
	{
		this.sensor = sensor;
		this.description = description;
		this.settings = settings;
		isDefault = !settings.Contains(Identifier.ToString());
		value = description.DefaultValue;
		if (!isDefault && !float.TryParse(settings.GetValue(Identifier.ToString(), "0"), NumberStyles.Float, CultureInfo.InvariantCulture, out value))
		{
			value = description.DefaultValue;
		}
	}

	public void Accept(IVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.VisitParameter(this);
	}

	public void Traverse(IVisitor visitor)
	{
	}
}
