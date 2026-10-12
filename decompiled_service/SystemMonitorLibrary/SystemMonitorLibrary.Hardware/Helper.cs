using System.Collections.Generic;

namespace SystemMonitorLibrary.Hardware;

internal static class Helper
{
	public static void DictionaryCheckAssign(ref Dictionary<string, string> objDictionary, string key, string value)
	{
		if (!objDictionary.ContainsKey(key))
		{
			objDictionary.Add(key, value);
		}
		else
		{
			objDictionary[key] = value;
		}
	}
}
