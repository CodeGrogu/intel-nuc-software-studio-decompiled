using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using IntelSdkClient.ENE.Interface;
using NucLedMgr.Interfaces;
using Prism.Events;
using Prism.Mvvm;

namespace NucLedMgr.ENE.Models;

internal class EneColorManager : BindableBase, IColorManager
{
	private const int NUM_COLOR = 8;

	private Color _currentColor;

	private Color _first;

	private Color _second;

	private Color _third;

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
				if (RecentColors != null && !RecentColors.Contains(_currentColor))
				{
					if (RecentColors.Count >= 8)
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

	public EneColorManager(IEventAggregator events, IEneClient eneClient)
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
			Color.FromArgb(byte.MaxValue, byte.MaxValue, 105, 180),
			Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		};
	}
}
