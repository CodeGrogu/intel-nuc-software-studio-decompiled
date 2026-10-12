using System;
using System.Collections.Generic;
using System.Threading;
using OpenHardwareMonitor.Hardware.LPC;

namespace OpenHardwareMonitor.Hardware.Mainboard;

internal sealed class SuperIOHardware : Hardware
{
	private delegate float? ReadValueDelegate(int index);

	private delegate void UpdateDelegate();

	private class Voltage
	{
		public readonly string Name;

		public readonly int Index;

		public readonly float Ri;

		public readonly float Rf;

		public readonly float Vf;

		public readonly bool Hidden;

		public Voltage(string name, int index)
			: this(name, index, hidden: false)
		{
		}

		public Voltage(string name, int index, bool hidden)
			: this(name, index, 0f, 1f, 0f, hidden)
		{
		}

		public Voltage(string name, int index, float ri, float rf)
			: this(name, index, ri, rf, 0f, hidden: false)
		{
		}

		public Voltage(string name, int index, float ri, float rf, float vf, bool hidden)
		{
			Name = name;
			Index = index;
			Ri = ri;
			Rf = rf;
			Vf = vf;
			Hidden = hidden;
		}
	}

	private class Temperature
	{
		public readonly string Name;

		public readonly int Index;

		public Temperature(string name, int index)
		{
			Name = name;
			Index = index;
		}
	}

	private class Fan
	{
		public readonly string Name;

		public readonly int Index;

		public Fan(string name, int index)
		{
			Name = name;
			Index = index;
		}
	}

	private class Ctrl
	{
		public readonly string Name;

		public readonly int Index;

		public Ctrl(string name, int index)
		{
			Name = name;
			Index = index;
		}
	}

	private readonly Mainboard mainboard;

	private readonly ISuperIO superIO;

	private readonly List<Sensor> voltages = new List<Sensor>();

	private readonly List<Sensor> temperatures = new List<Sensor>();

	private readonly List<Sensor> fans = new List<Sensor>();

	private readonly List<Sensor> controls = new List<Sensor>();

	private readonly ReadValueDelegate readVoltage;

	private readonly ReadValueDelegate readTemperature;

	private readonly ReadValueDelegate readFan;

	private readonly ReadValueDelegate readControl;

	private readonly UpdateDelegate postUpdate;

	private readonly Mutex mutex;

	public override HardwareType HardwareType => HardwareType.SuperIO;

	public override IHardware Parent => mainboard;

	public SuperIOHardware(Mainboard mainboard, ISuperIO superIO, Manufacturer manufacturer, Model model, ISettings settings)
		: base(ChipName.GetName(superIO.Chip), new Identifier("lpc", superIO.Chip.ToString().ToLowerInvariant()), settings)
	{
		this.mainboard = mainboard;
		this.superIO = superIO;
		GetBoardSpecificConfiguration(superIO, manufacturer, model, out var v, out var t, out var f, out var c, out readVoltage, out readTemperature, out readFan, out readControl, out postUpdate, out mutex);
		CreateVoltageSensors(superIO, settings, v);
		CreateTemperatureSensors(superIO, settings, t);
		CreateFanSensors(superIO, settings, f);
		CreateControlSensors(superIO, settings, c);
	}

	private void CreateControlSensors(ISuperIO superIO, ISettings settings, IList<Ctrl> c)
	{
		foreach (Ctrl item in c)
		{
			int index = item.Index;
			if (index >= superIO.Controls.Length)
			{
				continue;
			}
			Sensor sensor = new Sensor(item.Name, index, SensorType.Control, this, settings);
			Control control = new Control(sensor, settings, 0f, 100f);
			control.ControlModeChanged += (Control cc) =>
			{
				switch (cc.ControlMode)
				{
				case ControlMode.Undefined:
					break;
				case ControlMode.Default:
					superIO.SetControl(index, null);
					break;
				case ControlMode.Software:
					superIO.SetControl(index, (byte)((double)cc.SoftwareValue * 2.55));
					break;
				}
			};
			control.SoftwareControlValueChanged += (Control cc) =>
			{
				if (cc.ControlMode == ControlMode.Software)
				{
					superIO.SetControl(index, (byte)((double)cc.SoftwareValue * 2.55));
				}
			};
			switch (control.ControlMode)
			{
			case ControlMode.Default:
				superIO.SetControl(index, null);
				break;
			case ControlMode.Software:
				superIO.SetControl(index, (byte)((double)control.SoftwareValue * 2.55));
				break;
			}
			sensor.Control = control;
			controls.Add(sensor);
			ActivateSensor(sensor);
		}
	}

	private void CreateFanSensors(ISuperIO superIO, ISettings settings, IList<Fan> f)
	{
		foreach (Fan item2 in f)
		{
			if (item2.Index < superIO.Fans.Length)
			{
				Sensor item = new Sensor(item2.Name, item2.Index, SensorType.Fan, this, settings);
				fans.Add(item);
			}
		}
	}

	private void CreateTemperatureSensors(ISuperIO superIO, ISettings settings, IList<Temperature> t)
	{
		foreach (Temperature item2 in t)
		{
			if (item2.Index < superIO.Temperatures.Length)
			{
				Sensor item = new Sensor(item2.Name, item2.Index, SensorType.Temperature, this, new ParameterDescription[1]
				{
					new ParameterDescription("Offset [°C]", "Temperature offset.", 0f)
				}, settings);
				temperatures.Add(item);
			}
		}
	}

	private void CreateVoltageSensors(ISuperIO superIO, ISettings settings, IList<Voltage> v)
	{
		foreach (Voltage item2 in v)
		{
			if (item2.Index < superIO.Voltages.Length)
			{
				Sensor item = new Sensor(item2.Name, item2.Index, item2.Hidden, SensorType.Voltage, this, new ParameterDescription[3]
				{
					new ParameterDescription("Ri [kΩ]", "Input resistance.\nVoltage = value + (value - Vf) * Ri / Rf.", item2.Ri),
					new ParameterDescription("Rf [kΩ]", "Reference resistance.\nVoltage = value + (value - Vf) * Ri / Rf.", item2.Rf),
					new ParameterDescription("Vf [V]", "Reference voltage.\nVoltage = value + (value - Vf) * Ri / Rf.", item2.Vf)
				}, settings);
				voltages.Add(item);
			}
		}
	}

	private static void GetBoardSpecificConfiguration(ISuperIO superIO, Manufacturer manufacturer, Model model, out IList<Voltage> v, out IList<Temperature> t, out IList<Fan> f, out IList<Ctrl> c, out ReadValueDelegate readVoltage, out ReadValueDelegate readTemperature, out ReadValueDelegate readFan, out ReadValueDelegate readControl, out UpdateDelegate postUpdate, out Mutex mutex)
	{
		readVoltage = (int index) => superIO.Voltages[index];
		readTemperature = (int index) => superIO.Temperatures[index];
		readFan = (int index) => superIO.Fans[index];
		readControl = (int index) => superIO.Controls[index];
		postUpdate = () =>
		{
		};
		mutex = null;
		v = new List<Voltage>();
		t = new List<Temperature>();
		f = new List<Fan>();
		c = new List<Ctrl>();
		switch (superIO.Chip)
		{
		case Chip.IT8705F:
		case Chip.IT8712F:
		case Chip.IT8716F:
		case Chip.IT8718F:
		case Chip.IT8720F:
		case Chip.IT8726F:
			GetITEConfigurationsA(superIO, manufacturer, model, v, t, f, c, ref readFan, ref postUpdate, ref mutex);
			break;
		case Chip.IT8620E:
		case Chip.IT8628E:
		case Chip.IT8655E:
		case Chip.IT8665E:
		case Chip.IT8686E:
		case Chip.IT8688E:
		case Chip.IT8721F:
		case Chip.IT8728F:
		case Chip.IT8771E:
		case Chip.IT8772E:
			GetITEConfigurationsB(superIO, manufacturer, model, v, t, f, c);
			break;
		case Chip.IT879XE:
			GetITEConfigurationsC(superIO, manufacturer, model, v, t, f, c);
			break;
		case Chip.F71858:
		{
			v.Add(new Voltage("VCC3V", 0, 150f, 150f));
			v.Add(new Voltage("VSB3V", 1, 150f, 150f));
			v.Add(new Voltage("Battery", 2, 150f, 150f));
			for (int num3 = 0; num3 < superIO.Temperatures.Length; num3++)
			{
				t.Add(new Temperature("Temperature #" + (num3 + 1), num3));
			}
			for (int num4 = 0; num4 < superIO.Fans.Length; num4++)
			{
				f.Add(new Fan("Fan #" + (num4 + 1), num4));
			}
			break;
		}
		case Chip.F71882:
		case Chip.F71862:
		case Chip.F71889F:
		case Chip.F71869:
		case Chip.F71808E:
		case Chip.F71889ED:
		case Chip.F71889AD:
		case Chip.F71869A:
			GetFintekConfiguration(superIO, manufacturer, model, v, t, f, c);
			break;
		case Chip.W83627EHF:
			GetWinbondConfigurationEHF(manufacturer, model, v, t, f);
			break;
		case Chip.W83627DHG:
		case Chip.W83667HG:
		case Chip.W83627DHGP:
		case Chip.W83667HGB:
			GetWinbondConfigurationHG(manufacturer, model, v, t, f);
			break;
		case Chip.W83627HF:
		case Chip.W83627THF:
		case Chip.W83687THF:
			v.Add(new Voltage("CPU VCore", 0));
			v.Add(new Voltage("Voltage #2", 1, hidden: true));
			v.Add(new Voltage("Voltage #3", 2, hidden: true));
			v.Add(new Voltage("AVCC", 3, 34f, 51f));
			v.Add(new Voltage("Voltage #5", 4, hidden: true));
			v.Add(new Voltage("5VSB", 5, 34f, 51f));
			v.Add(new Voltage("VBAT", 6));
			t.Add(new Temperature("CPU", 0));
			t.Add(new Temperature("Auxiliary", 1));
			t.Add(new Temperature("System", 2));
			f.Add(new Fan("System Fan", 0));
			f.Add(new Fan("CPU Fan", 1));
			f.Add(new Fan("Auxiliary Fan", 2));
			break;
		case Chip.NCT6771F:
		case Chip.NCT6776F:
			GetNuvotonConfigurationF(superIO, manufacturer, model, v, t, f, c);
			break;
		case Chip.NCT610X:
		{
			v.Add(new Voltage("CPU VCore", 0));
			v.Add(new Voltage("Voltage #0", 1, hidden: true));
			v.Add(new Voltage("AVCC", 2, 34f, 34f));
			v.Add(new Voltage("3VCC", 3, 34f, 34f));
			v.Add(new Voltage("Voltage #1", 4, hidden: true));
			v.Add(new Voltage("Voltage #2", 5, hidden: true));
			v.Add(new Voltage("Reserved", 6, hidden: true));
			v.Add(new Voltage("3VSB", 7, 34f, 34f));
			v.Add(new Voltage("VBAT", 8, 34f, 34f));
			v.Add(new Voltage("Voltage #10", 9, hidden: true));
			t.Add(new Temperature("SYS", 1));
			t.Add(new Temperature("CPU Core", 2));
			t.Add(new Temperature("AUX", 3));
			for (int num = 0; num < superIO.Fans.Length; num++)
			{
				f.Add(new Fan("Fan #" + (num + 1), num));
			}
			for (int num2 = 0; num2 < superIO.Controls.Length; num2++)
			{
				c.Add(new Ctrl("Fan Control #" + (num2 + 1), num2));
			}
			break;
		}
		case Chip.NCT6779D:
		case Chip.NCT6791D:
		case Chip.NCT6792D:
		case Chip.NCT6792DA:
		case Chip.NCT6793D:
		case Chip.NCT6795D:
		case Chip.NCT6796D:
		case Chip.NCT6796DR:
		case Chip.NCT6798D:
		case Chip.NCT6797D:
			GetNuvotonConfigurationD(superIO, manufacturer, model, v, t, f, c);
			break;
		default:
			GetDefaultConfiguration(superIO, v, t, f, c);
			break;
		}
	}

	private static void GetDefaultConfiguration(ISuperIO superIO, IList<Voltage> v, IList<Temperature> t, IList<Fan> f, IList<Ctrl> c)
	{
		for (int i = 0; i < superIO.Voltages.Length; i++)
		{
			v.Add(new Voltage("Voltage #" + (i + 1), i, hidden: true));
		}
		for (int j = 0; j < superIO.Temperatures.Length; j++)
		{
			t.Add(new Temperature("Temperature #" + (j + 1), j));
		}
		for (int k = 0; k < superIO.Fans.Length; k++)
		{
			f.Add(new Fan("Fan #" + (k + 1), k));
		}
		for (int l = 0; l < superIO.Controls.Length; l++)
		{
			c.Add(new Ctrl("Fan Control #" + (l + 1), l));
		}
	}

	private static void GetITEConfigurationsA(ISuperIO superIO, Manufacturer manufacturer, Model model, IList<Voltage> v, IList<Temperature> t, IList<Fan> f, IList<Ctrl> c, ref ReadValueDelegate readFan, ref UpdateDelegate postUpdate, ref Mutex mutex)
	{
		switch (manufacturer)
		{
		case Manufacturer.ASUS:
			switch (model)
			{
			case Model.Crosshair_III_Formula:
			{
				v.Add(new Voltage("VBat", 8));
				t.Add(new Temperature("CPU", 0));
				for (int num = 0; num < superIO.Fans.Length; num++)
				{
					f.Add(new Fan("Fan #" + (num + 1), num));
				}
				break;
			}
			case Model.M2N_SLI_DELUXE:
				v.Add(new Voltage("CPU VCore", 0));
				v.Add(new Voltage("+3.3V", 1));
				v.Add(new Voltage("+5V", 3, 6.8f, 10f));
				v.Add(new Voltage("+12V", 4, 30f, 10f));
				v.Add(new Voltage("+5VSB", 7, 6.8f, 10f));
				v.Add(new Voltage("VBat", 8));
				t.Add(new Temperature("CPU", 0));
				t.Add(new Temperature("Motherboard", 1));
				f.Add(new Fan("CPU Fan", 0));
				f.Add(new Fan("Chassis Fan #1", 1));
				f.Add(new Fan("Power Fan", 2));
				break;
			case Model.M4A79XTD_EVO:
				v.Add(new Voltage("+5V", 3, 6.8f, 10f));
				v.Add(new Voltage("VBat", 8));
				t.Add(new Temperature("CPU", 0));
				t.Add(new Temperature("Motherboard", 1));
				f.Add(new Fan("CPU Fan", 0));
				f.Add(new Fan("Chassis Fan #1", 1));
				f.Add(new Fan("Chassis Fan #2", 2));
				break;
			default:
			{
				v.Add(new Voltage("CPU VCore", 0));
				v.Add(new Voltage("Voltage #2", 1, hidden: true));
				v.Add(new Voltage("Voltage #3", 2, hidden: true));
				v.Add(new Voltage("Voltage #4", 3, hidden: true));
				v.Add(new Voltage("Voltage #5", 4, hidden: true));
				v.Add(new Voltage("Voltage #6", 5, hidden: true));
				v.Add(new Voltage("Voltage #7", 6, hidden: true));
				v.Add(new Voltage("Voltage #8", 7, hidden: true));
				v.Add(new Voltage("VBat", 8));
				for (int l = 0; l < superIO.Temperatures.Length; l++)
				{
					t.Add(new Temperature("Temperature #" + (l + 1), l));
				}
				for (int m = 0; m < superIO.Fans.Length; m++)
				{
					f.Add(new Fan("Fan #" + (m + 1), m));
				}
				for (int n = 0; n < superIO.Controls.Length; n++)
				{
					c.Add(new Ctrl("Fan Control #" + (n + 1), n));
				}
				break;
			}
			}
			break;
		case Manufacturer.ASRock:
		{
			if (model == Model.P55_Deluxe)
			{
				GetASRockConfiguration(superIO, v, t, f, ref readFan, ref postUpdate, ref mutex);
				break;
			}
			v.Add(new Voltage("CPU VCore", 0));
			v.Add(new Voltage("Voltage #2", 1, hidden: true));
			v.Add(new Voltage("Voltage #3", 2, hidden: true));
			v.Add(new Voltage("Voltage #4", 3, hidden: true));
			v.Add(new Voltage("Voltage #5", 4, hidden: true));
			v.Add(new Voltage("Voltage #6", 5, hidden: true));
			v.Add(new Voltage("Voltage #7", 6, hidden: true));
			v.Add(new Voltage("Voltage #8", 7, hidden: true));
			v.Add(new Voltage("VBat", 8));
			for (int num8 = 0; num8 < superIO.Temperatures.Length; num8++)
			{
				t.Add(new Temperature("Temperature #" + (num8 + 1), num8));
			}
			for (int num9 = 0; num9 < superIO.Fans.Length; num9++)
			{
				f.Add(new Fan("Fan #" + (num9 + 1), num9));
			}
			break;
		}
		case Manufacturer.DFI:
			switch (model)
			{
			case Model.LP_BI_P45_T2RS_Elite:
				v.Add(new Voltage("CPU VCore", 0));
				v.Add(new Voltage("FSB VTT", 1));
				v.Add(new Voltage("+3.3V", 2));
				v.Add(new Voltage("+5V", 3, 6.8f, 10f));
				v.Add(new Voltage("+12V", 4, 30f, 10f));
				v.Add(new Voltage("NB Core", 5));
				v.Add(new Voltage("VDIMM", 6));
				v.Add(new Voltage("+5VSB", 7, 6.8f, 10f));
				v.Add(new Voltage("VBat", 8));
				t.Add(new Temperature("CPU", 0));
				t.Add(new Temperature("System", 1));
				t.Add(new Temperature("Chipset", 2));
				f.Add(new Fan("Fan #1", 0));
				f.Add(new Fan("Fan #2", 1));
				f.Add(new Fan("Fan #3", 2));
				break;
			case Model.LP_DK_P55_T3eH9:
				v.Add(new Voltage("CPU VCore", 0));
				v.Add(new Voltage("VTT", 1));
				v.Add(new Voltage("+3.3V", 2));
				v.Add(new Voltage("+5V", 3, 6.8f, 10f));
				v.Add(new Voltage("+12V", 4, 30f, 10f));
				v.Add(new Voltage("CPU PLL", 5));
				v.Add(new Voltage("DRAM", 6));
				v.Add(new Voltage("+5VSB", 7, 6.8f, 10f));
				v.Add(new Voltage("VBat", 8));
				t.Add(new Temperature("Chipset", 0));
				t.Add(new Temperature("CPU PWM", 1));
				t.Add(new Temperature("CPU", 2));
				f.Add(new Fan("Fan #1", 0));
				f.Add(new Fan("Fan #2", 1));
				f.Add(new Fan("Fan #3", 2));
				break;
			default:
			{
				v.Add(new Voltage("CPU VCore", 0));
				v.Add(new Voltage("VTT", 1, hidden: true));
				v.Add(new Voltage("+3.3V", 2, hidden: true));
				v.Add(new Voltage("+5V", 3, 6.8f, 10f, 0f, hidden: true));
				v.Add(new Voltage("+12V", 4, 30f, 10f, 0f, hidden: true));
				v.Add(new Voltage("Voltage #6", 5, hidden: true));
				v.Add(new Voltage("DRAM", 6, hidden: true));
				v.Add(new Voltage("+5VSB", 7, 6.8f, 10f, 0f, hidden: true));
				v.Add(new Voltage("VBat", 8));
				for (int num5 = 0; num5 < superIO.Temperatures.Length; num5++)
				{
					t.Add(new Temperature("Temperature #" + (num5 + 1), num5));
				}
				for (int num6 = 0; num6 < superIO.Fans.Length; num6++)
				{
					f.Add(new Fan("Fan #" + (num6 + 1), num6));
				}
				for (int num7 = 0; num7 < superIO.Controls.Length; num7++)
				{
					c.Add(new Ctrl("Fan Control #" + (num7 + 1), num7));
				}
				break;
			}
			}
			break;
		case Manufacturer.Gigabyte:
			switch (model)
			{
			case Model._965P_S3:
				v.Add(new Voltage("CPU VCore", 0));
				v.Add(new Voltage("DRAM", 1));
				v.Add(new Voltage("+3.3V", 2));
				v.Add(new Voltage("+5V", 3, 6.8f, 10f));
				v.Add(new Voltage("+12V", 7, 24.3f, 8.2f));
				v.Add(new Voltage("VBat", 8));
				t.Add(new Temperature("System", 0));
				t.Add(new Temperature("CPU", 1));
				f.Add(new Fan("CPU Fan", 0));
				f.Add(new Fan("System Fan", 1));
				break;
			case Model.EP45_DS3R:
			case Model.EP45_UD3R:
			case Model.X38_DS5:
				v.Add(new Voltage("CPU VCore", 0));
				v.Add(new Voltage("DRAM", 1));
				v.Add(new Voltage("+3.3V", 2));
				v.Add(new Voltage("+5V", 3, 6.8f, 10f));
				v.Add(new Voltage("+12V", 7, 24.3f, 8.2f));
				v.Add(new Voltage("VBat", 8));
				t.Add(new Temperature("System", 0));
				t.Add(new Temperature("CPU", 1));
				f.Add(new Fan("CPU Fan", 0));
				f.Add(new Fan("System Fan #2", 1));
				f.Add(new Fan("Power Fan", 2));
				f.Add(new Fan("System Fan #1", 3));
				break;
			case Model.EX58_EXTREME:
				v.Add(new Voltage("CPU VCore", 0));
				v.Add(new Voltage("DRAM", 1));
				v.Add(new Voltage("+5V", 3, 6.8f, 10f));
				v.Add(new Voltage("VBat", 8));
				t.Add(new Temperature("System", 0));
				t.Add(new Temperature("CPU", 1));
				t.Add(new Temperature("Northbridge", 2));
				f.Add(new Fan("CPU Fan", 0));
				f.Add(new Fan("System Fan #2", 1));
				f.Add(new Fan("Power Fan", 2));
				f.Add(new Fan("System Fan #1", 3));
				break;
			case Model.P35_DS3:
			case Model.P35_DS3L:
				v.Add(new Voltage("CPU VCore", 0));
				v.Add(new Voltage("DRAM", 1));
				v.Add(new Voltage("+3.3V", 2));
				v.Add(new Voltage("+5V", 3, 6.8f, 10f));
				v.Add(new Voltage("+12V", 7, 24.3f, 8.2f));
				v.Add(new Voltage("VBat", 8));
				t.Add(new Temperature("System", 0));
				t.Add(new Temperature("CPU", 1));
				f.Add(new Fan("CPU Fan", 0));
				f.Add(new Fan("System Fan #1", 1));
				f.Add(new Fan("System Fan #2", 2));
				f.Add(new Fan("Power Fan", 3));
				break;
			case Model.EX58_UD3R:
			case Model.H55_USB3:
			case Model.P55_UD4:
			case Model.P55A_UD3:
			case Model.P55M_UD4:
				v.Add(new Voltage("CPU VCore", 0));
				v.Add(new Voltage("DRAM", 1));
				v.Add(new Voltage("+3.3V", 2));
				v.Add(new Voltage("+5V", 3, 6.8f, 10f));
				v.Add(new Voltage("+12V", 5, 24.3f, 8.2f));
				v.Add(new Voltage("VBat", 8));
				t.Add(new Temperature("System", 0));
				t.Add(new Temperature("CPU", 2));
				f.Add(new Fan("CPU Fan", 0));
				f.Add(new Fan("System Fan #2", 1));
				f.Add(new Fan("Power Fan", 2));
				f.Add(new Fan("System Fan #1", 3));
				break;
			case Model.H55N_USB3:
				v.Add(new Voltage("CPU VCore", 0));
				v.Add(new Voltage("DRAM", 1));
				v.Add(new Voltage("+3.3V", 2));
				v.Add(new Voltage("+5V", 3, 6.8f, 10f));
				v.Add(new Voltage("+12V", 5, 24.3f, 8.2f));
				v.Add(new Voltage("VBat", 8));
				t.Add(new Temperature("System", 0));
				t.Add(new Temperature("CPU", 2));
				f.Add(new Fan("CPU Fan", 0));
				f.Add(new Fan("System Fan", 1));
				break;
			case Model.G41M_Combo:
			case Model.G41MT_S2:
			case Model.G41MT_S2P:
				v.Add(new Voltage("CPU VCore", 0));
				v.Add(new Voltage("DRAM", 1));
				v.Add(new Voltage("+3.3V", 2));
				v.Add(new Voltage("+5V", 3, 6.8f, 10f));
				v.Add(new Voltage("+12V", 7, 24.3f, 8.2f));
				v.Add(new Voltage("VBat", 8));
				t.Add(new Temperature("CPU", 2));
				f.Add(new Fan("CPU Fan", 0));
				f.Add(new Fan("System Fan", 1));
				break;
			case Model.GA_970A_UD3:
				v.Add(new Voltage("CPU VCore", 0));
				v.Add(new Voltage("DRAM", 1));
				v.Add(new Voltage("+3.3V", 2));
				v.Add(new Voltage("+5V", 3, 6.8f, 10f));
				v.Add(new Voltage("+12V", 4, 24.3f, 8.2f));
				v.Add(new Voltage("VBat", 8));
				t.Add(new Temperature("System", 0));
				t.Add(new Temperature("CPU", 1));
				f.Add(new Fan("CPU Fan", 0));
				f.Add(new Fan("System Fan #1", 1));
				f.Add(new Fan("System Fan #2", 2));
				f.Add(new Fan("Power Fan", 4));
				c.Add(new Ctrl("PWM 1", 0));
				c.Add(new Ctrl("PWM 2", 1));
				c.Add(new Ctrl("PWM 3", 2));
				break;
			case Model.GA_MA770T_UD3:
			case Model.GA_MA770T_UD3P:
			case Model.GA_MA790X_UD3P:
				v.Add(new Voltage("CPU VCore", 0));
				v.Add(new Voltage("DRAM", 1));
				v.Add(new Voltage("+3.3V", 2));
				v.Add(new Voltage("+5V", 3, 6.8f, 10f));
				v.Add(new Voltage("+12V", 4, 24.3f, 8.2f));
				v.Add(new Voltage("VBat", 8));
				t.Add(new Temperature("System", 0));
				t.Add(new Temperature("CPU", 1));
				f.Add(new Fan("CPU Fan", 0));
				f.Add(new Fan("System Fan #1", 1));
				f.Add(new Fan("System Fan #2", 2));
				f.Add(new Fan("Power Fan", 3));
				break;
			case Model.GA_MA78LM_S2H:
				v.Add(new Voltage("CPU VCore", 0));
				v.Add(new Voltage("DRAM", 1));
				v.Add(new Voltage("+3.3V", 2));
				v.Add(new Voltage("+5V", 3, 6.8f, 10f));
				v.Add(new Voltage("+12V", 4, 24.3f, 8.2f));
				v.Add(new Voltage("VBat", 8));
				t.Add(new Temperature("System", 0));
				t.Add(new Temperature("CPU", 1));
				t.Add(new Temperature("VRM", 2));
				f.Add(new Fan("CPU Fan", 0));
				f.Add(new Fan("System Fan #1", 1));
				f.Add(new Fan("System Fan #2", 2));
				f.Add(new Fan("Power Fan", 3));
				break;
			case Model.GA_MA785GM_US2H:
			case Model.GA_MA785GMT_UD2H:
				v.Add(new Voltage("CPU VCore", 0));
				v.Add(new Voltage("DRAM", 1));
				v.Add(new Voltage("+3.3V", 2));
				v.Add(new Voltage("+5V", 3, 6.8f, 10f));
				v.Add(new Voltage("+12V", 4, 24.3f, 8.2f));
				v.Add(new Voltage("VBat", 8));
				t.Add(new Temperature("System", 0));
				t.Add(new Temperature("CPU", 1));
				f.Add(new Fan("CPU Fan", 0));
				f.Add(new Fan("System Fan", 1));
				f.Add(new Fan("NB Fan", 2));
				break;
			case Model.X58A_UD3R:
				v.Add(new Voltage("CPU VCore", 0));
				v.Add(new Voltage("DRAM", 1));
				v.Add(new Voltage("+3.3V", 2));
				v.Add(new Voltage("+5V", 3, 6.8f, 10f));
				v.Add(new Voltage("+12V", 5, 24.3f, 8.2f));
				v.Add(new Voltage("VBat", 8));
				t.Add(new Temperature("System", 0));
				t.Add(new Temperature("CPU", 1));
				t.Add(new Temperature("Northbridge", 2));
				f.Add(new Fan("CPU Fan", 0));
				f.Add(new Fan("System Fan #2", 1));
				f.Add(new Fan("Power Fan", 2));
				f.Add(new Fan("System Fan #1", 3));
				break;
			default:
			{
				v.Add(new Voltage("CPU VCore", 0));
				v.Add(new Voltage("DRAM", 1, hidden: true));
				v.Add(new Voltage("+3.3V", 2, hidden: true));
				v.Add(new Voltage("+5V", 3, 6.8f, 10f, 0f, hidden: true));
				v.Add(new Voltage("Voltage #5", 4, hidden: true));
				v.Add(new Voltage("Voltage #6", 5, hidden: true));
				v.Add(new Voltage("Voltage #7", 6, hidden: true));
				v.Add(new Voltage("Voltage #8", 7, hidden: true));
				v.Add(new Voltage("VBat", 8));
				for (int num2 = 0; num2 < superIO.Temperatures.Length; num2++)
				{
					t.Add(new Temperature("Temperature #" + (num2 + 1), num2));
				}
				for (int num3 = 0; num3 < superIO.Fans.Length; num3++)
				{
					f.Add(new Fan("Fan #" + (num3 + 1), num3));
				}
				for (int num4 = 0; num4 < superIO.Controls.Length; num4++)
				{
					c.Add(new Ctrl("Fan Control #" + (num4 + 1), num4));
				}
				break;
			}
			}
			break;
		default:
		{
			v.Add(new Voltage("CPU VCore", 0));
			v.Add(new Voltage("Voltage #2", 1, hidden: true));
			v.Add(new Voltage("Voltage #3", 2, hidden: true));
			v.Add(new Voltage("Voltage #4", 3, hidden: true));
			v.Add(new Voltage("Voltage #5", 4, hidden: true));
			v.Add(new Voltage("Voltage #6", 5, hidden: true));
			v.Add(new Voltage("Voltage #7", 6, hidden: true));
			v.Add(new Voltage("Voltage #8", 7, hidden: true));
			v.Add(new Voltage("VBat", 8));
			for (int i = 0; i < superIO.Temperatures.Length; i++)
			{
				t.Add(new Temperature("Temperature #" + (i + 1), i));
			}
			for (int j = 0; j < superIO.Fans.Length; j++)
			{
				f.Add(new Fan("Fan #" + (j + 1), j));
			}
			for (int k = 0; k < superIO.Controls.Length; k++)
			{
				c.Add(new Ctrl("Fan Control #" + (k + 1), k));
			}
			break;
		}
		}
	}

	private static void GetASRockConfiguration(ISuperIO superIO, IList<Voltage> v, IList<Temperature> t, IList<Fan> f, ref ReadValueDelegate readFan, ref UpdateDelegate postUpdate, ref Mutex mutex)
	{
		v.Add(new Voltage("CPU VCore", 0));
		v.Add(new Voltage("+3.3V", 2));
		v.Add(new Voltage("+12V", 4, 30f, 10f));
		v.Add(new Voltage("+5V", 5, 6.8f, 10f));
		v.Add(new Voltage("VBat", 8));
		t.Add(new Temperature("CPU", 0));
		t.Add(new Temperature("Motherboard", 1));
		f.Add(new Fan("CPU Fan", 0));
		f.Add(new Fan("Chassis Fan #1", 1));
		mutex = new Mutex(initiallyOwned: false, "ASRockOCMark");
		bool flag = false;
		try
		{
			flag = mutex.WaitOne(10, exitContext: false);
		}
		catch (AbandonedMutexException)
		{
		}
		catch (InvalidOperationException)
		{
		}
		if (!flag)
		{
			return;
		}
		f.Add(new Fan("Chassis Fan #2", 2));
		f.Add(new Fan("Chassis Fan #3", 3));
		f.Add(new Fan("Power Fan", 4));
		readFan = (int index) =>
		{
			if (index < 2)
			{
				return superIO.Fans[index];
			}
			byte? b = superIO.ReadGPIO(7);
			if (!b.HasValue)
			{
				return (float?)null;
			}
			int[] array = new int[3] { 5, 3, 6 };
			return (((b.Value >> 3) & 7) != array[index - 2]) ? ((float?)null) : superIO.Fans[2];
		};
		int fanIndex = 0;
		postUpdate = () =>
		{
			byte? b = superIO.ReadGPIO(7);
			if (b.HasValue)
			{
				int[] array = new int[3] { 5, 3, 6 };
				superIO.WriteGPIO(7, (byte)((b.Value & 0xC7) | (array[fanIndex] << 3)));
				fanIndex = (fanIndex + 1) % 3;
			}
		};
	}

	private static void GetITEConfigurationsB(ISuperIO superIO, Manufacturer manufacturer, Model model, IList<Voltage> v, IList<Temperature> t, IList<Fan> f, IList<Ctrl> c)
	{
		switch (manufacturer)
		{
		case Manufacturer.ECS:
		{
			if (model == Model.A890GXM_A)
			{
				v.Add(new Voltage("CPU VCore", 0));
				v.Add(new Voltage("VDIMM", 1));
				v.Add(new Voltage("NB Voltage", 2));
				v.Add(new Voltage("Analog +3.3V", 3, 10f, 10f));
				v.Add(new Voltage("Standby +3.3V", 7, 10f, 10f));
				v.Add(new Voltage("VBat", 8, 10f, 10f));
				t.Add(new Temperature("CPU", 0));
				t.Add(new Temperature("System", 1));
				t.Add(new Temperature("Northbridge", 2));
				f.Add(new Fan("CPU Fan", 0));
				f.Add(new Fan("System Fan", 1));
				f.Add(new Fan("Power Fan", 2));
				break;
			}
			v.Add(new Voltage("Voltage #1", 0, hidden: true));
			v.Add(new Voltage("Voltage #2", 1, hidden: true));
			v.Add(new Voltage("Voltage #3", 2, hidden: true));
			v.Add(new Voltage("Analog +3.3V", 3, 10f, 10f, 0f, hidden: true));
			v.Add(new Voltage("Voltage #5", 4, hidden: true));
			v.Add(new Voltage("Voltage #6", 5, hidden: true));
			v.Add(new Voltage("Voltage #7", 6, hidden: true));
			v.Add(new Voltage("Standby +3.3V", 7, 10f, 10f, 0f, hidden: true));
			v.Add(new Voltage("VBat", 8, 10f, 10f));
			for (int l = 0; l < superIO.Temperatures.Length; l++)
			{
				t.Add(new Temperature("Temperature #" + (l + 1), l));
			}
			for (int m = 0; m < superIO.Fans.Length; m++)
			{
				f.Add(new Fan("Fan #" + (m + 1), m));
			}
			for (int n = 0; n < superIO.Controls.Length; n++)
			{
				c.Add(new Ctrl("Fan Control #" + (n + 1), n));
			}
			break;
		}
		case Manufacturer.Gigabyte:
			switch (model)
			{
			case Model.H61M_DS2_REV_1_2:
			case Model.H61M_USB3_B3_REV_2_0:
				v.Add(new Voltage("VTT", 0));
				v.Add(new Voltage("+12V", 2, 30.9f, 10f));
				v.Add(new Voltage("CPU VCore", 5));
				v.Add(new Voltage("DRAM", 6));
				v.Add(new Voltage("Standby +3.3V", 7, 10f, 10f));
				v.Add(new Voltage("VBat", 8, 10f, 10f));
				t.Add(new Temperature("System", 0));
				t.Add(new Temperature("CPU", 2));
				f.Add(new Fan("CPU Fan", 0));
				f.Add(new Fan("System Fan", 1));
				break;
			case Model.H67A_UD3H_B3:
			case Model.H67A_USB3_B3:
				v.Add(new Voltage("VTT", 0));
				v.Add(new Voltage("+5V", 1, 15f, 10f));
				v.Add(new Voltage("+12V", 2, 30.9f, 10f));
				v.Add(new Voltage("CPU VCore", 5));
				v.Add(new Voltage("DRAM", 6));
				v.Add(new Voltage("Standby +3.3V", 7, 10f, 10f));
				v.Add(new Voltage("VBat", 8, 10f, 10f));
				t.Add(new Temperature("System", 0));
				t.Add(new Temperature("CPU", 2));
				f.Add(new Fan("CPU Fan", 0));
				f.Add(new Fan("System Fan #1", 1));
				f.Add(new Fan("Power Fan", 2));
				f.Add(new Fan("System Fan #2", 3));
				break;
			case Model.X570_AORUS_MASTER:
				v.Add(new Voltage("CPU VCore", 0));
				v.Add(new Voltage("+3.3V", 1, 29.4f, 45.3f));
				v.Add(new Voltage("+12V", 2, 10f, 2f));
				v.Add(new Voltage("+5V", 3, 15f, 10f));
				v.Add(new Voltage("CPU VCore SOC", 4));
				v.Add(new Voltage("CPU VDDP", 5));
				v.Add(new Voltage("DRAM CH(A/B)", 6));
				v.Add(new Voltage("Standby +3.3V", 7, 1f, 1f));
				v.Add(new Voltage("VBat", 8, 1f, 1f));
				t.Add(new Temperature("System 1", 0));
				t.Add(new Temperature("EC_TEMP1", 1));
				t.Add(new Temperature("CPU", 2));
				t.Add(new Temperature("PCIEX16", 3));
				t.Add(new Temperature("VRM MOS", 4));
				t.Add(new Temperature("PCH", 5));
				f.Add(new Fan("CPU Fan", 0));
				f.Add(new Fan("System 1 Fan", 1));
				f.Add(new Fan("System 2 Fan", 2));
				f.Add(new Fan("PCH Fan", 3));
				f.Add(new Fan("CPU OPT Fan", 4));
				c.Add(new Ctrl("CPU Fan", 0));
				c.Add(new Ctrl("System 1 Fan", 1));
				c.Add(new Ctrl("System 2 Fan", 2));
				c.Add(new Ctrl("PCH Fan", 3));
				c.Add(new Ctrl("CPU OPT Fan", 4));
				break;
			case Model.Z390_M_GAMING:
			case Model.Z390_AORUS_ULTRA:
			case Model.Z390_UD:
				v.Add(new Voltage("CPU VCore", 0));
				v.Add(new Voltage("+3.3V", 1, 6.49f, 10f));
				v.Add(new Voltage("+12V", 2, 5f, 1f));
				v.Add(new Voltage("+5V", 3, 1.5f, 1f));
				v.Add(new Voltage("CPU VCCGT", 4));
				v.Add(new Voltage("CPU VCCSA", 5));
				v.Add(new Voltage("VDDQ", 6));
				v.Add(new Voltage("DDRVTT", 7));
				v.Add(new Voltage("PCHCore", 8));
				t.Add(new Temperature("System1", 0));
				t.Add(new Temperature("PCH", 1));
				t.Add(new Temperature("CPU", 2));
				t.Add(new Temperature("PCIEX16", 3));
				t.Add(new Temperature("VRM MOS", 4));
				t.Add(new Temperature("System2", 5));
				f.Add(new Fan("CPU Fan", 0));
				f.Add(new Fan("System Fan #1", 1));
				f.Add(new Fan("System Fan #2", 2));
				f.Add(new Fan("System Fan #3", 3));
				break;
			case Model.Z68A_D3H_B3:
				v.Add(new Voltage("VTT", 0));
				v.Add(new Voltage("+3.3V", 1, 6.49f, 10f));
				v.Add(new Voltage("+12V", 2, 30.9f, 10f));
				v.Add(new Voltage("+5V", 3, 7.15f, 10f));
				v.Add(new Voltage("CPU VCore", 5));
				v.Add(new Voltage("DRAM", 6));
				v.Add(new Voltage("Standby +3.3V", 7, 10f, 10f));
				v.Add(new Voltage("VBat", 8, 10f, 10f));
				t.Add(new Temperature("System", 0));
				t.Add(new Temperature("CPU", 2));
				f.Add(new Fan("CPU Fan", 0));
				f.Add(new Fan("System Fan #1", 1));
				f.Add(new Fan("Power Fan", 2));
				f.Add(new Fan("System Fan #2", 3));
				break;
			case Model.P67A_UD3_B3:
			case Model.P67A_UD3R_B3:
			case Model.P67A_UD4_B3:
			case Model.Z68AP_D3:
			case Model.Z68X_UD3H_B3:
				v.Add(new Voltage("VTT", 0));
				v.Add(new Voltage("+3.3V", 1, 6.49f, 10f));
				v.Add(new Voltage("+12V", 2, 30.9f, 10f));
				v.Add(new Voltage("+5V", 3, 7.15f, 10f));
				v.Add(new Voltage("CPU VCore", 5));
				v.Add(new Voltage("DRAM", 6));
				v.Add(new Voltage("Standby +3.3V", 7, 10f, 10f));
				v.Add(new Voltage("VBat", 8, 10f, 10f));
				t.Add(new Temperature("System", 0));
				t.Add(new Temperature("CPU", 2));
				f.Add(new Fan("CPU Fan", 0));
				f.Add(new Fan("System Fan #2", 1));
				f.Add(new Fan("Power Fan", 2));
				f.Add(new Fan("System Fan #1", 3));
				break;
			case Model.Z68X_UD7_B3:
				v.Add(new Voltage("VTT", 0));
				v.Add(new Voltage("+3.3V", 1, 6.49f, 10f));
				v.Add(new Voltage("+12V", 2, 30.9f, 10f));
				v.Add(new Voltage("+5V", 3, 7.15f, 10f));
				v.Add(new Voltage("CPU VCore", 5));
				v.Add(new Voltage("DRAM", 6));
				v.Add(new Voltage("Standby +3.3V", 7, 10f, 10f));
				v.Add(new Voltage("VBat", 8, 10f, 10f));
				t.Add(new Temperature("System", 0));
				t.Add(new Temperature("CPU", 1));
				t.Add(new Temperature("System 3", 2));
				f.Add(new Fan("CPU Fan", 0));
				f.Add(new Fan("Power Fan", 1));
				f.Add(new Fan("System Fan #1", 2));
				f.Add(new Fan("System Fan #2", 3));
				f.Add(new Fan("System Fan #3", 4));
				break;
			default:
			{
				v.Add(new Voltage("Voltage #1", 0, hidden: true));
				v.Add(new Voltage("Voltage #2", 1, hidden: true));
				v.Add(new Voltage("Voltage #3", 2, hidden: true));
				v.Add(new Voltage("Voltage #4", 3, hidden: true));
				v.Add(new Voltage("Voltage #5", 4, hidden: true));
				v.Add(new Voltage("Voltage #6", 5, hidden: true));
				v.Add(new Voltage("Voltage #7", 6, hidden: true));
				v.Add(new Voltage("Standby +3.3V", 7, 10f, 10f, 0f, hidden: true));
				v.Add(new Voltage("VBat", 8, 10f, 10f));
				for (int num = 0; num < superIO.Temperatures.Length; num++)
				{
					t.Add(new Temperature("Temperature #" + (num + 1), num));
				}
				for (int num2 = 0; num2 < superIO.Fans.Length; num2++)
				{
					f.Add(new Fan("Fan #" + (num2 + 1), num2));
				}
				for (int num3 = 0; num3 < superIO.Controls.Length; num3++)
				{
					c.Add(new Ctrl("Fan Control #" + (num3 + 1), num3));
				}
				break;
			}
			}
			break;
		case Manufacturer.Shuttle:
		{
			if (model == Model.FH67)
			{
				v.Add(new Voltage("CPU VCore", 0));
				v.Add(new Voltage("DRAM", 1));
				v.Add(new Voltage("PCH VCCIO", 2));
				v.Add(new Voltage("CPU VCCIO", 3));
				v.Add(new Voltage("Graphic Voltage", 4));
				v.Add(new Voltage("Standby +3.3V", 7, 10f, 10f));
				v.Add(new Voltage("VBat", 8, 10f, 10f));
				t.Add(new Temperature("System", 0));
				t.Add(new Temperature("CPU", 1));
				f.Add(new Fan("Fan #1", 0));
				f.Add(new Fan("CPU Fan", 1));
				break;
			}
			v.Add(new Voltage("Voltage #1", 0, hidden: true));
			v.Add(new Voltage("Voltage #2", 1, hidden: true));
			v.Add(new Voltage("Voltage #3", 2, hidden: true));
			v.Add(new Voltage("Voltage #4", 3, hidden: true));
			v.Add(new Voltage("Voltage #5", 4, hidden: true));
			v.Add(new Voltage("Voltage #6", 5, hidden: true));
			v.Add(new Voltage("Voltage #7", 6, hidden: true));
			v.Add(new Voltage("Standby +3.3V", 7, 10f, 10f, 0f, hidden: true));
			v.Add(new Voltage("VBat", 8, 10f, 10f));
			for (int num4 = 0; num4 < superIO.Temperatures.Length; num4++)
			{
				t.Add(new Temperature("Temperature #" + (num4 + 1), num4));
			}
			for (int num5 = 0; num5 < superIO.Fans.Length; num5++)
			{
				f.Add(new Fan("Fan #" + (num5 + 1), num5));
			}
			for (int num6 = 0; num6 < superIO.Controls.Length; num6++)
			{
				c.Add(new Ctrl("Fan Control #" + (num6 + 1), num6));
			}
			break;
		}
		default:
		{
			v.Add(new Voltage("Voltage #1", 0, hidden: true));
			v.Add(new Voltage("Voltage #2", 1, hidden: true));
			v.Add(new Voltage("Voltage #3", 2, hidden: true));
			v.Add(new Voltage("Voltage #4", 3, hidden: true));
			v.Add(new Voltage("Voltage #5", 4, hidden: true));
			v.Add(new Voltage("Voltage #6", 5, hidden: true));
			v.Add(new Voltage("Voltage #7", 6, hidden: true));
			v.Add(new Voltage("Standby +3.3V", 7, 10f, 10f, 0f, hidden: true));
			v.Add(new Voltage("VBat", 8, 10f, 10f));
			for (int i = 0; i < superIO.Temperatures.Length; i++)
			{
				t.Add(new Temperature("Temperature #" + (i + 1), i));
			}
			for (int j = 0; j < superIO.Fans.Length; j++)
			{
				f.Add(new Fan("Fan #" + (j + 1), j));
			}
			for (int k = 0; k < superIO.Controls.Length; k++)
			{
				c.Add(new Ctrl("Fan Control #" + (k + 1), k));
			}
			break;
		}
		}
	}

	private static void GetITEConfigurationsC(ISuperIO superIO, Manufacturer manufacturer, Model model, IList<Voltage> v, IList<Temperature> t, IList<Fan> f, IList<Ctrl> c)
	{
		if (manufacturer == Manufacturer.Gigabyte)
		{
			if (model == Model.X570_AORUS_MASTER)
			{
				v.Add(new Voltage("CPU VDD18", 0));
				v.Add(new Voltage("DDRVTT CH(A/B)", 1));
				v.Add(new Voltage("Chipset Core", 2));
				v.Add(new Voltage("Voltage #4", 3, hidden: true));
				v.Add(new Voltage("CPU VDD18", 4));
				v.Add(new Voltage("PM_CLDO12", 5));
				v.Add(new Voltage("Voltage #7", 6, hidden: true));
				v.Add(new Voltage("Standby +3.3V", 7, 1f, 1f));
				v.Add(new Voltage("VBat", 8, 1f, 1f));
				t.Add(new Temperature("PCIEX8", 0));
				t.Add(new Temperature("EC_TEMP2", 1));
				t.Add(new Temperature("System 2", 2));
				f.Add(new Fan("System 5 Pump", 0));
				f.Add(new Fan("System 6 Pump", 1));
				f.Add(new Fan("System 4 Fan", 2));
				return;
			}
			v.Add(new Voltage("Voltage #1", 0, hidden: true));
			v.Add(new Voltage("Voltage #2", 1, hidden: true));
			v.Add(new Voltage("Voltage #3", 2, hidden: true));
			v.Add(new Voltage("Voltage #4", 3, hidden: true));
			v.Add(new Voltage("Voltage #5", 4, hidden: true));
			v.Add(new Voltage("Voltage #6", 5, hidden: true));
			v.Add(new Voltage("Voltage #7", 6, hidden: true));
			v.Add(new Voltage("Standby +3.3V", 7, 10f, 10f, 0f, hidden: true));
			v.Add(new Voltage("VBat", 8, 10f, 10f));
			for (int i = 0; i < superIO.Temperatures.Length; i++)
			{
				t.Add(new Temperature("Temperature #" + (i + 1), i));
			}
			for (int j = 0; j < superIO.Fans.Length; j++)
			{
				f.Add(new Fan("Fan #" + (j + 1), j));
			}
			for (int k = 0; k < superIO.Controls.Length; k++)
			{
				c.Add(new Ctrl("Fan Control #" + (k + 1), k));
			}
		}
		else
		{
			v.Add(new Voltage("Voltage #1", 0, hidden: true));
			v.Add(new Voltage("Voltage #2", 1, hidden: true));
			v.Add(new Voltage("Voltage #3", 2, hidden: true));
			v.Add(new Voltage("Voltage #4", 3, hidden: true));
			v.Add(new Voltage("Voltage #5", 4, hidden: true));
			v.Add(new Voltage("Voltage #6", 5, hidden: true));
			v.Add(new Voltage("Voltage #7", 6, hidden: true));
			v.Add(new Voltage("Standby +3.3V", 7, 10f, 10f, 0f, hidden: true));
			v.Add(new Voltage("VBat", 8, 10f, 10f));
			for (int l = 0; l < superIO.Temperatures.Length; l++)
			{
				t.Add(new Temperature("Temperature #" + (l + 1), l));
			}
			for (int m = 0; m < superIO.Fans.Length; m++)
			{
				f.Add(new Fan("Fan #" + (m + 1), m));
			}
			for (int n = 0; n < superIO.Controls.Length; n++)
			{
				c.Add(new Ctrl("Fan Control #" + (n + 1), n));
			}
		}
	}

	private static void GetFintekConfiguration(ISuperIO superIO, Manufacturer manufacturer, Model model, IList<Voltage> v, IList<Temperature> t, IList<Fan> f, IList<Ctrl> c)
	{
		if (manufacturer == Manufacturer.EVGA)
		{
			if (model == Model.X58_SLI_Classified)
			{
				v.Add(new Voltage("VCC3V", 0, 150f, 150f));
				v.Add(new Voltage("CPU VCore", 1, 47f, 100f));
				v.Add(new Voltage("DIMM", 2, 47f, 100f));
				v.Add(new Voltage("CPU VTT", 3, 24f, 100f));
				v.Add(new Voltage("IOH Vcore", 4, 24f, 100f));
				v.Add(new Voltage("+5V", 5, 51f, 12f));
				v.Add(new Voltage("+12V", 6, 56f, 6.8f));
				v.Add(new Voltage("3VSB", 7, 150f, 150f));
				v.Add(new Voltage("VBat", 8, 150f, 150f));
				t.Add(new Temperature("CPU", 0));
				t.Add(new Temperature("VREG", 1));
				t.Add(new Temperature("System", 2));
				f.Add(new Fan("CPU Fan", 0));
				f.Add(new Fan("Power Fan", 1));
				f.Add(new Fan("Chassis Fan", 2));
			}
			else
			{
				v.Add(new Voltage("VCC3V", 0, 150f, 150f));
				v.Add(new Voltage("CPU VCore", 1));
				v.Add(new Voltage("Voltage #3", 2, hidden: true));
				v.Add(new Voltage("Voltage #4", 3, hidden: true));
				v.Add(new Voltage("Voltage #5", 4, hidden: true));
				v.Add(new Voltage("Voltage #6", 5, hidden: true));
				v.Add(new Voltage("Voltage #7", 6, hidden: true));
				v.Add(new Voltage("VSB3V", 7, 150f, 150f));
				v.Add(new Voltage("VBat", 8, 150f, 150f));
				for (int i = 0; i < superIO.Temperatures.Length; i++)
				{
					t.Add(new Temperature("Temperature #" + (i + 1), i));
				}
				for (int j = 0; j < superIO.Fans.Length; j++)
				{
					f.Add(new Fan("Fan #" + (j + 1), j));
				}
			}
		}
		else
		{
			v.Add(new Voltage("VCC3V", 0, 150f, 150f));
			v.Add(new Voltage("CPU VCore", 1));
			v.Add(new Voltage("Voltage #3", 2, hidden: true));
			v.Add(new Voltage("Voltage #4", 3, hidden: true));
			v.Add(new Voltage("Voltage #5", 4, hidden: true));
			v.Add(new Voltage("Voltage #6", 5, hidden: true));
			if (superIO.Chip != Chip.F71808E)
			{
				v.Add(new Voltage("Voltage #7", 6, hidden: true));
			}
			v.Add(new Voltage("VSB3V", 7, 150f, 150f));
			v.Add(new Voltage("VBat", 8, 150f, 150f));
			for (int k = 0; k < superIO.Temperatures.Length; k++)
			{
				t.Add(new Temperature("Temperature #" + (k + 1), k));
			}
			for (int l = 0; l < superIO.Fans.Length; l++)
			{
				f.Add(new Fan("Fan #" + (l + 1), l));
			}
			for (int m = 0; m < superIO.Controls.Length; m++)
			{
				c.Add(new Ctrl("Fan Control #" + (m + 1), m));
			}
		}
	}

	private static void GetNuvotonConfigurationF(ISuperIO superIO, Manufacturer manufacturer, Model model, IList<Voltage> v, IList<Temperature> t, IList<Fan> f, IList<Ctrl> c)
	{
		if (manufacturer == Manufacturer.ASUS)
		{
			switch (model)
			{
			case Model.P8P67:
			case Model.P8P67_EVO:
			case Model.P8P67_PRO:
				v.Add(new Voltage("CPU VCore", 0));
				v.Add(new Voltage("+12V", 1, 11f, 1f));
				v.Add(new Voltage("Analog +3.3V", 2, 34f, 34f));
				v.Add(new Voltage("+3.3V", 3, 34f, 34f));
				v.Add(new Voltage("+5V", 4, 12f, 3f));
				v.Add(new Voltage("Standby +3.3V", 7, 34f, 34f));
				v.Add(new Voltage("VBAT", 8, 34f, 34f));
				t.Add(new Temperature("CPU", 0));
				t.Add(new Temperature("Auxiliary", 2));
				t.Add(new Temperature("Motherboard", 3));
				f.Add(new Fan("Chassis Fan #1", 0));
				f.Add(new Fan("CPU Fan", 1));
				f.Add(new Fan("Power Fan", 2));
				f.Add(new Fan("Chassis Fan #2", 3));
				c.Add(new Ctrl("Chassis Fan #2", 0));
				c.Add(new Ctrl("CPU Fan", 1));
				c.Add(new Ctrl("Chassis Fan #1", 2));
				return;
			case Model.P8P67_M_PRO:
				v.Add(new Voltage("CPU VCore", 0));
				v.Add(new Voltage("+12V", 1, 11f, 1f));
				v.Add(new Voltage("Analog +3.3V", 2, 34f, 34f));
				v.Add(new Voltage("+3.3V", 3, 34f, 34f));
				v.Add(new Voltage("+5V", 4, 12f, 3f));
				v.Add(new Voltage("Voltage #6", 5, hidden: true));
				v.Add(new Voltage("Voltage #7", 6, hidden: true));
				v.Add(new Voltage("Standby +3.3V", 7, 34f, 34f));
				v.Add(new Voltage("VBAT", 8, 34f, 34f));
				t.Add(new Temperature("CPU", 0));
				t.Add(new Temperature("Motherboard", 3));
				f.Add(new Fan("Chassis Fan #1", 0));
				f.Add(new Fan("CPU Fan", 1));
				f.Add(new Fan("Chassis Fan #2", 2));
				f.Add(new Fan("Power Fan", 3));
				f.Add(new Fan("Auxiliary Fan", 4));
				return;
			case Model.P8Z68_V_PRO:
			{
				v.Add(new Voltage("CPU VCore", 0));
				v.Add(new Voltage("+12V", 1, 11f, 1f));
				v.Add(new Voltage("Analog +3.3V", 2, 34f, 34f));
				v.Add(new Voltage("+3.3V", 3, 34f, 34f));
				v.Add(new Voltage("+5V", 4, 12f, 3f));
				v.Add(new Voltage("Standby +3.3V", 7, 34f, 34f));
				v.Add(new Voltage("VBAT", 8, 34f, 34f));
				t.Add(new Temperature("CPU", 0));
				t.Add(new Temperature("Auxiliary", 2));
				t.Add(new Temperature("Motherboard", 3));
				for (int i = 0; i < superIO.Fans.Length; i++)
				{
					f.Add(new Fan("Fan #" + (i + 1), i));
				}
				for (int j = 0; j < superIO.Controls.Length; j++)
				{
					c.Add(new Ctrl("Fan #" + (j + 1), j));
				}
				return;
			}
			case Model.P9X79:
			{
				v.Add(new Voltage("CPU VCore", 0));
				v.Add(new Voltage("+12V", 1, 11f, 1f));
				v.Add(new Voltage("Analog +3.3V", 2, 34f, 34f));
				v.Add(new Voltage("+3.3V", 3, 34f, 34f));
				v.Add(new Voltage("+5V", 4, 12f, 3f));
				v.Add(new Voltage("Standby +3.3V", 7, 34f, 34f));
				v.Add(new Voltage("VBAT", 8, 34f, 34f));
				t.Add(new Temperature("CPU", 0));
				t.Add(new Temperature("Motherboard", 3));
				for (int k = 0; k < superIO.Fans.Length; k++)
				{
					f.Add(new Fan("Fan #" + (k + 1), k));
				}
				for (int l = 0; l < superIO.Controls.Length; l++)
				{
					c.Add(new Ctrl("Fan Control #" + (l + 1), l));
				}
				return;
			}
			}
			v.Add(new Voltage("CPU VCore", 0));
			v.Add(new Voltage("Voltage #2", 1, hidden: true));
			v.Add(new Voltage("AVCC", 2, 34f, 34f));
			v.Add(new Voltage("3VCC", 3, 34f, 34f));
			v.Add(new Voltage("Voltage #5", 4, hidden: true));
			v.Add(new Voltage("Voltage #6", 5, hidden: true));
			v.Add(new Voltage("Voltage #7", 6, hidden: true));
			v.Add(new Voltage("3VSB", 7, 34f, 34f));
			v.Add(new Voltage("VBAT", 8, 34f, 34f));
			t.Add(new Temperature("CPU Core", 0));
			t.Add(new Temperature("Temperature #1", 1));
			t.Add(new Temperature("Temperature #2", 2));
			t.Add(new Temperature("Temperature #3", 3));
			for (int m = 0; m < superIO.Fans.Length; m++)
			{
				f.Add(new Fan("Fan #" + (m + 1), m));
			}
			for (int n = 0; n < superIO.Controls.Length; n++)
			{
				c.Add(new Ctrl("Fan Control #" + (n + 1), n));
			}
		}
		else
		{
			v.Add(new Voltage("CPU VCore", 0));
			v.Add(new Voltage("Voltage #2", 1, hidden: true));
			v.Add(new Voltage("AVCC", 2, 34f, 34f));
			v.Add(new Voltage("3VCC", 3, 34f, 34f));
			v.Add(new Voltage("Voltage #5", 4, hidden: true));
			v.Add(new Voltage("Voltage #6", 5, hidden: true));
			v.Add(new Voltage("Voltage #7", 6, hidden: true));
			v.Add(new Voltage("3VSB", 7, 34f, 34f));
			v.Add(new Voltage("VBAT", 8, 34f, 34f));
			t.Add(new Temperature("CPU Core", 0));
			t.Add(new Temperature("Temperature #1", 1));
			t.Add(new Temperature("Temperature #2", 2));
			t.Add(new Temperature("Temperature #3", 3));
			for (int num = 0; num < superIO.Fans.Length; num++)
			{
				f.Add(new Fan("Fan #" + (num + 1), num));
			}
			for (int num2 = 0; num2 < superIO.Controls.Length; num2++)
			{
				c.Add(new Ctrl("Fan Control #" + (num2 + 1), num2));
			}
		}
	}

	private static void GetNuvotonConfigurationD(ISuperIO superIO, Manufacturer manufacturer, Model model, IList<Voltage> v, IList<Temperature> t, IList<Fan> f, IList<Ctrl> c)
	{
		if (manufacturer == Manufacturer.ASUS)
		{
			if (model == Model.P8Z77_V)
			{
				v.Add(new Voltage("CPU VCore", 0));
				v.Add(new Voltage("Voltage #2", 1, hidden: true));
				v.Add(new Voltage("AVCC", 2, 34f, 34f));
				v.Add(new Voltage("3VCC", 3, 34f, 34f));
				v.Add(new Voltage("Voltage #5", 4, hidden: true));
				v.Add(new Voltage("Voltage #6", 5, hidden: true));
				v.Add(new Voltage("Voltage #7", 6, hidden: true));
				v.Add(new Voltage("3VSB", 7, 34f, 34f));
				v.Add(new Voltage("VBAT", 8, 34f, 34f));
				v.Add(new Voltage("VTT", 9));
				v.Add(new Voltage("Voltage #11", 10, hidden: true));
				v.Add(new Voltage("Voltage #12", 11, hidden: true));
				v.Add(new Voltage("Voltage #13", 12, hidden: true));
				v.Add(new Voltage("Voltage #14", 13, hidden: true));
				v.Add(new Voltage("Voltage #15", 14, hidden: true));
				t.Add(new Temperature("CPU Core", 0));
				t.Add(new Temperature("Auxiliary", 1));
				t.Add(new Temperature("Motherboard", 2));
				f.Add(new Fan("Chassis Fan #1", 0));
				f.Add(new Fan("CPU Fan", 1));
				f.Add(new Fan("Chassis Fan #2", 2));
				f.Add(new Fan("Chassis Fan #3", 3));
				c.Add(new Ctrl("Chassis Fan #1", 0));
				c.Add(new Ctrl("CPU  Fan", 1));
				c.Add(new Ctrl("Chassis Fan #2", 2));
				c.Add(new Ctrl("Chassis Fan #3", 3));
				return;
			}
			v.Add(new Voltage("CPU VCore", 0));
			v.Add(new Voltage("Voltage #2", 1, hidden: true));
			v.Add(new Voltage("AVCC", 2, 34f, 34f));
			v.Add(new Voltage("3VCC", 3, 34f, 34f));
			v.Add(new Voltage("Voltage #5", 4, hidden: true));
			v.Add(new Voltage("Voltage #6", 5, hidden: true));
			v.Add(new Voltage("Voltage #7", 6, hidden: true));
			v.Add(new Voltage("3VSB", 7, 34f, 34f));
			v.Add(new Voltage("VBAT", 8, 34f, 34f));
			v.Add(new Voltage("VTT", 9));
			v.Add(new Voltage("Voltage #11", 10, hidden: true));
			v.Add(new Voltage("Voltage #12", 11, hidden: true));
			v.Add(new Voltage("Voltage #13", 12, hidden: true));
			v.Add(new Voltage("Voltage #14", 13, hidden: true));
			v.Add(new Voltage("Voltage #15", 14, hidden: true));
			t.Add(new Temperature("CPU Core", 0));
			t.Add(new Temperature("Temperature #1", 1));
			t.Add(new Temperature("Temperature #2", 2));
			t.Add(new Temperature("Temperature #3", 3));
			t.Add(new Temperature("Temperature #4", 4));
			t.Add(new Temperature("Temperature #5", 5));
			t.Add(new Temperature("Temperature #6", 6));
			for (int i = 0; i < superIO.Fans.Length; i++)
			{
				f.Add(new Fan("Fan #" + (i + 1), i));
			}
			for (int j = 0; j < superIO.Controls.Length; j++)
			{
				c.Add(new Ctrl("Fan Control #" + (j + 1), j));
			}
		}
		else
		{
			v.Add(new Voltage("CPU VCore", 0));
			v.Add(new Voltage("Voltage #2", 1, hidden: true));
			v.Add(new Voltage("AVCC", 2, 34f, 34f));
			v.Add(new Voltage("3VCC", 3, 34f, 34f));
			v.Add(new Voltage("Voltage #5", 4, hidden: true));
			v.Add(new Voltage("Voltage #6", 5, hidden: true));
			v.Add(new Voltage("Voltage #7", 6, hidden: true));
			v.Add(new Voltage("3VSB", 7, 34f, 34f));
			v.Add(new Voltage("VBAT", 8, 34f, 34f));
			v.Add(new Voltage("VTT", 9));
			v.Add(new Voltage("Voltage #11", 10, hidden: true));
			v.Add(new Voltage("Voltage #12", 11, hidden: true));
			v.Add(new Voltage("Voltage #13", 12, hidden: true));
			v.Add(new Voltage("Voltage #14", 13, hidden: true));
			v.Add(new Voltage("Voltage #15", 14, hidden: true));
			t.Add(new Temperature("CPU Core", 0));
			t.Add(new Temperature("Temperature #1", 1));
			t.Add(new Temperature("Temperature #2", 2));
			t.Add(new Temperature("Temperature #3", 3));
			t.Add(new Temperature("Temperature #4", 4));
			t.Add(new Temperature("Temperature #5", 5));
			t.Add(new Temperature("Temperature #6", 6));
			for (int k = 0; k < superIO.Fans.Length; k++)
			{
				f.Add(new Fan("Fan #" + (k + 1), k));
			}
			for (int l = 0; l < superIO.Controls.Length; l++)
			{
				c.Add(new Ctrl("Fan Control #" + (l + 1), l));
			}
		}
	}

	private static void GetWinbondConfigurationEHF(Manufacturer manufacturer, Model model, IList<Voltage> v, IList<Temperature> t, IList<Fan> f)
	{
		if (manufacturer == Manufacturer.ASRock)
		{
			if (model == Model.AOD790GX_128M)
			{
				v.Add(new Voltage("CPU VCore", 0));
				v.Add(new Voltage("Analog +3.3V", 2, 34f, 34f));
				v.Add(new Voltage("+3.3V", 4, 10f, 10f));
				v.Add(new Voltage("+5V", 5, 20f, 10f));
				v.Add(new Voltage("+12V", 6, 28f, 5f));
				v.Add(new Voltage("Standby +3.3V", 7, 34f, 34f));
				v.Add(new Voltage("VBAT", 8, 34f, 34f));
				t.Add(new Temperature("CPU", 0));
				t.Add(new Temperature("Motherboard", 2));
				f.Add(new Fan("CPU Fan", 0));
				f.Add(new Fan("Chassis Fan", 1));
			}
			else
			{
				v.Add(new Voltage("CPU VCore", 0));
				v.Add(new Voltage("Voltage #2", 1, hidden: true));
				v.Add(new Voltage("AVCC", 2, 34f, 34f));
				v.Add(new Voltage("3VCC", 3, 34f, 34f));
				v.Add(new Voltage("Voltage #5", 4, hidden: true));
				v.Add(new Voltage("Voltage #6", 5, hidden: true));
				v.Add(new Voltage("Voltage #7", 6, hidden: true));
				v.Add(new Voltage("3VSB", 7, 34f, 34f));
				v.Add(new Voltage("VBAT", 8, 34f, 34f));
				v.Add(new Voltage("Voltage #10", 9, hidden: true));
				t.Add(new Temperature("CPU", 0));
				t.Add(new Temperature("Auxiliary", 1));
				t.Add(new Temperature("System", 2));
				f.Add(new Fan("System Fan", 0));
				f.Add(new Fan("CPU Fan", 1));
				f.Add(new Fan("Auxiliary Fan", 2));
				f.Add(new Fan("CPU Fan #2", 3));
				f.Add(new Fan("Auxiliary Fan #2", 4));
			}
		}
		else
		{
			v.Add(new Voltage("CPU VCore", 0));
			v.Add(new Voltage("Voltage #2", 1, hidden: true));
			v.Add(new Voltage("AVCC", 2, 34f, 34f));
			v.Add(new Voltage("3VCC", 3, 34f, 34f));
			v.Add(new Voltage("Voltage #5", 4, hidden: true));
			v.Add(new Voltage("Voltage #6", 5, hidden: true));
			v.Add(new Voltage("Voltage #7", 6, hidden: true));
			v.Add(new Voltage("3VSB", 7, 34f, 34f));
			v.Add(new Voltage("VBAT", 8, 34f, 34f));
			v.Add(new Voltage("Voltage #10", 9, hidden: true));
			t.Add(new Temperature("CPU", 0));
			t.Add(new Temperature("Auxiliary", 1));
			t.Add(new Temperature("System", 2));
			f.Add(new Fan("System Fan", 0));
			f.Add(new Fan("CPU Fan", 1));
			f.Add(new Fan("Auxiliary Fan", 2));
			f.Add(new Fan("CPU Fan #2", 3));
			f.Add(new Fan("Auxiliary Fan #2", 4));
		}
	}

	private static void GetWinbondConfigurationHG(Manufacturer manufacturer, Model model, IList<Voltage> v, IList<Temperature> t, IList<Fan> f)
	{
		switch (manufacturer)
		{
		case Manufacturer.ASRock:
			if (model == Model._880GMH_USB3)
			{
				v.Add(new Voltage("CPU VCore", 0));
				v.Add(new Voltage("+3.3V", 3, 34f, 34f));
				v.Add(new Voltage("+5V", 5, 15f, 7.5f));
				v.Add(new Voltage("+12V", 6, 56f, 10f));
				v.Add(new Voltage("Standby +3.3V", 7, 34f, 34f));
				v.Add(new Voltage("VBAT", 8, 34f, 34f));
				t.Add(new Temperature("CPU", 0));
				t.Add(new Temperature("Motherboard", 2));
				f.Add(new Fan("Chassis Fan", 0));
				f.Add(new Fan("CPU Fan", 1));
				f.Add(new Fan("Power Fan", 2));
			}
			else
			{
				v.Add(new Voltage("CPU VCore", 0));
				v.Add(new Voltage("Voltage #2", 1, hidden: true));
				v.Add(new Voltage("AVCC", 2, 34f, 34f));
				v.Add(new Voltage("3VCC", 3, 34f, 34f));
				v.Add(new Voltage("Voltage #5", 4, hidden: true));
				v.Add(new Voltage("Voltage #6", 5, hidden: true));
				v.Add(new Voltage("Voltage #7", 6, hidden: true));
				v.Add(new Voltage("3VSB", 7, 34f, 34f));
				v.Add(new Voltage("VBAT", 8, 34f, 34f));
				t.Add(new Temperature("CPU", 0));
				t.Add(new Temperature("Auxiliary", 1));
				t.Add(new Temperature("System", 2));
				f.Add(new Fan("System Fan", 0));
				f.Add(new Fan("CPU Fan", 1));
				f.Add(new Fan("Auxiliary Fan", 2));
				f.Add(new Fan("CPU Fan #2", 3));
				f.Add(new Fan("Auxiliary Fan #2", 4));
			}
			break;
		case Manufacturer.ASUS:
			switch (model)
			{
			case Model.P6T:
			case Model.P6X58D_E:
			case Model.Rampage_II_GENE:
				v.Add(new Voltage("CPU VCore", 0));
				v.Add(new Voltage("+12V", 1, 11.5f, 1.91f));
				v.Add(new Voltage("Analog +3.3V", 2, 34f, 34f));
				v.Add(new Voltage("+3.3V", 3, 34f, 34f));
				v.Add(new Voltage("+5V", 4, 15f, 7.5f));
				v.Add(new Voltage("Standby +3.3V", 7, 34f, 34f));
				v.Add(new Voltage("VBAT", 8, 34f, 34f));
				t.Add(new Temperature("CPU", 0));
				t.Add(new Temperature("Motherboard", 2));
				f.Add(new Fan("Chassis Fan #1", 0));
				f.Add(new Fan("CPU Fan", 1));
				f.Add(new Fan("Power Fan", 2));
				f.Add(new Fan("Chassis Fan #2", 3));
				f.Add(new Fan("Chassis Fan #3", 4));
				break;
			case Model.Rampage_Extreme:
				v.Add(new Voltage("CPU VCore", 0));
				v.Add(new Voltage("+12V", 1, 12f, 2f));
				v.Add(new Voltage("Analog +3.3V", 2, 34f, 34f));
				v.Add(new Voltage("+3.3V", 3, 34f, 34f));
				v.Add(new Voltage("+5V", 4, 15f, 7.5f));
				v.Add(new Voltage("Standby +3.3V", 7, 34f, 34f));
				v.Add(new Voltage("VBAT", 8, 34f, 34f));
				t.Add(new Temperature("CPU", 0));
				t.Add(new Temperature("Motherboard", 2));
				f.Add(new Fan("Chassis Fan #1", 0));
				f.Add(new Fan("CPU Fan", 1));
				f.Add(new Fan("Power Fan", 2));
				f.Add(new Fan("Chassis Fan #2", 3));
				f.Add(new Fan("Chassis Fan #3", 4));
				break;
			default:
				v.Add(new Voltage("CPU VCore", 0));
				v.Add(new Voltage("Voltage #2", 1, hidden: true));
				v.Add(new Voltage("AVCC", 2, 34f, 34f));
				v.Add(new Voltage("3VCC", 3, 34f, 34f));
				v.Add(new Voltage("Voltage #5", 4, hidden: true));
				v.Add(new Voltage("Voltage #6", 5, hidden: true));
				v.Add(new Voltage("Voltage #7", 6, hidden: true));
				v.Add(new Voltage("3VSB", 7, 34f, 34f));
				v.Add(new Voltage("VBAT", 8, 34f, 34f));
				t.Add(new Temperature("CPU", 0));
				t.Add(new Temperature("Auxiliary", 1));
				t.Add(new Temperature("System", 2));
				f.Add(new Fan("System Fan", 0));
				f.Add(new Fan("CPU Fan", 1));
				f.Add(new Fan("Auxiliary Fan", 2));
				f.Add(new Fan("CPU Fan #2", 3));
				f.Add(new Fan("Auxiliary Fan #2", 4));
				break;
			}
			break;
		default:
			v.Add(new Voltage("CPU VCore", 0));
			v.Add(new Voltage("Voltage #2", 1, hidden: true));
			v.Add(new Voltage("AVCC", 2, 34f, 34f));
			v.Add(new Voltage("3VCC", 3, 34f, 34f));
			v.Add(new Voltage("Voltage #5", 4, hidden: true));
			v.Add(new Voltage("Voltage #6", 5, hidden: true));
			v.Add(new Voltage("Voltage #7", 6, hidden: true));
			v.Add(new Voltage("3VSB", 7, 34f, 34f));
			v.Add(new Voltage("VBAT", 8, 34f, 34f));
			t.Add(new Temperature("CPU", 0));
			t.Add(new Temperature("Auxiliary", 1));
			t.Add(new Temperature("System", 2));
			f.Add(new Fan("System Fan", 0));
			f.Add(new Fan("CPU Fan", 1));
			f.Add(new Fan("Auxiliary Fan", 2));
			f.Add(new Fan("CPU Fan #2", 3));
			f.Add(new Fan("Auxiliary Fan #2", 4));
			break;
		}
	}

	public override string GetReport()
	{
		return superIO.GetReport();
	}

	public override void Update()
	{
		superIO.Update();
		foreach (Sensor voltage in voltages)
		{
			float? num = readVoltage(voltage.Index);
			if (num.HasValue)
			{
				voltage.Value = num + (num - voltage.Parameters[2].Value) * voltage.Parameters[0].Value / voltage.Parameters[1].Value;
				ActivateSensor(voltage);
			}
		}
		foreach (Sensor temperature in temperatures)
		{
			float? num2 = readTemperature(temperature.Index);
			if (num2.HasValue)
			{
				temperature.Value = num2 + temperature.Parameters[0].Value;
				ActivateSensor(temperature);
			}
		}
		foreach (Sensor fan in fans)
		{
			float? value = readFan(fan.Index);
			if (value.HasValue)
			{
				fan.Value = value;
				if (value.Value > 0f)
				{
					ActivateSensor(fan);
				}
			}
		}
		foreach (Sensor control in controls)
		{
			float? value2 = readControl(control.Index);
			control.Value = value2;
		}
		postUpdate();
	}

	public override void Close()
	{
		foreach (Sensor control in controls)
		{
			superIO.SetControl(control.Index, null);
		}
		base.Close();
	}
}
