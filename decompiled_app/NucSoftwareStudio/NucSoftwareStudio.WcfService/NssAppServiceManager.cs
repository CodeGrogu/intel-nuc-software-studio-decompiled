using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using NucSoftwareStudio.Interfaces;

namespace NucSoftwareStudio.WcfService;

public class NssAppServiceManager : INssAppServiceManager
{
	private readonly ServiceHost _serviceHost;

	private readonly NssAppService _nssAppService;

	public NssAppServiceManager()
	{
		_nssAppService = new NssAppService();
		_serviceHost = new ServiceHost(_nssAppService, new Uri("net.pipe://localhost/NssTrayApp/service"));
		_serviceHost.AddServiceEndpoint(typeof(INssAppServiceContract), new NetNamedPipeBinding(), "NssTrayAppEndpoint");
		_serviceHost.Open();
	}

	public static void SendTrayAppLaunchMessage()
	{
		EndpointAddress endpointAddress = new EndpointAddress("net.pipe://localhost/NssTrayApp/service/NssTrayAppEndpoint");
		((IChannelFactory<INssAppServiceContract>)new ChannelFactory<INssAppServiceContract>(new NetNamedPipeBinding(), endpointAddress)).CreateChannel(endpointAddress).ShowMainWindow();
	}
}
