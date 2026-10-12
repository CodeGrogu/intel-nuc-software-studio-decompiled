using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MS.Internal.IO.Packaging.CompoundFile;

namespace System.IO.Packaging;

internal class DataSpaceManager
{
	internal delegate void TransformInitializeEventHandler(object sender, TransformInitializationEventArgs e);

	internal const int TransformIdentifierTypes_PredefinedTransformName = 1;

	internal int Count
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal event TransformInitializeEventHandler OnTransformInitialization
	{
		[CompilerGenerated]
		add
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		[CompilerGenerated]
		remove
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	static DataSpaceManager()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal DataSpaceManager(StorageRoot containerInstance)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	public void Dispose()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void RemoveContainerFromDataSpaceMap(CompoundFileReference target)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void CheckDisposedStatus()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void DefineDataSpace(string[] transformStack, string newDataSpaceLabel)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal bool DataSpaceIsDefined(string dataSpaceLabel)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal string DataSpaceOf(CompoundFileReference target)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal List<IDataTransform> GetTransformsForStreamInfo(StreamInfo streamInfo)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal string DefineDataSpace(string[] transformStack)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal bool TransformLabelIsDefined(string transformLabel)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal Stream GetPrimaryInstanceStreamOf(string transformLabel)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal StorageInfo GetInstanceDataStorageOf(string transformLabel)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal IDataTransform GetTransformFromName(string transformLabel)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void DefineTransform(string transformClassName, string newTransformLabel)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal string DefineTransform(string transformClassName)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void CallTransformInitializers(TransformInitializationEventArgs initArguments)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void CreateDataSpaceMapping(CompoundFileReference containerReference, string label)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal Stream CreateDataSpaceStream(CompoundFileStreamReference containerReference, Stream rawStream)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
