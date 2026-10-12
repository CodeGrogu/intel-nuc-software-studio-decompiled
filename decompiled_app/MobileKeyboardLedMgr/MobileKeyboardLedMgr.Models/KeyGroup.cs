using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using MobileKeyboardLedMgr.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using UniwillSdkClient.UniwillService;

namespace MobileKeyboardLedMgr.Models;

public class KeyGroup : BindableBase
{
	private readonly IEventAggregator _events;

	private string _name;

	private KeyboardUiData _keyboardUiData;

	private DelegateCommand _applyKeyGroup;

	public string Name
	{
		get
		{
			return _name;
		}
		set
		{
			SetProperty(ref _name, value, "Name");
		}
	}

	public KeyboardUiData KeyboardUiData
	{
		get
		{
			return _keyboardUiData;
		}
		set
		{
			SetProperty(ref _keyboardUiData, value, "KeyboardUiData");
		}
	}

	public Color SelectedColor
	{
		get
		{
			return _keyboardUiData.SelectedColor;
		}
		set
		{
			if (SelectedColor != value)
			{
				KeyboardUiData.SelectedColor = value;
				KeyboardUiData.UpdateMultipleKeys(GetSelectedKeyUiData());
			}
		}
	}

	public HashSet<int> SelectedKeyIds => _keyboardUiData.SelectedKeyIds;

	public DelegateCommand ApplyKeyGroup
	{
		get
		{
			return _applyKeyGroup;
		}
		set
		{
			SetProperty(ref _applyKeyGroup, value, "ApplyKeyGroup");
		}
	}

	public KeyGroup(IEventAggregator events, RGBKeyboardCapabilities keyboardType, string name, Color selectedColor, HashSet<int> selectedKeyIds)
	{
		_events = events;
		_name = name;
		_keyboardUiData = new KeyboardUiData(events, keyboardType, editable: false, selectedColor, isForKeyGroup: true, selectedKeyIds);
		if (selectedKeyIds != null && selectedKeyIds.Count > 0)
		{
			KeyboardUiData.UpdateMultipleKeys(GetSelectedKeyUiData());
		}
		_applyKeyGroup = new DelegateCommand(ApplyKeyGroupToKeyboard);
	}

	public KeyGroup DeepCopy()
	{
		return new KeyGroup(_events)
		{
			Name = _name,
			KeyboardUiData = _keyboardUiData.DeepCopy()
		};
	}

	private KeyGroup(IEventAggregator events)
	{
		_events = events;
		_applyKeyGroup = new DelegateCommand(ApplyKeyGroupToKeyboard);
	}

	private void ApplyKeyGroupToKeyboard()
	{
		List<KeyUiData> list = new List<KeyUiData>();
		foreach (KeyUiData selectedKeyUiDatum in GetSelectedKeyUiData())
		{
			list.Add(new KeyUiData(selectedKeyUiDatum.Id, Color.FromArgb(selectedKeyUiDatum.LedColor.A, selectedKeyUiDatum.LedColor.R, selectedKeyUiDatum.LedColor.G, selectedKeyUiDatum.LedColor.B), selected: false, selectedKeyUiDatum.Label, 1.0, on: false));
		}
		_events.GetEvent<UpdateMultipleKeys>().Publish(list);
	}

	private List<KeyUiData> GetSelectedKeyUiData()
	{
		return (from k in KeyboardUiData.KeysUiDataDict
			where SelectedKeyIds.Contains(k.Key)
			select k.Value).ToList();
	}

	public override string ToString()
	{
		return Name;
	}
}
