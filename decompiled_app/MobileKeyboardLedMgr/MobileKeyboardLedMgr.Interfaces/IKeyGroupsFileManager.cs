using System.Collections.Generic;
using System.Windows.Media;
using MobileKeyboardLedMgr.Models;

namespace MobileKeyboardLedMgr.Interfaces;

public interface IKeyGroupsFileManager
{
	List<KeyGroup> ReadSavedKeyGroups(Color selectedColor);

	void SaveKeyGroups(List<KeyGroup> keyGroups);
}
