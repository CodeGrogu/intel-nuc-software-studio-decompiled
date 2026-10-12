using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace OpenHardwareMonitor.Hardware.LPC;

internal class LMSensors
{
	private class LMChip : ISuperIO
	{
		private string path;

		private readonly Chip chip;

		private readonly float?[] voltages;

		private readonly float?[] temperatures;

		private readonly float?[] fans;

		private readonly float?[] controls;

		private readonly FileStream[] voltageStreams;

		private readonly FileStream[] temperatureStreams;

		private readonly FileStream[] fanStreams;

		public Chip Chip => chip;

		public float?[] Voltages => voltages;

		public float?[] Temperatures => temperatures;

		public float?[] Fans => fans;

		public float?[] Controls => controls;

		public LMChip(Chip chip, string path)
		{
			this.path = path;
			this.chip = chip;
			string[] files = Directory.GetFiles(path, "in*_input");
			voltages = new float?[files.Length];
			voltageStreams = new FileStream[files.Length];
			for (int i = 0; i < files.Length; i++)
			{
				voltageStreams[i] = new FileStream(files[i], FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			}
			string[] files2 = Directory.GetFiles(path, "temp*_input");
			temperatures = new float?[files2.Length];
			temperatureStreams = new FileStream[files2.Length];
			for (int j = 0; j < files2.Length; j++)
			{
				temperatureStreams[j] = new FileStream(files2[j], FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			}
			string[] files3 = Directory.GetFiles(path, "fan*_input");
			fans = new float?[files3.Length];
			fanStreams = new FileStream[files3.Length];
			for (int k = 0; k < files3.Length; k++)
			{
				fanStreams[k] = new FileStream(files3[k], FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			}
			controls = new float?[0];
		}

		public byte? ReadGPIO(int index)
		{
			return null;
		}

		public void WriteGPIO(int index, byte value)
		{
		}

		public string GetReport()
		{
			return null;
		}

		public void SetControl(int index, byte? value)
		{
		}

		private string ReadFirstLine(Stream stream)
		{
			StringBuilder stringBuilder = new StringBuilder();
			try
			{
				stream.Seek(0L, SeekOrigin.Begin);
				int num = stream.ReadByte();
				while (num != -1 && num != 10)
				{
					stringBuilder.Append((char)num);
					num = stream.ReadByte();
				}
			}
			catch
			{
			}
			return stringBuilder.ToString();
		}

		public void Update()
		{
			for (int i = 0; i < voltages.Length; i++)
			{
				string s = ReadFirstLine(voltageStreams[i]);
				try
				{
					voltages[i] = 0.001f * (float)long.Parse(s, CultureInfo.InvariantCulture);
				}
				catch
				{
					voltages[i] = null;
				}
			}
			for (int j = 0; j < temperatures.Length; j++)
			{
				string s2 = ReadFirstLine(temperatureStreams[j]);
				try
				{
					temperatures[j] = 0.001f * (float)long.Parse(s2, CultureInfo.InvariantCulture);
				}
				catch
				{
					temperatures[j] = null;
				}
			}
			for (int k = 0; k < fans.Length; k++)
			{
				string s3 = ReadFirstLine(fanStreams[k]);
				try
				{
					fans[k] = long.Parse(s3, CultureInfo.InvariantCulture);
				}
				catch
				{
					fans[k] = null;
				}
			}
		}

		public void Close()
		{
			FileStream[] array = voltageStreams;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Close();
			}
			array = temperatureStreams;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Close();
			}
			array = fanStreams;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Close();
			}
		}
	}

	private readonly List<LMChip> lmChips = new List<LMChip>();

	public ISuperIO[] SuperIO => lmChips.ToArray();

	public LMSensors()
	{
		string[] directories = Directory.GetDirectories("/sys/class/hwmon/");
		foreach (string text in directories)
		{
			string[] array = new string[2] { "/device", "" };
			foreach (string text2 in array)
			{
				string text3 = text + text2;
				string text4 = null;
				try
				{
					using StreamReader streamReader = new StreamReader(text3 + "/name");
					text4 = streamReader.ReadLine();
				}
				catch (IOException)
				{
				}
				switch (text4)
				{
				case "atk0110":
					lmChips.Add(new LMChip(Chip.ATK0110, text3));
					break;
				case "f71858fg":
					lmChips.Add(new LMChip(Chip.F71858, text3));
					break;
				case "f71862fg":
					lmChips.Add(new LMChip(Chip.F71862, text3));
					break;
				case "f71869":
					lmChips.Add(new LMChip(Chip.F71869, text3));
					break;
				case "f71869a":
					lmChips.Add(new LMChip(Chip.F71869A, text3));
					break;
				case "f71882fg":
					lmChips.Add(new LMChip(Chip.F71882, text3));
					break;
				case "f71889a":
					lmChips.Add(new LMChip(Chip.F71889AD, text3));
					break;
				case "f71878ad":
					lmChips.Add(new LMChip(Chip.F71878AD, text3));
					break;
				case "f71889ed":
					lmChips.Add(new LMChip(Chip.F71889ED, text3));
					break;
				case "f71889fg":
					lmChips.Add(new LMChip(Chip.F71889F, text3));
					break;
				case "f71808e":
					lmChips.Add(new LMChip(Chip.F71808E, text3));
					break;
				case "it8705":
					lmChips.Add(new LMChip(Chip.IT8705F, text3));
					break;
				case "it8712":
					lmChips.Add(new LMChip(Chip.IT8712F, text3));
					break;
				case "it8716":
					lmChips.Add(new LMChip(Chip.IT8716F, text3));
					break;
				case "it8718":
					lmChips.Add(new LMChip(Chip.IT8718F, text3));
					break;
				case "it8720":
					lmChips.Add(new LMChip(Chip.IT8720F, text3));
					break;
				case "nct6775":
					lmChips.Add(new LMChip(Chip.NCT6771F, text3));
					break;
				case "nct6776":
					lmChips.Add(new LMChip(Chip.NCT6776F, text3));
					break;
				case "nct6779":
					lmChips.Add(new LMChip(Chip.NCT6779D, text3));
					break;
				case "nct6791":
					lmChips.Add(new LMChip(Chip.NCT6791D, text3));
					break;
				case "nct6792":
					lmChips.Add(new LMChip(Chip.NCT6792D, text3));
					break;
				case "nct6793":
					lmChips.Add(new LMChip(Chip.NCT6793D, text3));
					break;
				case "nct6795":
					lmChips.Add(new LMChip(Chip.NCT6795D, text3));
					break;
				case "nct6796":
					lmChips.Add(new LMChip(Chip.NCT6796D, text3));
					break;
				case "nct6797":
					lmChips.Add(new LMChip(Chip.NCT6797D, text3));
					break;
				case "nct6798":
					lmChips.Add(new LMChip(Chip.NCT6798D, text3));
					break;
				case "w83627ehf":
					lmChips.Add(new LMChip(Chip.W83627EHF, text3));
					break;
				case "w83627dhg":
					lmChips.Add(new LMChip(Chip.W83627DHG, text3));
					break;
				case "w83667hg":
					lmChips.Add(new LMChip(Chip.W83667HG, text3));
					break;
				case "w83627hf":
					lmChips.Add(new LMChip(Chip.W83627HF, text3));
					break;
				case "w83627thf":
					lmChips.Add(new LMChip(Chip.W83627THF, text3));
					break;
				case "w83687thf":
					lmChips.Add(new LMChip(Chip.W83687THF, text3));
					break;
				}
			}
		}
	}

	public void Close()
	{
		foreach (LMChip lmChip in lmChips)
		{
			lmChip.Close();
		}
	}
}
