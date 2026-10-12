using System.Collections.Generic;

namespace MobileKeyboardLedMgr.Storage.Data;

public class KeyGroupStorageData
{
	public string Name { get; set; }

	public List<int> SelectedKeyIds { get; set; }
}
