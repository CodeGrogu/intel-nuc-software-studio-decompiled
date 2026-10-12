using System;
using System.Threading.Tasks;
using ContextSensingClient;
using Infrastructure.Models;
using Prism.Events;
using Prism.Logging;
using Prism.Mvvm;
using UserPresenceMgr.Events;
using UserPresenceMgr.Interfaces;

namespace UserPresenceMgr.Models;

public class UserPresenceManager : BindableBase, IUserPresenceManager
{
	private readonly ILoggerFacade _logger;

	private readonly IEventAggregator _events;

	private readonly HumanPresenceCallback _hupListener;

	private readonly ClientBackendLink _client;

	private LockFeatureData _lockFeatures;

	public static readonly int TIME_INTERVAL_MIN = (int)TimeSpan.FromSeconds(30.0).TotalMilliseconds;

	public static readonly int TIME_INTERVAL_MAX = (int)TimeSpan.FromMinutes(2.5).TotalMilliseconds;

	public static readonly int TIME_INTERVAL = (int)TimeSpan.FromSeconds(30.0).TotalMilliseconds;

	public static readonly int PERCENT_MIN = 0;

	public static readonly int PERCENT_MAX = 100;

	private WakeFeatureData _wakeFeatures;

	private NoLockOnPresenceFeatureData _noLockOnPresenceFeatures;

	private AppFeatures _appFeatures;

	private bool _serviceAvailable = true;

	private bool _featureSupported = true;

	public LockFeatureData LockFeatures
	{
		get
		{
			return _lockFeatures;
		}
		set
		{
			SetProperty(ref _lockFeatures, value, "LockFeatures");
		}
	}

	public WakeFeatureData WakeFeatures
	{
		get
		{
			return _wakeFeatures;
		}
		set
		{
			SetProperty(ref _wakeFeatures, value, "WakeFeatures");
		}
	}

	public NoLockOnPresenceFeatureData NoLockOnPresenceFeatures
	{
		get
		{
			return _noLockOnPresenceFeatures;
		}
		set
		{
			SetProperty(ref _noLockOnPresenceFeatures, value, "NoLockOnPresenceFeatures");
		}
	}

	public bool ServiceAvailable
	{
		get
		{
			return _serviceAvailable;
		}
		set
		{
			SetProperty(ref _serviceAvailable, value, "ServiceAvailable");
		}
	}

	public bool FeatureSupported
	{
		get
		{
			return _featureSupported;
		}
		set
		{
			SetProperty(ref _featureSupported, value, "FeatureSupported");
		}
	}

	public UserPresenceManager(IEventAggregator events, ILoggerFacade logger)
	{
		_logger = logger;
		_events = events;
		_events.GetEvent<UserPresenceUpdateSettingEvent>().Subscribe(UpdateSetting);
		_events.GetEvent<UserPresenceEnableFeatureEvent>().Subscribe(EnableFeature);
		_logger.Log("UserPresenceManager ctor", Category.Debug, Priority.High);
		_hupListener = new HumanPresenceCallback(_events, _logger);
		_client = new ClientBackendLink(_hupListener);
		_lockFeatures = new LockFeatureData(_events, new LockFeatures());
		_wakeFeatures = new WakeFeatureData(_events, new WakeFeatures());
		_noLockOnPresenceFeatures = new NoLockOnPresenceFeatureData(_events, new NoLockOnPresenceFeatures());
		GetOptions(FeatureType.APPLICATION);
		GetOptions(FeatureType.LOCK);
		GetOptions(FeatureType.NOLOCKONPRESENCE);
		GetOptions(FeatureType.WAKE);
	}

	private void EnableFeature(Tuple<FeatureType, bool> feature)
	{
		_logger.Log("HumanaPresenceManager EnableFeature: " + feature.Item1.ToString() + " " + feature.Item2, Category.Debug, Priority.High);
		if (feature.Item2)
		{
			_client.Enable(feature.Item1);
		}
		else
		{
			_client.Disable(feature.Item1);
		}
	}

	private void UpdateSetting(SetOptionData data)
	{
		_logger.Log("HumanaPresenceManager UpdateSetting: " + data.FeatureType.ToString() + " " + data.FeatureProperty, Category.Debug, Priority.High);
		if (ServiceAvailable)
		{
			_client.SetOption(data.FeatureType, data.FeatureProperty, data.Data).ContinueWith((Task t) =>
			{
				GetOptions(data.FeatureType);
			});
		}
	}

	private void GetOptions(FeatureType type)
	{
		_logger.Log("HumanaPresenceManager GetOptions: " + type, Category.Debug, Priority.High);
		if (!ServiceAvailable)
		{
			return;
		}
		_hupListener.Reset();
		try
		{
			_client.GetOptions(type).ContinueWith((Task<FeatureSetting> t) =>
			{
				if (t.Exception != null)
				{
					foreach (Exception innerException in t.Exception.InnerExceptions)
					{
						_ = innerException;
						if (t.Exception.InnerExceptions[0].GetType().Equals(typeof(ErrorException)) && ((ErrorException)t.Exception.InnerExceptions[0]).errorType == ErrorType.ServiceUnavailable)
						{
							ServiceAvailable = false;
							break;
						}
					}
					return;
				}
				UpdateOptions(type, t.Result);
			});
		}
		catch (Exception exception)
		{
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
		}
	}

	private void UpdateOptions(FeatureType type, FeatureSetting settings)
	{
		_logger.Log("HumanaPresenceManager UpdateOptions: " + type.ToString() + " " + settings, Category.Debug, Priority.High);
		if (settings is LockFeatures features)
		{
			LockFeatures.UpdateData(features);
		}
		else if (settings is WakeFeatures features2)
		{
			WakeFeatures.UpdateData(features2);
		}
		else if (settings is NoLockOnPresenceFeatures features3)
		{
			NoLockOnPresenceFeatures.UpdateData(features3);
		}
		else if (settings is AppFeatures appFeatures)
		{
			_appFeatures = appFeatures;
			ApplicationFeatureData.ApplyAppSettings(_events, appFeatures);
		}
		_events.GetEvent<UserPresenceSettingUpdatedEvent>().Publish(type);
	}
}
