using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace UniwillSdkClient.UniwillService;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "KeyState", Namespace = "http://schemas.datacontract.org/2004/07/IntelSDK")]
public class KeyState : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private bool StateField;

	[OptionalField]
	private KeyType TypeField;

	[Browsable(false)]
	public ExtensionDataObject ExtensionData
	{
		get
		{
			return extensionDataField;
		}
		set
		{
			extensionDataField = value;
		}
	}

	[DataMember]
	public bool State
	{
		get
		{
			return StateField;
		}
		set
		{
			if (!StateField.Equals(value))
			{
				StateField = value;
				RaisePropertyChanged("State");
			}
		}
	}

	[DataMember]
	public KeyType Type
	{
		get
		{
			return TypeField;
		}
		set
		{
			if (!TypeField.Equals(value))
			{
				TypeField = value;
				RaisePropertyChanged("Type");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
