using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using Infrastructure.Converters;
using Infrastructure.Events;
using NucSoftwareStudio.Interfaces;
using NucSoftwareStudio.Models;
using NucSoftwareStudio.Models.TrayApp;
using NucSoftwareStudio.Properties;
using Prism.Events;
using UniwillSdkClient.Events;
using Windows.Storage;

namespace NucSoftwareStudio;

public class TrayApp : ITrayApp
{
	private const string EXIT_APP_KEY = "TrayAppExitString";

	private static readonly string EXIT_STRING = ResourceDictionaryConverter.GetString("TrayAppExitString");

	private static readonly string OPEN_STRING = ResourceDictionaryConverter.GetString("TrayAppMenuAppName");

	private static readonly string VERSION_STRING = ResourceDictionaryConverter.GetString("Version") + " " + AppVersionData.GetVersionString();

	private const int LED_PROFILES_MENU_INDEX = 4;

	private Window _mainWindow;

	private readonly NotifyIcon _trayIcon;

	private readonly IEventAggregator _events;

	private readonly OsdNotificationManager _osdManager = new OsdNotificationManager();

	private Dictionary<string, EventHandler> _ledProfiles;

	private Dictionary<string, EventHandler> _performancePresets;

	private string _currentProfile;

	public TrayApp(IEventAggregator events)
	{
		_events = events;
		_events.GetEvent<AddTrayAppMenuItemsEvent>().Subscribe(AddPerformancePresets);
		_events.GetEvent<WmiProfileChangedEvent>().Subscribe(LedProfileChanged);
		_events.GetEvent<ArduinoProfileChangedEvent>().Subscribe(LedProfileChanged);
		_events.GetEvent<EneProfileChangedEvent>().Subscribe(LedProfileChanged);
		_events.GetEvent<ArduinoLedProfilesChangedEvent>().Subscribe(UpdateLedProfiles);
		_events.GetEvent<WmiLedProfilesChangedEvent>().Subscribe(UpdateLedProfiles);
		_events.GetEvent<EneLedProfilesChangedEvent>().Subscribe(UpdateLedProfiles);
		_osdManager.Start(_events);
		_ledProfiles = new Dictionary<string, EventHandler>();
		_performancePresets = new Dictionary<string, EventHandler>();
		_currentProfile = string.Empty;
		_trayIcon = new NotifyIcon
		{
			Text = ResourceDictionaryConverter.GetString("TrayAppMenuAppName"),
			Icon = Resources.NUC_icon,
			Visible = true
		};
		_trayIcon.MouseClick += TrayIcon_MouseClick;
		ReformatMenu();
	}

	private void UpdateLedProfiles(Dictionary<string, EventHandler> profiles)
	{
		_ledProfiles = profiles;
		ReformatMenu();
	}

	private void AddPerformancePresets(Dictionary<string, EventHandler> Items)
	{
		_performancePresets = Items;
		ReformatMenu();
	}

	private void LedProfileChanged(string currentProfile)
	{
		if (!(currentProfile != _currentProfile) || _ledProfiles.Count == 0)
		{
			return;
		}
		for (int i = 4; i < 4 + _ledProfiles.Count && i < _trayIcon.ContextMenuStrip.Items.Count; i++)
		{
			if ((_trayIcon?.ContextMenuStrip?.Items[i] as ToolStripMenuItem)?.Text == currentProfile)
			{
				(_trayIcon?.ContextMenuStrip?.Items[i] as ToolStripMenuItem).ForeColor = TrayAppColorTable.TRAY_MENU_TEXT_CURRENTLY_ACTIVE_COLOR;
			}
			if ((_trayIcon?.ContextMenuStrip?.Items[i] as ToolStripMenuItem).Text == _currentProfile)
			{
				(_trayIcon?.ContextMenuStrip?.Items[i] as ToolStripMenuItem).ForeColor = TrayAppColorTable.TRAY_MENU_TEXT_COLOR;
			}
		}
		_currentProfile = currentProfile;
	}

	private void ReformatMenu()
	{
		ContextMenuStrip contextMenuStrip = new ContextMenuStrip
		{
			Font = TrayAppMenuRenderer.MenuFont,
			ForeColor = TrayAppColorTable.TRAY_MENU_TEXT_COLOR,
			Renderer = new TrayAppMenuRenderer()
		};
		ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(OPEN_STRING)
		{
			Padding = new Padding(10)
		};
		toolStripMenuItem.Click += Open_Click;
		contextMenuStrip.Items.Add(toolStripMenuItem);
		contextMenuStrip.Items.Add(new ToolStripSeparator
		{
			Padding = new Padding(10)
		});
		ToolStripMenuItem value = new ToolStripMenuItem(VERSION_STRING)
		{
			Padding = new Padding(10)
		};
		contextMenuStrip.Items.Add(value);
		if (_ledProfiles.Count > 0)
		{
			contextMenuStrip.Items.Add(new ToolStripSeparator
			{
				Padding = new Padding(10)
			});
			foreach (KeyValuePair<string, EventHandler> ledProfile in _ledProfiles)
			{
				ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem(ledProfile.Key)
				{
					Padding = new Padding(10)
				};
				if (ledProfile.Key == _currentProfile)
				{
					toolStripMenuItem2.ForeColor = TrayAppColorTable.TRAY_MENU_TEXT_CURRENTLY_ACTIVE_COLOR;
				}
				toolStripMenuItem2.Click += ledProfile.Value;
				contextMenuStrip.Items.Add(toolStripMenuItem2);
			}
		}
		if (_performancePresets.Count > 0)
		{
			contextMenuStrip.Items.Add(new ToolStripSeparator
			{
				Padding = new Padding(10)
			});
			foreach (KeyValuePair<string, EventHandler> preset in _performancePresets)
			{
				ToolStripMenuItem toolStripMenuItem3 = new ToolStripMenuItem(preset.Key)
				{
					Padding = new Padding(10)
				};
				toolStripMenuItem3.Click += (object sender, EventArgs e) =>
				{
					preset.Value(sender, e);
				};
				contextMenuStrip.Items.Add(toolStripMenuItem3);
			}
		}
		contextMenuStrip.Items.Add(new ToolStripSeparator
		{
			Padding = new Padding(10)
		});
		ToolStripMenuItem toolStripMenuItem4 = new ToolStripMenuItem(EXIT_STRING)
		{
			Padding = new Padding(10)
		};
		toolStripMenuItem4.Click += Exit_Click;
		contextMenuStrip.Items.Add(toolStripMenuItem4);
		_trayIcon.ContextMenuStrip = contextMenuStrip;
	}

	public void SetMainWindow(Window MainWindow)
	{
		_mainWindow = MainWindow;
	}

	private void TrayIcon_MouseClick(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			_events.GetEvent<SubscribeEvent>().Publish(payload: true);
			if (System.Windows.Application.Current.MainWindow == null)
			{
				System.Windows.Application.Current.MainWindow = _mainWindow;
			}
			System.Windows.Application.Current.MainWindow.WindowState = WindowState.Normal;
			System.Windows.Application.Current.MainWindow.Show();
			System.Windows.Application.Current.MainWindow.Activate();
		}
	}

	private void Open_Click(object sender, EventArgs e)
	{
		_mainWindow.Show();
	}

	private void Exit_Click(object sender, EventArgs e)
	{
		if (ApplicationData.Current.LocalSettings.Values.ContainsKey("isFreshLaunch"))
		{
			ApplicationData.Current.LocalSettings.Values["isFreshLaunch"] = true;
		}
		System.Windows.Application.Current.Shutdown();
		_trayIcon.Visible = false;
		_trayIcon.Dispose();
	}
}
