using System.Windows;
using System.Windows.Media;
using MobileKeyboardLedMgr.Constants;
using Prism.Mvvm;

namespace MobileKeyboardLedMgr.Models;

public class KeyUiData : BindableBase
{
	private const int HEIGHT = 25;

	private const int MARGIN = 1;

	public int _id;

	private Color _ledColor;

	private bool _selected;

	private bool? _on;

	public int Id
	{
		get
		{
			return _id;
		}
		set
		{
			SetProperty(ref _id, value, "Id");
		}
	}

	public Color LedColor
	{
		get
		{
			return _ledColor;
		}
		set
		{
			SetProperty(ref _ledColor, value, "LedColor");
		}
	}

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

	public string Label { get; set; }

	public double Width { get; set; }

	public double Height { get; set; }

	public Thickness Margin { get; set; }

	public Thickness BorderThickness { get; set; }

	public bool SecondRow { get; set; }

	public bool? On
	{
		get
		{
			return _on;
		}
		set
		{
			SetProperty(ref _on, value, "On");
			if (_on == false)
			{
				LedColor = DefaultColors.OFF_COLOR_UI;
			}
		}
	}

	public KeyUiData(int id, Color ledColor, bool selected, string label, double span, bool on, bool secondRow = false)
	{
		_id = id;
		_on = on;
		SecondRow = secondRow;
		LedColor = ledColor;
		Selected = selected;
		Width = 25.0 * span;
		if (span > 1.0)
		{
			Width += 2.0 * (span - 1.0);
		}
		if (!secondRow)
		{
			Height = 25.0;
			Margin = new Thickness(0.5);
			BorderThickness = new Thickness(0.75);
			Label = label;
		}
		else
		{
			Height = 27.25;
			Margin = new Thickness(0.5, -1.5, 0.5, 0.5);
			BorderThickness = new Thickness(0.75, 0.0, 0.75, 0.75);
			Label = "";
		}
	}

	public KeyUiData DeepCopy()
	{
		return new KeyUiData
		{
			Id = _id,
			LedColor = Color.FromArgb(_ledColor.A, _ledColor.R, _ledColor.G, _ledColor.B),
			Selected = _selected,
			Label = Label,
			Width = Width,
			Height = Height,
			On = _on
		};
	}

	private KeyUiData()
	{
	}
}
