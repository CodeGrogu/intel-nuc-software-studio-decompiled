using System;
using System.Text;
using OpenHardwareMonitor.Hardware.LPC;

namespace OpenHardwareMonitor.Hardware.Mainboard;

internal class Mainboard : IHardware, IElement
{
	private readonly SMBIOS smbios;

	private readonly string name;

	private string customName;

	private readonly ISettings settings;

	private readonly LPCIO lpcio;

	private readonly LMSensors lmSensors;

	private readonly Hardware[] superIOHardware;

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

	public Identifier Identifier => new Identifier("mainboard");

	public HardwareType HardwareType => HardwareType.Mainboard;

	public virtual IHardware Parent => null;

	public IHardware[] SubHardware => superIOHardware;

	public ISensor[] Sensors => new ISensor[0];

	public event SensorEventHandler SensorAdded;

	public event SensorEventHandler SensorRemoved;

	public Mainboard(SMBIOS smbios, ISettings settings)
	{
		this.settings = settings;
		this.smbios = smbios;
		Manufacturer manufacturer = ((smbios.Board == null) ? Manufacturer.Unknown : Identification.GetManufacturer(smbios.Board.ManufacturerName));
		Model model = ((smbios.Board == null) ? Model.Unknown : Identification.GetModel(smbios.Board.ProductName));
		if (smbios.Board != null)
		{
			if (!string.IsNullOrEmpty(smbios.Board.ProductName))
			{
				if (manufacturer == Manufacturer.Unknown)
				{
					name = smbios.Board.ProductName;
				}
				else
				{
					name = manufacturer.ToString() + " " + smbios.Board.ProductName;
				}
			}
			else
			{
				name = manufacturer.ToString();
			}
		}
		else
		{
			name = Manufacturer.Unknown.ToString();
		}
		customName = settings.GetValue(new Identifier(Identifier, "name").ToString(), name);
		ISuperIO[] superIO;
		if (OperatingSystem.IsUnix)
		{
			lmSensors = new LMSensors();
			superIO = lmSensors.SuperIO;
		}
		else
		{
			lpcio = new LPCIO();
			superIO = lpcio.SuperIO;
		}
		superIOHardware = new Hardware[superIO.Length];
		for (int i = 0; i < superIO.Length; i++)
		{
			superIOHardware[i] = new SuperIOHardware(this, superIO[i], manufacturer, model, settings);
		}
	}

	public string GetReport()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("Mainboard");
		stringBuilder.AppendLine();
		stringBuilder.Append(smbios.GetReport());
		if (lpcio != null)
		{
			stringBuilder.Append(lpcio.GetReport());
		}
		byte[] table = FirmwareTable.GetTable(FirmwareTable.Provider.ACPI, "TAMG");
		if (table != null)
		{
			GigabyteTAMG gigabyteTAMG = new GigabyteTAMG(table);
			stringBuilder.Append(gigabyteTAMG.GetReport());
		}
		return stringBuilder.ToString();
	}

	public void Update()
	{
	}

	public void Close()
	{
		if (lmSensors != null)
		{
			lmSensors.Close();
		}
		Hardware[] array = superIOHardware;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Close();
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

	public void Traverse(IVisitor visitor)
	{
		Hardware[] array = superIOHardware;
		for (int i = 0; i < array.Length; i++)
		{
			((IElement)array[i]).Accept(visitor);
		}
	}
}
