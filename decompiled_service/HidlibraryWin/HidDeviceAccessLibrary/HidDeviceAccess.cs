using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.HumanInterfaceDevice;
using Windows.Storage;
using Windows.Storage.Streams;

namespace HidDeviceAccessLibrary;

public sealed class HidDeviceAccess
{
	public delegate void LogDelegate(string Message);

	private LogDelegate _logger;

	private static HidDeviceAccess _hidDeviceAccess;

	private static readonly object _singletonCreationLock = new object();

	private HidDevice _device;

	private DeviceInformation _deviceInformation;

	private DataReader _dataReaderObject;

	private DataWriter _dataWriterObject;

	private DeviceWatcher _deviceWatcher;

	private readonly ManagementEventWatcher watcherAdd = new ManagementEventWatcher();

	private readonly ManagementEventWatcher watcherRemove = new ManagementEventWatcher();

	private bool _watcherStarted;

	private bool _isEnabledAutoReconnect;

	private readonly byte[] _getVersionCmd = new byte[8] { 128, 0, 0, 0, 0, 0, 0, 0 };

	private bool IsDeviceConnected => _device != null;

	public static HidDeviceAccess GetHidDeviceAccessInstance
	{
		get
		{
			if (_hidDeviceAccess == null)
			{
				lock (_singletonCreationLock)
				{
					if (_hidDeviceAccess == null)
					{
						_hidDeviceAccess = new HidDeviceAccess();
					}
				}
			}
			return _hidDeviceAccess;
		}
	}

	private HidDeviceAccess()
	{
		_device = null;
		_deviceInformation = null;
		_dataReaderObject = null;
		_dataWriterObject = null;
		_watcherStarted = false;
		_isEnabledAutoReconnect = true;
		ConnectToDevice().ConfigureAwait(continueOnCapturedContext: false);
	}

	public void SetLogger(LogDelegate logger)
	{
		_logger = logger;
	}

	public void DisconnectFromDevice()
	{
		if (IsDeviceConnected)
		{
			CloseDevice();
		}
	}

	public async Task SendEffectToDevice(EffectData EffectType)
	{
		await DeviceWriteAsync(new byte[8]
		{
			8,
			(byte)EffectType.LedStatus,
			(byte)EffectType.Effect,
			(byte)EffectType.Speed,
			(byte)EffectType.Light,
			0,
			(byte)EffectType.Direction,
			0
		}).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<FwVersion> GetFwVerison()
	{
		await DeviceWriteAsync(_getVersionCmd).ConfigureAwait(continueOnCapturedContext: false);
		return new FwVersion(await DeviceReadAsync().ConfigureAwait(continueOnCapturedContext: false));
	}

	private async Task<bool> ConnectToDevice()
	{
		bool succcessfullyConnected = false;
		if (IsDeviceConnected)
		{
			DisconnectFromDevice();
		}
		if (await InitializeDevice() && await OpenDeviceAsync())
		{
			succcessfullyConnected = true;
			if (!_watcherStarted)
			{
				StartDeviceWatcher();
			}
		}
		return succcessfullyConnected;
	}

	private async Task<uint> DeviceWriteAsync(byte[] message)
	{
		if (_device == null)
		{
			await ConnectToDevice().ConfigureAwait(continueOnCapturedContext: false);
		}
		uint bytesWritten = 0u;
		if (message.Length == 8)
		{
			try
			{
				HidFeatureReport val = _device.CreateFeatureReport();
				_dataWriterObject = new DataWriter();
				_dataWriterObject.WriteByte((byte)0);
				_dataWriterObject.WriteBytes(message);
				val.put_Data(_dataWriterObject.DetachBuffer());
				bytesWritten = await _device.SendFeatureReportAsync(val);
			}
			catch (Exception)
			{
			}
			finally
			{
				_dataWriterObject.DetachStream();
				_dataWriterObject = null;
			}
		}
		return bytesWritten;
	}

	private async Task<byte[]> DeviceReadAsync()
	{
		if (_device == null)
		{
			await ConnectToDevice().ConfigureAwait(continueOnCapturedContext: false);
		}
		byte[] byteArrayRead = new byte[9];
		try
		{
			HidFeatureReport val = await _device.GetFeatureReportAsync();
			if (val != null)
			{
				_dataReaderObject = DataReader.FromBuffer(val.Data);
				_dataReaderObject.ReadBytes(byteArrayRead);
			}
		}
		catch (Exception)
		{
		}
		finally
		{
			_dataReaderObject.DetachStream();
			_dataReaderObject = null;
		}
		return byteArrayRead;
	}

	private async Task<bool> InitializeDevice()
	{
		string deviceSelector = HidDevice.GetDeviceSelector((ushort)65283, (ushort)1, (ushort)1165, (ushort)24583);
		RegisterForDeviceWatcherEvents();
		DeviceInformationCollection val = await DeviceInformation.FindAllAsync(deviceSelector);
		if (((IEnumerable<DeviceInformation>)val).Any())
		{
			_deviceInformation = ((IReadOnlyList<DeviceInformation>)val)[0];
			return true;
		}
		return false;
	}

	private async Task<bool> OpenDeviceAsync()
	{
		_device = await HidDevice.FromIdAsync(_deviceInformation.Id, (FileAccessMode)1);
		if (_device != null)
		{
			return true;
		}
		return false;
	}

	private void CloseDevice()
	{
		if (_device != null)
		{
			CloseCurrentlyConnectedDevice();
		}
		if (_deviceWatcher != null)
		{
			if (_watcherStarted)
			{
				StopDeviceWatcher();
				UnregisterFromDeviceWatcherEvents();
			}
			_deviceWatcher = null;
		}
		_deviceInformation = null;
		_hidDeviceAccess = null;
		_isEnabledAutoReconnect = true;
	}

	private void CloseCurrentlyConnectedDevice()
	{
		if (_device != null)
		{
			_device.Dispose();
			_device = null;
		}
	}

	private void UnregisterFromDeviceWatcherEvents()
	{
		WqlEventQuery query = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2");
		watcherAdd.EventArrived -= OnDeviceAdded;
		watcherAdd.Query = query;
		WqlEventQuery query2 = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 3");
		watcherRemove.EventArrived -= OnDeviceRemoved;
		watcherRemove.Query = query2;
	}

	private void RegisterForDeviceWatcherEvents()
	{
		WqlEventQuery query = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2");
		watcherAdd.EventArrived += OnDeviceAdded;
		watcherAdd.Query = query;
		WqlEventQuery query2 = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 3");
		watcherRemove.EventArrived += OnDeviceRemoved;
		watcherRemove.Query = query2;
	}

	private void StartDeviceWatcher()
	{
		_watcherStarted = true;
		watcherAdd.Start();
		watcherRemove.Start();
	}

	private void StopDeviceWatcher()
	{
		watcherAdd.Stop();
		watcherRemove.Stop();
		_watcherStarted = false;
	}

	private async void OnDeviceAdded(object sender, EventArrivedEventArgs e)
	{
		if (_deviceInformation != null && !IsDeviceConnected && _isEnabledAutoReconnect && ((IEnumerable<DeviceInformation>)(await DeviceInformation.FindAllAsync(HidDevice.GetDeviceSelector((ushort)65283, (ushort)1, (ushort)1165, (ushort)24583)))).Any())
		{
			_isEnabledAutoReconnect = await OpenDeviceAsync();
		}
	}

	private async void OnDeviceRemoved(object sender, EventArrivedEventArgs e)
	{
		if (IsDeviceConnected && !((IEnumerable<DeviceInformation>)(await DeviceInformation.FindAllAsync(HidDevice.GetDeviceSelector((ushort)65283, (ushort)1, (ushort)1165, (ushort)24583)))).Any())
		{
			CloseCurrentlyConnectedDevice();
		}
	}
}
