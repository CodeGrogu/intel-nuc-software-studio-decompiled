using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using CustomizationMgr.Interfaces;
using Infrastructure.Models;
using MobileKeyboardLedMgr.Constants;
using MobileKeyboardLedMgr.Events;
using MobileKeyboardLedMgr.Interfaces;
using MobileKeyboardLedMgr.Models.KingCounty;
using MobileKeyboardLedMgr.Models.Profiles;
using Prism.Events;
using Prism.Logging;
using Prism.Mvvm;
using UniwillSdkClient.Interfaces;
using UniwillSdkClient.UniwillService;

namespace MobileKeyboardLedMgr.Models;

public class KeyboardPatternManager : BindableBase, IKeyboardPatternManager
{
	private readonly IEventAggregator _events;

	private readonly UniwillServiceClient _client;

	private readonly IKeyboardColorManager _colorMgr;

	private readonly ILoggerFacade _logger;

	private bool _errorMessage = true;

	private KeyboardPattern _selectedPattern;

	private PresetPatternsKC _presetPatterns;

	private int _speed;

	private int _acBrightness;

	private int _dcBrightness;

	private bool _on = true;

	private bool _projectAuroraEnabled;

	private bool _projectAuroraInstalled;

	public RGBKeyboardCapabilities KeyboardType { get; private set; }

	public bool ErrorMessage
	{
		get
		{
			return _errorMessage;
		}
		set
		{
			SetProperty(ref _errorMessage, value, "ErrorMessage");
		}
	}

	public KeyboardUiData KeyboardUiData { get; set; }

	public List<KeyboardPattern> DefaultPatterns { get; set; }

	public KeyboardPattern SelectedPattern
	{
		get
		{
			return _selectedPattern;
		}
		set
		{
			_selectedPattern = value;
			if (_selectedPattern.SelectedEffect == RGBKeyboardEffect.UserMode)
			{
				KeyboardUiData.Editable = true;
				if (_selectedPattern.SelectedColors.First().SelectedColor == DefaultColors.OFF_COLOR_UI)
				{
					KeyboardUiData.SelectedColor = DefaultColors.SELECTED;
					_selectedPattern.SelectedColors.First().SelectedColor = DefaultColors.SELECTED;
				}
				else
				{
					KeyboardUiData.SelectedColor = _selectedPattern.SelectedColors.First().SelectedColor;
				}
			}
			else
			{
				KeyboardUiData.Editable = false;
			}
		}
	}

	public PresetPatternsKC PresetPatterns
	{
		get
		{
			return _presetPatterns;
		}
		set
		{
			SetProperty(ref _presetPatterns, value, "PresetPatterns");
		}
	}

	public int Speed
	{
		get
		{
			return _speed;
		}
		set
		{
			SetProperty(ref _speed, value, "Speed");
		}
	}

	public int AcBrightness
	{
		get
		{
			return _acBrightness;
		}
		set
		{
			SetProperty(ref _acBrightness, value, "AcBrightness");
		}
	}

	public int DcBrightness
	{
		get
		{
			return _dcBrightness;
		}
		set
		{
			SetProperty(ref _dcBrightness, value, "DcBrightness");
		}
	}

	public bool On
	{
		get
		{
			return _on;
		}
		set
		{
			_on = value;
			EnableLeds(value);
		}
	}

	public bool ProjectAuroraEnabled
	{
		get
		{
			return _projectAuroraEnabled;
		}
		set
		{
			_projectAuroraEnabled = value;
			_client.EnableProjectAurora(value);
		}
	}

	public bool ProjectAuroraInstalled => _projectAuroraInstalled;

	public KeyboardPatternManager(ILoggerFacade logger, IEventAggregator events, IUniwillClient uniwillClient, IKeyboardColorManager colorMgr, ICustomResourceManager resourceManager, ISettingsFileManager settingsFileManager)
	{
		KeyboardPatternManager keyboardPatternManager = this;
		ErrorMessage = false;
		_colorMgr = colorMgr;
		_logger = logger;
		_client = uniwillClient.Client;
		_events = events;
		_events.GetEvent<CustomKeyColorChangeEvent>().Subscribe(KeyboardColorSelectionChanged);
		_events.GetEvent<MultipleCustomKeyColorsChangedEvent>().Subscribe(MultipleKeyboardColorSelectionsChanged);
		_events.GetEvent<UpdateColorFromColorPickerEvent>().Subscribe(ColorSelectionChanged);
		KeyboardType = uniwillClient.GetKeyboardType();
		KeyboardUiData = new KeyboardUiData(_events, KeyboardType, editable: false, colorMgr.CurrentColor, isForKeyGroup: false, null);
		PresetPatterns = new PresetPatternsKC(_colorMgr, _events, settingsFileManager, KeyboardType);
		DefaultPatterns = PresetPatterns.Patterns;
		Task.Run(() =>
		{
			keyboardPatternManager.ProjectAuroraInitialization(resourceManager, settingsFileManager);
		}).ConfigureAwait(continueOnCapturedContext: false);
	}

	private void ProjectAuroraInitialization(ICustomResourceManager resourceManager, ISettingsFileManager settingsFileManager)
	{
		try
		{
			_projectAuroraInstalled = _client.GetProjectAuroraInstalled();
			if (!_projectAuroraInstalled)
			{
				if (_client.GetProjectAuroraState())
				{
					_client.EnableProjectAurora(Enable: false);
				}
				_projectAuroraEnabled = false;
			}
			else
			{
				_projectAuroraEnabled = _client.GetProjectAuroraState();
				if (!_projectAuroraEnabled && resourceManager.IsFirstLaunch() && settingsFileManager?.CustomDefaultSettings != null)
				{
					_projectAuroraEnabled = settingsFileManager.CustomDefaultSettings.ProjectAuroraEnabled;
					if (_projectAuroraEnabled)
					{
						_client.EnableProjectAurora(_projectAuroraEnabled);
					}
				}
			}
			if (_projectAuroraEnabled)
			{
				_on = false;
				ObservableCollection<KeyColorData> keys = MonocolorPattern.Generate(KeyboardType, DefaultColors.OFF_COLOR_UI, enabled: false);
				KeyboardUiData.ApplyPattern(keys);
				KeyboardUiData.DisableAll();
			}
			else
			{
				On = true;
			}
		}
		catch (Exception)
		{
			ErrorMessage = true;
		}
		_events.GetEvent<ProjectAuroraEvent>().Publish();
	}

	public void UpdateIdleTimer(int tickValue, bool isAc)
	{
		int seconds = KeyboardIdleTimer.ConvertIdleTimerToSeconds(tickValue);
		try
		{
			_client.SetIdleTimerValue(seconds, isAc);
		}
		catch (Exception exception)
		{
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
		}
	}

	private void KeyboardColorSelectionChanged(KeyColorData key)
	{
		KeyColorData keyColorData = SelectedPattern.Keys.FirstOrDefault((KeyColorData k) => k.Index == key.Index);
		if (keyColorData != null)
		{
			keyColorData.SelectedColor = key.SelectedColor;
		}
		else
		{
			SelectedPattern.Keys.Add(key);
		}
		ApplySelectedPattern();
		_events.GetEvent<EnableSaveEvent>().Publish();
	}

	private void MultipleKeyboardColorSelectionsChanged(List<KeyColorData> keys)
	{
		foreach (KeyColorData key in keys)
		{
			KeyColorData keyColorData = SelectedPattern.Keys.FirstOrDefault((KeyColorData k) => k.Index == key.Index);
			if (keyColorData != null)
			{
				keyColorData.SelectedColor = key.SelectedColor;
			}
			else
			{
				SelectedPattern.Keys.Add(key);
			}
		}
		ApplySelectedPattern();
		_events.GetEvent<EnableSaveEvent>().Publish();
	}

	private void ColorSelectionChanged(ColorPickerColors color)
	{
		KeyValuePair<int, Color>? keyValuePair = SelectedPattern.AvailableColors.First((KeyValuePair<int, Color> c) => c.Key == color.Index);
		if (SelectedPattern.SelectedEffect == RGBKeyboardEffect.UserMode)
		{
			KeyboardUiData.SelectedColor = color.SelectedColor;
		}
		if (keyValuePair.HasValue)
		{
			SelectedPattern.UpdateColorList(keyValuePair.Value, color.SelectedColor);
		}
		else
		{
			SelectedPattern.UpdateColorList(new KeyValuePair<int, Color>(color.Index, color.SelectedColor), color.SelectedColor);
		}
		ApplySelectedPattern();
		_colorMgr.SaveRecentColors(SelectedPattern.RecentColors);
		_events.GetEvent<EnableSaveEvent>().Publish();
		_events.GetEvent<UpdateSelectedColorsEvent>().Publish();
		RaisePropertyChanged("SelectedColors");
		RaisePropertyChanged("KeyboardUiData");
	}

	public void ApplyPattern(string Name)
	{
		KeyboardPattern keyboardPattern = DefaultPatterns.First((KeyboardPattern p) => p.Name == Name);
		if (keyboardPattern != null)
		{
			SelectedPattern = keyboardPattern;
		}
	}

	public void ApplySelectedPattern()
	{
		if (!_on)
		{
			return;
		}
		KeyboardUiData.ApplyPattern(SelectedPattern.Keys);
		try
		{
			_client.ApplyKeyboardLedProfileAsync(SelectedPattern.ToUniwillPatternData());
		}
		catch (Exception exception)
		{
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
		}
	}

	public void ApplyBrightness(bool isAcBrightness)
	{
		try
		{
			if (isAcBrightness)
			{
				_client.SetKeyboardBrightnessAsync(SelectedPattern.ACBrightness, isAcBrightness);
			}
			else
			{
				_client.SetKeyboardBrightnessAsync(SelectedPattern.DCBrightness, isAcBrightness);
			}
		}
		catch (Exception exception)
		{
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
		}
	}

	public void EnableLeds(bool Enable)
	{
		try
		{
			_client.EnableKeyboardLeds(Enable);
		}
		catch (Exception exception)
		{
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
		}
		if (Enable)
		{
			if (SelectedPattern != null)
			{
				ApplySelectedPattern();
			}
		}
		else
		{
			ObservableCollection<KeyColorData> keys = MonocolorPattern.Generate(KeyboardType, Colors.DimGray, enabled: true);
			KeyboardUiData.ApplyPattern(keys);
			KeyboardUiData.DisableAll();
			_events.GetEvent<KeyboardStatusUpdateEvent>().Publish();
		}
	}
}
