using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Infrastructure.Models;
using NucLedMgr.WMI.Models.Indicators;
using Prism.Logging;

namespace NucLedMgr.WMI.CpuUsage;

public class CpuServiceClient
{
	private readonly ILoggerFacade _logger;

	private readonly EndpointAddress _endpoint;

	private IChannelFactory<ICpuService> _factory;

	private ICpuService _proxy;

	public CpuServiceClient(ILoggerFacade logger)
	{
		_endpoint = new EndpointAddress("net.pipe://localhost/CpuService/NssCpuService");
		_logger = logger;
		CreateProxy();
	}

	public void AddLed(CpuIndicator setting)
	{
		if (_proxy == null)
		{
			CreateProxy();
		}
		try
		{
			_proxy.AddSetting(setting);
		}
		catch (EndpointNotFoundException exception)
		{
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
		}
		catch (Exception exception2)
		{
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception2), Category.Exception, Priority.High);
		}
	}

	public void RemoveLed(CpuIndicator setting)
	{
		if (_proxy == null)
		{
			CreateProxy();
		}
		try
		{
			_proxy.RemoveSetting(setting);
		}
		catch (EndpointNotFoundException exception)
		{
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
		}
		catch (Exception exception2)
		{
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception2), Category.Exception, Priority.High);
		}
	}

	private void CreateProxy()
	{
		if (_factory == null)
		{
			_factory = new ChannelFactory<ICpuService>(new NetNamedPipeBinding(), _endpoint);
			_factory.Faulted += FactoryFaulted;
		}
		try
		{
			_proxy = _factory.CreateChannel(_endpoint);
			((ICommunicationObject)_proxy).Faulted += ProxyFaulted;
		}
		catch (Exception exception)
		{
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
		}
	}

	public void CloseConnection()
	{
		if (_proxy != null)
		{
			try
			{
				((IClientChannel)_proxy).Close();
			}
			catch (Exception exception)
			{
				_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
				if (_proxy != null)
				{
					((IClientChannel)_proxy).Abort();
				}
			}
		}
		if (_factory != null)
		{
			try
			{
				_factory.Close();
			}
			catch (Exception exception2)
			{
				_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception2), Category.Exception, Priority.High);
				_factory?.Abort();
			}
		}
		_proxy = null;
		_factory = null;
	}

	private void ProxyFaulted(object sender, EventArgs e)
	{
		try
		{
			((IClientChannel)_proxy).Abort();
		}
		catch (Exception exception)
		{
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
		}
		CreateProxy();
	}

	private void FactoryFaulted(object sender, EventArgs e)
	{
		try
		{
			((IClientChannel)_proxy).Abort();
		}
		catch (Exception exception)
		{
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception), Category.Exception, Priority.High);
		}
		try
		{
			_factory.Abort();
		}
		catch (Exception exception2)
		{
			_logger.Log(NssDebugLogger.ExceptionMessageFormatter(exception2), Category.Exception, Priority.High);
		}
		CreateProxy();
	}
}
