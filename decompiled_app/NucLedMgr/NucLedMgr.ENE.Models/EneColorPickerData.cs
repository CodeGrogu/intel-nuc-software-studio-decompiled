using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Media;
using NucLedMgr.Interfaces;
using NucLedMgr.Models;

namespace NucLedMgr.ENE.Models;

public class EneColorPickerData : ColorPickerData
{
	public delegate void ColorChangeDelegate(bool SetOnly);

	private readonly ColorChangeDelegate _colorChangeDelegate;

	private const string SELECTED_COLOR_VALUE_PROPERTY = "SelectedColorValue";

	private int _selectedColorValue;

	private const string SELECTED_COLOR_PROPERTY = "SelectedColor";

	public int SelectedColorValue
	{
		get
		{
			return _selectedColorValue;
		}
		set
		{
			SetProperty(ref _selectedColorValue, value, "SelectedColorValue");
			_selectedColor = ColorFromFastLedHsv(value);
			_colorMgr.CurrentColor = _selectedColor;
			RaisePropertyChanged("SelectedColor");
			_colorChangeDelegate(SetOnly: true);
		}
	}

	public override System.Windows.Media.Color SelectedColor
	{
		get
		{
			return _selectedColor;
		}
		set
		{
			SetProperty(ref _selectedColor, value, "SelectedColor");
			_colorMgr.CurrentColor = value;
			_selectedColorValue = FastLedHueFromColor(value);
			RaisePropertyChanged("SelectedColorValue");
			_colorChangeDelegate(SetOnly: true);
		}
	}

	public int ColorCount { get; set; }

	public EneColorPickerData(int index, bool rainbow, IColorManager colorMgr, System.Windows.Media.Color selectedColor, bool enable, int colorCount, ColorChangeDelegate delegateFunction)
		: base(index, rainbow, colorMgr, selectedColor)
	{
		_selectedColorValue = FastLedHueFromColor(selectedColor);
		_colorChangeDelegate = delegateFunction;
		EnableColor = enable;
		ColorCount = colorCount;
	}

	public void UpdateUiColorHsv(int ColorValue)
	{
		_selectedColorValue = ColorValue;
		_selectedColor = ColorFromFastLedHsv(ColorValue);
		_colorMgr.CurrentColor = _selectedColor;
		RaisePropertyChanged("SelectedColorValue");
		RaisePropertyChanged("SelectedColor");
	}

	public void UpdateUiColor(System.Windows.Media.Color newColor)
	{
		_selectedColorValue = FastLedHueFromColor(newColor);
		_selectedColor = newColor;
		_colorMgr.CurrentColor = _selectedColor;
		RaisePropertyChanged("SelectedColorValue");
		RaisePropertyChanged("SelectedColor");
	}

	protected override void UpdateColor(System.Windows.Media.Color? newColor)
	{
		Task.Run(() =>
		{
			if (newColor.HasValue)
			{
				_selectedColorValue = FastLedHueFromColor(newColor.Value);
				_selectedColor = ColorFromFastLedHsv(_selectedColorValue);
				_colorMgr.CurrentColor = _selectedColor;
				_colorChangeDelegate(SetOnly: false);
				RaisePropertyChanged("SelectedColor");
				RaisePropertyChanged("RecentColors");
			}
		});
	}

	public static System.Windows.Media.Color ColorFromFastLedHsv(int FastLedHue)
	{
		return FastLedHue switch
		{
			0 => System.Windows.Media.Color.FromArgb(byte.MaxValue, byte.MaxValue, 0, 0), 
			32 => System.Windows.Media.Color.FromArgb(byte.MaxValue, byte.MaxValue, 165, 0), 
			70 => System.Windows.Media.Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0), 
			96 => System.Windows.Media.Color.FromArgb(byte.MaxValue, 0, byte.MaxValue, 0), 
			128 => System.Windows.Media.Color.FromArgb(byte.MaxValue, 0, byte.MaxValue, byte.MaxValue), 
			160 => System.Windows.Media.Color.FromArgb(byte.MaxValue, 0, 0, byte.MaxValue), 
			192 => System.Windows.Media.Color.FromArgb(byte.MaxValue, 85, 0, 171), 
			224 => System.Windows.Media.Color.FromArgb(byte.MaxValue, byte.MaxValue, 105, 180), 
			255 => System.Windows.Media.Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 
			_ => System.Windows.Media.Color.FromArgb(byte.MaxValue, byte.MaxValue, 0, 0), 
		};
	}

	public static int FastLedHueFromColor(System.Windows.Media.Color color)
	{
		System.Drawing.Color color2 = System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
		if (color2.A == byte.MaxValue && color2.R == byte.MaxValue && color2.G == byte.MaxValue && color2.B == byte.MaxValue)
		{
			return 255;
		}
		return ConvertFastLedRainbowHue((int)(color2.GetHue() / 360f * 255f));
	}

	public static int ConvertFastLedRainbowHue(int FastLedHue)
	{
		int num = 0;
		return FastLedHue switch
		{
			0 => 0, 
			27 => 32, 
			42 => 70, 
			85 => 96, 
			127 => 128, 
			170 => 160, 
			191 => 192, 
			233 => 224, 
			_ => 0, 
		};
	}
}
