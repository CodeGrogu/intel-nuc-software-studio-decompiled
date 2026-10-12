using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CustomizationMgr.Enums;
using CustomizationMgr.Interfaces;
using Infrastructure.Events;
using Infrastructure.Models;
using Microsoft.Win32;
using Newtonsoft.Json.Serialization;
using Prism.Events;
using Prism.Logging;
using Svg;
using Windows.Storage;

namespace CustomizationMgr.Models;

public class CustomResourceManager : ICustomResourceManager
{
	private readonly ILoggerFacade _logger;

	private readonly IEventAggregator _events;

	private readonly Dictionary<string, ResourceDictionary> _customStrings = new Dictionary<string, ResourceDictionary>();

	private readonly Dictionary<string, ResourceDictionary> _defaultStrings = new Dictionary<string, ResourceDictionary>();

	private readonly List<object> _defaultSettings = new List<object>();

	private readonly List<Type> _customSettingsTypes = new List<Type>();

	private Dictionary<IconType, BitmapSource> _customIcons;

	private CustomAppTitleSettings _customAppTitleSettings;

	private CultureInfo _currentUiCulture;

	private CultureInfo _customDefaultCulture;

	private readonly CultureInfo _defaultCulture;

	public CustomAppSettings CustomAppSettings { get; private set; }

	public CustomResourceManager(ILoggerFacade logger, IEventAggregator events)
	{
		_logger = logger;
		_events = events;
		_defaultCulture = new CultureInfo("en-US");
		_currentUiCulture = CultureInfo.CurrentUICulture;
		InitializeResources();
	}

	public bool IsFirstLaunch()
	{
		object obj;
		try
		{
			if (ApplicationConstants.OptimizationsFileExists())
			{
				ApplicationData.Current.LocalSettings.Values["FirstLaunch"] = false;
				return false;
			}
			if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("FirstLaunch"))
			{
				ApplicationData.Current.LocalSettings.Values["FirstLaunch"] = true;
				return true;
			}
			obj = ApplicationData.Current.LocalSettings.Values["FirstLaunch"];
		}
		catch (Exception exception)
		{
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
			obj = false;
		}
		return (bool)obj;
	}

	public void LoadCustomResources()
	{
		_customAppTitleSettings = LoadCustomAppTitleSettings();
		CustomAppSettings = GetSavedSettings();
		if (CustomAppSettings != null)
		{
			try
			{
				_customDefaultCulture = new CultureInfo(CustomAppSettings.DefaultLanguage);
			}
			catch (Exception)
			{
			}
		}
		LoadCustomStyles();
		LoadCustomStrings();
		_events.GetEvent<LogoLoadedEvent>().Publish();
		if (_defaultStrings != null)
		{
			if (_defaultStrings.ContainsKey(GetDefaultCultureId()))
			{
				Application.Current.Resources.MergedDictionaries.Add(_defaultStrings[GetDefaultCultureId()]);
			}
			if (_defaultStrings.ContainsKey(GetCurrentUiCultureId()))
			{
				Application.Current.Resources.MergedDictionaries.Add(_defaultStrings[GetCurrentUiCultureId()]);
			}
		}
		if (_customStrings != null)
		{
			if (_customStrings.ContainsKey(GetCustomDefaultCultureId()))
			{
				Application.Current.Resources.MergedDictionaries.Add(_customStrings[GetCustomDefaultCultureId()]);
			}
			if (_customStrings.ContainsKey(GetCurrentUiCultureId()))
			{
				Application.Current.Resources.MergedDictionaries.Add(_customStrings[GetCurrentUiCultureId()]);
			}
		}
		if (_customAppTitleSettings != null)
		{
			Dictionary<string, string> customAppTitle = _customAppTitleSettings.CustomAppTitle;
			if (customAppTitle != null && customAppTitle.Count() > 0)
			{
				List<AppTitleLocation> customAppTitleLocation = _customAppTitleSettings.CustomAppTitleLocation;
				if (customAppTitleLocation != null && customAppTitleLocation.Count() > 0 && !string.IsNullOrEmpty(_customAppTitleSettings.DefaultTitleLanguage))
				{
					ApplyCustomAppTitleStrings(_customAppTitleSettings.CustomAppTitleLocation, _customAppTitleSettings.DefaultTitleLanguage, _customAppTitleSettings.CustomAppTitle);
					return;
				}
			}
		}
		_events.GetEvent<CustomAppTitleEvent>().Publish(payload: false);
	}

	private CustomAppTitleSettings LoadCustomAppTitleSettings()
	{
		List<Type> knownTypes = new List<Type>
		{
			typeof(CustomAppTitleSettings),
			typeof(Dictionary<string, string>),
			typeof(List<AppTitleLocation>)
		};
		return NssJsonSerializer.Deserialize<CustomAppTitleSettings>(CustomSettingsPathConstants.APP_TITLE_SETTINGS, knownTypes, JsonErrorHandler);
	}

	private void ApplyCustomAppTitleStrings(List<AppTitleLocation> CustomLocations, string DefaultTitleLanguage, Dictionary<string, string> CustomTitles)
	{
		if (!string.IsNullOrEmpty(DefaultTitleLanguage) && CustomTitles.ContainsKey(DefaultTitleLanguage))
		{
			ResourceDictionary resourceDictionary = new ResourceDictionary();
			AddTitleValue(resourceDictionary, CustomLocations, AppTitleLocation.MainTitleBar, "MainTitleBarAppName", CustomTitles, DefaultTitleLanguage);
			AddTitleValue(resourceDictionary, CustomLocations, AppTitleLocation.TrayAppMenu, "TrayAppMenuAppName", CustomTitles, DefaultTitleLanguage);
			AddTitleValue(resourceDictionary, CustomLocations, AppTitleLocation.TrayAppHover, "TrayAppHoverAppName", CustomTitles, DefaultTitleLanguage);
			Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
		}
		string currentUiCultureId = GetCurrentUiCultureId();
		if (CustomTitles.ContainsKey(currentUiCultureId))
		{
			ResourceDictionary resourceDictionary2 = new ResourceDictionary();
			AddTitleValue(resourceDictionary2, CustomLocations, AppTitleLocation.MainTitleBar, "MainTitleBarAppName", CustomTitles, currentUiCultureId);
			AddTitleValue(resourceDictionary2, CustomLocations, AppTitleLocation.TrayAppMenu, "TrayAppMenuAppName", CustomTitles, currentUiCultureId);
			AddTitleValue(resourceDictionary2, CustomLocations, AppTitleLocation.TrayAppHover, "TrayAppHoverAppName", CustomTitles, currentUiCultureId);
			Application.Current.Resources.MergedDictionaries.Add(resourceDictionary2);
		}
		if (CustomLocations.Contains(AppTitleLocation.MainTitleBar))
		{
			_events.GetEvent<CustomAppTitleEvent>().Publish(payload: true);
		}
		else
		{
			_events.GetEvent<CustomAppTitleEvent>().Publish(payload: false);
		}
	}

	private void AddTitleValue(ResourceDictionary resources, List<AppTitleLocation> customLocations, AppTitleLocation location, string titleKey, Dictionary<string, string> customTitles, string languageKey)
	{
		if (customLocations.Contains(location))
		{
			resources.Add(titleKey, customTitles[languageKey]);
		}
	}

	public void GenerateDefaultFile()
	{
		CustomAppSettings defaultSettings = CustomAppSettings.GetDefaultSettings(_defaultSettings);
		string directoryName = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
		string path = Path.Combine(directoryName, "AppSettings.json");
		NssJsonSerializer.SerializeWithBinding(defaultSettings, path, _customSettingsTypes);
		CustomAppTitleSettings defaultSettings2 = CustomAppTitleSettings.GetDefaultSettings();
		string path2 = Path.Combine(directoryName, "CustomAppTitle.json");
		NssJsonSerializer.Serialize(defaultSettings2, path2);
	}

	public void LoadComponentStyleDictionary(string SourceUriString)
	{
		ResourceDictionary item = new ResourceDictionary
		{
			Source = new Uri(SourceUriString)
		};
		Application.Current.Resources.MergedDictionaries.Add(item);
	}

	public void LoadComponentStrings(string AssemblyName)
	{
		LoadStringsFromAssembly(AssemblyName);
	}

	public void AddKnownCustomSettingType(Type type, object defaultSetting)
	{
		_customSettingsTypes.Add(type);
		_defaultSettings.Add(defaultSetting);
	}

	public void AddKnownCustomSettingTypes(List<Type> types)
	{
		foreach (Type type in types)
		{
			_customSettingsTypes.Add(type);
		}
	}

	private string GetDefaultCultureId()
	{
		if (_defaultCulture == null)
		{
			return "";
		}
		return _defaultCulture.IetfLanguageTag.ToLower();
	}

	public string GetCustomDefaultCultureId()
	{
		if (_customDefaultCulture == null)
		{
			return "";
		}
		return _customDefaultCulture.IetfLanguageTag.ToLower();
	}

	private string GetCurrentUiCultureId()
	{
		if (_currentUiCulture == null)
		{
			return "";
		}
		return _currentUiCulture.IetfLanguageTag.ToLower();
	}

	private void InitializeResources()
	{
		LoadDefaultStrings();
		LoadDefaultStyles();
	}

	private void JsonErrorHandler(object sender, Newtonsoft.Json.Serialization.ErrorEventArgs errorArgs)
	{
		_logger.Log("Error parsing custom data: " + errorArgs.ErrorContext.Error.Message, Category.Warn, Priority.High);
		errorArgs.ErrorContext.Handled = true;
	}

	private CustomAppSettings GetSavedSettings()
	{
		_customSettingsTypes.Add(typeof(CustomAppSettings));
		return CustomSettingsDeserializer.DeserializeCustomSettings(CustomSettingsPathConstants.APP_SETTINGS, _customSettingsTypes, JsonErrorHandler);
	}

	private void UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
	{
		if (e.Category == UserPreferenceCategory.Locale)
		{
			_currentUiCulture = CultureInfo.InstalledUICulture;
		}
	}

	private void LoadDefaultStrings()
	{
		LoadStringsFromAssembly("Infrastructure");
		if (_defaultStrings != null)
		{
			if (_defaultStrings.ContainsKey(GetDefaultCultureId()))
			{
				Application.Current.Resources.MergedDictionaries.Add(_defaultStrings[GetDefaultCultureId()]);
			}
			if (_defaultStrings.ContainsKey(GetCurrentUiCultureId()))
			{
				Application.Current.Resources.MergedDictionaries.Add(_defaultStrings[GetCurrentUiCultureId()]);
			}
		}
	}

	private void LoadStringsFromAssembly(string AssemblyName)
	{
		Assembly assembly = Assembly.Load(AssemblyName);
		using ResourceReader resourceReader = new ResourceReader(assembly.GetManifestResourceStream(assembly.GetName().Name + ".g.resources"));
		foreach (DictionaryEntry item2 in resourceReader)
		{
			string[] array = item2.Key.ToString().Split(StyleLogoConstants.FILE_DELIMITERS);
			if (array[0].Equals("i18N", StringComparison.OrdinalIgnoreCase) && new FileInfo(item2.Key.ToString()).Extension.Equals(".baml"))
			{
				string key = array[array.Length - 2].ToLower();
				string text = "pack://application:,,,/" + AssemblyName + ";component/";
				ResourceDictionary item = new ResourceDictionary
				{
					Source = new Uri(text + item2.Key.ToString().Replace(".baml", ".xaml"))
				};
				if (!_defaultStrings.ContainsKey(key))
				{
					_defaultStrings.Add(key, new ResourceDictionary());
				}
				_defaultStrings[key].MergedDictionaries.Add(item);
			}
		}
	}

	private void LoadCustomStrings()
	{
		if (!Directory.Exists(CustomSettingsPathConstants.LANGUAGES_PATH))
		{
			return;
		}
		string[] files = Directory.GetFiles(CustomSettingsPathConstants.LANGUAGES_PATH);
		foreach (string text in files)
		{
			string key = ReadLanguageTagFromFileName(text);
			List<Type> knownTypes = new List<Type> { typeof(Dictionary<string, string>) };
			Dictionary<string, string> dictionary = NssJsonSerializer.Deserialize<Dictionary<string, string>>(text, knownTypes, JsonErrorHandler);
			ResourceDictionary resourceDictionary = new ResourceDictionary();
			foreach (KeyValuePair<string, string> item in dictionary)
			{
				resourceDictionary.Add(item.Key, item.Value);
			}
			if (!_customStrings.ContainsKey(key))
			{
				_customStrings.Add(key, new ResourceDictionary());
			}
			_customStrings[key].MergedDictionaries.Add(resourceDictionary);
		}
	}

	private string ReadLanguageTagFromFileName(string FileName)
	{
		return Path.GetFileNameWithoutExtension(FileName).Split(StyleLogoConstants.PERIOD_FILE_DELIMITER).Last()
			.ToLower();
	}

	private void LoadCustomStyles()
	{
		if (Directory.Exists(CustomSettingsPathConstants.ROOT_PATH))
		{
			LoadCustomColors();
			LoadCustomIcons();
			LoadStyleDictionaries("Brushes");
			LoadStyleDictionaries("General");
		}
	}

	private void LoadDefaultStyles()
	{
		LoadStyleDictionaries("Colors");
		LoadStyleDictionaries("Brushes");
		LoadStyleDictionaries("Icons");
		LoadStyleDictionaries("General");
		foreach (string rESOURCE_DICTIONARY_NAME in StyleLogoConstants.RESOURCE_DICTIONARY_NAMES)
		{
			LoadStyleDictionaries(rESOURCE_DICTIONARY_NAME);
		}
	}

	private void LoadStyleDictionaries(string Name)
	{
		ResourceDictionary item = new ResourceDictionary
		{
			Source = new Uri(StyleLogoConstants.STYLE_RESOURCE_PATH + Name + ".xaml")
		};
		Application.Current.Resources.MergedDictionaries.Add(item);
	}

	private void LoadCustomColors()
	{
		Dictionary<string, System.Windows.Media.Color> dictionary = ReadColors();
		if (dictionary == null)
		{
			return;
		}
		foreach (KeyValuePair<string, System.Windows.Media.Color> item in dictionary)
		{
			if (Application.Current.Resources.Contains(item.Key))
			{
				Application.Current.Resources[item.Key] = item.Value;
			}
			else
			{
				Application.Current.Resources.Add(item.Key, item.Value);
			}
		}
	}

	public Dictionary<string, System.Windows.Media.Color> ReadColors()
	{
		List<Type> knownTypes = new List<Type> { typeof(Dictionary<string, System.Windows.Media.Color>) };
		return NssJsonSerializer.Deserialize<Dictionary<string, System.Windows.Media.Color>>(CustomSettingsPathConstants.COLORS, knownTypes, JsonErrorHandler);
	}

	public KeyValuePair<AppLogoType, BitmapSource> AppLogo()
	{
		BitmapSource value = null;
		if (CustomAppSettings == null)
		{
			return new KeyValuePair<AppLogoType, BitmapSource>(AppLogoType.Default, value);
		}
		AppLogoType logoType = CustomAppSettings.LogoType;
		if ((uint)logoType > 1u && logoType == AppLogoType.Custom)
		{
			value = LoadCustomLogo();
		}
		return new KeyValuePair<AppLogoType, BitmapSource>(CustomAppSettings.LogoType, value);
	}

	public BitmapSource GetCustomIcon(IconType type)
	{
		if (_customIcons != null && _customIcons.ContainsKey(type))
		{
			return _customIcons[type];
		}
		return null;
	}

	private void LoadCustomIcons()
	{
		_customIcons = new Dictionary<IconType, BitmapSource>();
		IEnumerable<IconType> source = Enum.GetValues(typeof(IconType)).Cast<IconType>();
		if (!Directory.Exists(CustomSettingsPathConstants.ICONS_PATH))
		{
			return;
		}
		string[] files = Directory.GetFiles(CustomSettingsPathConstants.ICONS_PATH);
		foreach (string path in files)
		{
			string typeName = Path.GetFileNameWithoutExtension(path);
			IconType iconType = source.FirstOrDefault((IconType x) => x.ToString().Equals(typeName));
			if (iconType != IconType.Error)
			{
				BitmapSource bitmapSource = LoadBitmap(path, 60);
				if (bitmapSource != null)
				{
					_customIcons.Add(iconType, bitmapSource);
				}
			}
		}
	}

	private BitmapSource LoadCustomLogo()
	{
		if (!Directory.Exists(CustomSettingsPathConstants.ROOT_PATH))
		{
			return null;
		}
		string[] files = Directory.GetFiles(CustomSettingsPathConstants.ROOT_PATH);
		foreach (string path in files)
		{
			if (Path.GetFileNameWithoutExtension(path).Equals("AppLogo", StringComparison.OrdinalIgnoreCase))
			{
				return LoadBitmap(path, 36);
			}
		}
		return null;
	}

	private BitmapSource LoadBitmap(string path, int? height = null)
	{
		Bitmap bitmap = null;
		if (Path.GetExtension(path).Equals(".svg", StringComparison.OrdinalIgnoreCase))
		{
			SvgDocument svgDocument = SvgDocument.Open(path);
			bitmap = ((!height.HasValue) ? svgDocument.Draw() : svgDocument.Draw(0, height.Value));
		}
		else
		{
			try
			{
				bitmap = new Bitmap(path);
			}
			catch (Exception exception)
			{
				_logger.Log("Invalid logo format.", Category.Debug, Priority.High);
				_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
			}
		}
		if (bitmap == null)
		{
			return null;
		}
		return CreateBitmapSource(bitmap);
	}

	private BitmapSource CreateBitmapSource(Bitmap bitmap)
	{
		IntPtr hbitmap = bitmap.GetHbitmap();
		BitmapSource bitmapSource = null;
		try
		{
			return Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
		}
		finally
		{
			DeleteObject(hbitmap);
		}
	}

	[DllImport("gdi32.dll")]
	private static extern bool DeleteObject(IntPtr hObject);
}
