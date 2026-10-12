using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Media;
using Infrastructure.Models;
using MobileKeyboardLedMgr.Constants;
using MobileKeyboardLedMgr.Interfaces;
using MobileKeyboardLedMgr.Models.Profiles;
using MobileKeyboardLedMgr.Storage.Data;
using Prism.Logging;
using Prism.Mvvm;

namespace MobileKeyboardLedMgr.Models;

public class KeyboardColorManager : BindableBase, IKeyboardColorManager
{
	private static readonly string COLORS_DIRECTORY = Path.Combine(ApplicationConstants.GetProfileFolderPath(), "SavedColors\\");

	private const string RECENT_FILENAME = "Recent.json";

	private readonly List<Type> ACCEPTED_JSON_TYPES = new List<Type> { typeof(RecentColorsData) };

	private const int NUM_COLOR = 7;

	private readonly ILoggerFacade _logger;

	private Color _currentColor;

	private Color _first;

	private Color _second;

	private Color _third;

	public Color CurrentColor
	{
		get
		{
			return _currentColor;
		}
		set
		{
			_currentColor = value;
			try
			{
				if (!RecentColors.Contains(_currentColor))
				{
					if (RecentColors.Count >= 7)
					{
						RecentColors.Remove(RecentColors.First());
					}
					RecentColors.Add(_currentColor);
				}
			}
			catch
			{
			}
		}
	}

	public Color First
	{
		get
		{
			return _first;
		}
		set
		{
			SetProperty(ref _first, value, "First");
		}
	}

	public Color Second
	{
		get
		{
			return _second;
		}
		set
		{
			SetProperty(ref _second, value, "Second");
		}
	}

	public Color Third
	{
		get
		{
			return _third;
		}
		set
		{
			SetProperty(ref _third, value, "Third");
		}
	}

	public ObservableCollection<Color> StandardColors { get; }

	public ObservableCollection<Color> RecentColors { get; set; }

	public KeyboardColorManager(ILoggerFacade logger, ISettingsFileManager settingsFileManager)
	{
		_logger = logger;
		if (settingsFileManager.CustomDefaultSettings?.DefaultStandardColors != null)
		{
			KeyboardLedCustomSettings customDefaultSettings = settingsFileManager.CustomDefaultSettings;
			if (customDefaultSettings != null && customDefaultSettings.DefaultStandardColors.Count > 0)
			{
				_currentColor = settingsFileManager.CustomDefaultSettings.DefaultStandardColors[0];
				StandardColors = new ObservableCollection<Color>(settingsFileManager.CustomDefaultSettings.DefaultStandardColors);
				goto IL_00a2;
			}
		}
		_currentColor = DefaultColors.SELECTED;
		StandardColors = new ObservableCollection<Color>(DefaultColors.COLORS);
		goto IL_00a2;
		IL_00a2:
		RecentColors = GetRecentColors();
	}

	public void SaveRecentColors(ObservableCollection<Color> recent)
	{
		try
		{
			RecentColors = recent;
			if (!Directory.Exists(COLORS_DIRECTORY))
			{
				Directory.CreateDirectory(COLORS_DIRECTORY);
			}
			if (File.Exists(Path.Combine(COLORS_DIRECTORY, "Recent.json")))
			{
				File.Delete(Path.Combine(COLORS_DIRECTORY, "Recent.json"));
			}
			NssJsonSerializer.SerializeWithBinding(new RecentColorsData(recent), Path.Combine(COLORS_DIRECTORY, "Recent.json"), ACCEPTED_JSON_TYPES);
		}
		catch (Exception exception)
		{
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
		}
	}

	private ObservableCollection<Color> GetRecentColors()
	{
		RecentColorsData recentColorsData = new RecentColorsData();
		if (!Directory.Exists(COLORS_DIRECTORY))
		{
			Directory.CreateDirectory(COLORS_DIRECTORY);
		}
		if (Directory.GetFiles(COLORS_DIRECTORY).ToList().Count > 0)
		{
			recentColorsData = NssJsonSerializer.DeserializeWithBinding<RecentColorsData>(Path.Combine(COLORS_DIRECTORY, "Recent.json"), ACCEPTED_JSON_TYPES);
		}
		else
		{
			recentColorsData.Initialize();
		}
		return recentColorsData.Recent;
	}
}
