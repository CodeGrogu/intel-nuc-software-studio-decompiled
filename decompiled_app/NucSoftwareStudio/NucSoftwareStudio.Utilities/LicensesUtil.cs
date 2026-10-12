using System;
using System.Collections.Generic;
using System.IO;
using Infrastructure.Models;
using NucSoftwareStudio.Properties;
using Windows.Storage;

namespace NucSoftwareStudio.Utilities;

public static class LicensesUtil
{
	private static readonly string LICENSES_DIR_NAME = "Licenses";

	private static readonly string LOCAL_DIR_NAME = "Local";

	private static readonly string LICENSES_DIRECTORY = Path.Combine(ApplicationConstants.GetProfileFolderPath(), LICENSES_DIR_NAME);

	private static readonly string LICENSES_UWP_SUB_PATH = Path.Combine(LOCAL_DIR_NAME, "NucSoftwareStudio", LICENSES_DIR_NAME);

	private const string LIC_TXT_EXT = "-License.txt";

	private const string COMMON_SERVICE_LOCATOR_FILENAME = "CommonServiceLocator-License.txt";

	private const string FIZZLER_FILENAME = "Fizzler-License.txt";

	private const string LOG4NET_FILENAME = "log4net-License.txt";

	private const string MICROSOFT_TOOLKIT_UWP_NOTIFICATION_FILENAME = "Microsoft.Toolkit.Uwp.Notifications-License.txt";

	private const string NEWTONSON_JSON_FILENAME = "Newtonsoft.Json-License.txt";

	private const string PRISM_CORE_FILENAME = "Prism.Core-License.txt";

	private const string PRISM_UNITY_FILENAME = "Prism.Unity-License.txt";

	private const string PRISM_WPF_FILENAME = "Prism.Wpf-License.txt";

	private const string SVG_FILENAME = "Svg-License.txt";

	private const string TASK_SCHEDULER_FILENAME = "TaskScheduler-License.txt";

	private const string UNITY_ABSTRACTIONS_FILENAME = "Unity.Abstractions-License.txt";

	private const string UNITY_CONTAINER_FILENAME = "Unity.Container-License.txt";

	public static void StoreLicenses()
	{
		List<(string, string)> list = new List<(string, string)>
		{
			("CommonServiceLocator-License.txt", Resources.CommonServiceLocator_License),
			("Fizzler-License.txt", Resources.Fizzler_License),
			("log4net-License.txt", Resources.log4net_License),
			("Microsoft.Toolkit.Uwp.Notifications-License.txt", Resources.Microsoft_Toolkit_Uwp_Notifications_License),
			("Newtonsoft.Json-License.txt", Resources.Newtonsoft_Json_License),
			("Prism.Core-License.txt", Resources.Prism_Core_License),
			("Prism.Unity-License.txt", Resources.Prism_Unity_License),
			("Prism.Wpf-License.txt", Resources.Prism_Wpf_License),
			("Svg-License.txt", Resources.Svg_License),
			("TaskScheduler-License.txt", Resources.TaskScheduler_License),
			("Unity.Abstractions-License.txt", Resources.Unity_Abstractions_License),
			("Unity.Container-License.txt", Resources.Unity_Container_License)
		};
		CreateLicensesDirectory();
		foreach (var item3 in list)
		{
			string item = item3.Item1;
			string item2 = item3.Item2;
			string path = Path.Combine(LICENSES_DIRECTORY, item);
			if (!File.Exists(path))
			{
				File.WriteAllText(path, item2);
			}
		}
		list.Clear();
	}

	public static string GetLicensesDirectoryRealPath()
	{
		try
		{
			string text = Path.Combine(ApplicationData.Current.LocalCacheFolder.Path, LICENSES_UWP_SUB_PATH);
			if (!string.IsNullOrEmpty(text) && Directory.Exists(text))
			{
				return text;
			}
			return LICENSES_DIRECTORY;
		}
		catch (Exception)
		{
			return LICENSES_DIRECTORY;
		}
	}

	private static void CreateLicensesDirectory()
	{
		if (!Directory.Exists(LICENSES_DIRECTORY))
		{
			Directory.CreateDirectory(LICENSES_DIRECTORY);
		}
	}
}
