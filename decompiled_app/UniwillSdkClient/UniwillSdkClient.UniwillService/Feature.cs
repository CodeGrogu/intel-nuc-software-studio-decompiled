using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace UniwillSdkClient.UniwillService;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "Feature", Namespace = "http://schemas.datacontract.org/2004/07/IntelSDK")]
public class Feature : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private int IndexField;

	[OptionalField]
	private int PriorityLevelField;

	[OptionalField]
	private FeatureInterfaceType[] SupportedInterfacesField;

	[OptionalField]
	private FeatureType TypeField;

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
	public int Index
	{
		get
		{
			return IndexField;
		}
		set
		{
			if (!IndexField.Equals(value))
			{
				IndexField = value;
				RaisePropertyChanged("Index");
			}
		}
	}

	[DataMember]
	public int PriorityLevel
	{
		get
		{
			return PriorityLevelField;
		}
		set
		{
			if (!PriorityLevelField.Equals(value))
			{
				PriorityLevelField = value;
				RaisePropertyChanged("PriorityLevel");
			}
		}
	}

	[DataMember]
	public FeatureInterfaceType[] SupportedInterfaces
	{
		get
		{
			return SupportedInterfacesField;
		}
		set
		{
			if (SupportedInterfacesField != value)
			{
				SupportedInterfacesField = value;
				RaisePropertyChanged("SupportedInterfaces");
			}
		}
	}

	[DataMember]
	public FeatureType Type
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
