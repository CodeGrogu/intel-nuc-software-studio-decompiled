using System.Resources;
using System.Runtime.CompilerServices;
using FxResources.System.Collections.Immutable;

namespace System;

internal static class SR
{
	private static ResourceManager s_resourceManager;

	internal static ResourceManager ResourceManager => s_resourceManager ?? (s_resourceManager = new ResourceManager(typeof(FxResources.System.Collections.Immutable.SR)));

	internal static string Arg_KeyNotFoundWithKey => GetResourceString("Arg_KeyNotFoundWithKey");

	internal static string ArrayInitializedStateNotEqual => GetResourceString("ArrayInitializedStateNotEqual");

	internal static string ArrayLengthsNotEqual => GetResourceString("ArrayLengthsNotEqual");

	internal static string CannotFindOldValue => GetResourceString("CannotFindOldValue");

	internal static string CapacityMustBeGreaterThanOrEqualToCount => GetResourceString("CapacityMustBeGreaterThanOrEqualToCount");

	internal static string CapacityMustEqualCountOnMove => GetResourceString("CapacityMustEqualCountOnMove");

	internal static string CollectionModifiedDuringEnumeration => GetResourceString("CollectionModifiedDuringEnumeration");

	internal static string DuplicateKey => GetResourceString("DuplicateKey");

	internal static string InvalidEmptyOperation => GetResourceString("InvalidEmptyOperation");

	internal static string InvalidOperationOnDefaultArray => GetResourceString("InvalidOperationOnDefaultArray");

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool UsingResourceKeys()
	{
		return false;
	}

	internal static string GetResourceString(string resourceKey, string? defaultString = null)
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

	internal static string Format(string resourceFormat, object? p1)
	{
		if (UsingResourceKeys())
		{
			return string.Join(", ", resourceFormat, p1);
		}
		return string.Format(resourceFormat, p1);
	}

	internal static string Format(string resourceFormat, object? p1, object? p2)
	{
		if (UsingResourceKeys())
		{
			return string.Join(", ", resourceFormat, p1, p2);
		}
		return string.Format(resourceFormat, p1, p2);
	}

	internal static string Format(string resourceFormat, object? p1, object? p2, object? p3)
	{
		if (UsingResourceKeys())
		{
			return string.Join(", ", resourceFormat, p1, p2, p3);
		}
		return string.Format(resourceFormat, p1, p2, p3);
	}

	internal static string Format(string resourceFormat, params object?[]? args)
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

	internal static string Format(IFormatProvider? provider, string resourceFormat, object? p1)
	{
		if (UsingResourceKeys())
		{
			return string.Join(", ", resourceFormat, p1);
		}
		return string.Format(provider, resourceFormat, p1);
	}

	internal static string Format(IFormatProvider? provider, string resourceFormat, object? p1, object? p2)
	{
		if (UsingResourceKeys())
		{
			return string.Join(", ", resourceFormat, p1, p2);
		}
		return string.Format(provider, resourceFormat, p1, p2);
	}

	internal static string Format(IFormatProvider? provider, string resourceFormat, object? p1, object? p2, object? p3)
	{
		if (UsingResourceKeys())
		{
			return string.Join(", ", resourceFormat, p1, p2, p3);
		}
		return string.Format(provider, resourceFormat, p1, p2, p3);
	}

	internal static string Format(IFormatProvider? provider, string resourceFormat, params object?[]? args)
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
