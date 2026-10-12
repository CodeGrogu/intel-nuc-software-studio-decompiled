using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace OpenHardwareMonitor.Hardware.CPU;

internal sealed class AMD17CPU : AMDCPU
{
	private struct TctlOffsetItem
	{
		public string Name { get; set; }

		public float Offset { get; set; }
	}

	private class Core
	{
		private readonly AMD17CPU cpu;

		private readonly GroupAffinity affinity;

		private readonly Sensor powerSensor;

		private readonly Sensor clockSensor;

		private DateTime lastEnergyTime;

		private uint lastEnergyConsumed;

		private float? power;

		public float? Power => power;

		public Core(int index, CPUID[] threads, AMD17CPU cpu, ISettings settings)
		{
			this.cpu = cpu;
			affinity = threads[0].Affinity;
			string name = cpu.CoreString(index);
			powerSensor = new Sensor(name, index + 2, SensorType.Power, cpu, settings);
			clockSensor = new Sensor(name, index + 1, SensorType.Clock, cpu, settings);
			if (cpu.energyUnitMultiplier != 0f && Ring0.RdmsrTx(3221291674u, out var eax, out var _, affinity))
			{
				lastEnergyTime = DateTime.UtcNow;
				lastEnergyConsumed = eax;
				cpu.ActivateSensor(powerSensor);
			}
		}

		private double? GetMultiplier()
		{
			if (Ring0.Rdmsr(3221291667u, out var eax, out var _))
			{
				uint num = (eax >> 8) & 0x3F;
				uint num2 = eax & 0xFF;
				return 2.0 * (double)num2 / (double)num;
			}
			return null;
		}

		public void Update()
		{
			DateTime dateTime = DateTime.MinValue;
			double? num = null;
			GroupAffinity groupAffinity = ThreadAffinity.Set(affinity);
			if (Ring0.Rdmsr(3221291674u, out var eax, out var _))
			{
				dateTime = DateTime.UtcNow;
			}
			num = GetMultiplier();
			ThreadAffinity.Set(groupAffinity);
			if (cpu.energyUnitMultiplier != 0f)
			{
				float num2 = (float)(dateTime - lastEnergyTime).TotalSeconds;
				if ((double)num2 > 0.01)
				{
					power = cpu.energyUnitMultiplier * (float)(eax - lastEnergyConsumed) / num2;
					powerSensor.Value = power;
					lastEnergyTime = dateTime;
					lastEnergyConsumed = eax;
				}
			}
			if (num.HasValue)
			{
				float? value = (float?)(num * (double?)cpu.busClock.Value);
				clockSensor.Value = value;
				if (value.HasValue)
				{
					cpu.ActivateSensor(clockSensor);
				}
			}
		}
	}

	private readonly Core[] cores;

	private readonly Sensor coreTemperature;

	private readonly Sensor tctlTemperature;

	private readonly Sensor ccdMaxTemperature;

	private readonly Sensor ccdAvgTemperature;

	private readonly Sensor[] ccdTemperatures;

	private readonly Sensor packagePowerSensor;

	private readonly Sensor coresPowerSensor;

	private readonly Sensor busClock;

	private const uint FAMILY_17H_M01H_THM_TCON_TEMP = 366592u;

	private const uint FAMILY_17H_M01H_THM_TCON_TEMP_RANGE_SEL = 524288u;

	private const uint FAMILY_17H_M70H_CCD_TEMP_VALID = 2048u;

	private uint maxCcdCount;

	private const uint MSR_RAPL_PWR_UNIT = 3221291673u;

	private const uint MSR_CORE_ENERGY_STAT = 3221291674u;

	private const uint MSR_PKG_ENERGY_STAT = 3221291675u;

	private const uint MSR_P_STATE_0 = 3221291108u;

	private const uint MSR_FAMILY_17H_P_STATE = 3221291667u;

	private float energyUnitMultiplier;

	private uint lastEnergyConsumed;

	private DateTime lastEnergyTime;

	private readonly double timeStampCounterMultiplier;

	private IEnumerable<TctlOffsetItem> tctlOffsetItems = new TctlOffsetItem[6]
	{
		new TctlOffsetItem
		{
			Name = "AMD Ryzen 5 1600X",
			Offset = 20f
		},
		new TctlOffsetItem
		{
			Name = "AMD Ryzen 7 1700X",
			Offset = 20f
		},
		new TctlOffsetItem
		{
			Name = "AMD Ryzen 7 1800X",
			Offset = 20f
		},
		new TctlOffsetItem
		{
			Name = "AMD Ryzen 7 2700X",
			Offset = 10f
		},
		new TctlOffsetItem
		{
			Name = "AMD Ryzen Threadripper 19",
			Offset = 27f
		},
		new TctlOffsetItem
		{
			Name = "AMD Ryzen Threadripper 29",
			Offset = 27f
		}
	};

	private readonly float tctlOffset;

	private uint FAMILY_17H_M70H_CCD_TEMP(uint i)
	{
		return 366932 + i * 4;
	}

	public AMD17CPU(int processorIndex, CPUID[][] cpuid, ISettings settings)
		: base(processorIndex, cpuid, settings)
	{
		string brandString = cpuid[0][0].BrandString;
		if (!string.IsNullOrEmpty(brandString))
		{
			foreach (TctlOffsetItem tctlOffsetItem in tctlOffsetItems)
			{
				if (brandString.StartsWith(tctlOffsetItem.Name))
				{
					tctlOffset = tctlOffsetItem.Offset;
					break;
				}
			}
		}
		coreTemperature = new Sensor("CPU Package", 0, SensorType.Temperature, this, new ParameterDescription[1]
		{
			new ParameterDescription("Offset [°C]", "Temperature offset.", 0f)
		}, base.settings);
		if (tctlOffset != 0f)
		{
			tctlTemperature = new Sensor("CPU Tctl", 1, defaultHidden: true, SensorType.Temperature, this, new ParameterDescription[1]
			{
				new ParameterDescription("Offset [°C]", "Temperature offset.", 0f)
			}, base.settings);
		}
		ccdMaxTemperature = new Sensor("CPU CCD Max", 2, SensorType.Temperature, this, base.settings);
		ccdAvgTemperature = new Sensor("CPU CCD Average", 3, SensorType.Temperature, this, base.settings);
		uint edx = model & 0xF0;
		if (edx == 48 || edx == 112)
		{
			maxCcdCount = 8u;
		}
		else
		{
			maxCcdCount = 4u;
		}
		ccdTemperatures = new Sensor[maxCcdCount];
		for (int i = 0; i < ccdTemperatures.Length; i++)
		{
			ccdTemperatures[i] = new Sensor("CPU CCD #" + (i + 1), i + 4, SensorType.Temperature, this, new ParameterDescription[1]
			{
				new ParameterDescription("Offset [°C]", "Temperature offset.", 0f)
			}, base.settings);
		}
		if (Ring0.Rdmsr(3221291673u, out var eax, out edx))
		{
			energyUnitMultiplier = 1f / (float)(1 << (int)((eax >> 8) & 0x1F));
		}
		if (energyUnitMultiplier != 0f && Ring0.Rdmsr(3221291675u, out var eax2, out edx))
		{
			lastEnergyTime = DateTime.UtcNow;
			lastEnergyConsumed = eax2;
			packagePowerSensor = new Sensor("CPU Package", 0, SensorType.Power, this, settings);
			ActivateSensor(packagePowerSensor);
		}
		coresPowerSensor = new Sensor("CPU Cores", 1, SensorType.Power, this, settings);
		busClock = new Sensor("Bus Speed", 0, SensorType.Clock, this, settings);
		timeStampCounterMultiplier = GetTimeStampCounterMultiplier();
		if (timeStampCounterMultiplier > 0.0)
		{
			busClock.Value = (float)(TimeStampCounterFrequency / timeStampCounterMultiplier);
			ActivateSensor(busClock);
		}
		cores = new Core[coreCount];
		for (int j = 0; j < cores.Length; j++)
		{
			cores[j] = new Core(j, cpuid[j], this, settings);
		}
	}

	protected override uint[] GetMSRs()
	{
		return new uint[5] { 3221291108u, 3221291667u, 3221291673u, 3221291674u, 3221291675u };
	}

	private IList<uint> GetSmnRegisters()
	{
		List<uint> list = new List<uint>();
		list.Add(366592u);
		for (uint num = 0u; num < maxCcdCount; num++)
		{
			list.Add(FAMILY_17H_M70H_CCD_TEMP(num));
		}
		return list;
	}

	public override string GetReport()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(base.GetReport());
		stringBuilder.Append("Time Stamp Counter Multiplier: ");
		stringBuilder.AppendLine(timeStampCounterMultiplier.ToString(CultureInfo.InvariantCulture));
		stringBuilder.AppendLine();
		if (Ring0.WaitPciBusMutex(100))
		{
			stringBuilder.AppendLine("SMN Registers");
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(" Register  Value");
			IList<uint> smnRegisters = GetSmnRegisters();
			for (int i = 0; i < smnRegisters.Count; i++)
			{
				if (ReadSmnRegister(smnRegisters[i], out var value))
				{
					stringBuilder.Append(" ");
					stringBuilder.Append(smnRegisters[i].ToString("X8", CultureInfo.InvariantCulture));
					stringBuilder.Append("  ");
					stringBuilder.Append(value.ToString("X8", CultureInfo.InvariantCulture));
					stringBuilder.AppendLine();
				}
			}
			stringBuilder.AppendLine();
			Ring0.ReleasePciBusMutex();
		}
		return stringBuilder.ToString();
	}

	private double GetTimeStampCounterMultiplier()
	{
		Ring0.Rdmsr(3221291108u, out var eax, out var _);
		uint num = (eax >> 8) & 0x3F;
		uint num2 = eax & 0xFF;
		return 2.0 * (double)num2 / (double)num;
	}

	private bool ReadSmnRegister(uint address, out uint value)
	{
		if (!Ring0.WritePciConfig(0u, 96u, address))
		{
			value = 0u;
			return false;
		}
		return Ring0.ReadPciConfig(0u, 100u, out value);
	}

	public override void Update()
	{
		base.Update();
		if (Ring0.WaitPciBusMutex(10))
		{
			if (ReadSmnRegister(366592u, out var value))
			{
				float num = (float)((value >> 21) & 0x7FF) / 8f;
				if ((value & 0x80000) != 0)
				{
					num -= 49f;
				}
				if (tctlTemperature != null)
				{
					tctlTemperature.Value = num + tctlTemperature.Parameters[0].Value;
					ActivateSensor(tctlTemperature);
				}
				num -= tctlOffset;
				coreTemperature.Value = num + coreTemperature.Parameters[0].Value;
				ActivateSensor(coreTemperature);
			}
			float num2 = float.MinValue;
			int num3 = 0;
			float num4 = 0f;
			for (uint num5 = 0u; num5 < ccdTemperatures.Length; num5++)
			{
				if (ReadSmnRegister(FAMILY_17H_M70H_CCD_TEMP(num5), out value) && (value & 0x800) != 0)
				{
					float num6 = (float)(value & 0x7FF) / 8f - 49f;
					num6 += ccdTemperatures[num5].Parameters[0].Value;
					if (num6 > num2)
					{
						num2 = num6;
					}
					num3++;
					num4 += num6;
					ccdTemperatures[num5].Value = num6;
					ActivateSensor(ccdTemperatures[num5]);
				}
			}
			if (num3 > 1)
			{
				ccdMaxTemperature.Value = num2;
				ActivateSensor(ccdMaxTemperature);
				ccdAvgTemperature.Value = num4 / (float)num3;
				ActivateSensor(ccdAvgTemperature);
			}
			Ring0.ReleasePciBusMutex();
		}
		if (energyUnitMultiplier != 0f && Ring0.Rdmsr(3221291675u, out var eax, out var _))
		{
			DateTime utcNow = DateTime.UtcNow;
			float num7 = (float)(utcNow - lastEnergyTime).TotalSeconds;
			if ((double)num7 > 0.01)
			{
				packagePowerSensor.Value = energyUnitMultiplier * (float)(eax - lastEnergyConsumed) / num7;
				lastEnergyTime = utcNow;
				lastEnergyConsumed = eax;
			}
		}
		float? num8 = 0f;
		for (int i = 0; i < cores.Length; i++)
		{
			cores[i].Update();
			num8 += cores[i].Power;
		}
		coresPowerSensor.Value = num8;
		if (num8.HasValue)
		{
			ActivateSensor(coresPowerSensor);
		}
	}
}
