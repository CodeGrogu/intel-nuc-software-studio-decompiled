using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using FeatureMgr.Enums;
using FeatureMgr.Models;
using NucLedMgr.Interfaces;
using Prism.Mvvm;

namespace NucLedMgr.WMI.Models;

public class ColorManager : BindableBase, IColorManager
{
	private const int NUM_COLOR = 7;

	private NucType _nucType;

	private Color _currentColor;

	private Color _first;

	private Color _second;

	private Color _third;

	public NucType NucType
	{
		get
		{
			return _nucType;
		}
		set
		{
			SetProperty(ref _nucType, value, "NucType");
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
			_currentColor = value;
			try
			{
				if (!RecentColors.Contains(_currentColor))
				{
					if (RecentColors.Count >= 7)
					{
						RecentColors.Remove(RecentColors.First());
					}
					RecentColors.Add(_currentColor);
				}
			}
			catch
			{
			}
		}
	}

	public Color First
	{
		get
		{
			return _first;
		}
		set
		{
			SetProperty(ref _first, value, "First");
		}
	}

	public Color Second
	{
		get
		{
			return _second;
		}
		set
		{
			SetProperty(ref _second, value, "Second");
		}
	}

	public Color Third
	{
		get
		{
			return _third;
		}
		set
		{
			SetProperty(ref _third, value, "Third");
		}
	}

	public ObservableCollection<Color> StandardColors { get; set; }

	public ObservableCollection<Color> RecentColors { get; set; }

	public ObservableCollection<Color> PantherPowerColors { get; set; }

	public ColorManager()
	{
		_nucType = PlatformDetail.GetNucType();
		_currentColor = Colors.Purple;
		if (_nucType == NucType.Panther || _nucType == NucType.Phantom || _nucType == NucType.Serpent)
		{
			StandardColors = new ObservableCollection<Color>
			{
				Colors.Red,
				Colors.Orange,
				Colors.Yellow,
				Colors.Green,
				Colors.Blue,
				Colors.Indigo,
				Colors.Violet,
				Colors.White
			};
			RecentColors = new ObservableCollection<Color>
			{
				Colors.Red,
				Colors.Orange,
				Colors.Yellow,
				Colors.Green,
				Colors.Blue,
				Colors.Indigo,
				Colors.Violet
			};
		}
		else
		{
			StandardColors = new ObservableCollection<Color>
			{
				Color.FromArgb(byte.MaxValue, byte.MaxValue, 0, 0),
				Colors.Orange,
				Colors.Yellow,
				Color.FromArgb(byte.MaxValue, 0, byte.MaxValue, 0),
				Color.FromArgb(byte.MaxValue, 0, byte.MaxValue, byte.MaxValue),
				Color.FromArgb(byte.MaxValue, 0, 0, byte.MaxValue),
				Color.FromArgb(byte.MaxValue, 85, 0, 171),
				Color.FromArgb(byte.MaxValue, byte.MaxValue, 105, 180)
			};
			RecentColors = new ObservableCollection<Color>
			{
				Color.FromArgb(byte.MaxValue, byte.MaxValue, 0, 0),
				Colors.Orange,
				Colors.Yellow,
				Color.FromArgb(byte.MaxValue, 0, byte.MaxValue, 0),
				Color.FromArgb(byte.MaxValue, 0, 0, byte.MaxValue),
				Color.FromArgb(byte.MaxValue, 85, 0, 171),
				Color.FromArgb(byte.MaxValue, byte.MaxValue, 105, 180)
			};
		}
		PantherPowerColors = new ObservableCollection<Color>
		{
			Colors.Blue,
			Color.FromArgb(byte.MaxValue, byte.MaxValue, 194, 0)
		};
	}
}
