using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace IntelSdkClient.AlexaStateService;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "AlexaState", Namespace = "http://schemas.datacontract.org/2004/07/Alexa.Extensions")]
public enum AlexaState
{
	[EnumMember]
	Idle,
	[EnumMember]
	Listening,
	[EnumMember]
	ActiveListening,
	[EnumMember]
	Thinking,
	[EnumMember]
	Speaking,
	[EnumMember]
	MicrophoneOff,
	[EnumMember]
	SystemError,
	[EnumMember]
	Off,
	[EnumMember]
	IncomingNotification,
	[EnumMember]
	NotificationQueued,
	[EnumMember]
	DoNotDisturb
}
