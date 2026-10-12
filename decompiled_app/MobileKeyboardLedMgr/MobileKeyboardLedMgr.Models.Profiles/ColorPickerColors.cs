using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Media;
using MobileKeyboardLedMgr.Events;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using UniwillSdkClient.UniwillService;

namespace MobileKeyboardLedMgr.Models.Profiles;

[JsonObject(MemberSerialization.OptIn)]
public class ColorPickerColors : BindableBase
{
	protected readonly IEventAggregator _events;

	private int _index;

	protected System.Windows.Media.Color _selectedColor;

	private bool _selected;

	private RGBKeyboardEffect _effect;

	private bool _enabled;

	private DelegateCommand<System.Windows.Media.Color?> _selectColor;

	private DelegateCommand _openColorPicker;

	private DelegateCommand _toggleColor;

	[JsonProperty]
	public int Index
	{
		get
		{
			return _index;
		}
		private set
		{
			SetProperty(ref _index, value, "Index");
		}
	}

	[JsonProperty]
	public virtual System.Windows.Media.Color SelectedColor
	{
		get
		{
			return _selectedColor;
		}
		set
		{
			SetProperty(ref _selectedColor, value, "SelectedColor");
		}
	}

	[JsonProperty]
	public bool Selected
	{
		get
		{
			return _selected;
		}
		set
		{
			SetProperty(ref _selected, value, "Selected");
			ToggleCurrentColor();
			Enabled = value;
		}
	}

	public RGBKeyboardEffect Effect
	{
		get
		{
			return _effect;
		}
		set
		{
			SetProperty(ref _effect, value, "Effect");
		}
	}

	public bool IsSelectable
	{
		get
		{
			bool flag = true;
			if (_effect == RGBKeyboardEffect.Wave)
			{
				return _index switch
				{
					0 => false, 
					1 => false, 
					_ => true, 
				};
			}
			if (_index == 0)
			{
				return false;
			}
			return true;
		}
	}

	[JsonProperty]
	public bool Enabled
	{
		get
		{
			return _enabled;
		}
		set
		{
			SetProperty(ref _enabled, value, "Enabled");
			_selected = value;
			RaisePropertyChanged("Selected");
		}
	}

	public DelegateCommand<System.Windows.Media.Color?> SelectColor
	{
		get
		{
			return _selectColor;
		}
		set
		{
			SetProperty(ref _selectColor, value, "SelectColor");
		}
	}

	public ObservableCollection<System.Windows.Media.Color> StandardColors { get; private set; }

	public ObservableCollection<System.Windows.Media.Color> RecentColors { get; private set; }

	public DelegateCommand OpenColorPicker
	{
		get
		{
			return _openColorPicker;
		}
		set
		{
			SetProperty(ref _openColorPicker, value, "OpenColorPicker");
		}
	}

	public DelegateCommand ToggleColor
	{
		get
		{
			return _toggleColor;
		}
		set
		{
			SetProperty(ref _toggleColor, value, "ToggleColor");
		}
	}

	public ColorPickerColors(IEventAggregator events, int index, System.Windows.Media.Color selectedColor, RGBKeyboardEffect effect, ObservableCollection<System.Windows.Media.Color> standardColors, ObservableCollection<System.Windows.Media.Color> recentColors, bool editingEnabled, bool selected)
	{
		_events = events;
		_index = index;
		_selectedColor = selectedColor;
		_enabled = editingEnabled;
		_selected = selected;
		_effect = effect;
		_selectColor = new DelegateCommand<System.Windows.Media.Color?>(UpdateColor);
		_openColorPicker = new DelegateCommand(SelectFromColorPicker);
		_toggleColor = new DelegateCommand(ToggleCurrentColor);
		StandardColors = standardColors;
		RecentColors = recentColors;
	}

	public ColorPickerColors DeepCopy()
	{
		return new ColorPickerColors(_events, _index, System.Windows.Media.Color.FromArgb(_selectedColor.A, _selectedColor.R, _selectedColor.G, _selectedColor.B), _effect, StandardColors, RecentColors, _enabled, _selected);
	}

	public void Deselect()
	{
		_selected = false;
		Enabled = false;
	}

	public void Select()
	{
		_selected = true;
		Enabled = true;
	}

	protected virtual void UpdateColor(System.Windows.Media.Color? newColor)
	{
		if (newColor.HasValue)
		{
			SelectedColor = newColor.Value;
			_events.GetEvent<UpdateColorFromColorPickerEvent>().Publish(this);
			RaisePropertyChanged("RecentColors");
		}
	}

	private void SelectFromColorPicker()
	{
		using ColorDialog colorDialog = new ColorDialog
		{
			FullOpen = true,
			Color = System.Drawing.Color.FromArgb(SelectedColor.A, SelectedColor.R, SelectedColor.G, SelectedColor.B)
		};
		colorDialog.ShowDialog();
		System.Drawing.Color color = colorDialog.Color;
		UpdateColor(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));
	}

	private void ToggleCurrentColor()
	{
		_events.GetEvent<ToggleColorEvent>().Publish(this);
	}

	public SerializableColorPickerColors GetSerializableColorPickerColors()
	{
		return new SerializableColorPickerColors(this);
	}
}
