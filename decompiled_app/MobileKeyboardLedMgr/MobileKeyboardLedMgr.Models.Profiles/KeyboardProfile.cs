using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;

namespace MobileKeyboardLedMgr.Models.Profiles;

[JsonObject(MemberSerialization.OptIn)]
public class KeyboardProfile : BindableBase
{
	private bool _selected;

	private DelegateCommand _selectItem;

	public string Id { get; }

	public string Name { get; set; }

	public KeyboardPattern Pattern { get; set; }

	public int AcIdleTimer { get; set; }

	public int DcIdleTimer { get; set; }

	public bool IsEnabled { get; set; }

	public bool Selected
	{
		get
		{
			return _selected;
		}
		set
		{
			SetProperty(ref _selected, value, "Selected");
		}
	}

	public bool Default { get; set; }

	public DelegateCommand SelectItem
	{
		get
		{
			return _selectItem;
		}
		set
		{
			SetProperty(ref _selectItem, value, "SelectItem");
		}
	}

	public KeyboardProfile(KeyboardProfile profile)
	{
		_selectItem = new DelegateCommand(() =>
		{
			Selected = true;
		});
		Id = profile.Id;
		Name = profile.Name;
		Pattern = profile.Pattern;
		AcIdleTimer = profile.AcIdleTimer;
		DcIdleTimer = profile.DcIdleTimer;
		IsEnabled = profile.IsEnabled;
		Selected = profile.Selected;
		Default = profile.Default;
	}

	public KeyboardProfile(string name, KeyboardPattern pattern, int acIdleTimer, int dcIdleTimer, bool isEnabled)
	{
		_selectItem = new DelegateCommand(() =>
		{
			Selected = true;
		});
		Id = name;
		Name = name;
		Pattern = pattern;
		AcIdleTimer = acIdleTimer;
		DcIdleTimer = dcIdleTimer;
		IsEnabled = isEnabled;
		Selected = false;
		Default = false;
	}

	public KeyboardProfile()
	{
		_selectItem = new DelegateCommand(() =>
		{
			Selected = true;
		});
		Selected = false;
		Default = false;
		IsEnabled = true;
	}

	public SerializableKeyboardProfile GetSerializableKeyboardProfile()
	{
		return new SerializableKeyboardProfile(this);
	}
}
