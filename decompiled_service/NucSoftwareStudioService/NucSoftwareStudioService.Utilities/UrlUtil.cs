using System;

namespace NucSoftwareStudioService.Utilities;

public static class UrlUtil
{
	private static readonly string[] SCHEMES = new string[3] { "http", "net.pipe", "net.tcp" };

	public static string ConvertUri(string uriStr, ConvType convType)
	{
		return ConvertUri(uriStr, SCHEMES[(int)convType], convType == ConvType.N_PIPE);
	}

	public static string ConvertUri(string uriStr, string scheme, bool removePort = false)
	{
		Uri uri = new Uri(uriStr);
		if (removePort)
		{
			return uri.GetComponents(UriComponents.PathAndQuery | UriComponents.Scheme | UriComponents.UserInfo | UriComponents.Host | UriComponents.Fragment, UriFormat.UriEscaped).Replace(uri.Scheme, scheme);
		}
		return uriStr.Replace(uri.Scheme, scheme);
	}
}
