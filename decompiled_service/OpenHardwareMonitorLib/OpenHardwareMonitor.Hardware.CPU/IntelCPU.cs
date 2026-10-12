using System;
using System.Globalization;
using System.Text;
using System.Threading;

namespace OpenHardwareMonitor.Hardware.CPU;

internal sealed class IntelCPU : GenericCPU
{
	private enum Microarchitecture
	{
		Unknown,
		NetBurst,
		Core,
		Atom,
		Nehalem,
		SandyBridge,
		IvyBridge,
		Haswell,
		Broadwell,
		Silvermont,
		Skylake,
		Airmont,
		KabyLake,
		Goldmont,
		GoldmontPlus,
		CannonLake,
		IceLake,
		CometLake,
		Tremont,
		TigerLake
	}

	private readonly Sensor[] coreTemperatures;

	private readonly Sensor packageTemperature;

	private readonly Sensor[] coreClocks;

	private readonly Sensor busClock;

	private readonly Sensor[] powerSensors;

	private readonly Microarchitecture microarchitecture;

	private readonly double timeStampCounterMultiplier;

	private const uint IA32_THERM_STATUS_MSR = 412u;

	private const uint IA32_TEMPERATURE_TARGET = 418u;

	private const uint IA32_PERF_STATUS = 408u;

	private const uint MSR_PLATFORM_INFO = 206u;

	private const uint IA32_PACKAGE_THERM_STATUS = 433u;

	private const uint MSR_RAPL_POWER_UNIT = 1542u;

	private const uint MSR_PKG_ENERY_STATUS = 1553u;

	private const uint MSR_DRAM_ENERGY_STATUS = 1561u;

	private const uint MSR_PP0_ENERY_STATUS = 1593u;

	private const uint MSR_PP1_ENERY_STATUS = 1601u;

	private readonly uint[] energyStatusMSRs = new uint[4] { 1553u, 1593u, 1601u, 1561u };

	private readonly string[] powerSensorLabels = new string[4] { "CPU Package", "CPU Cores", "CPU Graphics", "CPU DRAM" };

	private float energyUnitMultiplier;

	private DateTime[] lastEnergyTime;

	private uint[] lastEnergyConsumed;

	private float[] Floats(float f)
	{
		float[] array = new float[coreCount];
		for (int i = 0; i < coreCount; i++)
		{
			array[i] = f;
		}
		return array;
	}

	private float[] GetTjMaxFromMSR()
	{
		float[] array = new float[coreCount];
		for (int i = 0; i < coreCount; i++)
		{
			if (Ring0.RdmsrTx(418u, out var eax, out var _, cpuid[i][0].Affinity))
			{
				array[i] = (eax >> 16) & 0xFF;
			}
			else
			{
				array[i] = 100f;
			}
		}
		return array;
	}

	public IntelCPU(int processorIndex, CPUID[][] cpuid, ISettings settings)
		: base(processorIndex, cpuid, settings)
	{
		float[] array;
		switch (family)
		{
		case 6u:
			switch (model)
			{
			case 15u:
				this.microarchitecture = Microarchitecture.Core;
				switch (stepping)
				{
				case 6u:
					switch (coreCount)
					{
					case 2:
						array = Floats(90f);
						break;
					case 4:
						array = Floats(100f);
						break;
					default:
						array = Floats(95f);
						break;
					}
					array = Floats(90f);
					break;
				case 11u:
					array = Floats(100f);
					break;
				case 13u:
					array = Floats(95f);
					break;
				default:
					array = Floats(95f);
					break;
				}
				break;
			case 23u:
				this.microarchitecture = Microarchitecture.Core;
				array = Floats(100f);
				break;
			case 28u:
				this.microarchitecture = Microarchitecture.Atom;
				array = stepping switch
				{
					2u => Floats(90f), 
					10u => Floats(100f), 
					_ => Floats(90f), 
				};
				break;
			case 26u:
			case 30u:
			case 31u:
			case 37u:
			case 44u:
			case 46u:
			case 47u:
				this.microarchitecture = Microarchitecture.Nehalem;
				array = GetTjMaxFromMSR();
				break;
			case 42u:
			case 45u:
				this.microarchitecture = Microarchitecture.SandyBridge;
				array = GetTjMaxFromMSR();
				break;
			case 58u:
			case 62u:
				this.microarchitecture = Microarchitecture.IvyBridge;
				array = GetTjMaxFromMSR();
				break;
			case 60u:
			case 63u:
			case 69u:
			case 70u:
				this.microarchitecture = Microarchitecture.Haswell;
				array = GetTjMaxFromMSR();
				break;
			case 61u:
			case 71u:
			case 79u:
			case 86u:
				this.microarchitecture = Microarchitecture.Broadwell;
				array = GetTjMaxFromMSR();
				break;
			case 54u:
				this.microarchitecture = Microarchitecture.Atom;
				array = GetTjMaxFromMSR();
				break;
			case 55u:
			case 74u:
			case 77u:
			case 90u:
			case 93u:
				this.microarchitecture = Microarchitecture.Silvermont;
				array = GetTjMaxFromMSR();
				break;
			case 78u:
			case 85u:
			case 94u:
				this.microarchitecture = Microarchitecture.Skylake;
				array = GetTjMaxFromMSR();
				break;
			case 76u:
				this.microarchitecture = Microarchitecture.Airmont;
				array = GetTjMaxFromMSR();
				break;
			case 142u:
			case 158u:
				this.microarchitecture = Microarchitecture.KabyLake;
				array = GetTjMaxFromMSR();
				break;
			case 92u:
			case 95u:
				this.microarchitecture = Microarchitecture.Goldmont;
				array = GetTjMaxFromMSR();
				break;
			case 122u:
				this.microarchitecture = Microarchitecture.GoldmontPlus;
				array = GetTjMaxFromMSR();
				break;
			case 102u:
				this.microarchitecture = Microarchitecture.CannonLake;
				array = GetTjMaxFromMSR();
				break;
			case 106u:
			case 108u:
			case 125u:
			case 126u:
				this.microarchitecture = Microarchitecture.IceLake;
				array = GetTjMaxFromMSR();
				break;
			case 165u:
			case 166u:
				this.microarchitecture = Microarchitecture.CometLake;
				array = GetTjMaxFromMSR();
				break;
			case 134u:
				this.microarchitecture = Microarchitecture.Tremont;
				array = GetTjMaxFromMSR();
				break;
			case 140u:
			case 141u:
				this.microarchitecture = Microarchitecture.TigerLake;
				array = GetTjMaxFromMSR();
				break;
			default:
				this.microarchitecture = Microarchitecture.Unknown;
				array = Floats(100f);
				break;
			}
			break;
		case 15u:
		{
			uint num = model;
			if (num <= 4 || num == 6)
			{
				this.microarchitecture = Microarchitecture.NetBurst;
				array = Floats(100f);
			}
			else
			{
				this.microarchitecture = Microarchitecture.Unknown;
				array = Floats(100f);
			}
			break;
		}
		default:
			this.microarchitecture = Microarchitecture.Unknown;
			array = Floats(100f);
			break;
		}
		switch (this.microarchitecture)
		{
		case Microarchitecture.NetBurst:
		case Microarchitecture.Core:
		case Microarchitecture.Atom:
		{
			if (Ring0.Rdmsr(408u, out var _, out var edx2))
			{
				timeStampCounterMultiplier = (double)((edx2 >> 8) & 0x1F) + 0.5 * (double)((edx2 >> 14) & 1);
			}
			break;
		}
		case Microarchitecture.Nehalem:
		case Microarchitecture.SandyBridge:
		case Microarchitecture.IvyBridge:
		case Microarchitecture.Haswell:
		case Microarchitecture.Broadwell:
		case Microarchitecture.Silvermont:
		case Microarchitecture.Skylake:
		case Microarchitecture.Airmont:
		case Microarchitecture.KabyLake:
		case Microarchitecture.Goldmont:
		case Microarchitecture.GoldmontPlus:
		case Microarchitecture.CannonLake:
		case Microarchitecture.IceLake:
		case Microarchitecture.CometLake:
		case Microarchitecture.Tremont:
		case Microarchitecture.TigerLake:
		{
			if (Ring0.Rdmsr(206u, out var eax, out var _))
			{
				timeStampCounterMultiplier = (eax >> 8) & 0xFF;
			}
			break;
		}
		default:
			timeStampCounterMultiplier = 0.0;
			break;
		}
		if (cpuid[0][0].Data.GetLength(0) > 6 && (cpuid[0][0].Data[6, 0] & 1) != 0 && this.microarchitecture != Microarchitecture.Unknown)
		{
			coreTemperatures = new Sensor[coreCount];
			for (int i = 0; i < coreTemperatures.Length; i++)
			{
				coreTemperatures[i] = new Sensor(CoreString(i), i, SensorType.Temperature, this, new ParameterDescription[2]
				{
					new ParameterDescription("TjMax [°C]", "TjMax temperature of the core sensor.\nTemperature = TjMax - TSlope * Value.", array[i]),
					new ParameterDescription("TSlope [°C]", "Temperature slope of the digital thermal sensor.\nTemperature = TjMax - TSlope * Value.", 1f)
				}, settings);
				ActivateSensor(coreTemperatures[i]);
			}
		}
		else
		{
			coreTemperatures = new Sensor[0];
		}
		if (cpuid[0][0].Data.GetLength(0) > 6 && (cpuid[0][0].Data[6, 0] & 0x40) != 0 && this.microarchitecture != Microarchitecture.Unknown)
		{
			packageTemperature = new Sensor("CPU Package", coreTemperatures.Length, SensorType.Temperature, this, new ParameterDescription[2]
			{
				new ParameterDescription("TjMax [°C]", "TjMax temperature of the package sensor.\nTemperature = TjMax - TSlope * Value.", array[0]),
				new ParameterDescription("TSlope [°C]", "Temperature slope of the digital thermal sensor.\nTemperature = TjMax - TSlope * Value.", 1f)
			}, settings);
			ActivateSensor(packageTemperature);
		}
		busClock = new Sensor("Bus Speed", 0, SensorType.Clock, this, settings);
		coreClocks = new Sensor[coreCount];
		for (int j = 0; j < coreClocks.Length; j++)
		{
			coreClocks[j] = new Sensor(CoreString(j), j + 1, SensorType.Clock, this, settings);
			if (HasTimeStampCounter && this.microarchitecture != Microarchitecture.Unknown)
			{
				ActivateSensor(coreClocks[j]);
			}
		}
		if (this.microarchitecture == Microarchitecture.SandyBridge || this.microarchitecture == Microarchitecture.IvyBridge || this.microarchitecture == Microarchitecture.Haswell || this.microarchitecture == Microarchitecture.Broadwell || this.microarchitecture == Microarchitecture.Skylake || this.microarchitecture == Microarchitecture.Silvermont || this.microarchitecture == Microarchitecture.Airmont || this.microarchitecture == Microarchitecture.KabyLake || this.microarchitecture == Microarchitecture.Goldmont || this.microarchitecture == Microarchitecture.GoldmontPlus || this.microarchitecture == Microarchitecture.CannonLake || this.microarchitecture == Microarchitecture.IceLake || this.microarchitecture == Microarchitecture.CometLake || this.microarchitecture == Microarchitecture.Tremont || this.microarchitecture == Microarchitecture.TigerLake)
		{
			powerSensors = new Sensor[energyStatusMSRs.Length];
			lastEnergyTime = new DateTime[energyStatusMSRs.Length];
			lastEnergyConsumed = new uint[energyStatusMSRs.Length];
			if (Ring0.Rdmsr(1542u, out var eax3, out var edx3))
			{
				Microarchitecture microarchitecture = this.microarchitecture;
				if (microarchitecture == Microarchitecture.Silvermont || microarchitecture == Microarchitecture.Airmont)
				{
					energyUnitMultiplier = 1E-06f * (float)(1 << (int)((eax3 >> 8) & 0x1F));
				}
				else
				{
					energyUnitMultiplier = 1f / (float)(1 << (int)(eax3 >> 8));
				}
			}
			if (energyUnitMultiplier != 0f)
			{
				for (int k = 0; k < energyStatusMSRs.Length; k++)
				{
					if (Ring0.Rdmsr(energyStatusMSRs[k], out eax3, out edx3))
					{
						lastEnergyTime[k] = DateTime.UtcNow;
						lastEnergyConsumed[k] = eax3;
						powerSensors[k] = new Sensor(powerSensorLabels[k], k, SensorType.Power, this, settings);
						ActivateSensor(powerSensors[k]);
					}
				}
			}
		}
		Update();
	}

	protected override uint[] GetMSRs()
	{
		return new uint[10] { 206u, 408u, 412u, 418u, 433u, 1542u, 1553u, 1561u, 1593u, 1601u };
	}

	public override string GetReport()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(base.GetReport());
		stringBuilder.Append("Microarchitecture: ");
		stringBuilder.AppendLine(microarchitecture.ToString());
		stringBuilder.Append("Time Stamp Counter Multiplier: ");
		stringBuilder.AppendLine(timeStampCounterMultiplier.ToString(CultureInfo.InvariantCulture));
		stringBuilder.AppendLine();
		return stringBuilder.ToString();
	}

	public override void Update()
	{
		base.Update();
		for (int i = 0; i < coreTemperatures.Length; i++)
		{
			if (Ring0.RdmsrTx(412u, out var eax, out var _, cpuid[i][0].Affinity) && (eax & 0x80000000u) != 0)
			{
				float num = (eax & 0x7F0000) >> 16;
				float value = coreTemperatures[i].Parameters[0].Value;
				float value2 = coreTemperatures[i].Parameters[1].Value;
				coreTemperatures[i].Value = value - value2 * num;
			}
			else
			{
				coreTemperatures[i].Value = null;
			}
		}
		if (packageTemperature != null)
		{
			if (Ring0.RdmsrTx(433u, out var eax2, out var _, cpuid[0][0].Affinity) && (eax2 & 0x80000000u) != 0)
			{
				float num2 = (eax2 & 0x7F0000) >> 16;
				float value3 = packageTemperature.Parameters[0].Value;
				float value4 = packageTemperature.Parameters[1].Value;
				packageTemperature.Value = value3 - value4 * num2;
			}
			else
			{
				packageTemperature.Value = null;
			}
		}
		if (HasTimeStampCounter && timeStampCounterMultiplier > 0.0)
		{
			double num3 = 0.0;
			for (int j = 0; j < coreClocks.Length; j++)
			{
				Thread.Sleep(1);
				if (Ring0.RdmsrTx(408u, out var eax3, out var _, cpuid[j][0].Affinity))
				{
					num3 = TimeStampCounterFrequency / timeStampCounterMultiplier;
					switch (microarchitecture)
					{
					case Microarchitecture.Nehalem:
					{
						uint num6 = eax3 & 0xFF;
						coreClocks[j].Value = (float)((double)num6 * num3);
						break;
					}
					case Microarchitecture.SandyBridge:
					case Microarchitecture.IvyBridge:
					case Microarchitecture.Haswell:
					case Microarchitecture.Broadwell:
					case Microarchitecture.Silvermont:
					case Microarchitecture.Skylake:
					case Microarchitecture.KabyLake:
					case Microarchitecture.Goldmont:
					case Microarchitecture.GoldmontPlus:
					case Microarchitecture.CannonLake:
					case Microarchitecture.IceLake:
					case Microarchitecture.CometLake:
					case Microarchitecture.Tremont:
					case Microarchitecture.TigerLake:
					{
						uint num5 = (eax3 >> 8) & 0xFF;
						coreClocks[j].Value = (float)((double)num5 * num3);
						break;
					}
					default:
					{
						double num4 = (double)((eax3 >> 8) & 0x1F) + 0.5 * (double)((eax3 >> 14) & 1);
						coreClocks[j].Value = (float)(num4 * num3);
						break;
					}
					}
				}
				else
				{
					coreClocks[j].Value = (float)TimeStampCounterFrequency;
				}
			}
			if (num3 > 0.0)
			{
				busClock.Value = (float)num3;
				ActivateSensor(busClock);
			}
		}
		if (powerSensors == null)
		{
			return;
		}
		Sensor[] array = powerSensors;
		foreach (Sensor sensor in array)
		{
			if (sensor != null && Ring0.Rdmsr(energyStatusMSRs[sensor.Index], out var eax4, out var _))
			{
				DateTime utcNow = DateTime.UtcNow;
				uint num7 = eax4;
				float num8 = (float)(utcNow - lastEnergyTime[sensor.Index]).TotalSeconds;
				if (!((double)num8 < 0.01))
				{
					sensor.Value = energyUnitMultiplier * (float)(num7 - lastEnergyConsumed[sensor.Index]) / num8;
					lastEnergyTime[sensor.Index] = utcNow;
					lastEnergyConsumed[sensor.Index] = num7;
				}
			}
		}
	}
}
