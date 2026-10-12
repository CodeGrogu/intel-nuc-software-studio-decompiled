using System.Threading.Tasks;
using HidDeviceAccessLibrary;
using NucSoftwareStudioService.LedControl.Interfaces;
using NucSoftwareStudioService.LedControl.Models;

namespace NucSoftwareStudioService.LedControl.USB;

public class HidLedControl : ILedControl
{
	private HidDeviceAccess _hidDevice;

	public HidLedControl()
	{
		_hidDevice = HidDeviceAccess.GetHidDeviceAccessInstance;
	}

	public void SetEffect(byte State, byte Speed, byte Brightness, byte Direction)
	{
		EffectData hidEffect = new EffectData
		{
			Speed = Speed,
			Light = Brightness,
			Direction = Direction,
			Effect = State
		};
		SetHidEffect(hidEffect).Wait();
	}

	public void TurnOff()
	{
		SetHidEffect().Wait();
	}

	public void Shutdown()
	{
		SetHidEffect().ContinueWith((Task _) =>
		{
			_hidDevice.DisconnectFromDevice();
		}).ConfigureAwait(continueOnCapturedContext: false);
	}

	private async Task SetHidEffect(EffectData NewEffect = null)
	{
		if (NewEffect == null)
		{
			NewEffect = new EffectData
			{
				LedStatus = AlexaConstants.BLANK
			};
		}
		if (_hidDevice == null)
		{
			_hidDevice = HidDeviceAccess.GetHidDeviceAccessInstance;
		}
		await _hidDevice.SendEffectToDevice(NewEffect);
	}
}
