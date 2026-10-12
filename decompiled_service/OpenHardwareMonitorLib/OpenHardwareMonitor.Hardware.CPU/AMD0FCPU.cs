using System.Globalization;
using System.Text;
using System.Threading;

namespace OpenHardwareMonitor.Hardware.CPU;

internal sealed class AMD0FCPU : AMDCPU
{
	private readonly Sensor[] coreTemperatures;

	private readonly Sensor[] coreClocks;

	private readonly Sensor busClock;

	private const uint FIDVID_STATUS = 3221291074u;

	private const byte MISCELLANEOUS_CONTROL_FUNCTION = 3;

	private const ushort MISCELLANEOUS_CONTROL_DEVICE_ID = 4355;

	private const uint THERMTRIP_STATUS_REGISTER = 228u;

	private readonly byte thermSenseCoreSelCPU0;

	private readonly byte thermSenseCoreSelCPU1;

	private readonly uint miscellaneousControlAddress;

	public AMD0FCPU(int processorIndex, CPUID[][] cpuid, ISettings settings)
		: base(processorIndex, cpuid, settings)
	{
		float num = -49f;
		uint num2 = cpuid[0][0].Model;
		if (num2 >= 105 && num2 != 193 && num2 != 108 && num2 != 124)
		{
			num += 21f;
		}
		if (num2 < 40)
		{
			thermSenseCoreSelCPU0 = 0;
			thermSenseCoreSelCPU1 = 4;
		}
		else
		{
			thermSenseCoreSelCPU0 = 4;
			thermSenseCoreSelCPU1 = 0;
		}
		if (cpuid[0][0].ExtData.GetLength(0) > 7 && (cpuid[0][0].ExtData[7, 3] & 1) != 0)
		{
			coreTemperatures = new Sensor[coreCount];
			for (int i = 0; i < coreCount; i++)
			{
				coreTemperatures[i] = new Sensor("Core #" + (i + 1), i, SensorType.Temperature, this, new ParameterDescription[1]
				{
					new ParameterDescription("Offset [°C]", "Temperature offset of the thermal sensor.\nTemperature = Value + Offset.", num)
				}, settings);
			}
		}
		else
		{
			coreTemperatures = new Sensor[0];
		}
		miscellaneousControlAddress = GetPciAddress(3, 4355);
		busClock = new Sensor("Bus Speed", 0, SensorType.Clock, this, settings);
		coreClocks = new Sensor[coreCount];
		for (int j = 0; j < coreClocks.Length; j++)
		{
			coreClocks[j] = new Sensor(CoreString(j), j + 1, SensorType.Clock, this, settings);
			if (HasTimeStampCounter)
			{
				ActivateSensor(coreClocks[j]);
			}
		}
		Update();
	}

	protected override uint[] GetMSRs()
	{
		return new uint[1] { 3221291074u };
	}

	public override string GetReport()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(base.GetReport());
		stringBuilder.Append("Miscellaneous Control Address: 0x");
		stringBuilder.AppendLine(miscellaneousControlAddress.ToString("X", CultureInfo.InvariantCulture));
		stringBuilder.AppendLine();
		return stringBuilder.ToString();
	}

	public override void Update()
	{
		base.Update();
		if (Ring0.WaitPciBusMutex(10))
		{
			if (miscellaneousControlAddress != uint.MaxValue)
			{
				for (uint num = 0u; num < coreTemperatures.Length; num++)
				{
					if (Ring0.WritePciConfig(miscellaneousControlAddress, 228u, (num != 0) ? thermSenseCoreSelCPU1 : thermSenseCoreSelCPU0))
					{
						if (Ring0.ReadPciConfig(miscellaneousControlAddress, 228u, out var value))
						{
							coreTemperatures[num].Value = (float)((value >> 16) & 0xFF) + coreTemperatures[num].Parameters[0].Value;
							ActivateSensor(coreTemperatures[num]);
						}
						else
						{
							DeactivateSensor(coreTemperatures[num]);
						}
					}
				}
			}
			Ring0.ReleasePciBusMutex();
		}
		if (!HasTimeStampCounter)
		{
			return;
		}
		double num2 = 0.0;
		for (int i = 0; i < coreClocks.Length; i++)
		{
			Thread.Sleep(1);
			if (Ring0.RdmsrTx(3221291074u, out var eax, out var _, cpuid[i][0].Affinity))
			{
				double num3 = 0.5 * (double)((eax & 0x3F) + 8);
				double num4 = 0.5 * (double)(((eax >> 16) & 0x3F) + 8);
				coreClocks[i].Value = (float)(num3 * TimeStampCounterFrequency / num4);
				num2 = (float)(TimeStampCounterFrequency / num4);
			}
			else
			{
				coreClocks[i].Value = (float)TimeStampCounterFrequency;
			}
		}
		if (num2 > 0.0)
		{
			busClock.Value = (float)num2;
			ActivateSensor(busClock);
		}
	}
}
