using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;

namespace SerialDeviceAccessLibrary;

public class SerialDeviceAccess
{
	public delegate void BtnStatusChangedHandler(BntStatus bntStatus);

	public delegate void CurrentStatusChangedHandler(LedStatus getStatus);

	private static SerialDeviceAccess _serialDeviceAccess;

	private static readonly object _singletonCreationLock = new object();

	private static SerialPort _serialPort;

	private static readonly LedPatternType _ledPatternType = new LedPatternType();

	private readonly ErrorDict _errorDict = new ErrorDict();

	private static readonly List<char> _ledChannelType = new List<char> { 'A', 'B', 'C', 'D' };

	private static int _sendCommandDelayTime = 100;

	private static readonly Dictionary<byte, LedPattern> _LEDStatus = new Dictionary<byte, LedPattern>();

	private static readonly List<byte> _paletteStatus = new List<byte>();

	private static bool _isPattern8;

	private static bool _isPattern9;

	private static string _vid = "067B";

	private static string _pid = "2323";

	private static readonly ThreadStart _setDevice = SetTheDevice;

	private static Thread _writeThread;

	private static List<LedPattern> patternListSendingBuffer = null;

	private static List<LedPattern> patternListPendingBuffer = null;

	private static bool _isSendFinished = true;

	private static readonly object _bufferLock = new object();

	public static string SerialPortError { get; set; }

	public string ReturnMessage { get; set; }

	public static SerialDeviceAccess GetSerialDeviceAccessInstance
	{
		get
		{
			if (_serialDeviceAccess == null)
			{
				lock (_singletonCreationLock)
				{
					if (_serialDeviceAccess == null)
					{
						_serialDeviceAccess = new SerialDeviceAccess();
					}
				}
			}
			return _serialDeviceAccess;
		}
	}

	private static bool IsDeviceConnected => _serialPort != null;

	public static event BtnStatusChangedHandler BtnStatusChanged;

	public static event CurrentStatusChangedHandler CurrentStatusChanged;

	public SerialDeviceAccess()
	{
		foreach (char item in _ledChannelType)
		{
			_LEDStatus.Add(Convert.ToByte(item), new LedPattern(Convert.ToByte(item), 1, 0, 5, 1, 1));
		}
		_paletteStatus.Add(0);
		_paletteStatus.Add(0);
		_paletteStatus.Add(0);
		_paletteStatus.Add(0);
		_paletteStatus.Add(96);
		_paletteStatus.Add(160);
		_paletteStatus.Add(0);
	}

	public static bool ConnectToDevice()
	{
		bool result = false;
		if (IsDeviceConnected)
		{
			return InitializeDevice();
		}
		if (InitializeDevice())
		{
			if (OpenDevice())
			{
				result = true;
			}
			else
			{
				_serialPort = null;
			}
		}
		else
		{
			SerialPortError = SerialPortErrors.NotInstalled.ToString();
		}
		return result;
	}

	public static void DisconnectFromDevice()
	{
		if (IsDeviceConnected)
		{
			ReadEventClose();
			CloseDevice();
		}
	}

	public LedPatternType GetSupportedPatterns()
	{
		return _ledPatternType;
	}

	public string TranslateErrorToString(byte errorNum)
	{
		return _errorDict.MsgErrorDict[errorNum];
	}

	public static void SetLEDPattern(List<LedPattern> patternList)
	{
		_isSendFinished = false;
		if (_serialDeviceAccess == null)
		{
			_serialDeviceAccess = new SerialDeviceAccess();
		}
		if (patternListSendingBuffer != null)
		{
			patternListPendingBuffer = patternList;
		}
		else
		{
			patternListSendingBuffer = patternList;
		}
		if (_writeThread == null)
		{
			_writeThread = new Thread(_setDevice);
		}
		if (_writeThread.ThreadState == ThreadState.Stopped)
		{
			_writeThread.Abort();
			_writeThread = new Thread(_setDevice);
		}
		if (_writeThread.ThreadState == ThreadState.Unstarted)
		{
			_writeThread.Start();
		}
	}

	public static void GetCurrentPatterns()
	{
		uint num = 100u;
		if (_writeThread != null)
		{
			while (!_isSendFinished)
			{
				num--;
				Thread.Sleep(10);
				if (num == 0)
				{
					break;
				}
			}
		}
		if (IsDeviceConnected)
		{
			DeviceWrite(ConvertStringtoUTF8ByteArray("STA?\n"));
			SendCommandDelayTime();
			return;
		}
		throw new ArgumentException("[Error] please connect device first.");
	}

	public static LedStatus GetCurrentPatternsV2()
	{
		Thread.Sleep(1000);
		if (_serialDeviceAccess == null)
		{
			_serialDeviceAccess = new SerialDeviceAccess();
		}
		LedStatus result = new LedStatus();
		uint num = 100u;
		if (_writeThread != null)
		{
			while (!_isSendFinished)
			{
				num--;
				Thread.Sleep(10);
				if (num == 0)
				{
					break;
				}
			}
		}
		if (IsDeviceConnected)
		{
			DeviceWrite(ConvertStringtoUTF8ByteArray("STA?\n"));
			SendCommandDelayTime();
			int bytesToRead = _serialPort.BytesToRead;
			if (bytesToRead == 7)
			{
				string text = _serialPort.ReadExisting();
				if (text == "BTN:1\r\n")
				{
					BtnStatusChanged(BntStatus.On);
				}
				if (text == "BTN:0\r\n")
				{
					BtnStatusChanged(BntStatus.Off);
				}
			}
			else
			{
				int bytesToRead2 = _serialPort.BytesToRead;
				while (bytesToRead != bytesToRead2)
				{
					bytesToRead = _serialPort.BytesToRead;
					Thread.Sleep(10);
					bytesToRead2 = _serialPort.BytesToRead;
				}
				byte[] array = DeviceRead();
				if (array.Length <= 1)
				{
					throw new ArgumentException("[Error] receive fail.");
				}
				result = TranslateMsgToLedPattern(array, 25u);
			}
			return result;
		}
		throw new ArgumentException("[Error] please connect device first.");
	}

	public static byte[] GetLedConnectionStatus()
	{
		if (IsDeviceConnected)
		{
			DeviceWrite(ConvertStringtoUTF8ByteArray("AUTONUM?\n"));
		}
		if (IsDeviceConnected)
		{
			Thread.Sleep(200);
			return _serialPort.ReadExisting().Split(',').Select(byte.Parse)
				.ToArray();
		}
		return new byte[4];
	}

	public static string GetControllerIdentifier()
	{
		ReadEventClose();
		if (IsDeviceConnected)
		{
			Thread.Sleep(1800);
			DeviceWrite(ConvertStringtoUTF8ByteArray("ID?\n"));
			Thread.Sleep(100);
			string result = ConvertUTF8ByteArrayToString(DeviceRead());
			ReadEventOpen();
			return result;
		}
		throw new ArgumentException("[Error] please connect device first.");
	}

	public static bool ResetAllPattern()
	{
		if (IsDeviceConnected)
		{
			DeviceWrite(ConvertStringtoUTF8ByteArray("RST\n"));
			SendCommandDelayTime();
			_isPattern8 = false;
			_isPattern9 = false;
		}
		return true;
	}

	public static byte GetNowLEDPatternIndex()
	{
		if (_isPattern8)
		{
			return 8;
		}
		if (_isPattern9)
		{
			return 9;
		}
		return 0;
	}

	public static bool DisableAll()
	{
		if (IsDeviceConnected)
		{
			if (_isPattern8)
			{
				ExitPattern8();
				SendCommandDelayTime();
			}
			else if (_isPattern9)
			{
				ExitPattern9();
				SendCommandDelayTime();
			}
			DeviceWrite(ConvertStringtoUTF8ByteArray("BTN:0\n"));
			SendCommandDelayTime();
		}
		return true;
	}

	public static bool EnableAll()
	{
		if (IsDeviceConnected)
		{
			DeviceWrite(ConvertStringtoUTF8ByteArray("BTN:1\n"));
			SendCommandDelayTime();
		}
		return true;
	}

	public static void SetDeviceInfo(string vid, string pid)
	{
		if (!IsDeviceConnected)
		{
			_vid = vid;
			_pid = pid;
		}
	}

	public static void SetSendCommandDelayTime(int delaytime = 100)
	{
		_sendCommandDelayTime = delaytime;
	}

	public static void SendCommandDelayTime()
	{
		Thread.Sleep(_sendCommandDelayTime);
	}

	private static bool EnterPattern9()
	{
		if (!_isPattern9)
		{
			byte[] array = ConvertStringtoUTF8ByteArray("\n");
			uint num = DeviceWrite(CombineByteArray(ConvertStringtoUTF8ByteArray("A"), ConvertStringtoUTF8ByteArray("P"), ConvertStringtoUTF8ByteArray("9"), array));
			SendCommandDelayTime();
			if (num == 0)
			{
				throw new ArgumentException("[Error] write data to device.");
			}
			_isPattern9 = true;
			return true;
		}
		return false;
	}

	private static bool ExitPattern9()
	{
		if (_isPattern9)
		{
			byte[] array = ConvertStringtoUTF8ByteArray("\n");
			uint num = DeviceWrite(CombineByteArray(ConvertStringtoUTF8ByteArray("B"), ConvertStringtoUTF8ByteArray("P"), ConvertStringtoUTF8ByteArray("1"), array));
			SendCommandDelayTime();
			if (num == 0)
			{
				throw new ArgumentException("[Error] write data to device.");
			}
			_isPattern9 = false;
			return true;
		}
		return false;
	}

	private static bool ExitPattern8()
	{
		if (_isPattern8)
		{
			byte[] array = ConvertStringtoUTF8ByteArray("\n");
			uint num = DeviceWrite(CombineByteArray(ConvertStringtoUTF8ByteArray("A"), ConvertStringtoUTF8ByteArray("P"), ConvertStringtoUTF8ByteArray("1"), array));
			SendCommandDelayTime();
			if (num == 0)
			{
				throw new ArgumentException("[Error] write data to device.");
			}
			_isPattern8 = false;
			return true;
		}
		return false;
	}

	private static void SetTheDevice()
	{
		while (patternListSendingBuffer != null)
		{
			List<List<byte>> list = new List<List<byte>>();
			foreach (LedPattern item in patternListSendingBuffer)
			{
				List<byte[]> list2 = new List<byte[]>();
				if (_isPattern9 && item.Channel != 65 && item.PatternIndex != 9)
				{
					ExitPattern9();
					SendCommandDelayTime();
				}
				if (_isPattern8 && item.PatternIndex != 8)
				{
					ExitPattern8();
					SendCommandDelayTime();
				}
				List<byte> list3 = CheckPatternParameters(item);
				if (list3.Count == 0)
				{
					if (item.PatternIndex == 9)
					{
						EnterPattern9();
					}
					list2 = TranslateLedPatternToMsg(item);
				}
				foreach (byte[] item2 in list2)
				{
					if (DeviceWrite(item2) == 0)
					{
						throw new ArgumentException("[Error] write data to device.");
					}
					SendCommandDelayTime();
				}
				list.Add(list3);
				if (item.PatternIndex == 8)
				{
					break;
				}
			}
			lock (_bufferLock)
			{
				if (patternListPendingBuffer != null)
				{
					patternListSendingBuffer = patternListPendingBuffer;
					patternListPendingBuffer = null;
					_isSendFinished = false;
				}
				else
				{
					patternListSendingBuffer = null;
					_isSendFinished = true;
				}
			}
		}
	}

	private static byte[] ConvertStringtoUTF8ByteArray(string rawData)
	{
		return Encoding.UTF8.GetBytes(rawData);
	}

	private static string ConvertUTF8ByteArrayToString(byte[] rawData)
	{
		return Encoding.UTF8.GetString(rawData);
	}

	private static List<byte[]> TranslateLedPatternToMsg(LedPattern ledPattern)
	{
		List<byte[]> list = new List<byte[]>();
		byte[] array = ConvertStringtoUTF8ByteArray(Convert.ToChar(ledPattern.Channel).ToString());
		LedPattern ledPattern2 = _LEDStatus[array[0]];
		byte[] array2 = ConvertStringtoUTF8ByteArray(ledPattern.PatternIndex.ToString());
		byte[] array3 = ConvertStringtoUTF8ByteArray("\n");
		if (ledPattern.PatternIndex == 8)
		{
			_isPattern8 = true;
			if (ledPattern.Channel != 65)
			{
				ledPattern.Channel = 65;
			}
			if (ledPattern.PatternIndex != ledPattern2.PatternIndex)
			{
				byte[] array4 = ConvertStringtoUTF8ByteArray("P");
				byte[] item = CombineByteArray(array, array4, array2, array3);
				list.Add(item);
			}
			foreach (char item2 in _ledChannelType)
			{
				array = ConvertStringtoUTF8ByteArray(item2.ToString());
				byte[] item = CombineByteArray(array, ConvertStringtoUTF8ByteArray("V:"), ConvertStringtoUTF8ByteArray(ledPattern.MaxBrightness.ToString()), array3);
				list.Add(item);
			}
			_LEDStatus[array[0]] = ledPattern;
			return list;
		}
		_isPattern8 = false;
		if (ledPattern.PatternIndex != ledPattern2.PatternIndex)
		{
			byte[] array5 = ConvertStringtoUTF8ByteArray("P");
			byte[] item = CombineByteArray(array, array5, array2, array3);
			list.Add(item);
		}
		int count = ledPattern.Palette.Count;
		if (count == _ledPatternType.LedPatternTypeOnePaletteNums)
		{
			if (ledPattern.Channel == 65)
			{
				if (ledPattern.Palette[0] != _paletteStatus[0])
				{
					_paletteStatus[0] = ledPattern.Palette[0];
					byte[] item = CombineByteArray(ConvertStringtoUTF8ByteArray("C1:"), ConvertStringtoUTF8ByteArray(ledPattern.Palette[0].ToString()), array3);
					list.Add(item);
				}
			}
			else if (ledPattern.Channel == 66)
			{
				if (ledPattern.Palette[0] != _paletteStatus[1])
				{
					_paletteStatus[1] = ledPattern.Palette[0];
					byte[] item = CombineByteArray(ConvertStringtoUTF8ByteArray("C2:"), ConvertStringtoUTF8ByteArray(ledPattern.Palette[0].ToString()), array3);
					list.Add(item);
				}
			}
			else if (ledPattern.Channel == 67)
			{
				if (ledPattern.Palette[0] != _paletteStatus[2])
				{
					_paletteStatus[2] = ledPattern.Palette[0];
					byte[] item = CombineByteArray(ConvertStringtoUTF8ByteArray("C3:"), ConvertStringtoUTF8ByteArray(ledPattern.Palette[0].ToString()), array3);
					list.Add(item);
				}
			}
			else if (ledPattern.Channel == 68 && ledPattern.Palette[0] != _paletteStatus[3])
			{
				_paletteStatus[3] = ledPattern.Palette[0];
				byte[] item = CombineByteArray(ConvertStringtoUTF8ByteArray("C4:"), ConvertStringtoUTF8ByteArray(ledPattern.Palette[0].ToString()), array3);
				list.Add(item);
			}
		}
		else if (count == _ledPatternType.LedPatternTypeTwoPaletteNums)
		{
			if (ledPattern.Palette[0] != _paletteStatus[4])
			{
				_paletteStatus[4] = ledPattern.Palette[0];
				byte[] item = CombineByteArray(ConvertStringtoUTF8ByteArray("C5:"), ConvertStringtoUTF8ByteArray(ledPattern.Palette[0].ToString()), array3);
				list.Add(item);
			}
			if (ledPattern.Palette[1] != _paletteStatus[5])
			{
				_paletteStatus[5] = ledPattern.Palette[1];
				byte[] item = CombineByteArray(ConvertStringtoUTF8ByteArray("C6:"), ConvertStringtoUTF8ByteArray(ledPattern.Palette[1].ToString()), array3);
				list.Add(item);
			}
			if (ledPattern.Palette[2] != _paletteStatus[6])
			{
				_paletteStatus[6] = ledPattern.Palette[2];
				byte[] item = CombineByteArray(ConvertStringtoUTF8ByteArray("C7:"), ConvertStringtoUTF8ByteArray(ledPattern.Palette[2].ToString()), array3);
				list.Add(item);
			}
		}
		if (ledPattern.MaxBrightness != LedPatternDefaultValueSettings.ParametersDefaultValue && ledPattern.MaxBrightness != ledPattern2.MaxBrightness)
		{
			byte[] item = CombineByteArray(array, ConvertStringtoUTF8ByteArray("V:"), ConvertStringtoUTF8ByteArray(ledPattern.MaxBrightness.ToString()), array3);
			list.Add(item);
		}
		if (ledPattern.RainBow != LedPatternDefaultValueSettings.ParametersDefaultValue && ledPattern.RainBow != ledPattern2.RainBow)
		{
			byte[] item = CombineByteArray(array, ConvertStringtoUTF8ByteArray("R:"), ConvertStringtoUTF8ByteArray(ledPattern.RainBow.ToString()), array3);
			list.Add(item);
		}
		if (ledPattern.Speed != LedPatternDefaultValueSettings.ParametersDefaultValue && ledPattern.Speed != ledPattern2.Speed)
		{
			byte[] item = CombineByteArray(array, ConvertStringtoUTF8ByteArray("D:"), ConvertStringtoUTF8ByteArray(ledPattern.Speed.ToString()), array3);
			list.Add(item);
		}
		_LEDStatus[array[0]] = ledPattern;
		return list;
	}

	private static List<byte> CheckPatternParameters(LedPattern ledPattern)
	{
		List<byte> list = new List<byte>();
		if (ledPattern.Channel < 65 || ledPattern.Channel > 68)
		{
			list.Add(1);
		}
		if (ledPattern.PatternIndex > 9)
		{
			list.Add(2);
		}
		if (ledPattern.MaxBrightness != LedPatternDefaultValueSettings.ParametersDefaultValue && ledPattern.MaxBrightness > 5)
		{
			list.Add(3);
		}
		if (ledPattern.RainBow != LedPatternDefaultValueSettings.ParametersDefaultValue && ledPattern.RainBow != 1 && ledPattern.RainBow != 0)
		{
			list.Add(5);
		}
		if (ledPattern.Speed != LedPatternDefaultValueSettings.ParametersDefaultValue && ledPattern.Speed > 5)
		{
			list.Add(6);
		}
		return list;
	}

	private static List<byte> CheckPatternEightParameters(LedPattern ledPattern)
	{
		List<byte> list = new List<byte>();
		if (ledPattern.PatternIndex < 1 || ledPattern.PatternIndex > 8)
		{
			list.Add(2);
		}
		if (ledPattern.PatternIndex == 8)
		{
			if (ledPattern.MaxBrightness != LedPatternDefaultValueSettings.ParametersDefaultValue && ledPattern.MaxBrightness > 5)
			{
				list.Add(3);
			}
			if (ledPattern.Palette.Count != 0)
			{
				list.Add(4);
			}
			if (ledPattern.RainBow != LedPatternDefaultValueSettings.ParametersDefaultValue)
			{
				list.Add(5);
			}
			if (ledPattern.Speed != LedPatternDefaultValueSettings.ParametersDefaultValue)
			{
				list.Add(6);
			}
		}
		return list;
	}

	private static LedStatus TranslateMsgToLedPattern(byte[] messageList, uint receiveMessageLength)
	{
		LedStatus ledStatus = new LedStatus();
		List<LedPattern> list = new List<LedPattern>();
		string[] array = ConvertUTF8ByteArrayToString(messageList).Split(',', '\n');
		if (array != null && array.Length != 0 && (array[0] == "BTN:1\r" || array[0] == "BTN:0\r"))
		{
			array = array.Skip(1).ToArray();
		}
		string[] array2 = new string[receiveMessageLength];
		for (int i = 1; i < 20 && array.Length != receiveMessageLength * i - (i - 1); i++)
		{
			if (i == 19)
			{
				throw new ArgumentException("[Error]wrong return message");
			}
		}
		int num = array.Length - (int)receiveMessageLength;
		for (int j = 0; j < receiveMessageLength; j++)
		{
			array2[j] = array[num + j];
		}
		byte b = Convert.ToByte(array2[4]);
		byte value = Convert.ToByte(array2[5]);
		byte value2 = Convert.ToByte(array2[6]);
		byte value3 = Convert.ToByte(array2[7]);
		byte b2 = Convert.ToByte(array2[8]);
		byte b3 = Convert.ToByte(array2[9]);
		byte b4 = Convert.ToByte(array2[10]);
		_paletteStatus[0] = b;
		_paletteStatus[1] = value;
		_paletteStatus[2] = value2;
		_paletteStatus[3] = value3;
		_paletteStatus[4] = b2;
		_paletteStatus[5] = b3;
		_paletteStatus[6] = b4;
		uint num2 = 0u;
		foreach (char item in _ledChannelType)
		{
			byte b5 = Convert.ToByte(array2[num2]);
			byte maxBrightness = Convert.ToByte(array2[num2 + 11]);
			byte speed = Convert.ToByte(array2[num2 + 15]);
			byte rainBow = Convert.ToByte(array2[num2 + 19]);
			byte palette = Convert.ToByte(array2[num2 + 4]);
			LedPattern ledPattern;
			if (b5 == _ledPatternType.LedPatternTypeZeroIndex)
			{
				ledPattern = new LedPattern(Convert.ToByte(item), b5);
			}
			else if (b5 > _ledPatternType.LedPatternTypeZeroIndex && b5 <= _ledPatternType.LedPatternTypeOneMaxIndex)
			{
				ledPattern = new LedPattern(Convert.ToByte(item), b5, palette, maxBrightness, rainBow, speed);
			}
			else if (b5 > _ledPatternType.LedPatternTypeOneMaxIndex && b5 <= _ledPatternType.LedPatternTypeTwoMaxIndex)
			{
				ledPattern = new LedPattern(Convert.ToByte(item), b5, b2, b3, b4, maxBrightness, speed);
			}
			else
			{
				ledPattern = ((b5 <= _ledPatternType.LedPatternTypeTwoMaxIndex || b5 >= _ledPatternType.LedPatternTypeThreeMaxIndex) ? new LedPattern(Convert.ToByte(item), b5, b, maxBrightness, rainBow) : new LedPattern(Convert.ToByte(item), b5, maxBrightness));
			}
			list.Add(ledPattern);
			_LEDStatus[Convert.ToByte(item)] = ledPattern;
			num2++;
			if (b5 == 8)
			{
				_isPattern8 = true;
			}
			if (b5 == 9)
			{
				_isPattern9 = true;
			}
		}
		ledStatus.LedPatternList = list;
		BntStatus buttonStatus = ((array2[23] == "1\r" || array2[23] == "1") ? BntStatus.On : ((!(array2[23] == "0\r") && !(array2[23] == "0")) ? BntStatus.NotSetting : BntStatus.Off));
		ledStatus.ButtonStatus = buttonStatus;
		return ledStatus;
	}

	private static byte[] CombineByteArray(params byte[][] bytesArrayList)
	{
		byte[] array = new byte[bytesArrayList.Sum((byte[] a) => a.Length)];
		int num = 0;
		foreach (byte[] array2 in bytesArrayList)
		{
			Buffer.BlockCopy(array2, 0, array, num, array2.Length);
			num += array2.Length;
		}
		return array;
	}

	private static void ReadEventOpen()
	{
		_serialPort.DtrEnable = true;
		_serialPort.RtsEnable = true;
	}

	private static void ReadEventClose()
	{
		_serialPort.DataReceived -= DataReceivedHandler;
	}

	private static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
	{
		SendCommandDelayTime();
		int bytesToRead = _serialPort.BytesToRead;
		if (bytesToRead == 7)
		{
			string text = _serialPort.ReadExisting();
			if (text == "BTN:1\r\n")
			{
				BtnStatusChanged(BntStatus.On);
			}
			if (text == "BTN:0\r\n")
			{
				BtnStatusChanged(BntStatus.Off);
			}
			return;
		}
		int bytesToRead2 = _serialPort.BytesToRead;
		while (bytesToRead != bytesToRead2)
		{
			bytesToRead = _serialPort.BytesToRead;
			Thread.Sleep(10);
			bytesToRead2 = _serialPort.BytesToRead;
		}
		byte[] array = DeviceRead();
		if (array.Length <= 1)
		{
			throw new ArgumentException("[Error] receive fail.");
		}
		LedStatus getStatus = TranslateMsgToLedPattern(array, 25u);
		CurrentStatusChanged(getStatus);
	}

	private static byte[] DeviceRead()
	{
		ReconnectDevice();
		if (_serialPort.BytesToRead >= 1)
		{
			string s = _serialPort.ReadExisting();
			return Encoding.UTF8.GetBytes(s);
		}
		return new byte[0];
	}

	private static void ReconnectDevice()
	{
		if (_serialPort != null && !_serialPort.IsOpen)
		{
			_serialPort = null;
			ConnectToDevice();
		}
	}

	private static uint DeviceWrite(byte[] message)
	{
		ReconnectDevice();
		_serialPort.Write(message, 0, message.Length);
		return (uint)message.Length;
	}

	private static bool InitializeDevice()
	{
		string serialPortName = GetSerialPortName();
		if (serialPortName != null)
		{
			_serialPort = new SerialPort();
			SerialPortSetting(serialPortName);
			ReadEventOpen();
			return true;
		}
		return false;
	}

	private static void SerialPortSetting(string SeiralPortName)
	{
		_serialPort.PortName = SeiralPortName;
		_serialPort.BaudRate = 9600;
		_serialPort.Parity = Parity.None;
		_serialPort.DataBits = 8;
		_serialPort.StopBits = StopBits.One;
		_serialPort.Handshake = Handshake.None;
		_serialPort.ReadTimeout = 500;
		_serialPort.WriteTimeout = 500;
		_serialPort.DtrEnable = true;
		_serialPort.RtsEnable = true;
	}

	private static string GetSerialPortName()
	{
		foreach (ManagementObject item in new ManagementObjectSearcher("SELECT * FROM WIN32_PnPEntity").Get().OfType<ManagementObject>())
		{
			if (item["Caption"] == null || !item["Caption"].ToString().Contains("(COM"))
			{
				continue;
			}
			string text = item["Caption"].ToString();
			string text2 = item["deviceid"].ToString();
			int num = text2.IndexOf("VID_");
			int num2 = text2.IndexOf("PID_");
			if (num != -1 && num2 != -1)
			{
				string text3 = text2.Substring(num + 4).Substring(0, 4);
				string text4 = text2.Substring(num2 + 4).Substring(0, 4);
				if (text3 == _vid && text4 == _pid)
				{
					int num3 = text.IndexOf("(COM");
					int num4 = text.IndexOf(")");
					return text.Substring(num3 + 1, num4 - num3 - 1).Replace("COM", "com");
				}
			}
		}
		return null;
	}

	private static bool OpenDevice()
	{
		try
		{
			if (_serialPort != null)
			{
				_serialPort.Open();
				return true;
			}
			return false;
		}
		catch (Exception ex)
		{
			if (ex.ToString().Contains("is denied"))
			{
				SerialPortError = SerialPortErrors.Accessdenied.ToString();
			}
			else if (ex.ToString().Contains("does not exist"))
			{
				SerialPortError = SerialPortErrors.NotExist.ToString();
			}
			return false;
		}
	}

	private static void CloseDevice()
	{
		if (_serialPort != null)
		{
			_serialPort.Close();
			_serialPort = null;
		}
	}
}
