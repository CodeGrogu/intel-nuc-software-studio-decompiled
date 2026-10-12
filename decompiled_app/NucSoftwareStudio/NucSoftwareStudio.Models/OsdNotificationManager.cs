using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using NucSoftwareStudio.Models.Osd;
using NucSoftwareStudio.Views;
using Prism.Events;
using UniwillSdkClient.Events;
using UniwillSdkClient.UniwillService;

namespace NucSoftwareStudio.Models;

public class OsdNotificationManager
{
	private readonly OsdPopUp _popup = new OsdPopUp();

	private DispatcherTimer _timer;

	private static readonly TimeSpan _timerInterval = TimeSpan.FromSeconds(3.0);

	public void Start(IEventAggregator events)
	{
		events.GetEvent<OsdNotificationEvent>().Subscribe(ShowOsd);
		_timer = new DispatcherTimer
		{
			Interval = _timerInterval
		};
	}

	private void ShowOsd(KeyValuePair<OsdType, int> KeyState)
	{
		Application.Current.Dispatcher.Invoke(() =>
		{
			_popup.DataContext = new OsdMessageData(OsdResourceConverter.OsdTypeToMessage(KeyState.Key, KeyState.Value), OsdResourceConverter.OsdTypeToIcon(KeyState.Key, KeyState.Value));
			_popup.Show();
		});
		StartCloseTimer();
	}

	private void StartCloseTimer()
	{
		_timer.Tick += TimerTick;
		_timer.Start();
	}

	private void TimerTick(object sender, EventArgs e)
	{
		_timer.Stop();
		_timer.Tick -= TimerTick;
		Application.Current.Dispatcher.Invoke(() =>
		{
			_popup.Hide();
		});
	}
}
