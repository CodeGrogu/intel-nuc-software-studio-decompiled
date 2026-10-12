using System.Resources;
using FxResources.System.Diagnostics.EventLog;

namespace System;

internal static class SR
{
	private static readonly bool s_usingResourceKeys = AppContext.TryGetSwitch("System.Resources.UseSystemResourceKeys", out var isEnabled) && isEnabled;

	private static ResourceManager s_resourceManager;

	internal static ResourceManager ResourceManager => s_resourceManager ?? (s_resourceManager = new ResourceManager(typeof(FxResources.System.Diagnostics.EventLog.SR)));

	internal static string BadLogName => GetResourceString("BadLogName");

	internal static string CannotDeleteEqualSource => GetResourceString("CannotDeleteEqualSource");

	internal static string CantMonitorEventLog => GetResourceString("CantMonitorEventLog");

	internal static string CantOpenLog => GetResourceString("CantOpenLog");

	internal static string CantOpenLogAccess => GetResourceString("CantOpenLogAccess");

	internal static string CantReadLogEntryAt => GetResourceString("CantReadLogEntryAt");

	internal static string CantRetrieveEntries => GetResourceString("CantRetrieveEntries");

	internal static string DuplicateLogName => GetResourceString("DuplicateLogName");

	internal static string EventID => GetResourceString("EventID");

	internal static string IndexOutOfBounds => GetResourceString("IndexOutOfBounds");

	internal static string InitTwice => GetResourceString("InitTwice");

	internal static string InvalidCustomerLogName => GetResourceString("InvalidCustomerLogName");

	internal static string InvalidParameter => GetResourceString("InvalidParameter");

	internal static string InvalidParameterFormat => GetResourceString("InvalidParameterFormat");

	internal static string LocalLogAlreadyExistsAsSource => GetResourceString("LocalLogAlreadyExistsAsSource");

	internal static string LocalRegKeyMissing => GetResourceString("LocalRegKeyMissing");

	internal static string LocalSourceAlreadyExists => GetResourceString("LocalSourceAlreadyExists");

	internal static string LocalSourceNotRegistered => GetResourceString("LocalSourceNotRegistered");

	internal static string LogDoesNotExists => GetResourceString("LogDoesNotExists");

	internal static string LogEntryTooLong => GetResourceString("LogEntryTooLong");

	internal static string LogSourceMismatch => GetResourceString("LogSourceMismatch");

	internal static string MaximumKilobytesOutOfRange => GetResourceString("MaximumKilobytesOutOfRange");

	internal static string MessageNotFormatted => GetResourceString("MessageNotFormatted");

	internal static string MissingLog => GetResourceString("MissingLog");

	internal static string MissingLogProperty => GetResourceString("MissingLogProperty");

	internal static string MissingParameter => GetResourceString("MissingParameter");

	internal static string NeedSourceToOpen => GetResourceString("NeedSourceToOpen");

	internal static string NeedSourceToWrite => GetResourceString("NeedSourceToWrite");

	internal static string NoCurrentEntry => GetResourceString("NoCurrentEntry");

	internal static string NoLogName => GetResourceString("NoLogName");

	internal static string ParameterTooLong => GetResourceString("ParameterTooLong");

	internal static string PlatformNotSupported_EventLog => GetResourceString("PlatformNotSupported_EventLog");

	internal static string RegKeyMissing => GetResourceString("RegKeyMissing");

	internal static string RegKeyMissingShort => GetResourceString("RegKeyMissingShort");

	internal static string RegKeyNoAccess => GetResourceString("RegKeyNoAccess");

	internal static string RentionDaysOutOfRange => GetResourceString("RentionDaysOutOfRange");

	internal static string SomeLogsInaccessible => GetResourceString("SomeLogsInaccessible");

	internal static string SomeLogsInaccessibleToCreate => GetResourceString("SomeLogsInaccessibleToCreate");

	internal static string SourceAlreadyExists => GetResourceString("SourceAlreadyExists");

	internal static string SourceNotRegistered => GetResourceString("SourceNotRegistered");

	internal static string TooManyReplacementStrings => GetResourceString("TooManyReplacementStrings");

	internal static string LogAlreadyExistsAsSource => GetResourceString("LogAlreadyExistsAsSource");

	internal static string NotSupported_IONonFileDevices => GetResourceString("NotSupported_IONonFileDevices");

	private static bool UsingResourceKeys()
	{
		return s_usingResourceKeys;
	}

	internal static string GetResourceString(string resourceKey, string defaultString = null)
	{
		if (UsingResourceKeys())
		{
			return defaultString ?? resourceKey;
		}
		string text = null;
		try
		{
			text = ResourceManager.GetString(resourceKey);
		}
		catch (MissingManifestResourceException)
		{
		}
		if (defaultString != null && resourceKey.Equals(text))
		{
			return defaultString;
		}
		return text;
	}

	internal static string Format(string resourceFormat, object p1)
	{
		if (UsingResourceKeys())
		{
			return string.Join(", ", resourceFormat, p1);
		}
		return string.Format(resourceFormat, p1);
	}

	internal static string Format(string resourceFormat, object p1, object p2)
	{
		if (UsingResourceKeys())
		{
			return string.Join(", ", resourceFormat, p1, p2);
		}
		return string.Format(resourceFormat, p1, p2);
	}

	internal static string Format(string resourceFormat, object p1, object p2, object p3)
	{
		if (UsingResourceKeys())
		{
			return string.Join(", ", resourceFormat, p1, p2, p3);
		}
		return string.Format(resourceFormat, p1, p2, p3);
	}

	internal static string Format(string resourceFormat, params object[] args)
	{
		if (args != null)
		{
			if (UsingResourceKeys())
			{
				return resourceFormat + ", " + string.Join(", ", args);
			}
			return string.Format(resourceFormat, args);
		}
		return resourceFormat;
	}

	internal static string Format(IFormatProvider provider, string resourceFormat, object p1)
	{
		if (UsingResourceKeys())
		{
			return string.Join(", ", resourceFormat, p1);
		}
		return string.Format(provider, resourceFormat, p1);
	}

	internal static string Format(IFormatProvider provider, string resourceFormat, object p1, object p2)
	{
		if (UsingResourceKeys())
		{
			return string.Join(", ", resourceFormat, p1, p2);
		}
		return string.Format(provider, resourceFormat, p1, p2);
	}

	internal static string Format(IFormatProvider provider, string resourceFormat, object p1, object p2, object p3)
	{
		if (UsingResourceKeys())
		{
			return string.Join(", ", resourceFormat, p1, p2, p3);
		}
		return string.Format(provider, resourceFormat, p1, p2, p3);
	}

	internal static string Format(IFormatProvider provider, string resourceFormat, params object[] args)
	{
		if (args != null)
		{
			if (UsingResourceKeys())
			{
				return resourceFormat + ", " + string.Join(", ", args);
			}
			return string.Format(provider, resourceFormat, args);
		}
		return resourceFormat;
	}
}
