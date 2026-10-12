using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Markup;
using CustomizationMgr.Interfaces;
using FeatureMgr.Enums;
using FeatureMgr.Models;
using Infrastructure.Constants.Localization;
using Infrastructure.Events;
using NucSoftwareStudio.Properties;
using NucSoftwareStudio.Spalsh;
using Prism.Events;
using UniwillSdkClient.Events;
using Windows.Storage;

namespace NucSoftwareStudio.Views;

public partial class MainWindow : Window, IComponentConnector
{
	private readonly IEventAggregator _events;

	private readonly ICustomResourceManager _resources;

	private string _uiLanguage;

	private bool isInitialized;

	private SplashWindow splashWindow = new SplashWindow();

	private NucType nucType = PlatformDetail.GetNucType();

	public MainWindow(IEventAggregator events, ICustomResourceManager resources)
	{
		_events = events;
		_resources = resources;
		_uiLanguage = SupportedLocales.GetOsLocale();
		InitializeComponent();
		if (nucType == NucType.Serpent)
		{
			if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("isFreshLaunch"))
			{
				splashWindow.Show();
				ApplicationData.Current.LocalSettings.Values["isFreshLaunch"] = false;
				SplashHelper.AnimateSplash();
			}
			else if ((bool)ApplicationData.Current.LocalSettings.Values["isFreshLaunch"])
			{
				splashWindow.Show();
				ApplicationData.Current.LocalSettings.Values["isFreshLaunch"] = false;
				SplashHelper.AnimateSplash();
			}
			else
			{
				splashWindow.Close();
				ApplicationData.Current.LocalSettings.Values["isFreshLaunch"] = false;
			}
		}
		StateChanged += MainWindow_StateChanged;
		MaximizeButton.Visibility = ((WindowState == WindowState.Maximized) ? Visibility.Collapsed : Visibility.Visible);
		RestoreButton.Visibility = ((WindowState != WindowState.Maximized) ? Visibility.Collapsed : Visibility.Visible);
		_events.GetEvent<LogoLoadedEvent>().Subscribe(UpdateUiLanguage);
	}

	private void UpdateUiLanguage()
	{
		string customDefaultCultureId = _resources.GetCustomDefaultCultureId();
		if (!string.IsNullOrEmpty(customDefaultCultureId))
		{
			_uiLanguage = customDefaultCultureId;
		}
	}

	private void MainWindow_StateChanged(object sender, EventArgs e)
	{
		if (WindowState == WindowState.Maximized)
		{
			MaximizeButton.Visibility = Visibility.Collapsed;
			RestoreButton.Visibility = Visibility.Visible;
			_events.GetEvent<SubscribeEvent>().Publish(payload: true);
		}
		else if (WindowState == WindowState.Normal)
		{
			RestoreButton.Visibility = Visibility.Collapsed;
			MaximizeButton.Visibility = Visibility.Visible;
			_events.GetEvent<SubscribeEvent>().Publish(payload: true);
		}
		else
		{
			RestoreButton.Visibility = Visibility.Collapsed;
			MaximizeButton.Visibility = Visibility.Visible;
			_events.GetEvent<SubscribeEvent>().Publish(payload: false);
		}
	}

	private void EulaTextBox_Loaded(object sender, RoutedEventArgs e)
	{
		if (nucType == NucType.Serpent)
		{
			splashWindow.Close();
		}
		System.Windows.Controls.RichTextBox richTextBox = (System.Windows.Controls.RichTextBox)sender;
		MemoryStream localizedEulaStream = GetLocalizedEulaStream();
		TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
		textRange.Load(localizedEulaStream, System.Windows.DataFormats.Rtf);
		textRange.ApplyPropertyValue(System.Windows.Controls.Control.ForegroundProperty, richTextBox.Foreground);
	}

	private void EulaTextBox_Scroll(object sender, ScrollChangedEventArgs e)
	{
		if (((ScrollViewer)e.OriginalSource).ScrollableHeight != 0.0 && e.VerticalOffset == ((ScrollViewer)e.OriginalSource).ScrollableHeight)
		{
			AcceptButton.IsEnabled = true;
		}
	}

	private void MinimizeButton_Click(object sender, RoutedEventArgs e)
	{
		WindowState = WindowState.Minimized;
	}

	private void MaximizeRestoreButton_Click(object sender, RoutedEventArgs e)
	{
		if (WindowState == WindowState.Maximized)
		{
			WindowState = WindowState.Normal;
		}
		else
		{
			Screen currentScreen = GetCurrentScreen(this);
			if (currentScreen.Primary)
			{
				MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
				MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
			}
			else
			{
				MaxHeight = currentScreen.WorkingArea.Height;
				MaxWidth = currentScreen.WorkingArea.Width;
			}
			WindowState = WindowState.Maximized;
		}
		Show();
	}

	public Screen GetCurrentScreen(Window window)
	{
		return Screen.FromHandle(new WindowInteropHelper(window).Handle);
	}

	private void CloseButton_Click(object sender, RoutedEventArgs e)
	{
		Hide();
		_events.GetEvent<SubscribeEvent>().Publish(payload: false);
		ApplicationData.Current.LocalSettings.Values["FirstLaunch"] = false;
	}

	private MemoryStream GetLocalizedEulaStream()
	{
		return _uiLanguage.ToLower() switch
		{
			"en-us" => new MemoryStream(Encoding.Default.GetBytes(NucSoftwareStudio.Properties.Resources.Intel_OBL_Commercial_Use_License)), 
			"de-de" => new MemoryStream(Encoding.Default.GetBytes(NucSoftwareStudio.Properties.Resources.Intel_OBL_Commercial_Use_License_de_DE)), 
			"es-es" => new MemoryStream(Encoding.Default.GetBytes(NucSoftwareStudio.Properties.Resources.Intel_OBL_Commercial_Use_License_es_ES)), 
			"fr-fr" => new MemoryStream(Encoding.Default.GetBytes(NucSoftwareStudio.Properties.Resources.Intel_OBL_Commercial_Use_License_fr_FR)), 
			"it-it" => new MemoryStream(Encoding.Default.GetBytes(NucSoftwareStudio.Properties.Resources.Intel_OBL_Commercial_Use_License_it_IT)), 
			"ja-jp" => new MemoryStream(Encoding.Default.GetBytes(NucSoftwareStudio.Properties.Resources.Intel_OBL_Commercial_Use_License_ja_JP)), 
			"ko-kr" => new MemoryStream(Encoding.Default.GetBytes(NucSoftwareStudio.Properties.Resources.Intel_OBL_Commercial_Use_License_ko_KR)), 
			"pt-pt" => new MemoryStream(Encoding.Default.GetBytes(NucSoftwareStudio.Properties.Resources.Intel_OBL_Commercial_Use_License_pt_PT)), 
			"ru-ru" => new MemoryStream(Encoding.Default.GetBytes(NucSoftwareStudio.Properties.Resources.Intel_OBL_Commercial_Use_License_ru_RU)), 
			"zh-cn" => new MemoryStream(Encoding.Default.GetBytes(NucSoftwareStudio.Properties.Resources.Intel_OBL_Commercial_Use_License_zh_CN)), 
			"zh-tw" => new MemoryStream(Encoding.Default.GetBytes(NucSoftwareStudio.Properties.Resources.Intel_OBL_Commercial_Use_License_zh_TW)), 
			_ => new MemoryStream(Encoding.Default.GetBytes(NucSoftwareStudio.Properties.Resources.Intel_OBL_Commercial_Use_License)), 
		};
	}

	private void WindowSetToForeground(object sender, EventArgs e)
	{
		_events.GetEvent<SubscribeEvent>().Publish(payload: true);
	}

	protected override void OnClosing(CancelEventArgs e)
	{
		Visibility = Visibility.Hidden;
		_events.GetEvent<SubscribeEvent>().Publish(payload: false);
		e.Cancel = true;
	}

	private void Window_Loaded(object sender, RoutedEventArgs e)
	{
		if (nucType == NucType.Serpent && splashWindow != null)
		{
			splashWindow.Close();
		}
	}
}
