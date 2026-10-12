using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using Infrastructure.Logging;

namespace NucSoftwareStudioService.WMI;

internal static class WmiWrapperOld
{
	private const string WMI_SCOPE = "root\\WMI";

	private const string WMI_PATH = "CISD_WMI";

	private const string WMI_METHOD_COMMAND_NAME = "Cmd";

	private const string WMI_RESULT_DATA_KEY = "Data";

	private const string WMI_RESULT_BYTES_KEY = "Bytes";

	public const string ALEXA_LED_CONTROL_METHOD = "AlexaLedControl";

	public const string ALEXA_LED_CONTROL_METHOD_LEGACY = "PlatformConfigurationAccessV2";

	public const byte ALEXA_LED_CONTROL_FUNCTION = 1;

	public const string PLATFORM_CAPABILITY_REPORT_NAME = "PlatformCapabilityReport";

	public const int PLATFORM_CAPABILITY_RESULTS_LENGTH = 51;

	public const int LIGHTBAR_INDEX = 16;

	public const int WMI_INDEX = 0;

	public const int USB_INDEX = 3;

	public const int ALEXA_INDEX = 6;

	public const byte FEATURE_CAPABILITY_QUERY = 1;

	public const byte NO_ERROR = 0;

	public static byte[] WmiCall(string CommandName, byte[] Parameters, NssServiceLogger logger = null)
	{
		ManagementClass managementClass = null;
		try
		{
			ConnectionOptions options = new ConnectionOptions();
			ManagementScope scope = new ManagementScope("root\\WMI", options);
			ManagementPath path = new ManagementPath("CISD_WMI");
			managementClass = new ManagementClass(scope, path, null);
			ManagementBaseObject inParameters = managementClass.Methods[CommandName].InParameters;
			if (inParameters.Properties["Cmd"].Type.Equals(CimType.UInt32))
			{
				inParameters["Cmd"] = BitConverter.ToUInt32(Parameters, 0);
			}
			else if (inParameters.Properties["Cmd"].Type.Equals(CimType.UInt64))
			{
				inParameters["Cmd"] = BitConverter.ToUInt64(Parameters, 0);
			}
			using IEnumerator<ManagementObject> enumerator = managementClass.GetInstances().OfType<ManagementObject>().GetEnumerator();
			if (enumerator.MoveNext())
			{
				return ParseResults(enumerator.Current.InvokeMethod(CommandName, inParameters, null));
			}
		}
		catch (Exception ex)
		{
			logger?.Log(ex.Message, LogEventId.Exception, EventLogEntryType.Error);
		}
		finally
		{
			managementClass.Dispose();
		}
		return new byte[0];
	}

	public static byte[] ParseResults(ManagementBaseObject results)
	{
		foreach (PropertyData property in results.Properties)
		{
			if (!property.Name.Equals("Data"))
			{
				continue;
			}
			foreach (PropertyData property2 in ((ManagementBaseObject)property.Value).Properties)
			{
				if (property2.Name.Equals("Bytes"))
				{
					if (property2.Value.GetType() == typeof(byte[]))
					{
						return (byte[])property2.Value;
					}
					return new byte[0];
				}
			}
			return new byte[0];
		}
		return new byte[0];
	}

	public static bool DoesWmiMethodExist(string CommandName)
	{
		ManagementClass managementClass = null;
		try
		{
			ConnectionOptions options = new ConnectionOptions();
			ManagementScope scope = new ManagementScope("root\\WMI", options);
			ManagementPath path = new ManagementPath("CISD_WMI");
			managementClass = new ManagementClass(scope, path, null);
			_ = managementClass.Methods[CommandName].InParameters;
			return true;
		}
		catch (Exception)
		{
			return false;
		}
		finally
		{
			managementClass.Dispose();
		}
	}
}
