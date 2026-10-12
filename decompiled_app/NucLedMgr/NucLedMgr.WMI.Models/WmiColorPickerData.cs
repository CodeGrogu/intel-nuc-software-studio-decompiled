using System.Windows.Media;
using NucLedMgr.Interfaces;
using NucLedMgr.Models;

namespace NucLedMgr.WMI.Models;

public class WmiColorPickerData : ColorPickerData
{
	public delegate void ColorChangeDelegate(Color color);

	private readonly ColorChangeDelegate colorChangeDelegate;

	private bool _isApplyClick;

	private Color _selectedColorCanvas;

	private Color _currentColor;

	public override Color SelectedColor
	{
		get
		{
			return _selectedColor;
		}
		set
		{
			SetProperty(ref _selectedColor, value, "SelectedColor");
			_selectedColorCanvas = value;
			colorChangeDelegate(value);
			RaisePropertyChanged("SelectedColor");
		}
	}

	public bool IsApplyClick
	{
		get
		{
			return _isApplyClick;
		}
		set
		{
			SetProperty(ref _isApplyClick, value, "IsApplyClick");
		}
	}

	public Color SelectedColorCanvas
	{
		get
		{
			return _selectedColorCanvas;
		}
		set
		{
			SetProperty(ref _selectedColorCanvas, value, "SelectedColorCanvas");
		}
	}

	public Color CurrentColor
	{
		get
		{
			return _currentColor;
		}
		set
		{
			SetProperty(ref _currentColor, value, "CurrentColor");
			RaisePropertyChanged("CurrentColor");
			_selectedColor = value;
			if (IsApplyClick)
			{
				_colorMgr.CurrentColor = value;
				RaisePropertyChanged("SelectedColor");
				colorChangeDelegate(value);
				RaisePropertyChanged("RecentColors");
			}
		}
	}

	public Color PlaceholderColor
	{
		get
		{
			return _selectedColor;
		}
		set
		{
			SetProperty(ref _selectedColor, value, "PlaceholderColor");
		}
	}

	public WmiColorPickerData(int index, bool enabled, bool rainbow, IColorManager colorMgr, Color selectedColor, ColorChangeDelegate delegateFunction)
		: base(index, rainbow, colorMgr, selectedColor)
	{
		_selectedColor = selectedColor;
		_currentColor = selectedColor;
		colorChangeDelegate = delegateFunction;
	}

	protected override void UpdateColor(Color? newColor)
	{
		if (newColor.HasValue)
		{
			IsApplyClick = false;
			SelectedColor = newColor.Value;
			CurrentColor = newColor.Value;
			SelectedColorCanvas = newColor.Value;
			_colorMgr.CurrentColor = newColor.Value;
			RaisePropertyChanged("SelectedColor");
			colorChangeDelegate(newColor.Value);
			RaisePropertyChanged("RecentColors");
		}
	}
}
