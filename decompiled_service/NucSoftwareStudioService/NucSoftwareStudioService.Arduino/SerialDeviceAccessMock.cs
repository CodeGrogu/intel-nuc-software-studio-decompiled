using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SerialDeviceAccessLibrary;

namespace NucSoftwareStudioService.Arduino;

public class SerialDeviceAccessMock
{
	private readonly LedPatternType _ledPatternType = new LedPatternType();

	private readonly List<char> _ledChannelType = new List<char> { 'A', 'B', 'C', 'D' };

	private readonly int _sendCommandDelayTime;

	private readonly Dictionary<byte, LedPattern> _LEDStatus;

	private readonly List<byte> _paletteStatus;

	private bool _isPattern8;

	private bool _isPattern9;

	private readonly List<LedPattern> _currentPatterns = new List<LedPattern>();

	private BntStatus _buttonStatus = BntStatus.NotSetting;

	public SerialDeviceAccessMock()
	{
		_LEDStatus = new Dictionary<byte, LedPattern>();
		_paletteStatus = new List<byte>();
		_sendCommandDelayTime = 100;
		foreach (char item in _ledChannelType)
		{
			LedPattern ledPattern = new LedPattern(Convert.ToByte(item), 1, 0, 5, 1, 1);
			_currentPatterns.Add(ledPattern);
			_LEDStatus.Add(Convert.ToByte(item), ledPattern);
		}
		_paletteStatus.Add(0);
		_paletteStatus.Add(0);
		_paletteStatus.Add(0);
		_paletteStatus.Add(0);
		_paletteStatus.Add(96);
		_paletteStatus.Add(160);
		_paletteStatus.Add(0);
		_isPattern8 = false;
		_isPattern9 = false;
	}

	public void SetDeviceInfo(string vid, string pid)
	{
	}

	public bool ConnectToDevice()
	{
		return true;
	}

	public void DisconnectFromDevice()
	{
	}

	public LedPatternType GetSupportedPatterns()
	{
		return _ledPatternType;
	}

	private bool EnterPattern9()
	{
		if (!_isPattern9)
		{
			foreach (LedPattern currentPattern in _currentPatterns)
			{
				if (currentPattern.Channel == 65)
				{
					currentPattern.PatternIndex = 1;
					currentPattern.RainBow = 0;
					currentPattern.Speed = 5;
					currentPattern.MaxBrightness = 5;
					currentPattern.Palette = new List<byte> { 0 };
				}
				else
				{
					currentPattern.PatternIndex = 9;
					currentPattern.RainBow = 1;
					currentPattern.Speed = byte.MaxValue;
					currentPattern.MaxBrightness = 5;
					currentPattern.Palette = new List<byte> { 0 };
				}
			}
			_isPattern9 = true;
			return true;
		}
		return false;
	}

	private bool ExitPattern9()
	{
		if (_isPattern9)
		{
			foreach (LedPattern currentPattern in _currentPatterns)
			{
				currentPattern.PatternIndex = 1;
				currentPattern.RainBow = 0;
				currentPattern.Speed = 5;
				currentPattern.MaxBrightness = 5;
				currentPattern.Palette = new List<byte> { 0 };
			}
			_isPattern9 = false;
			return true;
		}
		return false;
	}

	private bool EnterPattern8()
	{
		if (!_isPattern8)
		{
			foreach (LedPattern currentPattern in _currentPatterns)
			{
				currentPattern.PatternIndex = 8;
				currentPattern.RainBow = 1;
				currentPattern.Speed = byte.MaxValue;
				currentPattern.MaxBrightness = 5;
				currentPattern.Palette = new List<byte> { 0 };
			}
			_isPattern8 = true;
			return true;
		}
		return false;
	}

	private bool ExitPattern8()
	{
		if (_isPattern8)
		{
			foreach (LedPattern currentPattern in _currentPatterns)
			{
				currentPattern.PatternIndex = 1;
				currentPattern.RainBow = 0;
				currentPattern.Speed = 5;
				currentPattern.MaxBrightness = 5;
				currentPattern.Palette = new List<byte> { 0 };
			}
			_isPattern8 = false;
			return true;
		}
		return false;
	}

	public LedStatus SetLEDPattern(List<LedPattern> patternList)
	{
		new List<List<byte>>();
		foreach (LedPattern ledPattern in patternList)
		{
			new List<byte[]>();
			new List<byte>();
			if (_isPattern9 && ledPattern.Channel != 65 && ledPattern.PatternIndex != 9)
			{
				ExitPattern9();
			}
			if (_isPattern8 && ledPattern.PatternIndex != 8)
			{
				ExitPattern8();
			}
			if (ledPattern.PatternIndex == 8)
			{
				EnterPattern8();
				break;
			}
			if (ledPattern.PatternIndex == 9)
			{
				EnterPattern9();
				break;
			}
			if (ThreeColorPattern(ledPattern.PatternIndex))
			{
				LedPattern ledPattern2 = _currentPatterns.Find((LedPattern x) => x.PatternIndex == ledPattern.PatternIndex);
				if (ledPattern2 != null)
				{
					if (ledPattern.PatternIndex != ledPattern2.PatternIndex)
					{
						List<byte> list = CurrentThreeColorPalette();
						if (list != null)
						{
							ledPattern.Palette = list;
						}
					}
					else
					{
						foreach (LedPattern currentPattern in _currentPatterns)
						{
							if (ThreeColorPattern(currentPattern.PatternIndex))
							{
								currentPattern.Palette = ledPattern.Palette;
							}
						}
					}
				}
			}
			foreach (LedPattern currentPattern2 in _currentPatterns)
			{
				if (currentPattern2.Channel == ledPattern.Channel)
				{
					currentPattern2.PatternIndex = ledPattern.PatternIndex;
					currentPattern2.RainBow = ledPattern.RainBow;
					currentPattern2.Speed = ledPattern.Speed;
					currentPattern2.MaxBrightness = ledPattern.MaxBrightness;
					currentPattern2.Palette = ledPattern.Palette;
				}
			}
		}
		return new LedStatus
		{
			LedPatternList = _currentPatterns,
			ButtonStatus = _buttonStatus
		};
	}

	private bool ThreeColorPattern(byte PatternIndex)
	{
		if (PatternIndex == 5 || PatternIndex == 6 || PatternIndex == 7)
		{
			return true;
		}
		return false;
	}

	private List<byte> CurrentThreeColorPalette()
	{
		List<byte> result = null;
		foreach (LedPattern currentPattern in _currentPatterns)
		{
			if (ThreeColorPattern(currentPattern.PatternIndex))
			{
				result = currentPattern.Palette;
				break;
			}
		}
		return result;
	}

	public LedStatus GetCurrentPatterns()
	{
		return new LedStatus
		{
			LedPatternList = _currentPatterns,
			ButtonStatus = _buttonStatus
		};
	}

	public string GetControllerIdentifier()
	{
		return "TestId";
	}

	public bool ResetAllPattern()
	{
		foreach (LedPattern currentPattern in _currentPatterns)
		{
			currentPattern.PatternIndex = 1;
			currentPattern.RainBow = 1;
			currentPattern.Speed = 5;
			currentPattern.MaxBrightness = 5;
			currentPattern.Palette = new List<byte> { 0 };
		}
		_isPattern8 = false;
		_isPattern9 = false;
		return true;
	}

	public bool DisableAll()
	{
		_buttonStatus = BntStatus.Off;
		new LedStatus
		{
			LedPatternList = _currentPatterns,
			ButtonStatus = _buttonStatus
		};
		return true;
	}

	public bool EnableAll()
	{
		_buttonStatus = BntStatus.On;
		new LedStatus
		{
			LedPatternList = _currentPatterns,
			ButtonStatus = _buttonStatus
		};
		return true;
	}

	public void SetSendCommandDelayTime(int delaytime = 100)
	{
	}

	public async Task SendCommandDelayTime()
	{
		await Task.Delay(_sendCommandDelayTime).ConfigureAwait(continueOnCapturedContext: false);
	}
}
