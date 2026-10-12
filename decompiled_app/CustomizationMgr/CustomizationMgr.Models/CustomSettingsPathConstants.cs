using System;
using System.IO;

namespace CustomizationMgr.Models;

public static class CustomSettingsPathConstants
{
	private static readonly string PROGRAM_FILES_PATH = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);

	public static readonly string ROOT_PATH = Path.Combine(PROGRAM_FILES_PATH, "Intel Corporation\\Intel(R) NUC Software Studio");

	public static readonly string LANGUAGES_PATH = Path.Combine(ROOT_PATH, "Languages\\");

	public static readonly string ICONS_PATH = Path.Combine(ROOT_PATH, "Icons");

	public static readonly string APP_SETTINGS = Path.Combine(ROOT_PATH, "AppSettings.json");

	public static readonly string COLORS = Path.Combine(ROOT_PATH, "Colors.json");

	public static readonly string APP_TITLE_SETTINGS = Path.Combine(ROOT_PATH, "CustomAppTitle.json");
}
