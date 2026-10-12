using System;
using System.Collections.Generic;
using System.IO;
using AlexaLightBarMgr.Interfaces;
using AlexaLightBarMgr.Storage.Data;
using Infrastructure.Models;
using Prism.Logging;

namespace AlexaLightBarMgr.Storage.Managers;

internal class AlexaSettingsFileManager : IAlexaSettingsFileManager
{
	private const string ALEXA_SETTINGS_DIRECTORY = "Alexa\\";

	private static readonly string ALEXA_SETTINGS_DIRECTORY_PATH = Path.Combine(ApplicationConstants.GetProfileFolderPath(), "Alexa\\");

	private const string ALEXA_SETTINGS_FILENAME = "Alexa.json";

	private static readonly string ALEXA_SETTINGS_FILE_PATH = Path.Combine(ALEXA_SETTINGS_DIRECTORY_PATH, "Alexa.json");

	private readonly ILoggerFacade _logger;

	private static readonly List<Type> ACCEPTED_JSON_TYPES = new List<Type> { typeof(AlexaSettingsData) };

	public AlexaSettingsFileManager(ILoggerFacade logger)
	{
		_logger = logger;
		CreateStorageDirectoryIfNeeded();
	}

	public AlexaSettingsData ReadSavedAlexaSettings()
	{
		AlexaSettingsData result = new AlexaSettingsData
		{
			LightbarEnabled = true
		};
		if (!CreateStorageDirectoryIfNeeded())
		{
			return result;
		}
		if (!File.Exists(ALEXA_SETTINGS_FILE_PATH))
		{
			return result;
		}
		try
		{
			return NssJsonSerializer.DeserializeWithBinding<AlexaSettingsData>(ALEXA_SETTINGS_FILE_PATH, ACCEPTED_JSON_TYPES);
		}
		catch (Exception exception)
		{
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
			return result;
		}
	}

	public void SaveAlexaSettings(AlexaSettingsData alexaSettingsData)
	{
		if (alexaSettingsData == null)
		{
			return;
		}
		CreateStorageDirectoryIfNeeded();
		if (File.Exists(ALEXA_SETTINGS_FILE_PATH))
		{
			File.Delete(ALEXA_SETTINGS_FILE_PATH);
		}
		try
		{
			NssJsonSerializer.SerializeWithBinding(alexaSettingsData, ALEXA_SETTINGS_FILE_PATH, ACCEPTED_JSON_TYPES);
		}
		catch (Exception exception)
		{
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
		}
	}

	private bool CreateStorageDirectoryIfNeeded()
	{
		if (!Directory.Exists(ALEXA_SETTINGS_DIRECTORY_PATH))
		{
			Directory.CreateDirectory(ALEXA_SETTINGS_DIRECTORY_PATH);
			return false;
		}
		return true;
	}
}
