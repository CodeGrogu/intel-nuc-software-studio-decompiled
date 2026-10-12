using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FeatureMgr.Events;
using FeatureMgr.Interfaces;
using FeatureMgr.Properties;
using Infrastructure.Converters;
using Infrastructure.Events;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace FeatureMgr.ViewModels;

public class ProfilesViewModel : BindableBase, INavigationAware
{
	private readonly IEventAggregator _events;

	private bool _supported;

	private List<string> _primaryContentRegions = new List<string>();

	private List<string> _secondaryContentRegions = new List<string>();

	private List<string> _buttonContentRegions = new List<string>();

	private bool _showPoweredByText;

	private ImageSource _nssIcon;

	private int _selectedProfile = -1;

	public bool Supported
	{
		get
		{
			return _supported;
		}
		set
		{
			SetProperty(ref _supported, value, "Supported");
		}
	}

	public List<string> PrimaryContentRegions
	{
		get
		{
			return _primaryContentRegions;
		}
		set
		{
			SetProperty(ref _primaryContentRegions, value, "PrimaryContentRegions");
		}
	}

	public List<string> SecondaryContentRegions
	{
		get
		{
			return _secondaryContentRegions;
		}
		set
		{
			SetProperty(ref _secondaryContentRegions, value, "SecondaryContentRegions");
		}
	}

	public List<string> ButtonContentRegions
	{
		get
		{
			return _buttonContentRegions;
		}
		set
		{
			SetProperty(ref _buttonContentRegions, value, "ButtonContentRegions");
		}
	}

	public string PoweredByText => ResourceDictionaryConverter.GetString("PoweredByNssString");

	public bool ShowPoweredByText
	{
		get
		{
			return _showPoweredByText;
		}
		set
		{
			SetProperty(ref _showPoweredByText, value, "ShowPoweredByText");
		}
	}

	public ImageSource NssIcon
	{
		get
		{
			return _nssIcon;
		}
		set
		{
			SetProperty(ref _nssIcon, value, "NssIcon");
		}
	}

	public int SelectedProfile
	{
		get
		{
			return _selectedProfile;
		}
		set
		{
			SetProperty(ref _selectedProfile, value, "SelectedProfile");
		}
	}

	public ProfilesViewModel(IEventAggregator events, IFeatureManager featureMgr)
	{
		_events = events;
		_events.GetEvent<ContentRegionsEvent>().Subscribe(LoadRegions);
		_events.GetEvent<UnsupportedPlatformEvent>().Subscribe(() =>
		{
			Supported = false;
		});
		_events.GetEvent<CustomAppTitleEvent>().Subscribe((bool x) =>
		{
			ShowPoweredByText = x;
			GenerateNssIcon();
		});
		_supported = true;
		featureMgr.LoadFeatures();
	}

	private void GenerateNssIcon()
	{
		using Bitmap bitmap = Resources.NUC_icon.ToBitmap();
		MemoryStream memoryStream = new MemoryStream();
		bitmap.Save(memoryStream, ImageFormat.Png);
		NssIcon = BitmapFrame.Create(memoryStream);
	}

	private void LoadRegions(List<Tuple<int, List<string>>> regions)
	{
		foreach (Tuple<int, List<string>> region in regions)
		{
			switch (region.Item1)
			{
			case 0:
				_buttonContentRegions = region.Item2;
				break;
			case 1:
				_primaryContentRegions = region.Item2;
				break;
			case 2:
				_secondaryContentRegions = region.Item2;
				break;
			}
		}
	}

	public void OnNavigatedTo(NavigationContext navigationContext)
	{
		_events.GetEvent<NavigateToProfilesView>().Publish();
	}

	public bool IsNavigationTarget(NavigationContext navigationContext)
	{
		return true;
	}

	public void OnNavigatedFrom(NavigationContext navigationContext)
	{
	}
}
