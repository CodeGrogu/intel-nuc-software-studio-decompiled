using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Security.RightsManagement;

namespace MS.Internal.IO.Packaging.CompoundFile;

internal class RightsManagementEncryptionTransform : IDataTransform
{
	internal delegate void UseLicenseStreamCallback(RightsManagementEncryptionTransform rmet, StreamInfo si, object param, ref bool stop);

	public bool IsReady
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public bool FixedSettings
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal int TransformIdentifierType
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	public object TransformIdentifier
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal CryptoProvider CryptoProvider
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
		set
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal static string ClassTransformIdentifier
	{
		get
		{
			/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
		}
	}

	internal RightsManagementEncryptionTransform(TransformEnvironment transformEnvironment)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal PublishLicense LoadPublishLicense()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void SavePublishLicense(PublishLicense publishLicense)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal UseLicense LoadUseLicense(ContentUser user)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void SaveUseLicense(ContentUser user, UseLicense useLicense)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void DeleteUseLicense(ContentUser user)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal IDictionary<ContentUser, UseLicense> GetEmbeddedUseLicenses()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	Stream IDataTransform.GetTransformedStream(Stream encodedStream, IDictionary transformContext)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void EnumUseLicenseStreams(UseLicenseStreamCallback callback, object param)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal UseLicense LoadUseLicenseAndUserFromStream(BinaryReader utf8Reader, out ContentUser user)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	internal void SaveUseLicenseForUser(ContentUser user, UseLicense useLicense)
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}

	static RightsManagementEncryptionTransform()
	{
		/*Error: Empty body found. Decompiled assembly might be a reference assembly.*/;
	}
}
