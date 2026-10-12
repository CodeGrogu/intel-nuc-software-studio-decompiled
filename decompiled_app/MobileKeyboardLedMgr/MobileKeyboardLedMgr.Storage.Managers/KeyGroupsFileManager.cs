using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using Infrastructure.Models;
using MobileKeyboardLedMgr.Interfaces;
using MobileKeyboardLedMgr.Models;
using MobileKeyboardLedMgr.Storage.Data;
using Prism.Events;
using Prism.Logging;
using UniwillSdkClient.UniwillService;

namespace MobileKeyboardLedMgr.Storage.Managers;

internal class KeyGroupsFileManager : IKeyGroupsFileManager
{
	private const string KEYGROUPS_DIRECTORY = "KeyGroups\\";

	private static readonly string KEYGROUPS_DIRECTORY_PATH = Path.Combine(ApplicationConstants.GetProfileFolderPath(), "KeyGroups\\");

	private const string KEYGROUPS_FILENAME = "KeyGroups.json";

	private static readonly string KEYGROUPS_FILE_PATH = Path.Combine(KEYGROUPS_DIRECTORY_PATH, "KeyGroups.json");

	private readonly IEventAggregator _events;

	private readonly ILoggerFacade _logger;

	private RGBKeyboardCapabilities _keyboardType;

	private static readonly List<Type> ACCEPTED_JSON_TYPES = new List<Type> { typeof(KeyGroupStorageData) };

	public KeyGroupsFileManager(IEventAggregator events, ILoggerFacade logger, IKeyboardPatternManager patternManager)
	{
		_events = events;
		_logger = logger;
		_keyboardType = patternManager.KeyboardType;
		CreateStorageDirectoryIfNeeded();
	}

	public List<KeyGroup> ReadSavedKeyGroups(Color selectedColor)
	{
		List<KeyGroup> list = new List<KeyGroup>();
		if (!CreateStorageDirectoryIfNeeded())
		{
			return list;
		}
		if (!File.Exists(KEYGROUPS_FILE_PATH))
		{
			return list;
		}
		try
		{
			foreach (KeyGroupStorageData item in NssJsonSerializer.DeserializeWithBinding<List<KeyGroupStorageData>>(KEYGROUPS_FILE_PATH, ACCEPTED_JSON_TYPES))
			{
				list.Add(new KeyGroup(_events, _keyboardType, item.Name, selectedColor, new HashSet<int>(item.SelectedKeyIds)));
			}
			return list;
		}
		catch (Exception exception)
		{
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
			return list;
		}
	}

	public void SaveKeyGroups(List<KeyGroup> keyGroups)
	{
		if (keyGroups == null || keyGroups.Count == 0)
		{
			return;
		}
		List<KeyGroupStorageData> list = new List<KeyGroupStorageData>();
		foreach (KeyGroup keyGroup in keyGroups)
		{
			KeyGroupStorageData item = new KeyGroupStorageData
			{
				Name = keyGroup.Name,
				SelectedKeyIds = new List<int>(keyGroup.SelectedKeyIds)
			};
			list.Add(item);
		}
		CreateStorageDirectoryIfNeeded();
		if (File.Exists(KEYGROUPS_FILE_PATH))
		{
			File.Delete(KEYGROUPS_FILE_PATH);
		}
		try
		{
			NssJsonSerializer.SerializeWithBinding(list, KEYGROUPS_FILE_PATH, ACCEPTED_JSON_TYPES);
		}
		catch (Exception exception)
		{
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
		}
	}

	private bool CreateStorageDirectoryIfNeeded()
	{
		if (!Directory.Exists(KEYGROUPS_DIRECTORY_PATH))
		{
			Directory.CreateDirectory(KEYGROUPS_DIRECTORY_PATH);
			return false;
		}
		return true;
	}
}
