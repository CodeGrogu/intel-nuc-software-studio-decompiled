using System;
using System.Collections.Generic;
using System.IO;
using Infrastructure.Models;
using IntelSdkClient;
using Microsoft.Win32;
using Prism.Logging;

namespace AlexaLightBarMgr;

public class AlexaServiceClient
{
	public const string ALEXA_REGISTRY_KEY = "SOFTWARE\\WOW6432Node\\Amazon\\Alexa";

	public const string MICROSOFT_LINK = "https://www.microsoft.com/en-us/p/alexa/9n12z3cctcnz?cid=intellightring";

	private const string ALEXA_FOLDER_NAME = "57540AMZNMobileLLC.AmazonAlexa_";

	private readonly ILoggerFacade _logger;

	private AlexaSettingsServiceProxy _proxy;

	public bool AlexaHostPresent { get; }

	public static bool A4pcDownloadNeeded()
	{
		string[] directories = Directory.GetDirectories(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Packages"));
		List<string> list = new List<string>();
		string[] array = directories;
		foreach (string text in array)
		{
			if (text.Contains("57540AMZNMobileLLC.AmazonAlexa_"))
			{
				list.Add(text);
			}
		}
		return list.Count == 0;
	}

	public AlexaServiceClient(ILoggerFacade logger)
	{
		_logger = logger;
		AlexaHostPresent = VerifyAlexaHostPresent();
	}

	~AlexaServiceClient()
	{
		_proxy.Close();
	}

	public bool EnableAlexa(bool Enable)
	{
		if (_proxy == null)
		{
			_proxy = new AlexaSettingsServiceProxy();
		}
		try
		{
			_proxy.SetLightBarState(Enable);
		}
		catch (Exception exception)
		{
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
			return false;
		}
		return true;
	}

	public bool GetLightbarSetting()
	{
		if (_proxy == null)
		{
			_proxy = new AlexaSettingsServiceProxy();
		}
		try
		{
			return _proxy.GetLightBarState();
		}
		catch (Exception exception)
		{
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
			return false;
		}
	}

	private bool VerifyAlexaHostPresent()
	{
		if (!VerifyRegistryKey())
		{
			return false;
		}
		if (ConnectToService())
		{
			return true;
		}
		return false;
	}

	private bool ConnectToService()
	{
		if (_proxy == null)
		{
			_proxy = new AlexaSettingsServiceProxy();
		}
		try
		{
			_proxy.GetLightBarState();
			return true;
		}
		catch (Exception exception)
		{
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
		}
		_proxy = null;
		return false;
	}

	private static bool VerifyRegistryKey()
	{
		return Registry.LocalMachine.OpenSubKey("SOFTWARE\\WOW6432Node\\Amazon\\Alexa") != null;
	}
}
