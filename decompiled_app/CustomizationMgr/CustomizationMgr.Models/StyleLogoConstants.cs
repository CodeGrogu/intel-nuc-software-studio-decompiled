using System.Collections.Generic;
using Infrastructure.Models;

namespace CustomizationMgr.Models;

public static class StyleLogoConstants
{
	public static readonly string STYLE_RESOURCE_PATH = AssemblyNames.ApplicationPackFromAssemblyName("Infrastructure") + "Styles/";

	public const string APP_LOGO_FILENAME = "AppLogo";

	public const string SVG_EXTENSION = ".svg";

	public const string CUSTOM_ICON_DIRECTORY = "Icons";

	public const int CUSTOM_SVG_ICON_HEIGHT = 60;

	public const int CUSTOM_SVG_LOGO_HEIGHT = 36;

	public const string CUSTOM_COLORS_FILENAME = "Colors.json";

	public const string COLORS_RESOURCE_DICTIONARY_NAME = "Colors";

	public const string BRUSHES_RESOURCE_DICTIONARY_NAME = "Brushes";

	public const string ICONS_RESOURCE_DICTIONARY_NAME = "Icons";

	public const string GENERAL_RESOURCE_DICTIONARY_NAME = "General";

	public static readonly List<string> RESOURCE_DICTIONARY_NAMES = new List<string>
	{
		"ToggleButtonStyles", "ScrollbarStyles", "ComboBoxStyles", "TextStyles", "TextBoxStyles", "ButtonStyles", "CardGridStyles", "CheckBoxStyles", "RadioButtonStyles", "SliderStyles",
		"TabControlStyles", "OsdIcons"
	};

	public const string JSON_FILE_EXTENSION = ".json";

	public const string XAML_FILE_EXTENSION = ".xaml";

	public const string COMPILED_XAML_FILE_EXTENSION = ".baml";

	public const string MANIFEST_RESOURCE_FILE_EXTENSION = ".g.resources";

	public static readonly char[] FILE_DELIMITERS = new char[2] { '.', '/' };

	public static readonly char[] PERIOD_FILE_DELIMITER = new char[1] { '.' };
}
