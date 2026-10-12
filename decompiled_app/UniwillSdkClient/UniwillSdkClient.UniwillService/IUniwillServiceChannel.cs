using System;
using System.CodeDom.Compiler;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace UniwillSdkClient.UniwillService;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public interface IUniwillServiceChannel : IUniwillService, IClientChannel, IContextChannel, IChannel, ICommunicationObject, IExtensibleObject<IContextChannel>, IDisposable
{
}
