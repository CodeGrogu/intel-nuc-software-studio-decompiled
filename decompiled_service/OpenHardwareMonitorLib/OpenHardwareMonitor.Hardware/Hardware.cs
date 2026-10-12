using System;
using OpenHardwareMonitor.Collections;

namespace OpenHardwareMonitor.Hardware;

internal abstract class Hardware : IHardware, IElement
{
	private readonly Identifier identifier;

	protected readonly string name;

	private string customName;

	protected readonly ISettings settings;

	protected readonly ListSet<ISensor> active = new ListSet<ISensor>();

	public IHardware[] SubHardware => new IHardware[0];

	public virtual IHardware Parent => null;

	public virtual ISensor[] Sensors => active.ToArray();

	public string Name
	{
		get
		{
			return customName;
		}
		set
		{
			if (!string.IsNullOrEmpty(value))
			{
				customName = value;
			}
			else
			{
				customName = name;
			}
			settings.SetValue(new Identifier(Identifier, "name").ToString(), customName);
		}
	}

	public Identifier Identifier => identifier;

	public abstract HardwareType HardwareType { get; }

	public event SensorEventHandler SensorAdded;

	public event SensorEventHandler SensorRemoved;

	public event HardwareEventHandler Closing;

	public Hardware(string name, Identifier identifier, ISettings settings)
	{
		this.settings = settings;
		this.identifier = identifier;
		this.name = name;
		customName = settings.GetValue(new Identifier(Identifier, "name").ToString(), name);
	}

	protected virtual void ActivateSensor(ISensor sensor)
	{
		if (active.Add(sensor) && SensorAdded != null)
		{
			SensorAdded(sensor);
		}
	}

	protected virtual void DeactivateSensor(ISensor sensor)
	{
		if (active.Remove(sensor) && SensorRemoved != null)
		{
			SensorRemoved(sensor);
		}
	}

	public virtual string GetReport()
	{
		return null;
	}

	public abstract void Update();

	public virtual void Close()
	{
		if (Closing != null)
		{
			Closing(this);
		}
	}

	public void Accept(IVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.VisitHardware(this);
	}

	public virtual void Traverse(IVisitor visitor)
	{
		foreach (ISensor item in active)
		{
			item.Accept(visitor);
		}
	}
}
