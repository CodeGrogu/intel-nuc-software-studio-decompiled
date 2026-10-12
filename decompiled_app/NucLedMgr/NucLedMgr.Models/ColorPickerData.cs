using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Media;
using Infrastructure.Converters;
using NucLedMgr.Interfaces;
using Prism.Commands;
using Prism.Mvvm;

namespace NucLedMgr.Models;

public class ColorPickerData : BindableBase
{
	protected IColorManager _colorMgr;

	private const string COLOR_RESOURCE_KEY = "Color";

	private string _currentColorString;

	protected System.Windows.Media.Color _selectedColor;

	protected ObservableCollection<System.Windows.Media.Color> _colors;

	private bool _rainbow;

	private bool _enableColor;

	protected const string RECENT_COLORS_PROPERTY = "RecentColors";

	private DelegateCommand<System.Windows.Media.Color?> _selectColor;

	private DelegateCommand _openColorPicker;

	public int Index { get; private set; }

	public string CurrentColorString
	{
		get
		{
			return _currentColorString;
		}
		set
		{
			SetProperty(ref _currentColorString, value, "CurrentColorString");
		}
	}

	public virtual System.Windows.Media.Color SelectedColor
	{
		get
		{
			return _selectedColor;
		}
		set
		{
			SetProperty(ref _selectedColor, value, "SelectedColor");
			_colorMgr.CurrentColor = value;
		}
	}

	public virtual ObservableCollection<System.Windows.Media.Color> Colors
	{
		get
		{
			return _colors;
		}
		set
		{
			SetProperty(ref _colors, value, "Colors");
		}
	}

	public bool Rainbow
	{
		get
		{
			return _rainbow;
		}
		set
		{
			SetProperty(ref _rainbow, value, "Rainbow");
		}
	}

	public bool EnableColor
	{
		get
		{
			return _enableColor;
		}
		set
		{
			SetProperty(ref _enableColor, value, "EnableColor");
		}
	}

	public ObservableCollection<System.Windows.Media.Color> StandardColors => _colorMgr.StandardColors;

	public ObservableCollection<System.Windows.Media.Color> PantherPowerColors => _colorMgr.PantherPowerColors;

	public ObservableCollection<System.Windows.Media.Color> RecentColors => _colorMgr.RecentColors;

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

	public ColorPickerData(int index, bool rainbow, IColorManager colorMgr, System.Windows.Media.Color selectedColor)
	{
		Index = index;
		_currentColorString = ResourceDictionaryConverter.GetString("Color") + " " + (index + 1);
		_colorMgr = colorMgr;
		_selectedColor = selectedColor;
		_selectColor = new DelegateCommand<System.Windows.Media.Color?>(UpdateColor);
		_openColorPicker = new DelegateCommand(SelectFromColorPicker);
		_rainbow = rainbow;
	}

	protected virtual void UpdateColor(System.Windows.Media.Color? newColor)
	{
		if (newColor.HasValue)
		{
			SelectedColor = newColor.Value;
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

	public void ShowRainbow(bool show)
	{
		Rainbow = show;
	}
}
