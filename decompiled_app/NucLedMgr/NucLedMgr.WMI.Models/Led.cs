using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using FeatureMgr.Enums;
using FeatureMgr.Models;
using IntelSdkClient.Alexa;
using IntelSdkClient.NucLed;
using Newtonsoft.Json;
using NucLedMgr.Interfaces;
using NucLedMgr.WMI.CpuUsage;
using NucLedMgr.WMI.Enums;
using NucLedMgr.WMI.Events;
using NucLedMgr.WMI.Interfaces;
using NucLedMgr.WMI.Models.ColorTypes;
using NucLedMgr.WMI.Models.Indicators;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace NucLedMgr.WMI.Models;

[JsonObject(MemberSerialization.OptIn)]
public class Led : BindableBase, ICloneable
{
	private IColorManager _colorMgr;

	private IEventAggregator _events;

	private CpuServiceClient _client;

	private string _name;

	private bool _s0Supported;

	private bool _s3Supported;

	private bool _readyModeSupported;

	private bool _s5Supported;

	private bool _gradient;

	private bool _downloadNeeded;

	private ObservableCollection<IIndicator> _supportedIndicators;

	private IIndicator _selectedIndicator;

	private bool _alexaEnable;

	private List<ControlItem> _controlItems;

	private ControlItem _selectedFrequency;

	private ControlItem _selectedBehavior;

	private PowerStateOptions _currentPowerOption;

	private LedType _lightup = LedType.Error;

	private Color _previousColor;

	private Color _selectedColor;

	private Color _hddPreviousColor;

	private NucType _nucType;

	private DelegateCommand _increaseFrequency;

	private DelegateCommand _decreaseFrequency;

	private DelegateCommand _highlightLed;

	private DelegateCommand _unhighlightLed;

	private DelegateCommand __updateUI;

	private DelegateCommand _openMicrosoftLink;

	private NucLedManager _ledService;

	private AlexaSettingsManager _alexaSettingsManager;

	private bool _processingChanges { get; set; }

	[JsonProperty]
	public LedType LedType { get; private set; }

	[JsonProperty]
	public ColorType ColorType { get; private set; }

	public string Name
	{
		get
		{
			return _name;
		}
		set
		{
			SetProperty(ref _name, value, "Name");
		}
	}

	public bool S0Supported
	{
		get
		{
			return _s0Supported;
		}
		set
		{
			SetProperty(ref _s0Supported, value, "S0Supported");
		}
	}

	public bool S3Supported
	{
		get
		{
			return _s3Supported;
		}
		set
		{
			SetProperty(ref _s3Supported, value, "S3Supported");
		}
	}

	public bool ReadyModeSupported
	{
		get
		{
			return _readyModeSupported;
		}
		set
		{
			SetProperty(ref _readyModeSupported, value, "ReadyModeSupported");
		}
	}

	public bool S5Supported
	{
		get
		{
			return _s5Supported;
		}
		set
		{
			SetProperty(ref _s5Supported, value, "S5Supported");
		}
	}

	public string IndicatorName => SelectedIndicator.IndicatorName;

	public bool A4pcDownloadNeeded
	{
		get
		{
			return _downloadNeeded;
		}
		set
		{
			SetProperty(ref _downloadNeeded, value, "A4pcDownloadNeeded");
		}
	}

	private List<IIndicator> indicatorList { get; set; }

	public ObservableCollection<IIndicator> SupportedIndicators
	{
		get
		{
			return _supportedIndicators;
		}
		set
		{
			SetProperty(ref _supportedIndicators, value, "SupportedIndicators");
		}
	}

	[JsonProperty]
	public IIndicator SelectedIndicator
	{
		get
		{
			return _selectedIndicator;
		}
		set
		{
			if (Name == null)
			{
				Name = GetLedName();
			}
			if (SupportedIndicators == null)
			{
				SetProperty(ref _selectedIndicator, value, "SelectedIndicator");
				SetGradient();
				return;
			}
			IIndicator value2 = SupportedIndicators.FirstOrDefault((IIndicator p) => p.IndicatorType.Equals(value.IndicatorType));
			SetProperty(ref _selectedIndicator, value2, "SelectedIndicator");
			_gradient = false;
			if (_selectedIndicator != null && _selectedIndicator.IndicatorType == IndicatorType.PowerState)
			{
				_currentPowerOption = PowerStateOptions.s0;
				GetPowerStateCurrentColor();
				if (ColorType == ColorType.SingleColor)
				{
					PowerStateIndicator_Single powerStateIndicator_Single = (PowerStateIndicator_Single)SelectedIndicator;
					S0Supported = powerStateIndicator_Single.S0Supported;
					S3Supported = powerStateIndicator_Single.S3Supported;
					ReadyModeSupported = false;
					S5Supported = false;
				}
				else
				{
					PowerStateIndicator_Multi powerStateIndicator_Multi = (PowerStateIndicator_Multi)SelectedIndicator;
					S0Supported = powerStateIndicator_Multi.S0Supported;
					S3Supported = powerStateIndicator_Multi.S3Supported;
					ReadyModeSupported = powerStateIndicator_Multi.ReadyModeSupported;
					S5Supported = powerStateIndicator_Multi.S5Supported;
				}
			}
			else
			{
				_currentPowerOption = PowerStateOptions.none;
				if (_selectedIndicator != null && _selectedIndicator.IndicatorType == IndicatorType.PowerLimit)
				{
					List<ControlItem> controlItems = new List<ControlItem>
					{
						new ControlItem(0, 1, supported: true)
					};
					ControlItems = controlItems;
					_gradient = true;
				}
				else if (_selectedIndicator != null)
				{
					_ = _selectedIndicator.IndicatorType;
					_ = 4;
				}
			}
			if (_events != null)
			{
				Task.Run(() =>
				{
					_events.GetEvent<LedsLoadedEvent>().Publish(payload: false);
					ChangeLed();
					_events.GetEvent<LedsLoadedEvent>().Publish(payload: true);
				});
			}
		}
	}

	[JsonProperty]
	public bool AlexaEnable
	{
		get
		{
			return _alexaEnable;
		}
		set
		{
			SetProperty(ref _alexaEnable, value, "AlexaEnable");
			if (_alexaSettingsManager != null)
			{
				_alexaSettingsManager.SetLightBarState(_alexaEnable);
			}
			RaisePropertyChanged("AlexaEnable");
		}
	}

	public List<ControlItem> ControlItems
	{
		get
		{
			return _controlItems;
		}
		set
		{
			SetProperty(ref _controlItems, value, "ControlItems");
			SelectedIndicator.SetControlItems(value);
			Task.Run(() =>
			{
				foreach (ControlItem controlItem in ControlItems)
				{
					if (controlItem.Supported)
					{
						WmiWrapper.SetControlItem(LedType, SelectedIndicator.IndicatorType, controlItem.Index, controlItem.Value, _ledService.Wmi, NucType);
					}
				}
			});
		}
	}

	public ControlItem SelectedFrequency
	{
		get
		{
			return _selectedFrequency;
		}
		set
		{
			SetProperty(ref _selectedFrequency, value, "SelectedFrequency");
			if (_events != null)
			{
				UpdateControlItems();
				RaisePropertyChanged("SelectedFrequency");
				_events.GetEvent<LedColorBehaviorChangeEvent>().Publish(this);
			}
		}
	}

	public ControlItem SelectedBehavior
	{
		get
		{
			return _selectedBehavior;
		}
		set
		{
			SetProperty(ref _selectedBehavior, value, "SelectedBehavior");
			if (_events != null)
			{
				UpdateControlItems();
				if (SelectedIndicator.IndicatorType == IndicatorType.PowerLimit)
				{
					GetSelectedColors();
				}
				_events.GetEvent<LedColorBehaviorChangeEvent>().Publish(this);
			}
		}
	}

	public PowerStateOptions CurrentPowerOption
	{
		get
		{
			return _currentPowerOption;
		}
		set
		{
			if (SelectedIndicator.IndicatorType != IndicatorType.PowerState)
			{
				_currentPowerOption = PowerStateOptions.none;
			}
			else
			{
				SetProperty(ref _currentPowerOption, value, "CurrentPowerOption");
				GetPowerStateCurrentColor();
				SetBehaviorAndFrequency();
				RefreshUi();
			}
			if (_events != null)
			{
				_events.GetEvent<LedColorBehaviorChangeEvent>().Publish(this);
			}
		}
	}

	public LedType Lightup
	{
		get
		{
			return _lightup;
		}
		set
		{
			SetProperty(ref _lightup, value, "Lightup");
		}
	}

	[JsonProperty]
	public Color PreviousColor
	{
		get
		{
			return _previousColor;
		}
		set
		{
			SetProperty(ref _previousColor, value, "PreviousColor");
		}
	}

	public Color SelectedColor
	{
		get
		{
			return _selectedColor;
		}
		set
		{
			SetProperty(ref _selectedColor, value, "SelectedColor");
		}
	}

	[JsonProperty]
	public Color HddPreviousColor
	{
		get
		{
			return _hddPreviousColor;
		}
		set
		{
			SetProperty(ref _hddPreviousColor, value, "HddPreviousColor");
		}
	}

	[JsonProperty]
	public NucType NucType
	{
		get
		{
			return _nucType;
		}
		set
		{
			SetProperty(ref _nucType, value, "NucType");
		}
	}

	public ObservableCollection<WmiColorPickerData> SelectedColors { get; set; }

	public DelegateCommand IncreaseFrequency
	{
		get
		{
			return _increaseFrequency;
		}
		set
		{
			SetProperty(ref _increaseFrequency, value, "IncreaseFrequency");
		}
	}

	public DelegateCommand DecreaseFrequency
	{
		get
		{
			return _decreaseFrequency;
		}
		set
		{
			SetProperty(ref _decreaseFrequency, value, "DecreaseFrequency");
		}
	}

	public DelegateCommand HighlightLed
	{
		get
		{
			return _highlightLed;
		}
		set
		{
			SetProperty(ref _highlightLed, value, "HighlightLed");
		}
	}

	public DelegateCommand UnhighlightLed
	{
		get
		{
			return _unhighlightLed;
		}
		set
		{
			SetProperty(ref _unhighlightLed, value, "UnhighlightLed");
		}
	}

	public DelegateCommand UpdateUI
	{
		get
		{
			return __updateUI;
		}
		set
		{
			SetProperty(ref __updateUI, value, "UpdateUI");
		}
	}

	public DelegateCommand OpenMicrosoftLink
	{
		get
		{
			return _openMicrosoftLink;
		}
		set
		{
			SetProperty(ref _openMicrosoftLink, value, "OpenMicrosoftLink");
		}
	}

	public Led(NucLedManager ledService, AlexaSettingsManager alexaSettingsManager, LedType ledType, IColorManager colorMgr, IEventAggregator events, CpuServiceClient client, NucType nucType = NucType.Error)
	{
		_ledService = ledService;
		_alexaSettingsManager = alexaSettingsManager;
		LedType = ledType;
		_colorMgr = colorMgr;
		_events = events;
		_client = client;
		_nucType = nucType;
		_name = GetLedName();
		_currentPowerOption = PowerStateOptions.none;
		ColorType = WmiWrapper.GetColorType(LedType, _ledService.Wmi, nucType);
		if (ColorType > ColorType.RGB || ColorType < ColorType.DualAmber)
		{
			ColorType = ColorType.RGB;
		}
		GetSelectedIndicator();
		if (SelectedIndicator != null)
		{
			if (_selectedIndicator.IndicatorType == IndicatorType.PowerLimit || _selectedIndicator.IndicatorType == IndicatorType.Software)
			{
				_gradient = true;
			}
			if (SelectedIndicator.ControlItems != null)
			{
				ControlItems = SelectedIndicator.ControlItems;
			}
			GetSelectedColors();
			Thread thread = new Thread(() =>
			{
				GetSupportedIndicators();
				SetBehaviorAndFrequency();
			});
			thread.IsBackground = true;
			thread.Start();
			_downloadNeeded = true;
			if (!AlexaPcDownloadNeeded())
			{
				_downloadNeeded = false;
			}
			_highlightLed = new DelegateCommand(ToggleHighlight);
			_unhighlightLed = new DelegateCommand(ToggleHighlight);
			__updateUI = new DelegateCommand(RefreshUi);
			_increaseFrequency = new DelegateCommand(Increase);
			_decreaseFrequency = new DelegateCommand(Decrease);
			_openMicrosoftLink = new DelegateCommand(OpenLink);
		}
	}

	[JsonConstructor]
	public Led()
	{
		_currentPowerOption = PowerStateOptions.none;
		_downloadNeeded = true;
		if (!AlexaPcDownloadNeeded())
		{
			_downloadNeeded = false;
		}
		_highlightLed = new DelegateCommand(ToggleHighlight);
		_unhighlightLed = new DelegateCommand(ToggleHighlight);
		__updateUI = new DelegateCommand(RefreshUi);
		_increaseFrequency = new DelegateCommand(Increase);
		_decreaseFrequency = new DelegateCommand(Decrease);
		_openMicrosoftLink = new DelegateCommand(OpenLink);
	}

	private void ChangeLed()
	{
		IndicatorType indicatorType = _selectedIndicator.IndicatorType;
		if (NucType == NucType.Serpent)
		{
			if (indicatorType != IndicatorType.Alexa)
			{
				if (LedType == LedType.Hdd)
				{
					AlexaEnable = false;
					WmiWrapper.SetAlexa(AlexaEnable, _ledService.Wmi);
					SelectedColor = HddPreviousColor;
				}
				WmiWrapper.SetIndicator(LedType, indicatorType, _ledService.Wmi, NucType);
				GetSelectedIndicator();
				if (SelectedColor != Colors.Black)
				{
					_previousColor = SelectedColor;
				}
				if (_selectedIndicator.IndicatorType == IndicatorType.Disable)
				{
					_selectedIndicator.ColorSetting.CurrentColor = Colors.Black;
				}
				else
				{
					_selectedIndicator.ColorSetting.CurrentColor = _previousColor;
					SelectedColor = _selectedIndicator.ColorSetting.CurrentColor;
					HddPreviousColor = SelectedColor;
				}
				SetBehaviorAndFrequency();
				UpdateControlItems();
				GetSelectedColors();
				RefreshUi();
			}
			else if (LedType == LedType.Hdd)
			{
				HddPreviousColor = SelectedColor;
				AlexaEnable = true;
				WmiWrapper.SetAlexa(AlexaEnable, _ledService.Wmi);
			}
		}
		else
		{
			WmiWrapper.SetIndicator(LedType, indicatorType, _ledService.Wmi, NucType);
			GetSelectedIndicator();
			if (SelectedColor != Colors.Black)
			{
				_previousColor = SelectedColor;
			}
			if (_selectedIndicator.IndicatorType == IndicatorType.Disable)
			{
				_selectedIndicator.ColorSetting.CurrentColor = Colors.Black;
			}
			else
			{
				_selectedIndicator.ColorSetting.CurrentColor = _previousColor;
				SelectedColor = _selectedIndicator.ColorSetting.CurrentColor;
			}
			SetBehaviorAndFrequency();
			UpdateControlItems();
			GetSelectedColors();
		}
	}

	public void SetLedPriviousColor(Color color)
	{
		_previousColor = color;
	}

	private void GetPowerStateCurrentColor()
	{
		if (SelectedIndicator is PowerStateIndicator_Single powerStateIndicator_Single)
		{
			switch (CurrentPowerOption)
			{
			case PowerStateOptions.s0:
				SelectedColor = powerStateIndicator_Single.S0.Color.CurrentColor;
				break;
			case PowerStateOptions.s3:
				SelectedColor = powerStateIndicator_Single.S3.Color.CurrentColor;
				break;
			}
		}
		else if (SelectedIndicator is PowerStateIndicator_Multi powerStateIndicator_Multi)
		{
			switch (CurrentPowerOption)
			{
			case PowerStateOptions.s0:
				SelectedColor = powerStateIndicator_Multi.S0.ColorSetting.CurrentColor;
				break;
			case PowerStateOptions.s3:
				SelectedColor = powerStateIndicator_Multi.S3.ColorSetting.CurrentColor;
				break;
			case PowerStateOptions.readyMode:
				SelectedColor = powerStateIndicator_Multi.ReadyMode.ColorSetting.CurrentColor;
				break;
			case PowerStateOptions.s5:
				SelectedColor = powerStateIndicator_Multi.S5.ColorSetting.CurrentColor;
				break;
			}
		}
	}

	public void GetSelectedColors()
	{
		SelectedColors = new ObservableCollection<WmiColorPickerData>
		{
			new WmiColorPickerData(0, enabled: true, _gradient, _colorMgr, Colors.Transparent, ColorChangeOccured),
			new WmiColorPickerData(0, enabled: true, _gradient, _colorMgr, Colors.Transparent, ColorChangeOccured),
			new WmiColorPickerData(0, enabled: true, _gradient, _colorMgr, Colors.Transparent, ColorChangeOccured),
			new WmiColorPickerData(0, enabled: true, _gradient, _colorMgr, Colors.Transparent, ColorChangeOccured)
		};
		if (SelectedIndicator is PowerStateIndicator_Single powerStateIndicator_Single)
		{
			if (S3Supported)
			{
				if (powerStateIndicator_Single.S3.Color.CurrentColor == Colors.Black)
				{
					powerStateIndicator_Single.S3.Color.CurrentColor = Colors.Blue;
				}
				SelectedColors[1].PlaceholderColor = powerStateIndicator_Single.S3.Color.CurrentColor;
			}
			if (S0Supported)
			{
				if (powerStateIndicator_Single.S0.Color.CurrentColor == Colors.Black)
				{
					powerStateIndicator_Single.S0.Color.CurrentColor = Colors.Blue;
				}
				SelectedColors[0].SelectedColor = powerStateIndicator_Single.S0.Color.CurrentColor;
				SelectedColors[0].CurrentColor = powerStateIndicator_Single.S0.Color.CurrentColor;
			}
		}
		else if (SelectedIndicator is PowerStateIndicator_Multi powerStateIndicator_Multi)
		{
			if (S5Supported)
			{
				if (powerStateIndicator_Multi.S5.ColorSetting.CurrentColor == Colors.Black)
				{
					powerStateIndicator_Multi.S5.ColorSetting.CurrentColor = Colors.Blue;
				}
				SelectedColors[3].PlaceholderColor = powerStateIndicator_Multi.S5.ColorSetting.CurrentColor;
			}
			if (ReadyModeSupported)
			{
				if (powerStateIndicator_Multi.ReadyMode.ColorSetting.CurrentColor == Colors.Black)
				{
					powerStateIndicator_Multi.ReadyMode.ColorSetting.CurrentColor = Colors.Blue;
				}
				SelectedColors[2].PlaceholderColor = powerStateIndicator_Multi.ReadyMode.ColorSetting.CurrentColor;
				SelectedColors[2].CurrentColor = powerStateIndicator_Multi.ReadyMode.ColorSetting.CurrentColor;
			}
			if (S3Supported)
			{
				if (powerStateIndicator_Multi.S3.ColorSetting.CurrentColor == Colors.Black)
				{
					powerStateIndicator_Multi.S3.ColorSetting.CurrentColor = Colors.Blue;
				}
				SelectedColors[1].PlaceholderColor = powerStateIndicator_Multi.S3.ColorSetting.CurrentColor;
			}
			if (S0Supported)
			{
				if (powerStateIndicator_Multi.S0.ColorSetting.CurrentColor == Colors.Black)
				{
					powerStateIndicator_Multi.S0.ColorSetting.CurrentColor = Colors.Blue;
				}
				SelectedColors[0].SelectedColor = powerStateIndicator_Multi.S0.ColorSetting.CurrentColor;
				SelectedColors[0].CurrentColor = powerStateIndicator_Multi.S0.ColorSetting.CurrentColor;
			}
		}
		else if (SelectedIndicator is PowerLimitIndicator_Multi powerLimitIndicator_Multi)
		{
			if (powerLimitIndicator_Multi.GetControlItems().Find((ControlItem i) => i.Index == 0).Value == 0)
			{
				SelectedColors[1].PlaceholderColor = Colors.Red;
				SelectedColors[0].SelectedColor = Colors.Green;
			}
			else
			{
				SelectedColors[1].PlaceholderColor = Colors.Black;
				SelectedColors[0].SelectedColor = powerLimitIndicator_Multi.ColorSetting.CurrentColor;
				SelectedColors[0].CurrentColor = powerLimitIndicator_Multi.ColorSetting.CurrentColor;
			}
		}
		else if (!(SelectedIndicator is SoftwareIndicator_Multi))
		{
			SelectedColors[0].SelectedColor = SelectedIndicator.ColorSetting.CurrentColor;
			SelectedColors[0].CurrentColor = SelectedIndicator.ColorSetting.CurrentColor;
		}
		SelectedColor = SelectedColors[0].SelectedColor;
		RaisePropertyChanged("SelectedColors");
	}

	private void InitializeColor(Color color, PowerStateOptions option = PowerStateOptions.none)
	{
		byte index;
		byte index2;
		byte index3;
		switch (option)
		{
		default:
			return;
		case PowerStateOptions.s0:
			index = 3;
			index2 = 4;
			index3 = 5;
			break;
		case PowerStateOptions.s3:
			index = 9;
			index2 = 10;
			index3 = 11;
			break;
		case PowerStateOptions.readyMode:
			index = 15;
			index2 = 16;
			index3 = 17;
			break;
		case PowerStateOptions.s5:
			index = 21;
			index2 = 22;
			index3 = 23;
			break;
		case PowerStateOptions.none:
			switch (SelectedIndicator.IndicatorType)
			{
			default:
				return;
			case IndicatorType.Ethernet:
				index = 2;
				index2 = 3;
				index3 = 4;
				break;
			case IndicatorType.PowerLimit:
				index = 2;
				index2 = 3;
				index3 = 4;
				break;
			case IndicatorType.Software:
				index = 3;
				index2 = 4;
				index3 = 5;
				break;
			case IndicatorType.HddActivity:
				index = 1;
				index2 = 2;
				index3 = 3;
				break;
			case IndicatorType.Wifi:
				index = 1;
				index2 = 2;
				index3 = 3;
				break;
			}
			break;
		}
		List<ControlItem> controlItems = SelectedIndicator.ControlItems;
		List<ControlItem> list = new List<ControlItem>();
		switch (ColorType)
		{
		default:
			return;
		case ColorType.DualAmber:
			if (color == DualColor.ColorBlue)
			{
				list.Add(new ControlItem(index, 0, supported: true));
			}
			else if (color == DualColor.ColorWhiteAmber)
			{
				list.Add(new ControlItem(index, 1, supported: true));
			}
			else
			{
				list.Add(new ControlItem(index, byte.MaxValue, supported: true));
			}
			break;
		case ColorType.DualWhite:
			if (color == DualColor.ColorBlue)
			{
				list.Add(new ControlItem(index, 0, supported: true));
			}
			else if (color == DualColor.ColorWhiteAmber)
			{
				list.Add(new ControlItem(index, 1, supported: true));
			}
			else
			{
				list.Add(new ControlItem(index, byte.MaxValue, supported: true));
			}
			break;
		case ColorType.RGB:
			if (NucType == NucType.Panther)
			{
				if (color == Colors.Red)
				{
					list.Add(new ControlItem(index, 1, supported: true));
				}
				else if (color == Colors.Orange)
				{
					list.Add(new ControlItem(index, 2, supported: true));
				}
				else if (color == Colors.Yellow)
				{
					list.Add(new ControlItem(index, 3, supported: true));
				}
				else if (color == Colors.Green)
				{
					list.Add(new ControlItem(index, 4, supported: true));
				}
				else if (color == Colors.Blue)
				{
					if (SelectedIndicator.IndicatorType == IndicatorType.PowerState && LedType == LedType.PowerButton)
					{
						list.Add(new ControlItem(index, 0, supported: true));
					}
					else
					{
						list.Add(new ControlItem(index, 5, supported: true));
					}
				}
				else if (color == Colors.Indigo)
				{
					list.Add(new ControlItem(index, 6, supported: true));
				}
				else if (color == Colors.Violet)
				{
					list.Add(new ControlItem(index, 7, supported: true));
				}
				else if (color == Colors.White)
				{
					list.Add(new ControlItem(index, 8, supported: true));
				}
				else if (color == Color.FromArgb(byte.MaxValue, byte.MaxValue, 194, 0))
				{
					list.Add(new ControlItem(index, 1, supported: true));
				}
				else
				{
					list.Add(new ControlItem(index, 5, supported: true));
				}
			}
			else
			{
				list.Add(new ControlItem(index, color.R, supported: true));
				list.Add(new ControlItem(index2, color.G, supported: true));
				list.Add(new ControlItem(index3, color.B, supported: true));
			}
			break;
		}
		foreach (ControlItem item in list)
		{
			foreach (ControlItem i in controlItems)
			{
				if (item.Index == i.Index)
				{
					i.Value = item.Value;
					Task.Run(() =>
					{
						WmiWrapper.SetControlItem(LedType, SelectedIndicator.IndicatorType, i.Index, i.Value, _ledService.Wmi, NucType);
					});
					break;
				}
			}
		}
		SelectedIndicator.SetControlItems(controlItems);
		SelectedColor = color;
		SelectedIndicator.ColorSetting.CurrentColor = color;
	}

	public void SetGradient()
	{
		switch (SelectedIndicator.IndicatorType)
		{
		case IndicatorType.Error:
		case IndicatorType.PowerState:
		case IndicatorType.HddActivity:
		case IndicatorType.Ethernet:
		case IndicatorType.Wifi:
		case IndicatorType.Disable:
			_gradient = false;
			break;
		case IndicatorType.Software:
		case IndicatorType.PowerLimit:
			_gradient = true;
			break;
		}
	}

	public void SetInterfaces(IEventAggregator events, IColorManager colorMgr, CpuServiceClient client, NucLedManager ledService, AlexaSettingsManager alexaSettingsManager)
	{
		_ledService = ledService;
		_alexaSettingsManager = alexaSettingsManager;
		_events = events;
		_colorMgr = colorMgr;
		_client = client;
		Thread thread = new Thread(() =>
		{
			if (_selectedIndicator != null && _selectedIndicator.IndicatorType == IndicatorType.PowerState)
			{
				_currentPowerOption = PowerStateOptions.s0;
				if (ColorType == ColorType.SingleColor)
				{
					PowerStateIndicator_Single powerStateIndicator_Single = (PowerStateIndicator_Single)SelectedIndicator;
					S0Supported = powerStateIndicator_Single.S0Supported;
					S3Supported = powerStateIndicator_Single.S3Supported;
					ReadyModeSupported = false;
					S5Supported = false;
				}
				else
				{
					PowerStateIndicator_Multi powerStateIndicator_Multi = (PowerStateIndicator_Multi)SelectedIndicator;
					S0Supported = powerStateIndicator_Multi.S0Supported;
					S3Supported = powerStateIndicator_Multi.S3Supported;
					ReadyModeSupported = powerStateIndicator_Multi.ReadyModeSupported;
					S5Supported = powerStateIndicator_Multi.S5Supported;
				}
			}
			_controlItems = SelectedIndicator.GetControlItems();
			GetSelectedColors();
			GetSupportedIndicators();
			SetBehaviorAndFrequency();
			RefreshUi();
		});
		thread.IsBackground = true;
		thread.Start();
	}

	private void Increase()
	{
		if (!_processingChanges)
		{
			_processingChanges = true;
			if (SelectedFrequency != null)
			{
				SelectedFrequency.Value++;
			}
			UpdateControlItems();
			RaisePropertyChanged("SelectedFrequency");
			_events.GetEvent<LedColorBehaviorChangeEvent>().Publish(this);
			_processingChanges = false;
		}
	}

	private void Decrease()
	{
		if (!_processingChanges)
		{
			_processingChanges = true;
			if (SelectedFrequency != null)
			{
				SelectedFrequency.Value--;
			}
			UpdateControlItems();
			RaisePropertyChanged("SelectedFrequency");
			_events.GetEvent<LedColorBehaviorChangeEvent>().Publish(this);
			_processingChanges = false;
		}
	}

	public void GetIndicatorFromCollection()
	{
		if (NucType == NucType.Serpent)
		{
			AlexaEnable = WmiWrapper.QueryAlexa(_ledService.Wmi);
			if (AlexaEnable && LedType == LedType.Hdd)
			{
				_selectedIndicator = SupportedIndicators.FirstOrDefault((IIndicator p) => p.IndicatorType.Equals(IndicatorType.Alexa));
			}
			else
			{
				_selectedIndicator = SupportedIndicators.FirstOrDefault((IIndicator p) => p.IndicatorType.Equals(SelectedIndicator.IndicatorType));
			}
			RaisePropertyChanged("SelectedIndicator");
			RaisePropertyChanged("SupportedIndicators");
		}
		else
		{
			_selectedIndicator = SupportedIndicators.FirstOrDefault((IIndicator p) => p.IndicatorType.Equals(SelectedIndicator.IndicatorType));
			RaisePropertyChanged("SelectedIndicator");
			RaisePropertyChanged("SupportedIndicators");
		}
	}

	public void GetSupportedIndicators()
	{
		List<IndicatorType> supportedIndicators = WmiWrapper.GetSupportedIndicators(LedType, _ledService.Wmi, NucType);
		if (supportedIndicators == null)
		{
			return;
		}
		supportedIndicators.Remove(SelectedIndicator.IndicatorType);
		supportedIndicators.Remove(IndicatorType.Software);
		indicatorList = new List<IIndicator>();
		foreach (IndicatorType item2 in supportedIndicators)
		{
			BitArray supportedControlItems = WmiWrapper.GetSupportedControlItems(LedType, item2, _ledService.Wmi, NucType);
			if (SelectedIndicator.ColorSetting != null)
			{
				IIndicator indicator = GetIndicator(item2, supportedControlItems, SelectedIndicator.ColorSetting.CurrentColor);
				if (indicator != null && !indicatorList.Contains(indicator))
				{
					indicatorList.Add(indicator);
				}
			}
		}
		SupportedIndicators = new ObservableCollection<IIndicator>(indicatorList) { _selectedIndicator };
		SupportedIndicators = new ObservableCollection<IIndicator>(SupportedIndicators.OrderBy((IIndicator Indicator) => Indicator.IndicatorType));
		if (_nucType == NucType.Serpent)
		{
			if (SupportedIndicators.Count > 0)
			{
				IIndicator indicator2 = GetIndicator(IndicatorType.Alexa, null, SelectedIndicator.ColorSetting.CurrentColor);
				IIndicator item = SupportedIndicators.FirstOrDefault((IIndicator x) => x.IndicatorType == IndicatorType.Alexa);
				if (!SupportedIndicators.Contains(item))
				{
					int insertionIndex = SupportedIndicators.Count - 1;
					Application.Current.Dispatcher.Invoke(() =>
					{
						SupportedIndicators.Insert(insertionIndex, indicator2);
					});
				}
				else
				{
					indicator2 = GetIndicator(IndicatorType.Alexa, null, SelectedIndicator.ColorSetting.CurrentColor);
					item = SupportedIndicators.FirstOrDefault((IIndicator x) => x.IndicatorType == IndicatorType.Alexa);
					SupportedIndicators.Remove(item);
					if (!SupportedIndicators.Contains(item))
					{
						int insertionIndex2 = SupportedIndicators.Count - 1;
						Application.Current.Dispatcher.Invoke(() =>
						{
							SupportedIndicators.Insert(insertionIndex2, indicator2);
						});
					}
				}
			}
			if (SupportedIndicators != null)
			{
				if (Name == LedConstants.MULTIPURPOSELED_NAME)
				{
					IEnumerable<IIndicator> enumerable = SupportedIndicators.Where((IIndicator x) => x.IndicatorType == IndicatorType.PowerState);
					if (enumerable != null && enumerable.ToList().Count > 0)
					{
						IIndicator indicator3 = SupportedIndicators.Where((IIndicator x) => x.IndicatorType == IndicatorType.PowerState)?.ToList()[0];
						Application.Current.Dispatcher.Invoke(() =>
						{
							SupportedIndicators.Remove(indicator3);
						});
					}
				}
				else
				{
					IEnumerable<IIndicator> enumerable2 = SupportedIndicators.Where((IIndicator x) => x.IndicatorType == IndicatorType.Alexa);
					if (enumerable2 != null && enumerable2.ToList().Count > 0)
					{
						IIndicator indicator4 = SupportedIndicators.Where((IIndicator x) => x.IndicatorType == IndicatorType.Alexa)?.ToList()[0];
						Application.Current.Dispatcher.Invoke(() =>
						{
							SupportedIndicators.Remove(indicator4);
						});
					}
				}
			}
		}
		GetIndicatorFromCollection();
	}

	public void GetSelectedIndicator()
	{
		IndicatorType indicator = WmiWrapper.GetIndicator(LedType, _ledService.Wmi);
		if (LedType == LedType.RGBHeader || indicator < IndicatorType.PowerState || indicator > IndicatorType.Disable)
		{
			return;
		}
		BitArray supportedControlItems = WmiWrapper.GetSupportedControlItems(LedType, indicator, _ledService.Wmi, NucType);
		List<ControlItem> list = new List<ControlItem>();
		for (byte b = 0; b < supportedControlItems.Count; b++)
		{
			if (supportedControlItems[b])
			{
				byte? controlItemValue = WmiWrapper.GetControlItemValue(LedType, indicator, b, _ledService.Wmi);
				if (controlItemValue.HasValue)
				{
					list.Add(new ControlItem(b, controlItemValue.Value, supported: true));
				}
			}
		}
		list.RemoveAll((ControlItem item) => item == null);
		_selectedIndicator = InitiatesIndicator(indicator, list);
		_controlItems = list;
	}

	public IIndicator InitiatesIndicator(IndicatorType selected, List<ControlItem> controlItems)
	{
		switch (selected)
		{
		case IndicatorType.Ethernet:
			if (ColorType != ColorType.SingleColor)
			{
				return new EthernetIndicator_Multi(ColorType, controlItems);
			}
			break;
		case IndicatorType.HddActivity:
			if (ColorType == ColorType.SingleColor)
			{
				return new HDDIndicator_Single(ColorType, controlItems);
			}
			return new HDDIndicator_Multi(ColorType, controlItems, NucType);
		case IndicatorType.PowerLimit:
			if (ColorType != ColorType.SingleColor)
			{
				return new PowerLimitIndicator_Multi(ColorType, controlItems);
			}
			break;
		case IndicatorType.PowerState:
		{
			if (ColorType == ColorType.SingleColor)
			{
				PowerStateIndicator_Single powerStateIndicator_Single = new PowerStateIndicator_Single(ColorType, controlItems);
				S0Supported = powerStateIndicator_Single.S0Supported;
				S3Supported = powerStateIndicator_Single.S3Supported;
				ReadyModeSupported = false;
				S5Supported = false;
				if (S0Supported)
				{
					_currentPowerOption = PowerStateOptions.s0;
				}
				return powerStateIndicator_Single;
			}
			PowerStateIndicator_Multi powerStateIndicator_Multi = new PowerStateIndicator_Multi(ColorType, controlItems, NucType, LedType);
			if (NucType == NucType.Panther)
			{
				S0Supported = true;
			}
			else
			{
				S0Supported = powerStateIndicator_Multi.S0Supported;
			}
			S3Supported = powerStateIndicator_Multi.S3Supported;
			ReadyModeSupported = powerStateIndicator_Multi.ReadyModeSupported;
			S5Supported = powerStateIndicator_Multi.S5Supported;
			if (S0Supported)
			{
				_currentPowerOption = PowerStateOptions.s0;
			}
			return powerStateIndicator_Multi;
		}
		case IndicatorType.Software:
			if (ColorType != ColorType.SingleColor)
			{
				return new SoftwareIndicator_Multi(ColorType, controlItems);
			}
			break;
		case IndicatorType.Wifi:
			if (ColorType != ColorType.SingleColor)
			{
				return new WifiIndicator_Multi(ColorType, controlItems);
			}
			break;
		case IndicatorType.Disable:
			return new DisableIndicator(ColorType);
		case IndicatorType.Alexa:
			return new AlexaIndicator();
		}
		return null;
	}

	public void ColorChangeOccured(Color color)
	{
		InitializeColor(color, CurrentPowerOption);
		RaisePropertyChanged("SelectedColor");
		RaisePropertyChanged("SelectedColors");
		if (_events != null)
		{
			_events.GetEvent<LedColorBehaviorChangeEvent>().Publish(this);
		}
		RefreshUi();
	}

	private IIndicator GetIndicator(IndicatorType selected, BitArray controlItems, Color color)
	{
		switch (selected)
		{
		case IndicatorType.Ethernet:
			if (ColorType != ColorType.SingleColor)
			{
				return new EthernetIndicator_Multi(ColorType, controlItems, color);
			}
			break;
		case IndicatorType.HddActivity:
			if (ColorType == ColorType.SingleColor)
			{
				return new HDDIndicator_Single(ColorType, controlItems, color);
			}
			return new HDDIndicator_Multi(ColorType, controlItems, color);
		case IndicatorType.PowerLimit:
			if (ColorType != ColorType.SingleColor)
			{
				return new PowerLimitIndicator_Multi(ColorType, controlItems, color);
			}
			break;
		case IndicatorType.PowerState:
		{
			if (ColorType == ColorType.SingleColor)
			{
				return new PowerStateIndicator_Single(ColorType, controlItems, color);
			}
			PowerStateIndicator_Multi powerStateIndicator_Multi = new PowerStateIndicator_Multi(ColorType, controlItems, color);
			S0Supported = powerStateIndicator_Multi.ReadyModeSupported && powerStateIndicator_Multi.S5Supported;
			return powerStateIndicator_Multi;
		}
		case IndicatorType.Software:
			if (ColorType != ColorType.SingleColor)
			{
				return new SoftwareIndicator_Multi(ColorType, controlItems, color);
			}
			break;
		case IndicatorType.Wifi:
			if (ColorType != ColorType.SingleColor)
			{
				return new WifiIndicator_Multi(ColorType, controlItems, color);
			}
			break;
		case IndicatorType.Alexa:
			if (ColorType != ColorType.SingleColor)
			{
				return new AlexaIndicator();
			}
			break;
		case IndicatorType.Disable:
			return new DisableIndicator(ColorType, controlItems);
		}
		return null;
	}

	private void ToggleHighlight()
	{
		if (_lightup == LedType.Error)
		{
			_lightup = LedType;
		}
		else
		{
			_lightup = LedType.Error;
		}
		RaisePropertyChanged("Lightup");
	}

	private string GetLedName()
	{
		_nucType = PlatformDetail.GetNucType();
		if (_nucType == NucType.Serpent)
		{
			return LedType switch
			{
				LedType.Eyes => LedConstants.EYES_NAME, 
				LedType.Front1 => LedConstants.FRONT1_NAME, 
				LedType.Front2 => LedConstants.FRONT2_NAME, 
				LedType.Front3 => LedConstants.FRONT3_NAME, 
				LedType.Skull => LedConstants.SKULL_NAME, 
				LedType.PowerButton => LedConstants.POWER_BUTTON_NAME, 
				LedType.Hdd => LedConstants.MULTIPURPOSELED_NAME, 
				_ => "", 
			};
		}
		return LedType switch
		{
			LedType.Eyes => LedConstants.EYES_NAME, 
			LedType.Front1 => LedConstants.FRONT1_NAME, 
			LedType.Front2 => LedConstants.FRONT2_NAME, 
			LedType.Front3 => LedConstants.FRONT3_NAME, 
			LedType.Skull => LedConstants.SKULL_NAME, 
			LedType.PowerButton => LedConstants.POWER_BUTTON_NAME, 
			LedType.Hdd => LedConstants.HDD_NAME, 
			_ => "", 
		};
	}

	private void UpdateControlItems()
	{
		if (SelectedIndicator.IndicatorType == IndicatorType.Wifi)
		{
			return;
		}
		List<ControlItem> controlItems = SelectedIndicator.ControlItems;
		byte? b;
		switch (SelectedIndicator.IndicatorType)
		{
		case IndicatorType.PowerState:
		case IndicatorType.HddActivity:
		case IndicatorType.Wifi:
		case IndicatorType.Software:
			b = 0;
			break;
		case IndicatorType.Ethernet:
		case IndicatorType.PowerLimit:
			b = (byte)1;
			break;
		default:
			b = null;
			break;
		}
		bool flag = false;
		foreach (ControlItem i in controlItems)
		{
			if (_selectedBehavior != null && _selectedBehavior.Index == i.Index)
			{
				i.Value = _selectedBehavior.Value;
				Task.Run(() =>
				{
					WmiWrapper.SetControlItem(LedType, SelectedIndicator.IndicatorType, i.Index, i.Value, _ledService.Wmi, NucType);
				});
				flag = true;
				break;
			}
			if (i.Index == b)
			{
				i.Value = 50;
				Task.Run(() =>
				{
					WmiWrapper.SetControlItem(LedType, SelectedIndicator.IndicatorType, i.Index, i.Value, _ledService.Wmi, NucType);
				});
			}
		}
		if (!flag && _selectedBehavior != null)
		{
			controlItems.Add(_selectedBehavior);
		}
		flag = false;
		foreach (ControlItem i2 in controlItems)
		{
			if (_selectedFrequency != null && _selectedFrequency.Index == i2.Index)
			{
				i2.Value = _selectedFrequency.Value;
				Task.Run(() =>
				{
					WmiWrapper.SetControlItem(LedType, SelectedIndicator.IndicatorType, i2.Index, i2.Value, _ledService.Wmi, NucType);
				});
				flag = true;
				break;
			}
		}
		if (!flag && _selectedFrequency != null)
		{
			controlItems.Add(_selectedFrequency);
		}
		SelectedIndicator.SetControlItems(controlItems);
		if (NucType == NucType.Panther)
		{
			ControlItems = new List<ControlItem>(controlItems);
		}
	}

	public void SetBehaviorAndFrequency()
	{
		_selectedFrequency = null;
		_selectedBehavior = null;
		List<ControlItem> controlItems = SelectedIndicator.ControlItems;
		byte? behaviorByte = null;
		byte? frequencyByte = null;
		byte? b = null;
		byte? b2;
		switch (SelectedIndicator.IndicatorType)
		{
		case IndicatorType.PowerState:
			if (SelectedIndicator is PowerStateIndicator_Single)
			{
				if (CurrentPowerOption == PowerStateOptions.s3)
				{
					behaviorByte = 7;
					frequencyByte = 8;
					b = 6;
				}
				else
				{
					behaviorByte = 1;
					frequencyByte = 2;
					b = 0;
				}
			}
			else
			{
				switch (CurrentPowerOption)
				{
				case PowerStateOptions.s3:
					behaviorByte = 7;
					frequencyByte = 8;
					b = 6;
					break;
				case PowerStateOptions.readyMode:
					behaviorByte = 13;
					frequencyByte = 14;
					b = 12;
					break;
				case PowerStateOptions.s5:
					behaviorByte = 19;
					frequencyByte = 20;
					b = 18;
					break;
				default:
					behaviorByte = 1;
					frequencyByte = 2;
					b = 0;
					break;
				}
			}
			if (behaviorByte.HasValue)
			{
				_selectedBehavior = controlItems.FirstOrDefault((ControlItem i) => i.Index == behaviorByte.Value);
			}
			if (_selectedBehavior == null)
			{
				b2 = WmiWrapper.GetControlItemValue(LedType, SelectedIndicator.IndicatorType, behaviorByte.Value, _ledService.Wmi).GetValueOrDefault();
				if (b2.HasValue)
				{
					SelectedBehavior = new ControlItem(behaviorByte.Value, b2.Value, supported: true);
				}
			}
			if (frequencyByte.HasValue)
			{
				_selectedFrequency = controlItems.FirstOrDefault((ControlItem i) => i.Index == frequencyByte.Value);
			}
			if (_selectedFrequency == null)
			{
				b2 = WmiWrapper.GetControlItemValue(LedType, SelectedIndicator.IndicatorType, frequencyByte.Value, _ledService.Wmi).GetValueOrDefault();
				if (b2.HasValue)
				{
					SelectedFrequency = new ControlItem(frequencyByte.Value, b2.Value, supported: true);
				}
			}
			if (b.HasValue)
			{
				WmiWrapper.SetControlItem(LedType, SelectedIndicator.IndicatorType, b.Value, 50, _ledService.Wmi, NucType);
			}
			break;
		case IndicatorType.Ethernet:
		case IndicatorType.PowerLimit:
			_selectedBehavior = controlItems.FirstOrDefault((ControlItem i) => i.Index == 0);
			if (_selectedBehavior == null)
			{
				b2 = WmiWrapper.GetControlItemValue(LedType, SelectedIndicator.IndicatorType, 0, _ledService.Wmi).GetValueOrDefault();
				if (b2.HasValue)
				{
					SelectedBehavior = new ControlItem(0, b2.Value, supported: true);
				}
			}
			break;
		case IndicatorType.HddActivity:
			_selectedBehavior = controlItems.FirstOrDefault((ControlItem i) => i.Index == 4);
			if (_selectedBehavior == null)
			{
				b2 = WmiWrapper.GetControlItemValue(LedType, SelectedIndicator.IndicatorType, 4, _ledService.Wmi).GetValueOrDefault();
				if (b2.HasValue)
				{
					SelectedBehavior = new ControlItem(4, b2.Value, supported: true);
				}
			}
			break;
		}
		RaisePropertyChanged("SelectedBehavior");
		RaisePropertyChanged("SelectedFrequency");
		if (_events != null)
		{
			_events.GetEvent<LedColorBehaviorChangeEvent>().Publish(this);
		}
	}

	public void RefreshUi()
	{
		RaisePropertyChanged("SelectedColor");
		RaisePropertyChanged("SelectedColors");
		RaisePropertyChanged("SelectedIndicator");
		RaisePropertyChanged("IndicatorName");
		RaisePropertyChanged("SelectedBehavior");
		RaisePropertyChanged("SelectedFrequency");
		RaisePropertyChanged("CurrentPowerOption");
		RaisePropertyChanged("SupportedIndicators");
		RaisePropertyChanged("SelectedLed");
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	private void OpenLink()
	{
		Process.Start("https://www.microsoft.com/en-us/p/alexa/9n12z3cctcnz?cid=intellightring");
	}

	public bool AlexaPcDownloadNeeded()
	{
		string[] directories = Directory.GetDirectories(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Packages"));
		List<string> list = new List<string>();
		string[] array = directories;
		foreach (string text in array)
		{
			if (text.Contains("57540AMZNMobileLLC.AmazonAlexa_"))
			{
				list.Add(text);
			}
		}
		return list.Count == 0;
	}
}
