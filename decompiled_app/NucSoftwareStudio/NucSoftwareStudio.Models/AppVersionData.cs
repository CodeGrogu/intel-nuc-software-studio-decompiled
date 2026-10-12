using Windows.ApplicationModel;

namespace NucSoftwareStudio.Models;

public static class AppVersionData
{
	public const string VERSION_KEY = "Version";

	public const string APP_NAME_KEY = "AppNameString";

	public const string LICENSES_KEY = "Licenses";

	public static string GetVersionString()
	{
		try
		{
			PackageVersion version = Package.Current.Id.Version;
			return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
		}
		catch
		{
		}
		return "";
	}
}
