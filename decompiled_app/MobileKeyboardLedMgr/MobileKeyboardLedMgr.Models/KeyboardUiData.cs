using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using Infrastructure.Converters;
using MobileKeyboardLedMgr.Constants;
using MobileKeyboardLedMgr.Events;
using MobileKeyboardLedMgr.Models.Profiles;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using UniwillSdkClient.UniwillService;

namespace MobileKeyboardLedMgr.Models;

public class KeyboardUiData : BindableBase
{
	private readonly IEventAggregator _events;

	private RGBKeyboardCapabilities _keyboardType;

	private List<List<KeyUiData>> _keysUiData;

	private bool _editable;

	private DelegateCommand<KeyUiData> _keyStateChanged;

	public List<List<KeyUiData>> KeysUiData
	{
		get
		{
			return _keysUiData;
		}
		set
		{
			SetProperty(ref _keysUiData, value, "KeysUiData");
		}
	}

	public bool Editable
	{
		get
		{
			return _editable;
		}
		set
		{
			SetProperty(ref _editable, value, "Editable");
		}
	}

	public DelegateCommand<KeyUiData> KeyStateChanged
	{
		get
		{
			return _keyStateChanged;
		}
		set
		{
			SetProperty(ref _keyStateChanged, value, "KeyStateChanged");
		}
	}

	public Color SelectedColor { get; set; }

	public bool IsForKeyGroup { get; set; }

	public HashSet<int> SelectedKeyIds { get; set; }

	public Dictionary<int, KeyUiData> KeysUiDataDict { get; }

	public KeyboardUiData(IEventAggregator events, RGBKeyboardCapabilities keyboardType, bool editable, Color selectedColor, bool isForKeyGroup, HashSet<int> selectedKeyIds)
	{
		_events = events;
		_keyboardType = keyboardType;
		_editable = editable;
		_keysUiData = new List<List<KeyUiData>>();
		for (int i = 0; i < 6; i++)
		{
			_keysUiData.Add(GenerateRow(KeyboardCapabilities.GetFirstKeyInRow(_keyboardType, i), KeyboardCapabilities.GetLastKeyInRow(keyboardType, i)));
		}
		SelectedColor = selectedColor;
		IsForKeyGroup = isForKeyGroup;
		KeysUiDataDict = BuildKeysUiDataDict();
		SelectedKeyIds = selectedKeyIds ?? new HashSet<int>();
		_keyStateChanged = new DelegateCommand<KeyUiData>(UpdateKey);
		if (!isForKeyGroup)
		{
			_events.GetEvent<UpdateMultipleKeys>().Subscribe(UpdateMultipleKeys);
		}
	}

	public void UpdateKey(KeyUiData key)
	{
		if (!key.Selected)
		{
			return;
		}
		if (key.LedColor != SelectedColor)
		{
			key.LedColor = SelectedColor;
			key.On = true;
			key.Selected = false;
			if (!IsForKeyGroup)
			{
				_events.GetEvent<CustomKeyColorChangeEvent>().Publish(new KeyColorData(key.Id, key.LedColor, enabled: true));
				return;
			}
			UpdateSelectedKeyIds(new List<KeyUiData>(1) { key }, areOffInUi: false);
		}
		else
		{
			key.On = false;
			key.Selected = false;
			if (!IsForKeyGroup)
			{
				_events.GetEvent<CustomKeyColorChangeEvent>().Publish(new KeyColorData(key.Id, DefaultColors.OFF_COLOR_UI, enabled: true));
				return;
			}
			UpdateSelectedKeyIds(new List<KeyUiData>(1) { key }, areOffInUi: true);
		}
	}

	public void UpdateMultipleKeys(List<KeyUiData> keys)
	{
		List<KeyColorData> list = new List<KeyColorData>();
		bool areOffInUi = false;
		List<KeyUiData> list2 = new List<KeyUiData>();
		foreach (KeyUiData key in keys)
		{
			List<int> list3 = KeyboardCapabilities.MultiRowKeyIds(_keyboardType, key.Id);
			if (list3.Count <= 1)
			{
				continue;
			}
			foreach (int item in list3)
			{
				if (item != key.Id)
				{
					list2.Add(KeysUiDataDict[item]);
				}
			}
		}
		keys.AddRange(list2);
		if (keys.Any((KeyUiData k) => KeysUiDataDict[k.Id].LedColor != SelectedColor))
		{
			foreach (KeyUiData key2 in keys)
			{
				key2.LedColor = SelectedColor;
				key2.On = true;
				key2.Selected = false;
				list.Add(new KeyColorData(key2.Id, key2.LedColor, enabled: true));
			}
		}
		else
		{
			foreach (KeyUiData key3 in keys)
			{
				key3.On = false;
				key3.Selected = false;
				areOffInUi = true;
				list.Add(new KeyColorData(key3.Id, DefaultColors.OFF_COLOR_UI, enabled: true));
			}
		}
		if (list.Count > 0 && !IsForKeyGroup)
		{
			_events.GetEvent<MultipleCustomKeyColorsChangedEvent>().Publish(list);
		}
		else
		{
			UpdateSelectedKeyIds(keys, areOffInUi);
		}
	}

	private List<KeyUiData> GenerateRow(int start, int end)
	{
		List<KeyUiData> list = new List<KeyUiData>();
		string text = "";
		switch (_keyboardType)
		{
		case RGBKeyboardCapabilities.KEY86:
			text = "iso";
			break;
		case RGBKeyboardCapabilities.KEY87:
			text = "jis";
			break;
		}
		for (int i = start; i <= end; i++)
		{
			string label = ResourceDictionaryConverter.GetString("key" + i + text);
			double keySpan = KeyboardCapabilities.GetKeySpan(_keyboardType, i);
			bool secondRow = KeyboardCapabilities.IsSecondRow(_keyboardType, i);
			list.Add(new KeyUiData(i, DefaultColors.OFF_COLOR_UI, selected: false, label, keySpan, on: false, secondRow));
		}
		return list;
	}

	public void ApplyPattern(ObservableCollection<KeyColorData> keys)
	{
		int num = 0;
		foreach (List<KeyUiData> keysUiDatum in _keysUiData)
		{
			foreach (KeyUiData item in keysUiDatum)
			{
				if (num >= keys.Count())
				{
					item.On = false;
					continue;
				}
				item.On = keys[num].SelectedColor != DefaultColors.OFF_COLOR_UI;
				item.LedColor = keys[num].SelectedColor;
				num++;
			}
		}
	}

	public void DisableAll()
	{
		foreach (List<KeyUiData> keysUiDatum in KeysUiData)
		{
			foreach (KeyUiData item in keysUiDatum)
			{
				item.On = false;
			}
		}
	}

	public KeyboardUiData DeepCopy()
	{
		List<List<KeyUiData>> list = new List<List<KeyUiData>>();
		HashSet<int> hashSet = new HashSet<int>();
		Dictionary<int, KeyUiData> dictionary = new Dictionary<int, KeyUiData>();
		foreach (List<KeyUiData> keysUiDatum in _keysUiData)
		{
			List<KeyUiData> list2 = new List<KeyUiData>();
			foreach (KeyUiData item in keysUiDatum)
			{
				KeyUiData keyUiData = item.DeepCopy();
				list2.Add(keyUiData);
				dictionary[keyUiData.Id] = keyUiData;
				if (SelectedKeyIds.Contains(keyUiData.Id))
				{
					hashSet.Add(keyUiData.Id);
				}
			}
			list.Add(list2);
		}
		return new KeyboardUiData(_events, dictionary)
		{
			Editable = _editable,
			SelectedColor = Color.FromArgb(SelectedColor.A, SelectedColor.R, SelectedColor.G, SelectedColor.B),
			IsForKeyGroup = IsForKeyGroup,
			KeysUiData = list,
			SelectedKeyIds = hashSet
		};
	}

	private KeyboardUiData(IEventAggregator events, Dictionary<int, KeyUiData> copiedKeysUiDataDict)
	{
		_events = events;
		KeysUiDataDict = copiedKeysUiDataDict;
		_keyStateChanged = new DelegateCommand<KeyUiData>(UpdateKey);
	}

	private void UpdateSelectedKeyIds(List<KeyUiData> keys, bool areOffInUi)
	{
		foreach (KeyUiData key in keys)
		{
			if (areOffInUi)
			{
				SelectedKeyIds.Remove(key.Id);
			}
			else
			{
				SelectedKeyIds.Add(key.Id);
			}
		}
	}

	private Dictionary<int, KeyUiData> BuildKeysUiDataDict()
	{
		Dictionary<int, KeyUiData> dictionary = new Dictionary<int, KeyUiData>();
		foreach (List<KeyUiData> keysUiDatum in KeysUiData)
		{
			foreach (KeyUiData item in keysUiDatum)
			{
				dictionary[item.Id] = item;
			}
		}
		return dictionary;
	}
}
